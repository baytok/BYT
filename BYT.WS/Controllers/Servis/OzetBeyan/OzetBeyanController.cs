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

        [Route("api/BYT/Servis/OzetBeyan/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<ObBeyan> GetBeyanname(string IslemInternalNo)
        {
            ObBeyan _beyanname = new ObBeyan();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                                     
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
            catch (Exception)
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

      
    }


}