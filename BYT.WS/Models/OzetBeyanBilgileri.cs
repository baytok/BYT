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

        [StringLength(30)]
        public string RefNo { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BeyanInternalNo { get; set; }


        [StringLength(20)]
        public string? BeyannameNo { get; set; }



        [StringLength(350)]
        public string Aciklamalar { get; set; }

        [StringLength(9)]
        public string AliciSaticiIliskisi { get; set; }

        [StringLength(20)]
        public string AliciVergiNo { get; set; }

        [StringLength(9)]
        public string AntrepoKodu { get; set; }

        [StringLength(20)]
        public string AsilSorumluVergiNo { get; set; }

        [StringLength(9)]
        public string BasitlestirilmisUsul { get; set; }

        [Required]
        [StringLength(16)]
        public string BankaKodu { get; set; }
        [Required]
        [StringLength(20)]
        public string BeyanSahibiVergiNo { get; set; }

        [StringLength(30)]
        public string BirlikKriptoNumarasi { get; set; }

        [StringLength(30)]
        public string BirlikKayitNumarasi { get; set; }

        [StringLength(9)]
        public string CikisUlkesi { get; set; }

        [StringLength(35)]
        public string CikistakiAracinKimligi { get; set; }

        [StringLength(9)]
        public string CikistakiAracinTipi { get; set; }

        [StringLength(9)]
        public string CikistakiAracinUlkesi { get; set; }

        [StringLength(40)]
        public string EsyaninBulunduguYer { get; set; }

        [StringLength(9)]
        public string GidecegiSevkUlkesi { get; set; }

        [StringLength(9)]
        public string GidecegiUlke { get; set; }

        [StringLength(9)]
        public string GirisGumrukIdaresi { get; set; }


        [StringLength(20)]
        public string GondericiVergiNo { get; set; }

        [Required]
        [StringLength(9)]
        public string Gumruk { get; set; }

        [StringLength(9)]
        public string IsleminNiteligi { get; set; }

        public int KapAdedi { get; set; }

        [StringLength(9)]
        public string Konteyner { get; set; }

        [Required]
        [StringLength(15)]
        public string Kullanici { get; set; }

        [StringLength(9)]
        public string LimanKodu { get; set; }

        [StringLength(50)]
        public string Mail1 { get; set; }

        [StringLength(50)]
        public string Mail2 { get; set; }

        [StringLength(50)]
        public string Mail3 { get; set; }

        [StringLength(30)]
        public string Mobil1 { get; set; }

        [StringLength(30)]
        public string Mobil2 { get; set; }

        [StringLength(20)]
        public string MusavirVergiNo { get; set; }

        [StringLength(9)]
        public string OdemeAraci { get; set; }


        [StringLength(12)]
        public string MusavirReferansNo { get; set; }

        [StringLength(12)]
        public string ReferansTarihi { get; set; }

        [Required]
        [StringLength(9)]
        public string Rejim { get; set; }

        [StringLength(35)]
        public string SinirdakiAracinKimligi { get; set; }

        [StringLength(9)]
        public string SinirdakiAracinTipi { get; set; }

        [StringLength(9)]
        public string SinirdakiAracinUlkesi { get; set; }

        [StringLength(9)]
        public string SinirdakiTasimaSekli { get; set; }

        [StringLength(250)]
        public string TasarlananGuzergah { get; set; }

        public decimal TelafiEdiciVergi { get; set; }

        [StringLength(50)]
        public string TescilStatu { get; set; }

        public DateTime? TescilTarihi { get; set; }


        [StringLength(9)]
        public string TeslimSekli { get; set; }

        [StringLength(40)]
        public string TeslimSekliYeri { get; set; }

        [StringLength(9)]
        public string TicaretUlkesi { get; set; }

        public decimal ToplamFatura { get; set; }

        [StringLength(9)]
        public string ToplamFaturaDovizi { get; set; }

        public decimal ToplamNavlun { get; set; }

        [StringLength(9)]
        public string ToplamNavlunDovizi { get; set; }

        public decimal ToplamSigorta { get; set; }

        [StringLength(9)]
        public string ToplamSigortaDovizi { get; set; }

        public decimal ToplamYurtDisiHarcamalar { get; set; }

        [StringLength(9)]
        public string ToplamYurtDisiHarcamalarDovizi { get; set; }
        public decimal ToplamYurtIciHarcamalar { get; set; }

        [StringLength(9)]
        public string VarisGumrukIdaresi { get; set; }

        public int YukBelgeleriSayisi { get; set; }

        [StringLength(40)]
        public string YuklemeBosaltmaYeri { get; set; }

        public DateTime? OlsuturulmaTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }
        public class ObOzetbeyanAcma
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


            [StringLength(9)]
            public string AmbarKodu { get; set; }

            [Required]
            public decimal Miktar { get; set; }

            [Required]
            [StringLength(9)]
            public string TasimaSatirNo { get; set; }

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
    }

