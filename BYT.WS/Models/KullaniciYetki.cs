using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class KullaniciYetki
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [StringLength(15)]
        public string KullaniciKod { get; set; }

        [Required]
        [StringLength(15)]
        public string YetkiKodu { get; set; }               
        
       
        [Required]
        public bool Aktif { get; set; }

        public DateTime SonIslemZamani { get; set; }

    }

}
