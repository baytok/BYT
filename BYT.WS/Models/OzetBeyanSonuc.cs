using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class ObOzetBeyanSonuc
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
   
    
    public class OzetBeyanXmlSonuc
    {
        public string SonucXml { get; set; }

        private string beyanname_no;

        public string Beyanname_no
        {
            get { return beyanname_no; }
            set { beyanname_no = value; }
        }

        private string tescil_tarihi;

        public string Tescil_tarihi
        {
            get { return tescil_tarihi; }
            set { tescil_tarihi = value; }
        }


        private List<HataMesaji> hatalar = new List<HataMesaji>();

        public List<HataMesaji> Hatalar
        {
            get { return hatalar; }
            set { hatalar = value; }
        }




    }
    
}
