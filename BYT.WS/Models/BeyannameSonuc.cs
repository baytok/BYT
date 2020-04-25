using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class BeyannameXmlSonuc
    {
        public string SonucXml { get; set; } 


        private string tip;

        public string Tip
        {
            get { return tip; }
            set { tip = value; }
        }
        private string tescil_tarihi;

        public string Tescil_tarihi
        {
            get { return tescil_tarihi; }
            set { tescil_tarihi = value; }
        }
        private string beyanname_no;

        public string Beyanname_no
        {
            get { return beyanname_no; }
            set { beyanname_no = value; }
        }
        private List<HataMesaji> hatalar = new List<HataMesaji>();

        public List<HataMesaji> Hatalar
        {
            get { return hatalar; }
            set { hatalar = value; }
        }
        private List<Soru> sorular = new List<Soru>();

        public List<Soru> Sorular
        {
            get { return sorular; }
            set { sorular = value; }
        }

        private List<Soru_Cevap> soru_cevap = new List<Soru_Cevap>();

        public List<Soru_Cevap> Soru_cevap
        {
            get { return soru_cevap; }
            set { soru_cevap = value; }
        }

        private List<Belge> belgeler = new List<Belge>();

        public List<Belge> Belgeler
        {
            get { return belgeler; }
            set { belgeler = value; }
        }
        private List<Vergi> vergiler = new List<Vergi>();

        public List<Vergi> Vergiler
        {
            get { return vergiler; }
            set { vergiler = value; }
        }
        private List<Toplam_Vergi> toplam_vergiler = new List<Toplam_Vergi>();

        public List<Toplam_Vergi> Toplam_vergiler
        {
            get { return toplam_vergiler; }
            set { toplam_vergiler = value; }
        }

        private List<Toplanan_Vergi> toplanan_vergiler = new List<Toplanan_Vergi>();

        public List<Toplanan_Vergi> Toplanan_vergiler
        {
            get { return toplanan_vergiler; }
            set { toplanan_vergiler = value; }
        }

        private List<Hesap_detay> hesap_detaylari = new List<Hesap_detay>();

        public List<Hesap_detay> Hesap_detaylari
        {
            get { return hesap_detaylari; }
            set { hesap_detaylari = value; }
        }
        private List<ozetbeyantescilbilgi> ozetbeyan_bilgi = new List<ozetbeyantescilbilgi>();

        public List<ozetbeyantescilbilgi> Ozetbeyan_bilgi
        {
            get { return ozetbeyan_bilgi; }
            set { ozetbeyan_bilgi = value; }
        }
        private List<Gumruk_Kiymeti> gumruk_kiymetleri = new List<Gumruk_Kiymeti>();

        public List<Gumruk_Kiymeti> Gumruk_kiymetleri
        {
            get { return gumruk_kiymetleri; }
            set { gumruk_kiymetleri = value; }
        }




        private List<Istatistiki_Kiymeti> istatistiki_kiymetleri = new List<Istatistiki_Kiymeti>();

        public List<Istatistiki_Kiymeti> Istatistiki_kiymetleri
        {
            get { return istatistiki_kiymetleri; }
            set { istatistiki_kiymetleri = value; }
        }



        private string cikti_kontrol_kodu;

        public string Cikti_kontrol_kodu
        {
            get { return cikti_kontrol_kodu; }
            set { cikti_kontrol_kodu = value; }
        }
        private string doviz_kuru_alis;

        public string Doviz_kuru_alis
        {
            get { return doviz_kuru_alis; }
            set { doviz_kuru_alis = value; }
        }
        private string doviz_kuru_satis;

        public string Doviz_kuru_satis
        {
            get { return doviz_kuru_satis; }
            set { doviz_kuru_satis = value; }
        }

        public string Muayene_memuru { get; set; }

        public string Kalan_kontor { get; set; }

    }
    public class Gumruk_Kiymeti
    {

        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }


        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

    }
    public class Istatistiki_Kiymeti
    {

        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }


        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

    }
    public class Soru_Cevap
    {

        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }
        private string soru_no;

        public string Soru_no
        {
            get { return soru_no; }
            set { soru_no = value; }
        }
        private string cevap;

        public string Cevap
        {
            get { return cevap; }
            set { cevap = value; }
        }

    }
    public class HataMesaji
    {

        [Required]
      
        public int HataKodu { get; set; }

        [Required]
        [StringLength(250)]
        public string HataAciklamasi { get; set; }

      

    }
    public class Belge
    {
        //public Belge(string pKod, string pAciklama, string pDoğrulama, string pReferans)
        //{
        //    this.Kod = pKod;
        //    this.Aciklama = pAciklama;
        //    this.Doğrulama = pDoğrulama;
        //    this.Referans = pReferans;

        //}
        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }
        private string kod;

        public string Kod
        {
            get { return kod; }
            set { kod = value; }
        }

        private string aciklama;

        public string Aciklama
        {
            get { return aciklama; }
            set { aciklama = value; }
        }

        private string dogrulama;

        public string Dogrulama
        {
            get { return dogrulama; }
            set { dogrulama = value; }
        }

        private string belge_tarihi;

        public string Belge_tarihi
        {
            get { return belge_tarihi; }
            set { belge_tarihi = value; }
        }
        private string referans;

        public string Referans
        {
            get { return referans; }
            set { referans = value; }
        }

        private string vize_tarihi;

        public string Vize_tarihi
        {
            get { return vize_tarihi; }
            set { vize_tarihi = value; }
        }
    }
    public class ozetbeyantescilbilgi
    {
        private string ozetbeyan_no;

        public string Ozetbeyan_no
        {
            get { return ozetbeyan_no; }
            set { ozetbeyan_no = value; }
        }
        private string tescil_tarihi;

        public string Tescil_tarihi
        {
            get { return tescil_tarihi; }
            set { tescil_tarihi = value; }
        }

    }
    public class Vergi
    {
        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }

        private string kod;

        public string Kod
        {
            get { return kod; }
            set { kod = value; }
        }

        private string aciklama;

        public string Aciklama
        {
            get { return aciklama; }
            set { aciklama = value; }
        }

        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

        private string oran;

        public string Oran
        {
            get { return oran; }
            set { oran = value; }
        }

        private string odeme_sekli;

        public string Odeme_sekli
        {
            get { return odeme_sekli; }
            set { odeme_sekli = value; }
        }

        private string matrah;

        public string Matrah
        {
            get { return matrah; }
            set { matrah = value; }
        }
    }
    public class Toplam_Vergi
    {
        private string kod;

        public string Kod
        {
            get { return kod; }
            set { kod = value; }
        }

        private string aciklama;

        public string Aciklama
        {
            get { return aciklama; }
            set { aciklama = value; }
        }

        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

        private string odeme_sekli;

        public string Odeme_sekli
        {
            get { return odeme_sekli; }
            set { odeme_sekli = value; }
        }
    }
    public class Toplanan_Vergi
    {

        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

        private string odeme_sekli;

        public string Odeme_sekli
        {
            get { return odeme_sekli; }
            set { odeme_sekli = value; }
        }
    }
    public class Soru
    {
        private string kod;

        public string Kod
        {
            get { return kod; }
            set { kod = value; }
        }

        private string aciklama;

        public string Aciklama
        {
            get { return aciklama; }
            set { aciklama = value; }
        }

        //private string cevap;

        //public string Cevap
        //{
        //    get { return cevap; }
        //    set { cevap = value; }
        //}

        private int kalem_no;

        public int Kalem_no
        {
            get { return kalem_no; }
            set { kalem_no = value; }
        }
        private string tip;

        public string Tip
        {
            get { return tip; }
            set { tip = value; }
        }
        private Cevaplar cevaplar;

        public Cevaplar Cevaplar
        {
            get { return cevaplar; }
            set { cevaplar = value; }
        }




    }
    public class Hesap_detay
    {


        private string aciklama;

        public string Aciklama
        {
            get { return aciklama; }
            set { aciklama = value; }
        }

        private string miktar;

        public string Miktar
        {
            get { return miktar; }
            set { miktar = value; }
        }

    }
    public class Cevaplar
    {

        //private string evet_1_kod;

        //public string Evet_1_kod
        //{
        //    get { return evet_1_kod; }
        //    set { evet_1_kod = value; }
        //}
        //private string evet_1_aciklama;

        //public string Evet_1_aciklama
        //{
        //    get { return evet_1_aciklama; }
        //    set { evet_1_aciklama = value; }
        //}

        //private string evet_2_kod;

        //public string Evet_2_kod
        //{
        //    get { return evet_2_kod; }
        //    set { evet_2_kod = value; }
        //}
        //private string evet_2_aciklama;

        //public string Evet_2_aciklama
        //{
        //    get { return evet_2_aciklama; }
        //    set { evet_2_aciklama = value; }
        //}

        //private string evet_3_kod;

        //public string Evet_3_kod
        //{
        //    get { return evet_3_kod; }
        //    set { evet_3_kod = value; }
        //}
        //private string evet_3_aciklama;

        //public string Evet_3_aciklama
        //{
        //    get { return evet_3_aciklama; }
        //    set { evet_3_aciklama = value; }
        //}

        //private string evet_4_kod;

        //public string Evet_4_kod
        //{
        //    get { return evet_4_kod; }
        //    set { evet_4_kod = value; }
        //}
        //private string evet_4_aciklama;

        //public string Evet_4_aciklama
        //{
        //    get { return evet_4_aciklama; }
        //    set { evet_4_aciklama = value; }
        //}

        //private string hayir_1_kod;

        //public string Hayir_1_kod
        //{
        //    get { return hayir_1_kod; }
        //    set { hayir_1_kod = value; }
        //}
        //private string hayir_1_aciklama;

        //public string Hayir_1_aciklama
        //{
        //    get { return hayir_1_aciklama; }
        //    set { hayir_1_aciklama = value; }
        //}

        //private string hayir_2_kod;

        //public string Hayir_2_kod
        //{
        //    get { return hayir_2_kod; }
        //    set { hayir_2_kod = value; }
        //}
        //private string hayir_2_aciklama;

        //public string Hayir_2_aciklama
        //{
        //    get { return hayir_2_aciklama; }
        //    set { hayir_2_aciklama = value; }
        //}

        //private string hayir_3_kod;

        //public string Hayir_3_kod
        //{
        //    get { return hayir_3_kod; }
        //    set { hayir_3_kod = value; }
        //}
        //private string hayir_3_aciklama;

        //public string Hayir_3_aciklama
        //{
        //    get { return hayir_3_aciklama; }
        //    set { hayir_3_aciklama = value; }
        //}

        //private string hayir_4_kod;

        //public string Hayir_4_kod
        //{
        //    get { return hayir_4_kod; }
        //    set { hayir_4_kod = value; }
        //}
        //private string hayir_4_aciklama;

        //public string Hayir_4_aciklama
        //{
        //    get { return hayir_4_aciklama; }
        //    set { hayir_4_aciklama = value; }
        //}
        private List<Evet> evetler = new List<Evet>();

        public List<Evet> Evetler
        {
            get { return evetler; }
            set { evetler = value; }
        }



        private List<Hayir> hayirlar = new List<Hayir>();

        public List<Hayir> Hayirlar
        {
            get { return hayirlar; }
            set { hayirlar = value; }
        }


    }
    public class Evet
    {
        private string sira;

        public string Sira
        {
            get { return sira; }
            set { sira = value; }
        }
        private string kodu;

        public string Kodu
        {
            get { return kodu; }
            set { kodu = value; }
        }
        private string aciklamasi;

        public string Aciklamasi
        {
            get { return aciklamasi; }
            set { aciklamasi = value; }
        }

    }
    public class Hayir
    {
        private string sira;

        public string Sira
        {
            get { return sira; }
            set { sira = value; }
        }
        private string kodu;

        public string Kodu
        {
            get { return kodu; }
            set { kodu = value; }
        }
        private string aciklamasi;

        public string Aciklamasi
        {
            get { return aciklamasi; }
            set { aciklamasi = value; }
        }

    }
    public class BeyannameSonuc
    {
        public string SonucXml { get; set; }

        public string IslemTipi { get; set; }

        public string TescilTarihi { get; set; }

        public string BeyannameNo { get; set; }

        public string CiktiSeriNo { get; set; }

        public string DovizKuruAlis { get; set; }

        public string DovizKuruSatis { get; set; }     

        public string MuayeneMemuru { get; set; }

        public string KalanKontor { get; set; }

        public List<DbSonucHatalar> Hatalar { get; set; }

        public List<DbSonucSorular> Sorular { get; set; }

        public List<DbSonucSoruCevaplar> SoruCevap { get; set; }

        public List<DbSonucBelgeler> Belgeler { get; set; }

        public List<DbSonucVergiler> Vergiler { get; set; }

        public List<DbSonucToplamVergiler> ToplamVergiler { get; set; }

        public List<DbSonucToplananVergiler> ToplananVergiler { get; set; }

        public List<DbSonucHesapDetaylar> HesapDetaylari { get; set; }

        public List<DbSonucOzetBeyan> OzetbeyanBilgi { get; set; }

        public List<DbSonucGumrukKiymeti> GumrukKiymetleri { get; set; }

        public List<DbSonucIstatistikiKiymet> IstatistikiKiymetler { get; set; }

      
        }
    public class DbSonucHatalar
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        [StringLength(30)]
        public int HataKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string HataAciklamasi { get; set; }


    }
    public class DbSonucSorular
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string SoruKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string SoruAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Tip { get; set; }

        [StringLength(int.MaxValue)]
        public string Cevaplar { get; set; }
    }
    public class DbSonucSoruCevaplar
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string SoruKodu { get; set; }

     
        [Required]
        [StringLength(10)]
        public string SoruCevap { get; set; }

    
    }
    public class DbSonucBelgeler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string BelgeKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string BelgeAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Dogrulama { get; set; }

        [Required]
        [StringLength(30)]
        public string Referans { get; set; }

        [Required]
        [StringLength(12)]
        public string BelgeTarihi { get; set; }
    }
    public class DbSonucVergiler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string VergiKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string VergiAciklamasi { get; set; }

      
        [StringLength(20)]
        public string Miktar { get; set; }

        [Required]
        [StringLength(5)]
        public string Oran { get; set; }

        [Required]
        [StringLength(3)]
        public string OdemeSekli { get; set; }

        [StringLength(20)]
        public string Matrah { get; set; }
    }
    public class DbSonucToplamVergiler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }


        [Required]
        [StringLength(10)]
        public string VergiKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string VergiAciklamasi { get; set; }


        [StringLength(20)]
        public string Miktar { get; set; }

       

        [Required]
        [StringLength(3)]
        public string OdemeSekli { get; set; }

      
    }
    public class DbSonucToplananVergiler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }


        [StringLength(20)]
        public string Miktar { get; set; }



        [Required]
        [StringLength(3)]
        public string OdemeSekli { get; set; }


    }
    public class DbSonucHesapDetaylar
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }


        [StringLength(20)]
        public string Miktar { get; set; }


        [Required]
        [StringLength(100)]
        public string Aciklama { get; set; }


    }
    public class DbSonucGumrukKiymeti
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }


        [StringLength(20)]
        public string Miktar { get; set; }




    }
    public class DbSonucIstatistikiKiymet
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        public int KalemNo { get; set; }


        [StringLength(20)]
        public string Miktar { get; set; }




    }
    public class DbSonucOzetBeyan
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }


        [StringLength(20)]
        public string OzetBeyanNo { get; set; }

        [StringLength(20)]
        public string TescilTarihi { get; set; }


    }

    public class DbSonucDigerBilgiler
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }


        [StringLength(10)]
        public string CiktiSeriNo { get; set; }

        [StringLength(10)]
        public string DovizKuruAlis { get; set; }

        [StringLength(10)]
        public string DovizKuruSatis { get; set; }

        [StringLength(50)]
        public string MuayeneMemuru { get; set; }

        [StringLength(10)]
        public string KalanKontor { get; set; }



    }

    public class DbBelge
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(10)]
        public string BelgeKodu { get; set; }

     
        [StringLength(1000)]
        public string BelgeAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Dogrulama { get; set; }

       
        [StringLength(30)]
        public string Referans { get; set; }

        [Required]
        [StringLength(12)]
        public string BelgeTarihi { get; set; }
    }
    public class DbVergi
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

     
        [Required]     
        public int VergiKodu { get; set; }

      
        [StringLength(1000)]
        public string VergiAciklamasi { get; set; }

        [Required]
        public decimal Miktar { get; set; }

        [Required]
        [StringLength(5)]
        public string Oran { get; set; }

        [Required]
        [StringLength(3)]
        public string OdemeSekli { get; set; }

        [Required]
        public decimal Matrah { get; set; }
    }
    public class DbSoruCevap
    {
        [Key]
        public int ID { get; set; }

      
        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

       
        [Required]
        [StringLength(10)]
        public string SoruKodu { get; set; }

        
        [StringLength(10)]
        public string SoruCevap { get; set; }

       
        [StringLength(1000)]
        public string SoruAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Tip { get; set; }

        [StringLength(int.MaxValue)]
        public string Cevaplar { get; set; }
    }

}
