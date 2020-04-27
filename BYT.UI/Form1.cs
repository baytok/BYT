using BYT.UI.KpsWs;
using BYT.UI.Models.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BYT.UI.ServiceHelper;

namespace BYT.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //ServisManager manager = new ServisManager();
            //var kullanici = manager.KullanicilariGetir("11111111100");
            //var list = manager.IslemleriGetirFromKullanici("11111111100");
            //if (list != null)
            //    dataGridView1.DataSource = list;


        }

        private void btnKontrolGonder_Click(object sender, EventArgs e)
        {
            ServisManager manager = new ServisManager();
            string internalNo = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["isleminternalno"].Value.ToString();
            string kullanici = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["kullanici"].Value.ToString();
            var values = manager.KontrolGonderPost(internalNo, kullanici);
            if (values != null && values.Veri != null)
            {
                MessageBox.Show(values.Veri.Hatalar != null && values.Veri.Hatalar.Count > 0 ? values.Veri.Hatalar[0].HataAciklamasi : values.Veri.Bilgiler[0].ReferansNo + "-" + values.Veri.Bilgiler[0].Sonuc);


                var list = manager.TarihceGetir(internalNo);

                if (list != null)
                    dataGridView2.DataSource = list;
            }

        }




        private void btnBeyanOlustur_Click(object sender, EventArgs e)
        {


            ServisManager manager = new ServisManager();
            //var valuess = manager.BeyannameOlustur("12344");
            var internalrefid = manager.RefIdAl("1000");

            string InternalNo = "11111111100" + "DB" + internalrefid.Veri.Bilgiler[0].SonucVeriler.ToString().PadLeft(6, '0');

            DbBeyan _beyan = new DbBeyan();



            _beyan.RefId = "11111111100" + "|" + "1000" + "|" + internalrefid.Veri.Bilgiler[0].SonucVeriler.ToString().PadLeft(6, '0');
            _beyan.BeyanInternalNo = InternalNo;

            _beyan.Aciklamalar = "deneme";
            _beyan.AliciSaticiIliskisi = "6";
            _beyan.AliciVergiNo = "11111";
            _beyan.AntrepoKodu = "";
            _beyan.AsilSorumluVergiNo = "";
            _beyan.BasitlestirilmisUsul = "";
            _beyan.BankaKodu = "999999999";
            _beyan.BeyanSahibiVergiNo = "11111";
            _beyan.BirlikKriptoNumarasi = "";
            _beyan.BirlikKayitNumarasi = "";
            _beyan.CikisUlkesi = "";
            _beyan.CikistakiAracinKimligi = "araba 34abc123";
            _beyan.CikistakiAracinTipi = "4";
            _beyan.CikistakiAracinUlkesi = "052";
            _beyan.EsyaninBulunduguYer = "garaj";
            _beyan.GidecegiSevkUlkesi = "052";
            _beyan.GidecegiUlke = "052";
            _beyan.GirisGumrukIdaresi = "067777";
            _beyan.GondericiVergiNo = "11111";
            _beyan.Gumruk = "067777";
            _beyan.IsleminNiteligi = "11";
            _beyan.KapAdedi = 10;
            _beyan.Konteyner = "HAYIR";
            _beyan.Kullanici = "11111111100";
            _beyan.LimanKodu = "";
            _beyan.Mail1 = "abc@ggg.com";
            _beyan.Mail2 = "";
            _beyan.Mail3 = "";
            _beyan.Mobil1 = "+905051234567";
            _beyan.Mobil2 = "";
            _beyan.MusavirVergiNo = "11111";
            _beyan.OdemeAraci = "7";
            _beyan.ReferansNo = "000002";
            _beyan.ReferansTarihi = "";
            _beyan.Rejim = "1000";
            _beyan.SinirdakiAracinKimligi = "34abc123";
            _beyan.SinirdakiAracinTipi = "4";
            _beyan.SinirdakiAracinUlkesi = "052";
            _beyan.SinirdakiTasimaSekli = "40";
            _beyan.TasarlananGuzergah = "";
            _beyan.TescilStatu = "Olsuturuldu";
            _beyan.TelafiEdiciVergi = 0;
            _beyan.TeslimSekli = "FOB";
            _beyan.TeslimSekliYeri = "Garaj";
            _beyan.TicaretUlkesi = "052";
            _beyan.ToplamFatura = 100;
            _beyan.ToplamFaturaDovizi = "USD";
            _beyan.ToplamNavlun = 0;
            _beyan.ToplamNavlunDovizi = "";
            _beyan.ToplamSigorta = 0;
            _beyan.ToplamSigortaDovizi = "";
            _beyan.ToplamYurtDisiHarcamalar = 0;
            _beyan.ToplamYurtDisiHarcamalarDovizi = "";
            _beyan.ToplamYurtIciHarcamalar = 0;
            _beyan.VarisGumrukIdaresi = "";
            _beyan.YukBelgeleriSayisi = 10;
            _beyan.YuklemeBosaltmaYeri = "Ev";

            //todo: firma oluştur


            var values = manager.BeyannameOlustur(_beyan);

            DbFirma _firma = new DbFirma();
            _firma.BeyanInternalNo = _beyan.BeyanInternalNo;
            _firma.AdUnvan = "deneme";
            _firma.CaddeSokakNo = "...";
            _firma.No = "123345";
            _firma.UlkeKodu = "052";
            _firma.IlIlce = "Ağrı";
            _firma.Faks = "34343";
            _firma.KimlikTuru = "VergiNo";
            _firma.Tip = "Gonderici";
            manager.FirmaOlustur(_firma);

            if (values != null && values.Veri != null)
                MessageBox.Show(values.Veri.Hatalar != null && values.Veri.Hatalar.Count > 0 ? values.Veri.Hatalar[0].HataAciklamasi : values.Veri.Bilgiler[0].ReferansNo + "-" + values.Veri.Bilgiler[0].Sonuc);
            var list = manager.IslemleriGetirFromKullanici("15781158208");

            if (list != null)
                dataGridView1.DataSource = list;
        }

        private void btnSonucSorgula_Click(object sender, EventArgs e)
        {
            ServisManager manager = new ServisManager();


            //DataGridViewSelectedRowCollection rows = dataGridView2.SelectedRows;
            //string guidOf = rows[0].Cells["guid"].Value.ToString();

            string guidOf = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells["guid"].Value.ToString();

            if (!string.IsNullOrEmpty(guidOf) && guidOf.Substring(0, 3) != "BYT")
            {
                var values = manager.SonuclariGetir(guidOf);
                if (values != null && values.Veri != null && values.Mesaj != null)
                    MessageBox.Show(values.Veri.Bilgiler[0].SonucVeriler.ToString());
            }
        }

        private void dataGridView2_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            else
            {
                btnSonucSorgula.Visible = true;
                //string guid = e.Row.Cells["guid"].Value.ToString();
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            else
            {
                ServisManager manager = new ServisManager();

                var list = manager.TarihceGetir(e.Row.Cells["IslemInternalNo"].Value.ToString());

                if (list != null)
                    dataGridView2.DataSource = list;

            }
            btnKontrolGonder.Visible = true;
            btnKalemOlustur.Visible = true;
        }

        private void btnKalemOlustur_Click(object sender, EventArgs e)
        {
            ServisManager manager = new ServisManager();
            DbKalem _kalem = new DbKalem();


            _kalem.Aciklama44 = "Aciklama44";
            _kalem.Adet = 10;
            _kalem.AlgilamaBirimi1 = "";
            _kalem.AlgilamaBirimi2 = "";
            _kalem.AlgilamaBirimi3 = "";
            _kalem.AlgilamaMiktari1 = 0;
            _kalem.AlgilamaMiktari2 = 0;
            _kalem.AlgilamaMiktari3 = 0;
            _kalem.BrutAgirlik = 100;
            _kalem.Cins = "BI";
            _kalem.EkKod = "";
            _kalem.GirisCikisAmaci = "";
            _kalem.GirisCikisAmaciAciklama = "";
            _kalem.Gtip = "910511000000";
            _kalem.FaturaMiktari = 100;
            _kalem.FaturaMiktariDovizi = "USD";
            _kalem.IkincilIslem = "";
            _kalem.ImalatciFirmaBilgisi = "HAYIR";
            _kalem.ImalatciVergiNo = "";
            _kalem.IstatistikiKiymet = 600;
            _kalem.IstatistikiMiktar = 10;
            _kalem.KalemIslemNiteligi = "";
            _kalem.KalemSiraNo = 1;
            _kalem.KullanilmisEsya = "";
            _kalem.Marka = "iphone";
            _kalem.MahraceIade = "";
            _kalem.MenseiUlke = "400";
            _kalem.Miktar = 10;
            _kalem.MiktarBirimi = "C62";
            _kalem.Muafiyetler1 = "";
            _kalem.Muafiyetler2 = "";
            _kalem.Muafiyetler3 = "";
            _kalem.Muafiyetler4 = "";
            _kalem.Muafiyetler5 = "";
            _kalem.MuafiyetAciklamasi = "";
            _kalem.NavlunMiktari = 0;
            _kalem.NavlunMiktariDovizi = "";
            _kalem.NetAgirlik = 100;
            _kalem.Numara = "12345";
            _kalem.Ozellik = "88";
            _kalem.ReferansTarihi = "";
            _kalem.SatirNo = "";
            _kalem.SigortaMiktari = 0;
            _kalem.SigortaMiktariDovizi = "";
            _kalem.SinirGecisUcreti = 0;
            _kalem.StmIlKodu = "";
            _kalem.TamamlayiciOlcuBirimi = "C62";
            _kalem.TarifeTanimi = "";
            _kalem.TicariTanimi = "TicariTanimi";
            _kalem.TeslimSekli = "FOB";
            _kalem.UluslararasiAnlasma = "";
            _kalem.YurtDisiDiger = 0;
            _kalem.YurtDisiDigerAciklama = "";
            _kalem.YurtDisiDigerDovizi = "";
            _kalem.YurtDisiDemuraj = 0;
            _kalem.YurtDisiDemurajDovizi = "";
            _kalem.YurtDisiFaiz = 0;
            _kalem.YurtDisiFaizDovizi = "";
            _kalem.YurtDisiKomisyon = 0;
            _kalem.YurtDisiKomisyonDovizi = "";
            _kalem.YurtDisiRoyalti = 0;
            _kalem.YurtDisiRoyaltiDovizi = "";
            _kalem.YurtIciBanka = 0;
            _kalem.YurtIciCevre = 0;
            _kalem.YurtIciDiger = 0;
            _kalem.YurtIciDigerAciklama = "";
            _kalem.YurtIciDepolama = 0;
            _kalem.YurtIciKkdf = 0;
            _kalem.YurtIciKultur = 0;
            _kalem.YurtIciLiman = 0;
            _kalem.YurtIciLiman = 0;


            string _beyanInternalNo = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["beyanInternalNo"].Value.ToString();
            _kalem.KalemInternalNo = _beyanInternalNo + "|" + _kalem.KalemSiraNo;
            _kalem.BeyanInternalNo = _beyanInternalNo;

            var values = manager.KalemOlustur(_kalem);

            DbMarka _marka = new DbMarka();
            _marka.BeyanInternalNo = _kalem.BeyanInternalNo;
            _marka.KalemInternalNo = _kalem.KalemInternalNo;
            _marka.MarkaTuru = "0";
            _marka.MarkaAdi = "Marka";
            _marka.MarkaKiymeti = 100;
            _marka.ReferansNo = "1223454545465";
            manager.MarkaOlustur(_marka);

            DbOdemeSekli _odeme = new DbOdemeSekli();
            _odeme.KalemInternalNo = _kalem.KalemInternalNo;
            _odeme.BeyanInternalNo = _kalem.BeyanInternalNo;
            _odeme.OdemeSekliKodu = "3";
            _odeme.OdemeTutari = 1;
            _odeme.TBFID = "12334565777";
            manager.OdemeSekliOlustur(_odeme);

            if (values != null && values.Veri != null)
                MessageBox.Show(values.Veri.Hatalar != null && values.Veri.Hatalar.Count > 0 ? values.Veri.Hatalar[0].HataAciklamasi : values.Veri.Bilgiler[0].ReferansNo + "-" + values.Veri.Bilgiler[0].Sonuc);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            TCKKBilgisi tckk = GetKPSClient.GetTCKKBilgisi(textBox1.Text, "127.0.0.1");
            MessageBox.Show(tckk.Ad + "-" + tckk.Soyad + "-" + tckk.AnneAd + "-" + tckk.BabaAd + "-" + tckk.DogumTarih + "-" + tckk.DogumYer);
        }
    }
}
