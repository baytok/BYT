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
    [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BeyannameSilmeController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public BeyannameSilmeController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpDelete("{IslemInternalNo}")]
        public async Task<ServisDurum> Delete(string IslemInternalNo)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    var tarihceValues = await _islemTarihceContext.Tarihce.Where(v => v.IslemInternalNo == islemValues.IslemInternalNo).ToListAsync();
                    var ozetBeyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var tcgbacmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSenet.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();

                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {

                            _beyannameContext.Entry(ozetBeyanValues).State = EntityState.Deleted;
                            foreach (var item in kalemValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in tamamlayiciValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in tcgbacmaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in markaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in konteynerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in odemeValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in teminatValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in firmaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
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
                            foreach (var item in kiymetValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in kiymetKalemValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in tarihceValues)
                            {
                                _islemTarihceContext.Entry(item).State = EntityState.Deleted;
                            }
                         
                            _islemTarihceContext.Entry(islemValues).State = EntityState.Deleted;

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