//using CryptoExchange.Net.Logging;
//TODO: а что это такое?

using amLogger;

namespace caLibProdStat;

public class ProductsUpdater
{
    public static void Start(Action? complete)
    {
        List<AnExchange> exchas = CaInfo.Exchanges;
        int cnt = 0;
        int ecnt = exchas.Count;

        foreach (AnExchange ex in exchas)
            Task.Run(() => {
                Log.Info($"ProductsUpdater({ex.Name})", "Started");
                ex.ProcessProducts();
                Log.Info($"ProductsUpdater({ex.Name})", "Comleted");
                cnt++;
                Log.Info($"ProductsUpdater({ex.Name})", $"cnt = {cnt} of {ecnt}");
                if (cnt == ecnt)
                {
                    Log.Info($"ProductsUpdater({ex.Name})", "cnt == exs.Count");
                    if (complete != null)
                    {
                        complete();
                        Log.Info($"ProductsUpdater({ex.Name})", "complete is called!");

                    }
                }
            });
    }
}
