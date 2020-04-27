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

namespace BYT.WS.Controllers.Servis.Beyanname
{
   // [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class KontrolGonderimController : ControllerBase
    {
        string[] EX = {"1000","1021","1023","1040","1042","1072","1091","2100","2123","2141","2151","2152","2153","2172","2191","2300","2340","2341",
            "2342","2351","2352","2353","2600","3141","3151","3152","3153","3158","3171"};
        string[] IM = {"4000","4010","4051","4053","4058","4071","4072","4091","4100","4121","4123","4171","4191","4200","4210","4251","4253","4258",
            "4271","4291","5100","5121","5123","5141","5171","5191","5200","5221","5223","5271","5291","5300","5321","5323","5341","5351","5352","5353",
            "5358","5371","5391","5800","6121","6123","6321","6323","6326","6521","6523","6771","9100","9171"};
        string[] AN = {"7100","7121","7123","7141","7151","7153","7158","7171","7191","7200","7241","7252","7272","7300"};
        string[] DG = { "8100", "8200"};
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }
        public KontrolGonderimController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;
            _sonucContext = sonucContext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }


        [Route("api/BYT/Servis/Beyanname/[controller]/{IslemInternalNo}/{Kullanici}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}")]
        public async Task<ServisDurum> GetKontrol(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var beyanValues = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == islemValues.BeyanInternalNo);
                var kalemValues = await _beyannameContext.DbKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();


                KontrolHizmeti.Gumruk_Biztalk_EImzaTescil_Kontrol_KontrolTalepPortSoapClient Kontrol = ServiceHelper.GetKontrolWsClient(_servisCredential.username, _servisCredential.password);

                KontrolHizmeti.Gelen gelen = new KontrolHizmeti.Gelen();


                #region Genel
                KontrolHizmeti.BeyannameBilgi _beyan = new KontrolHizmeti.BeyannameBilgi();


                _beyan.Aciklamalar = beyanValues.Aciklamalar;
                _beyan.Alici_satici_iliskisi = beyanValues.AliciSaticiIliskisi;
                _beyan.Alici_vergi_no = beyanValues.AliciVergiNo;
                _beyan.Antrepo_kodu = beyanValues.AntrepoKodu;
                _beyan.Asil_sorumlu_vergi_no = beyanValues.AsilSorumluVergiNo;
                _beyan.Basitlestirilmis_usul = beyanValues.BasitlestirilmisUsul;
                _beyan.Banka_kodu = beyanValues.BankaKodu;
                _beyan.Beyan_sahibi_vergi_no = beyanValues.BeyanSahibiVergiNo;
                _beyan.Birlik_kripto_numarasi = beyanValues.BirlikKriptoNumarasi;
                _beyan.Birlik_kayit_numarasi = beyanValues.BirlikKayitNumarasi;
                _beyan.Cikis_ulkesi = beyanValues.CikisUlkesi;
                _beyan.Cikistaki_aracin_kimligi = beyanValues.CikistakiAracinKimligi;
                _beyan.Cikistaki_aracin_tipi = beyanValues.CikistakiAracinTipi;
                _beyan.Cikistaki_aracin_ulkesi = beyanValues.CikistakiAracinUlkesi;
                _beyan.Esyanin_bulundugu_yer = beyanValues.EsyaninBulunduguYer;
                _beyan.FazlaMesaiID = "";
                _beyan.Gidecegi_sevk_ulkesi = beyanValues.GidecegiSevkUlkesi;
                _beyan.Gidecegi_ulke = beyanValues.GidecegiUlke;
                _beyan.Giris_gumruk_idaresi = beyanValues.GirisGumrukIdaresi;
                _beyan.Gonderici_vergi_no = beyanValues.GondericiVergiNo;
                _beyan.GUMRUK = beyanValues.Gumruk;
                _beyan.Islemin_niteligi = beyanValues.IsleminNiteligi;
                _beyan.Kap_adedi = beyanValues.KapAdedi != null ? beyanValues.KapAdedi : 0;
                _beyan.Konteyner = beyanValues.Konteyner;
                _beyan.Kullanici_kodu = beyanValues.Kullanici;
                _beyan.LimanKodu = beyanValues.LimanKodu;
                _beyan.mail1 = beyanValues.Mail1;
                _beyan.mail2 = beyanValues.Mail2;
                _beyan.mail3 = beyanValues.Mail3;
                _beyan.mobil1 = beyanValues.Mobil1;
                _beyan.mobil2 = beyanValues.Mobil2;
                _beyan.Musavir_referansi = beyanValues.MusavirReferansNo;
                _beyan.Musavir_vergi_no = beyanValues.MusavirVergiNo;
                _beyan.Odeme = "PESIN";
                //  _beyan.Odeme_araci = beyanValues.OdemeAraci;
                _beyan.Referans_no = beyanValues.RefNo;
                _beyan.Referans_tarihi = beyanValues.ReferansTarihi;
                _beyan.Rejim = beyanValues.Rejim;
                _beyan.Sinirdaki_aracin_kimligi = beyanValues.SinirdakiAracinKimligi;
                _beyan.Sinirdaki_aracin_tipi = beyanValues.SinirdakiAracinTipi;
                _beyan.Sinirdaki_aracin_ulkesi = beyanValues.SinirdakiAracinUlkesi;
                _beyan.Sinirdaki_tasima_sekli = beyanValues.SinirdakiTasimaSekli;
                _beyan.Tasarlanan_guzergah = beyanValues.TasarlananGuzergah;
                _beyan.Telafi_edici_vergi = beyanValues.TelafiEdiciVergi != null ? beyanValues.TelafiEdiciVergi : 0;
                _beyan.Teslim_sekli = beyanValues.TeslimSekli;
                _beyan.Teslim_yeri = beyanValues.TeslimSekliYeri;
                _beyan.Ticaret_ulkesi = beyanValues.TicaretUlkesi;
                _beyan.Toplam_fatura = beyanValues.ToplamFatura != null ? beyanValues.ToplamFatura : 0;
                _beyan.Toplam_fatura_dovizi = beyanValues.ToplamFaturaDovizi;
                _beyan.Toplam_navlun = beyanValues.ToplamNavlun != null ? beyanValues.ToplamNavlun : 0;
                _beyan.Toplan_navlun_dovizi = beyanValues.ToplamNavlunDovizi;
                _beyan.Toplam_sigorta = beyanValues.ToplamSigorta != null ? beyanValues.ToplamSigorta : 0;
                _beyan.Toplam_sigorta_dovizi = beyanValues.ToplamSigortaDovizi;
                _beyan.Toplam_yurt_disi_harcamalar = beyanValues.ToplamYurtDisiHarcamalar != null ? beyanValues.ToplamYurtDisiHarcamalar : 0;
                _beyan.Toplam_yurt_disi_harcamalarin_dovizi = beyanValues.ToplamYurtDisiHarcamalarDovizi;
                _beyan.Toplam_yurt_ici_harcamalar = beyanValues.ToplamYurtIciHarcamalar != null ? beyanValues.ToplamYurtIciHarcamalar : 0;
                _beyan.Varis_gumruk_idaresi = beyanValues.VarisGumrukIdaresi;
                _beyan.Yuk_belgeleri_sayisi = beyanValues.YukBelgeleriSayisi != null ? beyanValues.YukBelgeleriSayisi : 0;
                _beyan.Yukleme_bosaltma_yeri = beyanValues.YuklemeBosaltmaYeri;

                #endregion

                #region Kalem

                if (kalemValues != null && kalemValues.Count > 0)
                {
                    KontrolHizmeti.kalem _kalem;
                    List<KontrolHizmeti.kalem> _kalemList = new List<KontrolHizmeti.kalem>();
                    foreach (var item in kalemValues)
                    {
                        _kalem = new KontrolHizmeti.kalem();

                        _kalem.Aciklama_44 = item.Aciklama44;
                        _kalem.Adedi = item.Adet != null ? item.Adet : 0;
                        _kalem.Algilama_birimi_1 = item.AlgilamaBirimi1;
                        _kalem.Algilama_birimi_2 = item.AlgilamaBirimi2;
                        _kalem.Algilama_birimi_3 = item.AlgilamaBirimi3;
                        _kalem.Algilama_miktari_1 = item.AlgilamaMiktari1 != null ? item.AlgilamaMiktari1 : 0;
                        _kalem.Algilama_miktari_2 = item.AlgilamaMiktari2 != null ? item.AlgilamaMiktari2 : 0;
                        _kalem.Algilama_miktari_3 = item.AlgilamaMiktari3 != null ? item.AlgilamaMiktari3 : 0;
                        _kalem.Brut_agirlik = item.BrutAgirlik != null ? item.BrutAgirlik : 0;
                        _kalem.Cinsi = item.Cins;
                        _kalem.Ek_kod = item.EkKod;
                        _kalem.Giris_Cikis_Amaci = item.GirisCikisAmaci;
                        _kalem.Giris_Cikis_Amaci_Aciklama = item.GirisCikisAmaciAciklama;
                        _kalem.Gtip = item.Gtip;
                        _kalem.Fatura_miktari = item.FaturaMiktari != null ? item.FaturaMiktari : 0;
                        _kalem.Fatura_miktarinin_dovizi = item.FaturaMiktariDovizi;
                        _kalem.Ikincil_islem = item.IkincilIslem;
                        _kalem.Imalatci_firma_bilgisi = item.ImalatciFirmaBilgisi;
                        _kalem.Imalatci_Vergino = item.ImalatciVergiNo;
                        _kalem.Istatistiki_kiymet = item.IstatistikiKiymet != null ? item.IstatistikiKiymet : 0;
                        _kalem.Istatistiki_miktar = item.IstatistikiMiktar != null ? item.IstatistikiMiktar : 0;
                        _kalem.Kalem_Islem_Niteligi = item.KalemIslemNiteligi;
                        _kalem.Kalem_sira_no = item.KalemSiraNo;
                        _kalem.Kullanilmis_esya = item.KullanilmisEsya;
                        _kalem.Marka = item.Marka;
                        _kalem.Mahrece_iade = item.MahraceIade;
                        _kalem.Mensei_ulke = item.MenseiUlke;
                        _kalem.Miktar = item.Miktar != null ? item.Miktar : 0;
                        _kalem.Miktar_birimi = item.MiktarBirimi;
                        _kalem.Muafiyetler_1 = item.Muafiyetler1;
                        _kalem.Muafiyetler_2 = item.Muafiyetler2;
                        _kalem.Muafiyetler_3 = item.Muafiyetler3;
                        _kalem.Muafiyetler_4 = item.Muafiyetler4;
                        _kalem.Muafiyetler_5 = item.Muafiyetler5;
                        _kalem.Muafiyet_Aciklamasi = item.MuafiyetAciklamasi;
                        _kalem.Navlun_miktari = item.NavlunMiktari != null ? item.NavlunMiktari : 0;
                        _kalem.Navlun_miktarinin_dovizi = item.NavlunMiktariDovizi;
                        _kalem.Net_agirlik = item.NetAgirlik != null ? item.NetAgirlik : 0;
                        _kalem.Numara = item.Numara;
                        _kalem.Ozellik = item.Ozellik;
                        _kalem.Referans_Tarihi = "";
                        _kalem.Satir_no = item.SatirNo;
                        _kalem.Sigorta_miktari = item.SigortaMiktari != null ? item.NavlunMiktari : 0;
                        _kalem.Sigorta_miktarinin_dovizi = item.SigortaMiktariDovizi;
                        _kalem.Sinir_gecis_ucreti = item.SinirGecisUcreti != null ? item.SinirGecisUcreti : 0;
                        _kalem.STM_IlKodu = item.StmIlKodu;
                        _kalem.Tamamlayici_olcu_birimi = item.TamamlayiciOlcuBirimi;
                        _kalem.Tarifedeki_tanimi = "";
                        _kalem.Ticari_tanimi = item.TicariTanimi;
                        _kalem.Teslim_sekli = item.TeslimSekli;
                        _kalem.Uluslararasi_anlasma = item.UluslararasiAnlasma;
                        _kalem.YurtDisi_Diger = item.YurtDisiDiger != null ? item.YurtDisiDiger : 0;
                        _kalem.YurtDisi_Diger_Aciklama = item.YurtDisiDigerAciklama;
                        _kalem.YurtDisi_Diger_Dovizi = item.YurtDisiDigerDovizi;
                        _kalem.YurtDisi_Demuraj = item.YurtDisiDemuraj != null ? item.YurtDisiDemuraj : 0;
                        _kalem.YurtDisi_Demuraj_Dovizi = item.YurtDisiDemurajDovizi;
                        _kalem.YurtDisi_Faiz = item.YurtDisiFaiz != null ? item.YurtDisiFaiz : 0;
                        _kalem.YurtDisi_Faiz_Dovizi = item.YurtDisiFaizDovizi;
                        _kalem.YurtDisi_Komisyon = item.YurtDisiKomisyon != null ? item.YurtDisiKomisyon : 0;
                        _kalem.YurtDisi_Komisyon_Dovizi = item.YurtDisiKomisyonDovizi;
                        _kalem.YurtDisi_Royalti = item.YurtDisiRoyalti != null ? item.YurtDisiRoyalti : 0;
                        _kalem.YurtDisi_Royalti_Dovizi = item.YurtDisiRoyaltiDovizi;
                        _kalem.Yurtici_Banka = item.YurtIciBanka != null ? item.YurtIciBanka : 0;
                        _kalem.Yurtici_Cevre = item.YurtIciCevre != null ? item.YurtIciCevre : 0;
                        _kalem.Yurtici_Diger = item.YurtIciDiger != null ? item.YurtIciDiger : 0;
                        _kalem.Yurtici_Diger_Aciklama = item.YurtIciDigerAciklama;
                        _kalem.Yurtici_Depolama = item.YurtIciDepolama != null ? item.YurtIciDepolama : 0;
                        _kalem.Yurtici_Kkdf = item.YurtIciKkdf != null ? item.YurtIciKkdf : 0;
                        _kalem.Yurtici_Kultur = item.YurtIciKultur != null ? item.YurtIciKultur : 0;
                        _kalem.Yurtici_Liman = item.YurtIciLiman != null ? item.YurtIciLiman : 0;
                        _kalem.Yurtici_Tahliye = item.YurtIciTahliye != null ? item.YurtIciTahliye : 0;


                        var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                        if (tamamlayiciValues != null && tamamlayiciValues.Count > 0)
                        {
                            List<KontrolHizmeti.tamamlayici> _tamamlayiciList = new List<KontrolHizmeti.tamamlayici>();
                            KontrolHizmeti.tamamlayici _tamamlayici;
                            foreach (var tamamb in tamamlayiciValues)
                            {
                                _tamamlayici = new KontrolHizmeti.tamamlayici();
                                _tamamlayici.Tamamlayici_bilgi = tamamb.Bilgi;
                                _tamamlayici.Tamamlayici_bilgi_orani = tamamb.Oran;


                                _tamamlayiciList.Add(_tamamlayici);
                            }

                            _kalem.tamamlayici_bilgi = _tamamlayiciList.ToArray();
                        }
                        else _kalem.tamamlayici_bilgi = new KontrolHizmeti.tamamlayici[0];

                        var tcgbacmaiValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                        if (tcgbacmaiValues != null && tcgbacmaiValues.Count > 0)
                        {
                            List<KontrolHizmeti.tcgbacmakapatma> _acmaList = new List<KontrolHizmeti.tcgbacmakapatma>();
                            KontrolHizmeti.tcgbacmakapatma _acma;
                            foreach (var acma in tcgbacmaiValues)
                            {
                                _acma = new KontrolHizmeti.tcgbacmakapatma();
                                _acma.Kapatilan_beyanname_no = acma.BeyannameNo;
                                _acma.Kapatilan_kalem_no = acma.KalemNo;
                                _acma.Kapatilan_miktar = acma.Miktar;
                                _acma.Aciklama = acma.Aciklama;

                                _acmaList.Add(_acma);
                            }

                            _kalem.tcgbacmakapatma_bilgi = _acmaList.ToArray();
                        }
                        else _kalem.tcgbacmakapatma_bilgi = new KontrolHizmeti.tcgbacmakapatma[0];

                        var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                        if (markaValues != null && markaValues.Count > 0)
                        {
                            List<KontrolHizmeti.Marka> _markaList = new List<KontrolHizmeti.Marka>();
                            KontrolHizmeti.Marka _marka;
                            foreach (var marka in markaValues)
                            {
                                _marka = new KontrolHizmeti.Marka();
                                _marka.Marka_Adi = marka.MarkaAdi;
                                _marka.Marka_Kiymeti = marka.MarkaKiymeti != null ? marka.MarkaKiymeti : 0;
                                _marka.Marka_Tescil_No = marka.MarkaTescilNo;
                                _marka.Marka_Turu = marka.MarkaTuru;
                                _marka.Model = marka.Model;
                                _marka.Model_Yili = marka.ModelYili;
                                _marka.MotorGucu = marka.MotorGucu;
                                _marka.MotorNo = marka.MotorNo;
                                _marka.Motor_hacmi = marka.MotorHacmi;
                                _marka.MotorTipi = marka.MotorTipi;
                                _marka.Referans_No = marka.ReferansNo;
                                _marka.Renk = marka.Renk;
                                _marka.Silindir_adedi = marka.SilindirAdet != null ? marka.SilindirAdet : 0;
                                _marka.Vites = marka.Vites;
                                _markaList.Add(_marka);
                            }
                            _kalem.marka_model_bilgi = _markaList.ToArray();
                        }
                        else _kalem.marka_model_bilgi = new KontrolHizmeti.Marka[0];


                        var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                        if (konteynerValues != null && konteynerValues.Count > 0)
                        {
                            List<KontrolHizmeti.Konteyner> _konteynerList = new List<KontrolHizmeti.Konteyner>();
                            KontrolHizmeti.Konteyner _konteyner;
                            foreach (var konteyner in konteynerValues)
                            {
                                _konteyner = new KontrolHizmeti.Konteyner();
                                _konteyner.Ulke_Kodu = konteyner.UlkeKodu;
                                _konteyner.Konteyner_No = konteyner.KonteynerNo;


                                _konteynerList.Add(_konteyner);
                            }

                            _kalem.konteyner_Bilgi = _konteynerList.ToArray();
                        }
                        else _kalem.konteyner_Bilgi = new KontrolHizmeti.Konteyner[0];


                        var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                        if (odemeValues != null && odemeValues.Count > 0)
                        {
                            List<KontrolHizmeti.OdemeSekli> _odemeList = new List<KontrolHizmeti.OdemeSekli>();
                            KontrolHizmeti.OdemeSekli _odeme;
                            foreach (var odeme in odemeValues)
                            {
                                _odeme = new KontrolHizmeti.OdemeSekli();
                                _odeme.OdemeSekliKodu = odeme.OdemeSekliKodu;
                                _odeme.OdemeTutari = odeme.OdemeTutari != null ? odeme.OdemeTutari : 0;
                                _odeme.TBFID = odeme.TBFID;

                                _odemeList.Add(_odeme);
                            }

                            _kalem.OdemeSekilleri = _odemeList.ToArray();
                        }
                        else _kalem.OdemeSekilleri = new KontrolHizmeti.OdemeSekli[0];


                        _kalemList.Add(_kalem);
                    }
                    _beyan.Kalemler = _kalemList.ToArray();
                }
                else _beyan.Kalemler = new KontrolHizmeti.kalem[0];


                #endregion

                #region OZBY

                var ozetBeyanAcmaValues = await _beyannameContext.DbOzetbeyanAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                if (ozetBeyanAcmaValues != null && ozetBeyanAcmaValues.Count > 0)
                {
                    List<KontrolHizmeti.Ozetbeyan> _ozetBeyanList = new List<KontrolHizmeti.Ozetbeyan>();
                    KontrolHizmeti.Ozetbeyan _ozetBeyan;
                    foreach (var ozetbeyan in ozetBeyanAcmaValues)
                    {
                        _ozetBeyan = new KontrolHizmeti.Ozetbeyan();
                        _ozetBeyan.Ozetbeyan_no = ozetbeyan.OzetBeyanNo;
                        _ozetBeyan.Ozetbeyan_islem_kapsami = ozetbeyan.IslemKapsami;
                        _ozetBeyan.Ambar_ici = ozetbeyan.Ambar;
                        _ozetBeyan.Baska_rejim = ozetbeyan.BaskaRejim;
                        _ozetBeyan.Aciklama = ozetbeyan.Aciklama;


                        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbTasimaSenet.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == ozetbeyan.OzetBeyanNo).ToListAsync();

                        if (ozetBeyanAcmaTasimaSenediValues != null && ozetBeyanAcmaTasimaSenediValues.Count > 0)
                        {
                            List<KontrolHizmeti.tasimasenetleri> _ozetBeyanTasimaSenediList = new List<KontrolHizmeti.tasimasenetleri>();
                            KontrolHizmeti.tasimasenetleri _ozetBeyanTasimaSenedi;
                            foreach (var tasimaSenedi in ozetBeyanAcmaTasimaSenediValues)
                            {
                                _ozetBeyanTasimaSenedi = new KontrolHizmeti.tasimasenetleri();
                                _ozetBeyanTasimaSenedi.Tasima_senedi_no = tasimaSenedi.TasimaSenediNo;

                                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == ozetbeyan.OzetBeyanNo && v.TasimaSenetInternalNo == tasimaSenedi.TasimaSenetInternalNo).ToListAsync();

                                if (ozetBeyanAcmaTasimaSatirValues != null && ozetBeyanAcmaTasimaSatirValues.Count > 0)
                                {
                                    List<KontrolHizmeti.tasimasatirlari> ozetBeyanAcmaTasimaSatirList = new List<KontrolHizmeti.tasimasatirlari>();
                                    KontrolHizmeti.tasimasatirlari _ozetBeyanTasimaSatir;
                                    foreach (var satir in ozetBeyanAcmaTasimaSatirValues)
                                    {
                                        _ozetBeyanTasimaSatir = new KontrolHizmeti.tasimasatirlari();

                                        _ozetBeyanTasimaSatir.Acilacak_miktar = satir.Miktar != null ? satir.Miktar : 0;
                                        _ozetBeyanTasimaSatir.Ambar_kodu = satir.AmbarKodu;
                                        _ozetBeyanTasimaSatir.Tasima_satir_no = satir.TasimaSatirNo;

                                        ozetBeyanAcmaTasimaSatirList.Add(_ozetBeyanTasimaSatir);
                                    }
                                    _ozetBeyanTasimaSenedi.tasimasatir_bilgi = ozetBeyanAcmaTasimaSatirList.ToArray();
                                }
                                else _ozetBeyanTasimaSenedi.tasimasatir_bilgi = new KontrolHizmeti.tasimasatirlari[0];

                                _ozetBeyanTasimaSenediList.Add(_ozetBeyanTasimaSenedi);
                            }
                            _ozetBeyan.ozbyacma_bilgi = _ozetBeyanTasimaSenediList.ToArray();
                        }
                        else _ozetBeyan.ozbyacma_bilgi = new KontrolHizmeti.tasimasenetleri[0];

                        _ozetBeyanList.Add(_ozetBeyan);
                    }
                    _beyan.Ozetbeyanlar = _ozetBeyanList.ToArray();
                }
                else _beyan.Ozetbeyanlar = new KontrolHizmeti.Ozetbeyan[0];


                #endregion

                #region FIRMA

                var firmaValues = await _beyannameContext.DbFirma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                if (firmaValues != null && firmaValues.Count > 0)
                {
                    List<KontrolHizmeti.firma> _firmaList = new List<KontrolHizmeti.firma>();
                    KontrolHizmeti.firma _firma;
                    foreach (var firma in firmaValues)
                    {
                        _firma = new KontrolHizmeti.firma();
                        _firma.Adi_unvani = firma.AdUnvan;
                        _firma.Cadde_s_no = firma.CaddeSokakNo;
                        _firma.Faks = firma.Faks;
                        _firma.Il_ilce = firma.IlIlce;
                        _firma.Kimlik_turu = firma.KimlikTuru;
                        _firma.No = firma.No;
                        _firma.Posta_kodu = firma.PostaKodu;
                        _firma.Telefon = firma.Telefon;
                        _firma.Tip = firma.Tip;
                        _firma.Ulke_kodu = firma.UlkeKodu;

                        _firmaList.Add(_firma);

                    }
                    _beyan.Firma_bilgi = _firmaList.ToArray();
                }
                else _beyan.Firma_bilgi = new KontrolHizmeti.firma[0];

                #endregion

                #region Teminat

                var teminatValues = await _beyannameContext.DbTeminat.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                if (teminatValues != null && teminatValues.Count > 0)
                {
                    List<KontrolHizmeti.Teminat> _teminatList = new List<KontrolHizmeti.Teminat>();
                    KontrolHizmeti.Teminat _teminat;
                    foreach (var teminat in teminatValues)
                    {
                        _teminat = new KontrolHizmeti.Teminat();
                        _teminat.Teminat_sekli = teminat.TeminatSekli;
                        _teminat.Teminat_orani = teminat.TeminatOrani;
                        _teminat.Global_teminat_no = teminat.GlobalTeminatNo;
                        _teminat.Banka_mektubu_tutari = teminat.BankaMektubuTutari;
                        _teminat.Nakdi_teminat_tutari = teminat.NakdiTeminatTutari;
                        _teminat.Diger_tutar = teminat.DigerTutar;
                        _teminat.Diger_tutar_referansi = teminat.DigerTutarReferansi;
                        _teminat.Aciklama = teminat.Aciklama;

                        _teminatList.Add(_teminat);

                    }
                    _beyan.Teminat = _teminatList.ToArray();
                }
                else _beyan.Teminat = new KontrolHizmeti.Teminat[0];


                #endregion

                #region Kıymet


                var kiymetValues = await _beyannameContext.DbKiymetBildirim.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                if (kiymetValues != null && kiymetValues.Count > 0)
                {
                    List<KontrolHizmeti.Kiymet> _kiymetList = new List<KontrolHizmeti.Kiymet>();
                    KontrolHizmeti.Kiymet _kiymet;
                    foreach (var kiymet in kiymetValues)
                    {
                        _kiymet = new KontrolHizmeti.Kiymet();

                        _kiymet.AliciSatici = kiymet.AliciSatici;
                        _kiymet.AliciSaticiAyrintilar = kiymet.AliciSaticiAyrintilar;
                        _kiymet.Edim = kiymet.Edim;
                        _kiymet.Emsal = kiymet.Emsal;
                        _kiymet.FaturaTarihiSayisi = kiymet.FaturaTarihiSayisi;
                        _kiymet.GumrukIdaresiKarari = kiymet.GumrukIdaresiKarari;
                        _kiymet.Kisitlamalar = kiymet.Kisitlamalar;
                        _kiymet.KisitlamalarAyrintilar = kiymet.KisitlamalarAyrintilar;
                        _kiymet.Munasebet = kiymet.Munasebet;
                        _kiymet.Royalti = kiymet.Royalti;
                        _kiymet.RoyaltiKosullar = kiymet.RoyaltiKosullar;
                        _kiymet.SaticiyaIntikal = kiymet.SaticiyaIntikal;
                        _kiymet.SaticiyaIntikalKosullar = kiymet.SaticiyaIntikalKosullar;
                        _kiymet.SehirYer = kiymet.SehirYer;
                        _kiymet.SozlesmeTarihiSayisi = kiymet.SozlesmeTarihiSayisi;
                        _kiymet.Taahutname = kiymet.Taahhutname;
                        _kiymet.TeslimSekli = kiymet.TeslimSekli;

                        var kiymetKalemValues = await _beyannameContext.DbKiymetBildirimKalem.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KiymetInternalNo == kiymet.KiymetInternalNo).ToListAsync();

                        if (kiymetKalemValues != null && kiymetKalemValues.Count > 0)
                        {
                            List<KontrolHizmeti.KiymetKalem> _kiymetKalemList = new List<KontrolHizmeti.KiymetKalem>();
                            KontrolHizmeti.KiymetKalem _kiymetkalem;
                            foreach (var kalem in kiymetKalemValues)
                            {
                                _kiymetkalem = new KontrolHizmeti.KiymetKalem();
                                _kiymetkalem.BeyannameKalemNo = kalem.BeyannameKalemNo != null ? kalem.BeyannameKalemNo : 0;
                                _kiymetkalem.DigerOdemeler = kalem.DigerOdemeler != null ? kalem.DigerOdemeler : 0;
                                _kiymetkalem.DigerOdemelerNiteligi = kalem.DigerOdemelerNiteligi;
                                _kiymetkalem.DolayliIntikal = kalem.DolayliIntikal != null ? kalem.DolayliIntikal : 0;
                                _kiymetkalem.DolayliOdeme = kalem.DolayliOdeme != null ? kalem.DolayliOdeme : 0;
                                _kiymetkalem.GirisSonrasiNakliye = kalem.GirisSonrasiNakliye != null ? kalem.GirisSonrasiNakliye : 0;
                                _kiymetkalem.IthalaKatilanMalzeme = kalem.IthalaKatilanMalzeme != null ? kalem.IthalaKatilanMalzeme : 0;
                                _kiymetkalem.IthalaUretimAraclar = kalem.IthalaUretimAraclar != null ? kalem.IthalaUretimAraclar : 0;
                                _kiymetkalem.IthalaUretimTuketimMalzemesi = kalem.IthalaUretimTuketimMalzemesi != null ? kalem.IthalaUretimTuketimMalzemesi : 0;
                                _kiymetkalem.KapAmbalajBedeli = kalem.KapAmbalajBedeli != null ? kalem.KapAmbalajBedeli : 0;
                                _kiymetkalem.KiymetKalemNo = kalem.KiymetKalemNo != null ? kalem.KiymetKalemNo : 0;
                                _kiymetkalem.Komisyon = kalem.Komisyon != null ? kalem.Komisyon : 0;
                                _kiymetkalem.Nakliye = kalem.Nakliye != null ? kalem.Nakliye : 0;
                                _kiymetkalem.PlanTaslak = kalem.PlanTaslak != null ? kalem.PlanTaslak : 0;
                                _kiymetkalem.RoyaltiLisans = kalem.RoyaltiLisans != null ? kalem.RoyaltiLisans : 0;
                                _kiymetkalem.Sigorta = kalem.Sigorta != null ? kalem.Sigorta : 0;
                                _kiymetkalem.TeknikYardim = kalem.TeknikYardim != null ? kalem.TeknikYardim : 0;
                                _kiymetkalem.Tellaliye = kalem.Tellaliye != null ? kalem.Tellaliye : 0;
                                _kiymetkalem.VergiHarcFon = kalem.VergiHarcFon != null ? kalem.VergiHarcFon : 0;

                                _kiymetKalemList.Add(_kiymetkalem);
                            }
                            _kiymet.KiymetKalemler = _kiymetKalemList.ToArray();
                        }


                        _kiymetList.Add(_kiymet);

                    }
                    _beyan.KiymetBildirim = _kiymetList.ToArray();
                }
                else _beyan.KiymetBildirim = new KontrolHizmeti.Kiymet[0];

                #endregion

                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                      .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

                gelen.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
                gelen.RefID = islemValues.RefNo;
                gelen.Sifre =  "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
                gelen.IP = "";
                gelen.BeyannameBilgi = _beyan;
                var values = await Kontrol.KontrolAsync(gelen);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(values.Root.OuterXml);
                string guidOf = "", IslemDurumu = "", islemSonucu = "";
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
                                IslemDurumu = "İşlemde";
                                guidOf = n.InnerText;


                            }
                            else if (n.Name == "Durum")
                            {
                                islemSonucu = n.InnerText;


                            }
                        }

                    }
                }


                if (string.IsNullOrWhiteSpace(guidOf))
                    guidOf = "BYT:" + Guid.NewGuid().ToString();

                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {
                        islemValues.Kullanici = Kullanici;
                        islemValues.IslemDurumu = "Gonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("DB", "DBKG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "Kontrol";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = beyanValues.Gumruk;
                        _tarihce.Rejim = beyanValues.Rejim;
                        _tarihce.IslemInternalNo = islemValues.IslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = IslemDurumu;
                        _tarihce.IslemSonucu = islemSonucu;
                        _tarihce.IslemTipi = "1";
                        _tarihce.TicaretTipi = EX.Contains(beyanValues.Rejim) ? "EX" : IM.Contains(beyanValues.Rejim) ? "IM" : AN.Contains(beyanValues.Rejim) ? "AN" : DG.Contains(beyanValues.Rejim) ? "DG" : "";
                        _tarihce.GonderilenVeri = _tarihce.GonderilenVeri = SerializeToXML(gelen);
                        _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;


                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
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
                Bilgi blg = new Bilgi { IslemTipi = "Kontrol Gönderimi", ReferansNo = guidOf, GUID = guidOf, Sonuc = "Kontrol Gönderimi Gerçekleşti", SonucVeriler = null };
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