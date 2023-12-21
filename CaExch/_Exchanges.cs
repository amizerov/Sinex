namespace CaExch
{
    public class CaExchanges : List<AnExchange>
    {
        public CaExchanges() {
            this.Add(new CaBinance());
            this.Add(new CaBittrex());
            this.Add(new CaBybit());
            this.Add(new CaHuobi());
            this.Add(new CaKucoin());
            this.Add(new CaKraken());
            Add(new CaBitfinex());
            Add(new CaOKX());
            Add(new CaCoinEx());
            Add(new CaMexc());
            Add(new CaGate());
        }
    }
}
