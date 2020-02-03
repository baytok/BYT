using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Models.Dto
{
    public class DbOdemeSekli
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
        [StringLength(2)]
        public string OdemeSekliKodu { get; set; }

        [Required]

        public decimal OdemeTutari { get; set; }

        [Required]
        [StringLength(30)]
        public string TBFID { get; set; }


    }
}
