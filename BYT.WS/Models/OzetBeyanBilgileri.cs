using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class OzetBeyanBilgileri
    { }
    public class ObBeyan
    {

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OzetBeyanInternalNo { get; set; }

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
        public int KapAdedi { get; set; }

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
    
  
}


