using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
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

namespace BYT.WS.Controllers.Servis.OzetBeyan
{


    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanOlusturmaController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;
        private OzetBeyanDataContext _beyannameContext;
        public IConfiguration Configuration { get; }

        public OzetBeyanOlusturmaController(IslemTarihceDataContext islemTarihceContext, IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _beyannameContext = new OzetBeyanDataContext(options);
            _islemTarihceContext = islemTarihceContext;

        }

        [Route("api/BYT/Servis/OzetBeyan/[controller]/BeyannameOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PostBeyanname([FromBody]ObBeyan beyan)
        {
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;
            ServisDurum _servisDurum = new ServisDurum();

            //List<Hata> _hatalar = new List<Hata>();

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
                     

                        var ozetBeyanValues = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == beyan.OzetBeyanInternalNo && v.TescilStatu != "Tescil Edildi");
                        var beyannameContext = new OzetBeyanDataContext(options);
                        if (ozetBeyanValues != null)
                        {
                            _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.OzetBeyanInternalNo);

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
                            var internalrefid = beyannameContext.GetRefIdNextSequenceValue(beyan.BeyanTuru);
                            string InternalNo = beyan.BeyanTuru + beyan.KullaniciKodu + "OB" + internalrefid.ToString().PadLeft(5, '0');

                            beyan.OzetBeyanInternalNo = InternalNo;
                            beyan.XmlRefId = InternalNo;
                            beyan.TescilStatu = "Olusturuldu";
                            beyan.OlsuturulmaTarihi = DateTime.Now;
                            beyan.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(beyan).State = EntityState.Added;
                            await beyannameContext.SaveChangesAsync();


                            _islem.Kullanici = beyan.KullaniciKodu;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "OzetBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = beyan.XmlRefId;
                            _islem.BeyanInternalNo = beyan.OzetBeyanInternalNo;
                            _islem.IslemInternalNo = beyan.OzetBeyanInternalNo;
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
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }

                }


                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = _islem.IslemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                // string jsonData = JsonConvert.SerializeObject(result, Formatting.None);
                var Message = $"PostBeyanname {DateTime.UtcNow.ToLongTimeString()}";
                Log.Information("Message displayed: {Message}/{Gelen}/{Sonuç}", Message, JsonConvert.SerializeObject(beyan), JsonConvert.SerializeObject(_servisDurum));

                return _servisDurum;
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasitUgrakUlkeOlustur/{OzetBeyanInternalNo}")]
        [HttpPost("{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PosTasitUgrak([FromBody]ObTasitUgrakUlke[] tasitList, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

          

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var tasitValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();

                        foreach (var item in tasitValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in tasitList)
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
                Bilgi blg = new Bilgi { IslemTipi = "Taşıt Uğrak Ülke Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıt Uğrak Ülke Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSenetOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PutTasimaSenet([FromBody]ObTasimaSenet senet)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;

            var kalemValues = await _beyannameContext.ObTasimaSenet.FirstOrDefaultAsync(v => v.TasimaSenetInternalNo == senet.TasimaSenetInternalNo && v.OzetBeyanInternalNo == senet.OzetBeyanInternalNo);
            var beyannameContext = new OzetBeyanDataContext(options);
            using (var transaction = beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    if (kalemValues != null)
                    {
                        senet.SonIslemZamani = DateTime.Now;
                        beyannameContext.Entry(senet).State = EntityState.Modified;
                    }
                    else
                    {
                        var countKalemNo = (from u in _beyannameContext.ObTasimaSenet
                                            where u.OzetBeyanInternalNo == senet.OzetBeyanInternalNo
                                            select (u.SenetSiraNo)).Count();

                        if (countKalemNo > 0)
                        {
                            var maxKalemNo = (from u in _beyannameContext.ObTasimaSenet
                                              where u.OzetBeyanInternalNo == senet.OzetBeyanInternalNo
                                              select (u.SenetSiraNo)).Max();
                            var maxKalemInternalNo = (from u in _beyannameContext.ObTasimaSenet
                                                      where u.OzetBeyanInternalNo == senet.OzetBeyanInternalNo
                                                      select (u.TasimaSenetInternalNo)).Max();

                            senet.SenetSiraNo = Convert.ToInt32(maxKalemNo) + 1;
                            int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
                            senet.SonIslemZamani = DateTime.Now;
                            senet.TasimaSenetInternalNo = senet.OzetBeyanInternalNo + "|" + klNo;
                        }
                        else
                        {
                            senet.SenetSiraNo = 1;
                            senet.SonIslemZamani = DateTime.Now;
                            senet.TasimaSenetInternalNo = senet.OzetBeyanInternalNo + "|1";
                        }




                        beyannameContext.Entry(senet).State = EntityState.Added;
                    }

                    await beyannameContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Taşıma Senet Oluşturma/Değiştirme", ReferansNo = senet.TasimaSenetInternalNo, Sonuc = "Taşıma Senet Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSenetSil/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        [HttpDelete("{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> DeleteTasimaSenet(string TasimaSenetInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
               .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
               .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);

            using (var transaction = _beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    var senetValues = await _beyannameContext.ObTasimaSenet.FirstOrDefaultAsync(v => v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo);
                    if (senetValues != null)
                    {
                        _beyannameContext.Entry(senetValues).State = EntityState.Deleted;

                        var updateKalemNo = from u in _beyannameContext.ObTasimaSenet
                                            where u.OzetBeyanInternalNo == OzetBeyanInternalNo && u.SenetSiraNo > senetValues.SenetSiraNo
                                            select u;

                        foreach (ObTasimaSenet itm in updateKalemNo)
                        {
                            itm.SenetSiraNo = itm.SenetSiraNo - 1;

                        }

                        var satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();
                        if (satirValues.Count > 0)
                            foreach (var item in satirValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                                var satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.TasimaSatirInternalNo == item.TasimaSatirInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();
                                if (satirEsyaValues.Count > 0)
                                    foreach (var itm in satirEsyaValues)
                                    {
                                        _beyannameContext.Entry(itm).State = EntityState.Deleted;
                                    }
                            }

                        var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();
                        if (ihracatValues.Count > 0)
                            foreach (var item in ihracatValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                        var ulkeValues = await _beyannameContext.ObUgrakUlke.Where(v => v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();
                        if (ulkeValues.Count > 0)
                            foreach (var item in ulkeValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;

                            }


                        await _beyannameContext.SaveChangesAsync();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Taşıma Senet Silme", ReferansNo = senetValues.TasimaSenetInternalNo, Sonuc = "Taşıma Senet Silme Başarılı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;


                        return _servisDurum;
                    }
                    else
                    {
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Taşıma Senet Silme", ReferansNo = senetValues.TasimaSenetInternalNo, Sonuc = "Taşıma Senet Silme Başarısız", SonucVeriler = null };
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


        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSatirOlustur/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        [HttpPost("{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostTasimaSatir([FromBody]ObTasimaSatir[] satirBilgiList, string TasimaSenetInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

           
            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo).ToListAsync();

                        foreach (var item in satirValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                            var satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.TasimaSatirInternalNo == item.TasimaSatirInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();
                            if (satirEsyaValues.Count > 0)
                                foreach (var itm in satirEsyaValues)
                                {
                                    _beyannameContext.Entry(itm).State = EntityState.Deleted;
                                }

                        }
                        int i = 1;
                        foreach (var item in satirBilgiList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            item.SatirNo = i;
                            item.TasimaSatirInternalNo = item.TasimaSenetInternalNo + "|" + i;
                            _beyannameContext.Entry(item).State = EntityState.Added;
                            i++;
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
                Bilgi blg = new Bilgi { IslemTipi = "Taşıma Satır Oluştur", ReferansNo = TasimaSenetInternalNo, Sonuc = "Taşıma Satır Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/UgrakUlkeOlustur/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        [HttpPost("{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostUgrakUlke([FromBody]ObUgrakUlke[] ulkeBilgiList, string TasimaSenetInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ulkeValues = await _beyannameContext.ObUgrakUlke.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo).ToListAsync();

                        foreach (var item in ulkeValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in ulkeBilgiList)
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
                Bilgi blg = new Bilgi { IslemTipi = "Uğrak Ülke Oluştur", ReferansNo = TasimaSenetInternalNo, Sonuc = "Uğrak Ülke Oluşturma Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/Servis/OzetBeyan/[controller]/IhracatOlustur/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        [HttpPost("{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostIhracat([FromBody]ObIhracat[] ihracatBilgiList, string TasimaSenetInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();


            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo).ToListAsync();

                        foreach (var item in ihracatValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in ihracatBilgiList)
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
                Bilgi blg = new Bilgi { IslemTipi = "İhracat Oluştur", ReferansNo = TasimaSenetInternalNo, Sonuc = "İhracat Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSatirEsyaOlustur/{TasimaSatirInternalNo}/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        [HttpPost("{TasimaSatirInternalNo}/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostSatirEsya([FromBody]ObSatirEsya[] satirEsyaBilgiList, string TasimaSatirInternalNo, string TasimaSenetInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();           

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ihracatValues = await _beyannameContext.ObSatirEsya.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo && v.TasimaSatirInternalNo == TasimaSatirInternalNo).ToListAsync();

                        foreach (var item in ihracatValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in satirEsyaBilgiList)
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
                Bilgi blg = new Bilgi { IslemTipi = "Satır Eşya Oluştur", ReferansNo = TasimaSenetInternalNo, Sonuc = "Satır Eşya Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasiyiciFirmaOlustur/{OzetBeyanInternalNo}")]
        [HttpPost("{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PosTasiyiciFirma([FromBody]ObTasiyiciFirma firmaList, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

           
            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        var firmaValues = await _beyannameContext.ObTasiyiciFirma.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }

                        firmaList.SonIslemZamani = DateTime.Now;
                        _beyannameContext.Entry(firmaList).State = EntityState.Added;


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
                Bilgi blg = new Bilgi { IslemTipi = "Taşıyıcı Firma Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıyıcı Firma Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasiyiciFirmaSil/{OzetBeyanInternalNo}")]
        [HttpDelete("{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> DeleteTasiyiciFirma(string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
               .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
               .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);

            using (var transaction = _beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    var senetValues = await _beyannameContext.ObTasiyiciFirma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo);
                    if (senetValues != null)
                    {
                        _beyannameContext.Entry(senetValues).State = EntityState.Deleted;

                        await _beyannameContext.SaveChangesAsync();

                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Taşıyıcı Firma Silme", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıyıcı Firma Silme Başarılı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;


                        return _servisDurum;
                    }
                    else
                    {
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        List<Internal.Hata> lstht = new List<Internal.Hata>();
                        Bilgi blg = new Bilgi { IslemTipi = "Taşıyıcı Firma Silme", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıyıcı Firma Bulunamadı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = "Taşıyıcı Firma Bulunamadı" };
                        lstht.Add(ht);
                        _servisDurum.Hatalar = lstht;
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


        [Route("api/BYT/Servis/OzetBeyan/[controller]/OzetBeyanAcmaOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PutOzetBeyanAcma([FromBody]ObOzetBeyanAcma ozbyAcma)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;

            var ozbyAcmaValues = await _beyannameContext.ObOzetBeyanAcma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == ozbyAcma.OzetBeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == ozbyAcma.OzetBeyanAcmaBeyanInternalNo);
            var beyannameContext = new OzetBeyanDataContext(options);
            using (var transaction = beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    if (ozbyAcmaValues != null)
                    {
                        ozbyAcma.SonIslemZamani = DateTime.Now;
                        beyannameContext.Entry(ozbyAcma).State = EntityState.Modified;
                    }
                    else
                    {

                        var countKalemNo = (from u in beyannameContext.ObOzetBeyanAcma
                                            where u.OzetBeyanInternalNo == ozbyAcma.OzetBeyanInternalNo
                                            select (u.OzetBeyanInternalNo)).Count();

                        if (countKalemNo > 0)
                        {

                            var maxKalemInternalNo = (from u in beyannameContext.ObOzetBeyanAcma
                                                      where u.OzetBeyanInternalNo == ozbyAcma.OzetBeyanInternalNo
                                                      select (u.OzetBeyanAcmaBeyanInternalNo)).Max();

                            ozbyAcma.SonIslemZamani = DateTime.Now;

                            int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
                            ozbyAcma.OzetBeyanAcmaBeyanInternalNo = ozbyAcma.OzetBeyanInternalNo + "|" + klNo;
                           
                        }
                        else
                        {
                            ozbyAcma.SonIslemZamani = DateTime.Now;
                           ozbyAcma.OzetBeyanAcmaBeyanInternalNo = ozbyAcma.OzetBeyanInternalNo + "|1";
                        }

                        beyannameContext.Entry(ozbyAcma).State = EntityState.Added;
                    }

                    await beyannameContext.SaveChangesAsync();

                    transaction.Commit();

                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Özet Beyan Açma Oluşturma/Değiştirme", ReferansNo = ozbyAcma.OzetBeyanAcmaBeyanInternalNo, Sonuc = "Ozet Beyan Açma Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSenetOlustur/{OzetBeyanInternalNo}/{OzetBeyanAcmaBeyanInternalNo}")]
        [HttpPost("{OzetBeyanInternalNo}/{OzetBeyanAcmaBeyanInternalNo}")]
        public async Task<ServisDurum> PostTasimaSenet([FromBody]ObOzetBeyanAcmaTasimaSenet[] tasimaSenetList, string OzetBeyanInternalNo, string OzetBeyanAcmaBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;


            var beyannameContext = new OzetBeyanDataContext(options);

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
                var tasimaSenetValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo).ToListAsync();

                using (var transaction = beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (tasimaSenetValues.Count > 0)
                        {
                            foreach (var item in tasimaSenetList)
                            {
                                var tasima = tasimaSenetValues.Where(x => x.TasimaSenetInternalNo == item.TasimaSenetInternalNo).FirstOrDefault();
                                if (tasima != null)
                                {
                                    item.SonIslemZamani = DateTime.Now;
                                    beyannameContext.Entry(item).State = EntityState.Modified;

                                }
                                else
                                {
                                    var maxTasimaSenetInternalNo = (from u in beyannameContext.ObOzetBeyanAcmaTasimaSenet
                                                                    where u.OzetBeyanInternalNo == OzetBeyanInternalNo &&
                                                                    u.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo
                                                                    select (u.TasimaSenetInternalNo)).Max();


                                    int klNo = Convert.ToInt32(maxTasimaSenetInternalNo.Split('|')[2].ToString()) + 1;
                                    item.SonIslemZamani = DateTime.Now;
                                    item.OzetBeyanAcmaBeyanInternalNo = OzetBeyanAcmaBeyanInternalNo;
                                    item.TasimaSenetInternalNo = item.OzetBeyanAcmaBeyanInternalNo + "|" + klNo.ToString();
                                    beyannameContext.Entry(item).State = EntityState.Added;

                                }

                            }
                            foreach (var item in tasimaSenetValues)
                            {
                                var tasima = tasimaSenetList.Where(x => x.TasimaSenetInternalNo == item.TasimaSenetInternalNo).FirstOrDefault();
                                if (tasima == null)
                                {
                                    beyannameContext.Entry(item).State = EntityState.Deleted;
                                    var TasimaSatirValue = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo).ToListAsync();
                                    foreach (var satir in TasimaSatirValue)
                                    {
                                        beyannameContext.Entry(satir).State = EntityState.Deleted;

                                    }
                                }
                            }
                        }
                        else
                        {
                            int i = 1;
                            foreach (var item in tasimaSenetList)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                item.OzetBeyanAcmaBeyanInternalNo = OzetBeyanAcmaBeyanInternalNo;
                                item.TasimaSenetInternalNo = item.OzetBeyanAcmaBeyanInternalNo + "|" + i.ToString();
                                beyannameContext.Entry(item).State = EntityState.Added;
                                i++;
                            }
                        }


                        await beyannameContext.SaveChangesAsync();


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

                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Taşaıma Seneti Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıma Seneti Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TasimaSatirOlustur/{TasimaSenetInternalNo}/{OzetBeyanAcmaBeyanInternalNo}/{OzetBeyanInternalNo}")]
        [HttpPost("{TasimaSenetInternalNo}/{OzetBeyanAcmaBeyanInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostTasimaSatir([FromBody]ObOzetBeyanAcmaTasimaSatir[] tasimaSatirList, string TasimaSenetInternalNo, string OzetBeyanAcmaBeyanInternalNo, string OzetBeyanInternalNo)
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
                        var tasimaSenetValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo).ToListAsync();

                        foreach (var item in tasimaSenetValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        int i = 1;
                        foreach (var item in tasimaSatirList)
                        {
                            item.SonIslemZamani = DateTime.Now;
                            item.AcmaSatirNo = i;
                            _beyannameContext.Entry(item).State = EntityState.Added;
                            i++;
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

                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Taşaıma Satır Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıma Satır Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/OzetBeyanAcmaSil/{OzetBeyanAcmaBeyanInternalNo}/{OzetBeyanInternalNo}")]
        [HttpDelete("{OzetBeyanAcmaBeyanInternalNo}/{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> DeleteOzetBeyanAcma(string OzetBeyanAcmaBeyanInternalNo, string OzetBeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
               .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
               .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);

            using (var transaction = _beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    var ozbyAcmaValues = await _beyannameContext.ObOzetBeyanAcma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo);

                    if (ozbyAcmaValues != null)
                    {
                        _beyannameContext.Entry(ozbyAcmaValues).State = EntityState.Deleted;


                        var tasimaSenetiValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo).ToListAsync();
                        if (tasimaSenetiValues.Count > 0)
                            foreach (var item in tasimaSenetiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                                var tasimaSatiriValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo && v.OzetBeyanAcmaBeyanInternalNo == OzetBeyanAcmaBeyanInternalNo).ToListAsync();
                                if (tasimaSatiriValues.Count > 0)
                                    foreach (var satir in tasimaSatiriValues)
                                    {
                                        _beyannameContext.Entry(satir).State = EntityState.Deleted;
                                    }
                            }


                        await _beyannameContext.SaveChangesAsync();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Ozet Beyan Açma Silme", ReferansNo = ozbyAcmaValues.OzetBeyanInternalNo, Sonuc = "Özet Beyan Açma Silme Başarılı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;
                        return _servisDurum;
                    }
                    else
                    {
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Ozet Beyan Açma Silme", ReferansNo = ozbyAcmaValues.OzetBeyanInternalNo, Sonuc = "Ozet Beyan Açma Silme Başarısız", SonucVeriler = null };
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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/TeminatOlustur/{OzetBeyanInternalNo}")]
        [HttpPost("{OzetBeyanInternalNo}")]
        public async Task<ServisDurum> PostTeminat([FromBody]ObTeminat[] teminatList, string OzetBeyanInternalNo)
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
                        var teminatValues = await _beyannameContext.ObTeminat.Where(v => v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();

                        foreach (var item in teminatValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in teminatList)
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

                        return _servisDurum;
                    }

                }

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Teminat Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Teminat Oluşturma Başarılı", SonucVeriler = null };
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