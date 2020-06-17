using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.Data;
using BYT.WS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace BYT.WS.Controllers.Servis.Ncts
{
   // [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NctsController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public NctsController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/Servis/NctsBeyan/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbBeyan> GetBeyanname(string IslemInternalNo)
        {
            NbBeyan nctsBeyanValues = new NbBeyan();
              var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                     nctsBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                                     
                }
                return nctsBeyanValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/BeyanSahibi/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbBeyanSahibi> GetBeyanSahibi(string IslemInternalNo)
        {
            NbBeyanSahibi _firma = new NbBeyanSahibi();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbBeyanSahibi.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("api/BYT/Servis/AsilSorumluFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbAsilSorumluFirma> GetAsilSorumlu(string IslemInternalNo)
        {
            NbAsilSorumluFirma _firma = new NbAsilSorumluFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbAsilSorumluFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TasiyiFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbTasiyiciFirma> GetTasiyici(string IslemInternalNo)
        {
            NbTasiyiciFirma _firma = new NbTasiyiciFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbTasiyiciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/AliciFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbAliciFirma> GetAlici(string IslemInternalNo)
        {
            NbAliciFirma _firma = new NbAliciFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/GondericiFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbGondericiFirma> GetGonderici(string IslemInternalNo)
        {
            NbGondericiFirma _firma = new NbGondericiFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/GuvenliAliciFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbGuvenliAliciFirma> GetGuvenliAlici(string IslemInternalNo)
        {
            NbGuvenliAliciFirma _firma = new NbGuvenliAliciFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/GuvenliGondericiFirma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<NbGuvenliGondericiFirma> GetGuvenliGonderici(string IslemInternalNo)
        {
            NbGuvenliGondericiFirma _firma = new NbGuvenliGondericiFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Teminat/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbTeminat>> GetTeminat(string IslemInternalNo)
        {
            List<NbTeminat> _teminat = new List<NbTeminat>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _teminatValues = await _beyannameContext.NbTeminat.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync(); 

                    _teminat = _teminatValues;
                }

                return _teminat;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Muhur/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbMuhur>> GetMuhur(string IslemInternalNo)
        {
            List<NbMuhur> _muhur = new List<NbMuhur>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _muhurValues = await _beyannameContext.NbMuhur.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _muhur = _muhurValues;
                }

                return _muhur;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Rota/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbRota>> GetRota(string IslemInternalNo)
        {
            List<NbRota> _rota = new List<NbRota>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _rotaValues = await _beyannameContext.NbRota.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _rota = _rotaValues;
                }

                return _rota;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TransitGumruk/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbTransitGumruk>> GetTransitGumruk(string IslemInternalNo)
        {
            List<NbTransitGumruk> _transit = new List<NbTransitGumruk>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _transitValues = await _beyannameContext.NbTransitGumruk.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _transit = _transitValues;
                }

                return _transit;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Kalem/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbKalem>> GetKalem(string IslemInternalNo)
        {
            List<NbKalem> _kalem = new List<NbKalem>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __kalemValues = await _beyannameContext.NbKalem.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _kalem = __kalemValues;
                }

                return _kalem;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/KalemAliciFirma/[controller]/{IslemInternalNo}/{kalemInternalNo}")]
        [HttpGet("{IslemInternalNo}/{kalemInternalNo}")]
        public async Task<NbKalemAliciFirma> GetKalemAlici(string IslemInternalNo, string kalemInternalNo)
        {
            NbKalemAliciFirma _firma = new NbKalemAliciFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbKalemAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo== kalemInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/KalemGondericiFirma/[controller]/{IslemInternalNo}/{kalemInternalNo}")]
        [HttpGet("{IslemInternalNo}/{kalemInternalNo}")]
        public async Task<NbKalemGondericiFirma> GetKalemGonderici(string IslemInternalNo, string kalemInternalNo)
        {
            NbKalemGondericiFirma _firma = new NbKalemGondericiFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbKalemGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo== kalemInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/KalemGuvenliAliciFirma/[controller]/{IslemInternalNo}/{kalemInternalNo}")]
        [HttpGet("{IslemInternalNo}/{kalemInternalNo}")]
        public async Task<NbKalemGuvenliAliciFirma> GetKalemGuvenliAlici(string IslemInternalNo, string kalemInternalNo)
        {
            NbKalemGuvenliAliciFirma _firma = new NbKalemGuvenliAliciFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbKalemGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == kalemInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/KalemGuvenliGondericiFirma/[controller]/{IslemInternalNo}/{kalemInternalNo}")]
        [HttpGet("{IslemInternalNo}/{kalemInternalNo}")]
        public async Task<NbKalemGuvenliGondericiFirma> GetKalemGuvenliGonderici(string IslemInternalNo, string kalemInternalNo)
        {
            NbKalemGuvenliGondericiFirma _firma = new NbKalemGuvenliGondericiFirma();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kalemValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == kalemInternalNo);

                    _firma = kalemValues;
                }

                return _firma;

            }
            catch (Exception exc)
            {

                throw;
            }

        }


        [Route("api/BYT/Servis/Konteyner/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbKonteyner>> GetKonteyner(string IslemInternalNo)
        {
            List<NbKonteyner> _konteyner = new List<NbKonteyner>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _konteyner = _konteynerValues;
                }

                return _konteyner;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/HassasEsya/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbHassasEsya>> GetHassasEsya(string IslemInternalNo)
        {
            List<NbHassasEsya> _hassasEsya = new List<NbHassasEsya>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _hassasEsyaValues = await _beyannameContext.NbHassasEsya.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _hassasEsya = _hassasEsyaValues;
                }

                return _hassasEsya;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Kap/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbKap>> GetKap(string IslemInternalNo)
        {
            List<NbKap> _kap = new List<NbKap>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __kapValues = await _beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _kap = __kapValues;
                }

                return _kap;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/EkBilgi/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbEkBilgi>> GetEkBilgi(string IslemInternalNo)
        {
            List<NbEkBilgi> _ekBilgi = new List<NbEkBilgi>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var ___ekBilgiValues = await _beyannameContext.NbEkBilgi.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _ekBilgi = ___ekBilgiValues;
                }

                return _ekBilgi;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Belgeler/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbBelgeler>> GetBelgeler(string IslemInternalNo)
        {
            List<NbBelgeler> _belgeler = new List<NbBelgeler>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _belgeler = __belgelerValues;
                }

                return _belgeler;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/OncekiBelgeler/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbOncekiBelgeler>> GetOncekiBelgeler(string IslemInternalNo)
        {
            List<NbOncekiBelgeler> _belgeler = new List<NbOncekiBelgeler>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __belgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _belgeler = __belgelerValues;
                }

                return _belgeler;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/ObAcma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbObAcma>> GetObAcma(string IslemInternalNo)
        {
            List<NbObAcma> _obAcma = new List<NbObAcma>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var __obAcmaValues = await _beyannameContext.NbObAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _obAcma = __obAcmaValues;
                }

                return _obAcma;

            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/AbAcma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<NbAbAcma>> GetAbAcma(string IslemInternalNo)
        {
            List<NbAbAcma> _abAcma = new List<NbAbAcma>();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var _abAcmaValues = await _beyannameContext.NbAbAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _abAcma = _abAcmaValues;
                }

                return _abAcma;

            }
            catch (Exception exc)
            {

                throw;
            }

        }
    }





}