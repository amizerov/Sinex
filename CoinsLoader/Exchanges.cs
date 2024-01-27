using CoinsLoader.Worker;
namespace CoinsLoader;

public class Exchanges: List<AnExchange>
{
    static Exchanges? _this;
    public static Exchanges List()
    {
        if(_this != null) return _this;

        _this = new();

        //_this.Add(new Kucoin());      //  2
        //_this.Add(new Bybit());       //  5
        //_this.Add(new CaOKX());       //  8
        //_this.Add(new CoinEx());      //  9  
        //_this.Add(new Mexc());        // 10
        //_this.Add(new Gate());        // 11
        //_this.Add(new BitMart());     // 12
        _this.Add(new AscendEx());    // 13
        //_this.Add(new BingX());       // 15
        //_this.Add(new BitGet());      // 16

        return _this;
    }

}
