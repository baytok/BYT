using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using BYT.WS.AltYapi;
using BYT.WS.Controllers.api;
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

namespace BYT.WS.Controllers.Servis.OzetBeyan
{
   
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public OzetBeyanController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/Servis/OzetBeyan/[controller]/{IslemInternalNo}/{kullaniciId}")]
        [HttpGet("{IslemInternalNo}/{kullaniciId}")]
        public async Task<ObBeyan> GetBeyanname(string IslemInternalNo, string kullaniciId)
        {
            bool beyannameYetkisi = false;
            ObBeyan _beyanname = new ObBeyan();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);

            var options2 = new DbContextOptionsBuilder<KullaniciDataContext>()
             .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
             .Options;
            KullaniciDataContext _kullaniciContext = new KullaniciDataContext(options2);

            try
            {
                var kullanici = _kullaniciContext.Kullanici.Where(x => x.KullaniciKod == kullaniciId.Trim()).FirstOrDefault();
                var kullaniciYetki = _kullaniciContext.KullaniciYetki.Where(x => x.KullaniciKod == kullaniciId.Trim()).ToListAsync();
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "AD") > 0)
                    {
                        beyannameYetkisi = true;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "MU") > 0)
                    {
                        if (islemValues.MusteriNo == kullanici.MusteriNo)
                            beyannameYetkisi = true;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "FI") > 0)
                    {
                        if (islemValues.MusteriNo == kullanici.MusteriNo && islemValues.FirmaNo == kullanici.FirmaNo)
                            beyannameYetkisi = true;
                    }
                    else
                    {
                        if (islemValues.Kullanici == kullanici.KullaniciKod)
                            beyannameYetkisi = true;
                    }
                }

                if (islemValues != null && beyannameYetkisi)
                {

                    if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "AD") > 0)
                    {
                        _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                        return _beyanname;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "MU") > 0)
                    {
                        _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.MusteriNo==kullanici.MusteriNo);
                        return _beyanname;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "FI") > 0)
                    {
                        _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.MusteriNo==kullanici.MusteriNo && v.FirmaNo==kullanici.FirmaNo);
                        return _beyanname;
                    }
                    else
                    {
                        _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.KullaniciKodu==kullanici.KullaniciKod);
                        return _beyanname;
                    }
                  

                }
             
                return _beyanname;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TasitUgrakUlke/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObTasitUgrakUlke>> GetTasitUgrakUlke(string IslemInternalNo)
        {
            List<ObTasitUgrakUlke> _tasit = new List<ObTasitUgrakUlke>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _tasitValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _tasit = _tasitValues;
                }

                return _tasit;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TasimaSenet/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObTasimaSenet>> GetTasimaSenet(string IslemInternalNo)
        {
            List<ObTasimaSenet> _senet = new List<ObTasimaSenet>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _senetValues = await _beyannameContext.ObTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _senet = _senetValues;
                }

                return _senet;

            }
            catch (Exception exc)
            {

                throw exc;
            }

        }


        [Route("api/BYT/Servis/Ihracat/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObIhracat>> GetIhracat(string IslemInternalNo)
        {
            List<ObIhracat> _ihracat = new List<ObIhracat>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _ihracat = ihracatValues;
                }

                return _ihracat;

            }
            catch (Exception exc)
            {

                throw;
            }

        }



        [Route("api/BYT/Servis/UgrakUlke/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObUgrakUlke>> GetUgrakUlke(string IslemInternalNo)
        {
            List<ObUgrakUlke> _ulke = new List<ObUgrakUlke>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var ulkeValues = await _beyannameContext.ObUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _ulke = ulkeValues;
                }

                return _ulke;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TasimaSatir/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObTasimaSatir>> GetTasimaSatir(string IslemInternalNo)
        {
            List<ObTasimaSatir> _satir = new List<ObTasimaSatir>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _satir = _satirValues;
                }

                return _satir;

            }
            catch (Exception)
            {

                throw;
            }

        }


        [Route("api/BYT/Servis/TasimaSatirEsya/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObSatirEsya>> GetTasimaSatirEsya(string IslemInternalNo)
        {
            List<ObSatirEsya> _satirEsya = new List<ObSatirEsya>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _satirEsya = _satirEsyaValues;
                }

                return _satirEsya;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TasiyiciFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<ObTasiyiciFirma> GetTasiyiciFirma(string IslemInternalNo)
        {
            ObTasiyiciFirma _Firma = new ObTasiyiciFirma();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __FirmaValues = await _beyannameContext.ObTasiyiciFirma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);

                    _Firma = __FirmaValues;
                }

                return _Firma;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/OzetBeyanAcma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObOzetBeyanAcma>> GetOzetBeyanAcma(string IslemInternalNo)
        {
            List<ObOzetBeyanAcma> _ozbyAcma = new List<ObOzetBeyanAcma>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var ozetBeyanAcmaValues = await _beyannameContext.ObOzetBeyanAcma.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _ozbyAcma = ozetBeyanAcmaValues;
                }

                return _ozbyAcma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/OzetBeyanAcmaTasimaSenet/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObOzetBeyanAcmaTasimaSenet>> GetOzetBeyanAcmaTasimaSenet(string IslemInternalNo)
        {
            List<ObOzetBeyanAcmaTasimaSenet> _tasimaSenet = new List<ObOzetBeyanAcmaTasimaSenet>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _tasimaSenetValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _tasimaSenet = _tasimaSenetValues;
                }

                return _tasimaSenet;

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("api/BYT/Servis/OzetBeyanAcmaTasimaSatir/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObOzetBeyanAcmaTasimaSatir>> GetOzetBeyanAcmaTasimaSatir(string IslemInternalNo)
        {
            List<ObOzetBeyanAcmaTasimaSatir> _tasimaSatir = new List<ObOzetBeyanAcmaTasimaSatir>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __tasimaSatirValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _tasimaSatir = __tasimaSatirValues;
                }

                return _tasimaSatir;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Teminat/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<ObTeminat>> GetTeminat(string IslemInternalNo)
        {
            List<ObTeminat> _teminat = new List<ObTeminat>();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var teminatValues = await _beyannameContext.ObTeminat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _teminat = teminatValues;
                }

                return _teminat;

            }
            catch (Exception exc)
            {

                throw;
            }

        }
    }


}