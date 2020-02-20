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
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo);
                if (islemValues != null)
                {
                    var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                    var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var tcgbacmaValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbTasimaSenedi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    var internalrefid = _beyannameContext.GetRefIdNextSequenceValue(beyanValues.Rejim);
                    string InternalNo = beyanValues.Kullanici + "DBG" + internalrefid.ToString().PadLeft(6, '0');
                    var newbeyanValues = new DbBeyan
                    {
                        Aciklamalar = beyanValues.Aciklamalar,
                        AliciSaticiIliskisi = beyanValues.AliciSaticiIliskisi,
                        AliciVergiNo = beyanValues.AliciVergiNo,
                        AntrepoKodu = beyanValues.AntrepoKodu,
                        AsilSorumluVergiNo = beyanValues.AsilSorumluVergiNo,
                        BankaKodu = beyanValues.BankaKodu,
                        BasitlestirilmisUsul = beyanValues.BasitlestirilmisUsul,
                        BeyannameNo = beyanValues.BeyannameNo,
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
                        ReferansNo = beyanValues.ReferansNo,
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
                        RefId = beyanValues.Kullanici + "|" + beyanValues.Rejim + "|" + internalrefid.ToString().PadLeft(6, '0'),
                        BeyanInternalNo = InternalNo,
                        Rejim = beyanValues.Rejim,
                    };

                    List<DbKalem> lstKalem = new List<DbKalem>();
                    List<DbOdemeSekli> lstOdeme = new List<DbOdemeSekli>();
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


                        var odemeValues = _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == x.KalemInternalNo).ToListAsync();
                        if (odemeValues.Result.Count > 0)
                        {
                            foreach (var o in odemeValues.Result)
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
                    teminatValues.ForEach(x =>
                    {
                        x.BeyanInternalNo = newbeyanValues.BeyanInternalNo;
                    });

                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {

                            _beyannameContext.Entry(newbeyanValues).State = EntityState.Added;
                            foreach (var item in lstKalem)
                               _beyannameContext.Entry(item).State = EntityState.Added;
                            
                            foreach (var item in lstOdeme)
                               _beyannameContext.Entry(item).State = EntityState.Added;
                         
                            foreach (var item in teminatValues)
                                _beyannameContext.Entry(item).State = EntityState.Added;

                            await _beyannameContext.SaveChangesAsync();


                            Islem _islem = new Islem();
                            _islem.Kullanici = newbeyanValues.Kullanici;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "DetayliBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefId = newbeyanValues.RefId;
                            _islem.BeyanInternalNo = newbeyanValues.BeyanInternalNo;
                            _islem.IslemInternalNo = newbeyanValues.BeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();

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