using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Mesai
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string MesaiInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string MesaiID { get; set; }

        [StringLength(30)]
        public string TescilStatu { get; set; }

        [Required]
        public int AracAdedi { get; set; }

        [Required]
        [StringLength(9)]
        public string GumrukKodu { get; set; }


        [StringLength(150)]
        public string Adres { get; set; }

        [Required]
        [StringLength(20)]
        public string BeyannameNo { get; set; }


        [StringLength(30)]
        public string DigerNo { get; set; }

        [Required]
        [StringLength(100)]
        public string EsyaninBulunduguYer { get; set; }

       
        [StringLength(50)]
        public string EsyaninBulunduguYerAdi { get; set; }

        
        [StringLength(30)]
        public string EsyaninBulunduguYerKodu { get; set; }

        [Required]
        [StringLength(15)]
        public string FirmaVergiNo { get; set; }

        [StringLength(15)]
        public string GlobalHesaptanOdeme { get; set; }

        [StringLength(15)]
        public string GumrukSahasinda { get; set; }

        [StringLength(50)]
        public string IrtibatAdSoyad { get; set; }

        [StringLength(20)]
        public string IrtibatTelefonNo { get; set; }

        [StringLength(20)]
        public string IslemTipi { get; set; }

        [StringLength(20)]
        public string OdemeYapacakFirmaVergiNo { get; set; }
       
        public int NCTSSayisi { get; set; }

        public int OZBYSayisi { get; set; }
        public int Uzaklik { get; set; }

        public DateTime BaslangicZamani { get; set; }
        public DateTime? SonIslemZamani { get; set; }
    }


}


