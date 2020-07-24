using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace BYT.WS.Controllers.api
{
   
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IslemHizmetiController : ControllerBase
    {
        private IslemTarihceDataContext _islemContext;

        private readonly ServisCredential _servisCredential;

        public ILogger<IslemHizmetiController> _logger;
        public IslemHizmetiController(IslemTarihceDataContext islemcontext, IOptions<ServisCredential> servisCredential, ILogger<IslemHizmetiController> logger)
        {
            _islemContext = islemcontext;
            _logger = logger;
            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/[controller]")]
        [HttpGet]
        public async Task<Sonuc<ServisDurum>> Get()
        {
            ServisDurum _servisDurum = new ServisDurum();
            List<Bilgi> lstBlg = new List<Bilgi>();
            try
            {

                _logger.LogInformation("Information mesajı...");
                var islemValues = await _islemContext.Islem.ToListAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

               
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = "", Sonuc = "Sorgulama Başarılı", SonucVeriler = islemValues };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;

            }
            catch (Exception ex)
            {
                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeklenmeyenHata;
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = "", Sonuc = "Sorgulama Başarısız", SonucVeriler = ex.ToString() };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;
                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirilemedi" };
                return result;
            }

           
        }
        [Route("api/BYT/[controller]/KullaniciIleSorgulama/{Kullanici}")]
        [HttpGet("{Kullanici}")]
        public async Task<List<Islem>> GetIslemlerFromKullanici(string Kullanici)
        {
        

            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var islemValues =  await _islemContext.Islem.Where(v => v.Kullanici == Kullanici.Trim()).ToListAsync();
                //var result = new Sonuc<object>() { Veri = islemValues, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };

                var Message = $"GetIslemlerFromKullanici {DateTime.UtcNow.ToLongTimeString()}";
                Log.Information("Message displayed: {Message}", Message);
               _logger.LogInformation("Message displayed: {Message}", Message);
                return islemValues;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error mesajı...");
                return null;
            }


        }
        [Route("api/BYT/[controller]/RefNoIleSorgulama/{refNo}")]
        [HttpGet("{refId}")]
        public async Task<List<Islem>> GetIslemlerFromRefId(string refNo)
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var islemValues = await _islemContext.Islem.Where(v => v.RefNo == refNo.Trim()).ToListAsync();

                //_servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                //List<Bilgi> lstBlg = new List<Bilgi>();
                //Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = refId, Sonuc = "Sorgulama Başarılı", SonucVeriler = islemValues };
                //lstBlg.Add(blg);
                //_servisDurum.Bilgiler = lstBlg;

                //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return islemValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemOlustur/{Kullanici}")]
        [HttpPut("{Kullanici}")]
        public async Task<Sonuc<ServisDurum>> PutIslem(string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {
                Islem _islem = new Islem();
                _islem.Kullanici = Kullanici;
                _islem.IslemTipi = "Kontrol";
                _islem.BeyanTipi = "DetayliBeyan";
                _islem.IslemDurumu = "Olusturuldu";
                _islem.RefNo = "000002";
                _islem.IslemInternalNo = Kullanici+"G"+"000002"; //TODO: sequence sayı alıp 6 ya tamamlayalım
                _islem.OlusturmaZamani = DateTime.Now;
                _islem.SonIslemZamani = DateTime.Now;
                _islem.GonderimSayisi=0;

                _islemContext.Entry(_islem).State = EntityState.Added;
                await _islemContext.SaveChangesAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = Kullanici, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemOlustur")]
        [HttpPut()]
        public async Task<Sonuc<ServisDurum>> Put(Islem _islem)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {
               
                _islem.SonIslemZamani = DateTime.Now;
                _islemContext.Entry(_islem).State = EntityState.Added;
                await _islemContext.SaveChangesAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = _islem.Kullanici, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemListesi/{KullaniciKod}")]
        [HttpGet("{KullaniciKod}")]
        public async Task<List<Islem>> GetIslemList(string KullaniciKod)
        {
            try
            {

                var resultIslem = await _islemContext.Islem.Where(x => x.Kullanici == KullaniciKod.Trim() && x.Guidof!="").ToListAsync();

                return resultIslem;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemDatay/{Guid}")]
        [HttpGet("{Guid}")]
        public async Task<Tarihce> GetIslemDeail(string Guid)
        {
            try
            {

                var resultIslem = await _islemContext.Tarihce.Where(x => x.Guid == Guid).FirstOrDefaultAsync();


                return resultIslem;

            }
            catch (Exception ex)
            {

                return null;
            }


        }
        [Route("api/BYT/[controller]/DetayGuncelle/{Guid}")]
        [HttpPost("{Guid}")]
        public async Task<ServisDurum> PostDeail([FromBody] string imzaliVeri, string Guid)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();
            try
            {

                var resultTarihce = await _islemContext.Tarihce.Where(x => x.Guid == Guid).FirstOrDefaultAsync();
                resultTarihce.ImzaliVeri = imzaliVeri;
                resultTarihce.SonIslemZamani = DateTime.Now;
                resultTarihce.IslemDurumu = "Imzalandi";
                resultTarihce.IslemSonucu = "İmzalama Başarılı";
                _islemContext.Entry(resultTarihce).State = EntityState.Modified;

                var resultIslem = await _islemContext.Islem.Where(x => x.Guidof == Guid).FirstOrDefaultAsync();
                resultIslem.SonIslemZamani = DateTime.Now;
                resultIslem.IslemZamani = DateTime.Now;
                resultIslem.IslemDurumu = "Imzalandi";
                resultIslem.IslemSonucu = "İmzalama Başarılı";
                _islemContext.Entry(resultIslem).State = EntityState.Modified;

                await _islemContext.SaveChangesAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = Guid, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                return _servisDurum;

            }
            catch (Exception ex)
            {

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                List<Internal.Hata> lstht = new List<Internal.Hata>();

                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                lstht.Add(ht);
                _servisDurum.Hatalar = lstht;
                // var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                return _servisDurum;
            }


        }


    }





}