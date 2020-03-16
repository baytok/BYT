using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.api
{
    //  [Route("api/BYT/[controller]")]
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
        [Route("api/BYT/KullaniciGiris/[controller]/{Kullanici}/{sifre}")]
        [HttpGet("{Kullanici}/{sifre}")]
        public async Task<ServisDurum> GetGiris(string Kullanici, string sifre)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {
             

                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici.Trim() && v.KullaniciSifre == sifre.Trim() && v.Aktif == true).ToListAsync();

                if (kullaniciValues != null && kullaniciValues.Count > 0)
                {
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Giriş", ReferansNo = Kullanici, Sonuc = "Giriş Başarılı", SonucVeriler = kullaniciValues };
                    lstBlg.Add(blg);
                    _servisDurum.Bilgiler = lstBlg;
                }
                else
                {
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                    List<Internal.Hata> lstht = new List<Internal.Hata>();

                    Hata ht = new Hata { HataKodu = 1, HataAciklamasi = "Giriş İşlemi Başarısız"};
                    lstht.Add(ht);
                    _servisDurum.Hatalar = lstht;
                }
                 


                return _servisDurum;

            }
            catch (Exception ex)
            {

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                List<Internal.Hata> lstht = new List<Internal.Hata>();

                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                lstht.Add(ht);
                _servisDurum.Hatalar = lstht;
                return _servisDurum;
            }


        }

        [Route("api/BYT/Kullanicilar/[controller]")]
        [HttpGet]
        public async Task<List<Kullanici>> GetKullanici()
        {
            try
            {

                var kullaniciValues = await _kullaniciContext.Kullanici.ToListAsync();

                return kullaniciValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/KullaniciOlustur/[controller]")]
        [HttpPost]
        public async Task<ServisDurum> PosttKullanici([FromBody]Kullanici kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    kullanici.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(kullanici).State = EntityState.Added;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcı Oluşturma", ReferansNo = kullanici.KullaniciKod, Sonuc = "Kullanıcı Oluşturma Başarılı", SonucVeriler = null };
                    lstBlg.Add(blg);
                    _servisDurum.Bilgiler = lstBlg;


                    return _servisDurum;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                    List<Internal.Hata> lstht = new List<Internal.Hata>();

                    Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                    lstht.Add(ht);
                    _servisDurum.Hatalar = lstht;

                    return _servisDurum;

                }

            }


        }

        [Route("api/BYT/KullaniciDegistir/[controller]")]
        [HttpPut]
        public async Task<ServisDurum> PutKullanici([FromBody]Kullanici kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    kullanici.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(kullanici).State = EntityState.Modified;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcıda Değişiklik", ReferansNo = kullanici.KullaniciKod, Sonuc = "Kullanıcı Değişikliği Başarılı", SonucVeriler = null };
                    lstBlg.Add(blg);
                    _servisDurum.Bilgiler = lstBlg;


                    return _servisDurum;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                    List<Internal.Hata> lstht = new List<Internal.Hata>();

                    Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                    lstht.Add(ht);
                    _servisDurum.Hatalar = lstht;

                    return _servisDurum;
                }

            }

        }
    }
}
   
       

   