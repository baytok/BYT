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
        string sqlconnProd = @"data source=LAPTOP-IRDC0I6A,1433;uid=bytapp;password=bytapp123!!; initial catalog=BYTDb";
        public string Kullanici;
        public FrmListe()
        {
            InitializeComponent();
        }

        private void FrmListe_Load(object sender, EventArgs e)
        {
            SqlDataAdapter mydap = new SqlDataAdapter();
            DataSet myds = new DataSet();
            using (SqlConnection connection = new SqlConnection(sqlconnProd))
            {
                string commandText = "Select refno,islemInternalNo,beyanTipi,islemDurumu,islemSonucu,sonIslemZamani,GonderimSayisi,beyanNo, guidof from Islem Where guidof!='' and Kullanici = @Kullanici;";
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add("@Kullanici", SqlDbType.VarChar);
                command.Parameters["@Kullanici"].Value = Kullanici;
                mydap.SelectCommand = command;

                try
                {
                    connection.Open();
                    mydap.Fill(myds);
                    dataGridView1.DataSource = myds.Tables[0];
                }
                catch (Exception exc)
                {

                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string guidOf = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["guidof"].Value.ToString();
            BYTIslemler myDetail = new BYTIslemler(guidOf, null);
            myDetail.Show();
        }
    }
}
