using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Models.Dto
{
    public class DbMarka
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(500)]
        public string MarkaAdi { get; set; }

        [Required]
        public decimal MarkaKiymeti { get; set; }


        [StringLength(20)]
        public string MarkaTescilNo { get; set; }

        [Required]
        [StringLength(9)]
        public string MarkaTuru { get; set; }


        [StringLength(30)]
        public string Model { get; set; }


        public int MotorGucu { get; set; }

        [StringLength(30)]
        public string MotorHacmi { get; set; }


        [StringLength(30)]
        public string MotorNo { get; set; }

        [StringLength(20)]
        public string MotorTipi { get; set; }

        [StringLength(30)]
        public string ModelYili { get; set; }


        [StringLength(30)]
        public string Renk { get; set; }


        [StringLength(100)]
        public string ReferansNo { get; set; }


        public int SilindirAdet { get; set; }

        [StringLength(20)]
        public string Vites { get; set; }
    }
}
