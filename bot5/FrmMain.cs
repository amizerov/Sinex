using CaDb;
using Microsoft.EntityFrameworkCore;

namespace bot5;

public partial class FrmMain : Form
{
    public FrmMain()
    {
        InitializeComponent();
    }

    void LoadProducts()
    {
        using (CaDbContext db = new())
        {
            var prods = db.Database.SqlQuery<ProdEx>($"GetProductsExchanges");
            dgvProds.DataSource = prods.ToList();
            dgvProds.Columns[2].Visible = false;
            dgvProds.Columns[3].Visible = false;
            dgvProds.Columns[4].Visible = false;
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        LoadProducts();
    }
}

record ProdEx
{
    public string symbol { get; set; }
    public string exc { get; set; }
    public int c { get; set; }
    public DateTime dmin { get; set; }
    public DateTime dmax { get; set; }
}