using Microsoft.EntityFrameworkCore;

namespace CaDb;
public class CaDbContext : DbContext
{
    private String SqlConnectionString;
    public CaDbContext()
    {
        string path = "D:\\Projects\\Common\\Secrets\\SqlConnectionStringForCaProgerX.txt";
        if (File.Exists(path))
        {
            SqlConnectionString = File.ReadAllText(path);
        }
        else
            throw new Exception("File with Sql Connection is not found");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(SqlConnectionString);
    }
    public DbSet<Product>? Products { get; set; }
}