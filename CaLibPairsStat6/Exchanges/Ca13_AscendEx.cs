using CryptoExchange.Net.CommonObjects;

namespace caLibPairsStat6;

public class AscendEx : AnExchange
{
    public override int ID => 13;
    public const string BASE_URL = "https://ascendex.com";

    public override Coin GetCoinDetails(string baseAsset)
    {
        throw new NotImplementedException();
    }

    protected override Product ToProduct(object p)
    {
        throw new NotImplementedException();
    }

    protected override List<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    protected override List<Kline> GetLastKlines(string symbol)
    {
        throw new NotImplementedException();
    }
}
