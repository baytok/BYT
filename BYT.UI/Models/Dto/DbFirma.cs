using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Models.Dto
{
    public class DbFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }



        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

        [Required]
        [StringLength(15)]
        public string Faks { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }

        [Required]
        [StringLength(9)]
        public string KimlikTuru { get; set; }

        [Required]
        [StringLength(20)]
        public string No { get; set; }


        [StringLength(10)]
        public string PostaKodu { get; set; }


        [StringLength(15)]
        public string Telefon { get; set; }

        [Required]
        [StringLength(9)]
        public string Tip { get; set; }

        [Required]
        [StringLength(9)]
        public string UlkeKodu { get; set; }

    }
}
