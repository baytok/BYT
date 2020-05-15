using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Entities;
using BYT.WS.Internal;
using BYT.WS.Models;
using BYT.WS.Services.Kullanici;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.api
{
    //  [Route("api/BYT/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    [ApiController]
    public class KullaniciHizmetiController : ControllerBase
    {
        private readonly IKullaniciServis _userService;
        private KullaniciDataContext _kullaniciContext;
        public IConfiguration Configuration { get; }

        public KullaniciHizmetiController(IConfiguration configuration, IKullaniciServis userService)
        {
            _userService = userService;
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<KullaniciDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _kullaniciContext = new KullaniciDataContext(options);
        }
        [AllowAnonymous]
        [Route("api/BYT/KullaniciGiris/[controller]/{kullanici}/{sifre}")]
        [HttpGet("{kullanici}/{sifre}")]
        public async Task<KullaniciServisDurum> GetGiris(string kullanici, string sifre)
        {

            //if (user == null)
            //    return BadRequest("Username or password incorrect!");
            //return Ok(new { Kullanici = user.Value.KullaniciKod, Token = user.Value.token });

            KullaniciServisDurum _servisDurum = new KullaniciServisDurum();
            try
            {
                var kullaniciValues = _userService.Authenticate(kullanici.Trim(), sifre.Trim());

                if (kullaniciValues != null)
                {

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { GUID = null, IslemTipi = "Giriş", ReferansNo = kullanici, Sonuc = "Giriş Başarılı", SonucVeriler = kullaniciValues };
                    lstBlg.Add(blg);
                    _servisDurum.Bilgiler = lstBlg;
                    _servisDurum.KullaniciBilgileri = kullaniciValues;
                }
                else
                {
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                    List<Internal.Hata> lstht = new List<Internal.Hata>();

                    Hata ht = new Hata { HataKodu = 1, HataAciklamasi = "Giriş İşlemi Başarısız" };
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

                throw ex;
            }


        }


        [Route("api/BYT/KullaniciOlustur/[controller]")]
        [HttpPost]
        public async Task<ServisDurum> PostKullanici([FromBody]Kullanici kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    var _kullaniciValues = await _kullaniciContext.Kullanici.FirstOrDefaultAsync(v => v.KullaniciKod == kullanici.KullaniciKod);
                    if (_kullaniciValues != null)
                    {
                        if (_kullaniciValues.Aktif == false)
                        {
                            _kullaniciValues.Ad = kullanici.Ad;
                            _kullaniciValues.Soyad = kullanici.Soyad;
                            _kullaniciValues.Aktif = true;
                            _kullaniciValues.FirmaAd = kullanici.FirmaAd;
                            _kullaniciValues.KullaniciSifre = kullanici.KullaniciSifre;
                            _kullaniciValues.MailAdres = kullanici.MailAdres;
                            _kullaniciValues.Telefon = kullanici.Telefon;
                            _kullaniciValues.VergiNo = kullanici.VergiNo;
                            _kullaniciValues.SonIslemZamani = DateTime.Now;
                            _kullaniciContext.Entry(_kullaniciValues).State = EntityState.Modified;
                        }
                        else
                        {
                            transaction.Rollback();
                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                            List<Internal.Hata> lstht = new List<Internal.Hata>();

                            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = "Aktif Kullanıcı Bulunmaktadır." };
                            lstht.Add(ht);
                            _servisDurum.Hatalar = lstht;

                            return _servisDurum;
                        }
                    }
                    else
                    {
                        kullanici.SonIslemZamani = DateTime.Now;
                        _kullaniciContext.Entry(kullanici).State = EntityState.Added;
                    }
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

        [Route("api/BYT/KullaniciSil/[controller]/{kullaniciId}")]
        [HttpDelete("{kullaniciId}")]
        public async Task<ServisDurum> DeleteKullanici(int kullaniciId)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    var kullaniciValues = await _kullaniciContext.Kullanici.FirstOrDefaultAsync(v => v.ID == kullaniciId);
                    var kullaniciYetkiValues = await _kullaniciContext.KullaniciYetki.Where(v => v.KullaniciKod == kullaniciValues.KullaniciKod).ToListAsync();

                    _kullaniciContext.Entry(kullaniciValues).State = EntityState.Deleted;
                    foreach (var item in kullaniciYetkiValues)
                    {
                        _kullaniciContext.Entry(item).State = EntityState.Deleted;
                    }
                  
                    await _kullaniciContext.SaveChangesAsync();

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcı Silme", ReferansNo = kullaniciValues.KullaniciKod, Sonuc = "Kullanıcı Silme Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Musteriler/[controller]")]
        [HttpGet]
        public async Task<List<Musteri>> GetMusteri()
        {
            try
            {

                var musteriValues = await _kullaniciContext.Musteri.ToListAsync();

                return musteriValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/AktifMusteriler/[controller]")]
        [HttpGet]
        public async Task<List<Musteri>> GetAktifMusterler()
        {
            try
            {

                var musteriValues = await _kullaniciContext.Musteri.Where(x => x.Aktif == true).ToListAsync();

                return musteriValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/MusteriOlustur/[controller]")]
        [HttpPost]
        public async Task<ServisDurum> PostMusteri([FromBody]Musteri musteri)
        {
            ServisDurum _servisDurum = new ServisDurum();
            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    musteri.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(musteri).State = EntityState.Added;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Müşteri Oluşturma", ReferansNo = musteri.VergiNo, Sonuc = "Müşteri Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/MusteriDegistir/[controller]")]
        [HttpPut]
        public async Task<ServisDurum> PutMusteri([FromBody]Musteri musteri)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    musteri.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(musteri).State = EntityState.Modified;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Müşteri Değişiklik", ReferansNo = musteri.VergiNo, Sonuc = "Müşteri Değişikliği Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/MusteriSil/[controller]/{musteriId}")]
        [HttpDelete("{musteriId}")]
        public async Task<ServisDurum> DeleteMusteri(int musteriId)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    var musteriValues = await _kullaniciContext.Musteri.FirstOrDefaultAsync(v => v.ID == musteriId);

                    _kullaniciContext.Entry(musteriValues).State = EntityState.Deleted;
                    await _kullaniciContext.SaveChangesAsync();

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcı Silme", ReferansNo = musteriValues.VergiNo, Sonuc = "Kullanıcı Silme Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/Yetkiler/[controller]")]
        [HttpGet]
        public async Task<List<Yetki>> GetYetki()
        {
            try
            {

                var yetkiValues = await _kullaniciContext.Yetki.ToListAsync();

                return yetkiValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/AktifYetkiler/[controller]")]
        [HttpGet]
        public async Task<List<Yetki>> GetAktifYetkiler()
        {
            try
            {

                var yetkiValues = await _kullaniciContext.Yetki.Where(x => x.Aktif == true).ToListAsync();

                return yetkiValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/YetkiOlustur/[controller]")]
        [HttpPost]
        public async Task<ServisDurum> PostYetki([FromBody]Yetki yetki)
        {
            ServisDurum _servisDurum = new ServisDurum();
            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    yetki.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(yetki).State = EntityState.Added;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Yetki Oluşturma", ReferansNo = yetki.YetkiAdi, Sonuc = "Yetki Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/YetkiDegistir/[controller]")]
        [HttpPut]
        public async Task<ServisDurum> PutYetki([FromBody]Yetki yetki)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    yetki.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(yetki).State = EntityState.Modified;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Yetki Değişiklik", ReferansNo = yetki.YetkiAdi, Sonuc = "Yetki Değişikliği Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/YetkiSil/[controller]/{yetkiId}")]
        [HttpDelete("{yetkiId}")]
        public async Task<ServisDurum> DeleteYetki(int yetkiId)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    var yetkiValues = await _kullaniciContext.Yetki.FirstOrDefaultAsync(v => v.ID == yetkiId);

                    _kullaniciContext.Entry(yetkiValues).State = EntityState.Deleted;
                    await _kullaniciContext.SaveChangesAsync();

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Yetki Silme", ReferansNo = yetkiValues.YetkiAdi, Sonuc = "Yetki Silme Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/AktifKullaniciYetkiler/[controller]/{kullaniciKod}")]
        [HttpGet("{kullaniciKod}")]
        public async Task<List<KullaniciYetki>> GetAktifKullaniciYetkiler(string kullaniciKod)
        {
            try
            {

                var yetkiValues = await _kullaniciContext.KullaniciYetki.Where(x => x.KullaniciKod == kullaniciKod && x.Aktif == true).ToListAsync();

                return yetkiValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/KullaniciYetkiOlustur/[controller]/{kullaniciKod}")]
        [HttpPost("{kullaniciKod}")]
        public async Task<ServisDurum> PostKullaniciYetki([FromBody]KullaniciYetki[] kullaniciYetki,string kullaniciKod)
        {
            var options = new DbContextOptionsBuilder<KullaniciDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;
            var _kullanici2Context = new KullaniciDataContext(options);
            ServisDurum _servisDurum = new ServisDurum();
            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {

                    var _kullaniciYetkiler = await _kullaniciContext.KullaniciYetki.Where(v => v.KullaniciKod == kullaniciKod).ToListAsync();
                    if (_kullaniciYetkiler.Count > 0)
                    {
                        foreach (var items in _kullaniciYetkiler)
                        {
                            items.Aktif = false;
                            items.SonIslemZamani = DateTime.Now;
                            _kullaniciContext.Entry(items).State = EntityState.Modified;

                        }

                        foreach (var itm in kullaniciYetki)
                        {
                            var _kVr = _kullaniciYetkiler.FirstOrDefault(x => x.YetkiKodu == itm.YetkiKodu);
                            if (_kVr != null)
                            {
                                _kVr.Aktif = true;
                                _kVr.SonIslemZamani = DateTime.Now;
                                _kullaniciContext.Entry(_kVr).State = EntityState.Modified;

                            }
                            else
                            {
                                itm.SonIslemZamani = DateTime.Now;
                                _kullaniciContext.Entry(itm).State = EntityState.Added;

                            }

                        }
                    }
                    else
                    {
                        foreach (var item in kullaniciYetki)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _kullaniciContext.Entry(item).State = EntityState.Added;

                        }
                    }
                    await _kullaniciContext.SaveChangesAsync();

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcı Yetki Oluşturma", ReferansNo = kullaniciKod, Sonuc = "Kullanıcı Yetki Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/KullaniciYetkiDegistir/[controller]")]
        [HttpPut]
        public async Task<ServisDurum> PutKullaniciYetki([FromBody]KullaniciYetki kullaniciYetki)
        {
            ServisDurum _servisDurum = new ServisDurum();

            using (var transaction = _kullaniciContext.Database.BeginTransaction())
            {
                try
                {
                    kullaniciYetki.SonIslemZamani = DateTime.Now;
                    _kullaniciContext.Entry(kullaniciYetki).State = EntityState.Modified;
                    await _kullaniciContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kullanıcı Yetki Değişiklik", ReferansNo = kullaniciYetki.KullaniciKod, Sonuc = "Kullanıcı Yetki Değişikliği Başarılı", SonucVeriler = null };
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



