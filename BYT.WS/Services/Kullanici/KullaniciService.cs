
using BYT.WS.Data;
using BYT.WS.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
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

        public (string KullaniciKod, string token, string kullaniciAdi)? Authenticate(string KullaniciKod, string KullaniciSifre)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
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
            string kullaniciAdi = user.Ad + " " + user.Soyad;
            //Sonuçlarımızı tuple olarak dönüyoruz.

            return (user.KullaniciKod, generatedToken, kullaniciAdi);

        }

    }

}