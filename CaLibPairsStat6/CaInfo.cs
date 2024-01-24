namespace caLibPairsStat6;

public class CaInfo
{
    public static bool IsDbConnectionOk => new Db().Database.CanConnect();

    public static List<AnExchange> Exchanges => new()
    {
        new Kucoin(),
        new Bybit(),
        new CaOKX(),
        new CoinEx(),
        new Mexc(),
        new Gate(),
        new BitMart(),
        new AscendEx(),
        new BingX(),
        new BitGet(),
    };

    public static int KlineInterval = 1;
}
