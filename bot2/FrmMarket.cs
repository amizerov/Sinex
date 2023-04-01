using CaExch;
using Microsoft.EntityFrameworkCore;

namespace bot2
{
    public partial class FrmMarket : Form
    {
        public List<AnExchange> Exchanges = new(){
        new CaBinance(),
        new CaKucoin(),
        new CaHuobi(),
        new CaBittrex(),
        new CaBybit()
    };
        public FrmMarket()
        {
            InitializeComponent();
        }

        private void FrmMarket_Load(object sender, EventArgs e)
        {
            cbExchange.DisplayMember = "Name";
            foreach (var exchange in Exchanges)
            {
                cbExchange.Items.Add(exchange);
            }
            cbExchange.SelectedIndex = 0;
        }
        void LoadProducts()
        {
            using (CaDbContext dbContext = new CaDbContext())
            {
                int ExId = ((AnExchange)cbExchange.SelectedItem).ID;
                string search = txtSearch.Text.ToLower();

                var prods = dbContext.Products?.FromSql($"Sinex_Get_Products {ExId}, {search}");

                dgProducts.DataSource = prods?.ToList();
            }
        }

        private void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void dgProducts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewColumnCollection cols = dgProducts.Columns;
            cols[0].Visible = false;
            cols[2].Visible = false;
            foreach (DataGridViewColumn c in cols)
            {
                if (c.Index > 5) c.Visible = false;
            }
        }

        private void dgProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string? ex = dgProducts.Rows[e.RowIndex].Cells[2].Value.ToString();
            string? sy = dgProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (sy == null || ex == null) return;
            int exch_id = int.Parse(ex);
            string symbo = sy;

            FrmChart ch = new(exch_id, symbo);
            ch.Show(this);
        }
    }
}
