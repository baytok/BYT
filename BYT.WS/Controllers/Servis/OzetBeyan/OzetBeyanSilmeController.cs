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
    [Route("api/BYT/Servis/OzetBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanSilmeController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;
        private readonly ServisCredential _servisCredential;
       
        public IConfiguration Configuration { get; }

        public OzetBeyanSilmeController(IslemTarihceDataContext islemTarihcecontext,  IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
          

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpDelete("{IslemInternalNo}")]
        public async Task<ServisDurum> Delete(string IslemInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    var tarihceValues = await _islemTarihceContext.Tarihce.Where(v => v.IslemInternalNo == islemValues.IslemInternalNo).ToListAsync();
                    var ozetBeyanValues = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                    var senetValues = await _beyannameContext.ObTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ugrakValues = await _beyannameContext.ObUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var firmaValues = await _beyannameContext.ObTasiyiciFirma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                    var teminatValues = await _beyannameContext.ObTeminat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tasitUgrakValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaValues = await _beyannameContext.ObOzetBeyanAcma.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {

                            _beyannameContext.Entry(ozetBeyanValues).State = EntityState.Deleted;
                            foreach (var item in senetValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in ugrakValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in ihracatValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in satirValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in satirEsyaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in tasitUgrakValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in teminatValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                        
                            if(firmaValues!=null)
                            _beyannameContext.Entry(firmaValues).State = EntityState.Deleted;

                            foreach (var item in ozetBeyanAcmaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in ozetBeyanAcmaTasimaSenediValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in ozetBeyanAcmaTasimaSatirValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                          
                            _islemTarihceContext.Entry(islemValues).State = EntityState.Deleted;
                            foreach (var item in tarihceValues)
                            {
                                _islemTarihceContext.Entry(item).State = EntityState.Deleted;
                            }

                            await _beyannameContext.SaveChangesAsync();
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();
                            lstbilgi.Add(new Bilgi { IslemTipi = "Beyanname Silme", ReferansNo = IslemInternalNo, Sonuc = "Silme Başarılı", SonucVeriler = null, GUID = null });
                            _servisDurum.Bilgiler = lstbilgi;

                            return _servisDurum;
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                            List<Internal.Hata> lstht = new List<Internal.Hata>();

                            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                            lstht.Add(ht);
                            _servisDurum.Hatalar = lstht;

                            return _servisDurum;

                        }

                    }
                }



            }
            catch (Exception ex)
            {

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                List<Internal.Hata> lstht = new List<Internal.Hata>();

                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                lstht.Add(ht);
                _servisDurum.Hatalar = lstht;

                return _servisDurum;
            }
            return _servisDurum;
        }


    }





}