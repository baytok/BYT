﻿using System;
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
    public class BeyannameController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public BeyannameController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpGet("{IslemInternalNo}")]
        public async Task<BeyannameBilgileri> Get(string IslemInternalNo)
        {
            BeyannameBilgileri _beyanname = new BeyannameBilgileri();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo);
                if (islemValues != null)
                {
                    var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tcgbacmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbTasimaSenedi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();



                    _beyanname.beyanname = beyanValues;
                    _beyanname.kalemler = kalemValues;
                    _beyanname.tamamlayiciBilgi = tamamlayiciValues;
                    _beyanname.tcgbAcma = tcgbacmaValues;
                    _beyanname.marka = markaValues;
                    _beyanname.odemeSekli = odemeValues;
                    _beyanname.konteyner = konteynerValues;
                    _beyanname.ozetBeyanAcma = ozetBeyanAcmaValues;
                    _beyanname.tasimaSenetleri = ozetBeyanAcmaTasimaSenediValues;
                    _beyanname.tasimaSatirlari = ozetBeyanAcmaTasimaSatirValues;
                    _beyanname.teminat = teminatValues;
                    _beyanname.firma = firmaValues;
                    _beyanname.kiymet = kiymetValues;
                    _beyanname.kiymetKalem = kiymetKalemValues;
                }

                return _beyanname;

            }
            catch (Exception)
            {

                throw;
            }

        }


    }





}