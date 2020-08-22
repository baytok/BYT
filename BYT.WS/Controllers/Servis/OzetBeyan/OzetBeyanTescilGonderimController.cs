using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BYT.WS.AltYapi;
using BYT.WS.Controllers.api;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

namespace BYT.WS.Controllers.Servis.OzetBeyan
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanTescilGonderimController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }
        public OzetBeyanTescilGonderimController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }


        [Route("api/BYT/Servis/OzetBeyan/[controller]/{IslemInternalNo}/{Kullanici}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}")]
        public async Task<ServisDurum> GetTescilMesajiHazırla(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {

                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var ozetBeyanValues = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                var tasimaSenetValues = await _beyannameContext.ObTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                var ozetBeyanAcmalarValues = await _beyannameContext.ObOzetBeyanAcma.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                var tasiyiciFirmaValues = await _beyannameContext.ObTasiyiciFirma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                var tasitUgrakValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();


               Gelen gelen = new Gelen();
               OzetBeyanBilgisi _beyan = new OzetBeyanBilgisi();

                #region Genel
                if (ozetBeyanValues != null )
                {
                   
                    _beyan.BeyanSahibiVergiNo = ozetBeyanValues.BeyanSahibiVergiNo;
                    _beyan.BeyanTuru = ozetBeyanValues.BeyanTuru;
                    _beyan.Diger = ozetBeyanValues.Diger;
                    _beyan.DorseNo1 = ozetBeyanValues.DorseNo1;
                    _beyan.DorseNo1Uyrugu = ozetBeyanValues.DorseNo1Uyrugu;
                    _beyan.DorseNo2 = ozetBeyanValues.DorseNo2;
                    _beyan.DorseNo2Uyrugu = ozetBeyanValues.DorseNo2Uyrugu;
                    _beyan.EkBelgeSayisi = ozetBeyanValues.EkBelgeSayisi;
                    _beyan.EmniyetGuvenlik = ozetBeyanValues.EmniyetGuvenlik;
                    _beyan.GrupTasimaSenediNo = ozetBeyanValues.GrupTasimaSenediNo;
                    _beyan.GumrukIdaresi = ozetBeyanValues.GumrukIdaresi;
                    _beyan.KullaniciKodu = ozetBeyanValues.KullaniciKodu;
                    _beyan.Kurye = ozetBeyanValues.Kurye;
                    _beyan.LimanYerAdiBos = ozetBeyanValues.LimanYerAdiBos;
                    _beyan.LimanYerAdiYuk = ozetBeyanValues.LimanYerAdiYuk;
                    _beyan.OncekiBeyanNo = ozetBeyanValues.OncekiBeyanNo;
                    _beyan.PlakaSeferNo = ozetBeyanValues.PlakaSeferNo;
                    _beyan.ReferansNumarasi = ozetBeyanValues.ReferansNumarasi;
                    _beyan.RefNo = ozetBeyanValues.RefNo;
                    _beyan.Rejim = ozetBeyanValues.Rejim;
                    _beyan.TasimaSekli = ozetBeyanValues.TasimaSekli;
                    _beyan.TasitinAdi = ozetBeyanValues.TasitinAdi;
                    _beyan.TasiyiciVergiNo = ozetBeyanValues.TasiyiciVergiNo;
                    _beyan.TirAtaKarneNo = ozetBeyanValues.TirAtaKarneNo;
                    _beyan.UlkeKodu = ozetBeyanValues.UlkeKodu;
                    _beyan.UlkeKoduBos = ozetBeyanValues.UlkeKoduBos;
                    _beyan.UlkeKoduYuk = ozetBeyanValues.UlkeKoduYuk;
                    _beyan.VarisCikisGumrukIdaresi = ozetBeyanValues.VarisCikisGumrukIdaresi;
                    _beyan.VarisTarihSaati = ozetBeyanValues.VarisTarihSaati;
                    _beyan.XmlRefId = ozetBeyanValues.XmlRefId;
                    _beyan.YuklemeBosaltmaYeri = ozetBeyanValues.YuklemeBosaltmaYeri;
                }

                #endregion

                #region TaşıtUğrakÜlke
                if (tasitUgrakValues != null && tasitUgrakValues.Count > 0)
                {
                    TasitinUgradigiUlkeBilgisi _tulke;
                    List<TasitinUgradigiUlkeBilgisi> __tulkeList = new List<TasitinUgradigiUlkeBilgisi>();
                    foreach (var item in tasitUgrakValues)
                    {
                        _tulke = new TasitinUgradigiUlkeBilgisi();

                        _tulke.HareketTarihSaati = item.HareketTarihSaati!="" ? Convert.ToDateTime(item.HareketTarihSaati) :Convert.ToDateTime("0001 - 01 - 01T00: 00:00");
                        _tulke.LimanYerAdi = item.LimanYerAdi;
                        _tulke.UlkeKodu = item.UlkeKodu;

                        __tulkeList.Add(_tulke);
                    }
                    _beyan.TasitinUgradigiUlkeler = __tulkeList.ToList();
                }
                else _beyan.TasitinUgradigiUlkeler = new List<TasitinUgradigiUlkeBilgisi>();

                #endregion

                #region OzetBeyanAçma
                if (ozetBeyanAcmalarValues != null && ozetBeyanAcmalarValues.Count > 0)
                {

                    List<OzbyAcmaBilgisi> _ozetBeyanList = new List<OzbyAcmaBilgisi>();
                    OzbyAcmaBilgisi _ozetBeyan;
                    foreach (var ozetbeyan in ozetBeyanAcmalarValues)
                    {
                        _ozetBeyan = new OzbyAcmaBilgisi();
                        _ozetBeyan.BeyannameNo = ozetbeyan.OzetBeyanNo;
                        _ozetBeyan.AcmaSekli = ozetbeyan.IslemKapsami;
                        _ozetBeyan.AmbardaMi = ozetbeyan.Ambar;
                        _ozetBeyan.BaskaRejimleAcilacakMi = ozetbeyan.BaskaRejim;
                        _ozetBeyan.Aciklama = ozetbeyan.Aciklama;


                        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == ozetbeyan.OzetBeyanAcmaBeyanInternalNo).ToListAsync();

                        if (ozetBeyanAcmaTasimaSenediValues != null && ozetBeyanAcmaTasimaSenediValues.Count > 0)
                        {
                            List<OzbyAcmaSenetBilgisi> _ozetBeyanTasimaSenediList = new List<OzbyAcmaSenetBilgisi>();
                            OzbyAcmaSenetBilgisi _ozetBeyanTasimaSenedi;
                            foreach (var tasimaSenedi in ozetBeyanAcmaTasimaSenediValues)
                            {
                                _ozetBeyanTasimaSenedi = new OzbyAcmaSenetBilgisi();
                                _ozetBeyanTasimaSenedi.AcilanSenetNo = tasimaSenedi.TasimaSenediNo;

                                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == ozetbeyan.OzetBeyanAcmaBeyanInternalNo && v.TasimaSenetInternalNo == tasimaSenedi.TasimaSenetInternalNo).ToListAsync();

                                if (ozetBeyanAcmaTasimaSatirValues != null && ozetBeyanAcmaTasimaSatirValues.Count > 0)
                                {
                                    List<OzbyAcmaSatirBilgisi> ozetBeyanAcmaTasimaSatirList = new List<OzbyAcmaSatirBilgisi>();
                                    OzbyAcmaSatirBilgisi _ozetBeyanTasimaSatir;
                                    foreach (var satir in ozetBeyanAcmaTasimaSatirValues)
                                    {
                                        _ozetBeyanTasimaSatir = new OzbyAcmaSatirBilgisi();

                                        _ozetBeyanTasimaSatir.AcilacakMiktar = satir.AcilacakMiktar != null ? satir.AcilacakMiktar : 0;
                                        _ozetBeyanTasimaSatir.AcmaSatirNo = satir.AcmaSatirNo!=null ? satir.AcmaSatirNo: 0 ;
                                        _ozetBeyanTasimaSatir.AmbardakiMiktar = satir.AmbardakiMiktar != null ? satir.AmbardakiMiktar : 0;
                                        _ozetBeyanTasimaSatir.AmbarKodu = satir.AmbarKodu ;
                                        _ozetBeyanTasimaSatir.Birim = satir.Birim;
                                        _ozetBeyanTasimaSatir.EsyaCinsi = satir.EsyaCinsi;
                                        _ozetBeyanTasimaSatir.KapatilanMiktar = satir.KapatilanMiktar != null ? satir.KapatilanMiktar : 0;
                                        _ozetBeyanTasimaSatir.MarkaNo = satir.MarkaNo;
                                        _ozetBeyanTasimaSatir.OlcuBirimi = satir.OlcuBirimi;
                                        _ozetBeyanTasimaSatir.ToplamMiktar = satir.ToplamMiktar != null ? satir.ToplamMiktar : 0;

                                        ozetBeyanAcmaTasimaSatirList.Add(_ozetBeyanTasimaSatir);
                                    }
                                    _ozetBeyanTasimaSenedi.OzbyAcmaSatirlari = ozetBeyanAcmaTasimaSatirList.ToList();
                                }
                                else _ozetBeyanTasimaSenedi.OzbyAcmaSatirlari = new List<OzbyAcmaSatirBilgisi>();

                                _ozetBeyanTasimaSenediList.Add(_ozetBeyanTasimaSenedi);
                            }
                            _ozetBeyan.OzbyAcmaSenetleri = _ozetBeyanTasimaSenediList.ToList();
                        }
                        else _ozetBeyan.OzbyAcmaSenetleri = new List<OzbyAcmaSenetBilgisi>();

                        _ozetBeyanList.Add(_ozetBeyan);
                    }
                    _beyan.OzbyAcmalar = _ozetBeyanList.ToList();

                }
                else _beyan.OzbyAcmalar = new List<OzbyAcmaBilgisi>();
                #endregion

                #region TaşıyıcıFirma
                if (tasiyiciFirmaValues != null )
                {
                    FirmaBilgisi _tasiyiciFirma = new FirmaBilgisi();

                    _tasiyiciFirma.AdiUnvani = tasiyiciFirmaValues.AdUnvan;
                    _tasiyiciFirma.CadSNo = tasiyiciFirmaValues.CaddeSokakNo;
                    _tasiyiciFirma.Fax = tasiyiciFirmaValues.Faks;
                    _tasiyiciFirma.IlIlce = tasiyiciFirmaValues.IlIlce;
                    _tasiyiciFirma.KimlikNo = tasiyiciFirmaValues.No;
                    _tasiyiciFirma.KimlikTuru = tasiyiciFirmaValues.KimlikTuru;
                    _tasiyiciFirma.PostaKodu = tasiyiciFirmaValues.PostaKodu;
                    _tasiyiciFirma.Tel= tasiyiciFirmaValues.Telefon;
                    _tasiyiciFirma.UlkeKodu = tasiyiciFirmaValues.UlkeKodu;
                    _tasiyiciFirma.VergiDairesikodu = "";
                    _beyan.TasiyiciFirma = _tasiyiciFirma;
                }
                else _beyan.TasiyiciFirma = new FirmaBilgisi();
                #endregion

                #region TaşımaSenet

                if (tasimaSenetValues != null && tasimaSenetValues.Count > 0)
                {
                    TasimaSenediBilgisi _senet;
                    List<TasimaSenediBilgisi> _senetList = new List<TasimaSenediBilgisi>();
                    foreach (var item in tasimaSenetValues)
                    {
                        _senet = new TasimaSenediBilgisi();

                        _senet.AcentaAdi = item.AcentaAdi;
                        _senet.AcentaVergiNo = item.AcentaVergiNo;
                        _senet.AktarmaTipi = item.AktarmaTipi;
                        _senet.AktarmaYapilacakMi = item.AktarmaYapilacak;
                        _senet.AliciAdi = item.AliciAdi;
                        _senet.AliciVergiNo = item.AliciVergiNo;
                        _senet.AmbarHariciMi = item.AmbarHarici;
                        _senet.BildirimTarafiAdi = item.BildirimTarafiAdi;
                        _senet.BildirimTarafiVergiNo = item.BildirimTarafiVergiNo;
                        _senet.DuzenlendigiUlke = item.DuzenlendigiUlke;
                        _senet.EmniyetGuvenlikT = item.EmniyetGuvenlik;
                        _senet.EsyaninBulunduguYer = item.EsyaninBulunduguYer;
                        _senet.FaturaDoviz = item.FaturaDoviz;
                        _senet.FaturaToplami = item.FaturaToplami != null ? item.FaturaToplami : 0;
                        _senet.GondericiAdi = item.GondericiAdi;
                        _senet.GondericiVergiNo = item.GondericiVergiNo;
                        _senet.GrupMu = item.Grup;
                        _senet.KonteynerMi = item.Konteyner;
                        _senet.NavlunDoviz = item.NavlunDoviz;
                        _senet.NavlunTutari = item.NavlunTutari != null ? item.NavlunTutari : 0;
                        _senet.OdemeSekli = item.OdemeSekli;
                        _senet.OncekiSeferNumarasi = item.OncekiSeferNumarasi;
                        _senet.OncekiSeferTarihi = item.OncekiSeferTarihi != "" ? Convert.ToDateTime(item.OncekiSeferTarihi) : Convert.ToDateTime("0001 - 01 - 01T00: 00:00") ;
                        _senet.OzetBeyanNo = item.OzetBeyanNo;
                        _senet.RoroMu = item.Roro;
                        _senet.SenetSiraNo = item.SenetSiraNo.ToString();
                        _senet.TasimaSenediNo = item.TasimaSenediNo;

                        var satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo).ToListAsync();

                        if (satirValues != null && satirValues.Count > 0)
                        {
                            List<TasimaSatiriBilgisi> _satirList = new List<TasimaSatiriBilgisi>();
                            TasimaSatiriBilgisi _satir;
                            foreach (var satir in satirValues)
                            {
                                _satir = new TasimaSatiriBilgisi();
                               
                                _satir.BrutAgirlik = satir.BrutAgirlik != null ? satir.BrutAgirlik : 0;
                                _satir.KapAdedi = satir.KapAdet != null ? satir.KapAdet : 0;
                                _satir.KapCinsi = satir.KapCinsi;
                                _satir.KonteynerTipi = satir.KonteynerTipi;
                                _satir.KonteynerYukDurumu = satir.KonteynerYukDurumu;
                                _satir.MarkaNo = satir.MarkaNo;
                                _satir.MuhurNumarasi = satir.MuhurNumarasi;
                                _satir.NetAgirlik = satir.NetAgirlik != null ? satir.NetAgirlik : 0;
                                _satir.OlcuBirimi = satir.OlcuBirimi;
                                _satir.SatirNo = satir.SatirNo != null ? satir.SatirNo : 0;

                                var satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo && v.TasimaSatirInternalNo== satir.TasimaSatirInternalNo).ToListAsync();
                                if (satirEsyaValues != null && satirEsyaValues.Count > 0)
                                {
                                    List<EsyaBilgisi> _satirEsyaList = new List<EsyaBilgisi>();
                                    EsyaBilgisi _satirEsya;
                                    foreach (var satirEsya in satirEsyaValues)
                                    {
                                        _satirEsya = new EsyaBilgisi();
                                        _satirEsya.BmEsyaKodu = satirEsya.BmEsyaKodu;
                                        _satirEsya.BrutAgirlik = satirEsya.BrutAgirlik != null ? satirEsya.BrutAgirlik : 0;
                                        _satirEsya.EsyaKodu = satirEsya.EsyaKodu;
                                        _satirEsya.EsyaninTanimi = satirEsya.EsyaninTanimi;
                                        _satirEsya.KalemFiyati = satirEsya.KalemFiyati != null ? satirEsya.KalemFiyati : 0;
                                        _satirEsya.KalemFiyatiDoviz = satirEsya.KalemFiyatiDoviz;
                                        _satirEsya.KalemSiraNo = satirEsya.KalemSiraNo != null ? satirEsya.KalemSiraNo : 0;
                                        _satirEsya.NetAgirlik = satirEsya.NetAgirlik != null ? satirEsya.NetAgirlik : 0;
                                        _satirEsya.OlcuBirimi = satirEsya.OlcuBirimi;
                                        _satirEsyaList.Add(_satirEsya);
                                    }
                                    _satir.EsyaBilgileri = _satirEsyaList.ToList();
                                }
                                else _satir.EsyaBilgileri = new List<EsyaBilgisi>();


                               _satirList.Add(_satir);
                            }

                            _senet.TasimaSatirlari = _satirList.ToList();
                        }
                        else _senet.TasimaSatirlari = new List<TasimaSatiriBilgisi>();


                        var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo).ToListAsync();

                        if (ihracatValues != null && ihracatValues.Count > 0)
                        {
                            List<IhracatBilgisi> _ihracatList = new List<IhracatBilgisi>();
                            IhracatBilgisi _ihracat;
                            foreach (var ihr in ihracatValues)
                            {
                                _ihracat = new IhracatBilgisi();
                                _ihracat.BrutAgirlik = ihr.BrutAgirlik != null ? ihr.BrutAgirlik : 0; 
                                _ihracat.KapAdedi = ihr.KapAdet != null ? ihr.KapAdet : 0;
                                _ihracat.Numarasi = ihr.Numara;
                                _ihracat.ParcaliMi = ihr.Parcali;
                                _ihracat.Tipi = ihr.Tip;
                                
                                _ihracatList.Add(_ihracat);
                            }

                            _senet.Ihracatlar = _ihracatList.ToList();
                        }
                        else _senet.Ihracatlar = new List<IhracatBilgisi>();

                        var ugrakUlkeValues = await _beyannameContext.ObUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == item.TasimaSenetInternalNo).ToListAsync();

                        if (ugrakUlkeValues != null && ugrakUlkeValues.Count > 0)
                        {
                            List<UgranilanUlkeBilgisi> _ugrakList = new List<UgranilanUlkeBilgisi>();
                            UgranilanUlkeBilgisi _ugrak;
                            foreach (var ugrak in ugrakUlkeValues)
                            {
                                _ugrak = new UgranilanUlkeBilgisi();
                                _ugrak.LimanYerAdi=ugrak.LimanYerAdi;
                                _ugrak.UlkeKodu = ugrak.UlkeKodu;

                                _ugrakList.Add(_ugrak);
                            }

                            _senet.UgranilanUlkeler = _ugrakList.ToList();
                        }
                        else _senet.UgranilanUlkeler = new List<UgranilanUlkeBilgisi>();



                        _senetList.Add(_senet);
                    }
                    _beyan.TasimaSenetleri = _senetList.ToList();
                }
                else _beyan.TasimaSenetleri = new List<TasimaSenediBilgisi>();


                #endregion

                #region Teminat

                //var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                //if (teminatValues != null && teminatValues.Count > 0)
                //{
                //    List<KontrolHizmeti.Teminat> _teminatList = new List<KontrolHizmeti.Teminat>();
                //    KontrolHizmeti.Teminat _teminat;
                //    foreach (var teminat in teminatValues)
                //    {
                //        _teminat = new KontrolHizmeti.Teminat();
                //        _teminat.Teminat_sekli = teminat.TeminatSekli;
                //        _teminat.Teminat_orani = teminat.TeminatOrani;
                //        _teminat.Global_teminat_no = teminat.GlobalTeminatNo;
                //        _teminat.Banka_mektubu_tutari = teminat.BankaMektubuTutari;
                //        _teminat.Nakdi_teminat_tutari = teminat.NakdiTeminatTutari;
                //        _teminat.Diger_tutar = teminat.DigerTutar;
                //        _teminat.Diger_tutar_referansi = teminat.DigerTutarReferansi;
                //        _teminat.Aciklama = teminat.Aciklama;

                //        _teminatList.Add(_teminat);

                //    }
                //    _beyan.Teminat = _teminatList.ToArray();
                //}
                //else _beyan.Teminat = new KontrolHizmeti.Teminat[0];


                #endregion



                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                          .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                          .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

                gelen.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
                gelen.RefID = islemValues.RefNo;
                gelen.Sifre = "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
                gelen.IP = "";
                gelen.OzetBeyanBilgisi = _beyan;

                string imzasizMesaj = SerializeToXML(gelen);


                string guidOf = "BYT:" + Guid.NewGuid().ToString();
                string newIslemInternalNo = IslemInternalNo;
                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {


                        if (islemValues.IslemTipi == "OzetBeyan")
                        {

                            islemValues.Kullanici = Kullanici;
                            islemValues.IslemDurumu = "Imzala";
                            islemValues.IslemZamani = DateTime.Now;
                            islemValues.SonIslemZamani = DateTime.Now;
                            islemValues.IslemSonucu = "Tescil Mesaji Olusturuldu";
                            islemValues.Guidof = guidOf;
                            islemValues.GonderimSayisi++;
                            _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        }
                        else
                        {
                            Islem _islem = new Islem();
                            newIslemInternalNo = islemValues.BeyanInternalNo.Replace("OB", "OBTG");
                            _islem.Kullanici = islemValues.Kullanici;
                            _islem.IslemDurumu = "Imzala";
                            _islem.IslemInternalNo = newIslemInternalNo;
                            _islem.IslemZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.IslemSonucu = "Tescil Mesaji Olusturuldu";
                            _islem.Guidof = guidOf;
                            _islem.RefNo = islemValues.RefNo;
                            _islem.BeyanInternalNo = islemValues.BeyanInternalNo;
                            _islem.BeyanTipi = islemValues.BeyanTipi;
                            _islem.IslemTipi = "OzetBeyan";
                            _islem.GonderimSayisi++;
                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                        }
                        //TODO: bu guid dışında imzala aşamasında kalmış aynı IslemInternalNo ile ilgili kayıtlar iptal edilsin
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = ozetBeyanValues.GumrukIdaresi;
                        _tarihce.Rejim = ozetBeyanValues.BeyanTuru;
                        _tarihce.IslemInternalNo = newIslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = "Imzala";
                        _tarihce.IslemSonucu = "Tescil Mesaji Olusturuldu";
                        _tarihce.IslemTipi = "0";
                        _tarihce.TicaretTipi = ozetBeyanValues.Rejim;
                        _tarihce.GonderilecekVeri = imzasizMesaj;
                        _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;

                        ozetBeyanValues.SonIslemZamani = DateTime.Now;
                        ozetBeyanValues.TescilStatu = "Tescil Mesaji Olusturuldu";
                        _beyannameContext.Entry(ozetBeyanValues).State = EntityState.Modified;

                        await _islemTarihceContext.SaveChangesAsync();
                        await _beyannameContext.SaveChangesAsync();

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lsthtt = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lsthtt.Add(ht);
                        _servisDurum.Hatalar = lsthtt;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }
                }



                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Tescil Gönderimi", ReferansNo = guidOf, GUID = guidOf, Sonuc = "Tescil Gönderimi Gerçekleşti", SonucVeriler = null };
                lstBlg.Add(blg);


                _servisDurum.Bilgiler = lstBlg;


                var Message = $"GetTescil {DateTime.UtcNow.ToLongTimeString()}";
                Log.Information("Message displayed: {Message}", Message, _servisDurum);

                return _servisDurum;

            }
            catch (Exception ex)
            {

                List<Internal.Hata> lstht = new List<Internal.Hata>();
                Internal.Hata ht = new Internal.Hata { HataKodu = 1, HataAciklamasi = ex.ToString() };
                lstht.Add(ht);
                _servisDurum.Hatalar = lstht;

                return _servisDurum;
            }


        }

        [Route("api/BYT/Servis/OzetBeyan/[controller]/{IslemInternalNo}/{Kullanici}/{Guid}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}/{Guid}")]
        public async Task<ServisDurum> GetTescil(string IslemInternalNo, string Kullanici, string Guid)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {

                var tarihceValues = await _islemTarihceContext.Tarihce.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guid == Guid.Trim() && v.IslemDurumu == "Imzalandi");
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guidof == Guid.Trim() && v.IslemDurumu == "Imzalandi");
                var beyanBeyanValues = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);


                string guidOf = "", IslemDurumu = "", islemSonucu = "";
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("Root");
                root.InnerText = tarihceValues.ImzaliVeri;
                OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient Tescil = ServiceHelper.GetOzetBeyanWSClient(_servisCredential.username, _servisCredential.password);
                await Tescil.OzetBeyanAsync(root);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(root.OuterXml);

                if (doc.HasChildNodes)
                {
                    foreach (XmlNode n in doc.ChildNodes[0].ChildNodes)
                    {

                        if (n.Name == "Message")
                        {
                            IslemDurumu = "Hata";
                            islemSonucu = n.InnerText;
                            break;
                        }

                        else
                        {
                            if (n.Name == "Guid")
                            {
                                IslemDurumu = "Islemde";
                                guidOf = n.InnerText;


                            }
                            else if (n.Name == "Durum")
                            {
                                islemSonucu = n.InnerText;


                            }
                        }

                    }
                }


                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {
                        islemValues.Kullanici = Kullanici;
                        islemValues.IslemDurumu = "Gonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("OB", "OBTG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "OzbyTescil";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();



                        tarihceValues.Guid = guidOf;
                        tarihceValues.IslemDurumu = IslemDurumu;
                        tarihceValues.IslemSonucu = islemSonucu;
                        tarihceValues.GondermeZamani = DateTime.Now;
                        tarihceValues.SonIslemZamani = DateTime.Now;
                        tarihceValues.GonderimNo = islemValues.GonderimSayisi;

                        beyanBeyanValues.SonIslemZamani = DateTime.Now;
                        beyanBeyanValues.TescilStatu = "Tescil Gonderildi";

                        _islemTarihceContext.Entry(tarihceValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lsthtt = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lsthtt.Add(ht);
                        _servisDurum.Hatalar = lsthtt;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
                    }
                }



                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                List<Internal.Hata> lstht = new List<Internal.Hata>();

                if (IslemDurumu == "Hata")
                {
                    Internal.Hata ht = new Internal.Hata { HataKodu = 1, HataAciklamasi = islemSonucu };
                    lstht.Add(ht);
                }
                Bilgi blg = new Bilgi { IslemTipi = "Tescil Gönderimi", ReferansNo = guidOf, GUID = guidOf, Sonuc = "Tescil Gönderimi Gerçekleşti", SonucVeriler = null };
                lstBlg.Add(blg);


                _servisDurum.Bilgiler = lstBlg;
                _servisDurum.Hatalar = lstht;



                return _servisDurum;

            }
            catch (Exception ex)
            {

                List<Internal.Hata> lstht = new List<Internal.Hata>();
                Internal.Hata ht = new Internal.Hata { HataKodu = 1, HataAciklamasi = ex.ToString() };
                lstht.Add(ht);
                _servisDurum.Hatalar = lstht;

                return _servisDurum;
            }


        }
        public static string SerializeToXML(object responseObject)
        {
            //DefaultNamespace of XSD
            //We have to Namespace compulsory, if XSD using any namespace.

            XmlSerializer xmlObj = new XmlSerializer(responseObject.GetType());
            String XmlizedString = null;

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter XmlObjWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            XmlObjWriter.Formatting = Formatting.Indented;
            xmlObj.Serialize(XmlObjWriter, responseObject);
            memoryStream = (MemoryStream)XmlObjWriter.BaseStream;

            UTF8Encoding encoding = new UTF8Encoding();
            XmlizedString = encoding.GetString(memoryStream.ToArray());
            XmlObjWriter.Close();
            XmlizedString = XmlizedString.Substring(1);

            return XmlizedString;
        }
    }


}