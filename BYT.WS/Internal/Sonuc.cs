using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Internal
{
    public class Sonuc<T>
    {
        public T Veri { get; set; }
        public string Mesaj { get; set; }
        public bool Islem { get; set; }

    }
}
