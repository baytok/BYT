using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYT.UI
{
    
    public partial class FrmListe : Form
    {
      
        public string Kullanici, Token;
        public FrmListe()
        {
            InitializeComponent();
        }

        private void FrmListe_Load(object sender, EventArgs e)
        {
            ServisManager manager = new ServisManager();
            var list = manager.IslemleriGetir(Kullanici, Token);

            if (list != null)
                dataGridView1.DataSource = list;

            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            ServisManager manager = new ServisManager();
            var list = manager.IslemleriGetir(Kullanici, Token);

            if (list != null)
                dataGridView1.DataSource = list;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string guidOf = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["guidof"].Value.ToString();
            BYTIslemler myDetail = new BYTIslemler(guidOf, null);
            myDetail.Token = Token;
            myDetail.Show();
        }
    }
}
