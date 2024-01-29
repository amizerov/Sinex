using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;

namespace CaExch2;

public class CaExchanges : List<AnExchange>
{
    static CaExchanges? _this;
    public static CaExchanges List()
    {
        if (_this != null) return _this;

        _this = new();

        _this.Add(new CaKucoin());      //  2
        _this.Add(new CaBybit());       //  5
        _this.Add(new CaOKX());       //  8
        _this.Add(new CaCoinEx());      //  9  
        _this.Add(new CaMexc());        // 10
        _this.Add(new CaGate());        // 11
        _this.Add(new CaBitMart());     // 12
        _this.Add(new CaAscendEx());    // 13
        _this.Add(new CaBingX());       // 15
        _this.Add(new CaBitGet());      // 16

        return _this;
    }

}