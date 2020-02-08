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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.api
{
    [Route("api/BYT/[controller]")]
    [ApiController]
    public class TarihceHizmetiController : ControllerBase
    {
       
        private IslemTarihceDataContext _islemTarihceContext;
        private readonly ServisCredential _servisCredential;
       
        public TarihceHizmetiController(IslemTarihceDataContext islemTarihceContext, IOptions<ServisCredential> servisCredential)
        {
            _islemTarihceContext = islemTarihceContext;
          
              _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpGet("{IslemInternalNo}")]
        public async Task<List<Tarihce>> GetTarihce(string IslemInternalNo)
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var tarihceValues = await _islemTarihceContext.Tarihce.Where(v => v.IslemInternalNo == IslemInternalNo).ToListAsync();

                //_servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                //List<Bilgi> lstBlg = new List<Bilgi>();
                //Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = IslemInternalNo, Sonuc = "Sorgulama Başarılı", SonucVeriler = tarihceValues };
                //lstBlg.Add(blg);
                //_servisDurum.Bilgiler = lstBlg;

                //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };
              

                return tarihceValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [HttpPut("{Guid}")]
        public async Task<ActionResult> PutTarihce(string Guid)
        {
            try
            {

                SonucHizmeti.GumrukWSSoapClient sonuc = ServiceHelper.GetSonucWSClient(_servisCredential.username, _servisCredential.password);


                var values = await sonuc.IslemSonucGetir4Async(Guid);


                //var islemValues = await _islemContext.Islem.Update(v => v.InternalNo == InternalNo);
                //TODO: Tarihçeye ve isleme yaz
                return Ok(values);
            }
            catch (Exception ex)
            {

                return null;
            }

          
        }
    }

   
       

       
}