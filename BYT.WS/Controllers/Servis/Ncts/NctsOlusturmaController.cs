using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.Controllers.api;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace BYT.WS.Controllers.Servis.Ncts
{

    //[Route("api/BYT/Servis/NctsBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NctsOlusturmaController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;
        private NctsDataContext _beyannameContext;
        public IConfiguration Configuration { get; }

        public NctsOlusturmaController(IslemTarihceDataContext islemTarihceContext, IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _beyannameContext = new NctsDataContext(options);
            _islemTarihceContext = islemTarihceContext;

        }

        [Route("api/BYT/Servis/NctsBeyan/[controller]/BeyannameOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PostBeyanname([FromBody]NbBeyan beyan)
        {
            var options = new DbContextOptionsBuilder<NctsDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;
            var options2 = new DbContextOptionsBuilder<KullaniciDataContext>()
           .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
           .Options;
            KullaniciDataContext _kullaniciContext = new KullaniciDataContext(options2);
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            //DbBeyanModelValidator vbValidator = new DbBeyanModelValidator();
            //ValidationResult validationResult = new ValidationResult();
            //validationResult = vbValidator.Validate(beyan);

            //if (!validationResult.IsValid)
            //{

            //    Hata ht = new Hata();
            //    for (int i = 0; i < validationResult.Errors.Count; i++)
            //    {
            //        ht = new Hata();
            //        ht.HataKodu = (i + 1);
            //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
            //        _hatalar.Add(ht);
            //    }

            //    _servisDurum.Hatalar = _hatalar;

            //    //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
            //    return _servisDurum;
            //}
            Islem _islem = new Islem();
            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {

                        var nctsBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == beyan.NctsBeyanInternalNo && v.TescilStatu != "Tescil Edildi");
                        var beyannameContext = new NctsDataContext(options);
                        if (nctsBeyanValues != null)
                        {
                            _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.NctsBeyanInternalNo);

                            beyan.TescilStatu = "Guncellendi";
                            beyan.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(beyan).State = EntityState.Modified;
                            await beyannameContext.SaveChangesAsync();


                            _islem.IslemDurumu = "Guncellendi";
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();

                        }

                        else
                        {
                            var kullanici = _kullaniciContext.Kullanici.Where(x => x.KullaniciKod == beyan.Kullanici).FirstOrDefault();
                            var internalrefid = beyannameContext.GetRefIdNextSequenceValue(beyan.Rejim);
                            string InternalNo = beyan.Rejim + beyan.Kullanici + "NB" + internalrefid.ToString().PadLeft(5, '0');

                            beyan.MusteriNo = kullanici.MusteriNo;
                            beyan.FirmaNo = kullanici.FirmaNo;
                            beyan.NctsBeyanInternalNo = InternalNo;
                            beyan.RefNo = InternalNo;
                            beyan.TescilStatu = "Olusturuldu";
                            beyan.OlsuturulmaTarihi = DateTime.Now;
                            beyan.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(beyan).State = EntityState.Added;
                            await beyannameContext.SaveChangesAsync();


                            _islem.Kullanici = beyan.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "Ncts";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = beyan.RefNo;
                            _islem.BeyanInternalNo = beyan.NctsBeyanInternalNo;
                            _islem.IslemInternalNo = beyan.NctsBeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        // var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }


                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = _islem.IslemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                var Message = $"PostBeyanname {DateTime.UtcNow.ToLongTimeString()}";
                Log.Information("Message displayed: {Message}/{Gelen}/{Sonuç}", Message, JsonConvert.SerializeObject(beyan),JsonConvert.SerializeObject(_servisDurum));

                return _servisDurum;
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/Servis/NctsBeyan/[controller]/BeyanSahibiOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostBeyanSahibi([FromBody] NbBeyanSahibi beyanList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbBeyanSahibi.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        if (!string.IsNullOrEmpty(beyanList.AdUnvan) || !string.IsNullOrEmpty(beyanList.CaddeSokakNo) || !string.IsNullOrEmpty(beyanList.IlIlce) || !string.IsNullOrEmpty(beyanList.No) )
                        {
                            beyanList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(beyanList).State = EntityState.Added;
                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Beyan Sahibi Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Beyan Sahibi Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/TasiyiciFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostTasiyiciFirma([FromBody] NbTasiyiciFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbTasiyiciFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Taşıyıcı Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Taşıyıcı Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/AsilSorumluFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostAsilSorumluFirma([FromBody] NbAsilSorumluFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbAsilSorumluFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (firmaList.AdUnvan != "")
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }


                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Asıl Sorumlu Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Asıl Sorumlu Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/GondericiFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostGondericiFirma([FromBody] NbGondericiFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbGondericiFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (firmaList.AdUnvan != "")
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }


                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Gönderici Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Gönderici Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/AliciFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostAliciFirma([FromBody] NbAliciFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbAliciFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (firmaList.AdUnvan != "")
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }


                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Alıcı Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Alıcı Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/GuvenliGondericiFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostGuvenliGondericiFirma([FromBody] NbGuvenliGondericiFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbGuvenliGondericiFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }


                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Güvenli Gönderici Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Güvenli Gönderici Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/GuvenliAliciFirmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostGuvenliAliciFirma([FromBody] NbGuvenliAliciFirma firmaList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.NbGuvenliAliciFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (!string.IsNullOrEmpty(firmaList.AdUnvan)  || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Güvenli Alıcı Firma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Güvenli Alıcı Firma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/TeminatOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostTeminat([FromBody] NbTeminat[] teminatBilgiList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
            //ValidationResult validationResult = new ValidationResult();
            //validationResult = vbValidator.Validate(odeme);

            //if (!validationResult.IsValid)
            //{

            //    Hata ht = new Hata();
            //    for (int i = 0; i < validationResult.Errors.Count; i++)
            //    {
            //        ht = new Hata();
            //        ht.HataKodu = (i + 1);
            //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
            //        _hatalar.Add(ht);
            //    }

            //    _servisDurum.Hatalar = _hatalar;

            //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
            //    return result;
            //}

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var teminatValues = await _beyannameContext.NbTeminat.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in teminatValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in teminatBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Teminat Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Teminat Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/MuhurOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostMuhur([FromBody] NbMuhur[] muhurBilgiList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
            //ValidationResult validationResult = new ValidationResult();
            //validationResult = vbValidator.Validate(odeme);

            //if (!validationResult.IsValid)
            //{

            //    Hata ht = new Hata();
            //    for (int i = 0; i < validationResult.Errors.Count; i++)
            //    {
            //        ht = new Hata();
            //        ht.HataKodu = (i + 1);
            //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
            //        _hatalar.Add(ht);
            //    }

            //    _servisDurum.Hatalar = _hatalar;

            //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
            //    return result;
            //}

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var muhurValues = await _beyannameContext.NbMuhur.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in muhurValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in muhurBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Mühür Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Mühür Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/RotaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostRota([FromBody] NbRota[] rotaBilgiList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
            //ValidationResult validationResult = new ValidationResult();
            //validationResult = vbValidator.Validate(odeme);

            //if (!validationResult.IsValid)
            //{

            //    Hata ht = new Hata();
            //    for (int i = 0; i < validationResult.Errors.Count; i++)
            //    {
            //        ht = new Hata();
            //        ht.HataKodu = (i + 1);
            //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
            //        _hatalar.Add(ht);
            //    }

            //    _servisDurum.Hatalar = _hatalar;

            //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
            //    return result;
            //}

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var rotaValues = await _beyannameContext.NbRota.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in rotaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in rotaBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Rota Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Rota Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/TransitGumrukOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostTransitGumruk([FromBody] NbTransitGumruk[] transitGumrukBilgiList, string NctsBeyanInternalNo)

        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
            //ValidationResult validationResult = new ValidationResult();
            //validationResult = vbValidator.Validate(odeme);

            //if (!validationResult.IsValid)
            //{

            //    Hata ht = new Hata();
            //    for (int i = 0; i < validationResult.Errors.Count; i++)
            //    {
            //        ht = new Hata();
            //        ht.HataKodu = (i + 1);
            //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
            //        _hatalar.Add(ht);
            //    }

            //    _servisDurum.Hatalar = _hatalar;

            //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
            //    return result;
            //}

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var transitValues = await _beyannameContext.NbTransitGumruk.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in transitValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in transitGumrukBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Transit Gümrük Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Transit Gümrük Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemSil/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpDelete("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> DeleteKalem(string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
               .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
               .Options;
            var _beyannameContext = new NctsDataContext(options);

            using (var transaction = _beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    var kalemValues = await _beyannameContext.NbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo);
                    if (kalemValues != null)
                    {
                        _beyannameContext.Entry(kalemValues).State = EntityState.Deleted;

                        var updateKalemNo = from u in _beyannameContext.NbKalem
                                            where u.NctsBeyanInternalNo == NctsBeyanInternalNo && u.KalemSiraNo > kalemValues.KalemSiraNo
                                            select u;

                        foreach (NbKalem itm in updateKalemNo)
                        {
                            itm.KalemSiraNo = itm.KalemSiraNo - 1;

                        }

                        var kalemAliciValues = await _beyannameContext.NbKalemAliciFirma.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (kalemAliciValues.Count > 0)
                            foreach (var item in kalemAliciValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var kalemGondericiValues = await _beyannameContext.NbKalemGondericiFirma.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (kalemGondericiValues.Count > 0)
                            foreach (var item in kalemGondericiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                        var kalemGuvenliAliciValues = await _beyannameContext.NbKalemGuvenliAliciFirma.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (kalemGuvenliAliciValues.Count > 0)
                            foreach (var item in kalemGuvenliAliciValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var kalemGuvenliGondericiValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (kalemGuvenliGondericiValues.Count > 0)
                            foreach (var item in kalemGuvenliGondericiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                        var konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (konteynerValues.Count > 0)
                            foreach (var item in konteynerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var hassasEsyaValues = await _beyannameContext.NbHassasEsya.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (hassasEsyaValues.Count > 0)
                            foreach (var item in hassasEsyaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var kapValues = await _beyannameContext.NbKap.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (kapValues.Count > 0)
                            foreach (var item in kapValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var ekbilgiValues = await _beyannameContext.NbEkBilgi.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (ekbilgiValues.Count > 0)
                            foreach (var item in ekbilgiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (belgelerValues.Count > 0)
                            foreach (var item in belgelerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var oncekiBelgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.KalemInternalNo == KalemInternalNo && v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();
                        if (oncekiBelgelerValues.Count > 0)
                            foreach (var item in oncekiBelgelerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        await _beyannameContext.SaveChangesAsync();
                        NctsBeyanKalemKapKiloGuncelle(kalemValues.NctsBeyanInternalNo);
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Kalem Silme", ReferansNo = kalemValues.KalemInternalNo, Sonuc = "Kalem Silme Başarılı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;


                        return _servisDurum;
                    }
                    else
                    {
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Kalem Silme", ReferansNo = kalemValues.KalemInternalNo, Sonuc = "Kalem Silme Başarısız", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;


                        return _servisDurum;
                    }
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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PutKalem([FromBody] NbKalem kalem)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;

            var kalemValues = await _beyannameContext.NbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == kalem.KalemInternalNo && v.NctsBeyanInternalNo == kalem.NctsBeyanInternalNo);
            var beyannameContext = new NctsDataContext(options);
            using (var transaction = beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    if (kalemValues != null)
                    {
                        kalem.SonIslemZamani = DateTime.Now;
                        beyannameContext.Entry(kalem).State = EntityState.Modified;
                    }
                    else
                    {
                        var countKalemNo = (from u in _beyannameContext.NbKalem
                                            where u.NctsBeyanInternalNo == kalem.NctsBeyanInternalNo
                                            select (u.KalemSiraNo)).Count();

                        if (countKalemNo > 0)
                        {
                            var maxKalemNo = (from u in _beyannameContext.NbKalem
                                              where u.NctsBeyanInternalNo == kalem.NctsBeyanInternalNo
                                              select (u.KalemSiraNo)).Max();
                            var maxKalemInternalNo = (from u in _beyannameContext.NbKalem
                                                      where u.NctsBeyanInternalNo == kalem.NctsBeyanInternalNo
                                                      select (u.KalemInternalNo)).Max();

                            kalem.KalemSiraNo = Convert.ToInt32(maxKalemNo) + 1;
                            int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
                            kalem.SonIslemZamani = DateTime.Now;
                            kalem.KalemInternalNo = kalem.NctsBeyanInternalNo + "|" + klNo;
                        }
                        else
                        {
                            kalem.KalemSiraNo = 1;
                            kalem.SonIslemZamani = DateTime.Now;
                            kalem.KalemInternalNo = kalem.NctsBeyanInternalNo + "|1";
                        }




                        beyannameContext.Entry(kalem).State = EntityState.Added;
                    }

                    await beyannameContext.SaveChangesAsync();
                  
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    NctsBeyanKalemKapKiloGuncelle(kalem.NctsBeyanInternalNo);
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kalem Oluşturma/Değiştirme", ReferansNo = kalem.KalemInternalNo, Sonuc = "Kalem Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
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

        void NctsBeyanKalemKapKiloGuncelle(string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;

             var beyannameContext = new NctsDataContext(options);
            int ToplamKapSayisi = 0; decimal ToplamBrutAgirlik = 0;
            try
            {
                var beyanValues = beyannameContext.NbBeyan.FirstOrDefault(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo);
                var kalemValues = beyannameContext.NbKalem.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToList();
                var kapValues = beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToList();
               if(kalemValues!=null)
                foreach (var item in kalemValues)
                {
                  
                    ToplamBrutAgirlik = ToplamBrutAgirlik + item.BurutAgirlik;
                }

                if (kapValues != null)
                    foreach (var item in kapValues)
                {

                    ToplamKapSayisi = ToplamKapSayisi + item.KapAdet;
                }
                beyanValues.KalemSayisi = kalemValues.Count;
                beyanValues.ToplamKapSayisi = ToplamKapSayisi;
                beyanValues.KalemToplamBrutKG = ToplamBrutAgirlik;
                _beyannameContext.Entry(beyanValues).State = EntityState.Modified;
                _beyannameContext.SaveChangesAsync();
            }
            catch (Exception ex)
            { 
            }

        }
     
        
        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemAliciFirmaOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKalemAlici([FromBody] NbKalemAliciFirma firmaList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var aliciValues = await _beyannameContext.NbKalemAliciFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in aliciValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }


                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Alıcı Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Alıcı Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemGondericiFirmaOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKalemGonderici([FromBody] NbKalemGondericiFirma firmaList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var gondericiValues = await _beyannameContext.NbKalemGondericiFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in gondericiValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }
                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Gönderici Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Gönderici Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemGuvenliAliciFirmaOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKalemGuvenliAlici([FromBody] NbKalemGuvenliAliciFirma firmaList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var aliciValues = await _beyannameContext.NbKalemGuvenliAliciFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in aliciValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Güvenli Alıcı Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Güvenli Alıcı Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/KalemGuvenliGondericiFirmaOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKalemGuvenliGonderici([FromBody] NbKalemGuvenliGondericiFirma firmaList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var gondericiValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in gondericiValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        if (!string.IsNullOrEmpty(firmaList.AdUnvan) || !string.IsNullOrEmpty(firmaList.CaddeSokakNo) || !string.IsNullOrEmpty(firmaList.Dil) || !string.IsNullOrEmpty(firmaList.IlIlce) || !string.IsNullOrEmpty(firmaList.No) || !string.IsNullOrEmpty(firmaList.PostaKodu) || !string.IsNullOrEmpty(firmaList.UlkeKodu))
                        {
                            firmaList.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(firmaList).State = EntityState.Added;
                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Güvenli Gönderici Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Güvenli Gönderici Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/KonteynerOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKonteyner([FromBody] NbKonteyner[] konteynerBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in konteynerValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in konteynerBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Konteyner Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Konteyner Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/HassasEsyaOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostHassasEsya([FromBody] NbHassasEsya[] esyaBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var hassasEsyaValues = await _beyannameContext.NbHassasEsya.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in hassasEsyaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in esyaBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Hassas Eşya Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Hassas Eşya Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/KapOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostKap([FromBody] NbKap[] kapBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var kapValues = await _beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in kapValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in kapBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Kap Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Kap Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/EkBilgiOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostEkBilgi([FromBody] NbEkBilgi[] ekBilgiBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ekBilgiValues = await _beyannameContext.NbEkBilgi.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in ekBilgiValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in ekBilgiBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Ek Bilgi Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Ek Bilgi Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/BelgelerOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostBelgeler([FromBody] NbBelgeler[] belgelerBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in belgelerValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in belgelerBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Belgeler Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Belgeler Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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


        [Route("api/BYT/Servis/NctsBeyan/[controller]/OncekiBelgelerOlustur/{KalemInternalNo}/{NctsBeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostOncekiBelgeler([FromBody] NbOncekiBelgeler[] belgelerBilgiList, string KalemInternalNo, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var belgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in belgelerValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in belgelerBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Kalem Önceki Belgeler Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Kalem Önceki Belgeler Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/ObAcmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostObAcma([FromBody] NbObAcma[] acmaBilgiList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var acmaValues = await _beyannameContext.NbObAcma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo ).ToListAsync();

                        foreach (var item in acmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in acmaBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Ob Açma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Ob Açma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

        [Route("api/BYT/Servis/NctsBeyan/[controller]/AbAcmaOlustur/{NctsBeyanInternalNo}")]
        [HttpPost("{NctsBeyanInternalNo}")]
        public async Task<ServisDurum> PostAbAcma([FromBody] NbAbAcma[] acmaBilgiList, string NctsBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var acmaValues = await _beyannameContext.NbAbAcma.Where(v => v.NctsBeyanInternalNo == NctsBeyanInternalNo).ToListAsync();

                        foreach (var item in acmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in acmaBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(item).State = EntityState.Added;

                        }

                        await _beyannameContext.SaveChangesAsync();


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lstht = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Ab Açma Oluştur", ReferansNo = NctsBeyanInternalNo, Sonuc = "Ab Açma Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

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

    }

}