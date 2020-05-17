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
    public class BeyannameKopyalamaController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }

        public BeyannameKopyalamaController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [HttpPost("{IslemInternalNo}")]
        public async Task<ServisDurum> PostKopylama(string IslemInternalNo)
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
                    var ozetBeyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    var internalrefid = _beyannameContext.GetRefIdNextSequenceValue(ozetBeyanValues.Rejim);
                    string InternalNo = ozetBeyanValues.Rejim+ozetBeyanValues.Kullanici + "DB" + internalrefid.ToString().PadLeft(5, '0');
                    

                    List<DbKalem> lstKalem = new List<DbKalem>();
                    List<DbOdemeSekli> lstOdeme = new List<DbOdemeSekli>();
                    List<DbTamamlayiciBilgi> lstTamam = new List<DbTamamlayiciBilgi>();
                    List<DbKonteyner> lstKonteyner = new List<DbKonteyner>();
                    List<DbMarka> lstMarka = new List<DbMarka>();
                    List<DbBeyannameAcma> lstBeyannameAcma = new List<DbBeyannameAcma>();
                    List<DbOzetBeyanAcma> lstOzetBeyan = new List<DbOzetBeyanAcma>();
                    List<DbOzetBeyanAcmaTasimaSenet> lstTasimaSenedi = new List<DbOzetBeyanAcmaTasimaSenet>();
                    List<DbOzetBeyanAcmaTasimaSatir> lstTasimaSatiri = new List<DbOzetBeyanAcmaTasimaSatir>();
                    List<DbKiymetBildirim> lstKiymet = new List<DbKiymetBildirim>();
                    List<DbKiymetBildirimKalem> lstKiymetKalem = new List<DbKiymetBildirimKalem>();
                
                    var newozetBeyanValues = new DbBeyan
                    {
                        Aciklamalar = ozetBeyanValues.Aciklamalar,
                        AliciSaticiIliskisi = ozetBeyanValues.AliciSaticiIliskisi,
                        AliciVergiNo = ozetBeyanValues.AliciVergiNo,
                        AntrepoKodu = ozetBeyanValues.AntrepoKodu,
                        AsilSorumluVergiNo = ozetBeyanValues.AsilSorumluVergiNo,
                        BankaKodu = ozetBeyanValues.BankaKodu,
                        BasitlestirilmisUsul = ozetBeyanValues.BasitlestirilmisUsul,
                        //BeyannameNo = ozetBeyanValues.BeyannameNo,
                        BeyanSahibiVergiNo = ozetBeyanValues.BeyanSahibiVergiNo,
                        BirlikKayitNumarasi = ozetBeyanValues.BirlikKayitNumarasi,
                        BirlikKriptoNumarasi = ozetBeyanValues.BirlikKriptoNumarasi,
                        CikistakiAracinKimligi = ozetBeyanValues.CikistakiAracinKimligi,
                        CikistakiAracinTipi = ozetBeyanValues.CikistakiAracinTipi,
                        CikistakiAracinUlkesi = ozetBeyanValues.CikistakiAracinUlkesi,
                        CikisUlkesi = ozetBeyanValues.CikisUlkesi,
                        EsyaninBulunduguYer = ozetBeyanValues.EsyaninBulunduguYer,
                        GidecegiSevkUlkesi = ozetBeyanValues.GidecegiSevkUlkesi,
                        GidecegiUlke = ozetBeyanValues.GidecegiUlke,
                        GirisGumrukIdaresi = ozetBeyanValues.GirisGumrukIdaresi,
                        GondericiVergiNo = ozetBeyanValues.GondericiVergiNo,
                        Gumruk = ozetBeyanValues.Gumruk,
                        IsleminNiteligi = ozetBeyanValues.IsleminNiteligi,
                        KapAdedi = ozetBeyanValues.KapAdedi,
                        Konteyner = ozetBeyanValues.Konteyner,
                        Kullanici = ozetBeyanValues.Kullanici,
                        LimanKodu = ozetBeyanValues.LimanKodu,
                        Mail1 = ozetBeyanValues.Mail1,
                        Mail2 = ozetBeyanValues.Mail2,
                        Mail3 = ozetBeyanValues.Mail3,
                        Mobil1 = ozetBeyanValues.Mobil1,
                        Mobil2 = ozetBeyanValues.Mobil2,
                        MusavirVergiNo = ozetBeyanValues.MusavirVergiNo,
                        OdemeAraci = ozetBeyanValues.OdemeAraci,
                        MusavirReferansNo = ozetBeyanValues.MusavirReferansNo,
                        ReferansTarihi = ozetBeyanValues.ReferansTarihi,
                        SinirdakiAracinKimligi = ozetBeyanValues.SinirdakiAracinKimligi,
                        SinirdakiAracinTipi = ozetBeyanValues.SinirdakiAracinTipi,
                        SinirdakiAracinUlkesi = ozetBeyanValues.SinirdakiAracinUlkesi,
                        SinirdakiTasimaSekli = ozetBeyanValues.SinirdakiTasimaSekli,
                        TasarlananGuzergah = ozetBeyanValues.TasarlananGuzergah,
                        TelafiEdiciVergi = ozetBeyanValues.TelafiEdiciVergi,
                        TescilStatu = "Olusturuldu",
                        //TescilTarihi = ozetBeyanValues.TescilTarihi,
                        TeslimSekli = ozetBeyanValues.TeslimSekli,
                        TeslimSekliYeri = ozetBeyanValues.TeslimSekliYeri,
                        TicaretUlkesi = ozetBeyanValues.TicaretUlkesi,
                        ToplamFatura = ozetBeyanValues.ToplamFatura,
                        ToplamFaturaDovizi = ozetBeyanValues.ToplamFaturaDovizi,
                        ToplamNavlun = ozetBeyanValues.ToplamNavlun,
                        ToplamNavlunDovizi = ozetBeyanValues.ToplamNavlunDovizi,
                        ToplamSigorta = ozetBeyanValues.ToplamSigorta,
                        ToplamSigortaDovizi = ozetBeyanValues.ToplamSigortaDovizi,
                        ToplamYurtDisiHarcamalar = ozetBeyanValues.ToplamYurtDisiHarcamalar,
                        ToplamYurtDisiHarcamalarDovizi = ozetBeyanValues.ToplamYurtDisiHarcamalarDovizi,
                        ToplamYurtIciHarcamalar = ozetBeyanValues.ToplamYurtIciHarcamalar,
                        VarisGumrukIdaresi = ozetBeyanValues.VarisGumrukIdaresi,
                        YukBelgeleriSayisi = ozetBeyanValues.YukBelgeleriSayisi,
                        YuklemeBosaltmaYeri = ozetBeyanValues.YuklemeBosaltmaYeri,
                        RefNo = InternalNo,
                        BeyanInternalNo = InternalNo,
                        Rejim = ozetBeyanValues.Rejim,
                        OlsuturulmaTarihi=DateTime.Now
                    };


                    foreach (var x in kalemValues)
                    {
                        DbKalem kalem = new DbKalem
                        {
                            Aciklama44 = x.Aciklama44,
                            Adet = x.Adet,
                            AlgilamaBirimi1 = x.AlgilamaBirimi1,
                            AlgilamaBirimi2 = x.AlgilamaBirimi2,
                            AlgilamaBirimi3 = x.AlgilamaBirimi3,
                            AlgilamaMiktari1 = x.AlgilamaMiktari1,
                            AlgilamaMiktari2 = x.AlgilamaMiktari2,
                            AlgilamaMiktari3 = x.AlgilamaMiktari3,
                            BrutAgirlik = x.BrutAgirlik,
                            Cins = x.Cins,
                            EkKod = x.EkKod,
                            FaturaMiktari = x.FaturaMiktari,
                            FaturaMiktariDovizi = x.FaturaMiktariDovizi,
                            GirisCikisAmaci = x.GirisCikisAmaci,
                            GirisCikisAmaciAciklama = x.GirisCikisAmaciAciklama,
                            Gtip = x.Gtip,
                            IkincilIslem = x.IkincilIslem,
                            ImalatciFirmaBilgisi = x.ImalatciFirmaBilgisi,
                            ImalatciVergiNo = x.ImalatciVergiNo,
                            IstatistikiKiymet = x.IstatistikiKiymet,
                            IstatistikiMiktar = x.IstatistikiMiktar,
                            KalemIslemNiteligi = x.KalemIslemNiteligi,
                            KalemSiraNo = x.KalemSiraNo,
                            KullanilmisEsya = x.KullanilmisEsya,
                            MahraceIade = x.MahraceIade,
                            Marka = x.Marka,
                            MenseiUlke = x.MenseiUlke,
                            Miktar = x.Miktar,
                            MiktarBirimi = x.MiktarBirimi,
                            MuafiyetAciklamasi = x.MuafiyetAciklamasi,
                            Muafiyetler1 = x.Muafiyetler1,
                            Muafiyetler2 = x.Muafiyetler2,
                            Muafiyetler3 = x.Muafiyetler3,
                            Muafiyetler4 = x.Muafiyetler4,
                            Muafiyetler5 = x.Muafiyetler5,
                            NavlunMiktari = x.NavlunMiktari,
                            NavlunMiktariDovizi = x.NavlunMiktariDovizi,
                            NetAgirlik = x.NetAgirlik,
                            Numara = x.Numara,
                            Ozellik = x.Ozellik,
                           // ReferansTarihi = x.ReferansTarihi,
                            SatirNo = x.SatirNo,
                            SigortaMiktari = x.SigortaMiktari,
                            SigortaMiktariDovizi = x.SigortaMiktariDovizi,
                            SinirGecisUcreti = x.SinirGecisUcreti,
                            StmIlKodu = x.StmIlKodu,
                            TamamlayiciOlcuBirimi = x.TamamlayiciOlcuBirimi,
                           // TarifeTanimi = x.TarifeTanimi,
                            TeslimSekli = x.TeslimSekli,
                            TicariTanimi = x.TicariTanimi,
                            UluslararasiAnlasma = x.UluslararasiAnlasma,
                            YurtDisiDemuraj = x.YurtDisiDemuraj,
                            YurtDisiDemurajDovizi = x.YurtDisiDemurajDovizi,
                            YurtDisiDiger = x.YurtDisiDiger,
                            YurtDisiDigerAciklama = x.YurtDisiDigerAciklama,
                            YurtDisiDigerDovizi = x.YurtDisiDigerDovizi,
                            YurtDisiFaiz = x.YurtDisiFaiz,
                            YurtDisiFaizDovizi = x.YurtDisiFaizDovizi,
                            YurtDisiKomisyon = x.YurtDisiKomisyon,
                            YurtDisiKomisyonDovizi = x.YurtDisiKomisyonDovizi,
                            YurtDisiRoyalti = x.YurtDisiRoyalti,
                            YurtDisiRoyaltiDovizi = x.YurtDisiRoyaltiDovizi,
                            YurtIciBanka = x.YurtIciBanka,
                            YurtIciCevre = x.YurtIciCevre,
                            YurtIciDepolama = x.YurtIciDepolama,
                            YurtIciDiger = x.YurtIciDiger,
                            YurtIciDigerAciklama = x.YurtIciDigerAciklama,
                            YurtIciKkdf = x.YurtIciKkdf,
                            YurtIciKultur = x.YurtIciKultur,
                            YurtIciLiman = x.YurtIciLiman,
                            YurtIciTahliye = x.YurtIciTahliye,
                            BeyanInternalNo = newozetBeyanValues.BeyanInternalNo,
                            KalemInternalNo = newozetBeyanValues.BeyanInternalNo + "|" + x.KalemSiraNo

                        };

                        var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                        var tcgbacmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                        var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                        var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                        var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                       
                            foreach (var o in odemeValues)
                            {
                                DbOdemeSekli y = new DbOdemeSekli
                                {
                                    BeyanInternalNo = kalem.BeyanInternalNo,
                                    KalemInternalNo = kalem.KalemInternalNo,
                                    OdemeSekliKodu = o.OdemeSekliKodu,
                                    OdemeTutari = o.OdemeTutari,
                                    TBFID = o.TBFID

                                };
                                lstOdeme.Add(y);
                            }
                        
                        
                            foreach (var o in tamamlayiciValues)
                            {
                                DbTamamlayiciBilgi y = new DbTamamlayiciBilgi
                                {
                                    BeyanInternalNo = kalem.BeyanInternalNo,
                                    KalemInternalNo = kalem.KalemInternalNo,
                                    Bilgi = o.Bilgi,
                                    Gtip = o.Gtip,
                                    Oran = o.Oran

                                };
                                lstTamam.Add(y);
                            }
                        

                            foreach (var o in tcgbacmaValues)
                            {
                                DbBeyannameAcma y = new DbBeyannameAcma
                                {
                                    BeyanInternalNo = kalem.BeyanInternalNo,
                                    KalemInternalNo = kalem.KalemInternalNo,
                                    Aciklama = o.Aciklama,
                                    BeyannameNo = o.BeyannameNo,
                                    KalemNo = o.KalemNo,
                                    Miktar = o.Miktar

                                };
                                lstBeyannameAcma.Add(y);
                            }
                      

                            foreach (var o in tcgbacmaValues)
                            {
                                DbBeyannameAcma y = new DbBeyannameAcma
                                {
                                    BeyanInternalNo = kalem.BeyanInternalNo,
                                    KalemInternalNo = kalem.KalemInternalNo,
                                    Aciklama = o.Aciklama,
                                    BeyannameNo = o.BeyannameNo,
                                    KalemNo = o.KalemNo,
                                    Miktar = o.Miktar

                                };
                                lstBeyannameAcma.Add(y);
                            }
                       
                            foreach (var o in markaValues)
                            {
                                DbMarka y = new DbMarka
                                {
                                    BeyanInternalNo = kalem.BeyanInternalNo,
                                    KalemInternalNo = kalem.KalemInternalNo,
                                    MarkaAdi = o.MarkaAdi,
                                    MarkaKiymeti = o.MarkaKiymeti,
                                    MarkaTescilNo = o.MarkaTescilNo,
                                    MarkaTuru = o.MarkaTuru,
                                    Model = o.Model,
                                    ModelYili = o.ModelYili,
                                    MotorGucu = o.MotorGucu,
                                    MotorHacmi = o.MotorHacmi,
                                    MotorNo = o.MotorNo,
                                    MotorTipi = o.MotorTipi,
                                    ReferansNo = o.ReferansNo,
                                    Renk = o.Renk,
                                    SilindirAdet = o.SilindirAdet,
                                    Vites = o.Vites


                                };
                                lstMarka.Add(y);
                            }
                       



                        lstKalem.Add(kalem);
                    }

                    //kalemValues.ForEach(x =>
                    //{
                    //    x = new DbKalem
                    //    {
                    //        Aciklama44 = x.Aciklama44,
                    //    };
                    //    lstKalem.Add(x);
                    //});

                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    teminatValues.ForEach(x =>
                    {
                        x.BeyanInternalNo = newozetBeyanValues.BeyanInternalNo;
                    });
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    firmaValues.ForEach(x =>
                    {
                        x.BeyanInternalNo = newozetBeyanValues.BeyanInternalNo;
                    });



                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    int i = 1;
                    foreach (var o in ozetBeyanAcmaValues)
                    {
                      
                        DbOzetBeyanAcma ozet = new DbOzetBeyanAcma
                        {
                            Aciklama = o.Aciklama,
                            Ambar = o.Ambar,
                            BaskaRejim = o.BaskaRejim,
                            IslemKapsami = o.IslemKapsami,
                            OzetBeyanNo = o.OzetBeyanNo,
                            BeyanInternalNo = newozetBeyanValues.BeyanInternalNo,
                            OzetBeyanInternalNo = newozetBeyanValues.BeyanInternalNo + "|" + i.ToString()
                        };
                        i++;
                        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSenet.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == o.OzetBeyanInternalNo).ToListAsync();

                        if (ozetBeyanAcmaTasimaSenediValues.Count > 0)
                        {
                            int j = 1;
                            foreach (var t in ozetBeyanAcmaTasimaSenediValues)
                            {
                               
                                DbOzetBeyanAcmaTasimaSenet tasima = new DbOzetBeyanAcmaTasimaSenet
                                {
                                    BeyanInternalNo = ozet.BeyanInternalNo,
                                    OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                    TasimaSenediNo = t.TasimaSenediNo,
                                    TasimaSenetInternalNo = ozet.OzetBeyanInternalNo + "|" + j.ToString()

                                };
                                j++;
                                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == o.OzetBeyanInternalNo && v.TasimaSenetInternalNo == t.TasimaSenetInternalNo).ToListAsync();
                                if (ozetBeyanAcmaTasimaSatirValues.Count > 0)
                                {
                                    foreach (var s in ozetBeyanAcmaTasimaSatirValues)
                                    {
                                        DbOzetBeyanAcmaTasimaSatir satir = new DbOzetBeyanAcmaTasimaSatir
                                        {
                                            AmbarKodu = s.AmbarKodu,
                                            BeyanInternalNo = ozet.BeyanInternalNo,
                                            OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                            TasimaSenetInternalNo = tasima.TasimaSenetInternalNo,
                                            TasimaSatirNo = s.TasimaSatirNo,
                                            Miktar = s.Miktar
                                        };
                                        lstTasimaSatiri.Add(satir);
                                    }
                                }

                                lstTasimaSenedi.Add(tasima);
                            }
                        }

                        lstOzetBeyan.Add(ozet);
                    }


                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var k in kiymetValues)
                    {
                        DbKiymetBildirim kiymet = new DbKiymetBildirim
                        {
                            BeyanInternalNo=newozetBeyanValues.BeyanInternalNo,
                            KiymetInternalNo=newozetBeyanValues.BeyanInternalNo,
                            AliciSatici = k.AliciSatici,
                            AliciSaticiAyrintilar =k.AliciSaticiAyrintilar,
                            Edim=k.Edim,
                            Emsal=k.Emsal,
                            FaturaTarihiSayisi=k.FaturaTarihiSayisi,
                            GumrukIdaresiKarari=k.GumrukIdaresiKarari,
                            Kisitlamalar=k.Kisitlamalar,
                            KisitlamalarAyrintilar=k.KisitlamalarAyrintilar,
                            Munasebet=k.Munasebet,
                            Royalti=k.Royalti,
                            RoyaltiKosullar=k.RoyaltiKosullar,
                            SaticiyaIntikal=k.SaticiyaIntikal,
                            SaticiyaIntikalKosullar=k.SaticiyaIntikalKosullar,
                            SehirYer=k.SehirYer,
                            SozlesmeTarihiSayisi=k.SozlesmeTarihiSayisi,
                            Taahhutname = k.Taahhutname,
                            TeslimSekli=k.TeslimSekli
                          
                        };
                        var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KiymetInternalNo == k.KiymetInternalNo).ToListAsync();
                        foreach (var kk in kiymetKalemValues)
                        {
                            DbKiymetBildirimKalem kkalem = new DbKiymetBildirimKalem
                            {
                                BeyanInternalNo = newozetBeyanValues.BeyanInternalNo,
                                KiymetInternalNo = kiymet.BeyanInternalNo,
                                BeyannameKalemNo=kk.BeyannameKalemNo,
                                DigerOdemeler=kk.DigerOdemeler,
                                DigerOdemelerNiteligi=kk.DigerOdemelerNiteligi,
                                DolayliIntikal=kk.DolayliIntikal,
                                DolayliOdeme=kk.DolayliOdeme,
                                GirisSonrasiNakliye=kk.GirisSonrasiNakliye,
                                IthalaKatilanMalzeme=kk.IthalaKatilanMalzeme,
                                IthalaUretimAraclar=kk.IthalaUretimAraclar,
                                IthalaUretimTuketimMalzemesi=kk.IthalaUretimTuketimMalzemesi,
                                KapAmbalajBedeli=kk.KapAmbalajBedeli,
                                KiymetKalemNo=kk.KiymetKalemNo,
                                Komisyon=kk.Komisyon,
                                Nakliye=kk.Nakliye,
                                PlanTaslak=kk.PlanTaslak,
                                RoyaltiLisans=kk.RoyaltiLisans,
                                Sigorta=kk.Sigorta,
                                TeknikYardim=kk.TeknikYardim,
                                Tellaliye=kk.Tellaliye,
                                VergiHarcFon=kk.VergiHarcFon
                            };
                            lstKiymetKalem.Add(kkalem);
                        }

                        lstKiymet.Add(kiymet);
                    }

               
                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {

                            _beyannameContext.Entry(newozetBeyanValues).State = EntityState.Added;
                            foreach (var item in lstKalem)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstOdeme)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstTamam)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstBeyannameAcma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstMarka)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstKonteyner)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in teminatValues)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in firmaValues)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstOzetBeyan)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstTasimaSenedi)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstTasimaSatiri)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstKiymet)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstKiymetKalem)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            await _beyannameContext.SaveChangesAsync();


                            Islem _islem = new Islem();
                            _islem.Kullanici = newozetBeyanValues.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "DetayliBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = newozetBeyanValues.RefNo;
                            _islem.BeyanInternalNo = newozetBeyanValues.BeyanInternalNo;
                            _islem.IslemInternalNo = newozetBeyanValues.BeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();
                            lstbilgi.Add(new Bilgi { IslemTipi = "Beyanname Kopyalama", ReferansNo = newozetBeyanValues.BeyanInternalNo, Sonuc = "Kopyalama Başarılı", SonucVeriler = null, GUID = null });
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