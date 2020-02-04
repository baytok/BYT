using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BYT.WS.Controllers.api
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BilgiHizmetiController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private BilgiDataContext _bilgiContext;
        public BilgiHizmetiController(IConfiguration configuration)
        {
            Configuration = configuration;
            var options = new DbContextOptionsBuilder<BilgiDataContext>()
                       .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                       .Options;
            _bilgiContext = new BilgiDataContext(options);
        }

        [Route("api/[controller]/ReferansId/{Rejim}")]
        [HttpPost("{Rejim}")]
        public async Task<Sonuc<ServisDurum>> PostRefId(string Rejim)
        {
            try
            {

                ServisDurum _servisDurum = new ServisDurum();
                
                var results = _bilgiContext.GetRefIdNextSequenceValue(Rejim);
                int? nextSequenceValue = results;

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = nextSequenceValue.ToString(), Sonuc = "Sorgulama Başarılı", SonucVeriler = nextSequenceValue };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };

                return result;

            }
            catch (Exception ex)
            {

                return null;
            }


        }


       


    }





}