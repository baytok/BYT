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

namespace BYT.WS.Controllers.Servis.Beyanname
{

    //[Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BeyannameOlusturmaController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;
        private BeyannameDataContext _beyannameContext;
        public IConfiguration Configuration { get; }

        public BeyannameOlusturmaController(IslemTarihceDataContext islemTarihceContext, IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _beyannameContext = new BeyannameDataContext(options);
            _islemTarihceContext = islemTarihceContext;

        }


        [Route("api/BYT/Servis/Beyanname/[controller]/{Kullanici}")]
        [HttpPut("{Kullanici}")]
        public async Task<Sonuc<ServisDurum>> PutBeyanname(string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum(); DbBeyan _beyan = new DbBeyan(); DbKalem _kalem = new DbKalem();

            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {
                        _beyan.Kullanici = Kullanici;
                        _beyan.Rejim = "1000";
                        _beyan.Gumruk = "067777";
                        _beyan.RefNo = "000002";
                        _beyan.BeyanInternalNo = Kullanici + "DB" + "000002"; //TODO: sequence sayı alıp 6 ya tamamlayalım


                        _kalem.BeyanInternalNo = _beyan.BeyanInternalNo;
                        _kalem.KalemSiraNo = 1;
                        _kalem.KalemInternalNo = "123";
                        _kalem.Gtip = "1111";


                        _beyannameContext.Entry(_beyan).State = EntityState.Added;
                        await _beyannameContext.SaveChangesAsync();
                        _beyannameContext.Entry(_kalem).State = EntityState.Added;
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
                        return rresult;
                    }

                }



                Islem _islem = new Islem();
                _islem.Kullanici = Kullanici;
                _islem.IslemTipi = "";
                _islem.BeyanTipi = "DetayliBeyan";
                _islem.IslemDurumu = "Olusturuldu";
                _islem.RefNo = _beyan.RefNo;
                _islem.BeyanInternalNo = _beyan.BeyanInternalNo;
                _islem.IslemInternalNo = _beyan.BeyanInternalNo;
                _islem.OlusturmaZamani = DateTime.Now;
                _islem.GonderimSayisi = 0;

                _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                await _islemTarihceContext.SaveChangesAsync();


                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = Kullanici, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;
            }
            catch (Exception ex)
            {

                return null;
            }


        }


        [Route("api/BYT/Servis/Beyanname/[controller]/BeyannameOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PostBeyanname([FromBody]DbBeyan beyan)
        {
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            DbBeyanModelValidator vbValidator = new DbBeyanModelValidator();
            ValidationResult validationResult = new ValidationResult();
            validationResult = vbValidator.Validate(beyan);

            if (!validationResult.IsValid)
            {

                Hata ht = new Hata();
                for (int i = 0; i < validationResult.Errors.Count; i++)
                {
                    ht = new Hata();
                    ht.HataKodu = (i + 1);
                    ht.HataAciklamasi = validationResult.Errors[i].ErrorMessage;
                    _hatalar.Add(ht);
                }

                _servisDurum.Hatalar = _hatalar;

                //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilmedi" };
                return _servisDurum;
            }
            Islem _islem = new Islem();
            try
            {

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {

                        var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.BeyanInternalNo && v.TescilStatu != "Tescil Edildi");
                        var beyannameContext = new BeyannameDataContext(options);
                        if (beyanValues != null)
                        {
                            _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.BeyanInternalNo);

                            beyan.TescilStatu = "Güncellendi";
                            beyannameContext.Entry(beyan).State = EntityState.Modified;
                            await beyannameContext.SaveChangesAsync();


                            _islem.IslemDurumu = "Güncellendi";
                            _islem.OlusturmaZamani = DateTime.Now;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();

                        }

                        else
                        {
                            var internalrefid = beyannameContext.GetRefIdNextSequenceValue(beyan.Rejim);
                            string InternalNo = beyan.Rejim + beyan.Kullanici + "DB" + internalrefid.ToString().PadLeft(5, '0');

                            beyan.BeyanInternalNo = InternalNo;
                            beyan.RefNo = InternalNo;
                            beyan.MusavirReferansNo = "BYT" + beyan.MusavirReferansNo;
                            beyan.TescilStatu = "Olusturuldu";
                            beyannameContext.Entry(beyan).State = EntityState.Added;
                            await beyannameContext.SaveChangesAsync();


                            _islem.Kullanici = beyan.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "DetayliBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = beyan.RefNo;
                            _islem.BeyanInternalNo = beyan.BeyanInternalNo;
                            _islem.IslemInternalNo = beyan.BeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
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

                return _servisDurum;
            }
            catch (Exception ex)
            {

                return null;
            }


        }


        [Route("api/BYT/Servis/Beyanname/[controller]/KalemSil/{kalemInternalNo}/{BeyanInternalNo}")]
        [HttpDelete("{kalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> DeleteKalem(string kalemInternalNo, string BeyanInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
               .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
               .Options;
            var _beyannameContext = new BeyannameDataContext(options);

            using (var transaction = _beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    var kalemValues = await _beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);

                    _beyannameContext.Entry(kalemValues).State = EntityState.Deleted;

                    var updateKalemNo = from u in _beyannameContext.DbKalem
                                        where u.BeyanInternalNo == BeyanInternalNo && u.KalemSiraNo > kalemValues.KalemSiraNo
                                        select u;

                    foreach (DbKalem itm in updateKalemNo)
                    {
                        itm.KalemSiraNo = itm.KalemSiraNo - 1;



                    }

                    var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
                    if (odemeValues.Count > 0)
                        foreach (var item in odemeValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                        }

                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
                    if (markaValues.Count > 0)
                        foreach (var item in markaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                        }

                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
                    if (konteynerValues.Count > 0)
                        foreach (var item in konteynerValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                        }

                    var acmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
                    if (acmaValues.Count > 0)
                        foreach (var item in acmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                        }

                    var tammalayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo).ToListAsync();
                    if (tammalayiciValues.Count > 0)
                        foreach (var item in markaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;
                        }

                    await _beyannameContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
                    List<Bilgi> lstBlg = new List<Bilgi>();
                    Bilgi blg = new Bilgi { IslemTipi = "Kalem Silme", ReferansNo = kalemValues.KalemInternalNo, Sonuc = "Kalem Silme Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/Servis/Beyanname/[controller]/KalemOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PutKalem([FromBody]DbKalem kalem)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;
        
            var kalemValues = await _beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == kalem.KalemInternalNo && v.BeyanInternalNo == kalem.BeyanInternalNo);
            var beyannameContext = new BeyannameDataContext(options);
            using (var transaction = beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    if (kalemValues != null)
                    {

                        beyannameContext.Entry(kalem).State = EntityState.Modified;
                    }
                    else
                    {
                        var countKalemNo = (from u in _beyannameContext.DbKalem
                                          where u.BeyanInternalNo == kalem.BeyanInternalNo
                                          select (u.KalemSiraNo)).Count();

                        if (countKalemNo>0)
                        {
                            var maxKalemNo = (from u in _beyannameContext.DbKalem
                                              where u.BeyanInternalNo == kalem.BeyanInternalNo
                                              select (u.KalemSiraNo)).Max();
                            var maxKalemInternalNo = (from u in _beyannameContext.DbKalem
                                                      where u.BeyanInternalNo == kalem.BeyanInternalNo
                                                      select (u.KalemInternalNo)).Max();

                            kalem.KalemSiraNo = Convert.ToInt32(maxKalemNo) + 1;
                            int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
                            kalem.KalemInternalNo = kalem.BeyanInternalNo + "|" + klNo;
                        }
                        else
                        {
                            kalem.KalemSiraNo =1;
                            kalem.KalemInternalNo = kalem.BeyanInternalNo + "|1";
                        }
                       

                   
                       
                        beyannameContext.Entry(kalem).State = EntityState.Added;
                    }

                    await beyannameContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
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


        [Route("api/BYT/Servis/Beyanname/[controller]/OdemeSekliOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostOdeme([FromBody]DbOdemeSekli[] odemeBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo== KalemInternalNo).ToListAsync();
                     
                        foreach (var item in odemeValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in odemeBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Ödeme Şekli Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Ödeme Şekşi Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/KonteynerOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostKonteyner([FromBody]DbKonteyner[] konteynerBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in konteynerValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in konteynerBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Konteyner Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Konteyner Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/TamamlayiciBilgiOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostTamamlayici([FromBody]DbTamamlayiciBilgi[] tamamlayiciBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in tamamlayiciValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in tamamlayiciBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Konteyner Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Konteyner Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/MarkaOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostMarka([FromBody]DbMarka[] markaBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in markaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in markaBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Marka Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Marka Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/BeyannameAcmaOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostBeyannameAcma([FromBody]DbBeyannameAcma[] acmaBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var acmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in acmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in acmaBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Beyanname Açma Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Beyanname Açma Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/VergiOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostVergiler([FromBody]DbVergi[] vergiBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var vergiValues = await _beyannameContext.DbVergi.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in vergiValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in vergiBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Vergi Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Vergi Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/BelgeOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostBelgeler([FromBody]DbBelge[] belgeBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var belgeValues = await _beyannameContext.DbBelge.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in belgeValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in belgeBilgiList)
                        {
                          
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
                Bilgi blg = new Bilgi { IslemTipi = "Belge Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Belge Oluşturma Başarılı", SonucVeriler = null };
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


        [Route("api/BYT/Servis/Beyanname/[controller]/SoruCevapOlustur/{KalemInternalNo}/{BeyanInternalNo}")]
        [HttpPost("{KalemInternalNo}/{BeyanInternalNo}")]
        public async Task<ServisDurum> PostSoruCevap([FromBody]DbSoruCevap[] soruCevapBilgiList, string KalemInternalNo, string BeyanInternalNo)
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
                        var sorucevapValues = await _beyannameContext.DbSoruCevap.Where(v => v.BeyanInternalNo == BeyanInternalNo && v.KalemInternalNo == KalemInternalNo).ToListAsync();

                        foreach (var item in sorucevapValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in soruCevapBilgiList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Soru Cevap Oluştur", ReferansNo = KalemInternalNo, Sonuc = "Soru Cevap Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/TeminatOlustur/{BeyanInternalNo}")]
        [HttpPost("{BeyanInternalNo}")]
        public async Task<ServisDurum> PostTeminat([FromBody]DbTeminat[] teminatList, string BeyanInternalNo)
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
                        var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == BeyanInternalNo).ToListAsync();

                        foreach (var item in teminatValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in teminatList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Teminat Oluştur", ReferansNo = BeyanInternalNo, Sonuc = "Teminat Oluşturma Başarılı", SonucVeriler = null };
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

        [Route("api/BYT/Servis/Beyanname/[controller]/FirmaOlustur/{BeyanInternalNo}")]
        [HttpPost("{BeyanInternalNo}")]
        public async Task<ServisDurum> PostFirma([FromBody]DbFirma[] firmaList, string BeyanInternalNo)
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
                        var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == BeyanInternalNo).ToListAsync();

                        foreach (var item in firmaValues)
                        {
                            _beyannameContext.Entry(item).State = EntityState.Deleted;

                        }
                        foreach (var item in firmaList)
                        {
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
                Bilgi blg = new Bilgi { IslemTipi = "Firma Oluştur", ReferansNo = BeyanInternalNo, Sonuc = "Firma Oluşturma Başarılı", SonucVeriler = null };
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
        [Route("api/BYT/Servis/Beyanname/[controller]/KiymetBildirimOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PutKiymet([FromBody]DbKalem kalem)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;

            var kalemValues = await _beyannameContext.DbKalem.FirstOrDefaultAsync(v => v.KalemInternalNo == kalem.KalemInternalNo && v.BeyanInternalNo == kalem.BeyanInternalNo);
            var beyannameContext = new BeyannameDataContext(options);
            using (var transaction = beyannameContext.Database.BeginTransaction())
            {
                try
                {
                    if (kalemValues != null)
                    {

                        beyannameContext.Entry(kalem).State = EntityState.Modified;
                    }
                    else
                    {
                        var maxKalemNo = (from u in _beyannameContext.DbKalem
                                          where u.BeyanInternalNo == kalem.BeyanInternalNo
                                          select (u.KalemSiraNo)).Max();
                        var maxKalemInternalNo = (from u in _beyannameContext.DbKalem
                                                  where u.BeyanInternalNo == kalem.BeyanInternalNo
                                                  select (u.KalemInternalNo)).Max();

                        kalem.KalemSiraNo = Convert.ToInt32(maxKalemNo) + 1;

                        int klNo = Convert.ToInt32(maxKalemInternalNo.Split('|')[1].ToString()) + 1;
                        kalem.KalemInternalNo = kalem.BeyanInternalNo + "|" + klNo;
                        beyannameContext.Entry(kalem).State = EntityState.Added;
                    }

                    await beyannameContext.SaveChangesAsync();
                    _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                    transaction.Commit();
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







    }

}