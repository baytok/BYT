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

        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public NctsSonucHizmetiController(BeyannameSonucDataContext sonucContext, IConfiguration configuration)
        {

            Configuration = configuration;
            _sonucContext = sonucContext;

        }

        [HttpGet("{IslemInternalNo}/{Guid}")]
        public async Task<BeyannameSonuc> Get(string IslemInternalNo, string Guid)
        {
            BeyannameSonuc beyanSonuc = new BeyannameSonuc();

            try
            {
                var _hatalar = await _sonucContext.DbSonucHatalar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
               
                return beyanSonuc;

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }





}