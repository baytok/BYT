using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Islem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string RefNo { get; set; }

        [Required]
        [StringLength(15)]
        public string Kullanici { get; set; }

      
        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }

        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

    
        [StringLength(30)]
        public string BeyanNo { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanTipi { get; set; }
    

        [Required]
        [StringLength(10)]
        public string IslemTipi { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemDurumu { get; set; }

       
        [StringLength(500)]
        public string IslemSonucu { get; set; }

        public DateTime OlusturmaZamani { get; set; }

        public DateTime IslemZamani { get; set; }

        [StringLength(50)]
        public string Guidof { get; set; }

        public int GonderimSayisi { get; set; }

        

    }

}
