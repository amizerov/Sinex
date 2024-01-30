namespace bot5;

public class Bandle
{
    public int Id { get; set; }
    public string coin { get; set; } = "";
    public string exchBuy { get; set; } = "";
    public string exchSell { get; set; } = "";
    public float priceBuyBid { get; set; }
    public float priceBuyAsk { get; set; }
    public float priceSellBid { get; set; }
    public float priceSellAsk { get; set; }
    public float volBuy { get; set; }
    public float volSell { get; set; }
    public string chain { get; set; } = "";
    public float withdrawFee { get; set; }
    public DateTime? dtu { get; set; }
}

