using amLogger;
using CaSecrets;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBot1;

public class Telega
{
    static Telega? _this;

    TelegramBotClient botClient = new(Secrets.Sinex_CaTelegramBotToken);
    CancellationTokenSource cts = new();

    public Telega()
    {
        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Log.Error("Telega", ErrorMessage);
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        await Db.AddCaTeleBotUser(chatId);
    }
    async Task SendMessage(string msg)
    {
        List<long> chatIds = Db.GetCaTeleBotUsers();
        foreach (var chId in chatIds)
        {
            try
            {
                await botClient.SendTextMessageAsync(
                    chatId: chId,
                    text: msg
                );
            }
            catch (Exception ex)
            {
                Log.Error("Send", ex.Message);
            }
        }
    }
    public static async Task ProcessBundle(Bandle b)
    {
        if(_this == null) _this = new Telega();

        var (profit, msg) = CreateMessage(b);

        if (profit > 100)
        {
            await _this.SendMessage(msg);
            await b.Save();
        }
    }
    static (double, string) CreateMessage(Bandle b)
    {
        var pbb = b.priceBuyBid;
        var pba = b.priceBuyAsk;
        var pbm = (pbb + pba) / 2;
        var psb = b.priceSellBid;
        var psa = b.priceSellAsk;
        var psm = (psb + psa) / 2;
        var proc = Math.Round(100 * (psm - pbm) / pbm, 2);
        var recVol = Math.Round(Math.Min(b.volBuy, b.volSell)/3, 2);
        var commis = Math.Round(b.withdrawFee * pbm, 2);
        var profit = Math.Round(recVol * proc / 100 - commis, 2);

        string msg = $"{b.coin}/USDT\n\r";
        msg += $"{b.exchBuy} вывод (тут покупаем) \n\r";
        msg += $"Цена в стакане: {pbm} [{pbb}-{pba}]\n\r";
        msg += $"Цена последней сделки: {b.lastBuy}\n\r";
        msg += $"Объем в стакане на продажу: {b.volBuy}$\n\r";
        msg += $"{b.exchSell} ввод (тут продаем)\n\r";
        msg += $"Цена в стакане: {psm} [{psb}-{psa}]\n\r";
        msg += $"Цена последней сделки: {b.lastSell}\n\r";
        msg += $"Объем в стакане на покупку: {b.volSell}$\n\r";
        msg += $"Комиссия за вывод: {commis}$\n\r";
        msg += $"Сеть: {b.chain}\n\r";
        msg += $"Процент прибыли: {proc}%\n\r";
        msg += $"Рекомендуемая сумма вложения: {recVol}$\n\r";
        msg += $"Прибыль с учетом комиссии: {profit}$";

        return (profit, msg);
    }
}
