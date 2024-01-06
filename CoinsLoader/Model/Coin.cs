namespace CoinsLoader;

public class Coin
{
    public int id { get; set; }
    public int exchId { get; set; }
    public string? asset { get; set; }
    public string? network { get; set; }
    public string? contract { get; set; }
    public string? logoPath { get; set; }
    public string? longName { get; set; }
    public string? description { get; set; }
    public bool? allowDeposit { get; set; }
    public bool? allowWithdraw { get; set; }
    public DateTime? dtu { get; set; }
    
    //public DateTime dtc { get; set; }

    public async Task Save()
    {
        await Db.SaveCoin(this);
    }
}

