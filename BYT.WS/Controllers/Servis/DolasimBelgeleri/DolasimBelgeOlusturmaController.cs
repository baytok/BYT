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

namespace BYT.WS.Controllers.Servis.DolasimBelgeleri
{

    //[Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DolasimBelgeOlusturmaController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;
        private BeyannameDataContext _beyannameContext;
        public IConfiguration Configuration { get; }

        public DolasimBelgeOlusturmaController(IslemTarihceDataContext islemTarihceContext, IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _beyannameContext = new BeyannameDataContext(options);
            _islemTarihceContext = islemTarihceContext;

        }

       

        [Route("api/BYT/Servis/Dolasim/[controller]/DolasimBelgesiOlustur")]
        [HttpPost]
        public async Task<ServisDurum> PostDolasim([FromBody] Dolasim dolasim)
        {
          
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
           .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
           .Options;

            var options2 = new DbContextOptionsBuilder<KullaniciDataContext>()
           .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
           .Options;
            KullaniciDataContext _kullaniciContext = new KullaniciDataContext(options2);

            ServisDurum _servisDurum = new ServisDurum();

            List<Hata> _hatalar = new List<Hata>();

            Islem _islem = new Islem();
            try
            {
                var beyannameContext = new BeyannameDataContext(options);
                var dolasimValues = await beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == dolasim.DolasimInternalNo && v.TescilStatu != "Tescil Edildi");

                using (var transaction = _beyannameContext.Database.BeginTransaction())
                {
                    try
                    {

                   
                        if (dolasimValues != null)
                        {
                            dolasim.TescilStatu = "Guncellendi";
                            dolasim.SonIslemZamani = DateTime.Now;
                        
                            _beyannameContext.Entry(dolasim).State = EntityState.Modified;
                            await _beyannameContext.SaveChangesAsync();

                            _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == dolasim.DolasimInternalNo);

                            _islem.IslemDurumu = "Guncellendi";
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();

                        }

                        else
                        {
                            var internalrefid = beyannameContext.GetDolasimIdNextSequenceValue();
                            string InternalNo = dolasim.BelgeNo+ "ATR" + internalrefid.ToString().PadLeft(5, '0');
                            var kullanici = _kullaniciContext.Kullanici.Where(x => x.KullaniciKod == dolasim.TcKimlikNo).FirstOrDefault();
                            dolasim.MusteriNo = kullanici.MusteriNo;
                            dolasim.FirmaNo = kullanici.FirmaNo;

                            dolasim.DolasimInternalNo = InternalNo;
                            dolasim.TescilStatu = "Olusturuldu";
                            dolasim.OlsuturulmaTarihi = DateTime.Now;
                            dolasim.SonIslemZamani = DateTime.Now;
                            beyannameContext.Entry(dolasim).State = EntityState.Added;
                            await beyannameContext.SaveChangesAsync();

                            _islem.MusteriNo = dolasim.MusteriNo;
                            _islem.FirmaNo = dolasim.FirmaNo;
                            _islem.Kullanici = dolasim.TcKimlikNo;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "Dolasim";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = dolasim.RefNo;
                            _islem.BeyanInternalNo = dolasim.DolasimInternalNo;
                            _islem.IslemInternalNo = dolasim.DolasimInternalNo;
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
                Bilgi blg = new Bilgi { IslemTipi = "Dolaşım Oluştur", ReferansNo = dolasim.DolasimInternalNo, Sonuc = "Dolaşım Oluşturma Başarılı", SonucVeriler = null };
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


       

        [Route("api/BYT/Servis/Dolasim/[controller]/DolasimSil/{dolasimInternalNo}")]
        [HttpDelete("{dolasimInternalNo}")]
        public async Task<ServisDurum> DeleteDolasim(string dolasimInternalNo)
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
                    var dolasimValues = await _beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == dolasimInternalNo);

                    if (dolasimValues != null)
                    {
                        _beyannameContext.Entry(dolasimValues).State = EntityState.Deleted;

                        var _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.BeyanInternalNo == dolasimInternalNo);

                        _islemTarihceContext.Entry(_islem).State = EntityState.Deleted;
                        await _islemTarihceContext.SaveChangesAsync();

                        await _beyannameContext.SaveChangesAsync();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Dolaşım Silme", ReferansNo = dolasimInternalNo, Sonuc = "Dolaşım Silme Başarılı", SonucVeriler = null };
                        lstBlg.Add(blg);
                        _servisDurum.Bilgiler = lstBlg;
                        return _servisDurum;
                    }
                    else
                    {
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarisiz;
                        transaction.Commit();
                        List<Bilgi> lstBlg = new List<Bilgi>();
                        Bilgi blg = new Bilgi { IslemTipi = "Dolaşım Silme", ReferansNo = dolasimInternalNo, Sonuc = "Dolaşım Silme Başarısız", SonucVeriler = null };
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

    }

}