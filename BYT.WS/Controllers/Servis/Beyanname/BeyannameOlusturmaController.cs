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
                   
                        var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == beyan.BeyanInternalNo && v.TescilStatu!="Tescil Edilmiş");
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
                            string InternalNo = beyan.Kullanici + "DB" + internalrefid.ToString().PadLeft(6, '0');

                            beyan.BeyanInternalNo = InternalNo;
                            beyan.RefNo = "11111111100" + "|" + "1000" + "|" + internalrefid.ToString().PadLeft(6, '0');
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

     [Route("api/BYT/Servis/Beyanname/[controller]/BeyannameKopyalama/{IslemInternalNo}")]
     [HttpPost]
     public async Task<ServisDurum> PostBeyannameKopyalama(string IslemInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();
            string yeniIslemInternalNo = "11111111100DBKG000012";
            try
            {
                // Kopyalama Procedure
                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Beyanname Kopyalama", ReferansNo = yeniIslemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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
               
                return _servisDurum;
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
                        itm.KalemSiraNo = itm.KalemSiraNo-1;
                       
                    }


                    var odemeValues = await _beyannameContext.DbOdemeSekli.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
                    if(odemeValues!=null)
                    _beyannameContext.Entry(odemeValues).State = EntityState.Deleted;

                    var markaValues = await _beyannameContext.DbMarka.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
                    if (markaValues != null)
                        _beyannameContext.Entry(markaValues).State = EntityState.Deleted;

                    var konteynerValues = await _beyannameContext.DbKonteyner.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
                    if (konteynerValues != null)
                        _beyannameContext.Entry(konteynerValues).State = EntityState.Deleted;

                    var acmaValues = await _beyannameContext.DbBeyannameAcma.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
                    if (acmaValues != null)
                        _beyannameContext.Entry(acmaValues).State = EntityState.Deleted;

                    var tammalayiciValues = await _beyannameContext.DbTamamlayiciBilgi.FirstOrDefaultAsync(v => v.KalemInternalNo == kalemInternalNo && v.BeyanInternalNo == BeyanInternalNo);
                    if (tammalayiciValues != null)
                        _beyannameContext.Entry(tammalayiciValues).State = EntityState.Deleted;

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
                        var maxKalemNo = (from u in _beyannameContext.DbKalem
                                      where u.BeyanInternalNo == kalem.BeyanInternalNo
                                      select (u.KalemSiraNo)).Max();
                      
                        kalem.KalemSiraNo = Convert.ToInt32(maxKalemNo)+1;
                        kalem.KalemInternalNo = kalem.BeyanInternalNo + "|" + kalem.KalemSiraNo;
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


    [Route("api/BYT/Servis/Beyanname/[controller]/OdemeSekliOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostOdeme([FromBody]DbOdemeSekli odeme)
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


                    _beyannameContext.Entry(odeme).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = odeme.KalemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/MarkaOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostMarka([FromBody]DbMarka marka)
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


                    _beyannameContext.Entry(marka).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = marka.KalemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/KonteynerOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostKonteyner([FromBody]DbKonteyner konteyner)
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


                    _beyannameContext.Entry(konteyner).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = konteyner.KalemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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


    [Route("api/BYT/Servis/Beyanname/[controller]/BeyannameAcmaOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostBeyannameAcma([FromBody]DbBeyannameAcma acmabeyan)
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


                    _beyannameContext.Entry(acmabeyan).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = acmabeyan.KalemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/TamamlayiciBilgiOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostTamamlayiciBilgi([FromBody]DbTamamlayiciBilgi tamamlayici)
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


                    _beyannameContext.Entry(tamamlayici).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = tamamlayici.KalemInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/FirmaOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostFirma([FromBody]DbFirma firma)
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


                    _beyannameContext.Entry(firma).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = firma.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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


    [Route("api/BYT/Servis/Beyanname/[controller]/TeminatOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostTeminat([FromBody]DbTeminat teminat)
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


                    _beyannameContext.Entry(teminat).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = teminat.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/OzetBeyanOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostOzetBeyanAcma([FromBody]DbOzetbeyanAcma ozetbeyan)
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


                    _beyannameContext.Entry(ozetbeyan).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = ozetbeyan.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/TasimaSenetiOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostTasimaSenedi([FromBody]DbTasimaSatir tasima)
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


                    _beyannameContext.Entry(tasima).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = tasima.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/TasimaSatiriOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostTasimaSatir([FromBody]DbTasimaSatir tasima)
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


                    _beyannameContext.Entry(tasima).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = tasima.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/KiymetBildirimOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostKiymet([FromBody]DbKiymetBildirim kiymet)
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


                    _beyannameContext.Entry(kiymet).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = kiymet.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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

    [Route("api/BYT/Servis/Beyanname/[controller]/KiymetBildirimKalemOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostKiymetKalem([FromBody]DbKiymetBildirimKalem kiymet)
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


                    _beyannameContext.Entry(kiymet).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = kiymet.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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


    [Route("api/BYT/Servis/Beyanname/[controller]/VergiOlustur")]
    [HttpPost]
    public async Task<Sonuc<ServisDurum>> PostVergi([FromBody]DbVergi vergi)
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


                    _beyannameContext.Entry(vergi).State = EntityState.Added;
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

            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

            List<Bilgi> lstBlg = new List<Bilgi>();
            Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = vergi.BeyanInternalNo, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
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


}

}