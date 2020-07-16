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
    public class NctsBeyanKopyalamaController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        public IConfiguration Configuration { get; }

        public NctsBeyanKopyalamaController(IslemTarihceDataContext islemTarihcecontext,  IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
      

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpPost("{IslemInternalNo}")]
        public async Task<ServisDurum> PostKopylama(string IslemInternalNo)
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
                    var nctsBeyanValues = await _beyannameContext.NbBeyan.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.NbKalem.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    var internalrefid = _beyannameContext.GetRefIdNextSequenceValue(nctsBeyanValues.Rejim);
                    string InternalNo = nctsBeyanValues.Rejim + nctsBeyanValues.Kullanici + "NB" + internalrefid.ToString().PadLeft(5, '0');

                    List<NbKalem> lstKalem = new List<NbKalem>();
                    List<NbAbAcma> lstAbAcma = new List<NbAbAcma>();
                    List<NbObAcma> lstObAcma = new List<NbObAcma>();
                    List<NbTransitGumruk> lstTransitGumruk = new List<NbTransitGumruk>();
                    List<NbMuhur> lstMuhur= new List<NbMuhur>();
                    List<NbRota> lstRota = new List<NbRota>();
                    List<NbTeminat> lstTeminat = new List<NbTeminat>();
                    NbTasiyiciFirma tasiyiciFirma = new NbTasiyiciFirma();
                    NbAsilSorumluFirma asilSorumluFirma = new NbAsilSorumluFirma();
                    NbAliciFirma aliciFirma = new NbAliciFirma();
                    NbGondericiFirma gondericiFirma = new NbGondericiFirma();
                    NbGuvenliAliciFirma guvenlialiciFirma = new NbGuvenliAliciFirma();
                    NbGuvenliGondericiFirma guvenligondericiFirma = new NbGuvenliGondericiFirma();
                    List<NbBelgeler> lstBelgeler = new List<NbBelgeler>();
                    List<NbOncekiBelgeler> lstOncekiBelgeler = new List<NbOncekiBelgeler>();
                    List<NbEkBilgi> lstEkBilgiler = new List<NbEkBilgi>();
                    List<NbKap> lstKap = new List<NbKap>();
                    List<NbKonteyner> lstKonteyner = new List<NbKonteyner>();
                    List<NbHassasEsya> lstHassasEsya = new List<NbHassasEsya>();
                    List<NbKalemAliciFirma> lstkalemaliciFirma = new List<NbKalemAliciFirma>();
                    List < NbKalemGondericiFirma> lstkalemgondericiFirma = new List<NbKalemGondericiFirma>();
                    List < NbKalemGuvenliAliciFirma> lstkalemguvenlialiciFirma = new List<NbKalemGuvenliAliciFirma>();
                    List < NbKalemGuvenliGondericiFirma> lstkalemguvenligondericiFirma = new  List<NbKalemGuvenliGondericiFirma>();

                    var newnctsBeyanValues = new NbBeyan
                    {
                        BeyanTipi= nctsBeyanValues.BeyanTipi,
                        BeyanTipiDil=nctsBeyanValues.BeyanTipiDil,
                        BosaltmaYer=nctsBeyanValues.BosaltmaYer,
                        GuvenliBeyan= nctsBeyanValues.GuvenliBeyan,
                        HareketGumruk= nctsBeyanValues.HareketGumruk,
                        CikisTasimaSekli= nctsBeyanValues.CikisTasimaSekli,
                        CikisTasitKimligi= nctsBeyanValues.CikisTasitKimligi,
                        CikisTasitKimligiDil= nctsBeyanValues.CikisTasitKimligiDil,
                        CikisTasitUlke= nctsBeyanValues.CikisTasitUlke,
                        CikisUlke= nctsBeyanValues.CikisUlke,
                        DahildeTasimaSekli= nctsBeyanValues.DahildeTasimaSekli,
                        DamgaVergi= nctsBeyanValues.DamgaVergi,
                        Dorse1= nctsBeyanValues.Dorse1,
                        Dorse2= nctsBeyanValues.Dorse2,
                        EsyaKabulYer= nctsBeyanValues.EsyaKabulYer,
                        EsyaOnayYer= nctsBeyanValues.EsyaOnayYer,
                        EsyaYer= nctsBeyanValues.EsyaYer,
                        KalemSayisi= nctsBeyanValues.KalemSayisi,
                        KalemToplamBrutKG= nctsBeyanValues.KalemToplamBrutKG,
                        Konteyner= nctsBeyanValues.Konteyner,
                        KontrolSonuc= nctsBeyanValues.KontrolSonuc,
                        KonveyansRefNo= nctsBeyanValues.KonveyansRefNo,
                        Kullanici= nctsBeyanValues.Kullanici,
                        MusavirKimlikNo= nctsBeyanValues.MusavirKimlikNo,
                        NctsBeyanInternalNo= InternalNo,
                        RefaransNo= nctsBeyanValues.RefaransNo,
                        RefNo= nctsBeyanValues.RefNo,
                        Rejim= nctsBeyanValues.Rejim,
                        OdemeAraci= nctsBeyanValues.OdemeAraci,
                        SinirGumruk= nctsBeyanValues.SinirGumruk,
                        SinirTasimaSekli= nctsBeyanValues.SinirTasimaSekli,
                        SinirTasitKimligi= nctsBeyanValues.SinirTasitKimligi,
                        SinirTasitKimligiDil= nctsBeyanValues.SinirTasitKimligiDil,
                        SinirTasitUlke= nctsBeyanValues.SinirTasitUlke,
                        SonIslemZamani=DateTime.Now,
                        SureSinir= nctsBeyanValues.SureSinir,
                        ToplamKapSayisi= nctsBeyanValues.ToplamKapSayisi,
                        Tanker= nctsBeyanValues.Tanker,
                        Temsilci= nctsBeyanValues.Temsilci,
                        TemsilKapasite= nctsBeyanValues.TemsilKapasite,
                        TemsilKapasiteDil= nctsBeyanValues.TemsilKapasiteDil,
                        TescilTarihi=DateTime.Now,
                        VarisGumruk= nctsBeyanValues.VarisGumruk,
                        VarisGumrukYetkilisi= nctsBeyanValues.VarisGumrukYetkilisi,
                        VarisUlke= nctsBeyanValues.VarisUlke,
                        Yer= nctsBeyanValues.Yer,
                        YukBosYerDil= nctsBeyanValues.YukBosYerDil,
                        YerTarihDil= nctsBeyanValues.YerTarihDil,
                        YuklemeYeri= nctsBeyanValues.YuklemeYeri,
                        TescilStatu = "Olusturuldu",
                        OlsuturulmaTarihi = DateTime.Now

                    };
                 
                    var teminatValues = await _beyannameContext.NbTeminat.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var t in teminatValues)
                    {
                        NbTeminat teminat = new NbTeminat
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            DigerRefNo=t.DigerRefNo,
                            DovizCinsi=t.DovizCinsi,
                            ECGecerliDegil=t.ECGecerliDegil,
                            ErisimKodu=t.ErisimKodu,
                            TeminatTipi=t.TeminatTipi,
                            Tutar=t.Tutar,
                            GRNNo=t.GRNNo,
                            UlkeGecerliDegil=t.UlkeGecerliDegil,
                            SonIslemZamani=DateTime.Now,
                        


                        };
                        lstTeminat.Add(teminat);
                    }

                    var obAcmaValues = await _beyannameContext.NbObAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var o in obAcmaValues)
                    {
                        NbObAcma acma = new NbObAcma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AmbarIci=o.AmbarIci,
                            AmbarKodu=o.AmbarKodu,
                            IslemKapsami=o.IslemKapsami,
                            Miktar=o.Miktar,
                            OzetBeyanNo=o.OzetBeyanNo,
                            TasimaSatirNo=o.TasimaSatirNo,
                            TasimaSenetNo=o.TasimaSenetNo,
                            SonIslemZamani=DateTime.Now


                        };
                        lstObAcma.Add(acma);
                    }
                    var abAcmaValues = await _beyannameContext.NbAbAcma.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var a in abAcmaValues)
                    {
                        NbAbAcma acma = new NbAbAcma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            Aciklama = a.Aciklama,
                            AcilanKalemNo=a.AcilanKalemNo,
                            BeyannameNo=a.BeyannameNo,
                            DovizCinsi=a.DovizCinsi,
                            IsleminNiteligi=a.IsleminNiteligi,
                            KalemNo=a.KalemNo,
                            Kiymet=a.Kiymet,
                            Miktar=a.Miktar,
                            OdemeSekli=a.OdemeSekli,
                            TeslimSekli=a.TeslimSekli,
                            TicaretUlkesi=a.TicaretUlkesi,
                            SonIslemZamani=DateTime.Now

                        };
                        lstAbAcma.Add(acma);
                    }

                    var transitGumrukValues = await _beyannameContext.NbTransitGumruk.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var trg in transitGumrukValues)
                    {
                        NbTransitGumruk transit = new NbTransitGumruk
                        {
                           NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                           Gumruk= trg.Gumruk,
                           VarisTarihi= trg.VarisTarihi,                         
                           SonIslemZamani = DateTime.Now


                        };
                        lstTransitGumruk.Add(transit);
                    }

                    var muhurValues = await _beyannameContext.NbMuhur.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var m in muhurValues)
                    {
                        NbMuhur muhur = new NbMuhur
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            Dil = m.Dil,
                            MuhurNo = m.MuhurNo,
                            SonIslemZamani = DateTime.Now


                        };
                        lstMuhur.Add(muhur);
                    }

                    var rotaValues = await _beyannameContext.NbRota.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var r in rotaValues)
                    {
                        NbRota rota = new NbRota
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            UlkeKodu = r.UlkeKodu,
                            SonIslemZamani = DateTime.Now


                        };
                        lstRota.Add(rota);
                    }

                    
                    var asilSorumlufirmaValues = await _beyannameContext.NbAsilSorumluFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (asilSorumlufirmaValues != null)
                    {
                        asilSorumluFirma = new NbAsilSorumluFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan = asilSorumlufirmaValues.AdUnvan,
                            CaddeSokakNo = asilSorumlufirmaValues.CaddeSokakNo,
                            IlIlce = asilSorumlufirmaValues.IlIlce,
                            No = asilSorumlufirmaValues.No,
                            PostaKodu = asilSorumlufirmaValues.PostaKodu,
                            UlkeKodu = asilSorumlufirmaValues.UlkeKodu,
                            Dil = asilSorumlufirmaValues.Dil,
                            SonIslemZamani = DateTime.Now
                        };
                    }

                    var tasiyicifirmaValues = await _beyannameContext.NbTasiyiciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (tasiyicifirmaValues != null)
                    {
                        tasiyiciFirma = new NbTasiyiciFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan = tasiyicifirmaValues.AdUnvan,
                            CaddeSokakNo = tasiyicifirmaValues.CaddeSokakNo,
                            IlIlce = tasiyicifirmaValues.IlIlce,
                            No = tasiyicifirmaValues.No,
                            PostaKodu = tasiyicifirmaValues.PostaKodu,
                            UlkeKodu = tasiyicifirmaValues.UlkeKodu,
                            Dil = tasiyicifirmaValues.Dil,
                            SonIslemZamani = DateTime.Now
                        };
                    }

                    var alicifirmaValues = await _beyannameContext.NbAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (alicifirmaValues != null)
                    {
                        aliciFirma = new NbAliciFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan= alicifirmaValues.AdUnvan,
                            CaddeSokakNo= alicifirmaValues.CaddeSokakNo,
                            IlIlce= alicifirmaValues.IlIlce,
                            No= alicifirmaValues.No,
                            PostaKodu= alicifirmaValues.PostaKodu,
                            UlkeKodu= alicifirmaValues.UlkeKodu,
                            Dil= alicifirmaValues.Dil,
                            SonIslemZamani=DateTime.Now
                        };
                    }

                    var gondericifirmaValues = await _beyannameContext.NbGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (gondericifirmaValues != null)
                    {
                        gondericiFirma = new NbGondericiFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan = gondericifirmaValues.AdUnvan,
                            CaddeSokakNo = gondericifirmaValues.CaddeSokakNo,
                            IlIlce = gondericifirmaValues.IlIlce,
                            No = gondericifirmaValues.No,
                            PostaKodu = gondericifirmaValues.PostaKodu,
                            UlkeKodu = gondericifirmaValues.UlkeKodu,
                            Dil = gondericifirmaValues.Dil,
                            SonIslemZamani = DateTime.Now
                        };
                    }

                    var guvenlialicifirmaValues = await _beyannameContext.NbGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (guvenlialicifirmaValues != null)
                    {
                        guvenlialiciFirma = new NbGuvenliAliciFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan = guvenlialicifirmaValues.AdUnvan,
                            CaddeSokakNo = guvenlialicifirmaValues.CaddeSokakNo,
                            IlIlce = guvenlialicifirmaValues.IlIlce,
                            No = guvenlialicifirmaValues.No,
                            PostaKodu = guvenlialicifirmaValues.PostaKodu,
                            UlkeKodu = guvenlialicifirmaValues.UlkeKodu,
                            Dil = guvenlialicifirmaValues.Dil,
                            SonIslemZamani = DateTime.Now
                        };
                    }

                    var guvenligondericifirmaValues = await _beyannameContext.NbGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (guvenligondericifirmaValues != null)
                    {
                        guvenligondericiFirma = new NbGuvenliGondericiFirma
                        {
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                            AdUnvan = guvenligondericifirmaValues.AdUnvan,
                            CaddeSokakNo = guvenligondericifirmaValues.CaddeSokakNo,
                            IlIlce = guvenligondericifirmaValues.IlIlce,
                            No = guvenligondericifirmaValues.No,
                            PostaKodu = guvenligondericifirmaValues.PostaKodu,
                            UlkeKodu = guvenligondericifirmaValues.UlkeKodu,
                            Dil = guvenligondericifirmaValues.Dil,
                            SonIslemZamani = DateTime.Now
                        };
                    }

                 

                    foreach (var x in kalemValues)
                    {

                        NbKalem kalem = new NbKalem
                        {
                            BurutAgirlik = x.BurutAgirlik,
                            IhrBeyanNo = x.IhrBeyanNo,
                            IhrBeyanParcali = x.IhrBeyanParcali,
                            IhrBeyanTip = x.IhrBeyanTip,
                            Gtip = x.Gtip,
                            CikisUlkesi = x.CikisUlkesi,
                            KalemSiraNo = x.KalemSiraNo,
                            Kiymet = x.Kiymet,
                            KiymetDoviz = x.KiymetDoviz,
                            KonsimentoRef = x.KonsimentoRef,
                            NetAgirlik = x.NetAgirlik,
                            RejimKodu = x.RejimKodu,
                            TicariTanim = x.TicariTanim,
                            TicariTanimDil = x.TicariTanimDil,
                            TptChMOdemeKod = x.TptChMOdemeKod,
                            UNDG = x.UNDG,
                            VarisUlkesi = x.VarisUlkesi,
                            SonIslemZamani = DateTime.Now,
                            KalemInternalNo = newnctsBeyanValues.NctsBeyanInternalNo + "|" + x.KalemSiraNo,
                            NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,


                        };

                        var belgelerValues = await _beyannameContext.NbBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var blg in belgelerValues)
                        {
                            NbBelgeler b = new NbBelgeler
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                BelgeDil=blg.BelgeDil,
                                BelgeTipi=blg.BelgeTipi,
                                RefNo=blg.RefNo,
                                TamamlayiciOlcu=blg.TamamlayiciOlcu,
                                TamamlayiciOlcuDil=blg.TamamlayiciOlcuDil,
                                SonIslemZamani=DateTime.Now
                             

                            };
                            lstBelgeler.Add(b);
                        }

                        var oncekibelgelerValues = await _beyannameContext.NbOncekiBelgeler.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var oblg in oncekibelgelerValues)
                        {
                            NbOncekiBelgeler b = new NbOncekiBelgeler
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                BelgeDil = oblg.BelgeDil,
                                BelgeTipi = oblg.BelgeTipi,
                                RefNo = oblg.RefNo,
                                TamamlayiciBilgi = oblg.TamamlayiciBilgi,
                                TamamlayiciBilgiDil = oblg.TamamlayiciBilgiDil,
                                SonIslemZamani = DateTime.Now


                            };
                            lstOncekiBelgeler.Add(b);
                        }

                        var ekBilgiValues = await _beyannameContext.NbEkBilgi.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var bilgi in ekBilgiValues)
                        {
                            NbEkBilgi bilgii = new NbEkBilgi
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                Dil = bilgi.Dil,
                                Ec2Ihr = bilgi.Ec2Ihr,
                                EkBilgi = bilgi.EkBilgi,
                                EkBilgiKod = bilgi.EkBilgiKod,
                                UlkeKodu = bilgi.UlkeKodu,
                                SonIslemZamani = DateTime.Now


                            };
                            lstEkBilgiler.Add(bilgii);
                        }

                        var kapValues = await _beyannameContext.NbKap.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var kap in kapValues)
                        {
                            NbKap kp = new NbKap
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                KapAdet = kap.KapAdet,
                                KapTipi = kap.KapTipi,
                                MarkaDil = kap.MarkaDil,
                                MarkaNo = kap.MarkaNo,
                                ParcaSayisi = kap.ParcaSayisi,
                                SonIslemZamani = DateTime.Now


                            };
                            lstKap.Add(kp);
                        }

                        var konteynerValues = await _beyannameContext.NbKonteyner.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var konteyner in konteynerValues)
                        {
                            NbKonteyner kont = new NbKonteyner
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                KonteynerNo = konteyner.KonteynerNo,
                                Ulke = konteyner.Ulke,
                                SonIslemZamani = DateTime.Now


                            };
                            lstKonteyner.Add(kont);
                        }

                        var hassasEsyaValues = await _beyannameContext.NbHassasEsya.Where(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();

                        foreach (var hssEsya in hassasEsyaValues)
                        {
                            NbHassasEsya hs = new NbHassasEsya
                            {
                                NctsBeyanInternalNo = kalem.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                Kod = hssEsya.Kod,
                                SonIslemZamani = DateTime.Now


                            };
                            lstHassasEsya.Add(hs);
                        }

                        var kalemalicifirmaValues = await _beyannameContext.NbKalemAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo);
                        if (kalemalicifirmaValues != null)
                        {
                            NbKalemAliciFirma kalemaliciFirma = new NbKalemAliciFirma
                            {
                                NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                AdUnvan = kalemalicifirmaValues.AdUnvan,
                                CaddeSokakNo = kalemalicifirmaValues.CaddeSokakNo,
                                IlIlce = kalemalicifirmaValues.IlIlce,
                                No = kalemalicifirmaValues.No,
                                PostaKodu = kalemalicifirmaValues.PostaKodu,
                                UlkeKodu = kalemalicifirmaValues.UlkeKodu,
                                Dil = kalemalicifirmaValues.Dil,
                                SonIslemZamani = DateTime.Now
                            };
                            lstkalemaliciFirma.Add(kalemaliciFirma);
                        }

                        var kalemgondericifirmaValues = await _beyannameContext.NbKalemGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo);
                        if (kalemgondericifirmaValues != null)
                        {
                            NbKalemGondericiFirma kalemgondericiFirma = new NbKalemGondericiFirma
                            {
                                NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                AdUnvan = kalemgondericifirmaValues.AdUnvan,
                                CaddeSokakNo = kalemgondericifirmaValues.CaddeSokakNo,
                                IlIlce = kalemgondericifirmaValues.IlIlce,
                                No = kalemgondericifirmaValues.No,
                                PostaKodu = kalemgondericifirmaValues.PostaKodu,
                                UlkeKodu = kalemgondericifirmaValues.UlkeKodu,
                                Dil = kalemgondericifirmaValues.Dil,
                                SonIslemZamani = DateTime.Now
                            };
                            lstkalemgondericiFirma.Add(kalemgondericiFirma);
                        }

                        var kalemguvenlialicifirmaValues = await _beyannameContext.NbKalemGuvenliAliciFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo);
                        if (kalemguvenlialicifirmaValues != null)
                        {
                            NbKalemGuvenliAliciFirma kalemguvenlialicifirma = new NbKalemGuvenliAliciFirma
                            {
                                NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                AdUnvan = kalemguvenlialicifirmaValues.AdUnvan,
                                CaddeSokakNo = kalemguvenlialicifirmaValues.CaddeSokakNo,
                                IlIlce = kalemguvenlialicifirmaValues.IlIlce,
                                No = kalemguvenlialicifirmaValues.No,
                                PostaKodu = kalemguvenlialicifirmaValues.PostaKodu,
                                UlkeKodu = kalemguvenlialicifirmaValues.UlkeKodu,
                                Dil = kalemguvenlialicifirmaValues.Dil,
                                SonIslemZamani = DateTime.Now
                            };
                            lstkalemguvenlialiciFirma.Add(kalemguvenlialicifirma);
                        }

                        var kalemguvenligondericifirmaValues = await _beyannameContext.NbKalemGuvenliGondericiFirma.FirstOrDefaultAsync(v => v.NctsBeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo);
                        if (kalemguvenligondericifirmaValues != null)
                        {
                            NbKalemGuvenliGondericiFirma kalemguvenligondericifirma = new NbKalemGuvenliGondericiFirma
                            {
                                NctsBeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo,
                                KalemInternalNo = kalem.KalemInternalNo,
                                AdUnvan = kalemguvenligondericifirmaValues.AdUnvan,
                                CaddeSokakNo = kalemguvenligondericifirmaValues.CaddeSokakNo,
                                IlIlce = kalemguvenligondericifirmaValues.IlIlce,
                                No = kalemguvenligondericifirmaValues.No,
                                PostaKodu = kalemguvenligondericifirmaValues.PostaKodu,
                                UlkeKodu = kalemguvenligondericifirmaValues.UlkeKodu,
                                Dil = kalemguvenligondericifirmaValues.Dil,
                                SonIslemZamani = DateTime.Now
                            };
                            lstkalemguvenligondericiFirma.Add(kalemguvenligondericifirma);
                        }


                        lstKalem.Add(kalem);
                    }



                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {
                          
                            _beyannameContext.Entry(newnctsBeyanValues).State = EntityState.Added;
                            foreach (var item in lstKalem)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstAbAcma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstObAcma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstTransitGumruk)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstMuhur)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstRota)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstTeminat)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstBelgeler)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstOncekiBelgeler)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstEkBilgiler)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstKap)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstKonteyner)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstHassasEsya)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }

                            if(tasiyiciFirma.NctsBeyanInternalNo!=null)
                                _beyannameContext.Entry(tasiyiciFirma).State = EntityState.Added;

                            if (asilSorumluFirma.NctsBeyanInternalNo != null)
                                _beyannameContext.Entry(asilSorumluFirma).State = EntityState.Added;

                            if (aliciFirma.NctsBeyanInternalNo != null)
                                _beyannameContext.Entry(aliciFirma).State = EntityState.Added;

                            if (gondericiFirma.NctsBeyanInternalNo != null)
                                _beyannameContext.Entry(gondericiFirma).State = EntityState.Added;

                            if (guvenlialiciFirma.NctsBeyanInternalNo != null)
                                _beyannameContext.Entry(guvenlialiciFirma).State = EntityState.Added;

                            if (guvenligondericiFirma.NctsBeyanInternalNo != null)
                                _beyannameContext.Entry(guvenligondericiFirma).State = EntityState.Added;

                            foreach (var item in lstkalemguvenlialiciFirma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstkalemguvenligondericiFirma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstkalemgondericiFirma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstkalemaliciFirma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }

                            await _beyannameContext.SaveChangesAsync();


                            Islem _islem = new Islem();
                            _islem.Kullanici = newnctsBeyanValues.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "Ncts";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = newnctsBeyanValues.RefNo;
                            _islem.BeyanInternalNo = newnctsBeyanValues.NctsBeyanInternalNo;
                            _islem.IslemInternalNo = newnctsBeyanValues.NctsBeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();
                            lstbilgi.Add(new Bilgi { IslemTipi = "Beyanname Kopyalama", ReferansNo = newnctsBeyanValues.NctsBeyanInternalNo, Sonuc = "Kopyalama Başarılı", SonucVeriler = null, GUID = null });
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