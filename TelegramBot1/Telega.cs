using amLogger;
using CaSecrets;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBot1;


public class Telega
{
    public static event Action? RequestStart;
    static Telega? _this;

    TelegramBotClient botClient = new(Secrets.Sinex_CaTelegramBotToken);
    CancellationTokenSource cts = new();

    public static void Init()
    {
        if (_this == null) _this = new();
    }

    Telega()
    {
        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            // receive all update types except ChatMember related updates
            AllowedUpdates = [
                UpdateType.Message,         // Сообщения
                UpdateType.InlineQuery,     // Команды
                UpdateType.CallbackQuery    // Кнопки
            ],
            ThrowPendingUpdates = true
        };

        botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
    }

    Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
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

    async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        switch(update.Type)
        {
            case UpdateType.Message:
                if (update.Message is not { } message) return;
                await OnMessage(update.Message);
                break;
            case UpdateType.InlineQuery:
                break;
            default:
                break;
        }
    }
    async Task OnMessage(Message message)
    {
        var user = message.From;
        var chatId = message.Chat.Id;
        string userName = user == null ? "" : user.Username ?? "";

        await Db.AddCaTeleBotUser(chatId, userName);
    }
    public static async Task SendMessage(string msg)
    {
        List<long> chatIds = Db.GetCaTeleBotUsers();
        foreach (var chId in chatIds)
        {
            try
            {
                if(_this == null) _this = new();

                await _this.botClient.SendTextMessageAsync(
                    chatId: chId,
                    text: msg,
                    parseMode: ParseMode.Html
                );
            }
            catch (Exception ex)
            {
                Log.Error("Send", ex.Message);
            }
        }
    }
}
