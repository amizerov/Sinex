using amLogger;

namespace TelegramBot1;

public class Bandle
{
    public int Id { get; set; }
    public string coin { get; set; } = "";
    public string exchBuy { get; set; } = "";
    public string exchSell { get; set; } = "";
    public float priceBuyBid { get; set; }
    public float priceBuyAsk { get; set; }
    public float priceSellBid { get; set; }
    public float priceSellAsk { get; set; }
    public float volBuy { get; set; }
    public float volSell { get; set; }
    public float lastBuy { get; set; }
    public float lastSell { get; set; }
    public float lastVolBuy { get; set; }
    public float lastVolSell { get; set; }
    public string chain { get; set; } = "";
    public float withdrawFee { get; set; }
    public DateTime? dtu { get; set; }

    public async Task TryToPublish()
    {
        var (profit, proc, msg) = CreateMessage();

        if (profit > 99 && proc > 1.2 && proc < 15)
        {
            await Telega.SendMessageToAll(msg);
            await Db.SaveBandle(this);
            return;
        }
        if (profit < 99) Log.Trace("Telega", $"profit {profit} < 99");
        if (proc < 1.2) Log.Trace("Telega", $"proc {proc} < 1.2%");
        if (proc > 15) Log.Trace("Telega", $"proc {proc} > 15%");
    }

    public (double, double, string) CreateMessage()
    {
        var pbb = priceBuyBid;
        var pba = priceBuyAsk;
        var pbm = (pbb + pba) / 2;
        var psb = priceSellBid;
        var psa = priceSellAsk;
        var psm = (psb + psa) / 2;
        var proc = Math.Round(100 * (psm - pbm) / pbm, 2);
        var recVol = Math.Round(Math.Min(volBuy, volSell)/3, 2);
        var commis = Math.Round(withdrawFee * pbm, 2);
        var profit = Math.Round(recVol * proc / 100 - commis, 2);

        string msg = $"<b>{coin}</b> {exchBuy}->({chain})->{exchSell}\n\r";
        msg += $"<b>{exchBuy}</b> вывод (тут покупаем) \n\r";
        msg += $"Цена в стакане: {pbm} [{pbb}-{pba}]\n\r";
        msg += $"Цена последней сделки: {lastBuy}\n\r";
        msg += $"Объем в стакане на продажу: {volBuy}$\n\r";
        msg += $"<b>{exchSell}</b> ввод (тут продаем)\n\r";
        msg += $"Цена в стакане: {psm} [{psb}-{psa}]\n\r";
        msg += $"Цена последней сделки: {lastSell}\n\r";
        msg += $"Объем в стакане на покупку: {volSell}$\n\r";
        msg += $"Комиссия за вывод: {commis}$\n\r";
        msg += $"Сеть: <b>{chain}</b>\n\r";
        msg += $"Процент прибыли: <b>{proc}%</b>\n\r";
        msg += $"Рекомендуемая сумма вложения: <b>{recVol}</b>$\n\r";
        msg += $"Прибыль с учетом комиссии: <b>{profit}</b>$";

        return (profit, proc, msg);
    }
}

