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
    [Route("api/BYT/Servis/OzetBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanSonucHizmetiController : ControllerBase
    {

        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public OzetBeyanSonucHizmetiController(BeyannameSonucDataContext sonucContext, IConfiguration configuration)
        {

            Configuration = configuration;
            _sonucContext = sonucContext;

        }

        [HttpGet("{IslemInternalNo}/{Guid}")]
        public async Task<OzetBeyanXmlSonuc> Get(string IslemInternalNo, string Guid)
        {
            OzetBeyanXmlSonuc beyanSonuc = new OzetBeyanXmlSonuc();

            try
            {
                var _hatalar = await _sonucContext.ObSonucHatalar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _bilgiler = await _sonucContext.ObOzetBeyanSonuc.FirstOrDefaultAsync(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim());

                if (_bilgiler != null)
                {
                    beyanSonuc.TescilNo = _bilgiler.TescilNo;
                    beyanSonuc.TescilTarihi = _bilgiler.TescilTarihi;
                    beyanSonuc.KalemSayisi = _bilgiler.KalemSayisi;
                    
                }



                if (_hatalar.Count > 0)
                {

                    List<HataMesaji> lstHatalar = new List<HataMesaji>();
                    HataMesaji hatalar = new HataMesaji();
                    foreach (var item in _hatalar)
                    {
                        hatalar = new HataMesaji();
                        hatalar.HataKodu = item.HataKodu;
                        hatalar.HataAciklamasi = item.HataAciklamasi;

                        lstHatalar.Add(hatalar);
                    }

                    beyanSonuc.Hatalar = lstHatalar;
                }
                else beyanSonuc.Hatalar = new List<HataMesaji>();

          
                return beyanSonuc;

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }





}