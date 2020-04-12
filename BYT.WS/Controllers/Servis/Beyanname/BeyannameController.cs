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

namespace BYT.WS.Controllers.Servis.Beyanname
{
   // [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BeyannameController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public BeyannameController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/Servis/Beyanname/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<BeyannameBilgileri> GetBeyanname(string IslemInternalNo)
        {
            BeyannameBilgileri _beyanname = new BeyannameBilgileri();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tcgbacmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbTasimaSenedi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();



                    _beyanname.beyanname = beyanValues;
                    _beyanname.kalemler = kalemValues;
                    _beyanname.tamamlayiciBilgi = tamamlayiciValues;
                    _beyanname.tcgbAcma = tcgbacmaValues;
                    _beyanname.marka = markaValues;
                    _beyanname.odemeSekli = odemeValues;
                    _beyanname.konteyner = konteynerValues;
                    _beyanname.ozetBeyanAcma = ozetBeyanAcmaValues;
                    _beyanname.tasimaSenetleri = ozetBeyanAcmaTasimaSenediValues;
                    _beyanname.tasimaSatirlari = ozetBeyanAcmaTasimaSatirValues;
                    _beyanname.teminat = teminatValues;
                    _beyanname.firma = firmaValues;
                    _beyanname.kiymet = kiymetValues;
                    _beyanname.kiymetKalem = kiymetKalemValues;
                }

                return _beyanname;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Kalemler/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbKalem>> GetKalem(string IslemInternalNo)
        {
            List<DbKalem> _kalem = new List<DbKalem>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).OrderBy(x => x.KalemSiraNo).ToListAsync();

                    _kalem = kalemValues;
                }

                return _kalem;

            }
            catch (Exception)
            {

                throw;
            }

        }

      

        [Route("api/BYT/Servis/Odeme/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbOdemeSekli>> GetOdeme(string IslemInternalNo)
        {
            List<DbOdemeSekli> _odeme = new List<DbOdemeSekli>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _odeme = odemeValues;
                }

                return _odeme;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Konteyner/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbKonteyner>> GetKonteyner(string IslemInternalNo)
        {
            List<DbKonteyner> _konteyner = new List<DbKonteyner>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _konteyner = konteynerValues;
                }

                return _konteyner;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/TamamlayiciBilgi/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbTamamlayiciBilgi>> GetTamamlayiciBilgi(string IslemInternalNo)
        {
            List<DbTamamlayiciBilgi> _tamamlayici = new List<DbTamamlayiciBilgi>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _tamamlayici = tamamlayiciValues;
                }

                return _tamamlayici;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Marka/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbMarka>> GetMarka(string IslemInternalNo)
        {
            List<DbMarka> _marka = new List<DbMarka>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _marka = markaValues;
                }

                return _marka;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/KiymetBildirim/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbKiymetBildirim>> GetKiymet(string IslemInternalNo)
        {
            List<DbKiymetBildirim> _marka = new List<DbKiymetBildirim>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _marka = kiymetValues;
                }

                return _marka;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Teminat/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbTeminat>> GetTeminat(string IslemInternalNo)
        {
            List<DbTeminat> _teminat = new List<DbTeminat>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _teminat = teminatValues;
                }

                return _teminat;

            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("api/BYT/Servis/Firma/[controller]/{IslemInternalNo}")]
        [HttpGet("{IslemInternalNo}")]
        public async Task<List<DbFirma>> GetFirma(string IslemInternalNo)
        {
            List<DbFirma> _firma = new List<DbFirma>();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {

                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    _firma = firmaValues;
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