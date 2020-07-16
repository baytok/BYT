using BYT.UI.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYT.UI
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            ServisManager manage = new ServisManager();
            var result=manage.GirisPost(txtKullanici.Text,txtSifre.Text);

            if (result.ServisDurumKodlari == ServisDurumKodlari.IslemBasarili)
            {
                var token = result.KullaniciBilgileri.Token;
             
                var kullaniciKod = result.KullaniciBilgileri.KullaniciKod;
                var kullaniciAdi = result.KullaniciBilgileri.KullaniciAdi;
                var yetkiler = result.KullaniciBilgileri.Yetkiler;
                FrmListe myList = new FrmListe();
                myList.Kullanici = txtKullanici.Text;
                myList.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(result.Hatalar[0].HataAciklamasi);
            }
        }

        private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtKullanici.Text!="" && txtSifre.Text!="" &&  e.KeyChar==Convert.ToChar(Keys.Enter))
                btnGiris_Click(sender,null);
        }
    }
}
