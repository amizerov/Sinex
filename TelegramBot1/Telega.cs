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
        await _this.SendMessage(CreateMessage(b));
    }
    static string CreateMessage(Bandle b)
    {
        string msg = b.coin + "/USDT";
        msg += $"{b.exchBuy} вывод \n\r";
        msg += $"Цена: {(b.priceBuyBid + b.priceBuyAsk)/2} [{b.priceBuyBid}-{b.priceBuyAsk}]\n\r";
        msg += $"Объем: {b.volBuy}$\n\r";
        msg += $"{b.exchSell} ввод \n\r";
        msg += $"Цена: {(b.priceSellBid + b.priceSellAsk) / 2} [{b.priceSellBid}-{b.priceSellAsk}]\n\r";
        msg += $"Объем: {b.volSell}$\n\r";
        msg += $"Комиссия: {b.withdrawFee}\n\r";
        msg += "Сеть: " + b.chain + "\n\r";

        return msg;
    }
}
