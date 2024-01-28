namespace CoinsLoader.Worker;

public abstract class AnExchange
{
    public virtual Task GetCoins()
    {
        return Task.CompletedTask;
    }
    protected string ValidateChainCode(string chainName)
    {
        string code = chainName;

        if (code == "ETH") code = "ERC20";
        if (code == "TRX") code = "TRC20";
        if (code == "BSC") code = "BEP20";
        if (code == "BTC") code = "BRC20";

        return code;
    }
}
