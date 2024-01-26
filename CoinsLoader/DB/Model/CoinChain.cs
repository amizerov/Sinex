namespace CoinsLoader;

public class CoinChain
{
    public int id { get; set; }
    public int coinId { get; set; }
    public int? chainId { get; set; }
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
        await Db.SaveCoinChain(this);
    }
}
