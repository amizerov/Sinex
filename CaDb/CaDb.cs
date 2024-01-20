using Microsoft.EntityFrameworkCore;
using CaSecrets;

namespace CaDb;
public class CaDbContext : DbContext
{
    private String SqlConnectionString;
    public CaDbContext()
    {
        SqlConnectionString = Secrets.SqlConnectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(SqlConnectionString);
    }
    //public virtual DbSet<Product>? Products { get; set; }
    public DbSet<QuoteAsset>? Quotes { get; set; }
}