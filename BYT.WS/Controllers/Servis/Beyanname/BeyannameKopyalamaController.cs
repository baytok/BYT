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
                    var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    var internalrefid = _beyannameContext.GetRefIdNextSequenceValue(beyanValues.Rejim);
                    string InternalNo = beyanValues.Kullanici + "DB" + internalrefid.ToString().PadLeft(6, '0');

                    List<DbKalem> lstKalem = new List<DbKalem>();
                    List<DbOdemeSekli> lstOdeme = new List<DbOdemeSekli>();
                    List<DbTamamlayiciBilgi> lstTamam = new List<DbTamamlayiciBilgi>();
                    List<DbKonteyner> lstKonteyner = new List<DbKonteyner>();
                    List<DbMarka> lstMarka = new List<DbMarka>();
                    List<DbBeyannameAcma> lstBeyannameAcma = new List<DbBeyannameAcma>();
                    List<DbOzetbeyanAcma> lstOzetBeyan = new List<DbOzetbeyanAcma>();
                    List<DbTasimaSenedi> lstTasimaSenedi = new List<DbTasimaSenedi>();
                    List<DbTasimaSatir> lstTasimaSatiri = new List<DbTasimaSatir>();
                    List<DbKiymetBildirim> lstKiymet = new List<DbKiymetBildirim>();
                    List<DbKiymetBildirimKalem> lstKiymetKalem = new List<DbKiymetBildirimKalem>();
                    var newbeyanValues = new DbBeyan
                    {
                        Aciklamalar = beyanValues.Aciklamalar,
                        AliciSaticiIliskisi = beyanValues.AliciSaticiIliskisi,
                        AliciVergiNo = beyanValues.AliciVergiNo,
                        AntrepoKodu = beyanValues.AntrepoKodu,
                        AsilSorumluVergiNo = beyanValues.AsilSorumluVergiNo,
                        BankaKodu = beyanValues.BankaKodu,
                        BasitlestirilmisUsul = beyanValues.BasitlestirilmisUsul,
                        //BeyannameNo = beyanValues.BeyannameNo,
                        BeyanSahibiVergiNo = beyanValues.BeyanSahibiVergiNo,
                        BirlikKayitNumarasi = beyanValues.BirlikKayitNumarasi,
                        BirlikKriptoNumarasi = beyanValues.BirlikKriptoNumarasi,
                        CikistakiAracinKimligi = beyanValues.CikistakiAracinKimligi,
                        CikistakiAracinTipi = beyanValues.CikistakiAracinTipi,
                        CikistakiAracinUlkesi = beyanValues.CikistakiAracinUlkesi,
                        CikisUlkesi = beyanValues.CikisUlkesi,
                        EsyaninBulunduguYer = beyanValues.EsyaninBulunduguYer,
                        GidecegiSevkUlkesi = beyanValues.GidecegiSevkUlkesi,
                        GidecegiUlke = beyanValues.GidecegiUlke,
                        GirisGumrukIdaresi = beyanValues.GirisGumrukIdaresi,
                        GondericiVergiNo = beyanValues.GondericiVergiNo,
                        Gumruk = beyanValues.Gumruk,
                        IsleminNiteligi = beyanValues.IsleminNiteligi,
                        KapAdedi = beyanValues.KapAdedi,
                        Konteyner = beyanValues.Konteyner,
                        Kullanici = beyanValues.Kullanici,
                        LimanKodu = beyanValues.LimanKodu,
                        Mail1 = beyanValues.Mail1,
                        Mail2 = beyanValues.Mail2,
                        Mail3 = beyanValues.Mail3,
                        Mobil1 = beyanValues.Mobil1,
                        Mobil2 = beyanValues.Mobil2,
                        MusavirVergiNo = beyanValues.MusavirVergiNo,
                        OdemeAraci = beyanValues.OdemeAraci,
                        MusavirReferansNo = beyanValues.MusavirReferansNo,
                        ReferansTarihi = beyanValues.ReferansTarihi,
                        SinirdakiAracinKimligi = beyanValues.SinirdakiAracinKimligi,
                        SinirdakiAracinTipi = beyanValues.SinirdakiAracinTipi,
                        SinirdakiAracinUlkesi = beyanValues.SinirdakiAracinUlkesi,
                        SinirdakiTasimaSekli = beyanValues.SinirdakiTasimaSekli,
                        TasarlananGuzergah = beyanValues.TasarlananGuzergah,
                        TelafiEdiciVergi = beyanValues.TelafiEdiciVergi,
                        TescilStatu = "OLUSTURULDU",
                        //TescilTarihi = beyanValues.TescilTarihi,
                        TeslimSekli = beyanValues.TeslimSekli,
                        TeslimSekliYeri = beyanValues.TeslimSekliYeri,
                        TicaretUlkesi = beyanValues.TicaretUlkesi,
                        ToplamFatura = beyanValues.ToplamFatura,
                        ToplamFaturaDovizi = beyanValues.ToplamFaturaDovizi,
                        ToplamNavlun = beyanValues.ToplamNavlun,
                        ToplamNavlunDovizi = beyanValues.ToplamNavlunDovizi,
                        ToplamSigorta = beyanValues.ToplamSigorta,
                        ToplamSigortaDovizi = beyanValues.ToplamSigortaDovizi,
                        ToplamYurtDisiHarcamalar = beyanValues.ToplamYurtDisiHarcamalar,
                        ToplamYurtDisiHarcamalarDovizi = beyanValues.ToplamYurtDisiHarcamalarDovizi,
                        ToplamYurtIciHarcamalar = beyanValues.ToplamYurtIciHarcamalar,
                        VarisGumrukIdaresi = beyanValues.VarisGumrukIdaresi,
                        YukBelgeleriSayisi = beyanValues.YukBelgeleriSayisi,
                        YuklemeBosaltmaYeri = beyanValues.YuklemeBosaltmaYeri,
                        RefNo = beyanValues.Kullanici + "|" + beyanValues.Rejim + "|" + internalrefid.ToString().PadLeft(6, '0'),
                        BeyanInternalNo = InternalNo,
                        Rejim = beyanValues.Rejim,
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
                            ReferansTarihi = x.ReferansTarihi,
                            SatirNo = x.SatirNo,
                            SigortaMiktari = x.SigortaMiktari,
                            SigortaMiktariDovizi = x.SigortaMiktariDovizi,
                            SinirGecisUcreti = x.SinirGecisUcreti,
                            StmIlKodu = x.StmIlKodu,
                            TamamlayiciOlcuBirimi = x.TamamlayiciOlcuBirimi,
                            TarifeTanimi = x.TarifeTanimi,
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
                            BeyanInternalNo = newbeyanValues.BeyanInternalNo,
                            KalemInternalNo = newbeyanValues.BeyanInternalNo + "|" + x.KalemSiraNo

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
                        x.BeyanInternalNo = newbeyanValues.BeyanInternalNo;
                    });
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    firmaValues.ForEach(x =>
                    {
                        x.BeyanInternalNo = newbeyanValues.BeyanInternalNo;
                    });



                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var o in ozetBeyanAcmaValues)
                    {
                        DbOzetbeyanAcma ozet = new DbOzetbeyanAcma
                        {
                            Aciklama = o.Aciklama,
                            Ambar = o.Ambar,
                            BaskaRejim = o.BaskaRejim,
                            IslemKapsami = o.IslemKapsami,
                            OzetbeyanNo = o.OzetbeyanNo,
                            BeyanInternalNo = newbeyanValues.BeyanInternalNo,
                            OzetBeyanInternalNo = newbeyanValues.BeyanInternalNo + "|" + o.OzetbeyanNo
                        };

                        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbTasimaSenedi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == o.OzetBeyanInternalNo).ToListAsync();

                        if (ozetBeyanAcmaTasimaSenediValues.Count > 0)
                        {
                            foreach (var t in ozetBeyanAcmaTasimaSenediValues)
                            {
                                DbTasimaSenedi tasima = new DbTasimaSenedi
                                {
                                    BeyanInternalNo = ozet.BeyanInternalNo,
                                    OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                    TasimaSenediNo = t.TasimaSenediNo,
                                    TasimaInternalNo = ozet.OzetBeyanInternalNo + "|" + t.TasimaSenediNo

                                };

                                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == o.OzetBeyanInternalNo && v.TasimaInternalNo == t.TasimaInternalNo).ToListAsync();
                                if (ozetBeyanAcmaTasimaSatirValues.Count > 0)
                                {
                                    foreach (var s in ozetBeyanAcmaTasimaSatirValues)
                                    {
                                        DbTasimaSatir satir = new DbTasimaSatir
                                        {
                                            AmbarKodu = s.AmbarKodu,
                                            BeyanInternalNo = ozet.BeyanInternalNo,
                                            OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                            TasimaInternalNo = tasima.TasimaInternalNo,
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
                            BeyanInternalNo=newbeyanValues.BeyanInternalNo,
                            KiymetInternalNo=newbeyanValues.BeyanInternalNo,
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
                            Taahutname=k.Taahutname,
                            TeslimSekli=k.TeslimSekli
                          
                        };
                        var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KiymetInternalNo == k.KiymetInternalNo).ToListAsync();
                        foreach (var kk in kiymetKalemValues)
                        {
                            DbKiymetBildirimKalem kkalem = new DbKiymetBildirimKalem
                            {
                                BeyanInternalNo = newbeyanValues.BeyanInternalNo,
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

                            _beyannameContext.Entry(newbeyanValues).State = EntityState.Added;
                            foreach (var item in lstKalem)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstOdeme)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstTamam)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstBeyannameAcma)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstMarka)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstKonteyner)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in teminatValues)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in firmaValues)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstOzetBeyan)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstTasimaSenedi)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstTasimaSatiri)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstKiymet)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            foreach (var item in lstKiymetKalem)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            await _beyannameContext.SaveChangesAsync();


                            Islem _islem = new Islem();
                            _islem.Kullanici = newbeyanValues.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "DetayliBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = newbeyanValues.RefNo;
                            _islem.BeyanInternalNo = newbeyanValues.BeyanInternalNo;
                            _islem.IslemInternalNo = newbeyanValues.BeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();
                            lstbilgi.Add(new Bilgi { IslemTipi = "Beyanname Kopyalama", ReferansNo = newbeyanValues.BeyanInternalNo, Sonuc = "Kopyalama Başarılı", SonucVeriler = null, GUID = null });
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