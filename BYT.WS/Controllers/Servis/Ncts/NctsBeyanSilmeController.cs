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
    [Route("api/BYT/Servis/NctsBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NctsBeyanSilmeController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;
        private readonly ServisCredential _servisCredential;
       
        public IConfiguration Configuration { get; }

        public NctsBeyanSilmeController(IslemTarihceDataContext islemTarihcecontext,  IOptions<ServisCredential> servisCredential, IConfiguration configuration)
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
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    var tarihceValues = await _islemTarihceContext.Tarihce.Where(v => v.IslemInternalNo == islemValues.IslemInternalNo).ToListAsync();
                    var nctsBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    var beyanSahibiValues = await _beyannameContext.NbBeyanSahibi.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    var tasiyiciValues = await _beyannameContext.NbTasiyiciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo );
                    var asilSorumluValues = await _beyannameContext.NbAsilSorumluFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo );
                    var aliciValues = await _beyannameContext.NbAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo );
                    var gondericiValues = await _beyannameContext.NbGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo );
                    var guvenliAliciValues = await _beyannameContext.NbGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    var guvenliGondericiValues = await _beyannameContext.NbGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.NbKalem.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var teminatValues = await _beyannameContext.NbTeminat.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var muhurValues = await _beyannameContext.NbMuhur.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo ).ToListAsync();
                    var rotaValues = await _beyannameContext.NbRota.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var transitGumrukValues = await _beyannameContext.NbTransitGumruk.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kalemAliciValues = await _beyannameContext.NbKalemAliciFirma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kalemGondericiiValues = await _beyannameContext.NbKalemGondericiFirma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kalemGuvenliAliciValues = await _beyannameContext.NbKalemGuvenliAliciFirma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kalemGuvenliGondericiValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var hassasEsyaValues = await _beyannameContext.NbHassasEsya.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kapValues = await _beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ekBilgiValues = await _beyannameContext.NbEkBilgi.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var oncekiBelgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var obAcmaValues = await _beyannameContext.NbObAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var abAcmaValues = await _beyannameContext.NbAbAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();


                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {

                            _beyannameContext.Entry(nctsBeyanValues).State = EntityState.Deleted;
                            if (beyanSahibiValues != null)
                                _beyannameContext.Entry(beyanSahibiValues).State = EntityState.Deleted;
                            if (tasiyiciValues != null)
                                _beyannameContext.Entry(tasiyiciValues).State = EntityState.Deleted;
                            if (asilSorumluValues != null)
                                _beyannameContext.Entry(asilSorumluValues).State = EntityState.Deleted;
                            if (aliciValues != null)
                                _beyannameContext.Entry(aliciValues).State = EntityState.Deleted;
                            if (gondericiValues != null)
                                _beyannameContext.Entry(gondericiValues).State = EntityState.Deleted;
                            if (guvenliAliciValues != null)
                                _beyannameContext.Entry(guvenliAliciValues).State = EntityState.Deleted;
                            if (guvenliGondericiValues != null)
                                _beyannameContext.Entry(guvenliGondericiValues).State = EntityState.Deleted;

                            foreach (var item in kalemValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                            foreach (var item in teminatValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in muhurValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in rotaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in transitGumrukValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                            foreach (var item in kalemAliciValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in kalemGondericiiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in kalemGuvenliAliciValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in kalemGuvenliGondericiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in konteynerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in hassasEsyaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }

                            foreach (var item in kapValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in ekBilgiValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in belgelerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in oncekiBelgelerValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in obAcmaValues)
                            {
                                _beyannameContext.Entry(item).State = EntityState.Deleted;
                            }
                            foreach (var item in abAcmaValues)
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