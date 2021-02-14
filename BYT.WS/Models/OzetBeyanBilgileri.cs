using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Gelen
    {
        public string KullaniciAdi { get; set; }
        public string RefID { get; set; }
        public string Sifre { get; set; }
        public string IP { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.gumruk.gov.tr/", IsNullable = true)]
        public OzetBeyanBilgisi OzetBeyanBilgisi { get; set; }
    }

   
    public class OzetBeyanBilgisi
    {
         //   [Required]
        [StringLength(20)]
        public string MusteriNo { get; set; }

        //  [Required]
        [StringLength(20)]
        public string FirmaNo { get; set; }
        public string BeyanSahibiVergiNo { get; set; }
        public string BeyanTuru { get; set; }
        public string Diger { get; set; }
        public string DorseNo1 { get; set; }
        public string DorseNo1Uyrugu { get; set; }
        public string DorseNo2 { get; set; }
        public string DorseNo2Uyrugu { get; set; }
        public int EkBelgeSayisi { get; set; }
        public string EmniyetGuvenlik { get; set; }
        public string GrupTasimaSenediNo { get; set; }
        public string GumrukIdaresi { get; set; }
        public string KullaniciKodu { get; set; }
        public string Kurye { get; set; }
        public string LimanYerAdiBos { get; set; }
        public string LimanYerAdiYuk { get; set; }
        public string OncekiBeyanNo { get; set; }
        public string PlakaSeferNo { get; set; }
        public string ReferansNumarasi { get; set; }
        public string RefNo { get; set; }
        public string Rejim { get; set; }
        public string TasitinAdi { get; set; }
        public string TasimaSekli { get; set; }
        public string TasiyiciVergiNo { get; set; }
        public string TirAtaKarneNo { get; set; }
        public string UlkeKodu { get; set; }
        public string UlkeKoduBos { get; set; }
        public string UlkeKoduYuk { get; set; }
        public string YuklemeBosaltmaYeri { get; set; }
        public string VarisCikisGumrukIdaresi { get; set; }
        public string VarisTarihSaati { get; set; }
        public string XmlRefId { get; set; }

        public List<TasimaSenediBilgisi> TasimaSenetleri { get; set; }
        public List<OzbyAcmaBilgisi> OzbyAcmalar { get; set; }

        public List<TasitinUgradigiUlkeBilgisi> TasitinUgradigiUlkeler { get; set; }

        public FirmaBilgisi TasiyiciFirma { get; set; }

    }

    public class TasimaSenediBilgisi
    {
        public string AcentaAdi { get; set; }
        public string AcentaVergiNo { get; set; }
        public string AliciAdi { get; set; }
        public string AliciVergiNo { get; set; }
        public string AmbarHariciMi { get; set; }
        public string BildirimTarafiAdi { get; set; }
        public string BildirimTarafiVergiNo { get; set; }
        public string DuzenlendigiUlke { get; set; }
        public string EmniyetGuvenlikT { get; set; }
        public string EsyaninBulunduguYer { get; set; }
        public string FaturaDoviz { get; set; }
        public decimal FaturaToplami { get; set; }
        public string GondericiAdi { get; set; }
        public string GondericiVergiNo { get; set; }
        public string GrupMu { get; set; }
        public string KonteynerMi { get; set; }
        public string NavlunDoviz { get; set; }
        public decimal NavlunTutari { get; set; }
        public string OdemeSekli { get; set; }
        public string OncekiSeferNumarasi { get; set; }
        public DateTime OncekiSeferTarihi { get; set; }
        public string OzetBeyanNo { get; set; }
        public string RoroMu { get; set; }
        public string SenetSiraNo { get; set; }
        public string AktarmaYapilacakMi { get; set; }
        public string AktarmaTipi { get; set; }
        public string TasimaSenediNo { get; set; }

        public List<TasimaSatiriBilgisi> TasimaSatirlari { get; set; }
        public List<UgranilanUlkeBilgisi> UgranilanUlkeler { get; set; }
        public List<IhracatBilgisi> Ihracatlar { get; set; }
    }
    public class TasimaSatiriBilgisi
    {
        public decimal BrutAgirlik { get; set; }
        public int KapAdedi { get; set; }
        public string KapCinsi { get; set; }
        public string KonteynerTipi { get; set; }
        public string MarkaNo { get; set; }
        public string MuhurNumarasi { get; set; }
        public decimal NetAgirlik { get; set; }
        public string OlcuBirimi { get; set; }
        public int SatirNo { get; set; }
        public string KonteynerYukDurumu { get; set; }
        public List<EsyaBilgisi> EsyaBilgileri { get; set; }
    }
    public class EsyaBilgisi
    {
        public string BmEsyaKodu { get; set; }
        public decimal BrutAgirlik { get; set; }
        public string EsyaKodu { get; set; }
        public string EsyaninTanimi { get; set; }
        public decimal KalemFiyati { get; set; }
        public string KalemFiyatiDoviz { get; set; }
        public int KalemSiraNo { get; set; }
        public decimal NetAgirlik { get; set; }
        public string OlcuBirimi { get; set; }

    }
    public class UgranilanUlkeBilgisi
    {
        public string LimanYerAdi { get; set; }
        public string UlkeKodu { get; set; }
    }
    public class IhracatBilgisi
    {
        public decimal BrutAgirlik { get; set; }
        public int KapAdedi { get; set; }
        public string Numarasi { get; set; }
        public string ParcaliMi { get; set; }
        public string Tipi { get; set; }
    }
    public class OzbyAcmaBilgisi
    {
        public string AcmaSekli { get; set; }
        public string AmbardaMi { get; set; }
        public string BeyannameNo { get; set; }
        public string BaskaRejimleAcilacakMi { get; set; }
        public string Aciklama { get; set; }
        public List<OzbyAcmaSenetBilgisi> OzbyAcmaSenetleri { get; set; }
    }
    public class OzbyAcmaSenetBilgisi
    {
        public string AcilanSenetNo { get; set; }
        public List<OzbyAcmaSatirBilgisi> OzbyAcmaSatirlari { get; set; }

    }
    public class OzbyAcmaSatirBilgisi
    {
        public string AmbarKodu { get; set; }
        public decimal AmbardakiMiktar { get; set; }
        public decimal AcilacakMiktar { get; set; }
        public string MarkaNo { get; set; }
        public string EsyaCinsi { get; set; }
        public string Birim { get; set; }
        public decimal ToplamMiktar { get; set; }
        public decimal KapatilanMiktar { get; set; }
        public string OlcuBirimi { get; set; }
        public int AcmaSatirNo { get; set; }
    }
    public class TasitinUgradigiUlkeBilgisi
    {
        public DateTime HareketTarihSaati { get; set; }
        public string LimanYerAdi { get; set; }
        public string UlkeKodu { get; set; }

    }
    public class FirmaBilgisi
    {
        public string AdiUnvani { get; set; }
        public string CadSNo { get; set; }
        public string Fax { get; set; }
        public string IlIlce { get; set; }
        public string KimlikNo { get; set; }
        public string KimlikTuru { get; set; }
        public string PostaKodu { get; set; }
        public string Tel { get; set; }
        public string UlkeKodu { get; set; }
        public string VergiDairesikodu { get; set; }
    }


    public class ObBeyan
    {

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OzetBeyanInternalNo { get; set; }

        //   [Required]
        [StringLength(20)]
        public string MusteriNo { get; set; }

        //  [Required]
        [StringLength(20)]
        public string FirmaNo { get; set; }

        [StringLength(20)]
        public string? OzetBeyanNo { get; set; }

        [StringLength(20)]
        public string BeyanSahibiVergiNo { get; set; }

        [Required]
        [StringLength(9)]
        public string BeyanTuru { get; set; }

        [StringLength(500)]
        public string Diger { get; set; }

        [StringLength(25)]
        public string DorseNo1 { get; set; }

        [StringLength(9)]
        public string DorseNo1Uyrugu { get; set; }

        [StringLength(25)]
        public string DorseNo2 { get; set; }

        [StringLength(9)]
        public string DorseNo2Uyrugu { get; set; }

        public int EkBelgeSayisi { get; set; }

        [StringLength(9)]
        public string EmniyetGuvenlik { get; set; }

        [StringLength(20)]
        public string GrupTasimaSenediNo { get; set; }

        [StringLength(9)]
        public string GumrukIdaresi { get; set; }

        [StringLength(15)]
        public string KullaniciKodu { get; set; }

        [StringLength(9)]
        public string Kurye { get; set; }

        [StringLength(20)]
        public string LimanYerAdiBos { get; set; }

        [StringLength(20)]
        public string LimanYerAdiYuk { get; set; }

        [StringLength(20)]
        public string OncekiBeyanNo { get; set; }

        [StringLength(25)]
        public string PlakaSeferNo { get; set; }

        [StringLength(25)]
        public string ReferansNumarasi { get; set; }

        [StringLength(25)]
        public string RefNo { get; set; }

        [StringLength(9)]
        [Required]
        public string Rejim { get; set; }

        [StringLength(9)]
        public string TasimaSekli { get; set; }

        [StringLength(50)]
        public string TasitinAdi { get; set; }

        [StringLength(20)]
        public string TasiyiciVergiNo { get; set; }

        [StringLength(20)]
        public string TirAtaKarneNo { get; set; }

        [StringLength(9)]
        public string UlkeKodu { get; set; }

        [StringLength(9)]
        public string UlkeKoduYuk { get; set; }

        [StringLength(9)]
        public string UlkeKoduBos { get; set; }

        [StringLength(20)]
        public string YuklemeBosaltmaYeri { get; set; }

        [StringLength(9)]
        public string VarisCikisGumrukIdaresi { get; set; }

        [StringLength(12)]
        public string VarisTarihSaati { get; set; }

        [StringLength(35)]
        public string XmlRefId { get; set; }

        [StringLength(50)]
        public string TescilStatu { get; set; }

        public DateTime? TescilTarihi { get; set; }

        public DateTime? OlsuturulmaTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }

    public class ObTasitUgrakUlke
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [StringLength(12)]
        [Required]
        public string HareketTarihSaati { get; set; }

        [StringLength(20)]
        [Required]
        public string LimanYerAdi { get; set; }

        [StringLength(9)]
        [Required]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }

    public class ObTasiyiciFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }



        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }


        [StringLength(15)]
        public string Faks { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }

        [Required]
        [StringLength(9)]
        public string KimlikTuru { get; set; }

        [Required]
        [StringLength(20)]
        public string No { get; set; }


        [StringLength(10)]
        public string PostaKodu { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefon { get; set; }

        [Required]
        [StringLength(15)]
        public string Tip { get; set; }

        [Required]
        [StringLength(9)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }

    public class ObOzetBeyanAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanAcmaBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanNo { get; set; }

        [Required]
        [StringLength(9)]
        public string IslemKapsami { get; set; }

        [Required]
        [StringLength(9)]
        public string Ambar { get; set; }

        [Required]
        [StringLength(20)]
        public string BaskaRejim { get; set; }


        [StringLength(1500)]
        public string Aciklama { get; set; }


        public DateTime? SonIslemZamani { get; set; }

    }

    public class ObOzetBeyanAcmaTasimaSenet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanAcmaBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(20)]
        public string TasimaSenediNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }


    }

    public class ObOzetBeyanAcmaTasimaSatir
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanAcmaBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [StringLength(9)]
        public string AmbarKodu { get; set; }

        public decimal AmbardakiMiktar { get; set; }

        public decimal AcilacakMiktar { get; set; }

        [StringLength(60)]
        public string MarkaNo { get; set; }

        [StringLength(9)]
        public string EsyaCinsi { get; set; }

        [StringLength(9)]
        public string Birim { get; set; }
        public decimal ToplamMiktar { get; set; }
        public decimal KapatilanMiktar { get; set; }

        [StringLength(9)]
        public string OlcuBirimi { get; set; }
        public int AcmaSatirNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }

    public class ObTeminat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }


        [Required]
        [StringLength(9)]
        public string TeminatSekli { get; set; }

        [Required]

        public decimal TeminatOrani { get; set; }


        [StringLength(20)]
        public string GlobalTeminatNo { get; set; }


        public decimal BankaMektubuTutari { get; set; }


        public decimal NakdiTeminatTutari { get; set; }


        public decimal DigerTutar { get; set; }


        [StringLength(20)]
        public string DigerTutarReferansi { get; set; }



        [StringLength(100)]
        public string Aciklama { get; set; }
        public DateTime? SonIslemZamani { get; set; }

    }
    public class ObTasimaSenet
    {


        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(20)]
        public string TasimaSenediNo { get; set; }

        [StringLength(150)]
        public string AcentaAdi { get; set; }

        [StringLength(20)]
        public string AcentaVergiNo { get; set; }

        [StringLength(150)]
        public string AliciAdi { get; set; }

        [StringLength(20)]
        public string AliciVergiNo { get; set; }

        [StringLength(9)]
        public string AmbarHarici { get; set; }

        [StringLength(150)]
        public string BildirimTarafiAdi { get; set; }

        [StringLength(20)]
        public string BildirimTarafiVergiNo { get; set; }

        [StringLength(9)]
        public string DuzenlendigiUlke { get; set; }

        [StringLength(9)]
        public string EmniyetGuvenlik { get; set; }

        [StringLength(16)]
        public string EsyaninBulunduguYer { get; set; }

        [StringLength(9)]
        public string FaturaDoviz { get; set; }
        public decimal FaturaToplami { get; set; }

        [StringLength(150)]
        public string GondericiAdi { get; set; }

        [StringLength(20)]
        public string GondericiVergiNo { get; set; }

        [StringLength(9)]
        public string Grup { get; set; }

        [StringLength(9)]
        public string Konteyner { get; set; }

        [StringLength(9)]
        public string NavlunDoviz { get; set; }
        public decimal NavlunTutari { get; set; }

        [StringLength(9)]
        public string OdemeSekli { get; set; }

        [StringLength(20)]
        public string OncekiSeferNumarasi { get; set; }

        [StringLength(12)]
        public string OncekiSeferTarihi { get; set; }

        [StringLength(20)]
        public string OzetBeyanNo { get; set; }

        [StringLength(9)]
        public string Roro { get; set; }
        public int SenetSiraNo { get; set; }

        [StringLength(9)]
        public string AktarmaYapilacak { get; set; }

        [StringLength(20)]
        public string AktarmaTipi { get; set; }
        public DateTime? SonIslemZamani { get; set; }

    }
    public class ObIhracat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }
        [Required]
        public decimal BrutAgirlik { get; set; }
        [Required]
        public int KapAdet { get; set; }

        [StringLength(20)]
        [Required]
        public string Numara { get; set; }

        [StringLength(9)]
        [Required]
        public string Parcali { get; set; }

        [StringLength(9)]
        public string Tip { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class ObUgrakUlke
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [StringLength(30)]
        public string LimanYerAdi { get; set; }

        [StringLength(9)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class ObTasimaSatir
    {

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [StringLength(30)]
        public string TasimaSatirInternalNo { get; set; }

        public decimal BrutAgirlik { get; set; }
        public int KapAdet { get; set; }

        [StringLength(9)]
        public string KapCinsi { get; set; }

        [StringLength(9)]
        public string KonteynerTipi { get; set; }

        [StringLength(60)]
        public string MarkaNo { get; set; }

        [StringLength(35)]
        public string MuhurNumarasi { get; set; }
        public decimal NetAgirlik { get; set; }

        [StringLength(9)]
        public string OlcuBirimi { get; set; }
        public int SatirNo { get; set; }

        [StringLength(9)]
        public string KonteynerYukDurumu { get; set; }

        [StringLength(9)]
        public DateTime? SonIslemZamani { get; set; }

    }
    public class ObSatirEsya
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSatirInternalNo { get; set; }

        [StringLength(15)]
        public string BmEsyaKodu { get; set; }
        public decimal BrutAgirlik { get; set; }

        [StringLength(12)]
        public string EsyaKodu { get; set; }

        [StringLength(150)]
        public string EsyaninTanimi { get; set; }
        public decimal KalemFiyati { get; set; }

        [StringLength(9)]
        public string KalemFiyatiDoviz { get; set; }
        public int KalemSiraNo { get; set; }
        public decimal NetAgirlik { get; set; }

        [StringLength(9)]
        public string OlcuBirimi { get; set; }
        public DateTime? SonIslemZamani { get; set; }
    }

    public class ObBeyanAlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Tip { get; set; }

        public string OzetBeyanNo { get; set; }

     
        public string BeyanSahibiVergiNo { get; set; }

        public string BeyanTuru { get; set; }

        public string Diger { get; set; }

     
        public string DorseNo1 { get; set; }

    
        public string DorseNo1Uyrugu { get; set; }

    
        public string DorseNo2 { get; set; }

    
        public string DorseNo2Uyrugu { get; set; }

        public string EkBelgeSayisi { get; set; }

      
        public string EmniyetGuvenlik { get; set; }

       
        public string GrupTasimaSenediNo { get; set; }

      
        public string GumrukIdaresi { get; set; }

     
        public string KullaniciKodu { get; set; }

     
        public string Kurye { get; set; }

      
        public string LimanYerAdiBos { get; set; }

      
        public string LimanYerAdiYuk { get; set; }

       
        public string OncekiBeyanNo { get; set; }

      
        public string PlakaSeferNo { get; set; }

     
        public string ReferansNumarasi { get; set; }

     
        public string RefNo { get; set; }

      
        public string Rejim { get; set; }

        public string TasimaSekli { get; set; }

        public string TasitinAdi { get; set; }

     
        public string TasiyiciVergiNo { get; set; }

       
        public string TirAtaKarneNo { get; set; }

      
        public string UlkeKodu { get; set; }

    
        public string UlkeKoduYuk { get; set; }

     
        public string UlkeKoduBos { get; set; }

        
        public string YuklemeBosaltmaYeri { get; set; }

       
        public string VarisCikisGumrukIdaresi { get; set; }

       
        public string VarisTarihSaati { get; set; }

       
        public string XmlRefId { get; set; }

      
        public string TescilStatu { get; set; }

        public string TescilTarihi { get; set; }

     

    }
}


