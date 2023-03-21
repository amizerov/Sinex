namespace caLibProdStat;

public class CaInfo
{
    public static bool IsDbConnectionOk => new CaDb().Database.CanConnect();

    public static List<AnExchange> ExchasList => new()
    {
        new Binance(),
        new Kucoin(),
        new Huobi()
    };

    public static int KlineInterval = 30;
}
