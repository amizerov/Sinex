using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoinsLoader;
public class Chain
{
    public int id { get; set; }
    public string? code { get; set; }
    public string? name { get; set; }
    public string? name1 { get; set; }
    public string? name2 { get; set; }
    public DateTime? dtu { get; set; }

    public Chain()
    {
    }
    public Chain(string chainCode)
    {
        this.code = chainCode;
    }
    public async Task<int> Save()
    {
        return await Db.SaveChain(this);
    }
}
