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
            catch (Exception)
            {

                throw;
            }

        }


    }





}