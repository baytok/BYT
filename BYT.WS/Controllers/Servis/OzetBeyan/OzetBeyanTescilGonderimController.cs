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
        public async Task<ServisDurum> GetTescil(string IslemInternalNo, string Kullanici)
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
                var tasiyiciFirmaValues = await _beyannameContext.ObTasiyiciFirma.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                var tasitUgrakValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();


                OzetBeyanGelen gelen = new OzetBeyanGelen();


                #region Genel
                OzetBeyanBilgisi _beyan = new OzetBeyanBilgisi();

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


                #endregion

                #region TasıtUğrakÜlke
                if (tasitUgrakValues != null && tasitUgrakValues.Count > 0)
                {
                    TasitinUgradigiUlkeBilgisi _tulke;
                    List<TasitinUgradigiUlkeBilgisi> __tulkeList = new List<TasitinUgradigiUlkeBilgisi>();
                    foreach (var item in tasitUgrakValues)
                    {
                        _tulke = new TasitinUgradigiUlkeBilgisi();

                        _tulke.HareketTarihSaati = Convert.ToDateTime(item.HareketTarihSaati);
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
                
                    //    List<KontrolHizmeti.Ozetbeyan> _ozetBeyanList = new List<KontrolHizmeti.Ozetbeyan>();
                    //    KontrolHizmeti.Ozetbeyan _ozetBeyan;
                    //    foreach (var ozetbeyan in ozetBeyanAcmaValues)
                    //    {
                    //        _ozetBeyan = new KontrolHizmeti.Ozetbeyan();
                    //        _ozetBeyan.Ozetbeyan_no = ozetbeyan.OzetBeyanNo;
                    //        _ozetBeyan.Ozetbeyan_islem_kapsami = ozetbeyan.IslemKapsami;
                    //        _ozetBeyan.Ambar_ici = ozetbeyan.Ambar;
                    //        _ozetBeyan.Baska_rejim = ozetbeyan.BaskaRejim;
                    //        _ozetBeyan.Aciklama = ozetbeyan.Aciklama;


                    //        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSenet.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == ozetbeyan.OzetBeyanNo).ToListAsync();

                    //        if (ozetBeyanAcmaTasimaSenediValues != null && ozetBeyanAcmaTasimaSenediValues.Count > 0)
                    //        {
                    //            List<KontrolHizmeti.tasimasenetleri> _ozetBeyanTasimaSenediList = new List<KontrolHizmeti.tasimasenetleri>();
                    //            KontrolHizmeti.tasimasenetleri _ozetBeyanTasimaSenedi;
                    //            foreach (var tasimaSenedi in ozetBeyanAcmaTasimaSenediValues)
                    //            {
                    //                _ozetBeyanTasimaSenedi = new KontrolHizmeti.tasimasenetleri();
                    //                _ozetBeyanTasimaSenedi.Tasima_senedi_no = tasimaSenedi.TasimaSenediNo;

                    //                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.DbOzetBeyanAcmaTasimaSatir.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanInternalNo == ozetbeyan.OzetBeyanNo && v.TasimaSenetInternalNo == tasimaSenedi.TasimaSenetInternalNo).ToListAsync();

                    //                if (ozetBeyanAcmaTasimaSatirValues != null && ozetBeyanAcmaTasimaSatirValues.Count > 0)
                    //                {
                    //                    List<KontrolHizmeti.tasimasatirlari> ozetBeyanAcmaTasimaSatirList = new List<KontrolHizmeti.tasimasatirlari>();
                    //                    KontrolHizmeti.tasimasatirlari _ozetBeyanTasimaSatir;
                    //                    foreach (var satir in ozetBeyanAcmaTasimaSatirValues)
                    //                    {
                    //                        _ozetBeyanTasimaSatir = new KontrolHizmeti.tasimasatirlari();

                    //                        _ozetBeyanTasimaSatir.Acilacak_miktar = satir.Miktar != null ? satir.Miktar : 0;
                    //                        _ozetBeyanTasimaSatir.Ambar_kodu = satir.AmbarKodu;
                    //                        _ozetBeyanTasimaSatir.Tasima_satir_no = satir.TasimaSatirNo.ToString();

                    //                        ozetBeyanAcmaTasimaSatirList.Add(_ozetBeyanTasimaSatir);
                    //                    }
                    //                    _ozetBeyanTasimaSenedi.tasimasatir_bilgi = ozetBeyanAcmaTasimaSatirList.ToArray();
                    //                }
                    //                else _ozetBeyanTasimaSenedi.tasimasatir_bilgi = new KontrolHizmeti.tasimasatirlari[0];

                    //                _ozetBeyanTasimaSenediList.Add(_ozetBeyanTasimaSenedi);
                    //            }
                    //            _ozetBeyan.ozbyacma_bilgi = _ozetBeyanTasimaSenediList.ToArray();
                    //        }
                    //        else _ozetBeyan.ozbyacma_bilgi = new KontrolHizmeti.tasimasenetleri[0];

                    //        _ozetBeyanList.Add(_ozetBeyan);
                    //    }
                    //    _beyan.Ozetbeyanlar = _ozetBeyanList.ToArray();
                    
                }
                else _beyan.OzbyAcmalar = new List<OzbyAcmaBilgisi>();
                #endregion

                #region TaşıyıcıFirma
                if (tasiyiciFirmaValues != null && tasiyiciFirmaValues.Count > 0)
                {
                }
                else _beyan.TasiyiciFirma = new FirmaBilgisi();
                #endregion

                #region TaşımaSenet

                //if (kalemValues != null && kalemValues.Count > 0)
                //{
                //    KontrolHizmeti.kalem _kalem;
                //    List<KontrolHizmeti.kalem> _kalemList = new List<KontrolHizmeti.kalem>();
                //    foreach (var item in kalemValues)
                //    {
                //        _kalem = new KontrolHizmeti.kalem();

                //        _kalem.Aciklama_44 = item.Aciklama44;
                //        _kalem.Adedi = item.Adet != null ? item.Adet : 0;
                //        _kalem.Algilama_birimi_1 = item.AlgilamaBirimi1;
                //        _kalem.Algilama_birimi_2 = item.AlgilamaBirimi2;
                //        _kalem.Algilama_birimi_3 = item.AlgilamaBirimi3;
                //        _kalem.Algilama_miktari_1 = item.AlgilamaMiktari1 != null ? item.AlgilamaMiktari1 : 0;
                //        _kalem.Algilama_miktari_2 = item.AlgilamaMiktari2 != null ? item.AlgilamaMiktari2 : 0;
                //        _kalem.Algilama_miktari_3 = item.AlgilamaMiktari3 != null ? item.AlgilamaMiktari3 : 0;
                //        _kalem.Brut_agirlik = item.BrutAgirlik != null ? item.BrutAgirlik : 0;
                //        _kalem.Cinsi = item.Cins;
                //        _kalem.Ek_kod = item.EkKod;
                //        _kalem.Giris_Cikis_Amaci = item.GirisCikisAmaci;
                //        _kalem.Giris_Cikis_Amaci_Aciklama = item.GirisCikisAmaciAciklama;
                //        _kalem.Gtip = item.Gtip;
                //        _kalem.Fatura_miktari = item.FaturaMiktari != null ? item.FaturaMiktari : 0;
                //        _kalem.Fatura_miktarinin_dovizi = item.FaturaMiktariDovizi;
                //        _kalem.Ikincil_islem = item.IkincilIslem;
                //        _kalem.Imalatci_firma_bilgisi = item.ImalatciFirmaBilgisi;
                //        _kalem.Imalatci_Vergino = item.ImalatciVergiNo;
                //        _kalem.Istatistiki_kiymet = item.IstatistikiKiymet != null ? item.IstatistikiKiymet : 0;
                //        _kalem.Istatistiki_miktar = item.IstatistikiMiktar != null ? item.IstatistikiMiktar : 0;
                //        _kalem.Kalem_Islem_Niteligi = item.KalemIslemNiteligi;
                //        _kalem.Kalem_sira_no = item.KalemSiraNo;
                //        _kalem.Kullanilmis_esya = item.KullanilmisEsya;
                //        _kalem.Marka = item.Marka;
                //        _kalem.Mahrece_iade = item.MahraceIade;
                //        _kalem.Mensei_ulke = item.MenseiUlke;
                //        _kalem.Miktar = item.Miktar != null ? item.Miktar : 0;
                //        _kalem.Miktar_birimi = item.MiktarBirimi;
                //        _kalem.Muafiyetler_1 = item.Muafiyetler1;
                //        _kalem.Muafiyetler_2 = item.Muafiyetler2;
                //        _kalem.Muafiyetler_3 = item.Muafiyetler3;
                //        _kalem.Muafiyetler_4 = item.Muafiyetler4;
                //        _kalem.Muafiyetler_5 = item.Muafiyetler5;
                //        _kalem.Muafiyet_Aciklamasi = item.MuafiyetAciklamasi;
                //        _kalem.Navlun_miktari = item.NavlunMiktari != null ? item.NavlunMiktari : 0;
                //        _kalem.Navlun_miktarinin_dovizi = item.NavlunMiktariDovizi;
                //        _kalem.Net_agirlik = item.NetAgirlik != null ? item.NetAgirlik : 0;
                //        _kalem.Numara = item.Numara;
                //        _kalem.Ozellik = item.Ozellik;
                //        _kalem.Referans_Tarihi = "";
                //        _kalem.Satir_no = item.SatirNo;
                //        _kalem.Sigorta_miktari = item.SigortaMiktari != null ? item.NavlunMiktari : 0;
                //        _kalem.Sigorta_miktarinin_dovizi = item.SigortaMiktariDovizi;
                //        _kalem.Sinir_gecis_ucreti = item.SinirGecisUcreti != null ? item.SinirGecisUcreti : 0;
                //        _kalem.STM_IlKodu = item.StmIlKodu;
                //        _kalem.Tamamlayici_olcu_birimi = item.TamamlayiciOlcuBirimi;
                //        _kalem.Tarifedeki_tanimi = "";
                //        _kalem.Ticari_tanimi = item.TicariTanimi;
                //        _kalem.Teslim_sekli = item.TeslimSekli;
                //        _kalem.Uluslararasi_anlasma = item.UluslararasiAnlasma;
                //        _kalem.YurtDisi_Diger = item.YurtDisiDiger != null ? item.YurtDisiDiger : 0;
                //        _kalem.YurtDisi_Diger_Aciklama = item.YurtDisiDigerAciklama;
                //        _kalem.YurtDisi_Diger_Dovizi = item.YurtDisiDigerDovizi;
                //        _kalem.YurtDisi_Demuraj = item.YurtDisiDemuraj != null ? item.YurtDisiDemuraj : 0;
                //        _kalem.YurtDisi_Demuraj_Dovizi = item.YurtDisiDemurajDovizi;
                //        _kalem.YurtDisi_Faiz = item.YurtDisiFaiz != null ? item.YurtDisiFaiz : 0;
                //        _kalem.YurtDisi_Faiz_Dovizi = item.YurtDisiFaizDovizi;
                //        _kalem.YurtDisi_Komisyon = item.YurtDisiKomisyon != null ? item.YurtDisiKomisyon : 0;
                //        _kalem.YurtDisi_Komisyon_Dovizi = item.YurtDisiKomisyonDovizi;
                //        _kalem.YurtDisi_Royalti = item.YurtDisiRoyalti != null ? item.YurtDisiRoyalti : 0;
                //        _kalem.YurtDisi_Royalti_Dovizi = item.YurtDisiRoyaltiDovizi;
                //        _kalem.Yurtici_Banka = item.YurtIciBanka != null ? item.YurtIciBanka : 0;
                //        _kalem.Yurtici_Cevre = item.YurtIciCevre != null ? item.YurtIciCevre : 0;
                //        _kalem.Yurtici_Diger = item.YurtIciDiger != null ? item.YurtIciDiger : 0;
                //        _kalem.Yurtici_Diger_Aciklama = item.YurtIciDigerAciklama;
                //        _kalem.Yurtici_Depolama = item.YurtIciDepolama != null ? item.YurtIciDepolama : 0;
                //        _kalem.Yurtici_Kkdf = item.YurtIciKkdf != null ? item.YurtIciKkdf : 0;
                //        _kalem.Yurtici_Kultur = item.YurtIciKultur != null ? item.YurtIciKultur : 0;
                //        _kalem.Yurtici_Liman = item.YurtIciLiman != null ? item.YurtIciLiman : 0;
                //        _kalem.Yurtici_Tahliye = item.YurtIciTahliye != null ? item.YurtIciTahliye : 0;


                //        var tamamlayiciValues = await _beyannameContext.DbTamamlayiciBilgi.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                //        if (tamamlayiciValues != null && tamamlayiciValues.Count > 0)
                //        {
                //            List<KontrolHizmeti.tamamlayici> _tamamlayiciList = new List<KontrolHizmeti.tamamlayici>();
                //            KontrolHizmeti.tamamlayici _tamamlayici;
                //            foreach (var tamamb in tamamlayiciValues)
                //            {
                //                _tamamlayici = new KontrolHizmeti.tamamlayici();
                //                _tamamlayici.Tamamlayici_bilgi = tamamb.Bilgi;
                //                _tamamlayici.Tamamlayici_bilgi_orani = tamamb.Oran;


                //                _tamamlayiciList.Add(_tamamlayici);
                //            }

                //            _kalem.tamamlayici_bilgi = _tamamlayiciList.ToArray();
                //        }
                //        else _kalem.tamamlayici_bilgi = new KontrolHizmeti.tamamlayici[0];

                //        var tcgbacmaiValues = await _beyannameContext.DbBeyannameAcma.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                //        if (tcgbacmaiValues != null && tcgbacmaiValues.Count > 0)
                //        {
                //            List<KontrolHizmeti.tcgbacmakapatma> _acmaList = new List<KontrolHizmeti.tcgbacmakapatma>();
                //            KontrolHizmeti.tcgbacmakapatma _acma;
                //            foreach (var acma in tcgbacmaiValues)
                //            {
                //                _acma = new KontrolHizmeti.tcgbacmakapatma();
                //                _acma.Kapatilan_beyanname_no = acma.BeyannameNo;
                //                _acma.Kapatilan_kalem_no = acma.KalemNo;
                //                _acma.Kapatilan_miktar = acma.Miktar;
                //                _acma.Aciklama = acma.Aciklama;

                //                _acmaList.Add(_acma);
                //            }

                //            _kalem.tcgbacmakapatma_bilgi = _acmaList.ToArray();
                //        }
                //        else _kalem.tcgbacmakapatma_bilgi = new KontrolHizmeti.tcgbacmakapatma[0];

                //        var markaValues = await _beyannameContext.DbMarka.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                //        if (markaValues != null && markaValues.Count > 0)
                //        {
                //            List<KontrolHizmeti.Marka> _markaList = new List<KontrolHizmeti.Marka>();
                //            KontrolHizmeti.Marka _marka;
                //            foreach (var marka in markaValues)
                //            {
                //                _marka = new KontrolHizmeti.Marka();
                //                _marka.Marka_Adi = marka.MarkaAdi;
                //                _marka.Marka_Kiymeti = marka.MarkaKiymeti != null ? marka.MarkaKiymeti : 0;
                //                _marka.Marka_Tescil_No = marka.MarkaTescilNo;
                //                _marka.Marka_Turu = marka.MarkaTuru;
                //                _marka.Model = marka.Model;
                //                _marka.Model_Yili = marka.ModelYili;
                //                _marka.MotorGucu = marka.MotorGucu;
                //                _marka.MotorNo = marka.MotorNo;
                //                _marka.Motor_hacmi = marka.MotorHacmi;
                //                _marka.MotorTipi = marka.MotorTipi;
                //                _marka.Referans_No = marka.ReferansNo;
                //                _marka.Renk = marka.Renk;
                //                _marka.Silindir_adedi = marka.SilindirAdet != null ? marka.SilindirAdet : 0;
                //                _marka.Vites = marka.Vites;
                //                _markaList.Add(_marka);
                //            }
                //            _kalem.marka_model_bilgi = _markaList.ToArray();
                //        }
                //        else _kalem.marka_model_bilgi = new KontrolHizmeti.Marka[0];


                //        var konteynerValues = await _beyannameContext.DbKonteyner.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                //        if (konteynerValues != null && konteynerValues.Count > 0)
                //        {
                //            List<KontrolHizmeti.Konteyner> _konteynerList = new List<KontrolHizmeti.Konteyner>();
                //            KontrolHizmeti.Konteyner _konteyner;
                //            foreach (var konteyner in konteynerValues)
                //            {
                //                _konteyner = new KontrolHizmeti.Konteyner();
                //                _konteyner.Ulke_Kodu = konteyner.UlkeKodu;
                //                _konteyner.Konteyner_No = konteyner.KonteynerNo;


                //                _konteynerList.Add(_konteyner);
                //            }

                //            _kalem.konteyner_Bilgi = _konteynerList.ToArray();
                //        }
                //        else _kalem.konteyner_Bilgi = new KontrolHizmeti.Konteyner[0];


                //        var odemeValues = await _beyannameContext.DbOdemeSekli.Where(v => v.BeyanInternalNo == islemValues.BeyanInternalNo && v.KalemInternalNo == item.KalemInternalNo).ToListAsync();

                //        if (odemeValues != null && odemeValues.Count > 0)
                //        {
                //            List<KontrolHizmeti.OdemeSekli> _odemeList = new List<KontrolHizmeti.OdemeSekli>();
                //            KontrolHizmeti.OdemeSekli _odeme;
                //            foreach (var odeme in odemeValues)
                //            {
                //                _odeme = new KontrolHizmeti.OdemeSekli();
                //                _odeme.OdemeSekliKodu = odeme.OdemeSekliKodu;
                //                _odeme.OdemeTutari = odeme.OdemeTutari != null ? odeme.OdemeTutari : 0;
                //                _odeme.TBFID = odeme.TBFID;

                //                _odemeList.Add(_odeme);
                //            }

                //            _kalem.OdemeSekilleri = _odemeList.ToArray();
                //        }
                //        else _kalem.OdemeSekilleri = new KontrolHizmeti.OdemeSekli[0];


                //        _kalemList.Add(_kalem);
                //    }
                //    _beyan.Kalemler = _kalemList.ToArray();
                //}
                //else _beyan.Kalemler = new KontrolHizmeti.kalem[0];


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
                gelen.ozetBeyanBilgisi = _beyan;

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
                            islemValues.IslemSonucu = "Tescil Mesajı Oluşturuldu";
                            islemValues.Guidof = guidOf;
                            islemValues.GonderimSayisi++;
                            _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        }
                        else
                        {
                            Islem _islem = new Islem();
                            newIslemInternalNo = islemValues.BeyanInternalNo.Replace("DB", "DBTG");
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
                        _tarihce.IslemSonucu = "Tescil Mesajı Oluşturuldu";
                        _tarihce.IslemTipi = "0";
                        _tarihce.TicaretTipi = ozetBeyanValues.Rejim;
                        _tarihce.GonderilecekVeri = imzasizMesaj;
                        _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;

                        ozetBeyanValues.SonIslemZamani = DateTime.Now;
                        ozetBeyanValues.TescilStatu = "Tescil Mesajı Oluşturuldu";
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
                Bilgi blg = new Bilgi { IslemTipi = "Tescil Gönderimi", ReferansNo = imzasizMesaj, GUID = guidOf, Sonuc = "Tescil Gönderimi Gerçekleşti", SonucVeriler = null };
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
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {

                var tarihceValues = await _islemTarihceContext.Tarihce.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guid == Guid.Trim() && v.IslemDurumu == "Imzalandi");
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim() && v.Guidof == Guid.Trim() && v.IslemDurumu == "Imzalandi");


                string guidOf = "", IslemDurumu = "", islemSonucu = "";
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("Root");
                root.InnerText = tarihceValues.ImzaliVeri;
                TescilHizmeti.Gumruk_Biztalk_EImzaTescil_Tescil_PortTescilSoapClient Tescil = ServiceHelper.GetTescilWSClient(_servisCredential.username, _servisCredential.password);
                await Tescil.TescilAsync(root);

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


                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {
                        islemValues.Kullanici = Kullanici;
                        islemValues.IslemDurumu = "Gonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("DB", "DBTG");
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