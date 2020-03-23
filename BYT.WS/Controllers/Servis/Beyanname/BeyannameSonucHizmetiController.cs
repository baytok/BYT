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
    public class BeyannameSonucHizmetiController : ControllerBase
    {

        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public BeyannameSonucHizmetiController(BeyannameSonucDataContext sonucContext, IConfiguration configuration)
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
                var _belgeler = await _sonucContext.DbSonucBelgeler.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _vergiler = await _sonucContext.DbSonucVergiler.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _sorular = await _sonucContext.DbSonucSorular.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _sorucevaplar = await _sonucContext.DbSonucSoruCevaplar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _toplamvergiler = await _sonucContext.DbSonucToplamVergiler.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _toplananvergiler = await _sonucContext.DbSonucToplananVergiler.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _hesapdetaylar = await _sonucContext.DbSonucHesapDetaylar.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _ozetbeyan = await _sonucContext.DbSonucOzetBeyan.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _gumrukkiymeti = await _sonucContext.DbSonucGumrukKiymeti.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _istatitikikiymet = await _sonucContext.DbSonucIstatistikiKiymet.Where(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim()).ToListAsync();
                var _digerbilgiler = await _sonucContext.DbSonucDigerBilgiler.FirstOrDefaultAsync(v => v.Guid == Guid.Trim() && v.IslemInternalNo == IslemInternalNo.Trim());

                if (_digerbilgiler != null)
                {
                    beyanSonuc.DovizKuruAlis = _digerbilgiler.DovizKuruAlis;
                    beyanSonuc.DovizKuruSatis = _digerbilgiler.DovizKuruSatis;
                    beyanSonuc.CiktiSeriNo = _digerbilgiler.CiktiSeriNo;
                    beyanSonuc.MuayeneMemuru = _digerbilgiler.MuayeneMemuru;
                    beyanSonuc.KalanKontor = _digerbilgiler.KalanKontor;
                }



                if (_hatalar.Count > 0)
                {

                    List<DbSonucHatalar> lstHatalar = new List<DbSonucHatalar>();
                    DbSonucHatalar hatalar = new DbSonucHatalar();
                    foreach (var item in _hatalar)
                    {
                        hatalar = new DbSonucHatalar();
                        hatalar.HataKodu = item.HataKodu;
                        hatalar.HataAciklamasi = item.HataAciklamasi;

                        lstHatalar.Add(hatalar);
                    }

                    beyanSonuc.Hatalar = lstHatalar;
                }
                else beyanSonuc.Hatalar = new List<DbSonucHatalar>();

                if (_belgeler.Count > 0)
                {
                    List<DbSonucBelgeler> lstBelgeler = new List<DbSonucBelgeler>();
                    DbSonucBelgeler belgeler = new DbSonucBelgeler();
                    foreach (var item in _belgeler)
                    {
                        belgeler = new DbSonucBelgeler();
                        belgeler.BelgeKodu = item.BelgeKodu;
                        belgeler.BelgeAciklamasi = item.BelgeAciklamasi;
                        belgeler.KalemNo = item.KalemNo;
                        belgeler.Dogrulama = item.Dogrulama;
                        belgeler.Referans = item.Referans;
                        belgeler.BelgeTarihi = item.BelgeTarihi;
                        lstBelgeler.Add(belgeler);
                    }

                    beyanSonuc.Belgeler = lstBelgeler;
                }
                else beyanSonuc.Belgeler = new List<DbSonucBelgeler>();

                if (_vergiler.Count > 0)
                {
                    List<DbSonucVergiler> lstVergiler = new List<DbSonucVergiler>();
                    DbSonucVergiler vergiler = new DbSonucVergiler();
                    foreach (var item in _vergiler)
                    {
                        vergiler = new DbSonucVergiler();
                        vergiler.VergiKodu = item.VergiKodu;
                        vergiler.VergiAciklamasi = item.VergiAciklamasi;
                        vergiler.Oran = item.Oran;
                        vergiler.Miktar = item.Miktar;
                        vergiler.Matrah = item.Matrah;
                        vergiler.OdemeSekli = item.OdemeSekli;
                        vergiler.KalemNo = item.KalemNo;
                        lstVergiler.Add(vergiler);
                    }

                    beyanSonuc.Vergiler = lstVergiler;
                }
                else beyanSonuc.Vergiler = new List<DbSonucVergiler>();

                if (_sorular.Count > 0)
                {
                    List<DbSonucSorular> lstSorular = new List<DbSonucSorular>();
                    DbSonucSorular sorular = new DbSonucSorular();
                    foreach (var item in _sorular)
                    {
                        sorular = new DbSonucSorular();
                        sorular.SoruKodu = item.SoruKodu;
                        sorular.SoruAciklamasi = item.SoruAciklamasi;
                        sorular.KalemNo = item.KalemNo;
                        lstSorular.Add(sorular);
                    }

                    beyanSonuc.Sorular = lstSorular;
                }
                else beyanSonuc.Sorular = new List<DbSonucSorular>();

                if (_sorucevaplar.Count > 0)
                {
                    List<DbSonucSoruCevaplar> lstSoruCevaplar = new List<DbSonucSoruCevaplar>();
                    DbSonucSoruCevaplar soruCevaplar = new DbSonucSoruCevaplar();
                    foreach (var item in _sorucevaplar)
                    {
                        soruCevaplar = new DbSonucSoruCevaplar();
                        soruCevaplar.SoruKodu = item.SoruKodu;
                        soruCevaplar.SoruCevap = item.SoruCevap;
                        soruCevaplar.KalemNo = item.KalemNo;

                        lstSoruCevaplar.Add(soruCevaplar);
                    }

                    beyanSonuc.SoruCevap = lstSoruCevaplar;
                }
                else beyanSonuc.SoruCevap = new List<DbSonucSoruCevaplar>();


                if (_toplamvergiler.Count > 0)
                {
                    List<DbSonucToplamVergiler> lstToplamVergiler = new List<DbSonucToplamVergiler>();
                    DbSonucToplamVergiler toplamVergiler = new DbSonucToplamVergiler();
                    foreach (var item in _toplamvergiler)
                    {
                        toplamVergiler = new DbSonucToplamVergiler();
                        toplamVergiler.VergiKodu = item.VergiKodu;
                        toplamVergiler.VergiAciklamasi = item.VergiAciklamasi;
                        toplamVergiler.OdemeSekli = item.OdemeSekli;
                        toplamVergiler.Miktar = item.Miktar;


                        lstToplamVergiler.Add(toplamVergiler);
                    }

                    beyanSonuc.ToplamVergiler = lstToplamVergiler;
                }
                else beyanSonuc.ToplamVergiler = new List<DbSonucToplamVergiler>();

                if (_toplananvergiler.Count > 0)
                {
                    List<DbSonucToplananVergiler> lstToplananVergiler = new List<DbSonucToplananVergiler>();
                    DbSonucToplananVergiler toplananVergiler = new DbSonucToplananVergiler();
                    foreach (var item in _toplananvergiler)
                    {
                        toplananVergiler = new DbSonucToplananVergiler();

                        toplananVergiler.OdemeSekli = item.OdemeSekli;
                        toplananVergiler.Miktar = item.Miktar;


                        lstToplananVergiler.Add(toplananVergiler);
                    }

                    beyanSonuc.ToplananVergiler = lstToplananVergiler;
                }
                else beyanSonuc.ToplananVergiler = new List<DbSonucToplananVergiler>();

                if (_hesapdetaylar.Count > 0)
                {
                    List<DbSonucHesapDetaylar> lstHesapDetayari = new List<DbSonucHesapDetaylar>();
                    DbSonucHesapDetaylar hesapDetaylari = new DbSonucHesapDetaylar();
                    foreach (var item in _hesapdetaylar)
                    {
                        hesapDetaylari = new DbSonucHesapDetaylar();

                        hesapDetaylari.Aciklama = item.Aciklama;
                        hesapDetaylari.Miktar = item.Miktar;


                        lstHesapDetayari.Add(hesapDetaylari);
                    }

                    beyanSonuc.HesapDetaylari = lstHesapDetayari;
                }
                else beyanSonuc.HesapDetaylari = new List<DbSonucHesapDetaylar>();


                if (_ozetbeyan.Count > 0)
                {
                    List<DbSonucOzetBeyan> lstOzetBeyan = new List<DbSonucOzetBeyan>();
                    DbSonucOzetBeyan ozetBeyan = new DbSonucOzetBeyan();
                    foreach (var item in _ozetbeyan)
                    {
                        ozetBeyan = new DbSonucOzetBeyan();

                        ozetBeyan.OzetBeyanNo = item.OzetBeyanNo;
                        ozetBeyan.TescilTarihi = item.TescilTarihi;


                        lstOzetBeyan.Add(ozetBeyan);
                    }

                    beyanSonuc.OzetbeyanBilgi = lstOzetBeyan;
                }
                else beyanSonuc.OzetbeyanBilgi = new List<DbSonucOzetBeyan>();


                if (_gumrukkiymeti.Count > 0)
                {
                    List<DbSonucGumrukKiymeti> lstGumrukKiymeti = new List<DbSonucGumrukKiymeti>();
                    DbSonucGumrukKiymeti gumrukKiymeti = new DbSonucGumrukKiymeti();
                    foreach (var item in _gumrukkiymeti)
                    {
                        gumrukKiymeti = new DbSonucGumrukKiymeti();

                        gumrukKiymeti.KalemNo = item.KalemNo;
                        gumrukKiymeti.Miktar = item.Miktar;


                        lstGumrukKiymeti.Add(gumrukKiymeti);
                    }

                    beyanSonuc.GumrukKiymetleri = lstGumrukKiymeti;
                }
                else beyanSonuc.GumrukKiymetleri = new List<DbSonucGumrukKiymeti>();


                if (_istatitikikiymet.Count > 0)
                {
                    List<DbSonucIstatistikiKiymet> lstIstatistikiKiymeti = new List<DbSonucIstatistikiKiymet>();
                    DbSonucIstatistikiKiymet istatistikiKiymeti = new DbSonucIstatistikiKiymet();
                    foreach (var item in _istatitikikiymet)
                    {
                        istatistikiKiymeti = new DbSonucIstatistikiKiymet();

                        istatistikiKiymeti.KalemNo = item.KalemNo;
                        istatistikiKiymeti.Miktar = item.Miktar;


                        lstIstatistikiKiymeti.Add(istatistikiKiymeti);
                    }

                    beyanSonuc.IstatistikiKiymetler = lstIstatistikiKiymeti;
                }
                else beyanSonuc.IstatistikiKiymetler = new List<DbSonucIstatistikiKiymet>();


                return beyanSonuc;

            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }





}