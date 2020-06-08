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

                        var ozetBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == beyan.NctsBeyanInternalNo && v.TescilStatu != "Tescil Edildi");
                        var beyannameContext = new NctsDataContext(options);
                        if (ozetBeyanValues != null)
                        {
                            _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.NctsBeyanInternalNo);

                            beyan.TescilStatu = "Güncellendi";
                            beyan.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(beyan).State = EntityState.Modified;
                            await beyannameContext.SaveChangesAsync();


                            _islem.IslemDurumu = "Güncellendi";
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();

                        }

                        else
                        {
                            var internalrefid = beyannameContext.GetRefIdNextSequenceValue(beyan.Rejim);
                            string InternalNo = beyan.Rejim + beyan.Kullanici + "DB" + internalrefid.ToString().PadLeft(5, '0');

                            beyan.NctsBeyanInternalNo = InternalNo;
                            beyan.RefNo = InternalNo;
                            beyan.TescilStatu = "Olusturuldu";
                            beyan.OlsuturulmaTarihi = DateTime.Now;
                            beyan.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(beyan).State = EntityState.Added;
                            await beyannameContext.SaveChangesAsync();


                            _islem.Kullanici = beyan.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "DetayliBeyan";
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


        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KalemSil/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpDelete("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> DeleteKalem(string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //       .Options;
        //    var _beyannameContext = new BeyannameDataContext(options);

        //    using (var transaction = _beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var kalemValues = await _beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
        //            if (kalemValues != null)
        //            {
        //                _beyannameContext.Entry(kalemValues).State = EntityState.Deleted;

        //                var updateKalemNo = from u in _beyannameContext.DbKalem
        //                                    where u.BeyanInternalNo == BeyanInternalNo && u.KalemSiraNo > kalemValues.KalemSiraNo
        //                                    select u;

        //                foreach (DbKalem itm in updateKalemNo)
        //                {
        //                    itm.KalemSiraNo = itm.KalemSiraNo - 1;



        //                }

        //                var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (odemeValues.Count > 0)
        //                    foreach (var item in odemeValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var markaValues = await _beyannameContext.DbMarka.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (markaValues.Count > 0)
        //                    foreach (var item in markaValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (konteynerValues.Count > 0)
        //                    foreach (var item in konteynerValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var acmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (acmaValues.Count > 0)
        //                    foreach (var item in acmaValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var tammalayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (tammalayiciValues.Count > 0)
        //                    foreach (var item in tammalayiciValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var vergiValues = await _beyannameContext.DbVergi.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (vergiValues.Count > 0)
        //                    foreach (var item in vergiValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var belgeValues = await _beyannameContext.DbBelge.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (belgeValues.Count > 0)
        //                    foreach (var item in belgeValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                var soruCevapValues = await _beyannameContext.DbSoruCevap.Where(v => v.KalemInternalNo == KalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (soruCevapValues.Count > 0)
        //                    foreach (var item in soruCevapValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }

        //                await _beyannameContext.SaveChangesAsync();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Kalem Silme", ReferansNo = kalemValues.KalemInternalNo, Sonuc = "Kalem Silme Başarılı", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;


        //                return _servisDurum;
        //            }
        //            else
        //            {
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Kalem Silme", ReferansNo = kalemValues.KalemInternalNo, Sonuc = "Kalem Silme Başarısız", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;


        //                return _servisDurum;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;
        //        }

        //    }

        //}


        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KalemOlustur")]
        //[HttpPost]
        //public async Task<ServisDurum> PutKalem([FromBody]DbKalem kalem)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //      .Options;

        //    var kalemValues = await _beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == kalem.KalemInternalNo && v.BeyanInternalNo == kalem.BeyanInternalNo);
        //    var beyannameContext = new BeyannameDataContext(options);
        //    using (var transaction = beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (kalemValues != null)
        //            {
        //                kalem.SonIslemZamani = DateTime.Now;
        //                beyannameContext.Entry(kalem).State = EntityState.Modified;
        //            }
        //            else
        //            {
        //                var countKalemNo = (from u in _beyannameContext.DbKalem
        //                                    where u.BeyanInternalNo == kalem.BeyanInternalNo
        //                                    select (u.KalemSiraNo)).Count();

        //                if (countKalemNo > 0)
        //                {
        //                    var maxKalemNo = (from u in _beyannameContext.DbKalem
        //                                      where u.BeyanInternalNo == kalem.BeyanInternalNo
        //                                      select (u.KalemSiraNo)).Max();
        //                    var maxKalemInternalNo = (from u in _beyannameContext.DbKalem
        //                                              where u.BeyanInternalNo == kalem.BeyanInternalNo
        //                                              select (u.KalemInternalNo)).Max();

        //                    kalem.KalemSiraNo = Convert.ToInt32(maxKalemNo) + 1;
        //                    int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
        //                    kalem.SonIslemZamani = DateTime.Now;
        //                    kalem.KalemInternalNo = kalem.BeyanInternalNo + "|" + klNo;
        //                }
        //                else
        //                {
        //                    kalem.KalemSiraNo = 1;
        //                    kalem.SonIslemZamani = DateTime.Now;
        //                    kalem.KalemInternalNo = kalem.BeyanInternalNo + "|1";
        //                }




        //                beyannameContext.Entry(kalem).State = EntityState.Added;
        //            }

        //            await beyannameContext.SaveChangesAsync();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //            transaction.Commit();
        //            List<Bilgi> lstBlg = new List<Bilgi>();
        //            Bilgi blg = new Bilgi { IslemTipi = "Kalem Oluşturma/Değiştirme", ReferansNo = kalem.KalemInternalNo, Sonuc = "Kalem Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
        //            lstBlg.Add(blg);
        //            _servisDurum.Bilgiler = lstBlg;


        //            return _servisDurum;
        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;

        //        }

        //    }


        //}


        //[Route("api/BYT/Servis/NctsBeyan/[controller]/OdemeSekliOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostOdeme([FromBody]DbOdemeSekli[] odemeBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in odemeValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in odemeBilgiList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;
        //                var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Ödeme Şekli Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Ödeme Şekşi Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KonteynerOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostKonteyner([FromBody]DbKonteyner[] konteynerBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in konteynerValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in konteynerBilgiList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;
        //                var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Konteyner Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Konteyner Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/TamamlayiciBilgiOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostTamamlayici([FromBody]DbTamamlayiciBilgi[] tamamlayiciBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in tamamlayiciValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in tamamlayiciBilgiList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Konteyner Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Konteyner Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/MarkaOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostMarka([FromBody]DbMarka[] markaBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in markaValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in markaBilgiList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Marka Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Marka Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/BeyannameAcmaOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostBeyannameAcma([FromBody]DbBeyannameAcma[] acmaBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var acmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in acmaValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in acmaBilgiList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Beyanname Açma Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Beyanname Açma Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/VergiOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostVergiler([FromBody]DbVergi[] vergiBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //   .Options;
        //    var beyannameContext = new BeyannameDataContext(options);
        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var vergiValues = await _beyannameContext.DbVergi.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in vergiValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in vergiBilgiList)
        //                {
        //                    if (item.KalemNo == 0)
        //                    {
        //                        var kalemNo = await beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo);
        //                        item.KalemNo = kalemNo.KalemSiraNo;
        //                        item.SonIslemZamani = DateTime.Now;
        //                    }
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Vergi Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Vergi Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/BelgeOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostBelgeler([FromBody]DbBelge[] belgeBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //   .Options;
        //    var beyannameContext = new BeyannameDataContext(options);
        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var belgeValues = await _beyannameContext.DbBelge.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in belgeValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in belgeBilgiList)
        //                {
        //                    if (item.KalemNo == 0)
        //                    {
        //                        var kalemNo = await beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo);
        //                        item.KalemNo = kalemNo.KalemSiraNo;
        //                        item.SonIslemZamani = DateTime.Now;
        //                    }
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Belge Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Belge Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}


        //[Route("api/BYT/Servis/NctsBeyan/[controller]/SoruCevapOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostSoruCevap([FromBody]DbSoruCevap[] soruCevapBilgiList, string KalemInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //     .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //     .Options;
        //     var beyannameContext = new BeyannameDataContext(options);
        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var sorucevapValues = await _beyannameContext.DbSoruCevap.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

        //                foreach (var item in sorucevapValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in soruCevapBilgiList)
        //                {
                         
        //                    if(item.KalemNo==0)
        //                    {
        //                        var kalemNo = await beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo);
        //                        item.KalemNo = kalemNo.KalemSiraNo;
        //                        item.SonIslemZamani = DateTime.Now;
        //                    }
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Soru Cevap Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Soru Cevap Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/TeminatOlustur/{BeyanInternalNo}")]
        //[HttpPost("{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostTeminat([FromBody]DbTeminat[] teminatList, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == BeyanInternalNo).ToListAsync();

        //                foreach (var item in teminatValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in teminatList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Teminat Oluştur", ReferansNo = BeyanInternalNo, Sonuc = "Teminat Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/FirmaOlustur/{BeyanInternalNo}")]
        //[HttpPost("{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostFirma([FromBody]DbFirma[] firmaList, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == BeyanInternalNo).ToListAsync();

        //                foreach (var item in firmaValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                foreach (var item in firmaList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Firma Oluştur", ReferansNo = BeyanInternalNo, Sonuc = "Firma Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KiymetBildirimOlustur")]
        //[HttpPost]
        //public async Task<ServisDurum> PutKiymet([FromBody]DbKiymetBildirim kiymet)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //      .Options;

        //    var kiymetValues = await _beyannameContext.DbKiymetBildirim.FirstOrDefaultAsync(v => v.KiymetInternalNo == kiymet.KiymetInternalNo && v.BeyanInternalNo == kiymet.BeyanInternalNo);
        //    var beyannameContext = new BeyannameDataContext(options);
        //    using (var transaction = beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (kiymetValues != null)
        //            {
        //                kiymet.SonIslemZamani = DateTime.Now;
        //                beyannameContext.Entry(kiymet).State = EntityState.Modified;
        //            }
        //            else
        //            {

        //                var countKalemNo = (from u in beyannameContext.DbKiymetBildirim
        //                                    where u.BeyanInternalNo == kiymet.BeyanInternalNo
        //                                    select (u.KiymetInternalNo)).Count();

        //                if (countKalemNo > 0)
        //                {

        //                    var maxKalemInternalNo = (from u in beyannameContext.DbKiymetBildirim
        //                                              where u.BeyanInternalNo == kiymet.BeyanInternalNo
        //                                              select (u.KiymetInternalNo)).Max();

        //                    kiymet.SonIslemZamani = DateTime.Now;
        //                    int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
        //                    kiymet.KiymetInternalNo = kiymet.BeyanInternalNo + "|" + klNo;
        //                }
        //                else
        //                {
        //                    kiymet.SonIslemZamani = DateTime.Now;
        //                    kiymet.KiymetInternalNo = kiymet.BeyanInternalNo + "|1";
        //                }




        //                beyannameContext.Entry(kiymet).State = EntityState.Added;
        //            }

        //            await beyannameContext.SaveChangesAsync();

        //            transaction.Commit();

        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //            List<Bilgi> lstBlg = new List<Bilgi>();
        //            Bilgi blg = new Bilgi { IslemTipi = "Kiymet Oluşturma/Değiştirme", ReferansNo = kiymet.KiymetInternalNo, Sonuc = "Kiymet Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
        //            lstBlg.Add(blg);
        //            _servisDurum.Bilgiler = lstBlg;


        //            return _servisDurum;
        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;

        //        }

        //    }


        //}


        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KiymetSil/{kiymetInternalNo}/{BeyanInternalNo}")]
        //[HttpDelete("{kiymetInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> DeleteKiymet(string kiymetInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //       .Options;
        //    var _beyannameContext = new BeyannameDataContext(options);

        //    using (var transaction = _beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var kiymetValues = await _beyannameContext.DbKiymetBildirim.FirstOrDefaultAsync(v => v.KiymetInternalNo == kiymetInternalNo && v.BeyanInternalNo == BeyanInternalNo);

        //            if (kiymetValues != null)
        //            {
        //                _beyannameContext.Entry(kiymetValues).State = EntityState.Deleted;


        //                var kalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.KiymetInternalNo == kiymetInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (kalemValues.Count > 0)
        //                    foreach (var item in kalemValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                    }


        //                await _beyannameContext.SaveChangesAsync();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Kıymet Silme", ReferansNo = kiymetValues.KiymetInternalNo, Sonuc = "Kıymet Silme Başarılı", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;
        //                return _servisDurum;
        //            }
        //            else
        //            {
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Kıymet Silme", ReferansNo = kiymetValues.KiymetInternalNo, Sonuc = "Kıymet Silme Başarısız", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;
        //                return _servisDurum;
        //            }


        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;
        //        }

        //    }

        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/KiymetKalemOlustur/{KiymetInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{KiymetInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostKiymetKalemleri([FromBody]DbKiymetBildirimKalem[] kalemList, string KiymetInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var kalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KiymetInternalNo == KiymetInternalNo).ToListAsync();

        //                foreach (var item in kalemValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }
        //                int i = 1;
        //                foreach (var item in kalemList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    item.KiymetKalemNo = i;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;
        //                    i++;
        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Kıymet Kalemi Oluştur", ReferansNo = KiymetInternalNo, Sonuc = "Kıymet Kalemi Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/OzetBeyanAcmaOlustur")]
        //[HttpPost]
        //public async Task<ServisDurum> PutOzetBeyanAcma([FromBody]DbOzetBeyanAcma ozbyAcma)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //      .Options;

        //    var ozbyAcmaValues = await _beyannameContext.DbOzetbeyanAcma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == ozbyAcma.OzetBeyanInternalNo && v.BeyanInternalNo == ozbyAcma.BeyanInternalNo);
        //    var beyannameContext = new BeyannameDataContext(options);
        //    using (var transaction = beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (ozbyAcmaValues != null)
        //            {
        //                ozbyAcma.SonIslemZamani = DateTime.Now;
        //                beyannameContext.Entry(ozbyAcma).State = EntityState.Modified;
        //            }
        //            else
        //            {

        //                var countKalemNo = (from u in beyannameContext.DbOzetbeyanAcma
        //                                    where u.BeyanInternalNo == ozbyAcma.BeyanInternalNo
        //                                    select (u.OzetBeyanInternalNo)).Count();

        //                if (countKalemNo > 0)
        //                {

        //                    var maxKalemInternalNo = (from u in beyannameContext.DbOzetbeyanAcma
        //                                              where u.BeyanInternalNo == ozbyAcma.BeyanInternalNo
        //                                              select (u.OzetBeyanInternalNo)).Max();

        //                    ozbyAcma.SonIslemZamani = DateTime.Now;
        //                    int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
        //                    ozbyAcma.OzetBeyanInternalNo = ozbyAcma.BeyanInternalNo + "|" + klNo;
        //                }
        //                else
        //                {
        //                    ozbyAcma.SonIslemZamani = DateTime.Now;
        //                    ozbyAcma.OzetBeyanInternalNo = ozbyAcma.BeyanInternalNo + "|1";
        //                }

        //                beyannameContext.Entry(ozbyAcma).State = EntityState.Added;
        //            }

        //            await beyannameContext.SaveChangesAsync();

        //            transaction.Commit();

        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //            List<Bilgi> lstBlg = new List<Bilgi>();
        //            Bilgi blg = new Bilgi { IslemTipi = "Özet Beyan Açma Oluşturma/Değiştirme", ReferansNo = ozbyAcma.OzetBeyanInternalNo, Sonuc = "Ozet Beyan Açma Oluşturma/Değiştirme  Başarılı", SonucVeriler = null };
        //            lstBlg.Add(blg);
        //            _servisDurum.Bilgiler = lstBlg;


        //            return _servisDurum;
        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;

        //        }

        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/TasimaSenetOlustur/{OzetBeyanInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{OzetBeyanInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostTasimaSenet([FromBody]DbOzetBeyanAcmaTasimaSenet[] tasimaSenetList, string OzetBeyanInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //     .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //     .Options;


        //    var beyannameContext = new BeyannameDataContext(options);

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {
        //        var tasimaSenetValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSenet.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo).ToListAsync();

        //        using (var transaction = beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (tasimaSenetValues.Count > 0)
        //                {
        //                    foreach (var item in tasimaSenetList)
        //                    {
        //                        var tasima = tasimaSenetValues.Where(x => x.TasimaSenetInternalNo == item.TasimaSenetInternalNo).FirstOrDefault();
        //                        if (tasima != null)
        //                        {
        //                            item.SonIslemZamani = DateTime.Now;
        //                            beyannameContext.Entry(item).State = EntityState.Modified;
                                   
        //                        }
        //                        else
        //                        {
        //                            var maxTasimaSenetInternalNo = (from u in beyannameContext.DbOzetBeyanAcmaTasimaSenet
        //                                                            where u.BeyanInternalNo == BeyanInternalNo &&
        //                                                            u.OzetBeyanInternalNo == OzetBeyanInternalNo
        //                                                            select (u.TasimaSenetInternalNo)).Max();


        //                            int klNo = Convert.ToInt32(maxTasimaSenetInternalNo.Split('|')[2].ToString()) + 1;
        //                            item.SonIslemZamani = DateTime.Now;
        //                            item.TasimaSenetInternalNo = item.OzetBeyanInternalNo + "|" + klNo.ToString();
        //                            beyannameContext.Entry(item).State = EntityState.Added;

        //                       }

        //                    }
        //                    foreach (var item in tasimaSenetValues)
        //                    {
        //                        var tasima = tasimaSenetList.Where(x => x.TasimaSenetInternalNo == item.TasimaSenetInternalNo).FirstOrDefault();
        //                        if (tasima == null)
        //                        {
        //                            beyannameContext.Entry(item).State = EntityState.Deleted;
        //                            var TasimaSatirValue= await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo== item.TasimaSenetInternalNo).ToListAsync();
        //                            foreach (var satir in TasimaSatirValue)
        //                            {
        //                                beyannameContext.Entry(satir).State = EntityState.Deleted;

        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    int i = 1;
        //                    foreach (var item in tasimaSenetList)
        //                    {
        //                        item.SonIslemZamani = DateTime.Now;
        //                        item.TasimaSenetInternalNo = item.OzetBeyanInternalNo + "|" + i.ToString();
        //                        beyannameContext.Entry(item).State = EntityState.Added;
        //                        i++;
        //                    }
        //                }


        //                await beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Taşaıma Seneti Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıma Seneti Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/TasimaSatirOlustur/{TasimaSenetInternalNo}/{OzetBeyanInternalNo}/{BeyanInternalNo}")]
        //[HttpPost("{TasimaSenetInternalNo}/{OzetBeyanInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> PostTasimaSatir([FromBody]DbOzetBeyanAcmaTasimaSatir[] tasimaSatirList, string TasimaSenetInternalNo, string OzetBeyanInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();

        //    List<Hata> _hatalar = new List<Hata>();

        //    //DbKalemModelValidator vbValidator = new DbKalemModelValidator();
        //    //ValidationResult validationResult = new ValidationResult();
        //    //validationResult = vbValidator.Validate(odeme);

        //    //if (!validationResult.IsValid)
        //    //{

        //    //    Hata ht = new Hata();
        //    //    for (int i = 0; i < validationResult.Errors.Count; i++)
        //    //    {
        //    //        ht = new Hata();
        //    //        ht.HataKodu = (i + 1);
        //    //        ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
        //    //        _hatalar.Add(ht);
        //    //    }

        //    //    _servisDurum.Hatalar = _hatalar;

        //    //    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
        //    //    return result;
        //    //}

        //    try
        //    {

        //        using (var transaction = _beyannameContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var tasimaSenetValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.OzetBeyanInternalNo == OzetBeyanInternalNo && v.TasimaSenetInternalNo == TasimaSenetInternalNo).ToListAsync();

        //                foreach (var item in tasimaSenetValues)
        //                {
        //                    _beyannameContext.Entry(item).State = EntityState.Deleted;

        //                }

        //                foreach (var item in tasimaSatirList)
        //                {
        //                    item.SonIslemZamani = DateTime.Now;
        //                    _beyannameContext.Entry(item).State = EntityState.Added;

        //                }

        //                await _beyannameContext.SaveChangesAsync();


        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {

        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lstht = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lstht.Add(ht);
        //                _servisDurum.Hatalar = lstht;

        //                return _servisDurum;
        //            }

        //        }

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        Bilgi blg = new Bilgi { IslemTipi = "Taşaıma Satır Oluştur", ReferansNo = OzetBeyanInternalNo, Sonuc = "Taşıma Satır Oluşturma Başarılı", SonucVeriler = null };
        //        lstBlg.Add(blg);
        //        _servisDurum.Bilgiler = lstBlg;

        //        var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
        //        //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

        //        return _servisDurum;
        //    }
        //    catch (Exception ex)
        //    {

        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}

        //[Route("api/BYT/Servis/NctsBeyan/[controller]/OzetBeyanAcmaSil/{ozetBeyanInternalNo}/{BeyanInternalNo}")]
        //[HttpDelete("{ozetBeyanInternalNo}/{BeyanInternalNo}")]
        //public async Task<ServisDurum> DeleteOzetBeyanAcma(string ozetBeyanInternalNo, string BeyanInternalNo)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //       .Options;
        //    var _beyannameContext = new BeyannameDataContext(options);

        //    using (var transaction = _beyannameContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var ozbyAcmaValues = await _beyannameContext.DbOzetbeyanAcma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == ozetBeyanInternalNo && v.BeyanInternalNo == BeyanInternalNo);

        //            if (ozbyAcmaValues != null)
        //            {
        //                _beyannameContext.Entry(ozbyAcmaValues).State = EntityState.Deleted;


        //                var tasimaSenetiValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == ozetBeyanInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                if (tasimaSenetiValues.Count > 0)
        //                    foreach (var item in tasimaSenetiValues)
        //                    {
        //                        _beyannameContext.Entry(item).State = EntityState.Deleted;
        //                        var tasimaSatiriValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == ozetBeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
        //                        if (tasimaSatiriValues.Count > 0)
        //                            foreach (var satir in tasimaSatiriValues)
        //                            {
        //                                _beyannameContext.Entry(satir).State = EntityState.Deleted;
        //                            }
        //                    }


        //                await _beyannameContext.SaveChangesAsync();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Ozet Beyan Açma Silme", ReferansNo = ozbyAcmaValues.OzetBeyanInternalNo, Sonuc = "Özet Beyan Açma Silme Başarılı", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;
        //                return _servisDurum;
        //            }
        //            else
        //            {
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
        //                transaction.Commit();
        //                List<Bilgi> lstBlg = new List<Bilgi>();
        //                Bilgi blg = new Bilgi { IslemTipi = "Ozet Beyan Açma Silme", ReferansNo = ozbyAcmaValues.OzetBeyanInternalNo, Sonuc = "Ozet Beyan Açma Silme Başarısız", SonucVeriler = null };
        //                lstBlg.Add(blg);
        //                _servisDurum.Bilgiler = lstBlg;
        //                return _servisDurum;
        //            }


        //        }
        //        catch (Exception ex)
        //        {

        //            transaction.Rollback();
        //            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //            List<Internal.Hata> lstht = new List<Internal.Hata>();

        //            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //            lstht.Add(ht);
        //            _servisDurum.Hatalar = lstht;

        //            return _servisDurum;
        //        }

        //    }

        //}





    }

}