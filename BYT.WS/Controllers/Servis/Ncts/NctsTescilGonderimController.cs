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
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

namespace BYT.WS.Controllers.Servis.Ncts
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NctsTescilGonderimController : ControllerBase
    {

        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
      
        public IConfiguration Configuration { get; }
        public NctsTescilGonderimController(IslemTarihceDataContext islemTarihcecontext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
       
            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }


        [Route("api/BYT/Servis/Ncts/[controller]/{IslemInternalNo}/{Kullanici}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}")]
        public async Task<ServisDurum> GetTescilMesajiHazırla(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {

                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var nctsBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                CC015B gelen = new CC015B();

                #region Genel
                HEAHEA _beyan = new HEAHEA();
                _beyan.RefNumHEA4 = nctsBeyanValues.BeyannameNo != null ? nctsBeyanValues.BeyannameNo : "1";
                _beyan.AgrLocOfGooCodHEA38 = nctsBeyanValues.EsyaKabulYerKod;
                _beyan.AutLocOfGooCodHEA41 = nctsBeyanValues.EsyaOnayYer;
                _beyan.CodPlUnHEA357 = nctsBeyanValues.BosaltmaYer;
                _beyan.CodPlUnHEA357LNG = nctsBeyanValues.YukBosYerDil;
                _beyan.ComRefNumHEA = nctsBeyanValues.RefaransNo;
                _beyan.ConIndHEA96 = nctsBeyanValues.Konteyner == true ? '1' : '0';
                _beyan.ConRefNumHEA = nctsBeyanValues.KonveyansRefNo;
                _beyan.CouOfDesCodHEA30 = nctsBeyanValues.VarisUlke;
                _beyan.CouOfDisCodHEA55 = nctsBeyanValues.CikisUlke;
                _beyan.CusSubPlaHEA66 = nctsBeyanValues.EsyaYer;
                _beyan.DamgaVergi = nctsBeyanValues.DamgaVergi;
                _beyan.DecDatHEA383 = nctsBeyanValues.TescilTarihi;
                _beyan.DecPlaHEA394 = nctsBeyanValues.Yer;
                _beyan.DecPlaHEA394LNG = nctsBeyanValues.YerTarihDil;
                // _beyan.DiaLanIndAtDepHEA254
                // _beyan.AgrLocOfGooHEA39
                // _beyan.AgrLocOfGooHEA39LNG
                _beyan.IdeOfMeaOfTraAtDHEA78 = nctsBeyanValues.CikisTasitKimligi;
                _beyan.IdeOfMeaOfTraAtDHEA78LNG = nctsBeyanValues.CikisTasitKimligiDil;
                _beyan.IdeOfMeaOfTraCroHEA85 = nctsBeyanValues.SinirTasitKimligi;
                _beyan.IdeOfMeaOfTraCroHEA85LNG = nctsBeyanValues.SinirTasitKimligiDil;
                if (!string.IsNullOrWhiteSpace(nctsBeyanValues.DahildeTasimaSekli)) _beyan.InlTraModHEA75 = Convert.ToInt16(nctsBeyanValues.DahildeTasimaSekli);
                _beyan.MusavirKimlikNo = nctsBeyanValues.MusavirKimlikNo;
                _beyan.NatOfMeaOfTraAtDHEA80 = nctsBeyanValues.CikisTasitUlke;
                _beyan.NatOfMeaOfTraCroHEA87 = nctsBeyanValues.SinirTasitUlke;
                _beyan.NCTSAccDocHEA601LNG = nctsBeyanValues.BeyanTipiDil;
                _beyan.PlaOfLoaCodHEA46 = nctsBeyanValues.YuklemeYeri;
                _beyan.RefNumHEA4 = nctsBeyanValues.BeyannameNo;
                _beyan.SecHEA358 = nctsBeyanValues.GuvenliBeyan;
                _beyan.SpeCirIndHEA1 = nctsBeyanValues.BeyanTipi;
                _beyan.TotGroMasHEA307 = nctsBeyanValues.KalemToplamBrutKG;
                _beyan.TotNumOfIteHEA305 = nctsBeyanValues.KalemSayisi;
                _beyan.TotNumOfPacHEA306 = nctsBeyanValues.ToplamKapSayisi;
                _beyan.TraChaMetOfPayHEA1 = nctsBeyanValues.OdemeAraci;
                _beyan.TraModAtBorHEA76 = nctsBeyanValues.CikisTasimaSekli;
                _beyan.TruckId2 = nctsBeyanValues.Dorse1;
                _beyan.TruckId3 = nctsBeyanValues.Dorse2;
                _beyan.TypOfDecHEA24 = nctsBeyanValues.Rejim;
                _beyan.TypOfMeaOfTraCroHEA88 = nctsBeyanValues.SinirTasimaSekli;
                _beyan.Tanker = nctsBeyanValues.Tanker == false ? '0' : '1';
                _beyan.RefNumEBT1 = nctsBeyanValues.SinirGumruk;
                _beyan.AgrLocOfGooHEA39 = nctsBeyanValues.EsyaKabulYer;
                _beyan.AgrLocOfGooHEA39LNG = nctsBeyanValues.EsyaKabulYerDil;

                TRAPRIPC1 _asilSorumlu = new TRAPRIPC1();
                var asilSorumlufirmaValues = await _beyannameContext.NbAsilSorumluFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (asilSorumlufirmaValues != null)
                {
                    _asilSorumlu.CitPC124 = asilSorumlufirmaValues.IlIlce;
                    _asilSorumlu.CouPC125 = asilSorumlufirmaValues.UlkeKodu;
                    //_asilSorumlu.HITPC126 = asilSorumlufirmaValues.?????;
                    _asilSorumlu.NADLNGPC = asilSorumlufirmaValues.Dil;
                    _asilSorumlu.NamPC17 = asilSorumlufirmaValues.AdUnvan;
                    _asilSorumlu.PosCodPC123 = asilSorumlufirmaValues.PostaKodu;
                    _asilSorumlu.StrAndNumPC122 = asilSorumlufirmaValues.CaddeSokakNo;
                    _asilSorumlu.TINPC159 = asilSorumlufirmaValues.No;

                }

                TRACONCO1 _gonderici = new TRACONCO1();
                var gondericifirmaValues = await _beyannameContext.NbGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (gondericifirmaValues != null)
                {
                    _gonderici.CitCO124 = gondericifirmaValues.IlIlce;
                    _gonderici.CouCO125 = gondericifirmaValues.UlkeKodu;
                    _gonderici.NADLNGCO = gondericifirmaValues.Dil;
                    _gonderici.NamCO17 = gondericifirmaValues.AdUnvan;
                    _gonderici.PosCodCO123 = gondericifirmaValues.PostaKodu;
                    _gonderici.StrAndNumCO122 = gondericifirmaValues.CaddeSokakNo;
                    _gonderici.TINCO159 = gondericifirmaValues.No;

                }
                TRACONCE1 _alici = new TRACONCE1();
                var alicifirmaValues = await _beyannameContext.NbAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (alicifirmaValues != null)
                {
                    _alici.CitCE124 = alicifirmaValues.IlIlce;
                    _alici.CouCE125 = alicifirmaValues.UlkeKodu;
                    _alici.NADLNGCE = alicifirmaValues.Dil;
                    _alici.NamCE17 = alicifirmaValues.AdUnvan;
                    _alici.PosCodCE123 = alicifirmaValues.PostaKodu;
                    _alici.StrAndNumCE122 = alicifirmaValues.CaddeSokakNo;
                    _alici.TINCE159 = alicifirmaValues.No;

                }



                CARTRA100 _tasiyici = new CARTRA100();
                var tasiyicifirmaValues = await _beyannameContext.NbTasiyiciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (tasiyicifirmaValues != null)
                {
                    _tasiyici.CitCARTRA789 = tasiyicifirmaValues.IlIlce;
                    _tasiyici.CouCodCARTRA587 = tasiyicifirmaValues.UlkeKodu;
                    _tasiyici.NADCARTRA121 = tasiyicifirmaValues.Dil;
                    _tasiyici.NamCARTRA121 = tasiyicifirmaValues.AdUnvan;
                    _tasiyici.PosCodCARTRA121 = tasiyicifirmaValues.PostaKodu;
                    _tasiyici.StrAndNumCARTRA254 = tasiyicifirmaValues.CaddeSokakNo;
                    _tasiyici.TINCARTRA254 = tasiyicifirmaValues.No;

                }
                TRACORSEC037 _guvenliGonderici = new TRACORSEC037();
                var guvenliGondericifirmaValues = await _beyannameContext.NbGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (guvenliGondericifirmaValues != null)
                {
                    _guvenliGonderici.CitTRACORSEC038 = guvenliGondericifirmaValues.IlIlce;
                    _guvenliGonderici.CouCodTRACORSEC039 = guvenliGondericifirmaValues.UlkeKodu;
                    _guvenliGonderici.TRACORSEC037LNG = guvenliGondericifirmaValues.Dil;
                    _guvenliGonderici.NamTRACORSEC041 = guvenliGondericifirmaValues.AdUnvan;
                    _guvenliGonderici.PosCodTRACORSEC042 = guvenliGondericifirmaValues.PostaKodu;
                    _guvenliGonderici.StrNumTRACORSEC043 = guvenliGondericifirmaValues.CaddeSokakNo;
                    _guvenliGonderici.TINTRACORSEC044 = guvenliGondericifirmaValues.No;

                }

                TRACONSEC029 _guvenliAlici = new TRACONSEC029();
                var guvenliAlicifirmaValues = await _beyannameContext.NbGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);

                if (guvenliAlicifirmaValues != null)
                {
                    _guvenliAlici.CitTRACONSEC030 = guvenliAlicifirmaValues.IlIlce;
                    _guvenliAlici.CouCodTRACONSEC031 = guvenliAlicifirmaValues.UlkeKodu;
                    _guvenliAlici.TRACONSEC029LNG = guvenliAlicifirmaValues.Dil;
                    _guvenliAlici.NameTRACONSEC033 = guvenliAlicifirmaValues.AdUnvan;
                    _guvenliAlici.PosCodTRACONSEC034 = guvenliAlicifirmaValues.PostaKodu;
                    _guvenliAlici.StrNumTRACONSEC035 = guvenliAlicifirmaValues.CaddeSokakNo;
                    _guvenliAlici.TINTRACONSEC036 = guvenliAlicifirmaValues.No;

                }


                TRAAUTCONTRA _varisYetkilisi = new TRAAUTCONTRA();
                _varisYetkilisi.TINTRA59 = nctsBeyanValues.VarisGumrukYetkilisi;

                CUSOFFDEPEPT _hareketGumruk = new CUSOFFDEPEPT();
                _hareketGumruk.RefNumEPT1 = nctsBeyanValues.HareketGumruk;

                CUSOFFDESEST _varisGumruk = new CUSOFFDESEST();
                _varisGumruk.RefNumEST1 = nctsBeyanValues.VarisGumruk;

                CONRESERS _kontrolSonuc = new CONRESERS();
                _kontrolSonuc.ConResCodERS16 = nctsBeyanValues.KontrolSonuc;
                _kontrolSonuc.DatLimERS69 = nctsBeyanValues.SureSinir;

                REPREP _temsilci = new REPREP();
                _temsilci.NamREP5 = nctsBeyanValues.Temsilci;
                _temsilci.RepCapREP18 = nctsBeyanValues.TemsilKapasite;
                _temsilci.RepCapREP18LNG = nctsBeyanValues.TemsilKapasiteDil;


                var muhurValues = await _beyannameContext.NbMuhur.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                List<SEAINFSLI> _muhurList = new List<SEAINFSLI>();
                if (muhurValues != null && muhurValues.Count > 0)
                {
                    SEAINFSLI _muhur = new SEAINFSLI();
                    _muhur.SeaNumSLI2 = "";

                    List<SEAIDSID> _mhrList = new List<SEAIDSID>();
                    foreach (var item in muhurValues)
                    {

                        SEAIDSID mhr = new SEAIDSID();
                        mhr.SeaIdeSID1 = item.MuhurNo;
                        mhr.SeaIdeSID1LNG = item.Dil;
                        _mhrList.Add(mhr);
                    }
                    _muhur.Muhur = _mhrList;
                    _muhurList.Add(_muhur);

                }

                List<CUSOFFTRARNS> _transitGumrukList = new List<CUSOFFTRARNS>();
                var transitGumrukValues = await _beyannameContext.NbTransitGumruk.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                if (transitGumrukValues != null && transitGumrukValues.Count > 0)
                {

                    foreach (var item in transitGumrukValues)
                    {
                        CUSOFFTRARNS transitGumruk = new CUSOFFTRARNS();
                        transitGumruk.ArrTimTRACUS085 = item.VarisTarihi;
                        transitGumruk.RefNumRNS1 = item.Gumruk;
                        _transitGumrukList.Add(transitGumruk);
                    }

                }
                List<ITI> _rotaList = new List<ITI>();
                var rotaValues = await _beyannameContext.NbRota.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                if (rotaValues != null && rotaValues.Count > 0)
                {

                    foreach (var item in rotaValues)
                    {
                        ITI rota = new ITI();
                        rota.CouOfRouCodITI1 = item.UlkeKodu;
                        _rotaList.Add(rota);
                    }

                }

                var beyanSahibiValues = await _beyannameContext.NbBeyanSahibi.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                if (nctsBeyanValues.MusavirKimlikNo!="")
                {
                    BS _bs = new BS();
                    _bs.CitBeyanSahibi = beyanSahibiValues.IlIlce;
                    _bs.NamBeyanSahibi = beyanSahibiValues.AdUnvan;
                    _bs.StBeyanSahibi = beyanSahibiValues.CaddeSokakNo;
                    _bs.TinBeyanSahibi = beyanSahibiValues.No;
                    gelen.BS = _bs;
                }

                #endregion


                #region Teminat

                var teminatValues = await _beyannameContext.NbTeminat.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                List<GUAGUA> _teminatList = new List<GUAGUA>();
                if (teminatValues != null && teminatValues.Count > 0)
                {
                    GUAGUA _teminat = new GUAGUA();

                    List<GUAREFREF> _tmntList = new List<GUAREFREF>();
                    foreach (var item in teminatValues)
                    {

                        GUAREFREF teminat = new GUAREFREF();
                        teminat.AccCodREF6 = item.ErisimKodu;
                        teminat.AmoConREF7 = item.Tutar;
                        teminat.CurREF8 = item.DovizCinsi;
                        teminat.GuaRefNumGRNREF1 = item.GRNNo;
                        teminat.OthGuaRefREF4 = item.DigerRefNo;
                        teminat.VALLIMECVLE = item.ECGecerliDegil;
                        teminat.VALLIMNONECLIM = item.UlkeGecerliDegil;

                        _tmntList.Add(teminat);

                    }
                    _teminat.GuaTypGUA1 = teminatValues[0].TeminatTipi;
                    _teminat.GUAREFREF = _tmntList;

                    _teminatList.Add(_teminat);

                }


                #endregion

                #region Açmnalar     

                var obAcmalarValues = await _beyannameContext.NbObAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                List<ACMA> _ozetBeyanAcma = new List<ACMA>();
                if (obAcmalarValues != null && obAcmalarValues.Count > 0)
                {



                    foreach (var item in obAcmalarValues)
                    {
                        ACMA acma = new ACMA();
                        acma.DisInd = item.IslemKapsami;
                        acma.DisQty = item.Miktar;
                        acma.Idosext = item.OzetBeyanNo;
                        acma.Ltitref = item.TasimaSenetNo;
                        acma.TitlNum = item.TasimaSatirNo.ToString();
                        acma.WareCod = item.AmbarKodu;
                        acma.WareInd = item.AmbarIci;

                        _ozetBeyanAcma.Add(acma);
                    }



                }

                var abAcmalarValues = await _beyannameContext.NbAbAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                List<ACMA2> _beyannameAcma = new List<ACMA2>();
                if (abAcmalarValues != null && abAcmalarValues.Count > 0)
                {

                    foreach (var item in abAcmalarValues)
                    {

                        ACMA2 acma = new ACMA2();
                        acma.Iddtext = item.BeyannameNo;
                        acma.Lamvecom = item.Aciklama;
                        acma.Mamveval = item.Kiymet;
                        acma.Nartnumart = item.AcilanKalemNo;
                        acma.Nartnumart1 = item.KalemNo;
                        acma.Qamv = item.Miktar;
                        acma.Camvedev = item.DovizCinsi;
                        acma.Camvenct = item.TeslimSekli;
                        acma.Camvepystrs = item.TicaretUlkesi;
                        acma.Camvetrs = item.IsleminNiteligi;
                        acma.Camvetyppai = item.OdemeSekli;


                        _beyannameAcma.Add(acma);
                    }



                }
                #endregion

                #region Kalem
                List<GOOITEGDS> _kalem = new List<GOOITEGDS>();
                var kalemValues = await _beyannameContext.NbKalem.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                if (kalemValues != null && kalemValues.Count > 0)
                {

                    foreach (var klm in kalemValues)
                    {
                        GOOITEGDS kalem = new GOOITEGDS();

                        kalem.ComCodTarCodGDS10 = klm.Gtip;
                        kalem.ComRefNumGIM1 = klm.KonsimentoRef;
                        kalem.CouOfDesGDS59 = klm.VarisUlkesi;
                        kalem.CouOfDisGDS58 = klm.CikisUlkesi;
                        kalem.DecTypGDS15 = klm.RejimKodu;
                        kalem.GooDesGDS23 = klm.TicariTanim;
                        kalem.GooDesGDS23LNG = klm.TicariTanimDil;
                        kalem.GroMasGDS46 = klm.BurutAgirlik;
                        kalem.IhrBeyanNo = klm.IhrBeyanNo;
                        kalem.IhrBeyanParcali = klm.IhrBeyanParcali;
                        kalem.IhrBeyanTip = klm.IhrBeyanTip;
                        kalem.IteNumGDS7 = klm.KalemSiraNo;
                        if (!string.IsNullOrWhiteSpace(klm.TptChMOdemeKod)) kalem.MetOfPayGDI12 = Convert.ToChar(klm.TptChMOdemeKod);
                        kalem.NetMasGDS48 = klm.NetAgirlik;
                        kalem.UNDanGooCodGDI1 = klm.UNDG;


                        TRACONCO2 _kalemgonderici = new TRACONCO2();
                        var kalemgondericifirmaValues = await _beyannameContext.NbKalemGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo);

                        if (kalemgondericifirmaValues != null)
                        {
                            _kalemgonderici.CitCO224 = kalemgondericifirmaValues.IlIlce;
                            _kalemgonderici.CouCO225 = kalemgondericifirmaValues.UlkeKodu;
                            _kalemgonderici.NADLNGGTCO = kalemgondericifirmaValues.Dil;
                            _kalemgonderici.NamCO27 = kalemgondericifirmaValues.AdUnvan;
                            _kalemgonderici.PosCodCO223 = kalemgondericifirmaValues.PostaKodu;
                            _kalemgonderici.StrAndNumCO222 = kalemgondericifirmaValues.CaddeSokakNo;
                            _kalemgonderici.TINCO259 = kalemgondericifirmaValues.No;

                        }

                        TRACONCE2 _kalemAlici = new TRACONCE2();
                        var kalemalicifirmaValues = await _beyannameContext.NbKalemAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo);

                        if (kalemalicifirmaValues != null)
                        {
                            _kalemAlici.CitCE224 = kalemalicifirmaValues.IlIlce;
                            _kalemAlici.CouCE225 = kalemalicifirmaValues.UlkeKodu;
                            _kalemAlici.NADLNGGICE = kalemalicifirmaValues.Dil;
                            _kalemAlici.NamCE27 = kalemalicifirmaValues.AdUnvan;
                            _kalemAlici.PosCodCE223 = kalemalicifirmaValues.PostaKodu;
                            _kalemAlici.StrAndNumCE222 = kalemalicifirmaValues.CaddeSokakNo;
                            _kalemAlici.TINCE259 = kalemalicifirmaValues.No;

                        }




                        TRACORSECGOO021 _kalemguvenliGonderici = new TRACORSECGOO021();
                        var kalemguvenliGondericifirmaValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo);

                        if (kalemguvenliGondericifirmaValues != null)
                        {
                            _kalemguvenliGonderici.CitTRACORSECGOO022 = kalemguvenliGondericifirmaValues.IlIlce;
                            _kalemguvenliGonderici.CouCodTRACORSECGOO023 = kalemguvenliGondericifirmaValues.UlkeKodu;
                            _kalemguvenliGonderici.TRACORSECGOO021LNG = kalemguvenliGondericifirmaValues.Dil;
                            _kalemguvenliGonderici.NamTRACORSECGOO025 = kalemguvenliGondericifirmaValues.AdUnvan;
                            _kalemguvenliGonderici.PosCodTRACORSECGOO026 = kalemguvenliGondericifirmaValues.PostaKodu;
                            _kalemguvenliGonderici.StrNumTRACORSECGOO027 = kalemguvenliGondericifirmaValues.CaddeSokakNo;
                            _kalemguvenliGonderici.TINTRACORSECGOO028 = kalemguvenliGondericifirmaValues.No;

                        }

                        TRACONSECGOO013 _kalemguvenliAlici = new TRACONSECGOO013();
                        var kalemguvenliAlicifirmaValues = await _beyannameContext.NbKalemGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo);

                        if (kalemguvenliAlicifirmaValues != null)
                        {
                            _kalemguvenliAlici.CityTRACONSECGOO014 = kalemguvenliAlicifirmaValues.IlIlce;
                            _kalemguvenliAlici.CouCodTRACONSECGOO015 = kalemguvenliAlicifirmaValues.UlkeKodu;
                            _kalemguvenliAlici.TRACONSECGOO013LNG = kalemguvenliAlicifirmaValues.Dil;
                            _kalemguvenliAlici.NamTRACONSECGOO017 = kalemguvenliAlicifirmaValues.AdUnvan;
                            _kalemguvenliAlici.PosCodTRACONSECGOO018 = kalemguvenliAlicifirmaValues.PostaKodu;
                            _kalemguvenliAlici.StrNumTRACONSECGOO019 = kalemguvenliAlicifirmaValues.CaddeSokakNo;
                            _kalemguvenliAlici.TINTRACONSECGOO020 = kalemguvenliAlicifirmaValues.No;

                        }

                        List<PREADMREFAR2> _oncekiBelgeler = new List<PREADMREFAR2>();
                        var oncekiBelgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (oncekiBelgelerValues != null && oncekiBelgelerValues.Count > 0)
                        {
                            foreach (var item in oncekiBelgelerValues)
                            {
                                PREADMREFAR2 oncekiBelge = new PREADMREFAR2();
                                oncekiBelge.ComOfInfAR29 = item.TamamlayiciBilgi;
                                oncekiBelge.ComOfInfAR29LNG = item.TamamlayiciBilgiDil;
                                oncekiBelge.PreDocRefAR26 = item.RefNo;
                                oncekiBelge.PreDocRefLNG = item.BelgeDil;
                                oncekiBelge.PreDocTypAR21 = item.BelgeTipi;

                                _oncekiBelgeler.Add(oncekiBelge);
                            }
                        }

                        List<PRODOCDC2> _belgeler = new List<PRODOCDC2>();
                        var belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (belgelerValues != null && belgelerValues.Count > 0)
                        {
                            foreach (var item in belgelerValues)
                            {
                                PRODOCDC2 belge = new PRODOCDC2();
                                belge.ComOfInfDC25 = item.TamamlayiciOlcu;
                                belge.ComOfInfDC25LNG = item.TamamlayiciOlcuDil;
                                belge.DocRefDC23 = item.RefNo;
                                belge.DocRefDCLNG = item.BelgeDil;
                                belge.DocTypDC21 = item.BelgeTipi;

                                _belgeler.Add(belge);
                            }
                        }

                        List<SPEMENMT2> _ekBilgiler = new List<SPEMENMT2>();
                        var ekBilgilerValues = await _beyannameContext.NbEkBilgi.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (ekBilgilerValues != null && ekBilgilerValues.Count > 0)
                        {
                            foreach (var item in ekBilgilerValues)
                            {
                                SPEMENMT2 ekBilgi = new SPEMENMT2();
                                ekBilgi.AddInfCodMT23 = item.EkBilgiKod;
                                ekBilgi.AddInfMT21 = item.EkBilgi;
                                ekBilgi.AddInfMT21LNG = item.Dil;
                                ekBilgi.ExpFroCouMT25 = item.UlkeKodu;
                                ekBilgi.ExpFroECMT24 = item.Ec2Ihr;

                                _ekBilgiler.Add(ekBilgi);
                            }
                        }


                        List<PACGS2> _kaplar = new List<PACGS2>();
                        var kapValues = await _beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (kapValues != null && kapValues.Count > 0)
                        {
                            foreach (var item in kapValues)
                            {
                                PACGS2 kap = new PACGS2();
                                kap.KinOfPacGS23 = item.KapTipi;
                                kap.MarNumOfPacGS21 = item.MarkaNo;
                                kap.MarNumOfPacGS21LNG = item.MarkaDil;
                                kap.NumOfPacGS24 = item.KapAdet;
                                kap.NumOfPieGS25 = item.ParcaSayisi;

                                _kaplar.Add(kap);
                            }
                        }

                        List<CONNR2> _konteynerlar = new List<CONNR2>();
                        var konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (konteynerValues != null && konteynerValues.Count > 0)
                        {
                            foreach (var item in konteynerValues)
                            {
                                CONNR2 konteyner = new CONNR2();
                                konteyner.ConNumNR21 = item.KonteynerNo;
                                konteyner.KonteynerUlke = item.Ulke;


                                _konteynerlar.Add(konteyner);
                            }
                        }


                        List<SGICODSD2> _hassaslar = new List<SGICODSD2>();
                        var hassasValues = await _beyannameContext.NbHassasEsya.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == klm.KalemInternalNo).ToListAsync();

                        if (hassasValues != null && hassasValues.Count > 0)
                        {
                            foreach (var item in hassasValues)
                            {
                                SGICODSD2 hassas = new SGICODSD2();
                                hassas.SenGooCodSD22 = item.Kod;
                                hassas.SenQuaSD23 = item.Miktar;

                                _hassaslar.Add(hassas);
                            }
                        }

                        kalem.TRACONCE2 = _kalemAlici;
                        kalem.TRACONCO2 = _kalemgonderici;
                        kalem.TRACONSECGOO013 = _kalemguvenliAlici;
                        kalem.TRACORSECGOO021 = _kalemguvenliGonderici;
                        kalem.PREADMREFAR2 = _oncekiBelgeler;
                        kalem.PRODOCDC2 = _belgeler;
                        kalem.SPEMENMT2 = _ekBilgiler;
                        kalem.PACGS2 = _kaplar;
                        kalem.CONNR2 = _konteynerlar;
                        kalem.SGICODSD2 = _hassaslar;

                        _kalem.Add(kalem);
                    }

                }

                #endregion


                gelen.TRAPRIPC1 = _asilSorumlu;
                gelen.TRACONCE1 = _alici;
                gelen.TRACONCO1 = _gonderici;
                gelen.TRAAUTCONTRA = _varisYetkilisi;
                gelen.CUSOFFDEPEPT = _hareketGumruk;
                gelen.CUSOFFTRARNS = _transitGumrukList;
                gelen.CUSOFFDESEST = _varisGumruk;
                gelen.CONRESERS = _kontrolSonuc;
                gelen.REPREP = _temsilci;
                gelen.SEAINFSLI = _muhurList;
                gelen.ITI = _rotaList;
                gelen.CARTRA100 = _tasiyici;
                gelen.TRACONSEC029 = _guvenliAlici;
                gelen.TRACORSEC037 = _guvenliGonderici;
                gelen.HEAHEA = _beyan;
                gelen.GOOITEGDS = _kalem;
                gelen.GUAGUA = _teminatList;
                gelen.AB = _beyannameAcma;
                gelen.OB = _ozetBeyanAcma;

                gelen.SynIdeMES1 = "UNOC";
                gelen.MesSenMES3 = "NTA.TR";
                gelen.MesRecMES6 = "NTA.TR";
                gelen.SynVerNumMES2 = "3";
                gelen.DatOfPreMES9 = DateTime.Now.Year.ToString().Substring(0,2)+ DateTime.Now.Month.ToString()+ DateTime.Now.Day.ToString();
                gelen.TimOfPreMES10 = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                gelen.IntConRefMES11 = nctsBeyanValues.RefNo;
                gelen.AckReqMES16 = "0";
                gelen.TesIndMES18 = "0";
                gelen.MesIdeMES19 = "CC015BTR";
                gelen.MesTypMES20 = "CC015B";
                gelen.ComAccRefMES21 = "";
                          
              

                string imzasizMesaj = SerializeToXML(gelen);


                string guidOf = "BYT:" + Guid.NewGuid().ToString();
                string newIslemInternalNo = IslemInternalNo;
                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {


                        if (islemValues.IslemTipi == "Ncts")
                        {

                            islemValues.Kullanici = Kullanici;
                            islemValues.IslemDurumu = "Imzala";
                            islemValues.IslemZamani = DateTime.Now;
                            islemValues.SonIslemZamani = DateTime.Now;
                            islemValues.IslemSonucu = "Tescil Mesajı Oluşturuldu";
                            islemValues.Guidof = guidOf;
                            islemValues.GonderimSayisi++;
                            _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        }
                        else
                        {
                            Islem _islem = new Islem();
                            newIslemInternalNo = islemValues.BeyanInternalNo.Replace("NB", "NBTG");
                            _islem.Kullanici = islemValues.Kullanici;
                            _islem.IslemDurumu = "Imzala";
                            _islem.IslemInternalNo = newIslemInternalNo;
                            _islem.IslemZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.IslemSonucu = "Tescil Mesajı Oluşturuldu";
                            _islem.Guidof = guidOf;
                            _islem.RefNo = islemValues.RefNo;
                            _islem.BeyanInternalNo = islemValues.BeyanInternalNo;
                            _islem.BeyanTipi = islemValues.BeyanTipi;
                            _islem.IslemTipi = "Ncts";
                            _islem.GonderimSayisi++;
                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                        }
                        //TODO: bu guid dışında imzala aşamasında kalmış aynı IslemInternalNo ile ilgili kayıtlar iptal edilsin
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = nctsBeyanValues.HareketGumruk;
                        _tarihce.Rejim = nctsBeyanValues.BeyanTipi;
                        _tarihce.IslemInternalNo = newIslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = "Imzala";
                        _tarihce.IslemSonucu = "Tescil Mesajı Oluşturuldu";
                        _tarihce.IslemTipi = "4";
                        _tarihce.TicaretTipi = nctsBeyanValues.Rejim;
                        _tarihce.GonderilecekVeri = imzasizMesaj;
                        _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;

                        nctsBeyanValues.SonIslemZamani = DateTime.Now;
                        nctsBeyanValues.TescilStatu = "Tescil Mesajı Oluşturuldu";
                        _beyannameContext.Entry(nctsBeyanValues).State = EntityState.Modified;

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

        [Route("api/BYT/Servis/Ncts/[controller]/{IslemInternalNo}/{Kullanici}/{Guid}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}/{Guid}")]
        public async Task<ServisDurum> GetTescil(string IslemInternalNo, string Kullanici, string Guid)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<NctsDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new NctsDataContext(options);
            try
            {

                var tarihceValues = await _islemTarihceContext.Tarihce.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guid == Guid.Trim() && v.IslemDurumu == "Imzalandi");
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guidof == Guid.Trim() && v.IslemDurumu == "Imzalandi");

                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                     .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                     .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);

                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();
                string guidOf = "", IslemDurumu = "", islemSonucu = "";
            
                NctsHizmeti.WS2ServiceClient Tescil = ServiceHelper.GetNctsWSClient(_servisCredential.username, _servisCredential.password);
                string FIRM_ID = "BYT";
                string USER_ID = DateTime.Now.ToString("yyyyMMddHHmmss") +","+kullaniciValues.KullaniciKod+","+kullaniciValues.KullaniciSifre;
               if(islemValues.BeyanNo!="")
                {
                  
                    //Tescil.uploadmessagebymrnAsync; kullanılmıyor.
                    var result = await Tescil.uploadmessagebylrnAsync(FIRM_ID, USER_ID, islemValues.BeyanNo, "CC015B_R", "1", tarihceValues.ImzaliVeri);//düzeltme mesaj
                    guidOf = result.ToString();
                    //result için okuma? Hata? Guid?  000 ise başarılı, 000 değilse başarısız
                }
                else
                {

                    var result = await Tescil.submitdeclarationAsync(FIRM_ID, USER_ID, "CC015B_F", "1", tarihceValues.ImzaliVeri);//ilk mesaj
                    guidOf = result.ToString();
                    //result için okuma? Hata? Guid?  000 ise başarılı, 000 değilse başarısız
                }


                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(root.OuterXml);

                //if (doc.HasChildNodes)
                //{
                //    foreach (XmlNode n in doc.ChildNodes[0].ChildNodes)
                //    {

                //        if (n.Name == "Message")
                //        {
                //            IslemDurumu = "Hata";
                //            islemSonucu = n.InnerText;
                //            break;
                //        }

                //        else
                //        {
                //            if (n.Name == "Guid")
                //            {
                //                IslemDurumu = "Islemde";
                //                guidOf = n.InnerText;


                //            }
                //            else if (n.Name == "Durum")
                //            {
                //                islemSonucu = n.InnerText;


                //            }
                //        }

                //    }
                //}


                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {
                        islemValues.Kullanici = Kullanici;
                        islemValues.IslemDurumu = "Gonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("NB", "NBTG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "Tescil";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();



                        tarihceValues.Guid = guidOf;
                        tarihceValues.IslemDurumu = IslemDurumu;
                        tarihceValues.IslemSonucu = islemSonucu;
                        tarihceValues.GondermeZamani = DateTime.Now;
                        tarihceValues.SonIslemZamani = DateTime.Now;
                        tarihceValues.GonderimNo = islemValues.GonderimSayisi;


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