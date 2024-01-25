namespace CaExch2;

public class CaExchanges : List<AnExchange>
{
    public CaExchanges() {
        Add(new CaKucoin());
        Add(new CaBybit());
        Add(new CaOKX());
        Add(new CaCoinEx());
        Add(new CaMexc());
        Add(new CaGate());
        Add(new CaBitMart());
        Add(new CaAscendEx());
        Add(new CaBingX());
        Add(new CaBitGet());
    }
}
