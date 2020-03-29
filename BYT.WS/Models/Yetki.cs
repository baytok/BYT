using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Yetki
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

     
        [Required]
        [StringLength(50)]
        public string YetkiAdi { get; set; }

        [Required]
        [StringLength(500)]
        public string Aciklama { get; set; }
        
       
        [Required]
        public bool Aktif { get; set; }

        
        [Required]
        public DateTime SonIslemZamani { get; set; }

    }

}
