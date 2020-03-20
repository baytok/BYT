using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Services.Kullanici
{
    public interface IKullaniciServis
    {
       (string KullaniciKod, string token)? Authenticate(string KullaniciKod, string KullaniciSifre);

    }
}
