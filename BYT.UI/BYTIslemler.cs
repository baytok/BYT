using ArnicaCS_VPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BYT.UI
{
    public partial class BYTIslemler : Form
    {
        public SorgulamaHizmeti.GumrukWSSoapClient mysn = ServiceHelper.GetSonucWSClient("", "");
        public NumberFormatInfo nfi1 = new CultureInfo("en-US", false).NumberFormat;
        string sqlconnProd = @"data source=LAPTOP-IRDC0I6A,1433;uid=bytapp;password=bytapp123!!; initial catalog=BYTDb";

        public string Guid;
        public BYTIslemler(string s1, string s2)
        {
            Guid = s1;
            InitializeComponent();
        }
        private void BYTIslemler_Load(object sender, EventArgs e)
        {
            SqlDataAdapter mydap = new SqlDataAdapter();
            DataSet myds = new DataSet();
            XmlDocument dd = new XmlDocument();
            string commandText = "Select * from Tarihce Where Guid = @GUID;";

            using (SqlConnection connection = new SqlConnection(sqlconnProd))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add("@GUID", SqlDbType.VarChar);
                command.Parameters["@GUID"].Value = Guid;
                mydap.SelectCommand = command;
               
                try
                {
                    connection.Open();
                    mydap.Fill(myds);
                    if(myds!=null && myds.Tables!=null && myds.Tables.Count>0 && myds.Tables[0]!=null && myds.Tables[0].Rows.Count>0)
                    {
                        TxtGuid.Text = Guid;                        
                        TxtGuidTarih.Text = myds.Tables[0].Rows[0]["OlusturmaZamani"].ToString();
                        TxtBasTarih.Text= myds.Tables[0].Rows[0]["GondermeZamani"].ToString();
                        TxtGidTarih.Text = myds.Tables[0].Rows[0]["SonucZamani"].ToString();
                        TxtRefID.Text = myds.Tables[0].Rows[0]["RefNo"].ToString();
                        TxtTip.Text = myds.Tables[0].Rows[0]["IslemTipi"].ToString();
                        TxtTicaret.Text = myds.Tables[0].Rows[0]["TicaretTipi"].ToString();
                        TxtDurum.Text = myds.Tables[0].Rows[0]["IslemDurumu"].ToString();
                        TxtGumruk.Text = myds.Tables[0].Rows[0]["Gumruk"].ToString();
                        TxtRejim.Text = myds.Tables[0].Rows[0]["Rejim"].ToString();
                        TxtIslemSonucu.Text = myds.Tables[0].Rows[0]["IslemSonucu"].ToString();
                        rchImza.Text= myds.Tables[0].Rows[0]["ImzaliVeri"].ToString();

                        dd.InnerXml = myds.Tables[0].Rows[0]["GonderilecekVeri"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"); 
                        xmlGidecekVeri.XmlDocument = dd;
                        if (myds.Tables[0].Rows[0]["SonucVeri"].ToString() != "")
                        {
                            dd.InnerXml = myds.Tables[0].Rows[0]["SonucVeri"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
                            xmlSonuc.XmlDocument = dd;
                        }

                        if (rchImza.Text != "")
                            btnXml.Visible = !btnXml.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnImza_Click(object sender, EventArgs e)
        {
            object obj = xmlGidecekVeri.XmlText;
            if (xmlGidecekVeri.XmlText != "")
            {
                rchImza.Text = "";


                rchImza.Text = MesajImzalaT(xmlGidecekVeri.XmlText);
                //   rchImza.Text = MesajImzala(rchBeyanname.Text);
                if (rchImza.Text != "")
                    btnImzaliVeriKaydet.Visible = !btnImzaliVeriKaydet.Visible;
            }

        }
        private string MesajImzalaT(string msg) // msg imzalanack olan dosya 
        {
            return "fdkfmdklfmdkv fvfmvfokvmdocvmdokcscdcdcdcdcdcdcdcddddddddddddfdgthtrhyjyjghghfgrgbtbgfbgfggtgfbhgnhnhgbngbgfdbgfdbgfdbgfdbgfgbfgfgfgfgfdbgfhfshsgfhbghghbgsfbgaregtrytkjuylıoşluıyujzdhgrqwytjkykdthgfhtgfdhtgfnhgmjyhgfngfdhtrjmngtshgtruyıdtukymntzfdhatesryjkduıykjmhnyrthbfxndtukhjgntfdgasytkuhgnfghbvmdkcvmdokcmdokvmdokcvmdovkdvodmvkdkvmdvddm od odmvodmvodvmdovcmdkvmdvşdvşfdnvjşteogjtğoıhiktigndvşjkfdbjkg";
            ImzaIslemNesnesi ines;
            kokler eshsCerts = new kokler();
            ines = new ImzaIslemNesnesi();
            for (int i = 0; i < eshsCerts.getBase64Certs().Length; i++)
            {
                ines.eshsSertifikasiYukle(System.Convert.FromBase64String(eshsCerts.getBase64Certs()[i]));
            }

            string kartFirmaKutuphanesi = "akisp11.dll";
            int slotIndeks = 0;
            int certIndeks = 0;
            string kartPin = "";
            string imzalanacakStr = "";

            if (msg.Length == 0)
            {
                MessageBox.Show("İmzalanacak veri giriniz");
                return null;
            }
            else
                imzalanacakStr = msg;

            //if (this.txtPin.Text.Length == 0)
            //{
            //    MessageBox.Show("PIN giriniz");
            //    return;
            //}
            //else
            if (txtsifre.Text != "" && txtsifre.Text.Length >= 4)
                kartPin = txtsifre.Text;
            else
            {
                MessageBox.Show("Kart Şifresi boş olamaz");
                return null;
            }

            int res = ines.akilliKartFirmaKutuphanesiYukle(kartFirmaKutuphanesi);
            // işlem adım sonuçlarını ImzaIslemNesnesi üzerindeki sabitlerle kontrol ediyoruz
            if (res != ImzaIslemNesnesi.ISLEM_TAMAM)
            {
                MessageBox.Show("Akıllı kart firma kütüphanesi yüklenemedi, hata kodu = " + res);
                return null;
            }

            res = ines.akilliKartOturumuAc(slotIndeks);
            if (res != ImzaIslemNesnesi.ISLEM_TAMAM)
            {
                MessageBox.Show("Akıllı kart oturumu açılamadı, hata kodu = " + res);
                ines.akilliKartIslemleriniSonlandir();
                return null;
            }

            res = ines.sertifikayiDogrula(slotIndeks, certIndeks);
            if (res != ImzaIslemNesnesi.SERTIFIKA_GECERLI)
            {
                MessageBox.Show("Akıllı karttaki sertifika doğrulanamadı, hata kodu = " + res);
                ines.akilliKartOturumuKapat(slotIndeks);
                ines.akilliKartIslemleriniSonlandir();
                return null;
            }

            res = ines.akilliKarttakiSertifikaOCSPSorgusuYap(slotIndeks, certIndeks);
            if (res != ImzaIslemNesnesi.OCSP_SERTIFIKA_GECERLI)
            {
                MessageBox.Show("Akıllı karttaki sertifika geçerlilik kontrolu olumsuz, hata kodu = " + res);
                ines.akilliKartOturumuKapat(slotIndeks);
                ines.akilliKartIslemleriniSonlandir();
                return null;
            }

            byte[] certBytes = ines.sertifikayiAl(slotIndeks, certIndeks);
            SertifikaBilgiNesnesi sbn = new SertifikaBilgiNesnesi(certBytes);
            MessageBox.Show("Sertifika sahibi: " + sbn.sertifikaSahibiIsimAl());

            res = ines.akilliKartLogin(slotIndeks, kartPin);
            if (res != ImzaIslemNesnesi.ISLEM_TAMAM)
            {
                MessageBox.Show("Akıllı kart login hatası, hata kodu = " + res);
                ines.akilliKartOturumuKapat(slotIndeks);
                ines.akilliKartIslemleriniSonlandir();
                return null;
            }

            byte[] imzaBytes = ines.imzaOlustur(slotIndeks, certIndeks, System.Text.UTF8Encoding.UTF8.GetBytes(imzalanacakStr));

            if (imzaBytes != null)
            {
                MessageBox.Show("İmza oluşturuldu");
                //MessageBox.Show(System.Convert.ToBase64String(imzaBytes));
                //textBox1.Multiline = true;
                ines.akilliKartOturumuKapat(slotIndeks);
                ines.akilliKartIslemleriniSonlandir();

                return System.Convert.ToBase64String(imzaBytes);


            }
            else
            {
                MessageBox.Show("Hata: İmza oluşturulamadı");
                ines.akilliKartOturumuKapat(slotIndeks);
                ines.akilliKartIslemleriniSonlandir();
                return null;
            }


        }
        public static string SerializeToXML(object responseObject)
        {
            //DefaultNamespace of XSD
            //We have to Namespace compulsory, if XSD using any namespace.

            XmlSerializer xmlObj = new XmlSerializer(responseObject.GetType());
            String XmlizedString = null;

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter XmlObjWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            XmlObjWriter.Formatting = Formatting.Indented;
            xmlObj.Serialize(XmlObjWriter, responseObject);
            memoryStream = (MemoryStream)XmlObjWriter.BaseStream;

            UTF8Encoding encoding = new UTF8Encoding();
            XmlizedString = encoding.GetString(memoryStream.ToArray());
            XmlObjWriter.Close();
            XmlizedString = XmlizedString.Substring(1);

            return XmlizedString;
        }
        private string MesajImzala(string msg) // msg imzalanack olan dosya 
        {

            Encoding unicode = Encoding.UTF8;
            X509Certificate2 signerCert = null;
            string serino = "";

            try
            {
                byte[] msgBytes = unicode.GetBytes(msg);

                // Sertifika Numarası aşağıdaki gibi girilirse direk pin sorulacaktır
                serino = "151E2ACB0C0000F2";

                //if (signerCert == null)
                //    signerCert = BYT.UI.Imzala.GetSignerCert("151E2ACB0C0000F2");


                string result = Imzala.MesajImzala(msg, serino);

                byte[] encodedSignedCms = unicode.GetBytes(msg);

                //    System.IO.File.WriteAllBytes("imzali.txt", encodedSignedCms);

                //if (!Imzala.VerifyMsg(msgBytes, encodedSignedCms))
                //{

                //    throw new Exception("Mesaj Doğrulanamadı");

                //}
                return result;
            }
            catch
            {
                MessageBox.Show("Sertifika Bulunamadı");
            }

            return null;

        }

        private void btnKontrol_Click(object sender, EventArgs e)
        {
            KontrolHizmeti.Gumruk_Biztalk_EImzaTescil_Kontrol_KontrolTalepPortSoapClient kontrol = ServiceHelper.GetKontrolWsClient("", "");

            KontrolHizmeti.Gelen gelen = new KontrolHizmeti.Gelen();
            XmlNode me = null;
            try
            {
                KontrolHizmeti.Gelen myGelenEGEWSTEST = new KontrolHizmeti.Gelen();
                KontrolHizmeti.BeyannameBilgi mynewtcgb = new KontrolHizmeti.BeyannameBilgi();

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                {
                    return true;
                };

                if (!DosyaHazirlaEGEWSTEST(myGelenEGEWSTEST, mynewtcgb))
                {
                    MessageBox.Show("Veri Uyuşmazlığı var");
                    return;
                }

                me = kontrol.Kontrol(myGelenEGEWSTEST);




                MessageBox.Show(me.OuterXml);
                XmlDocument dd = new XmlDocument();
                dd.LoadXml(me.OuterXml);
                XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("Guid");
                string Guid = XmlNodeListObj2[0].ChildNodes[0].Value;
                TxtGuid.Text = Guid;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(me.OuterXml);
            }
        }
        bool DosyaHazirlaEGEWSTEST(KontrolHizmeti.Gelen myGelenEGEWSTEST, KontrolHizmeti.BeyannameBilgi mynewtcgb)
        {
            try
            {

                myGelenEGEWSTEST.BeyannameBilgi = new KontrolHizmeti.BeyannameBilgi();
                XmlDocument dd = new XmlDocument();

                dd.LoadXml(xmlGidecekVeri.XmlText);

                XmlNodeList XmlNodeListObj111 = dd.GetElementsByTagName("KullaniciAdi");
                if (XmlNodeListObj111[0] != null)
                    myGelenEGEWSTEST.KullaniciAdi = "15781158208";

                XmlNodeList XmlNodeListObj333 = dd.GetElementsByTagName("Sifre");
                if (XmlNodeListObj333[0] != null)
                    myGelenEGEWSTEST.Sifre = "19cd21ebad3e08b8f1955b6461bd2f41";
                XmlNodeList XmlNodeListObj222 = dd.GetElementsByTagName("RefID");
                if (XmlNodeListObj222[0] != null)
                    myGelenEGEWSTEST.RefID = XmlNodeListObj222[0].ChildNodes[0].Value;


                XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("GUMRUK");
                if (XmlNodeListObj2[0] != null)
                    if (XmlNodeListObj2[0].ChildNodes[0] != null)
                        mynewtcgb.GUMRUK = XmlNodeListObj2[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj1 = dd.GetElementsByTagName("Rejim");
                if (XmlNodeListObj1[0] != null)
                    if (XmlNodeListObj1[0].ChildNodes[0] != null)
                        mynewtcgb.Rejim = XmlNodeListObj1[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj3 = dd.GetElementsByTagName("Basitlestirilmis_usul");
                if (XmlNodeListObj3[0] != null)
                    if (XmlNodeListObj3[0].ChildNodes[0] != null)
                        mynewtcgb.Basitlestirilmis_usul = XmlNodeListObj3[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj4 = dd.GetElementsByTagName("Yuk_belgeleri_sayisi");
                if (XmlNodeListObj4[0] != null)
                    mynewtcgb.Yuk_belgeleri_sayisi = Convert.ToInt32(XmlNodeListObj4[0].ChildNodes[0].Value);
                XmlNodeList XmlNodeListObj5 = dd.GetElementsByTagName("Kap_adedi");
                if (XmlNodeListObj5[0] != null)
                    mynewtcgb.Kap_adedi = Convert.ToInt16(XmlNodeListObj5[0].ChildNodes[0].Value);
                XmlNodeList XmlNodeListObj6 = dd.GetElementsByTagName("Ticaret_ulkesi");
                if (XmlNodeListObj6[0] != null)
                    if (XmlNodeListObj6[0].ChildNodes[0] != null)
                        mynewtcgb.Ticaret_ulkesi = XmlNodeListObj6[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj7 = dd.GetElementsByTagName("Referans_no");
                if (XmlNodeListObj7[0] != null)
                    if (XmlNodeListObj7[0].ChildNodes[0] != null)
                        mynewtcgb.Referans_no = XmlNodeListObj7[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj8 = dd.GetElementsByTagName("Cikis_ulkesi");
                if (XmlNodeListObj8[0] != null)
                    if (XmlNodeListObj8[0].ChildNodes[0] != null)
                        mynewtcgb.Cikis_ulkesi = XmlNodeListObj8[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj9 = dd.GetElementsByTagName("Gidecegi_ulke");
                if (XmlNodeListObj9[0] != null)
                    if (XmlNodeListObj9[0].ChildNodes[0] != null)
                        mynewtcgb.Gidecegi_ulke = XmlNodeListObj9[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj10 = dd.GetElementsByTagName("Gidecegi_sevk_ulkesi");
                if (XmlNodeListObj10[0] != null)
                    if (XmlNodeListObj10[0].ChildNodes[0] != null)
                        mynewtcgb.Gidecegi_sevk_ulkesi = XmlNodeListObj10[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj11 = dd.GetElementsByTagName("Cikistaki_aracin_tipi");
                if (XmlNodeListObj11[0] != null)
                    if (XmlNodeListObj11[0].ChildNodes[0] != null)
                        mynewtcgb.Cikistaki_aracin_tipi = XmlNodeListObj11[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj12 = dd.GetElementsByTagName("Cikistaki_aracin_kimligi");
                if (XmlNodeListObj12[0] != null)
                    if (XmlNodeListObj12[0].ChildNodes[0] != null)
                        mynewtcgb.Cikistaki_aracin_kimligi = XmlNodeListObj12[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj13 = dd.GetElementsByTagName("Cikistaki_aracin_ulkesi");
                if (XmlNodeListObj13[0] != null)
                    if (XmlNodeListObj13[0].ChildNodes[0] != null)
                        mynewtcgb.Cikistaki_aracin_ulkesi = XmlNodeListObj13[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj14 = dd.GetElementsByTagName("Teslim_sekli");
                if (XmlNodeListObj14[0] != null)
                    if (XmlNodeListObj14[0].ChildNodes[0] != null)
                        mynewtcgb.Teslim_sekli = XmlNodeListObj14[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj15 = dd.GetElementsByTagName("Teslim_yeri");
                if (XmlNodeListObj15[0] != null)
                    if (XmlNodeListObj15[0].ChildNodes[0] != null)
                        mynewtcgb.Teslim_yeri = XmlNodeListObj15[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj16 = dd.GetElementsByTagName("Konteyner");
                if (XmlNodeListObj16[0] != null)
                    if (XmlNodeListObj16[0].ChildNodes[0] != null)
                        mynewtcgb.Konteyner = XmlNodeListObj16[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj17 = dd.GetElementsByTagName("Sinirdaki_aracin_tipi");
                if (XmlNodeListObj17[0] != null)
                    if (XmlNodeListObj17[0].ChildNodes[0] != null)
                        mynewtcgb.Sinirdaki_aracin_tipi = XmlNodeListObj17[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj18 = dd.GetElementsByTagName("Sinirdaki_aracin_kimligi");
                if (XmlNodeListObj18[0] != null)
                    if (XmlNodeListObj18[0].ChildNodes[0] != null)
                        mynewtcgb.Sinirdaki_aracin_kimligi = XmlNodeListObj18[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj19 = dd.GetElementsByTagName("Sinirdaki_aracin_ulkesi");
                if (XmlNodeListObj19[0] != null)
                    if (XmlNodeListObj19[0].ChildNodes[0] != null)
                        mynewtcgb.Sinirdaki_aracin_ulkesi = XmlNodeListObj19[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj20 = dd.GetElementsByTagName("Toplam_fatura");
                if (XmlNodeListObj20[0] != null)
                    if (XmlNodeListObj20[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_fatura = Convert.ToDecimal(XmlNodeListObj20[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj21 = dd.GetElementsByTagName("Toplam_fatura_dovizi");
                if (XmlNodeListObj21[0] != null)
                    if (XmlNodeListObj21[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_fatura_dovizi = XmlNodeListObj21[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj22 = dd.GetElementsByTagName("Toplam_navlun");
                if (XmlNodeListObj22[0] != null)
                    if (XmlNodeListObj22[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_navlun = Convert.ToDecimal(XmlNodeListObj22[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj23 = dd.GetElementsByTagName("Toplan_navlun_dovizi");
                if (XmlNodeListObj23[0] != null)
                    if (XmlNodeListObj23[0].ChildNodes[0] != null)
                        mynewtcgb.Toplan_navlun_dovizi = XmlNodeListObj23[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj24 = dd.GetElementsByTagName("Sinirdaki_tasima_sekli");
                if (XmlNodeListObj24[0] != null)
                    if (XmlNodeListObj24[0].ChildNodes[0] != null)
                        mynewtcgb.Sinirdaki_tasima_sekli = XmlNodeListObj24[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj25 = dd.GetElementsByTagName("Alici_satici_iliskisi");
                if (XmlNodeListObj25[0] != null)
                    if (XmlNodeListObj25[0].ChildNodes[0] != null)
                        mynewtcgb.Alici_satici_iliskisi = XmlNodeListObj25[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj26 = dd.GetElementsByTagName("Toplam_sigorta");
                if (XmlNodeListObj26[0] != null)
                    if (XmlNodeListObj26[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_sigorta = Convert.ToDecimal(XmlNodeListObj26[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj27 = dd.GetElementsByTagName("Toplam_sigorta_dovizi");
                if (XmlNodeListObj27[0] != null)
                    if (XmlNodeListObj27[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_sigorta_dovizi = XmlNodeListObj27[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj28 = dd.GetElementsByTagName("Yukleme_bosaltma_yeri");
                if (XmlNodeListObj28[0] != null)
                    if (XmlNodeListObj28[0].ChildNodes[0] != null)
                        mynewtcgb.Yukleme_bosaltma_yeri = XmlNodeListObj28[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj29 = dd.GetElementsByTagName("Yukleme_bosaltma_yerinin_gumruk_idaresi");
                //if (XmlNodeListObj29[0] != null)
                //    if (XmlNodeListObj29[0].ChildNodes[0] != null)
                //        mynewtcgb.Yukleme_bosaltma_yerinin_gumruk_idaresi = XmlNodeListObj29[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj30 = dd.GetElementsByTagName("Toplam_yurt_disi_harcamalar");
                if (XmlNodeListObj30[0] != null)
                    if (XmlNodeListObj30[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_yurt_disi_harcamalar = Convert.ToDecimal(XmlNodeListObj30[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj31 = dd.GetElementsByTagName("Toplam_yurt_disi_harcamalarin_dovizi");
                if (XmlNodeListObj31[0] != null)
                    if (XmlNodeListObj31[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_yurt_disi_harcamalarin_dovizi = XmlNodeListObj31[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj32 = dd.GetElementsByTagName("Toplam_yurt_ici_harcamalar");
                if (XmlNodeListObj32[0] != null)
                    if (XmlNodeListObj32[0].ChildNodes[0] != null)
                        mynewtcgb.Toplam_yurt_ici_harcamalar = Convert.ToDecimal(XmlNodeListObj32[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj33 = dd.GetElementsByTagName("Odeme_sekli");
                //if (XmlNodeListObj33[0] != null)
                //    if (XmlNodeListObj33[0].ChildNodes[0] != null)
                //        mynewtcgb.Odeme_sekli = XmlNodeListObj33[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj34 = dd.GetElementsByTagName("Banka_kodu");
                if (XmlNodeListObj34[0] != null)
                    if (XmlNodeListObj34[0].ChildNodes[0] != null)
                        mynewtcgb.Banka_kodu = XmlNodeListObj34[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj35 = dd.GetElementsByTagName("Esyanin_bulundugu_yer");
                if (XmlNodeListObj35[0] != null)
                    if (XmlNodeListObj35[0].ChildNodes[0] != null)
                        mynewtcgb.Esyanin_bulundugu_yer = XmlNodeListObj35[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj36 = dd.GetElementsByTagName("Varis_gumruk_idaresi");
                if (XmlNodeListObj36[0] != null)
                    if (XmlNodeListObj36[0].ChildNodes[0] != null)
                        mynewtcgb.Varis_gumruk_idaresi = XmlNodeListObj36[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj37 = dd.GetElementsByTagName("Antrepo_kodu");
                if (XmlNodeListObj37[0] != null)
                    if (XmlNodeListObj37[0].ChildNodes[0] != null)
                        mynewtcgb.Antrepo_kodu = XmlNodeListObj37[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj38 = dd.GetElementsByTagName("Tasarlanan_guzergah");
                if (XmlNodeListObj38[0] != null)
                    if (XmlNodeListObj38[0].ChildNodes[0] != null)
                        mynewtcgb.Tasarlanan_guzergah = XmlNodeListObj38[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj39 = dd.GetElementsByTagName("Telafi_edici_vergi");
                if (XmlNodeListObj39[0] != null)
                    if (XmlNodeListObj39[0].ChildNodes[0] != null)
                        mynewtcgb.Telafi_edici_vergi = Convert.ToDecimal(XmlNodeListObj39[0].ChildNodes[0].Value, nfi1);
                XmlNodeList XmlNodeListObj40 = dd.GetElementsByTagName("Giris_gumruk_idaresi");
                if (XmlNodeListObj40[0] != null)
                    if (XmlNodeListObj40[0].ChildNodes[0] != null)
                        mynewtcgb.Giris_gumruk_idaresi = XmlNodeListObj40[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj41 = dd.GetElementsByTagName("Islemin_niteligi");
                if (XmlNodeListObj41[0] != null)
                    if (XmlNodeListObj41[0].ChildNodes[0] != null)
                        mynewtcgb.Islemin_niteligi = XmlNodeListObj41[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj42 = dd.GetElementsByTagName("Aciklamalar");
                if (XmlNodeListObj42[0] != null)
                    if (XmlNodeListObj42[0].ChildNodes[0] != null)
                        mynewtcgb.Aciklamalar = XmlNodeListObj42[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj43 = dd.GetElementsByTagName("Kullanici_kodu");
                if (XmlNodeListObj43[0] != null)
                    if (XmlNodeListObj43[0].ChildNodes[0] != null)
                        mynewtcgb.Kullanici_kodu = "15781158208";
                XmlNodeList XmlNodeListObj44 = dd.GetElementsByTagName("Referans_tarihi");
                if (XmlNodeListObj44[0] != null)
                    if (XmlNodeListObj44[0].ChildNodes[0] != null)
                        mynewtcgb.Referans_tarihi = XmlNodeListObj44[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj45 = dd.GetElementsByTagName("Odeme");
                if (XmlNodeListObj45[0] != null)
                    if (XmlNodeListObj45[0].ChildNodes[0] != null)
                        mynewtcgb.Odeme = XmlNodeListObj45[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj46 = dd.GetElementsByTagName("Odeme_araci");
                if (XmlNodeListObj46[0] != null)
                    if (XmlNodeListObj46[0].ChildNodes[0] != null)
                        mynewtcgb.Odeme_araci = XmlNodeListObj46[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj47 = dd.GetElementsByTagName("Gonderici_vergi_no");
                if (XmlNodeListObj47[0] != null)
                    if (XmlNodeListObj47[0].ChildNodes[0] != null)
                        mynewtcgb.Gonderici_vergi_no = XmlNodeListObj47[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj48 = dd.GetElementsByTagName("Alici_vergi_no");
                if (XmlNodeListObj48[0] != null)
                    if (XmlNodeListObj48[0].ChildNodes[0] != null)
                        mynewtcgb.Alici_vergi_no = XmlNodeListObj48[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj49 = dd.GetElementsByTagName("Beyan_sahibi_vergi_no");
                if (XmlNodeListObj49[0] != null)
                    if (XmlNodeListObj49[0].ChildNodes[0] != null)
                        mynewtcgb.Beyan_sahibi_vergi_no = XmlNodeListObj49[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj50 = dd.GetElementsByTagName("Musavir_vergi_no");
                if (XmlNodeListObj50[0] != null)
                    if (XmlNodeListObj50[0].ChildNodes[0] != null)
                        mynewtcgb.Musavir_vergi_no = XmlNodeListObj50[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj51 = dd.GetElementsByTagName("Asil_sorumlu_vergi_no");
                if (XmlNodeListObj51[0] != null)
                    if (XmlNodeListObj51[0].ChildNodes[0] != null)
                        mynewtcgb.Asil_sorumlu_vergi_no = XmlNodeListObj51[0].ChildNodes[0].Value;

                XmlNodeList XmlNodeListObj90 = dd.GetElementsByTagName("Musavir_referansi");

                if (XmlNodeListObj90[0] != null)
                    if (XmlNodeListObj90[0].ChildNodes[0] != null)
                        mynewtcgb.Musavir_referansi = XmlNodeListObj90[0].ChildNodes[0].Value;

                XmlNodeList XmlNodeListObj91 = dd.GetElementsByTagName("mail1");
                if (XmlNodeListObj91[0] != null)
                    if (XmlNodeListObj91[0].ChildNodes[0] != null)
                        mynewtcgb.mail1 = XmlNodeListObj91[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj92 = dd.GetElementsByTagName("mail2");
                if (XmlNodeListObj92[0] != null)
                    if (XmlNodeListObj92[0].ChildNodes[0] != null)
                        mynewtcgb.mail2 = XmlNodeListObj92[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj93 = dd.GetElementsByTagName("mail3");
                if (XmlNodeListObj93[0] != null)
                    if (XmlNodeListObj93[0].ChildNodes[0] != null)
                        mynewtcgb.mail3 = XmlNodeListObj93[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj94 = dd.GetElementsByTagName("mobil1");
                if (XmlNodeListObj94[0] != null)
                    if (XmlNodeListObj94[0].ChildNodes[0] != null)
                        mynewtcgb.mobil1 = XmlNodeListObj94[0].ChildNodes[0].Value;
                XmlNodeList XmlNodeListObj95 = dd.GetElementsByTagName("mobil2");
                if (XmlNodeListObj95[0] != null)
                    if (XmlNodeListObj95[0].ChildNodes[0] != null)
                        mynewtcgb.mobil2 = XmlNodeListObj95[0].ChildNodes[0].Value;


                XmlNodeList XmlNodeListKiymetObj1 = dd.GetElementsByTagName("KiymetBildirim");
                if (XmlNodeListKiymetObj1[0].ChildNodes.Count > 0)
                {
                    mynewtcgb.KiymetBildirim = new KontrolHizmeti.Kiymet[XmlNodeListKiymetObj1[0].ChildNodes.Count];

                    XmlNodeList xKiymet = XmlNodeListKiymetObj1[0].ChildNodes;

                    for (int i = 0; i < xKiymet.Count; i++)
                    {
                        mynewtcgb.KiymetBildirim[i] = new KontrolHizmeti.Kiymet();

                        if (xKiymet[i]["TeslimSekli"] != null)
                            mynewtcgb.KiymetBildirim[i].TeslimSekli = xKiymet[i]["TeslimSekli"].InnerText;
                        if (xKiymet[i]["FaturaTarihiSayisi"] != null)
                            mynewtcgb.KiymetBildirim[i].FaturaTarihiSayisi = xKiymet[i]["FaturaTarihiSayisi"].InnerText;
                        if (xKiymet[i]["SozlesmeTarihiSayisi"] != null)
                            mynewtcgb.KiymetBildirim[i].SozlesmeTarihiSayisi = xKiymet[i]["SozlesmeTarihiSayisi"].InnerText;
                        if (xKiymet[i]["GumrukIdaresiKarari"] != null)
                            mynewtcgb.KiymetBildirim[i].GumrukIdaresiKarari = xKiymet[i]["GumrukIdaresiKarari"].InnerText;
                        if (xKiymet[i]["AliciSatici"] != null)
                            mynewtcgb.KiymetBildirim[i].AliciSatici = xKiymet[i]["AliciSatici"].InnerText;
                        if (xKiymet[i]["AliciSaticiAyrintilar"] != null)
                            mynewtcgb.KiymetBildirim[i].AliciSaticiAyrintilar = xKiymet[i]["AliciSaticiAyrintilar"].InnerText;
                        if (xKiymet[i]["Kisitlamalar"] != null)
                            mynewtcgb.KiymetBildirim[i].Kisitlamalar = xKiymet[i]["Kisitlamalar"].InnerText;
                        if (xKiymet[i]["Edim"] != null)
                            mynewtcgb.KiymetBildirim[i].Edim = xKiymet[i]["Edim"].InnerText;
                        if (xKiymet[i]["Munasebet"] != null)
                            mynewtcgb.KiymetBildirim[i].Munasebet = xKiymet[i]["Munasebet"].InnerText;
                        if (xKiymet[i]["KisitlamalarAyrintilar"] != null)
                            mynewtcgb.KiymetBildirim[i].KisitlamalarAyrintilar = xKiymet[i]["KisitlamalarAyrintilar"].InnerText;
                        if (xKiymet[i]["Royalti"] != null)
                            mynewtcgb.KiymetBildirim[i].Royalti = xKiymet[i]["Royalti"].InnerText;
                        if (xKiymet[i]["RoyaltiKosullar"] != null)
                            mynewtcgb.KiymetBildirim[i].RoyaltiKosullar = xKiymet[i]["RoyaltiKosullar"].InnerText;
                        if (xKiymet[i]["SaticiyaIntikal"] != null)
                            mynewtcgb.KiymetBildirim[i].SaticiyaIntikal = xKiymet[i]["SaticiyaIntikal"].InnerText;
                        if (xKiymet[i]["SaticiyaIntikalKosullar"] != null)
                            mynewtcgb.KiymetBildirim[i].SaticiyaIntikalKosullar = xKiymet[i]["SaticiyaIntikalKosullar"].InnerText;
                        if (xKiymet[i]["SehirYer"] != null)
                            mynewtcgb.KiymetBildirim[i].SehirYer = xKiymet[i]["SehirYer"].InnerText;
                        if (xKiymet[i]["Taahutname"] != null)
                            mynewtcgb.KiymetBildirim[i].Taahutname = xKiymet[i]["Taahutname"].InnerText;


                        XmlNode KiymetKalem = xKiymet[i]["KiymetKalemler"];


                        if (KiymetKalem != null)
                        {

                            mynewtcgb.KiymetBildirim[i].KiymetKalemler = new KontrolHizmeti.KiymetKalem[KiymetKalem.ChildNodes.Count];


                            for (int j = 0; j < KiymetKalem.ChildNodes.Count; j++)
                            {
                                XmlNode xKKbilgi = KiymetKalem.ChildNodes[j];
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j] = new KontrolHizmeti.KiymetKalem();
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].KiymetKalemNo = Convert.ToInt16(xKKbilgi["KiymetKalemNo"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].BeyannameKalemNo = Convert.ToInt16(xKKbilgi["BeyannameKalemNo"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].DolayliOdeme = Convert.ToDecimal(xKKbilgi["DolayliOdeme"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].Komisyon = Convert.ToDecimal(xKKbilgi["Komisyon"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].Tellaliye = Convert.ToDecimal(xKKbilgi["Tellaliye"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].KapAmbalajBedeli = Convert.ToDecimal(xKKbilgi["KapAmbalajBedeli"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].IthalaKatilanMalzeme = Convert.ToDecimal(xKKbilgi["IthalaKatilanMalzeme"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].IthalaUretimAraclar = Convert.ToDecimal(xKKbilgi["IthalaUretimAraclar"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].IthalaUretimTuketimMalzemesi = Convert.ToDecimal(xKKbilgi["IthalaUretimTuketimMalzemesi"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].PlanTaslak = Convert.ToDecimal(xKKbilgi["PlanTaslak"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].RoyaltiLisans = Convert.ToDecimal(xKKbilgi["RoyaltiLisans"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].DolayliIntikal = Convert.ToDecimal(xKKbilgi["DolayliIntikal"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].Nakliye = Convert.ToDecimal(xKKbilgi["Nakliye"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].Sigorta = Convert.ToDecimal(xKKbilgi["Sigorta"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].GirisSonrasiNakliye = Convert.ToDecimal(xKKbilgi["GirisSonrasiNakliye"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].TeknikYardim = Convert.ToDecimal(xKKbilgi["TeknikYardim"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].DigerOdemeler = Convert.ToDecimal(xKKbilgi["DigerOdemeler"].InnerText);
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].DigerOdemelerNiteligi = xKKbilgi["DigerOdemelerNiteligi"].InnerText;
                                mynewtcgb.KiymetBildirim[i].KiymetKalemler[j].VergiHarcFon = Convert.ToDecimal(xKKbilgi["VergiHarcFon"].InnerText);

                            }

                        }
                        else
                        {
                            mynewtcgb.KiymetBildirim[i].KiymetKalemler = new KontrolHizmeti.KiymetKalem[0];

                        }


                    }
                }
                //Kalemler....

                XmlNodeList XmlNodeListKalemObj1 = dd.GetElementsByTagName("Kalemler");
                if (XmlNodeListKalemObj1[0].ChildNodes.Count > 0)
                {
                    mynewtcgb.Kalemler = new KontrolHizmeti.kalem[XmlNodeListKalemObj1[0].ChildNodes.Count];

                    XmlNodeList xKalem = XmlNodeListKalemObj1[0].ChildNodes;

                    for (int i = 0; i < xKalem.Count; i++)
                    {

                        mynewtcgb.Kalemler[i] = new KontrolHizmeti.kalem();
                        if (xKalem[i]["Gtip"] != null)
                            mynewtcgb.Kalemler[i].Gtip = xKalem[i]["Gtip"].InnerText;

                        if (xKalem[i]["Imalatci_firma_bilgisi"] != null)
                            mynewtcgb.Kalemler[i].Imalatci_firma_bilgisi = xKalem[i]["Imalatci_firma_bilgisi"].InnerText;

                        if (xKalem[i]["Kalem_sira_no"] != null)
                            mynewtcgb.Kalemler[i].Kalem_sira_no = Convert.ToInt32(xKalem[i]["Kalem_sira_no"].InnerText);

                        if (xKalem[i]["Mensei_ulke"] != null)
                            mynewtcgb.Kalemler[i].Mensei_ulke = xKalem[i]["Mensei_ulke"].InnerText;

                        if (xKalem[i]["Brut_agirlik"] != null)
                            mynewtcgb.Kalemler[i].Brut_agirlik = Convert.ToDecimal(xKalem[i]["Brut_agirlik"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Brut_agirlik = 0;

                        if (xKalem[i]["Net_agirlik"] != null)
                            mynewtcgb.Kalemler[i].Net_agirlik = Convert.ToDecimal(xKalem[i]["Net_agirlik"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Net_agirlik = 0;

                        if (xKalem[i]["Tamamlayici_olcu_birimi"] != null)
                            mynewtcgb.Kalemler[i].Tamamlayici_olcu_birimi = xKalem[i]["Tamamlayici_olcu_birimi"].InnerText;

                        if (xKalem[i]["Istatistiki_miktar"] != null)
                            mynewtcgb.Kalemler[i].Istatistiki_miktar = Convert.ToDecimal(xKalem[i]["Istatistiki_miktar"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Istatistiki_miktar = 0;

                        if (xKalem[i]["Uluslararasi_anlasma"] != null)
                            mynewtcgb.Kalemler[i].Uluslararasi_anlasma = xKalem[i]["Uluslararasi_anlasma"].InnerText;

                        if (xKalem[i]["Algilama_birimi_1"] != null)
                            mynewtcgb.Kalemler[i].Algilama_birimi_1 = xKalem[i]["Algilama_birimi_1"].InnerText;

                        if (xKalem[i]["Algilama_miktari_1"] != null)
                            mynewtcgb.Kalemler[i].Algilama_miktari_1 = Convert.ToDecimal(xKalem[i]["Algilama_miktari_1"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Algilama_miktari_1 = 0;

                        if (xKalem[i]["Algilama_birimi_2"] != null)
                            mynewtcgb.Kalemler[i].Algilama_birimi_2 = xKalem[i]["Algilama_birimi_2"].InnerText;

                        if (xKalem[i]["Algilama_miktari_2"] != null)
                            mynewtcgb.Kalemler[i].Algilama_miktari_2 = Convert.ToDecimal(xKalem[i]["Algilama_miktari_2"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Algilama_miktari_2 = 0;

                        if (xKalem[i]["Muafiyetler_1"] != null)
                            mynewtcgb.Kalemler[i].Muafiyetler_1 = xKalem[i]["Muafiyetler_1"].InnerText;

                        if (xKalem[i]["Muafiyetler_2"] != null)
                            mynewtcgb.Kalemler[i].Muafiyetler_2 = xKalem[i]["Muafiyetler_2"].InnerText;

                        if (xKalem[i]["Muafiyetler_3"] != null)
                            mynewtcgb.Kalemler[i].Muafiyetler_3 = xKalem[i]["Muafiyetler_3"].InnerText;

                        if (xKalem[i]["Algilama_miktari_3"] != null)
                            mynewtcgb.Kalemler[i].Algilama_miktari_3 = Convert.ToDecimal(xKalem[i]["Algilama_miktari_3"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Algilama_miktari_3 = 0;

                        if (xKalem[i]["Algilama_birimi_3"] != null)
                            mynewtcgb.Kalemler[i].Algilama_birimi_3 = xKalem[i]["Algilama_birimi_3"].InnerText;

                        if (xKalem[i]["Teslim_sekli"] != null)
                            mynewtcgb.Kalemler[i].Teslim_sekli = xKalem[i]["Teslim_sekli"].InnerText;

                        if (xKalem[i]["Ek_kod"] != null)
                            mynewtcgb.Kalemler[i].Ek_kod = xKalem[i]["Ek_kod"].InnerText;

                        if (xKalem[i]["Ozellik"] != null)
                            mynewtcgb.Kalemler[i].Ozellik = xKalem[i]["Ozellik"].InnerText;

                        if (xKalem[i]["Fatura_miktari"] != null)
                            mynewtcgb.Kalemler[i].Fatura_miktari = Convert.ToDecimal(xKalem[i]["Fatura_miktari"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Fatura_miktari = 0;

                        if (xKalem[i]["Fatura_miktarinin_dovizi"] != null)
                            mynewtcgb.Kalemler[i].Fatura_miktarinin_dovizi = xKalem[i]["Fatura_miktarinin_dovizi"].InnerText;

                        if (xKalem[i]["Sinir_gecis_ucreti"] != null)
                            mynewtcgb.Kalemler[i].Sinir_gecis_ucreti = Convert.ToDecimal(xKalem[i]["Sinir_gecis_ucreti"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Sinir_gecis_ucreti = 0;

                        if (xKalem[i]["Navlun_miktari"] != null)
                            mynewtcgb.Kalemler[i].Navlun_miktari = Convert.ToDecimal(xKalem[i]["Navlun_miktari"].InnerText, nfi1);
                        //  else mynewtcgb.Kalemler[i].Navlun_miktari = 0;

                        if (xKalem[i]["Navlun_miktarinin_dovizi"] != null)
                            mynewtcgb.Kalemler[i].Navlun_miktarinin_dovizi = xKalem[i]["Navlun_miktarinin_dovizi"].InnerText;

                        if (xKalem[i]["Istatistiki_kiymet"] != null)
                            mynewtcgb.Kalemler[i].Istatistiki_kiymet = Convert.ToDecimal(xKalem[i]["Istatistiki_kiymet"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Istatistiki_kiymet = 0;

                        if (xKalem[i]["Sigorta_miktari"] != null)
                            mynewtcgb.Kalemler[i].Sigorta_miktari = Convert.ToDecimal(xKalem[i]["Sigorta_miktari"].InnerText, nfi1);
                        //  else mynewtcgb.Kalemler[i].Sigorta_miktari = 0;

                        if (xKalem[i]["Sigorta_miktarinin_dovizi"] != null)
                            mynewtcgb.Kalemler[i].Sigorta_miktarinin_dovizi = xKalem[i]["Sigorta_miktarinin_dovizi"].InnerText;

                        //if (xKalem[i]["Yurt_disi_harcamalar_miktari"] != null)
                        //    mynewtcgb.Kalemler[i].Yurt_disi_harcamalar_miktari = Convert.ToDecimal(xKalem[i]["Yurt_disi_harcamalar_miktari"].InnerText, nfi1);
                        ////  else mynewtcgb.Kalemler[i].Yurt_disi_harcamalar_miktari = 0;

                        //if (xKalem[i]["Yurt_disi_harcamalar_miktarinin_dovizi"] != null)
                        //    mynewtcgb.Kalemler[i].Yurt_disi_harcamalar_miktarinin_dovizi = xKalem[i]["Yurt_disi_harcamalar_miktarinin_dovizi"].InnerText;

                        //if (xKalem[i]["Yurt_ici_harcamalar"] != null)
                        //    mynewtcgb.Kalemler[i].Yurt_ici_harcamalar = Convert.ToDecimal(xKalem[i]["Yurt_ici_harcamalar"].InnerText, nfi1);
                        //  else mynewtcgb.Kalemler[i].Yurt_ici_harcamalar = 0;

                        //if (xKalem[i]["Yurt_ici_harcamalar_dovizi"] != null)
                        //    mynewtcgb.Kalemler[i].Yurt_ici_harcamalar_dovizi = xKalem[i]["Yurt_ici_harcamalar_dovizi"].InnerText;

                        if (xKalem[i]["Tarifedeki_tanimi"] != null)
                            mynewtcgb.Kalemler[i].Tarifedeki_tanimi = xKalem[i]["Tarifedeki_tanimi"].InnerText;

                        if (xKalem[i]["Ticari_tanimi"] != null)
                            mynewtcgb.Kalemler[i].Ticari_tanimi = xKalem[i]["Ticari_tanimi"].InnerText;

                        if (xKalem[i]["Marka"] != null)
                            mynewtcgb.Kalemler[i].Marka = xKalem[i]["Marka"].InnerText;

                        if (xKalem[i]["Numara"] != null)
                            mynewtcgb.Kalemler[i].Numara = xKalem[i]["Numara"].InnerText;

                        if (xKalem[i]["Cinsi"] != null)
                            mynewtcgb.Kalemler[i].Cinsi = xKalem[i]["Cinsi"].InnerText;

                        if (xKalem[i]["Adedi"] != null)
                            mynewtcgb.Kalemler[i].Adedi = Convert.ToDecimal(xKalem[i]["Adedi"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Adedi = 0;


                        if (xKalem[i]["Miktar_birimi"] != null)
                            mynewtcgb.Kalemler[i].Miktar_birimi = xKalem[i]["Miktar_birimi"].InnerText;

                        if (xKalem[i]["Mahrece_iade"] != null)
                            mynewtcgb.Kalemler[i].Mahrece_iade = xKalem[i]["Mahrece_iade"].InnerText;

                        if (xKalem[i]["Ikincil_islem"] != null)
                            mynewtcgb.Kalemler[i].Ikincil_islem = xKalem[i]["Ikincil_islem"].InnerText;

                        if (xKalem[i]["Satir_no"] != null)
                            mynewtcgb.Kalemler[i].Satir_no = xKalem[i]["Satir_no"].InnerText;

                        if (xKalem[i]["Miktar"] != null)
                            mynewtcgb.Kalemler[i].Miktar = Convert.ToDecimal(xKalem[i]["Miktar"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Miktar = 0;

                        if (xKalem[i]["Kdv_orani"] != null)
                            mynewtcgb.Kalemler[i].Kdv_orani = xKalem[i]["Kdv_orani"].InnerText;

                        if (xKalem[i]["Kullanilmis_esya"] != null)
                            mynewtcgb.Kalemler[i].Kullanilmis_esya = xKalem[i]["Kullanilmis_esya"].InnerText;

                        if (xKalem[i]["Yurtici_Banka"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Banka = Convert.ToDecimal(xKalem[i]["Yurtici_Banka"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Banka = 0;

                        if (xKalem[i]["Yurtici_Kkdf"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Kkdf = Convert.ToDecimal(xKalem[i]["Yurtici_Kkdf"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Kkdf = 0;

                        if (xKalem[i]["Yurtici_Liman"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Liman = Convert.ToDecimal(xKalem[i]["Yurtici_Liman"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Liman = 0;

                        if (xKalem[i]["Yurtici_Tahliye"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Tahliye = Convert.ToDecimal(xKalem[i]["Yurtici_Tahliye"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Tahliye = 0;

                        if (xKalem[i]["Yurtici_Depolama"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Depolama = Convert.ToDecimal(xKalem[i]["Yurtici_Depolama"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Depolama = 0;

                        if (xKalem[i]["Yurtici_Kultur"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Kultur = Convert.ToDecimal(xKalem[i]["Yurtici_Kultur"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Kultur = 0;

                        if (xKalem[i]["Yurtici_Cevre"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Cevre = Convert.ToDecimal(xKalem[i]["Yurtici_Cevre"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Cevre = 0;

                        if (xKalem[i]["Yurtici_Diger"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Diger = Convert.ToDecimal(xKalem[i]["Yurtici_Diger"].InnerText, nfi1);
                        else mynewtcgb.Kalemler[i].Yurtici_Diger = 0;

                        if (xKalem[i]["Yurtici_Diger_Aciklama"] != null)
                            mynewtcgb.Kalemler[i].Yurtici_Diger_Aciklama = xKalem[i]["Yurtici_Diger_Aciklama"].InnerText;
                        else mynewtcgb.Kalemler[i].Yurtici_Diger_Aciklama = "";

                        if (xKalem[i]["Muafiyet_Aciklamasi"] != null)
                            mynewtcgb.Kalemler[i].Muafiyet_Aciklamasi = xKalem[i]["Muafiyet_Aciklamasi"].InnerText;
                        else mynewtcgb.Kalemler[i].Muafiyet_Aciklamasi = "";


                        if (xKalem[i]["Referans_Tarihi"] != null)
                            mynewtcgb.Kalemler[i].Referans_Tarihi = xKalem[i]["Referans_Tarihi"].InnerText;
                        else mynewtcgb.Kalemler[i].Referans_Tarihi = "";


                        //if (xKalem[i]["YurtDisi_Banka"] != null)
                        //    mynewtcgb.Kalemler[i].YurtDisi_Banka = Convert.ToDecimal(xKalem[i]["YurtDisi_Banka"].InnerText);
                        //else mynewtcgb.Kalemler[i].YurtDisi_Banka = 0;
                        //if (xKalem[i]["YurtDisi_Banka_Dovizi"] != null)
                        //    mynewtcgb.Kalemler[i].YurtDisi_Banka_Dovizi = xKalem[i]["YurtDisi_Banka_Dovizi"].InnerText;
                        //else mynewtcgb.Kalemler[i].YurtDisi_Banka_Dovizi = "";
                        //if (xKalem[i]["YurtDisi_Depolama"] != null)
                        //    mynewtcgb.Kalemler[i].YurtDisi_Depolama = Convert.ToDecimal(xKalem[i]["YurtDisi_Depolama"].InnerText);
                        //else mynewtcgb.Kalemler[i].YurtDisi_Depolama = 0;
                        //if (xKalem[i]["YurtDisi_Depolama_Dovizi"] != null)
                        //    mynewtcgb.Kalemler[i].YurtDisi_Depolama_Dovizi = xKalem[i]["YurtDisi_Depolama_Dovizi"].InnerText;
                        //else mynewtcgb.Kalemler[i].YurtDisi_Depolama_Dovizi = "";
                        if (xKalem[i]["YurtDisi_Diger"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Diger = Convert.ToDecimal(xKalem[i]["YurtDisi_Diger"].InnerText);
                        else mynewtcgb.Kalemler[i].YurtDisi_Diger = 0;
                        if (xKalem[i]["YurtDisi_Diger_Dovizi"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Diger_Dovizi = xKalem[i]["YurtDisi_Diger_Dovizi"].InnerText;
                        else mynewtcgb.Kalemler[i].YurtDisi_Diger_Dovizi = "";
                        if (xKalem[i]["YurtDisi_Diger_Aciklama"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Diger_Aciklama = xKalem[i]["YurtDisi_Diger_Aciklama"].InnerText;
                        else mynewtcgb.Kalemler[i].YurtDisi_Diger_Aciklama = "";

                        if (xKalem[i]["YurtDisi_Komisyon"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Komisyon = Convert.ToDecimal(xKalem[i]["YurtDisi_Komisyon"].InnerText);
                        else mynewtcgb.Kalemler[i].YurtDisi_Komisyon = 0;
                        if (xKalem[i]["YurtDisi_Komisyon_Dovizi"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Komisyon_Dovizi = xKalem[i]["YurtDisi_Komisyon_Dovizi"].InnerText;
                        else mynewtcgb.Kalemler[i].YurtDisi_Komisyon_Dovizi = "";

                        if (xKalem[i]["YurtDisi_Royalti"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Royalti = Convert.ToDecimal(xKalem[i]["YurtDisi_Royalti"].InnerText);
                        else mynewtcgb.Kalemler[i].YurtDisi_Royalti = 0;
                        if (xKalem[i]["YurtDisi_Royalti_Dovizi"] != null)
                            mynewtcgb.Kalemler[i].YurtDisi_Royalti_Dovizi = xKalem[i]["YurtDisi_Royalti_Dovizi"].InnerText;
                        else mynewtcgb.Kalemler[i].YurtDisi_Royalti_Dovizi = "";
                        if (xKalem[i]["Giris_Cikis_Amaci"] != null)
                            mynewtcgb.Kalemler[i].Giris_Cikis_Amaci = xKalem[i]["Giris_Cikis_Amaci"].InnerText;
                        else mynewtcgb.Kalemler[i].Giris_Cikis_Amaci = "";
                        if (xKalem[i]["Giris_Cikis_Amaci_Aciklama"] != null)
                            mynewtcgb.Kalemler[i].Giris_Cikis_Amaci_Aciklama = xKalem[i]["Giris_Cikis_Amaci_Aciklama"].InnerText;
                        else mynewtcgb.Kalemler[i].Giris_Cikis_Amaci_Aciklama = "";
                        if (xKalem[i]["Kalem_Islem_Niteligi"] != null)
                            mynewtcgb.Kalemler[i].Kalem_Islem_Niteligi = xKalem[i]["Kalem_Islem_Niteligi"].InnerText;
                        else mynewtcgb.Kalemler[i].Kalem_Islem_Niteligi = "";
                        if (xKalem[i]["STM_IlKodu"] != null)
                            mynewtcgb.Kalemler[i].STM_IlKodu = xKalem[i]["STM_IlKodu"].InnerText;
                        else mynewtcgb.Kalemler[i].STM_IlKodu = "";




                        XmlNode tamamlayici = xKalem[i]["tamamlayici_bilgi"];
                        XmlNode acmalar = xKalem[i]["tcgbacmakapatma_bilgi"];
                        XmlNode marka = xKalem[i]["marka_model_bilgi"];
                        XmlNode vergiMuafiyet = xKalem[i]["VergiMuafiyetleri"];
                        XmlNode havaAkaryakit = xKalem[i]["HavacilikYakitTurleri"];


                        if (vergiMuafiyet != null)
                        {

                            mynewtcgb.Kalemler[i].VergiMuafiyetleri = new KontrolHizmeti.VergiMuafiyeti[vergiMuafiyet.ChildNodes.Count];


                            for (int j = 0; j < vergiMuafiyet.ChildNodes.Count; j++)
                            {
                                XmlNode xTbilgi = vergiMuafiyet.ChildNodes[j];
                                mynewtcgb.Kalemler[i].VergiMuafiyetleri[j] = new KontrolHizmeti.VergiMuafiyeti();
                                mynewtcgb.Kalemler[i].VergiMuafiyetleri[j].VergiMuafiyetKodu = xTbilgi["VergiMuafiyetKodu"].InnerText;

                            }

                        }
                        else
                        {
                            mynewtcgb.Kalemler[i].VergiMuafiyetleri = new KontrolHizmeti.VergiMuafiyeti[0];
                        }


                        //if (havaAkaryakit != null)
                        //{

                        //    mynewtcgb.Kalemler[i].HavacilikYakitTurleri = new KontrolHizmeti.HavacilikYakitTuru[havaAkaryakit.ChildNodes.Count];


                        //    for (int j = 0; j < havaAkaryakit.ChildNodes.Count; j++)
                        //    {
                        //        XmlNode xTbilgi = havaAkaryakit.ChildNodes[j];
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j] = new KontrolHizmeti.HavacilikYakitTuru();
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].FaturaNumarasi = xTbilgi["FaturaNumarası"].InnerText;
                        //        //mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].FaturaTuru = xTbilgi["FaturaTuru"].InnerText;
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].TicariUnvan = xTbilgi["TicariUnvan"].InnerText;
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].ToplamFaturaMiktar = Convert.ToDecimal(xTbilgi["ToplamFaturaMiktar"].InnerText);
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].VergiNumarasi = xTbilgi["VergiNumarasi"].InnerText;
                        //        mynewtcgb.Kalemler[i].HavacilikYakitTurleri[j].YakitTuru = xTbilgi["YakitTuru"].InnerText;
                        //    }

                        //}
                        //else
                        //{
                        //    mynewtcgb.Kalemler[i].VergiMuafiyetleri = new KontrolHizmeti.VergiMuafiyeti[0];
                        //}





                        if (tamamlayici != null)
                        {

                            mynewtcgb.Kalemler[i].tamamlayici_bilgi = new KontrolHizmeti.tamamlayici[tamamlayici.ChildNodes.Count];


                            for (int j = 0; j < tamamlayici.ChildNodes.Count; j++)
                            {
                                XmlNode xTbilgi = tamamlayici.ChildNodes[j];
                                mynewtcgb.Kalemler[i].tamamlayici_bilgi[j] = new KontrolHizmeti.tamamlayici();
                                mynewtcgb.Kalemler[i].tamamlayici_bilgi[j].Tamamlayici_bilgi = xTbilgi["Tamamlayici_bilgi"].InnerText;
                                mynewtcgb.Kalemler[i].tamamlayici_bilgi[j].Tamamlayici_bilgi_orani = xTbilgi["Tamamlayici_bilgi_orani"].InnerText;
                            }

                        }
                        else
                        {
                            mynewtcgb.Kalemler[i].tamamlayici_bilgi = new KontrolHizmeti.tamamlayici[0];

                        }


                        if (acmalar != null)
                        {

                            mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi = new KontrolHizmeti.tcgbacmakapatma[acmalar.ChildNodes.Count];


                            for (int j = 0; j < acmalar.ChildNodes.Count; j++)
                            {

                                XmlNode xAcma = acmalar.ChildNodes[j];
                                mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi[j] = new KontrolHizmeti.tcgbacmakapatma();
                                mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi[j].Kapatilan_beyanname_no = xAcma["Kapatilan_beyanname_no"].InnerText;
                                mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi[j].Kapatilan_kalem_no = Convert.ToInt32(xAcma["Kapatilan_kalem_no"].InnerText);
                                mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi[j].Kapatilan_miktar = Convert.ToDecimal(xAcma["Kapatilan_miktar"].InnerText, nfi1);
                            }

                        }
                        else
                        {
                            mynewtcgb.Kalemler[i].tcgbacmakapatma_bilgi = new KontrolHizmeti.tcgbacmakapatma[0];

                        }
                        //if (marka != null)
                        //{

                        //    mynewtcgb.Kalemler[i].marka_model_bilgi = new KontrolTest.Marka[marka.ChildNodes.Count];


                        //    for (int j = 0; j < marka.ChildNodes.Count; j++)
                        //    {

                        //        XmlNode xmarka = marka.ChildNodes[j];
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j] = new KontrolTest.Marka();
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j].Marka_Turu = xmarka["Marka_Turu"].InnerText;
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j].Marka_Tescil_No = xmarka["Marka_Tescil_No"].InnerText;
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j].Marka_Adi = xmarka["Marka_Adi"].InnerText;
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j].Marka_Adi = xmarka["Referans_No"].InnerText;
                        //        mynewtcgb.Kalemler[i].marka_model_bilgi[j].Marka_Kiymeti = Convert.ToDecimal(xmarka["Marka_Kiymeti"].InnerText, nfi1);
                        //    }

                        //}
                        //else
                        //{
                        //    mynewtcgb.Kalemler[i].marka_model_bilgi = new KontrolTest.Marka[0];

                        //}

                    }



                }

                // Özet Beyanlar

                XmlNodeList XmlNodeListOzbyObj1 = dd.GetElementsByTagName("Ozetbeyanlar");
                if (XmlNodeListOzbyObj1[0] != null)
                {

                    if (XmlNodeListOzbyObj1[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xOzby = XmlNodeListOzbyObj1[0].ChildNodes;
                        mynewtcgb.Ozetbeyanlar = new KontrolHizmeti.Ozetbeyan[XmlNodeListOzbyObj1[0].ChildNodes.Count];

                        for (int j = 0; j < xOzby.Count; j++)
                        {
                            mynewtcgb.Ozetbeyanlar[j] = new KontrolHizmeti.Ozetbeyan();
                            mynewtcgb.Ozetbeyanlar[j].Ozetbeyan_no = xOzby[j]["Ozetbeyan_no"].InnerText;
                            mynewtcgb.Ozetbeyanlar[j].Ozetbeyan_islem_kapsami = xOzby[j]["Ozetbeyan_islem_kapsami"].InnerText;
                            mynewtcgb.Ozetbeyanlar[j].Ambar_ici = xOzby[j]["Ambar_ici"].InnerText;
                            mynewtcgb.Ozetbeyanlar[j].Baska_rejim = xOzby[j]["Baska_rejim"].InnerText;
                            if (xOzby[j]["Aciklama"] != null)
                                mynewtcgb.Ozetbeyanlar[j].Aciklama = xOzby[j]["Aciklama"].InnerText;


                            XmlNode tasimasenetleri = xOzby[j]["ozbyacma_bilgi"];
                            if (tasimasenetleri != null)
                                if (tasimasenetleri.ChildNodes.Count > 0)
                                {
                                    mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi = new KontrolHizmeti.tasimasenetleri[tasimasenetleri.ChildNodes.Count];
                                    for (int i = 0; i < tasimasenetleri.ChildNodes.Count; i++)
                                    {
                                        XmlNode xTsenet = tasimasenetleri.ChildNodes[i];
                                        mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i] = new KontrolHizmeti.tasimasenetleri();
                                        mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].Tasima_senedi_no = xTsenet["Tasima_senedi_no"].InnerText;

                                        XmlNode tasimasatirlari = xTsenet["tasimasatir_bilgi"];
                                        if (tasimasatirlari != null)
                                            if (tasimasatirlari.ChildNodes.Count > 0)
                                            {
                                                mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].tasimasatir_bilgi = new KontrolHizmeti.tasimasatirlari[tasimasatirlari.ChildNodes.Count];
                                                for (int k = 0; k < tasimasatirlari.ChildNodes.Count; k++)
                                                {
                                                    XmlNode xTsatir = tasimasatirlari.ChildNodes[k];
                                                    mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].tasimasatir_bilgi[k] = new KontrolHizmeti.tasimasatirlari();
                                                    mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].tasimasatir_bilgi[k].Tasima_satir_no = xTsatir["Tasima_satir_no"].InnerText;
                                                    mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].tasimasatir_bilgi[k].Ambar_kodu = xTsatir["Ambar_kodu"].InnerText;
                                                    mynewtcgb.Ozetbeyanlar[j].ozbyacma_bilgi[i].tasimasatir_bilgi[k].Acilacak_miktar = Convert.ToDecimal(xTsatir["Acilacak_miktar"].InnerText, nfi1);
                                                }

                                            }

                                    }

                                }



                        }
                    }
                }





                // Teminat

                XmlNodeList XmlNodeListTemObj1 = dd.GetElementsByTagName("Teminat");
                if (XmlNodeListTemObj1[0] != null)
                {

                    if (XmlNodeListTemObj1[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xTem = XmlNodeListTemObj1[0].ChildNodes;
                        mynewtcgb.Teminat = new KontrolHizmeti.Teminat[XmlNodeListTemObj1[0].ChildNodes.Count];


                        for (int i = 0; i < xTem.Count; i++)
                        {

                            mynewtcgb.Teminat[i] = new KontrolHizmeti.Teminat();
                            if (xTem[i]["Teminat_sekli"] != null)
                                mynewtcgb.Teminat[i].Teminat_sekli = xTem[i]["Teminat_sekli"].InnerText;
                            if (xTem[i]["Teminat_orani"].InnerText != "")
                                mynewtcgb.Teminat[i].Teminat_orani = Convert.ToDecimal(xTem[i]["Teminat_orani"].InnerText, nfi1);
                            else mynewtcgb.Teminat[i].Teminat_orani = 0;
                            mynewtcgb.Teminat[i].Global_teminat_no = xTem[i]["Global_teminat_no"].InnerText;
                            if (xTem[i]["Banka_mektubu_tutari"].InnerText != "")
                                mynewtcgb.Teminat[i].Banka_mektubu_tutari = Convert.ToDecimal(xTem[i]["Banka_mektubu_tutari"].InnerText, nfi1);
                            else mynewtcgb.Teminat[i].Banka_mektubu_tutari = 0;
                            if (xTem[i]["Nakdi_teminat_tutari"].InnerText != "")
                                mynewtcgb.Teminat[i].Nakdi_teminat_tutari = Convert.ToDecimal(xTem[i]["Nakdi_teminat_tutari"].InnerText, nfi1);
                            else mynewtcgb.Teminat[i].Nakdi_teminat_tutari = 0;
                            if (xTem[i]["Diger_tutar"].InnerText != "")
                                mynewtcgb.Teminat[i].Diger_tutar = Convert.ToDecimal(xTem[i]["Diger_tutar"].InnerText, nfi1);
                            else mynewtcgb.Teminat[i].Diger_tutar = 0;
                            mynewtcgb.Teminat[i].Aciklama = xTem[i]["Aciklama"].InnerText;
                            mynewtcgb.Teminat[i].Diger_tutar_referansi = xTem[i]["Diger_tutar_referansi"].InnerText;


                        }



                    }
                }




                // Firma Bilgisi

                XmlNodeList XmlNodeListFrmObj1 = dd.GetElementsByTagName("Firma_bilgi");
                if (XmlNodeListFrmObj1[0] != null)
                {
                    if (XmlNodeListFrmObj1[0].ChildNodes.Count > 0)
                    {

                        XmlNodeList xFrm = XmlNodeListFrmObj1[0].ChildNodes;

                        mynewtcgb.Firma_bilgi = new KontrolHizmeti.firma[XmlNodeListFrmObj1[0].ChildNodes.Count];


                        for (int i = 0; i < xFrm.Count; i++)
                        {

                            mynewtcgb.Firma_bilgi[i] = new KontrolHizmeti.firma();
                            if (xFrm[i]["Adi_unvani"] != null)
                                mynewtcgb.Firma_bilgi[i].Adi_unvani = xFrm[i]["Adi_unvani"].InnerText;
                            if (xFrm[i]["Cadde_s_no"] != null)
                                mynewtcgb.Firma_bilgi[i].Cadde_s_no = xFrm[i]["Cadde_s_no"].InnerText;
                            if (xFrm[i]["Faks"] != null)
                                mynewtcgb.Firma_bilgi[i].Faks = xFrm[i]["Faks"].InnerText;
                            if (xFrm[i]["Il_ilce"] != null)
                                mynewtcgb.Firma_bilgi[i].Il_ilce = xFrm[i]["Il_ilce"].InnerText;
                            if (xFrm[i]["Posta_kodu"] != null)
                                mynewtcgb.Firma_bilgi[i].Posta_kodu = xFrm[i]["Posta_kodu"].InnerText;
                            if (xFrm[i]["Telefon"] != null)
                                mynewtcgb.Firma_bilgi[i].Telefon = xFrm[i]["Telefon"].InnerText;
                            if (xFrm[i]["Ulke_kodu"] != null)
                                mynewtcgb.Firma_bilgi[i].Ulke_kodu = xFrm[i]["Ulke_kodu"].InnerText;
                            if (xFrm[i]["Tip"] != null)
                                mynewtcgb.Firma_bilgi[i].Tip = xFrm[i]["Tip"].InnerText;
                            if (xFrm[i]["No"] != null)
                                mynewtcgb.Firma_bilgi[i].No = xFrm[i]["No"].InnerText;
                            if (xFrm[i]["Kimlik_turu"] != null)
                                mynewtcgb.Firma_bilgi[i].Kimlik_turu = xFrm[i]["Kimlik_turu"].InnerText;


                        }


                    }
                }

                //Dökümanlar....
                XmlNodeList XmlNodeListDokObj1 = dd.GetElementsByTagName("Dokumanlar");
                if (XmlNodeListDokObj1[0] != null)
                {
                    if (XmlNodeListDokObj1[0].ChildNodes.Count > 0)
                    {

                        XmlNodeList xDok = XmlNodeListDokObj1[0].ChildNodes;

                        mynewtcgb.Dokumanlar = new KontrolHizmeti.Dokuman[XmlNodeListDokObj1[0].ChildNodes.Count];


                        for (int i = 0; i < xDok.Count; i++)
                        {

                            mynewtcgb.Dokumanlar[i] = new KontrolHizmeti.Dokuman();
                            if (xDok[i]["Kalem_no"] != null)
                                mynewtcgb.Dokumanlar[i].Kalem_no = Convert.ToInt32(xDok[i]["Kalem_no"].InnerText);
                            if (xDok[i]["Kod"] != null)
                                mynewtcgb.Dokumanlar[i].Kod = xDok[i]["Kod"].InnerText;
                            if (xDok[i]["Dogrulama"] != null)
                                mynewtcgb.Dokumanlar[i].Dogrulama = xDok[i]["Dogrulama"].InnerText;
                            if (xDok[i]["Belge_tarihi"] != null)
                                mynewtcgb.Dokumanlar[i].Belge_tarihi = xDok[i]["Belge_tarihi"].InnerText;
                            if (xDok[i]["Referans"] != null)
                                mynewtcgb.Dokumanlar[i].Referans = xDok[i]["Referans"].InnerText;
                            //if (xDok[i]["Referans"] != null)
                            //    mynewtcgb.Dokumanlar[i].Vi = xDok[i]["Referans"].InnerText;

                        }
                    }
                }


                //Soru Cevaplar....
                XmlNodeList XmlNodeListSoruCevapObj1 = dd.GetElementsByTagName("Sorular_cevaplar");
                if (XmlNodeListSoruCevapObj1[0] != null)
                {
                    if (XmlNodeListSoruCevapObj1[0].ChildNodes.Count > 0)
                    {

                        XmlNodeList xSrCv = XmlNodeListSoruCevapObj1[0].ChildNodes;

                        mynewtcgb.Sorular_cevaplar = new KontrolHizmeti.Soru_Cevap[XmlNodeListSoruCevapObj1[0].ChildNodes.Count];


                        for (int i = 0; i < xSrCv.Count; i++)
                        {
                            mynewtcgb.Sorular_cevaplar[i] = new KontrolHizmeti.Soru_Cevap();
                            mynewtcgb.Sorular_cevaplar[i].Kalem_no = Convert.ToInt32(xSrCv[i]["Kalem_no"].InnerText);
                            mynewtcgb.Sorular_cevaplar[i].Soru_no = xSrCv[i]["Soru_no"].InnerText;
                            mynewtcgb.Sorular_cevaplar[i].Cevap = xSrCv[i]["Cevap"].InnerText;


                        }
                    }
                }

                myGelenEGEWSTEST.BeyannameBilgi = mynewtcgb;
                return true;
            }
            catch (Exception exc)
            {

                return false;

            }
        }
        private void btnTescil_Click(object sender, EventArgs e)
        {

            TescilHizmeti.Gumruk_Biztalk_EImzaTescil_Tescil_PortTescilSoapClient client = ServiceHelper.GetTescilWSClient("", "");

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                {
                    return true;
                };

                System.Xml.Linq.XDocument xmlDoc = new System.Xml.Linq.XDocument();
                System.Xml.Linq.XElement root = new System.Xml.Linq.XElement("Root", rchImza.Text);
                XmlElement sonuc = null;
                XmlElement element = null;


                XElement xt = new XElement(root);
                client.Tescil(ref xt);
                MessageBox.Show(xt.Value);



                if (comboBoxGonderimTuru.SelectedIndex == 5 || comboBoxGonderimTuru.SelectedIndex == 6)
                {
                    MessageBox.Show(element.InnerText);
                }
                //else
                //{
                //    MessageBox.Show(root.Value);
                //}

                //XmlDocument dd = new XmlDocument();
                //dd.LoadXml(root.Value);
                //XmlNodeList XmlNodeListObj1 = dd.GetElementsByTagName("RefID");
                //XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("Guid");

                //if (XmlNodeListObj1[0] != null && XmlNodeListObj2[0] != null)
                //{
                //    Refid = XmlNodeListObj1[0].ChildNodes[0].Value;
                //    Guid = XmlNodeListObj2[0].ChildNodes[0].Value;
                //    MessageBox.Show("GUID:" + Guid + "\n" + "REFID:" + Refid + "\n" + "Islem Basladi");
                //    TxtGuid.Text = Guid;
                //    frmSrg.TxtGuid.Text = Guid;
                //    frmSrg.btnGuid_Click(null, e);
                //    this.Close();
                //}
                //    else MessageBox.Show(root.OuterXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOzet_Click(object sender, EventArgs e)
        {
            //    try
            //    {
            //        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
            //        {
            //            return true;
            //        };

            //        System.Xml.Linq.XDocument xmlDoc = new System.Xml.Linq.XDocument();
            //        System.Xml.Linq.XElement root = new System.Xml.Linq.XElement("Root", rchImza.Text);
            //        XmlElement sonuc = null;
            //        XmlElement element = null;

            //            XmlDocument xDoc = new XmlDocument();
            //            element = xDoc.ReadNode(root.CreateReader()) as XmlElement;

            //            myozetBeyanEGEWS_Test.OzetBeyan(ref element);


            //        if (comboBoxGonderimTuru.SelectedIndex == 5 || comboBoxGonderimTuru.SelectedIndex == 7)
            //        {
            //            MessageBox.Show(element.InnerXml);
            //        }
            //        else
            //        {
            //            MessageBox.Show(root.Value);
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
        }

        private void btnTescilCevabi_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    XmlDocument dd = new XmlDocument();

            //    dd.LoadXml(xmlGidecekVeri.XmlText);

            //    XmlNodeList XmlNodeListObj333 = dd.GetElementsByTagName("Sifre");
            //    XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("GUMRUK");
            //    XmlNodeList XmlNodeListObj21 = dd.GetElementsByTagName("GumrukIdaresi");
            //    XmlNodeList XmlNodeListObj222 = dd.GetElementsByTagName("Musavir_referansi");
            //    if (TxtTip.Text != "Özet Beyan")
            //    {
            //        if (XmlNodeListObj222[0] != null)
            //        {
            //            mydstescilsonuc.Clear();
            //            sifre = XmlNodeListObj333[0].ChildNodes[0].Value;
            //            gumruk = XmlNodeListObj2[0].ChildNodes[0].Value;
            //            mreferans = XmlNodeListObj222[0].ChildNodes[0].Value;
            //            btnTescilCevabi.Enabled = false;


            //        }
            //        else
            //        {
            //            MessageBox.Show("Müşavir Referansı Yok");
            //            btnIptal.Visible = true;
            //        }
            //    }
            //    else
            //    {
            //        sifre = XmlNodeListObj333[0].ChildNodes[0].Value;
            //        gumruk = XmlNodeListObj21[0].ChildNodes[0].Value;
            //        btnTescilCevabi.Enabled = false;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    XmlDocument dd = new XmlDocument();

            //    dd.LoadXml(xmlGidecekVeri.XmlText);

            //    XmlNodeList XmlNodeListObj333 = dd.GetElementsByTagName("Sifre");

            //    string sonuc = mysn.SonuclanmamisIslemdekiMesajiIptalet(TxtKullanici.Text, XmlNodeListObj333[0].ChildNodes[0].Value, TxtGuid.Text);
            //    if (sonuc != "")
            //    {
            //        MessageBox.Show(sonuc);
            //        frmSrg.txtKullaniciKodu.Text = "";
            //        frmSrg.txtReferansNo.Text = "";
            //        frmSrg.radioButton2.Checked = true;
            //        frmSrg.txtGuidof.Text = Guid;
            //        frmSrg.btnGenel_Click(null, e);
            //        this.Close();
            //    }
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnETGB_Click(object sender, EventArgs e)
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
            //{
            //    return true;
            //};


            //    try
            //    {
            //        ETicaretEGE_Test.GumrukETApp_ETOrch_ReceiveInpPortClient etgbClient = new ETicaretEGE_Test.GumrukETApp_ETOrch_ReceiveInpPortClient();
            //        ETicaretEGE_Test.Root r = new ETicaretEGE_Test.Root();
            //        r.RequestMessage = rchImza.Text;
            //        ETicaretEGE_Test.OutputMessage sonuc = etgbClient.MesajGonder(r);

            //        MessageBox.Show(sonuc.Record.InnerXml);

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);

            //    }


        }

        private void btnGIN_Click(object sender, EventArgs e)
        {
            //GIN_EGEWS.Gumruk_Biztalk_EImzaTescil_GIN_GumrukIdareleriNotlariSoapClient ginClient = new GIN_EGEWS.Gumruk_Biztalk_EImzaTescil_GIN_GumrukIdareleriNotlariSoapClient();
            //GIN_EGEWS.Gelen ginMsg = new GIN_EGEWS.Gelen();
            //GIN_EGEWS.ginBilgi gb = new GIN_EGEWS.ginBilgi();

            //XmlDocument dd = new XmlDocument();
            //dd.LoadXml(xmlGidecekVeri.XmlText);


            //XmlNodeList XmlNodeListObj331 = dd.GetElementsByTagName("KullaniciAdi");
            //XmlNodeList XmlNodeListObj333 = dd.GetElementsByTagName("Sifre");
            //XmlNodeList XmlNodeListObj221 = dd.GetElementsByTagName("RefID");

            //XmlNodeList xnlGumrukIdaresi = dd.GetElementsByTagName("Gumruk_idaresi");
            //XmlNodeList xnlKullaniciKodu = dd.GetElementsByTagName("Kullanici_kodu");
            //XmlNodeList xnlBeyannameNo = dd.GetElementsByTagName("Beyanname_no");
            //XmlNodeList xnlMuhurNo = dd.GetElementsByTagName("Muhur_no");
            //XmlNodeList xnlMuhurSayisi = dd.GetElementsByTagName("Muhur_sayisi");
            //XmlNodeList xnlCikisSuresi = dd.GetElementsByTagName("Cikis_suresi");
            //XmlNodeList xnlAsilSorumlu = dd.GetElementsByTagName("Asil_sorumlu_vergino");
            //XmlNodeList xnlAciklama = dd.GetElementsByTagName("Aciklama");

            //if (xnlAciklama != null && xnlAciklama[0].ChildNodes.Count != 0)
            //    gb.Aciklama = xnlAciklama[0].ChildNodes[0].Value;
            //if (xnlGumrukIdaresi != null && xnlGumrukIdaresi[0].ChildNodes.Count != 0)
            //    gb.Gumruk_idaresi = xnlGumrukIdaresi[0].ChildNodes[0].Value;
            //if (xnlBeyannameNo != null && xnlBeyannameNo[0].ChildNodes.Count != 0)
            //    gb.Beyanname_no = xnlBeyannameNo[0].ChildNodes[0].Value;
            //if (xnlMuhurNo != null && xnlMuhurNo[0].ChildNodes.Count != 0)
            //    gb.Muhur_no = Decimal.Parse(xnlMuhurNo[0].ChildNodes[0].Value);
            //if (xnlMuhurSayisi != null && xnlMuhurSayisi[0].ChildNodes.Count != 0)
            //    gb.Muhur_sayisi = Decimal.Parse(xnlMuhurSayisi[0].ChildNodes[0].Value);
            //if (xnlCikisSuresi != null && xnlCikisSuresi[0].ChildNodes.Count != 0)
            //    gb.Cikis_suresi = Int32.Parse(xnlCikisSuresi[0].ChildNodes[0].Value);
            //if (xnlAsilSorumlu != null && xnlAsilSorumlu[0].ChildNodes.Count != 0)
            //    gb.Asil_sorumlu_vergino = xnlAsilSorumlu[0].ChildNodes[0].Value;
            //if (xnlKullaniciKodu != null && xnlKullaniciKodu[0].ChildNodes.Count != 0)
            //    gb.Kullanici_kodu = xnlKullaniciKodu[0].ChildNodes[0].Value;

            //ginMsg.KullaniciAdi = XmlNodeListObj331[0].ChildNodes[0].Value;
            //ginMsg.RefID = XmlNodeListObj221[0].ChildNodes[0].Value;
            //ginMsg.Sifre = XmlNodeListObj333[0].ChildNodes[0].Value;

            //ginMsg.ginBilgi = gb;

            //XmlNode xmlSonuc = ginClient.GINServiceReqRes(ginMsg);
            //MessageBox.Show(xmlSonuc.InnerText);

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //XmlDocument dd = new XmlDocument();

            //dd.LoadXml(xmlGidecekVeri.XmlText);

            //XmlNodeList XmlNodeListObj333 = dd.GetElementsByTagName("Sifre");
            //XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("GUMRUK");


            //MESAJSIL mysil = new MESAJSIL();
            //mysil.TxtGuid.Text = TxtGuid.Text;
            //mysil.TxtKullanici.Text = TxtKullanici.Text;
            //mysil.TxtSifre.Text = XmlNodeListObj333[0].ChildNodes[0].Value;
            //mysil.TxtGumruk.Text = XmlNodeListObj2[0].ChildNodes[0].Value;
            //mysil.ShowDialog();
        }

        private void btnSira_Click(object sender, EventArgs e)
        {
            //GetSonucWSClient client = new GetSonucWSClient();
            //   DataSet sonuc = client.IslemSonucGetir4(TxtGuid.Text);
            //if (sonuc.Tables[0].Rows.Count > 0)
            //{
            //    if (sonuc.Tables[0].Columns["Islem"] != null)
            //        MessageBox.Show(sonuc.Tables[0].Rows[0]["Islem"].ToString());
            //    else MessageBox.Show("İşlem Sonuçlanmıştır.");



            //}
        }

        private void btnXml_Click(object sender, EventArgs e)
        {
            //FRMXML myfr = new FRMXML();
            //Encoding unicode = Encoding.UTF8;
            //byte[] msgBytes = Convert.FromBase64String(rchImza.Text);
            //myfr.richTextBox1.Text = Imzala.PlainText(msgBytes);
            //myfr.richTextBox2.Text = Imzala.CertificatesInfo(msgBytes);
            //myfr.ShowDialog();
        }

        private void btnImzaliVeriKaydet_Click(object sender, EventArgs e)
        {
            string commandTarihceText = "update Tarihce set ImzaliVeri=@ImzaliVeri, SonIslemZamani=@Tarih  Where Guid = @GUID;";
            string commandIslemText = "update Islem set  SonIslemZamani=@Tarih,IslemZamani=@Tarih, IslemDurumu='İmzalandı' Where guidof = @GUID;";
            using (SqlConnection connection = new SqlConnection(sqlconnProd))
            { try
                {
                SqlCommand command = new SqlCommand(commandTarihceText, connection);
                command.Parameters.Add("@GUID", SqlDbType.VarChar);
                command.Parameters["@GUID"].Value = Guid;
                command.Parameters.Add("@Tarih", SqlDbType.DateTime2);
                command.Parameters["@Tarih"].Value = DateTime.Now;
                command.Parameters.Add("@ImzaliVeri", SqlDbType.VarChar);
                command.Parameters["@ImzaliVeri"].Value = rchImza.Text;

                    SqlCommand command2 = new SqlCommand(commandIslemText, connection);
                    command2.Parameters.Add("@GUID", SqlDbType.VarChar).Value = Guid;
                    command2.Parameters.Add("@Tarih", SqlDbType.DateTime2).Value = DateTime.Now; 
                 
                    
                    //connection.Open();
                    command.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Kaydetme İşlemi Başarılı");


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
