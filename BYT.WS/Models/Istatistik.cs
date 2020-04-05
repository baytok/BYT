using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    public class Istatistik
    {    
      
        public int BeyannameSayisi { get; set; }
        public int TescilBeyannameSayisi { get; set; }
        public int KontrolGonderimSayisi { get; set; }
        public int TescilGonderimSayisi { get; set; }
        public int SonucBeklenenSayisi { get; set; }
    }

}
