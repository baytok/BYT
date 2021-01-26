using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Musteri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

      //  [Required]
        [StringLength(20)]
        public string MusteriNo { get; set; }

        [Required]
        [StringLength(15)]
        public string VergiNo { get; set; }

        [Required]
        [StringLength(150)]
        public string MusteriAd { get; set; }
        
        [Required]
        [StringLength(150)]
        public string Adres { get; set; }

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
