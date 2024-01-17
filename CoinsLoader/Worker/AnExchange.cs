namespace CoinsLoader.Worker;

public abstract class AnExchange
{
    public virtual Task GetCoins()
    {
        return Task.CompletedTask;
    }
}
