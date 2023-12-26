namespace caLibProdStat;

public class CaInfo
{
    public static bool IsDbConnectionOk => new CaDb().Database.CanConnect();

    public static List<AnExchange> Exchanges => new()
    {
        new Binance(),
        new Kucoin(),
        new Huobi(),
        new Bittrex(),
        new Bybit(),
        new Bitfinex(),
        new Kraken(),
        new OKX(),
        new CoinEx(),
        new Mexc(),
        new Gate(),
        new BitMart()
    };

    public static int KlineInterval = 1;
}
