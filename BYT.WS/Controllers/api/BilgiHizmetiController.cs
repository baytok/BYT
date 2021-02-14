using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BYT.WS.Controllers.api
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BilgiHizmetiController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private BilgiDataContext _bilgiContext;
        public BilgiHizmetiController(IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<BilgiDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _bilgiContext = new BilgiDataContext(options);
        }

        [Route("api/BYT/[controller]/ReferansId/{Rejim}")]
        [HttpPost("{Rejim}")]
        public async Task<Sonuc<ServisDurum>> PostRefId(string Rejim)
        {
            try
            {

                ServisDurum _servisDurum = new ServisDurum();
                
                var results = _bilgiContext.GetRefIdNextSequenceValue(Rejim.Trim());
                int? nextSequenceValue = results;

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = nextSequenceValue.ToString(), Sonuc = "Sorgulama Başarılı", SonucVeriler = nextSequenceValue };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };

                return result;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/OzetBeyanAlan/[controller]/{Tip}")]
        [HttpPost("{Tip}")]
        public async Task<ObBeyanAlan> PostOzetBeyanAlanlar(string Tip)
        {
            try
            {
           

                ServisDurum _servisDurum = new ServisDurum();

                var results = await _bilgiContext.ObBeyanAlan.FirstOrDefaultAsync(z=>z.Tip == Tip.Trim());
           

                return results;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/Istatistik/[controller]/{KullaniciKod}")]
        [HttpGet("{KullaniciKod}")]
        public async Task<Istatistik> GetIstatistik(string KullaniciKod)
        {
            var options2 = new DbContextOptionsBuilder<KullaniciDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;
            KullaniciDataContext _kullaniciContext = new KullaniciDataContext(options2);

            try
            {

                Istatistik _istatistik = new Istatistik();

                var kullanici = _kullaniciContext.Kullanici.Where(x => x.KullaniciKod == KullaniciKod.Trim()).FirstOrDefault();
                var kullaniciYetki = _kullaniciContext.KullaniciYetki.Where(x => x.KullaniciKod == KullaniciKod.Trim()).ToListAsync();
                if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "AD") > 0)
                {
                    var resultIslem = _bilgiContext.Islem.ToList();
                    var resultTarihce = _bilgiContext.Tarihce.ToList();
                    var resultBeyan = _bilgiContext.DbBeyan.ToList();

                    _istatistik.KontrolGonderimSayisi = resultIslem.Where(x => x.IslemTipi == "Kontrol").Sum(y => y.GonderimSayisi);
                    _istatistik.BeyannameSayisi = resultBeyan.Count();
                    _istatistik.TescilBeyannameSayisi = resultTarihce.Where(x => x.IslemTipi == "Tescil" && x.BeyanNo != null).Count();
                    _istatistik.SonucBeklenenSayisi = resultIslem.Where(x => x.IslemDurumu == "Gonderildi").Count();
                }
                else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "MU") > 0)
                {
                    var resultIslem = _bilgiContext.Islem.Where(x => x.MusteriNo == kullanici.MusteriNo).ToList();

                    var resultTarihce = from a in _bilgiContext.Tarihce
                                        join b in _bilgiContext.Islem on a.IslemInternalNo equals b.IslemInternalNo
                                        where b.MusteriNo == kullanici.MusteriNo
                                        select new { a.IslemTipi, a.BeyanNo };

                  
                    var resultBeyan = _bilgiContext.DbBeyan.Where(x => x.MusteriNo == kullanici.MusteriNo).ToList();

                    _istatistik.KontrolGonderimSayisi = resultIslem.Where(x => x.IslemTipi == "Kontrol").Sum(y => y.GonderimSayisi);
                    _istatistik.BeyannameSayisi = resultBeyan.Count();
                    _istatistik.TescilBeyannameSayisi = resultTarihce.Where(x => x.IslemTipi == "Tescil" && x.BeyanNo != null).Count();
                    _istatistik.SonucBeklenenSayisi = resultIslem.Where(x => x.IslemDurumu == "Gonderildi").Count();
                }
                else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "FI") > 0)
                {
                    var resultIslem = _bilgiContext.Islem.Where(x => x.FirmaNo == kullanici.FirmaNo && x.MusteriNo== kullanici.MusteriNo).ToList();
                    var resultTarihce = from a in _bilgiContext.Tarihce
                                        join b in _bilgiContext.Islem on a.IslemInternalNo equals b.IslemInternalNo
                                        where b.MusteriNo == kullanici.MusteriNo && b.FirmaNo == kullanici.FirmaNo
                                        select new { a.IslemTipi, a.BeyanNo };
                  //  var resultTarihce = _bilgiContext.Tarihce.Where(x => x.FirmaNo == kullanici.FirmaNo && x.MusteriNo == kullanici.MusteriNo).ToList();
                    var resultBeyan = _bilgiContext.DbBeyan.Where(x => x.FirmaNo == kullanici.FirmaNo && x.MusteriNo == kullanici.MusteriNo).ToList();

                    _istatistik.KontrolGonderimSayisi = resultIslem.Where(x => x.IslemTipi == "Kontrol").Sum(y => y.GonderimSayisi);
                    _istatistik.BeyannameSayisi = resultBeyan.Count();
                    _istatistik.TescilBeyannameSayisi = resultTarihce.Where(x => x.IslemTipi == "Tescil" && x.BeyanNo != null).Count();
                    _istatistik.SonucBeklenenSayisi = resultIslem.Where(x => x.IslemDurumu == "Gonderildi").Count();
                }
                else
                {
                    var resultIslem = _bilgiContext.Islem.Where(x => x.Kullanici == kullanici.KullaniciKod ).ToList();
                    var resultTarihce = _bilgiContext.Tarihce.Where(x => x.Kullanici == kullanici.KullaniciKod ).ToList();
                    var resultBeyan = _bilgiContext.DbBeyan.Where(x => x.Kullanici == kullanici.KullaniciKod ).ToList();

                    _istatistik.KontrolGonderimSayisi = resultIslem.Where(x => x.IslemTipi == "Kontrol").Sum(y => y.GonderimSayisi);
                    _istatistik.BeyannameSayisi = resultBeyan.Count();
                    _istatistik.TescilBeyannameSayisi = resultTarihce.Where(x => x.IslemTipi == "Tescil" && x.BeyanNo != null).Count();
                    _istatistik.SonucBeklenenSayisi = resultIslem.Where(x => x.IslemDurumu == "Gonderildi").Count();
                }

             
                //_servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                //List<Bilgi> lstBlg = new List<Bilgi>();
                //Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = KullaniciKod, Sonuc = "Sorgulama Başarılı", SonucVeriler = nextSequenceValue };
                //lstBlg.Add(blg);
                //_servisDurum.Bilgiler = lstBlg;

                //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };

                return _istatistik;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

       
    }





}