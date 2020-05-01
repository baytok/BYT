
using BYT.WS.Data;
using BYT.WS.Helpers;
using BYT.WS.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace BYT.WS.Services.Kullanici
{
    public class KullaniciService : IKullaniciServis
    {
       
        public IConfiguration Configuration { get; }
        private readonly AppSettings _appSettings;
        private readonly KullaniciDataContext _kullaniciContext;
        public KullaniciService(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<KullaniciDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _kullaniciContext = new KullaniciDataContext(options);
           
            _appSettings = appSettings.Value;

        }

        public KullaniciBilgi? Authenticate(string KullaniciKod, string KullaniciSifre)
        {
            KullaniciBilgi _kullaniciBilgi = new KullaniciBilgi();
            try
            {
           
            var user = _kullaniciContext.Kullanici.SingleOrDefault(x => x.KullaniciKod == KullaniciKod && x.KullaniciSifre == KullaniciSifre && x.Aktif == true);
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.ID.ToString()),
                    new Claim(ClaimTypes.Name,user.KullaniciKod)

                }),

                //Tokenın hangi tarihe kadar geçerli olacağını ayarlıyoruz.
                Expires = DateTime.UtcNow.AddMinutes(30),
                //Son olarak imza için gerekli algoritma ve gizli anahtar bilgisini belirliyoruz.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            //Token oluşturuyoruz.
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Oluşturduğumuz tokenı string olarak bir değişkene atıyoruz.
            string generatedToken = tokenHandler.WriteToken(token);
         
                //Sonuçlarımızı tuple olarak dönüyoruz.

                var yetkiler = from a in _kullaniciContext.KullaniciYetki
                               join b in _kullaniciContext.Yetki on a.YetkiId equals b.ID
                               where a.Aktif==true && b.Aktif==true && a.KullaniciKod==KullaniciKod
                               select new { b.ID, b.YetkiAdi };

                //   var yetkiler = _kullaniciContext.KullaniciYetki.Where(x => x.KullaniciKod == KullaniciKod  && x.Aktif == true).Select(x=>x.YetkiId).ToArray();
                List<KullaniciYetkileri> lstYetki = new List<KullaniciYetkileri>();
                foreach (var item in yetkiler)
                {
                    KullaniciYetkileri Yetki = new KullaniciYetkileri();
                    Yetki.ID = item.ID;
                    Yetki.YetkiAdi = item.YetkiAdi;
                    lstYetki.Add(Yetki);
                }

                _kullaniciBilgi.KullaniciKod = user.KullaniciKod;
                _kullaniciBilgi.KullaniciAdi= user.Ad + " " + user.Soyad;
                _kullaniciBilgi.Token = generatedToken;
                _kullaniciBilgi.Yetkiler = lstYetki;
                return _kullaniciBilgi;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

    }

}