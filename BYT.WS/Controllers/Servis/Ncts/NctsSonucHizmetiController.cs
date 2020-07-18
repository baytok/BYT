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

namespace BYT.WS.Controllers.Servis.Ncts
{
    [Route("api/BYT/Servis/OzetBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NctsSonucHizmetiController : ControllerBase
    {
        private readonly ServisCredential _servisCredential;
        private IslemTarihceDataContext _islemTarihceContext;
        public IConfiguration Configuration { get; }

        public NctsSonucHizmetiController(IslemTarihceDataContext islemTarihcecontext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;

        }

        [HttpGet("{IslemInternalNo}/{Guid}")]
        public async Task<BeyannameSonuc> Get(string IslemInternalNo, string Guid)
        {
            BeyannameSonuc beyanSonuc = new BeyannameSonuc();

            try
            {
                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
              .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var islemValue= await _islemTarihceContext.Islem.FirstOrDefaultAsync(x=>x.IslemInternalNo==IslemInternalNo);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == islemValue.Kullanici).FirstOrDefaultAsync();
                string FIRM_ID = "BYT";
                string USER_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + "," + kullaniciValues.KullaniciKod + "," + kullaniciValues.KullaniciSifre;
                NctsHizmeti.WS2ServiceClient Tescil = ServiceHelper.GetNctsWSClient(_servisCredential.username, _servisCredential.password);
                //Tescil.getNotReadMessagesListAsync(FIRM_ID, USER_ID); // mail gibi kullanıcıları ile ilgili gelen mesajlara sık sık bakılacak, index veriyor
                var index= Tescil.getMessagesListByGuidAsync(FIRM_ID,USER_ID,Guid); // guid ile gidip mesajları, index veriyor
                //Tescil.downloadmessagebyindexAsync(FIRM_ID,USER_ID, index); // index ile mesajları alıyoruz.

                //var _hatalar = await _sonucContext.DbSonucHatalar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
               
                return beyanSonuc;

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }





}