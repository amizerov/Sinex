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

    public int exchId = 0;

    public Chain(string code)
    {
        this.code = code;
    }
    public async Task Save()
    {
        await Db.SaveChain(this);
    }
}
