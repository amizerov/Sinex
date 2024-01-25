//using CryptoExchange.Net.Logging;
//TODO: а что это такое?

using amLogger;

namespace caLibPairsStat6;

public class ProductsUpdater
{
    public static void Start(Action? complete)
    {
        List<AnExchange> exchas = CaInfo.Exchanges;
        int cnt = 0;
        int ecnt = exchas.Count;

        foreach (AnExchange ex in exchas)
            Task.Run(() => {
                Log.Info(ex.ID, $"ProductsUpdater({ex.Name})", "Started");
                ex.ProcessProducts();
                Log.Info(ex.ID, $"ProductsUpdater({ex.Name})", "Comleted");
                cnt++;
                Log.Info(ex.ID, $"ProductsUpdater({ex.Name})", $"cnt = {cnt} of {ecnt}");
                if (cnt == ecnt)
                {
                    Log.Info(ex.ID, $"ProductsUpdater({ex.Name})", "cnt == exs.Count");
                    if (complete != null)
                    {
                        complete();
                        Log.Info(ex.ID, $"ProductsUpdater({ex.Name})", "complete is called!");
                    }
                }
            });
    }
}
