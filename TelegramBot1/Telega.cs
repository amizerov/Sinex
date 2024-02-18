using amLogger;
using CaSecrets;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBot1;

public partial class Telega
{
    public static event Action? cmdStart; void CmdStart() => cmdStart?.Invoke();
    public static event Action? cmdReset; void CmdReset() => cmdReset?.Invoke();

    public static bool IsRunning { get => _isRunning; 
        set
        {
            _isRunning = value;
            string msg = _isRunning ? "Сканирование запущено" : "Сканирование остановлено";
            SendMessageToAll(msg);
        } 
    }
    static bool _isRunning = false;

    static Telega? _this;
    TelegramBotClient _botClient;
    CancellationTokenSource _cts;

    public static void Init()
    {
        if (_this == null) _this = new();
    }

    Telega()
    {
        _cts = new();
        _botClient = new(Secrets.Sinex_CaTelegramBotToken);
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery, UpdateType.InlineQuery],
            ThrowPendingUpdates = true,
        };
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, _cts.Token);
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
        if (update.CallbackQuery != null)
        {
            var cid = update.CallbackQuery.From.Id;
            var data = update.CallbackQuery.Data ?? "пусто";
            await SendMessageToOne(cid, data);
        }

        var message = update.Message;
        if(message == null) return;
        var chatId = message.Chat.Id;
        if (message.Type != MessageType.Text) return;
        string userName = message.From == null ? "" : message.From.Username ?? "";

        await Db.AddCaTeleBotUser(chatId, userName);

        await OnMessage(message.Text, chatId);
    }
    async Task SendMessageToOne(long chatId, string msg)
    {
        try
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: msg,
                parseMode: ParseMode.Html
            );
        }
        catch (Exception ex)
        {
            Log.Error("Send", ex.Message);
        }
    }
    public static async Task SendMessageToAll(string msg)
    {
        List<long> chatIds = Db.GetCaTeleBotUsers();
        foreach (var chId in chatIds)
        {
            try
            {
                if (_this == null) _this = new();

                await _this._botClient.SendTextMessageAsync(
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
