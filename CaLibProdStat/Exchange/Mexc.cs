using CryptoExchange.Net.CommonObjects;

namespace caLibProdStat;

public class Mexc : AnExchange
{
    public override int ID => 10;

    public override string Name => "Mexc";

    protected override List<Kline> GetLastKlines(string symbol)
    {
        throw new NotImplementedException();
    }

    protected override List<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    protected override Product ToProduct(object p)
    {
        throw new NotImplementedException();
    }
}
