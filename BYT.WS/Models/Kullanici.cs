using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Entities
{
    public class Kullanici
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [StringLength(15)]
        public string KullaniciKod { get; set; }


        [StringLength(15)]
        public string KullaniciSifre { get; set; }


        [StringLength(30)]
        public string Ad { get; set; }


        [Required]
        [StringLength(30)]
        public string Soyad { get; set; }


        [Required]
        [StringLength(15)]
        public string VergiNo { get; set; }

        [Required]
        [StringLength(150)]
        public string FirmaAd { get; set; }


        [Required]
        [StringLength(150)]
        public string MailAdres { get; set; }

        [Required]
        public bool Aktif { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefon { get; set; }


        [Required]
        public DateTime SonIslemZamani { get; set; }

    }
}
