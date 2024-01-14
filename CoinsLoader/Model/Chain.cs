namespace CoinsLoader;

public class Chain
{
    public int id { get; set; }
    public int coinId { get; set; }
    public string? chainName { get; set; }
    public string? contractAddress { get; set; }
    public double? withdrawFee { get; set; }
    public bool? allowDeposit { get; set; }
    public bool? allowWithdraw { get; set; }
    public double? minDepositAmt { get; set; }
    public double? minWithdrawal { get; set; }
    public DateTime? dtu { get; set; }
    public async Task Save()
    {
        await Db.SaveChain(this);
    }
}
