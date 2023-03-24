namespace caLibProdStat;

public class CaInfo
{
    public static bool IsDbConnectionOk => new CaDb().Database.CanConnect();

    public static List<AnExchange> ExchasList => new()
    {
        new Binance(),
        new Kucoin(),
        new Huobi(),
        new Bittrex()
    };

    public static int KlineInterval = 1;
}
