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
    [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DolasimBelgeSonucHizmetiController : ControllerBase
    {

        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public DolasimBelgeSonucHizmetiController(BeyannameSonucDataContext sonucContext, IConfiguration configuration)
        {

            Configuration = configuration;
            _sonucContext = sonucContext;

        }

        [HttpGet("{IslemInternalNo}/{Guid}")]
        public async Task<MesaiXmlSonuc> Get(string IslemInternalNo, string Guid)
        {
            MesaiXmlSonuc beyanSonuc = new MesaiXmlSonuc();

            try
            {
                var _hatalar = await _sonucContext.MesaiSonucHatalar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _bilgiler = await _sonucContext.MesaiSonuc.FirstOrDefaultAsync(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim());

                if (_bilgiler != null)
                {
                    beyanSonuc.MesaiID = _bilgiler.MesaiID;
                   
                }



                if (_hatalar.Count > 0)
                {

                    List<MesaiSonucHatalar> lstHatalar = new List<MesaiSonucHatalar>();
                    MesaiSonucHatalar hatalar = new MesaiSonucHatalar();
                    foreach (var item in _hatalar)
                    {
                        hatalar = new MesaiSonucHatalar();
                        hatalar.HataKodu = item.HataKodu;
                        hatalar.HataAciklamasi = item.HataAciklamasi;

                        lstHatalar.Add(hatalar);
                    }

                    beyanSonuc.Hatalar = lstHatalar;
                }
            
               
                return beyanSonuc;

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }





}