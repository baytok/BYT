using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class ObOzetBeyanSonuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Guid { get; set; }

        [StringLength(30)]      
        public string IslemInternalNo { get; set; }

        [StringLength(20)]
        public string TescilTarihi { get; set; }

        [StringLength(20)]
        public string TescilNo { get; set; }

        [StringLength(4)]
        public string KalemSayisi { get; set; }

        public int GonderimNo { get; set; }

        [StringLength(20)]
        public string Durum { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
   
    
    public class OzetBeyanXmlSonuc
    {
        public string SonucXml { get; set; }

        public string TescilTarihi { get; set; }

        public string TescilNo { get; set; }

        public List<HataMesaji> Hatalar { get; set; }

        public string KalemSayisi { get; set; }
    }

    public class ObSonucHatalar
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string IslemInternalNo { get; set; }


        [Required]
        [StringLength(50)]
        public string Guid { get; set; }

        [Required]
        public int GonderimNo { get; set; }

        [Required]
        [StringLength(30)]
        public int HataKodu { get; set; }

        [Required]
        [StringLength(1000)]
        public string HataAciklamasi { get; set; }
        public DateTime? SonIslemZamani { get; set; }


    }
}
