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
            try
            {

                Istatistik _istatistik = new Istatistik();

                var resultIslem = _bilgiContext.Islem.Where(x=>x.Kullanici==KullaniciKod.Trim()).ToList();
                var resultTarihce = _bilgiContext.Tarihce.Where(x => x.Kullanici == KullaniciKod.Trim()).ToList();
                var resultBeyan = _bilgiContext.DbBeyan.Where(x => x.Kullanici == KullaniciKod.Trim()).ToList();

                _istatistik.KontrolGonderimSayisi = resultIslem.Where(x=>x.IslemTipi=="Kontrol").Sum(y=>y.GonderimSayisi);
                _istatistik.BeyannameSayisi = resultBeyan.Count();
                _istatistik.TescilBeyannameSayisi = resultTarihce.Where(x => x.IslemTipi == "Tescil" && x.BeyanNo!=null).Count();
                _istatistik.SonucBeklenenSayisi = resultIslem.Where(x => x.IslemDurumu == "Gonderildi").Count();
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