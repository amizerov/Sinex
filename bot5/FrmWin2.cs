using CaDb;
using Microsoft.EntityFrameworkCore;

namespace bot5;

public partial class FrmWin2 : Form
{
    public FrmWin2()
    {
        InitializeComponent();
    }

    private void FrmWin2_Load(object sender, EventArgs e)
    {
        btnUpdate.PerformClick();
    }

    public void btnUpdate_Click(object sender, EventArgs e)
    {
        var prods = new List<Arbitrage>();
        dgvProds.DataSource = null;
        using (CaDbContext db = new())
        {
            prods = db.Database
                .SqlQuery<Arbitrage>(
                    @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage

                        select * from Sinex_Arbitrage 
                        where 
                            shotNumber = @n
                        and vol1 > 0
                        and vol2 > 0
                        and procDiffer > 1.5
                        order by procDiffer desc
                    "
            ).ToList<Arbitrage>();
        }

        dgvProds.DataSource = prods;
        dgvProds.Columns[0].Visible = false;
        dgvProds.Columns[1].Visible = false;
        dgvProds.Columns[3].Visible = false;
        //dgvProds.Columns[5].Visible = false;

        //dgvProds.Columns[0].Width = 60;
        //dgvProds.Columns[1].Width = 80;
        //dgvProds.Columns[2].Width = 50;
    }
}

class Arbitrage
{
    public int ID { get; set; }
    public int shotNumber { get; set; }
    public string? baseAsset { get; set; }
    public string? exchanges { get; set; }
    public string? exch1 { get; set; }
    public string? exch2 { get; set; }
    public double? procDiffer { get; set; }
    public double? vol1 { get; set; }
    public double? vol2 { get; set; }
}