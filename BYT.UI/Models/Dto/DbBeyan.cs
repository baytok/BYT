using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Models.Dto
{
    public class DbBeyan
    {
        [Required]
        [StringLength(30)]
        public string RefId { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BeyanInternalNo { get; set; }

      
        [StringLength(20)]
        public string BeyannameNo { get; set; }



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

        [StringLength(16)]
        public string BankaKodu { get; set; }
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
        public string ReferansNo { get; set; }

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

        public DateTime TescilTarihi { get; set; }

     
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

     

    }
}
