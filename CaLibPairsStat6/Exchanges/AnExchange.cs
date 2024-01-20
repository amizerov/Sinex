using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibPairsStat6;

public abstract class AnExchange
{
    public abstract int ID { get; }
    public virtual string Name => GetType().Name;

    /// <summary>
    /// Main method of the service.
    /// Get historical data for product,
    /// analyse and store results to db for futher use
    /// </summary>
    public void ProcessProducts()
    {
        try
        {
            List<Product> products = GetProducts();
            foreach (var product in products)
            {
                List<Kline> klines;
                try
                {
                    klines = GetLastKlines(product.symbol);

                    product.GetDetails();
                    product.CalcStat(klines);
                    product.SaveStatToDb();

                    int Number = products.IndexOf(product);
                    Log.Trace(ID, $"ProcessProducts({product.symbol})",
                        $"{Number} of {products.Count} - [{product.TraceMessage}]");

                    if(product.exchange == 4/*Bittrex*/)
                        Thread.Sleep(3000);
                    else
                        Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Log.Error(ID, $"GetKlines({product.symbol})", "Error: " + e.Message);
                }
            }
        }
        catch(Exception e)
        {
            Log.Error(ID, "ProcessProducts", "Error: " + e.Message);
        }
    }
    public abstract Coin GetCoinDetails(string baseAsset);

    /// <summary>
    /// Convert cpecific product for each exchange 
    /// to one model common class Product
    /// </summary>
    /// <param name="p">Object of type product for this exchange</param>
    /// <returns>Product of common type</returns>
    protected abstract Product ToProduct(Object p);

    /// <summary>
    /// Get list of all products
    /// </summary>
    /// <returns>List of all products for this exchange</returns>
    protected abstract List<Product> GetProducts();

    /// <summary>
    /// Get klines for symbol for a futher analysis
    /// </summary>
    /// <param name="symbol">Symbol of the product</param>
    /// <param name="IntervarInMinutes">Kline interval in minutes</param>
    /// <returns></returns>
    protected abstract List<Kline> GetLastKlines(string symbol);
}
