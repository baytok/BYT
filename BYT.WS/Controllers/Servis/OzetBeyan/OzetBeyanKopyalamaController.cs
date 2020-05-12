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

namespace BYT.WS.Controllers.Servis.OzetBeyan
{
    [Route("api/BYT/Servis/OzetBeyan/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanKopyalamaController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        public IConfiguration Configuration { get; }

        public OzetBeyanKopyalamaController(IslemTarihceDataContext islemTarihcecontext,  IOptions<ServisCredential> servisCredential, IConfiguration configuration)
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
            var options = new DbContextOptionsBuilder<OzetBeyanDataContext>()
                 .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                 .Options;
            var _beyannameContext = new OzetBeyanDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim());
                if (islemValues != null)
                {
                    var beyanValues = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                    var senetValues = await _beyannameContext.ObTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                    var internalrefid = _beyannameContext.GetRefIdNextSequenceValue(beyanValues.BeyanTuru);
                    string InternalNo = beyanValues.BeyanTuru + beyanValues.KullaniciKodu + "OB" + internalrefid.ToString().PadLeft(5, '0');

                    List<ObTasimaSenet> lstSenet = new List<ObTasimaSenet>();
                    List<ObUgrakUlke> lstUgrak = new List<ObUgrakUlke>();
                    List<ObTasitUgrakUlke> lstTasitUgrak = new List<ObTasitUgrakUlke>();
                    List<ObTeminat> lstTeminat = new List<ObTeminat>();
                    ObTasiyiciFirma tasiyiciFirma = new ObTasiyiciFirma();
                    List<ObIhracat> lstIhracat = new List<ObIhracat>();
                    List<ObTasimaSatir> lstSatir = new List<ObTasimaSatir>();
                    List<ObSatirEsya> lstSatirEsya = new List<ObSatirEsya>();
                    List<ObOzetBeyanAcma> lstOzetBeyanAcma = new List<ObOzetBeyanAcma>();
                    List<ObOzetBeyanAcmaTasimaSenet> lstOzetBeyanAcmaTasimaSenedi = new List<ObOzetBeyanAcmaTasimaSenet>();
                    List<ObOzetBeyanAcmaTasimaSatir> lstOzetBeyanAcmaTasimaSatiri = new List<ObOzetBeyanAcmaTasimaSatir>();

                    var newbeyanValues = new ObBeyan
                    {
                        BeyanSahibiVergiNo = beyanValues.BeyanSahibiVergiNo,
                        BeyanTuru = beyanValues.BeyanTuru,
                        Diger = beyanValues.Diger,
                        DorseNo1 = beyanValues.DorseNo1,
                        DorseNo1Uyrugu = beyanValues.DorseNo1Uyrugu,
                        DorseNo2 = beyanValues.DorseNo2,
                        DorseNo2Uyrugu = beyanValues.DorseNo2Uyrugu,
                        EkBelgeSayisi = beyanValues.EkBelgeSayisi,
                        EmniyetGuvenlik = beyanValues.EmniyetGuvenlik,
                        GrupTasimaSenediNo = beyanValues.GrupTasimaSenediNo,
                        GumrukIdaresi = beyanValues.GumrukIdaresi,
                        KullaniciKodu = beyanValues.KullaniciKodu,
                        Kurye = beyanValues.Kurye,
                        LimanYerAdiBos = beyanValues.LimanYerAdiBos,
                        LimanYerAdiYuk = beyanValues.LimanYerAdiYuk,
                        OlsuturulmaTarihi = DateTime.Now,
                        OncekiBeyanNo = beyanValues.OncekiBeyanNo,
                        OzetBeyanInternalNo = InternalNo,
                        OzetBeyanNo = beyanValues.OzetBeyanNo,
                        PlakaSeferNo = beyanValues.PlakaSeferNo,
                        ReferansNumarasi = beyanValues.ReferansNumarasi,
                        Rejim = beyanValues.Rejim,
                        TasimaSekli = beyanValues.TasimaSekli,
                        TasitinAdi = beyanValues.TasitinAdi,
                        TasiyiciVergiNo = beyanValues.TasiyiciVergiNo,
                        TirAtaKarneNo = beyanValues.TirAtaKarneNo,
                        UlkeKodu = beyanValues.UlkeKodu,
                        UlkeKoduBos = beyanValues.UlkeKoduBos,
                        UlkeKoduYuk = beyanValues.UlkeKoduYuk,
                        VarisCikisGumrukIdaresi = beyanValues.VarisCikisGumrukIdaresi,
                        VarisTarihSaati = beyanValues.VarisTarihSaati,
                        TescilStatu = "Olusturuldu",
                        YuklemeBosaltmaYeri = beyanValues.YuklemeBosaltmaYeri,
                        XmlRefId = InternalNo,

                    };


                    foreach (var x in senetValues)
                    {

                        ObTasimaSenet senet = new ObTasimaSenet
                        {
                            AcentaAdi = x.AcentaAdi,
                            AcentaVergiNo = x.AcentaVergiNo,
                            AktarmaTipi = x.AktarmaTipi,
                            AktarmaYapilacak = x.AktarmaYapilacak,
                            AliciAdi = x.AliciAdi,
                            AliciVergiNo = x.AliciVergiNo,
                            AmbarHarici = x.AmbarHarici,
                            BildirimTarafiAdi = x.BildirimTarafiAdi,
                            BildirimTarafiVergiNo = x.BildirimTarafiVergiNo,
                            DuzenlendigiUlke = x.DuzenlendigiUlke,
                            EmniyetGuvenlik = x.EmniyetGuvenlik,
                            EsyaninBulunduguYer = x.EsyaninBulunduguYer,
                            FaturaDoviz = x.FaturaDoviz,
                            FaturaToplami = x.FaturaToplami,
                            GondericiAdi = x.GondericiAdi,
                            GondericiVergiNo = x.GondericiVergiNo,
                            Grup = x.Grup,
                            Konteyner = x.Konteyner,
                            NavlunDoviz = x.NavlunDoviz,
                            NavlunTutari = x.NavlunTutari,
                            OdemeSekli = x.OdemeSekli,
                            OncekiSeferNumarasi = x.OncekiSeferNumarasi,
                            OncekiSeferTarihi = x.OncekiSeferTarihi,
                            OzetBeyanNo = x.OzetBeyanNo,
                            Roro = x.Roro,
                            TasimaSenediNo = x.TasimaSenediNo,
                            SenetSiraNo = x.SenetSiraNo,
                            TasimaSenetInternalNo = newbeyanValues.OzetBeyanInternalNo + "|" + x.SenetSiraNo,
                            OzetBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo,


                        };

                        var ugrakValues = await _beyannameContext.ObUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == x.TasimaSenetInternalNo).ToListAsync();

                        foreach (var o in ugrakValues)
                        {
                            ObUgrakUlke y = new ObUgrakUlke
                            {
                                OzetBeyanInternalNo = senet.OzetBeyanInternalNo,
                                TasimaSenetInternalNo = senet.TasimaSenetInternalNo,
                                LimanYerAdi = o.LimanYerAdi,
                                UlkeKodu = o.UlkeKodu,

                            };
                            lstUgrak.Add(y);
                        }

                        var ihracatValues = await _beyannameContext.ObIhracat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == x.TasimaSenetInternalNo).ToListAsync();

                        foreach (var o in ihracatValues)
                        {
                            ObIhracat y = new ObIhracat
                            {
                                OzetBeyanInternalNo = senet.OzetBeyanInternalNo,
                                TasimaSenetInternalNo = senet.TasimaSenetInternalNo,
                                BrutAgirlik = o.BrutAgirlik,
                                KapAdet = o.KapAdet,
                                Numara = o.Numara,
                                Parcali = o.Parcali,
                                Tip = o.Tip,


                            };
                            lstIhracat.Add(y);
                        }

                        var satirValues = await _beyannameContext.ObTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == x.TasimaSenetInternalNo).ToListAsync();

                        foreach (var o in satirValues)
                        {
                            ObTasimaSatir y = new ObTasimaSatir
                            {
                                OzetBeyanInternalNo = senet.OzetBeyanInternalNo,
                                TasimaSenetInternalNo = senet.TasimaSenetInternalNo,
                                BrutAgirlik = o.BrutAgirlik,
                                KapAdedi = o.KapAdedi,
                                KapCinsi = o.KapCinsi,
                                KonteynerTipi = o.KonteynerTipi,
                                KonteynerYukDurumu = o.KonteynerYukDurumu,
                                MarkaNo = o.MarkaNo,
                                MuhurNumarasi = o.MuhurNumarasi,
                                NetAgirlik = o.NetAgirlik,
                                OlcuBirimi = o.OlcuBirimi,
                                SatirNo = o.SatirNo,
                                TasimaSatirInternalNo = senet.TasimaSenetInternalNo + "|" + o.SatirNo,

                            };
                            var satirEsyaValues = await _beyannameContext.ObSatirEsya.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.TasimaSenetInternalNo == x.TasimaSenetInternalNo && v.TasimaSatirInternalNo == o.TasimaSatirInternalNo).ToListAsync();

                            foreach (var k in satirEsyaValues)
                            {
                                ObSatirEsya z = new ObSatirEsya
                                {
                                    OzetBeyanInternalNo = senet.OzetBeyanInternalNo,
                                    TasimaSenetInternalNo = senet.TasimaSenetInternalNo,
                                    TasimaSatirInternalNo = y.TasimaSatirInternalNo,
                                    BmEsyaKodu = k.BmEsyaKodu,
                                    BrutAgirlik = k.BrutAgirlik,
                                    EsyaKodu = k.EsyaKodu,
                                    EsyaninTanimi = k.EsyaninTanimi,
                                    KalemFiyati = k.KalemFiyati,
                                    KalemFiyatiDoviz = k.KalemFiyatiDoviz,
                                    KalemSiraNo = k.KalemSiraNo,
                                    NetAgirlik = k.NetAgirlik,
                                    OlcuBirimi = k.OlcuBirimi,

                                };
                                lstSatirEsya.Add(z);
                            }


                            lstSatir.Add(y);
                        }


                        lstSenet.Add(senet);
                    }

                    var firmaValues = await _beyannameContext.ObTasiyiciFirma.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo);
                    if (firmaValues != null)
                    {
                        tasiyiciFirma = new ObTasiyiciFirma
                        {
                            OzetBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo,
                            AdUnvan=firmaValues.AdUnvan,
                            CaddeSokakNo=firmaValues.CaddeSokakNo,
                            Faks=firmaValues.Faks,
                            IlIlce=firmaValues.IlIlce,
                            KimlikTuru=firmaValues.KimlikTuru,
                            No=firmaValues.No,
                            PostaKodu=firmaValues.PostaKodu,
                            Telefon=firmaValues.Telefon,
                            Tip=firmaValues.Tip,
                            UlkeKodu=firmaValues.UlkeKodu,
                            SonIslemZamani=DateTime.Now
                        };
                    }
                    
              
                    var teminatValues = await _beyannameContext.ObTeminat.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var o in teminatValues)
                    {
                        ObTeminat teminat = new ObTeminat
                        {
                          OzetBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo,
                          Aciklama=o.Aciklama,
                          BankaMektubuTutari=o.BankaMektubuTutari,
                          DigerTutar=o.DigerTutar,
                          DigerTutarReferansi=o.DigerTutarReferansi,
                          GlobalTeminatNo=o.GlobalTeminatNo,
                          NakdiTeminatTutari=o.NakdiTeminatTutari,
                          TeminatOrani=o.TeminatOrani,
                          TeminatSekli=o.TeminatSekli,
                          

                        };
                        lstTeminat.Add(teminat);
                    }

                    var tasitUgrakValues = await _beyannameContext.ObTasitUgrakUlke.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    foreach (var o in tasitUgrakValues)
                    {
                        ObTasitUgrakUlke ulke = new ObTasitUgrakUlke
                        {
                            OzetBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo,
                            HareketTarihSaati=o.HareketTarihSaati,
                            LimanYerAdi=o.LimanYerAdi,
                            UlkeKodu=o.UlkeKodu

                        };
                          lstTasitUgrak.Add(ulke);
                    }

                


                    var ozetBeyanAcmaValues = await _beyannameContext.ObOzetBeyanAcma.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo).ToListAsync();
                    int i = 1;
                    foreach (var o in ozetBeyanAcmaValues)
                    {
                       
                        ObOzetBeyanAcma ozet = new ObOzetBeyanAcma
                        {
                            Aciklama = o.Aciklama,
                            Ambar = o.Ambar,
                            BaskaRejim = o.BaskaRejim,
                            IslemKapsami = o.IslemKapsami,
                            OzetBeyanNo = o.OzetBeyanNo,
                            DahiliNoAcma=o.DahiliNoAcma,
                            OzetBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo,
                            OzetBeyanAcmaBeyanInternalNo = newbeyanValues.OzetBeyanInternalNo + "|" + i.ToString(),
                          
                        };
                        i++;
                        var ozetBeyanAcmaTasimaSenediValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSenet.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == o.OzetBeyanAcmaBeyanInternalNo).ToListAsync();

                        if (ozetBeyanAcmaTasimaSenediValues.Count > 0)
                        {
                            int j = 1;
                            foreach (var t in ozetBeyanAcmaTasimaSenediValues)
                            {
                               
                                ObOzetBeyanAcmaTasimaSenet tasima = new ObOzetBeyanAcmaTasimaSenet
                                {
                                    OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                    OzetBeyanAcmaBeyanInternalNo = ozet.OzetBeyanAcmaBeyanInternalNo,
                                    TasimaSenediNo = t.TasimaSenediNo,
                                    DahiliNoAcilanSenet=t.DahiliNoAcilanSenet,
                                    TasimaSenetInternalNo = ozet.OzetBeyanAcmaBeyanInternalNo + "|" + j.ToString(),
                                

                                };
                                j++;
                                var ozetBeyanAcmaTasimaSatirValues = await _beyannameContext.ObOzetBeyanAcmaTasimaSatir.Where(v => v.OzetBeyanInternalNo == islemValues.BeyanInternalNo && v.OzetBeyanAcmaBeyanInternalNo == o.OzetBeyanAcmaBeyanInternalNo && v.TasimaSenetInternalNo == t.TasimaSenetInternalNo).ToListAsync();
                                if (ozetBeyanAcmaTasimaSatirValues.Count > 0)
                                {
                                    foreach (var s in ozetBeyanAcmaTasimaSatirValues)
                                    {
                                     
                                        ObOzetBeyanAcmaTasimaSatir satir = new ObOzetBeyanAcmaTasimaSatir
                                        {
                                            AmbarKodu = s.AmbarKodu,
                                            OzetBeyanInternalNo = ozet.OzetBeyanInternalNo,
                                            OzetBeyanAcmaBeyanInternalNo = ozet.OzetBeyanAcmaBeyanInternalNo,
                                            TasimaSenetInternalNo = tasima.TasimaSenetInternalNo,
                                            AcmaSatirNo = s.AcmaSatirNo,
                                            AcilacakMiktar = s.AcilacakMiktar,
                                            AmbardakiMiktar=s.AmbardakiMiktar,
                                            Birim=s.Birim,
                                            EsyaCinsi=s.EsyaCinsi,
                                            KapatilanMiktar=s.KapatilanMiktar,
                                            MarkaNo=s.MarkaNo,
                                            OlcuBirimi=s.OlcuBirimi,
                                            ToplamMiktar=s.ToplamMiktar,
                                           
                                        };
                                     
                                        lstOzetBeyanAcmaTasimaSatiri.Add(satir);
                                    }
                                }

                                lstOzetBeyanAcmaTasimaSenedi.Add(tasima);
                            }
                        }

                        lstOzetBeyanAcma.Add(ozet);
                    }


                
                    using (var transaction = _beyannameContext.Database.BeginTransaction())
                    {
                        try
                        {
                            newbeyanValues.SonIslemZamani = DateTime.Now;
                            _beyannameContext.Entry(newbeyanValues).State = EntityState.Added;
                            _beyannameContext.Entry(tasiyiciFirma).State = EntityState.Added;

                            foreach (var item in lstTasitUgrak)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                      
                            foreach (var item in lstTeminat)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }

                            foreach (var item in lstOzetBeyanAcma)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }

                            foreach (var item in lstOzetBeyanAcmaTasimaSenedi)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                         
                            foreach (var item in lstOzetBeyanAcmaTasimaSatiri)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            
                            foreach (var item in lstSenet)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstIhracat)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstUgrak)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstSatir)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            foreach (var item in lstSatirEsya)
                            {
                                item.SonIslemZamani = DateTime.Now;
                                _beyannameContext.Entry(item).State = EntityState.Added;
                            }
                            

                            await _beyannameContext.SaveChangesAsync();


                            Islem _islem = new Islem();
                            _islem.Kullanici = newbeyanValues.KullaniciKodu;
                            _islem.IslemTipi = "";
                            _islem.BeyanTipi = "OzetBeyan";
                            _islem.IslemDurumu = "Olusturuldu";
                            _islem.RefNo = newbeyanValues.XmlRefId;
                            _islem.BeyanInternalNo = newbeyanValues.OzetBeyanInternalNo;
                            _islem.IslemInternalNo = newbeyanValues.OzetBeyanInternalNo;
                            _islem.OlusturmaZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _islem.GonderimSayisi = 0;

                            _islemTarihceContext.Entry(_islem).State = EntityState.Added;
                            await _islemTarihceContext.SaveChangesAsync();

                            transaction.Commit();

                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;
                            List<Internal.Bilgi> lstbilgi = new List<Internal.Bilgi>();
                            lstbilgi.Add(new Bilgi { IslemTipi = "Beyanname Kopyalama", ReferansNo = newbeyanValues.OzetBeyanInternalNo, Sonuc = "Kopyalama Başarılı", SonucVeriler = null, GUID = null });
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