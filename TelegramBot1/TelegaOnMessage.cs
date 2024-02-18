using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using amLogger;

namespace TelegramBot1;

public partial class Telega
{
    int _update = 0;
    async Task OnMessage(string? txt, long chatId)
    {
        switch (txt)
        {
            case "/status":
                await ShowStatus(chatId);
                break;
            case "/stscan":
                await StartScan();
                break;
            case "/restart":
                CmdReset();
                break;
            case "/inlineb":
                await _botClient.SendTextMessageAsync(
                     chatId: chatId,
                     text: "Сегодня найдены:",
                     replyMarkup: GetInlineCoinBtns()
                );
                break;
            case "/keybor":
                await _botClient.SendTextMessageAsync(
                     chatId: chatId,
                     text: "Select",
                     replyMarkup: replyKeyboardMarkup
                );
                break;
            case "/nokeyb":
                await ClearKeyboard();
                break;
            /////////////////////////////////////////////////////////////////////
            case "/spread":
                await SendMessageToOne(chatId, "Введите значение спрэда в % от 0,5 до 30");
                _update = 1;
                break;
            case "/minvol":
                await SendMessageToOne(chatId, "Введите минимальное значение объема");
                _update = 2;
                break;
            case "/maxvol":
                await SendMessageToOne(chatId, "Введите максимальное значение объема");
                _update = 3;
                break;
            case "/minpro":
                await SendMessageToOne(chatId, "Введите минимальное значение прибыли");
                _update = 4;
                break;
            default:
                if (_update > 0)
                {
                    double val = double.TryParse(txt, out val) ? val : 0;
                    if (val == 0)
                    {
                        await SendMessageToOne(chatId, "Неверное значение");
                        return;
                    }
                    switch (_update)
                    {
                        case 1:
                            await Db.UpdateSpread(val);
                            break;
                        case 2:
                            await Db.UpdateMinvol(val);
                            break;
                        case 3:
                            await Db.UpdateMaxvol(val);
                            break;
                        case 4:
                            await Db.UpdateMinpro(val);
                            break;
                    }
                    _update = 0;
                    await SendMessageToOne(chatId, "Значение обновлено");
                }
                break;
        }
    }
    async Task ShowStatus(long chatId)
    { 
        Params p = Db.LoadParams();
        string msg = "Сканирование остановлено\n";
        if (_isRunning)
        {
            msg = $"<b>Сканирование запущено</b>\n";
        }
        msg += $"/spread  Мин. спрэд: {p.minProc}\n" +
               $"/minvol  Мин. объем: {p.minVolu}\n" +
               $"/maxvol  Макс. объем: {p.maxVolu}\n" +
               $"/minpro  Мин. профит: {p.minProf}";

        await SendMessageToOne(chatId, msg);
        try
        {
            await _botClient.SendTextMessageAsync(
                 chatId: chatId,
                 text: "Сегодня найдены:",
                 replyMarkup: GetInlineCoinBtns()
            );
        }
        catch (Exception ex)
        {
            Log.Error("ShowStatus", ex.Message);
        }
    }
    async Task StartScan()
    {
        if (_isRunning)
        {
            await SendMessageToAll("Сканирование уже запущено");
            return;
        }
        else
        {
            await SendMessageToAll("Начинаю сканирование");
            CmdStart();
        }
    }
    async Task ClearKeyboard()
    {
        var replyKeyboardRemove = new ReplyKeyboardRemove();
        List<long> chatIds = Db.GetCaTeleBotUsers();
        foreach (var chId in chatIds)
        {
            try
            {
                await _botClient.SendTextMessageAsync(
                     chatId: chId,
                     text: "Кнопки убрал",
                     replyMarkup: replyKeyboardRemove
                );
            }
            catch (Exception ex)
            {
                Log.Error("Remove keyb", ex.Message);
            }
        }
    }
}
