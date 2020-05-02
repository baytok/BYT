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
        public string BeyanInternalNo { get; set; }

        [StringLength(20)]
        public string? OzetBeyanNo { get; set; }
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
        public string TasimaSekli { get; set; }

        public string TasitinAdi { get; set; }

        public string TasiyiciFirma { get; set; }

        public string TasiyiciVergiNo { get; set; }

        public string TirAtaKarneNo { get; set; }
        public string UlkeKodu { get; set; }
        public string UlkeKoduYuk { get; set; }
        public string UlkeKoduBos { get; set; }
        public string YuklemeBosaltmaYeri { get; set; }
        public string VarisCikisGumrukIdaresi { get; set; }
        public DateTime? VarisTarihSaati { get; set; }
        public string XmlRefId { get; set; }

        public DateTime? OlsuturulmaTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }
    public class ObOzetBeyanAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

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

        [StringLength(30)]
        public string DahiliNoAcma { get; set; }
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
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(50)]
        public string TasimaSenediNo { get; set; }

        [Required]
        [StringLength(50)]
        public string DahiliNoAcilanSenet { get; set; }

        public DateTime? SonIslemZamani { get; set; }

                     
    }

    public class ObOzetBeyanAcmaTasimaSatir
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        public string AmbarKodu { get; set; }

        public double AmbardakiMiktar { get; set; }

        public double AcilacakMiktar { get; set; }

        public string MarkaNo { get; set; }

        public string EsyaCinsi { get; set; }

        public string Birim { get; set; }
        public double ToplamMiktar { get; set; }
        public double KapatilanMiktar { get; set; }
        public string OlcuBirimi { get; set; }
        public int AcmaSatirNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }

    public class ObTasimaSenet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(50)]
        public string TasimaSenediNo { get; set; }

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
        public double FaturaToplami { get; set; }
        public string GondericiAdi { get; set; }

        public string GondericiVergiNo { get; set; }
        public string GrupMu { get; set; }
        public string KonteynerMi { get; set; }
        public string NavlunDoviz { get; set; }
        public double NavlunTutari { get; set; }
        public string OdemeSekli { get; set; }
        public string OncekiSeferNumarasi { get; set; }
        public DateTime OncekiSeferTarihi { get; set; }
        public string OzetBeyanNo { get; set; }
        public string RoroMu { get; set; }
        public string SenetSiraNo { get; set; }
        public string AktarmaYapilacakMi { get; set; }
        public string AktarmaTipi { get; set; }
        public DateTime? SonIslemZamani { get; set; }

   
    }

    public class ObTasimaSatir
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        public double BrutAgirlik { get; set; }
        public int KapAdedi { get; set; }

        public string KapCinsi { get; set; }
        public string KonteynerTipi { get; set; }
        public string MarkaNo { get; set; }
        public string MuhurNumarasi { get; set; }
        public double NetAgirlik { get; set; }
        public string OlcuBirimi { get; set; }
        public int SatirNo { get; set; }
        public string KonteynerYukDurumu { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }

    public class ObTeminat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }


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

    public class ObUgrakUlke
    {
        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }
        public string LimanYerAdi { get; set; }
        public string UlkeKodu { get; set; }
       
        public DateTime? SonIslemZamani { get; set; }
    }
    public class ObIhracat
    {
        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        public double BrutAgirlik { get; set; }
        public int KapAdedi { get; set; }
        public string Numarasi { get; set; }
        public string ParcaliMi { get; set; }
        public string Tipi { get; set; }

                       public DateTime? SonIslemZamani { get; set; }
    }

    public class ObSatirEsya
    {
        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }
        public string BmEsyaKodu { get; set; }
        public double BrutAgirlik { get; set; }
        public string EsyaKodu { get; set; }
        public string EsyaninTanimi { get; set; }
        public double KalemFiyati { get; set; }
        public string KalemFiyatiDoviz { get; set; }
        public int KalemSiraNo { get; set; }
        public double NetAgirlik { get; set; }

        public string OlcuBirimi { get; set; }
        public DateTime? SonIslemZamani { get; set; }
    }

    public class ObTasitUgrakUlke
    {
        [Required]
        [StringLength(30)]
        public DateTime OzetBeyanInternalNo { get; set; }
        public string HareketTarihSaati { get; set; }
        public string LimanYerAdi { get; set; }
        public string UlkeKodu { get; set; }
        public DateTime? SonIslemZamani { get; set; }
    }
}


