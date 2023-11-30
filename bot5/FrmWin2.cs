using amLogger;
using CaSecrets;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;
using Update = Telegram.Bot.Types.Update;

namespace bot5;

public partial class FrmWin2 : Form
{
    TelegramBotClient botClient = new(Secrets.Sinex_CaTelegramBotToken);
    CancellationTokenSource cts = new();

    public FrmWin2()
    {
        InitializeComponent();
    }

    private void FrmWin2_Load(object sender, EventArgs e)
    {
        btnUpdate.PerformClick();

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

    public void btnUpdate_Click(object sender, EventArgs e)
    {
        dgvProds.DataSource = null;

        dgvProds.DataSource = Data.GetArbitrages();
        dgvProds.Columns[0].Visible = false;
        dgvProds.Columns[1].Visible = false;
        dgvProds.Columns[3].Visible = false;
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
        string msg = CreateMessage();
        if (string.IsNullOrEmpty(msg)) return;

        await SendMessage(msg);   
    }
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        await Data.AddCaTeleBotUser(chatId);
    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    string CreateMessage()
    {
        string msg = "";
        dgvProds.DataSource = null;

        var prods = Data.GetArbitragesToSend();
        foreach (var p in prods)
        {
            msg += $"{p.baseAsset} - {p.exch1}/{p.exch2} - {p.procDiffer}% - {p.vol1}/{p.vol2}\n\r";
            Data.UpdateSentArbitr(p.ID);
        }
        return msg;
    }
    async Task SendMessage(string msg)
    {
        List<long> chatIds = Data.GetCaTeleBotUsers();
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
}
