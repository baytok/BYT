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

namespace BYT.WS.Controllers.Servis.DolasimBelgeleri
{
   // [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DolasimBelgeController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public DolasimBelgeController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/Servis/Dolasim/[controller]/{IslemInternalNo}/{kullaniciId}")]
        [HttpGet("{IslemInternalNo}/{kullaniciId}")]
        public async Task<Dolasim> GetDolasim(string IslemInternalNo, string kullaniciId)
        {
            bool beyannameYetkisi = false;
            Dolasim _dolasim = new Dolasim();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);

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
                        _dolasim = await _beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == islemValues.BeyanInternalNo);
                        return _dolasim;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "MU") > 0)
                    {
                        _dolasim = await _beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == islemValues.BeyanInternalNo && v.MusteriNo == kullanici.MusteriNo);
                        return _dolasim;
                    }
                    else if (kullaniciYetki.Result.Count(x => x.YetkiKodu == "FI") > 0)
                    {
                        _dolasim = await _beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == islemValues.BeyanInternalNo && v.MusteriNo == kullanici.MusteriNo && v.FirmaNo == kullanici.FirmaNo);
                        return _dolasim;
                    }
                    else
                    {
                        _dolasim = await _beyannameContext.Dolasim.FirstOrDefaultAsync(v => v.DolasimInternalNo == islemValues.BeyanInternalNo && v.TcKimlikNo == kullanici.KullaniciKod);
                        return _dolasim;
                    }
                }
                return _dolasim;


            }
            catch (Exception exc)
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
      

       
    }





}