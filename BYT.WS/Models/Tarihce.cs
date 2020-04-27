using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Tarihce
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [StringLength(30)]
        public string RefNo { get; set; }

       
        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(15)]
        public string Kullanici { get; set; }

      
        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        public int GonderimNo { get; set; }
        
        [StringLength(30)]
        public string BeyanNo { get; set; }

        [Required]
        [StringLength(10)]
        public string TicaretTipi { get; set; }

        [Required]
        [StringLength(10)]
        public string IslemTipi { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemDurumu { get; set; }

        
        [StringLength(500)]
        public string IslemSonucu { get; set; }

        [Required]
        [StringLength(10)]
        public string Gumruk { get; set; }

        [Required]
        [StringLength(10)]
        public string Rejim { get; set; }

        [StringLength(int.MaxValue)]
        public string GonderilecekVeri { get; set; }

        public DateTime OlusturmaZamani { get; set; }


        [StringLength(int.MaxValue)]
        public string GonderilenVeri { get; set; }

        public DateTime GondermeZamani { get; set; }


        [StringLength(int.MaxValue)]
        public string SonucVeri { get; set; }

        public DateTime SonucZamani { get; set; }


        [StringLength(int.MaxValue)]
        public string ServistekiVeri { get; set; }

        [StringLength(int.MaxValue)]
        public string ImzaliVeri { get; set; }
        


    }

}
