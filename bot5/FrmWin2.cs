using amLogger;
using CaSecrets;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
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

        if(File.Exists("ExcludeExches.txt"))
            txtExch.Text = File.ReadAllText("ExcludeExches.txt");
        if (File.Exists("ExcludeMonets.txt"))
            txtMon.Text = File.ReadAllText("ExcludeMonets.txt");
    }

    public void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!this.Visible) return;

        dgvProds.DataSource = null;

        string filterExc = ("'" + txtExch.Text + "'").Replace(" ", "").Replace(",", "','");
        string filterMon = ("'" + txtMon.Text + "'").Replace(" ", "").Replace(",", "','");

        List<Arbitrage> arbis = Db.GetArbitrages(filterExc, filterMon);



        dgvProds.DataSource = arbis;
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
        await Db.AddCaTeleBotUser(chatId);
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

        string filterExc = ("'" + txtExch.Text + "'").Replace(" ", "").Replace(",", "','");
        string filterMon = ("'" + txtMon.Text + "'").Replace(" ", "").Replace(",", "','");

        var prods = Db.GetArbitrages(filterExc, filterMon, true);
        foreach (var p in prods)
        {
            msg += $"{p.baseAsset} - {p.exch1}/{p.exch2} - {p.procDiffer}% - {p.vol1}/{p.vol2}\n\r";
            Db.SetSentFlagForArbitrage(p.ID);
        }
        return msg;
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

    private void FrmWin2_FormClosing(object sender, FormClosingEventArgs e)
    {
        File.WriteAllText("ExcludeExches.txt", txtExch.Text);
        File.WriteAllText("ExcludeMonets.txt", txtMon.Text);
    }
}
