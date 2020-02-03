using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciHizmetiController : ControllerBase
    {
       
        private KullaniciDataContext _kullaniciContext;
       public IConfiguration Configuration { get; }
      
        public KullaniciHizmetiController(IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<KullaniciDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _kullaniciContext = new KullaniciDataContext(options);
        }

        [HttpGet]
        public async Task<Sonuc<ServisDurum>> GetKullanici()
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var kullaniciValues = await _kullaniciContext.Kullanici.ToListAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = "", Sonuc = "Sorgulama Başarılı", SonucVeriler = kullaniciValues };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
              

                return result;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [HttpGet("{Kullanici}")]
        public async Task<Sonuc<ServisDurum>> GetKullanici(string Kullanici)
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).ToListAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = Kullanici, Sonuc = "Sorgulama Başarılı", SonucVeriler = kullaniciValues };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };


                return result;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

       
    }

   
       

       
}