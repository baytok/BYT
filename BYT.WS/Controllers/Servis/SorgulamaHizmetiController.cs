using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.Servis
{
    [Route("api/BYT/Servis/[controller]")]
    [ApiController]
    public class SorgulamaHizmetiController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;
        private BeyannameSonucDataContext _beyannameSonucTarihcecontext;
        private BeyannameDataContext _beyannameContext;
        private readonly ServisCredential _servisCredential;

        public SorgulamaHizmetiController(IslemTarihceDataContext islemTarihcecontext, BeyannameDataContext beyannameContex,  BeyannameSonucDataContext beyannameSonucTarihcecontext, IOptions<ServisCredential> servisCredential)
        {
            _islemTarihceContext = islemTarihcecontext;
            _beyannameSonucTarihcecontext = beyannameSonucTarihcecontext;
            _beyannameContext = beyannameContex;
            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }



        [HttpPost("{Guid}")]
        public async Task<Sonuc<ServisDurum>> GetSonuc(string Guid)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {

                SonucHizmeti.GumrukWSSoapClient sonuc = ServiceHelper.GetSonucWSClient(_servisCredential.username, _servisCredential.password);
                SonucHizmeti.ArrayOfXElement snc = new SonucHizmeti.ArrayOfXElement();

                snc = await sonuc.IslemSonucGetir4Async(Guid);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(snc.Nodes[1].FirstNode.ToString());
                string sonucXml = "", gelenXml = "", gidenXml = "";
                if (doc.HasChildNodes)
                {
                    foreach (XmlNode n in doc.ChildNodes)
                    {

                        if (n.Name == "NewDataSet")
                        {
                            sonucXml = n.InnerXml;
                            XmlDocument sncDoc = new XmlDocument();
                            sncDoc.LoadXml(sonucXml);
                            if (sncDoc.HasChildNodes)
                            {
                                foreach (XmlNode t in sncDoc.ChildNodes[0])
                                {
                                    if (t.Name == "GelenXML")
                                    {
                                        gelenXml = t.InnerText;
                                    }
                                    else if (t.Name == "GidenXML")
                                    {
                                        gidenXml = t.InnerText;
                                    }

                                }


                            }
                        }
                    }
                }

               

                using (var transaction = _islemTarihceContext.Database.BeginTransaction())
                {
                    try
                    {
                        var _tarihce = await _islemTarihceContext.Tarihce.FirstOrDefaultAsync(v => v.Guid == Guid);
                        var _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.Kullanici == _tarihce.Kullanici && v.RefId == _tarihce.RefId);
                        var _beyanname = await _beyannameContext.DbBeyan.FirstOrDefaultAsync(v => v.BeyanInternalNo == _islem.BeyanInternalNo);


                        var sonucObj= SonuclariTopla(gidenXml, Guid, _islem.IslemInternalNo);
                        // GonderilenVeriler(gelenXml);
                      

                        _tarihce.IslemDurumu = "Sonuclandi";
                        _tarihce.SonucZamani = DateTime.Now;
                        _tarihce.SonucVeri = gidenXml;
                        _tarihce.ServistekiVeri = gelenXml;
                        _tarihce.BeyanNo = sonucObj.Beyanname_no;
                        

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();

                     

                        _islem.IslemDurumu = "Sonuclandi";
                        _islem.IslemZamani = DateTime.Now;
                        _tarihce.BeyanNo = sonucObj.Beyanname_no;


                        _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();

                   
                        _beyanname.BeyannameNo = sonucObj.Beyanname_no;
                        if(!string.IsNullOrWhiteSpace(sonucObj.Tescil_tarihi))
                        _beyanname.TescilTarihi =   Convert.ToDateTime(sonucObj.Tescil_tarihi) ;
                        _beyanname.TescilStatu ="Tescil Edildi";

                        _beyannameContext.Entry(_beyanname).State = EntityState.Modified;
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
                        return rresult;
                    }
                }

             

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                List<Internal.Hata> lstht = new List<Internal.Hata>();

                //if (IslemDurumu == "Hata")
                //{
                //    Hata ht = new Hata { HataKodu = 1, HataAciklamasi = islemSonucu };
                //    lstht.Add(ht);
                //}

                Bilgi blg = new Bilgi { IslemTipi = "Sonuç Sorgulama", ReferansNo = Guid, Sonuc = "Sorgulama Başarılı", SonucVeriler = gidenXml };
                lstBlg.Add(blg);


                _servisDurum.Bilgiler = lstBlg;
                _servisDurum.Hatalar = lstht;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirildi" };


                return result;


            }
            catch (Exception ex)
            {

                return null;
            }


        }

       private BeyannameSonuc SonuclariTopla(string XML, string GuidOf, string InternalNo)
        {

            XmlDocument xd = new XmlDocument();
            BeyannameSonuc sonucObj = new BeyannameSonuc();

            try
            {
          
                xd.LoadXml(XML);

                XmlNodeList XmlNodeListObjhata = xd.GetElementsByTagName("Error");
                if (XmlNodeListObjhata[0] != null)
                {
                    // XmlNodeListObjhata[0].ChildNodes[0].Value;
                    return sonucObj;
                }
                sonucObj.SonucXml = XML;
                XmlNodeList XmlNodeListObj1 = xd.GetElementsByTagName("Beyanname_no");
                XmlNodeList XmlNodeListObj2 = xd.GetElementsByTagName("Tip");
                XmlNodeList XmlNodeListObj3 = xd.GetElementsByTagName("Tescil_tarihi");
                XmlNodeList XMLhatalar = xd.GetElementsByTagName("Hatalar");
                XmlNodeList XMLsorular = xd.GetElementsByTagName("Sorular");
                XmlNodeList XMLbelgeler = xd.GetElementsByTagName("Belgeler");
                XmlNodeList XMLvergiler = xd.GetElementsByTagName("Vergiler");
                XmlNodeList XMLsorularacevap = xd.GetElementsByTagName("Soru_cevap");
                XmlNodeList XMLtoplamvergiler = xd.GetElementsByTagName("Toplam_vergiler");
                XmlNodeList XMLtoplananvergiler = xd.GetElementsByTagName("Toplanan_vergiler");
                XmlNodeList XMLhesapdetaylari = xd.GetElementsByTagName("Hesap_detaylari");
                XmlNodeList XMLozetbeyanlar = xd.GetElementsByTagName("Ozetbeyan_bilgi");
                XmlNodeList XMLgumrukkiymetleri = xd.GetElementsByTagName("Gumruk_kiymetleri");
                XmlNodeList XMListatistikikiymetler = xd.GetElementsByTagName("Istatistiki_kiymetleri");
                XmlNodeList XMLdovizkurualis = xd.GetElementsByTagName("Doviz_kuru_alis");
                XmlNodeList XMLdovizkurusatis = xd.GetElementsByTagName("Doviz_kuru_satis");
                XmlNodeList XMKontrolkodu = xd.GetElementsByTagName("Cikti_kontrol_kodu");
                XmlNodeList XMMuayeneMemuru = xd.GetElementsByTagName("Muayene_memuru");
                XmlNodeList XMMLKalanKontor = xd.GetElementsByTagName("KalanKontor");

                if (XmlNodeListObj1[0] != null)
                {
                    if (XmlNodeListObj1[0].ChildNodes[0] != null)
                        sonucObj.Beyanname_no = XmlNodeListObj1[0].ChildNodes[0].Value;

                }

                if (XmlNodeListObj2[0] != null)
                {
                    if (XmlNodeListObj2[0].ChildNodes[0] != null)
                        sonucObj.Tip = XmlNodeListObj2[0].ChildNodes[0].Value;
                }
                if (XmlNodeListObj3[0] != null)
                {
                    if (XmlNodeListObj3[0].ChildNodes[0] != null)
                        sonucObj.Tescil_tarihi = XmlNodeListObj3[0].ChildNodes[0].Value;
                }

                #region Hatalar

                if (XMLhatalar[0].ChildNodes.Count > 0)
                {
                    XmlNodeList xHata = XMLhatalar[0].ChildNodes;

                    List<HataMesaji> shatalar = new List<HataMesaji>();
                   
                    int hata_kodu;
                    string hata_aciklamasi;
                    foreach (XmlNode var in xHata)
                    {
                        hata_kodu = !string.IsNullOrWhiteSpace(var.ChildNodes[0].InnerText) ? Convert.ToInt32(var.ChildNodes[0].InnerText):0;
                        hata_aciklamasi = var.ChildNodes[1].InnerText;

                        HataMesaji hataObj = new HataMesaji();
                        hataObj.HataKodu = hata_kodu;
                        hataObj.HataAciklamasi = hata_aciklamasi;

                        shatalar.Add(hataObj);

                        DbSonucHatalar _hata = new DbSonucHatalar();
                     
                        _hata.Guid = GuidOf;
                        _hata.IslemInternalNo = InternalNo;
                        _hata.HataKodu = hata_kodu;
                        _hata.HataAciklamasi = hata_aciklamasi;
                      

                        _beyannameSonucTarihcecontext.Entry(_hata).State = EntityState.Added;
                      

                    }
                    _beyannameSonucTarihcecontext.SaveChangesAsync();
                    sonucObj.Hatalar = shatalar;
                 
                   
                   
                    
                   

                }

                #endregion

                else
                {

                    #region Sorular
                    if (XMLsorular[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xSoru = XMLsorular[0].ChildNodes;
                        List<Soru> ssorular = new List<Soru>();
                      
                        string Aciklama;
                        string Kod;
                        string Tip;
                        int Kalem_no;
                        foreach (XmlNode var in xSoru)
                        {
                            Kod = var.ChildNodes[0].InnerText;
                            Aciklama = var.ChildNodes[1].InnerText;
                            Kalem_no = Convert.ToInt32(var.ChildNodes[2].InnerText);
                            Tip = var.ChildNodes[3].InnerText;
                            string a = var.ChildNodes[2].InnerText;
                            XmlNode cevaplar = var.ChildNodes[4];
                            XmlNode evetler = var.ChildNodes[4].ChildNodes[0];
                            XmlNode hayirlar = var.ChildNodes[4].ChildNodes[1];

                            Cevaplar lstCvp = new Cevaplar();

                            if (cevaplar.ChildNodes[0] != null)
                            {

                                if (evetler.ChildNodes[0] != null)
                                {
                                    //      for (int j = 0; j < evetler; j++)
                                    foreach (XmlNode lstevetler in evetler)
                                    {
                                        //  XmlNodeList varlstevetler = lstevetler.ChildNodes;
                                        Evet lstevet = new Evet();
                                        string evet_sira = lstevetler.ChildNodes[0].InnerText;
                                        string evet_kodu = lstevetler.ChildNodes[1].InnerText;
                                        string evet_aciklamasi = lstevetler.ChildNodes[2].InnerText;
                                        lstevet.Kodu = evet_kodu;
                                        lstevet.Sira = evet_sira;
                                        lstevet.Aciklamasi = evet_aciklamasi;

                                        lstCvp.Evetler.Add(lstevet);
                                                   }
                                }
                            }
                            if (cevaplar.ChildNodes[1] != null)
                            {
                                if (hayirlar.ChildNodes[0] != null)
                                {
                                    //  XmlNodeList lsthayirlar = hayirlar.ChildNodes[1].ChildNodes;
                                    //    for (int j = 0; j < hayirlar.Count; j++)
                                    foreach (XmlNode lsthayirlar in hayirlar)
                                    {
                                        Hayir lsthayir = new Hayir();
                                        string hayir_sira = lsthayirlar.ChildNodes[0].InnerText;
                                        string hayir_kodu = lsthayirlar.ChildNodes[1].InnerText;
                                        string hayir_aciklamasi = lsthayirlar.ChildNodes[2].InnerText;
                                        lsthayir.Kodu = hayir_kodu;
                                        lsthayir.Sira = hayir_sira;
                                        lsthayir.Aciklamasi = hayir_aciklamasi;
                                        lstCvp.Hayirlar.Add(lsthayir);
                                     }
                                }
                            }


                            Soru soruObj = new Soru();
                            soruObj.Aciklama = Aciklama;
                            soruObj.Kalem_no = Kalem_no;
                            soruObj.Kod = Kod;
                            soruObj.Tip = Tip;
                            soruObj.Cevaplar = (lstCvp);
                            ssorular.Add(soruObj);

                            DbSonucSorular _soru = new DbSonucSorular();
                            _soru.Guid = GuidOf;
                            _soru.IslemInternalNo = InternalNo;
                            _soru.KalemNo = Kalem_no;
                            _soru.SoruKodu = Kod;
                            _soru.SoruAciklamasi = Aciklama;
                            _soru.Tip = Tip;
                            _soru.Cevaplar = lstCvp.ToString();
                          

                            _beyannameSonucTarihcecontext.Entry(_soru).State = EntityState.Added;
                           

                        }
                        _beyannameSonucTarihcecontext.SaveChangesAsync();
                        sonucObj.Sorular = ssorular;
                    }

                    #endregion

                    #region Soru Cevap
                    {
                        if (XMLsorularacevap[0].ChildNodes != null)
                        {
                            if (XMLsorularacevap[0].ChildNodes.Count > 0)
                            {
                                XmlNodeList xSrCvp = XMLsorularacevap[0].ChildNodes;

                                List<Soru_Cevap> sorucevap = new List<Soru_Cevap>();
                                string soru_no;
                                string cevap;
                                int Kalem_no;

                                foreach (XmlNode var in xSrCvp)
                                {
                                    Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                                    soru_no = var.ChildNodes[1].InnerText;
                                    cevap = var.ChildNodes[2].InnerText;



                                    Soru_Cevap sorucevapObj = new Soru_Cevap();
                                    sorucevapObj.Soru_no = soru_no;
                                    sorucevapObj.Cevap = cevap;
                                    sorucevapObj.Kalem_no = Kalem_no;

                                    sorucevap.Add(sorucevapObj);


                                    DbSonucSoruCevaplar _soruCevap = new DbSonucSoruCevaplar();
                                    _soruCevap.Guid = GuidOf;
                                    _soruCevap.IslemInternalNo = InternalNo;
                                    _soruCevap.KalemNo = Kalem_no;
                                    _soruCevap.SoruKodu = soru_no;
                                    _soruCevap.Cevap = cevap;
                                  
                                   

                                    _beyannameSonucTarihcecontext.Entry(_soruCevap).State = EntityState.Added;
                                 
                                }
                                _beyannameSonucTarihcecontext.SaveChangesAsync();
                                sonucObj.Soru_cevap = sorucevap;
                            }
                        }
                        
                    }

                    #endregion

                    #region Belgeler
                    {
                        if (XMLbelgeler[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xBelge = XMLbelgeler[0].ChildNodes;

                            List<Belge> sBelgeler = new List<Belge>();
                            string Aciklama;
                            string Dogrulama;
                            string Kod;
                            string Referans;
                            string Tamamlama_tarih;
                            int Kalem_no;
                            foreach (XmlNode var in xBelge)
                            {
                                Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                                Kod = var.ChildNodes[1].InnerText;
                                Aciklama = var.ChildNodes[2].InnerText;
                                Dogrulama = var.ChildNodes[3].InnerText;
                                Referans = var.ChildNodes[4].InnerText;
                                Tamamlama_tarih = var.ChildNodes[5].InnerText;

                                Belge belgeObj = new Belge();
                                belgeObj.Aciklama = Aciklama;
                                belgeObj.Dogrulama = Dogrulama;
                                belgeObj.Kalem_no = Kalem_no;
                                belgeObj.Kod = Kod;
                                belgeObj.Referans = Referans;
                                belgeObj.Belge_tarihi = Tamamlama_tarih;

                                sBelgeler.Add(belgeObj);

                                DbSonucBelgeler _belge = new DbSonucBelgeler();
                                _belge.Guid = GuidOf;
                                _belge.IslemInternalNo = InternalNo;
                                _belge.KalemNo = Kalem_no;
                                _belge.BelgeKodu = Kod;
                                _belge.BelgeAciklamasi = Aciklama;
                                _belge.Dogrulama = Dogrulama;
                                _belge.Referans = Referans;
                                _belge.BelgeTarihi = Tamamlama_tarih;
                              
                                _beyannameSonucTarihcecontext.Entry(_belge).State = EntityState.Added;
                                
                               
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Belgeler = sBelgeler;
                        }
                     
                    }

                    #endregion

                    #region Vergiler
                    {
                        if (XMLvergiler[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xVergi = XMLvergiler[0].ChildNodes;

                            List<Vergi> sVergiler = new List<Vergi>();
                            string Aciklama;
                            string Kod;
                            string Miktar;
                            string Odeme_sekli;
                            string Oran;
                            string Matrah;
                            int Kalem_no;

                            foreach (XmlNode var in xVergi)
                            {
                                Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                                Kod = var.ChildNodes[1].InnerText;
                                Aciklama = var.ChildNodes[2].InnerText;
                                Miktar = var.ChildNodes[3].InnerText;
                                Oran = var.ChildNodes[4].InnerText;
                                Odeme_sekli = var.ChildNodes[5].InnerText;
                                Matrah = var.ChildNodes[6].InnerText;

                                Vergi vergiObj = new Vergi();
                                vergiObj.Aciklama = Aciklama;
                                vergiObj.Kod = Kod;
                                vergiObj.Kalem_no = Kalem_no;
                                vergiObj.Miktar = Miktar.Replace(".", ",");
                                vergiObj.Odeme_sekli = Odeme_sekli;
                                vergiObj.Oran = Oran;
                                vergiObj.Matrah = Matrah;

                                sVergiler.Add(vergiObj);

                                DbSonucVergiler _vergi = new DbSonucVergiler();
                                _vergi.Guid = GuidOf;
                                _vergi.IslemInternalNo = InternalNo;
                                _vergi.KalemNo = Kalem_no;
                                _vergi.VergiKodu = Kod;
                                _vergi.VergiAciklamasi = Aciklama;
                                _vergi.Miktar = Miktar;
                                _vergi.OdemeSekli = Odeme_sekli;
                                _vergi.Oran = Oran;
                                _vergi.Matrah = Matrah;

                                _beyannameSonucTarihcecontext.Entry(_vergi).State = EntityState.Added;
                               
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Vergiler = sVergiler;
                        }
                       
                    }

                    #endregion

                    #region Toplam Vergiler
                    {
                        if (XMLtoplamvergiler[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xtVergi = XMLtoplamvergiler[0].ChildNodes;

                            List<Toplam_Vergi> stVergiler = new List<Toplam_Vergi>();
                            string Aciklama;
                            string Kod;
                            string Miktar;
                            string Odeme_sekli;


                            foreach (XmlNode var in xtVergi)
                            {
                                Kod = var.ChildNodes[0].InnerText;
                                Aciklama = var.ChildNodes[1].InnerText;
                                Miktar = var.ChildNodes[2].InnerText;
                                Odeme_sekli = var.ChildNodes[3].InnerText;

                                Toplam_Vergi tvergiObj = new Toplam_Vergi();
                                tvergiObj.Aciklama = Aciklama;
                                tvergiObj.Kod = Kod;
                                tvergiObj.Miktar = Miktar;
                                tvergiObj.Odeme_sekli = Odeme_sekli;

                                stVergiler.Add(tvergiObj);

                                DbSonucToplamVergiler _tvergi = new DbSonucToplamVergiler();
                                _tvergi.Guid = GuidOf;
                                _tvergi.IslemInternalNo = InternalNo;
                                _tvergi.VergiKodu = Kod;
                                _tvergi.VergiAciklamasi = Aciklama;
                                _tvergi.Miktar = Miktar;
                                _tvergi.OdemeSekli = Odeme_sekli;
                        
                                _beyannameSonucTarihcecontext.Entry(_tvergi).State = EntityState.Added;
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Toplam_vergiler = stVergiler;
                        }
                    }

                    #endregion

                    #region Toplanan Vergiler
                    {
                        if (XMLtoplamvergiler[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xtoplananVergi = XMLtoplananvergiler[0].ChildNodes;

                            List<Toplanan_Vergi> stoplananVergiler = new List<Toplanan_Vergi>();

                            string Miktar;
                            string Odeme_sekli;


                            foreach (XmlNode var in xtoplananVergi)
                            {

                                Odeme_sekli = var.ChildNodes[1].InnerText;
                                Miktar = var.ChildNodes[0].InnerText;

                                Toplanan_Vergi toplananvergiObj = new Toplanan_Vergi();

                                toplananvergiObj.Miktar = Miktar;
                                toplananvergiObj.Odeme_sekli = Odeme_sekli;

                                stoplananVergiler.Add(toplananvergiObj);

                                DbSonucToplananVergiler _ttvergi = new DbSonucToplananVergiler();
                                _ttvergi.Guid = GuidOf;
                                _ttvergi.IslemInternalNo = InternalNo;
                                _ttvergi.Miktar = Miktar;
                                _ttvergi.OdemeSekli = Odeme_sekli;

                                _beyannameSonucTarihcecontext.Entry(_ttvergi).State = EntityState.Added;
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Toplanan_vergiler = stoplananVergiler;
                        }
                    }

                    #endregion

                    #region Hesap Detayları
                    {
                        if (XMLhesapdetaylari[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xtHesap = XMLhesapdetaylari[0].ChildNodes;

                            List<Hesap_detay> stHesap = new List<Hesap_detay>();
                            string Aciklama;
                            string Miktar;


                            foreach (XmlNode var in xtHesap)
                            {

                                Aciklama = var.ChildNodes[0].InnerText;
                                Miktar = var.ChildNodes[1].InnerText;

                                Hesap_detay thesapObj = new Hesap_detay();
                                thesapObj.Aciklama = Aciklama;
                                thesapObj.Miktar = Miktar;

                                stHesap.Add(thesapObj);

                                DbSonucHesapDetaylar _hesap = new DbSonucHesapDetaylar();
                                _hesap.Guid = GuidOf;
                                _hesap.IslemInternalNo = InternalNo;
                                _hesap.Miktar = Miktar;
                                _hesap.Aciklama = Aciklama;


                                _beyannameSonucTarihcecontext.Entry(_hesap).State = EntityState.Added;
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Hesap_detaylari = stHesap;
                        }
                    }
                    #endregion

                    #region Gümrük Kıymeti
                    {
                        if (XMLgumrukkiymetleri[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xtGKiymet = XMLgumrukkiymetleri[0].ChildNodes;

                            List<Gumruk_Kiymeti> stGKiymet = new List<Gumruk_Kiymeti>();
                            int Kalem_No;
                            string Miktar;


                            foreach (XmlNode var in xtGKiymet)
                            {

                                Kalem_No = Convert.ToInt16(var.ChildNodes[0].InnerText);
                                Miktar = var.ChildNodes[1].InnerText;

                                Gumruk_Kiymeti tgkiymetObj = new Gumruk_Kiymeti();
                                tgkiymetObj.Kalem_no = Kalem_No;
                                tgkiymetObj.Miktar = Miktar;

                                stGKiymet.Add(tgkiymetObj);

                                DbSonucGumrukKiymeti _kiymet = new DbSonucGumrukKiymeti();
                                _kiymet.Guid = GuidOf;
                                _kiymet.IslemInternalNo = InternalNo;
                                _kiymet.KalemNo = Kalem_No;
                                _kiymet.Miktar = Miktar;
                              
                                _beyannameSonucTarihcecontext.Entry(_kiymet).State = EntityState.Added;
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Gumruk_kiymetleri = stGKiymet;
                        }
                    }

                    #endregion

                    #region İstatistiki Kıymeti
                    {
                        if (XMListatistikikiymetler[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xtIKiymet = XMListatistikikiymetler[0].ChildNodes;

                            List<Istatistiki_Kiymeti> stIKiymet = new List<Istatistiki_Kiymeti>();
                            int Kalem_No;
                            string Miktar;


                            foreach (XmlNode var in xtIKiymet)
                            {

                                Kalem_No = Convert.ToInt16(var.ChildNodes[0].InnerText);
                                Miktar = var.ChildNodes[1].InnerText;

                                Istatistiki_Kiymeti tikiymetObj = new Istatistiki_Kiymeti();
                                tikiymetObj.Kalem_no = Kalem_No;
                                tikiymetObj.Miktar = Miktar;

                                stIKiymet.Add(tikiymetObj);

                                DbSonucIstatistikiKiymeti _istk = new DbSonucIstatistikiKiymeti();
                                _istk.Guid = GuidOf;
                                _istk.IslemInternalNo = InternalNo;
                                _istk.KalemNo = Kalem_No;
                                _istk.Miktar = Miktar;

                                _beyannameSonucTarihcecontext.Entry(_istk).State = EntityState.Added;
                            }
                            _beyannameSonucTarihcecontext.SaveChangesAsync();
                            sonucObj.Istatistiki_kiymetleri = stIKiymet;
                        }
                    }

                    #endregion

                    #region Özet Beyanlar

                    if (XMLozetbeyanlar[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xOzet = XMLozetbeyanlar[0].ChildNodes;

                        List<ozetbeyantescilbilgi> sozbler = new List<ozetbeyantescilbilgi>();
                        string ozetbeyanno;
                        string tesciltarihi;
                        foreach (XmlNode var in xOzet)
                        {
                            ozetbeyanno = var.ChildNodes[0].InnerText;
                            tesciltarihi = var.ChildNodes[1].InnerText;

                            ozetbeyantescilbilgi ozbyObj = new ozetbeyantescilbilgi();
                            ozbyObj.Ozetbeyan_no = ozetbeyanno;
                            ozbyObj.Tescil_tarihi = tesciltarihi;

                            sozbler.Add(ozbyObj);

                            DbSonucOzetBeyan _ozetB = new DbSonucOzetBeyan();
                            _ozetB.Guid = GuidOf;
                            _ozetB.IslemInternalNo = InternalNo;
                            _ozetB.OzetBeyanNo = ozetbeyanno;
                            _ozetB.TescilTarihi = tesciltarihi;

                            _beyannameSonucTarihcecontext.Entry(_ozetB).State = EntityState.Added;
                        }
                        _beyannameSonucTarihcecontext.SaveChangesAsync();
                        sonucObj.Ozetbeyan_bilgi = sozbler;
                    }

                    #endregion


                }

                //Diğer Bilgiler
                if (XMLdovizkurualis[0] != null)
                {
                    if (XMLdovizkurualis[0].ChildNodes.Count > 0)
                        sonucObj.Doviz_kuru_alis = XMLdovizkurualis[0].ChildNodes[0].Value;
                }
                if (XMLdovizkurusatis[0] != null)
                {
                    if (XMLdovizkurusatis[0].ChildNodes.Count > 0)
                        sonucObj.Doviz_kuru_satis = XMLdovizkurusatis[0].ChildNodes[0].Value;
                }
                if (XMKontrolkodu[0] != null)
                {
                    if (XMKontrolkodu[0].ChildNodes.Count > 0)
                        sonucObj.Cikti_kontrol_kodu = XMKontrolkodu[0].ChildNodes[0].Value;
                }
                
                if (XMMuayeneMemuru[0] != null)
                {
                    if (XMMuayeneMemuru[0].ChildNodes.Count > 0)
                        sonucObj.Muayene_memuru = XMMuayeneMemuru[0].ChildNodes[0].Value;
                }

                if (XMMLKalanKontor[0] != null)
                {
                    if (XMMLKalanKontor[0].ChildNodes.Count > 0)
                        sonucObj.Kalan_kontor = XMMLKalanKontor[0].ChildNodes[0].Value;
                }

                

                DbSonucDigerBilgiler _digerB = new DbSonucDigerBilgiler();
                _digerB.Guid = GuidOf;
                _digerB.IslemInternalNo = InternalNo;
                _digerB.DovizKuruAlis = sonucObj.Doviz_kuru_alis;
                _digerB.DovizKuruSatis = sonucObj.Doviz_kuru_satis;
                _digerB.CiktiSeriNo = sonucObj.Cikti_kontrol_kodu;
                _digerB.KalanKontor = sonucObj.Kalan_kontor;
                _digerB.MuayeneMemuru = sonucObj.Muayene_memuru;

                _beyannameSonucTarihcecontext.Entry(_digerB).State = EntityState.Added;
                _beyannameSonucTarihcecontext.SaveChangesAsync();

                return sonucObj;
            }
            catch (Exception EX)
            {

                return sonucObj;
            }


        }
         void GonderilenVeriler(string gXML)
        {
            List<Soru_Cevap> cevaptanGelenSoruCevap;
            List<Belge> cevaptanGelenBelgeler;
            List<Vergi> cevaptanGelenVergiler;
            string islemTipi, islemSonucu;
            string sonuc, sonucHata, beyannameNo, GuID, password;
            string cevaptanGelenBeyannameNo, cevaptanGelenTeslimTarihi, cevaptanGelenTescilTarihi, cevaptanGelenHatSonucu, cevaptanGelenMusavirRef, refid, cevaptanGelenRejim;
            string cevaptanGelenGINSonucu;
            List<object>[] cevaptanGelenTeminat, cevaptanGelenFirma, cevaptanGelenOZBY, cevaptanGelenKalemler, cevaptanGelenKiymet;
            XmlDocument dd = new XmlDocument();


            KontrolHizmeti.BeyannameBilgi mynewtcgb = new KontrolHizmeti.BeyannameBilgi();

            dd.LoadXml(gXML);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR", false);

            #region Ana Bilgiler

            XmlNodeList XmlNodeListObj2 = dd.GetElementsByTagName("GUMRUK");
            //if (XmlNodeListObj2 != null)
            //    if (XmlNodeListObj2.Count > 0)
            //        if (XmlNodeListObj2[0].ChildNodes[0].Value != Session["gumruk"].ToString())
            //        {
            //            MessageBox.Show("İşlem Yaptığınız Gümrük Yanlış,Lütfen Giriş Yaptığınız Gümrüğü Seçiniz...");
            //            return;
            //        }
            if (XmlNodeListObj2[0] != null)
                if (XmlNodeListObj2[0].ChildNodes[0] != null)
                    mynewtcgb.GUMRUK = XmlNodeListObj2[0].ChildNodes[0].Value;

            XmlNodeList XmlNodeListObj61 = dd.GetElementsByTagName("Birlik_kayit_numarasi");
            if (XmlNodeListObj61[0] != null)
                if (XmlNodeListObj61[0].ChildNodes[0] != null)
                    mynewtcgb.Birlik_kayit_numarasi = XmlNodeListObj61[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj62 = dd.GetElementsByTagName("Birlik_kripto_numarasi");
            if (XmlNodeListObj62[0] != null)
                if (XmlNodeListObj62[0].ChildNodes[0] != null)
                    mynewtcgb.Birlik_kripto_numarasi = XmlNodeListObj62[0].ChildNodes[0].Value;
            //XmlNodeList XmlNodeListObj63 = dd.GetElementsByTagName("Onceki_Gecici_BeyannameNo");
            //if (XmlNodeListObj63[0] != null)
            //    if (XmlNodeListObj63[0].ChildNodes[0] != null)
            //        if (cevaptanGelenBeyannameNo != null)
            //            mynewtcgb.Onceki_Gecici_BeyannameNo = cevaptanGelenBeyannameNo;

            XmlNodeList XmlNodeListObj64 = dd.GetElementsByTagName("Musavir_referansi");
            if (XmlNodeListObj64[0] != null)
                if (XmlNodeListObj64[0].ChildNodes[0] != null)
                    mynewtcgb.Musavir_referansi = XmlNodeListObj64[0].ChildNodes[0].Value;


            if (XmlNodeListObj2[0] != null)
                if (XmlNodeListObj2[0].ChildNodes[0] != null)
                    mynewtcgb.GUMRUK = XmlNodeListObj2[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj1 = dd.GetElementsByTagName("Rejim");
            if (XmlNodeListObj1[0] != null)
                if (XmlNodeListObj1[0].ChildNodes[0] != null)
                    mynewtcgb.Rejim = XmlNodeListObj1[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj3 = dd.GetElementsByTagName("Basitlestirilmis_usul");
            if (XmlNodeListObj3[0] != null)
                if (XmlNodeListObj3[0].ChildNodes[0] != null)
                    mynewtcgb.Basitlestirilmis_usul = XmlNodeListObj3[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj4 = dd.GetElementsByTagName("Yuk_belgeleri_sayisi");
            if (XmlNodeListObj4[0] != null)
                mynewtcgb.Yuk_belgeleri_sayisi = Convert.ToInt32(XmlNodeListObj4[0].ChildNodes[0].Value);
            XmlNodeList XmlNodeListObj5 = dd.GetElementsByTagName("Kap_adedi");
            if (XmlNodeListObj5[0] != null)
                mynewtcgb.Kap_adedi = Convert.ToInt16(XmlNodeListObj5[0].ChildNodes[0].Value);
            XmlNodeList XmlNodeListObj6 = dd.GetElementsByTagName("Ticaret_ulkesi");
            if (XmlNodeListObj6[0] != null)
                if (XmlNodeListObj6[0].ChildNodes[0] != null)
                    mynewtcgb.Ticaret_ulkesi = XmlNodeListObj6[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj7 = dd.GetElementsByTagName("Referans_no");
            if (XmlNodeListObj7[0] != null)
                if (XmlNodeListObj7[0].ChildNodes[0] != null)
                    mynewtcgb.Referans_no = XmlNodeListObj7[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj8 = dd.GetElementsByTagName("Cikis_ulkesi");
            if (XmlNodeListObj8[0] != null)
                if (XmlNodeListObj8[0].ChildNodes[0] != null)
                    mynewtcgb.Cikis_ulkesi = XmlNodeListObj8[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj9 = dd.GetElementsByTagName("Gidecegi_ulke");
            if (XmlNodeListObj9[0] != null)
                if (XmlNodeListObj9[0].ChildNodes[0] != null)
                    mynewtcgb.Gidecegi_ulke = XmlNodeListObj9[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj10 = dd.GetElementsByTagName("Gidecegi_sevk_ulkesi");
            if (XmlNodeListObj10[0] != null)
                if (XmlNodeListObj10[0].ChildNodes[0] != null)
                    mynewtcgb.Gidecegi_sevk_ulkesi = XmlNodeListObj10[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj11 = dd.GetElementsByTagName("Cikistaki_aracin_tipi");
            if (XmlNodeListObj11[0] != null)
                if (XmlNodeListObj11[0].ChildNodes[0] != null)
                    mynewtcgb.Cikistaki_aracin_tipi = XmlNodeListObj11[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj12 = dd.GetElementsByTagName("Cikistaki_aracin_kimligi");
            if (XmlNodeListObj12[0] != null)
                if (XmlNodeListObj12[0].ChildNodes[0] != null)
                    mynewtcgb.Cikistaki_aracin_kimligi = XmlNodeListObj12[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj13 = dd.GetElementsByTagName("Cikistaki_aracin_ulkesi");
            if (XmlNodeListObj13[0] != null)
                if (XmlNodeListObj13[0].ChildNodes[0] != null)
                    mynewtcgb.Cikistaki_aracin_ulkesi = XmlNodeListObj13[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj14 = dd.GetElementsByTagName("Teslim_sekli");
            if (XmlNodeListObj14[0] != null)
                if (XmlNodeListObj14[0].ChildNodes[0] != null)
                    mynewtcgb.Teslim_sekli = XmlNodeListObj14[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj15 = dd.GetElementsByTagName("Teslim_yeri");
            if (XmlNodeListObj15[0] != null)
                if (XmlNodeListObj15[0].ChildNodes[0] != null)
                    mynewtcgb.Teslim_yeri = XmlNodeListObj15[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj16 = dd.GetElementsByTagName("Konteyner");
            if (XmlNodeListObj16[0] != null)
                if (XmlNodeListObj16[0].ChildNodes[0] != null)
                    mynewtcgb.Konteyner = XmlNodeListObj16[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj17 = dd.GetElementsByTagName("Sinirdaki_aracin_tipi");
            if (XmlNodeListObj17[0] != null)
                if (XmlNodeListObj17[0].ChildNodes[0] != null)
                    mynewtcgb.Sinirdaki_aracin_tipi = XmlNodeListObj17[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj18 = dd.GetElementsByTagName("Sinirdaki_aracin_kimligi");
            if (XmlNodeListObj18[0] != null)
                if (XmlNodeListObj18[0].ChildNodes[0] != null)
                    mynewtcgb.Sinirdaki_aracin_kimligi = XmlNodeListObj18[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj19 = dd.GetElementsByTagName("Sinirdaki_aracin_ulkesi");
            if (XmlNodeListObj19[0] != null)
                if (XmlNodeListObj19[0].ChildNodes[0] != null)
                    mynewtcgb.Sinirdaki_aracin_ulkesi = XmlNodeListObj19[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj20 = dd.GetElementsByTagName("Toplam_fatura");
            if (XmlNodeListObj20[0] != null)
                if (XmlNodeListObj20[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_fatura = Convert.ToDecimal(XmlNodeListObj20[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            XmlNodeList XmlNodeListObj21 = dd.GetElementsByTagName("Toplam_fatura_dovizi");
            if (XmlNodeListObj21[0] != null)
                if (XmlNodeListObj21[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_fatura_dovizi = XmlNodeListObj21[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj22 = dd.GetElementsByTagName("Toplam_navlun");
            if (XmlNodeListObj22[0] != null)
                if (XmlNodeListObj22[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_navlun = Convert.ToDecimal(XmlNodeListObj22[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            XmlNodeList XmlNodeListObj23 = dd.GetElementsByTagName("Toplan_navlun_dovizi");
            if (XmlNodeListObj23[0] != null)
                if (XmlNodeListObj23[0].ChildNodes[0] != null)
                    mynewtcgb.Toplan_navlun_dovizi = XmlNodeListObj23[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj24 = dd.GetElementsByTagName("Sinirdaki_tasima_sekli");
            if (XmlNodeListObj24[0] != null)
                if (XmlNodeListObj24[0].ChildNodes[0] != null)
                    mynewtcgb.Sinirdaki_tasima_sekli = XmlNodeListObj24[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj25 = dd.GetElementsByTagName("Alici_satici_iliskisi");
            if (XmlNodeListObj25[0] != null)
                if (XmlNodeListObj25[0].ChildNodes[0] != null)
                    mynewtcgb.Alici_satici_iliskisi = XmlNodeListObj25[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj26 = dd.GetElementsByTagName("Toplam_sigorta");
            if (XmlNodeListObj26[0] != null)
                if (XmlNodeListObj26[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_sigorta = Convert.ToDecimal(XmlNodeListObj26[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            XmlNodeList XmlNodeListObj27 = dd.GetElementsByTagName("Toplam_sigorta_dovizi");
            if (XmlNodeListObj27[0] != null)
                if (XmlNodeListObj27[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_sigorta_dovizi = XmlNodeListObj27[0].ChildNodes[0].Value;

            XmlNodeList XmlNodeListObj28 = dd.GetElementsByTagName("Yukleme_bosaltma_yeri");
            if (XmlNodeListObj28[0] != null)
                if (XmlNodeListObj28[0].ChildNodes[0] != null)
                    mynewtcgb.Yukleme_bosaltma_yeri = XmlNodeListObj28[0].ChildNodes[0].Value;

            XmlNodeList XmlNodeListObj30 = dd.GetElementsByTagName("Toplam_yurt_disi_harcamalar");
            if (XmlNodeListObj30[0] != null)
                if (XmlNodeListObj30[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_yurt_disi_harcamalar = Convert.ToDecimal(XmlNodeListObj30[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            XmlNodeList XmlNodeListObj31 = dd.GetElementsByTagName("Toplam_yurt_disi_harcamalarin_dovizi");
            if (XmlNodeListObj31[0] != null)
                if (XmlNodeListObj31[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_yurt_disi_harcamalarin_dovizi = XmlNodeListObj31[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj32 = dd.GetElementsByTagName("Toplam_yurt_ici_harcamalar");
            if (XmlNodeListObj32[0] != null)
                if (XmlNodeListObj32[0].ChildNodes[0] != null)
                    mynewtcgb.Toplam_yurt_ici_harcamalar = Convert.ToDecimal(XmlNodeListObj32[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            //XmlNodeList XmlNodeListObj33 = dd.GetElementsByTagName("Odeme_sekli");
            //if (XmlNodeListObj33[0] != null)
            //    if (XmlNodeListObj33[0].ChildNodes[0] != null)
            //        mynewtcgb.Odeme_sekli = XmlNodeListObj33[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj34 = dd.GetElementsByTagName("Banka_kodu");
            if (XmlNodeListObj34[0] != null)
                if (XmlNodeListObj34[0].ChildNodes[0] != null)
                    mynewtcgb.Banka_kodu = XmlNodeListObj34[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj35 = dd.GetElementsByTagName("Esyanin_bulundugu_yer");
            if (XmlNodeListObj35[0] != null)
                if (XmlNodeListObj35[0].ChildNodes[0] != null)
                    mynewtcgb.Esyanin_bulundugu_yer = XmlNodeListObj35[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj36 = dd.GetElementsByTagName("Varis_gumruk_idaresi");
            if (XmlNodeListObj36[0] != null)
                if (XmlNodeListObj36[0].ChildNodes[0] != null)
                    mynewtcgb.Varis_gumruk_idaresi = XmlNodeListObj36[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj37 = dd.GetElementsByTagName("Antrepo_kodu");
            if (XmlNodeListObj37[0] != null)
                if (XmlNodeListObj37[0].ChildNodes[0] != null)
                    mynewtcgb.Antrepo_kodu = XmlNodeListObj37[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj38 = dd.GetElementsByTagName("Tasarlanan_guzergah");
            if (XmlNodeListObj38[0] != null)
                if (XmlNodeListObj38[0].ChildNodes[0] != null)
                    mynewtcgb.Tasarlanan_guzergah = XmlNodeListObj38[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj39 = dd.GetElementsByTagName("Telafi_edici_vergi");
            if (XmlNodeListObj39[0] != null)
                if (XmlNodeListObj39[0].ChildNodes[0] != null)
                    mynewtcgb.Telafi_edici_vergi = Convert.ToDecimal(XmlNodeListObj39[0].ChildNodes[0].Value.ToString().Replace(".", ","));
            XmlNodeList XmlNodeListObj40 = dd.GetElementsByTagName("Giris_gumruk_idaresi");
            if (XmlNodeListObj40[0] != null)
                if (XmlNodeListObj40[0].ChildNodes[0] != null)
                    mynewtcgb.Giris_gumruk_idaresi = XmlNodeListObj40[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj41 = dd.GetElementsByTagName("Islemin_niteligi");
            if (XmlNodeListObj41[0] != null)
                if (XmlNodeListObj41[0].ChildNodes[0] != null)
                    mynewtcgb.Islemin_niteligi = XmlNodeListObj41[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj42 = dd.GetElementsByTagName("Aciklamalar");
            if (XmlNodeListObj42[0] != null)
                if (XmlNodeListObj42[0].ChildNodes[0] != null)
                    mynewtcgb.Aciklamalar = XmlNodeListObj42[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj43 = dd.GetElementsByTagName("Kullanici_kodu");
            if (XmlNodeListObj43[0] != null)
                if (XmlNodeListObj43[0].ChildNodes[0] != null)
                    mynewtcgb.Kullanici_kodu = XmlNodeListObj43[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj44 = dd.GetElementsByTagName("Referans_tarihi");
            if (XmlNodeListObj44[0] != null)
                if (XmlNodeListObj44[0].ChildNodes[0] != null)
                    mynewtcgb.Referans_tarihi = XmlNodeListObj44[0].ChildNodes[0].Value;
            if (mynewtcgb.Referans_tarihi == "  /  /" || mynewtcgb.Referans_tarihi == null)
            {
                mynewtcgb.Referans_tarihi = "";
            }
            XmlNodeList XmlNodeListObj45 = dd.GetElementsByTagName("Odeme");
            if (XmlNodeListObj45[0] != null)
                if (XmlNodeListObj45[0].ChildNodes[0] != null)
                    mynewtcgb.Odeme = XmlNodeListObj45[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj46 = dd.GetElementsByTagName("Odeme_araci");
            if (XmlNodeListObj46[0] != null)
                if (XmlNodeListObj46[0].ChildNodes[0] != null)
                    mynewtcgb.Odeme_araci = XmlNodeListObj46[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj47 = dd.GetElementsByTagName("Gonderici_vergi_no");
            if (XmlNodeListObj47[0] != null)
                if (XmlNodeListObj47[0].ChildNodes[0] != null)
                    mynewtcgb.Gonderici_vergi_no = XmlNodeListObj47[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj48 = dd.GetElementsByTagName("Alici_vergi_no");
            if (XmlNodeListObj48[0] != null)
                if (XmlNodeListObj48[0].ChildNodes[0] != null)
                    mynewtcgb.Alici_vergi_no = XmlNodeListObj48[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj49 = dd.GetElementsByTagName("Beyan_sahibi_vergi_no");
            if (XmlNodeListObj49[0] != null)
                if (XmlNodeListObj49[0].ChildNodes[0] != null)
                    mynewtcgb.Beyan_sahibi_vergi_no = XmlNodeListObj49[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj50 = dd.GetElementsByTagName("Musavir_vergi_no");
            if (XmlNodeListObj50[0] != null)
                if (XmlNodeListObj50[0].ChildNodes[0] != null)
                    mynewtcgb.Musavir_vergi_no = XmlNodeListObj50[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj51 = dd.GetElementsByTagName("Asil_sorumlu_vergi_no");
            if (XmlNodeListObj51[0] != null)
                if (XmlNodeListObj51[0].ChildNodes[0] != null)
                    mynewtcgb.Asil_sorumlu_vergi_no = XmlNodeListObj51[0].ChildNodes[0].Value;

            XmlNodeList XmlNodeListObj52 = dd.GetElementsByTagName("mobil1");
            if (XmlNodeListObj52[0] != null)
                if (XmlNodeListObj52[0].ChildNodes[0] != null)
                    mynewtcgb.mobil1 = XmlNodeListObj52[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj53 = dd.GetElementsByTagName("mobil2");
            if (XmlNodeListObj53[0] != null)
                if (XmlNodeListObj53[0].ChildNodes[0] != null)
                    mynewtcgb.mobil2 = XmlNodeListObj53[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj54 = dd.GetElementsByTagName("mail1");
            if (XmlNodeListObj54[0] != null)
                if (XmlNodeListObj54[0].ChildNodes[0] != null)
                    mynewtcgb.mail1 = XmlNodeListObj54[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj55 = dd.GetElementsByTagName("mail2");
            if (XmlNodeListObj55[0] != null)
                if (XmlNodeListObj55[0].ChildNodes[0] != null)
                    mynewtcgb.mail2 = XmlNodeListObj55[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj56 = dd.GetElementsByTagName("mail3");
            if (XmlNodeListObj56[0] != null)
                if (XmlNodeListObj56[0].ChildNodes[0] != null)
                    mynewtcgb.mail3 = XmlNodeListObj56[0].ChildNodes[0].Value;
            XmlNodeList XmlNodeListObj57 = dd.GetElementsByTagName("LimanKodu");
            if (XmlNodeListObj57[0] != null)
                if (XmlNodeListObj57[0].ChildNodes[0] != null)
                    mynewtcgb.LimanKodu = XmlNodeListObj57[0].ChildNodes[0].Value;

            cevaptanGelenRejim = mynewtcgb.Rejim;

            #endregion

            #region Teminat

            XmlNodeList XmlNodeListTemObj1 = dd.GetElementsByTagName("Teminat");
            if (XmlNodeListTemObj1[0] != null)
            {
                if (XmlNodeListTemObj1[0].ChildNodes.Count > 0)
                {
                    List<object>[] teminat;

                    teminat = new List<object>[8];
                    for (int i = 0; i < teminat.Length; i++)
                    {

                        if (teminat[i] == null) teminat[i] = new List<object>();
                    }

                    XmlNodeList xTem = XmlNodeListTemObj1[0].ChildNodes;
                    string teminat_turu;
                    string teminat_oran;
                    string Global_teminat_no;
                    string teminat_aciklama;
                    decimal Nakdi_teminat_tutari;
                    decimal Diger_tutar;
                    decimal Banka_mektubu_tutari;
                    string Diger_teminat_referansi;

                    foreach (XmlNode var in xTem)
                    {
                        teminat_turu = var["Teminat_sekli"].InnerText;
                        teminat_oran = var["Teminat_orani"].InnerText;
                        Global_teminat_no = var["Global_teminat_no"].InnerText;
                        Banka_mektubu_tutari = Convert.ToDecimal(var["Banka_mektubu_tutari"].InnerText.ToString().Replace(".", ","));
                        Nakdi_teminat_tutari = Convert.ToDecimal(var["Nakdi_teminat_tutari"].InnerText.ToString().Replace(".", ","));
                        Diger_tutar = Convert.ToDecimal(var["Diger_tutar"].InnerText.ToString().Replace(".", ","));
                        teminat_aciklama = var["Aciklama"].InnerText;
                        Diger_teminat_referansi = var["Diger_tutar_referansi"].InnerText;

                        teminat[0].Add(teminat_turu);
                        teminat[1].Add(teminat_oran);
                        teminat[2].Add(Global_teminat_no);
                        teminat[3].Add(Banka_mektubu_tutari);
                        teminat[4].Add(Nakdi_teminat_tutari);
                        teminat[5].Add(Diger_tutar);
                        teminat[6].Add(teminat_aciklama);
                        teminat[7].Add(Diger_teminat_referansi);
                    }

                    cevaptanGelenTeminat = teminat;


                }
            }

            #endregion

            #region Firma

            XmlNodeList XmlNodeListFrmObj1 = dd.GetElementsByTagName("Firma_bilgi");
            if (XmlNodeListFrmObj1[0] != null)
            {
                if (XmlNodeListFrmObj1[0].ChildNodes.Count > 0)
                {
                    List<object>[] firma;

                    firma = new List<object>[10];
                    for (int i = 0; i < firma.Length; i++)
                    {

                        if (firma[i] == null) firma[i] = new List<object>();
                    }

                    XmlNodeList xFrm = XmlNodeListFrmObj1[0].ChildNodes;

                    string adi;
                    string cadde;
                    string faks;
                    string ililce;
                    string postakod;
                    string tel;
                    string ulkekod;
                    string tip;
                    string no;
                    string kmltur;

                    foreach (XmlNode var in xFrm)
                    {
                        if (var["Adi_unvani"] != null)
                        {
                            adi = var["Adi_unvani"].InnerText;
                        }
                        else
                        {
                            adi = "";
                        }

                        if (var["Cadde_s_no"] != null)
                        {
                            cadde = var["Cadde_s_no"].InnerText;
                        }
                        else
                        {
                            cadde = "";
                        }

                        if (var["Faks"] != null)
                        {
                            faks = var["Faks"].InnerText;
                        }
                        else
                        {
                            faks = "";
                        }

                        if (var["Il_ilce"] != null)
                        {
                            ililce = var["Il_ilce"].InnerText;
                        }
                        else
                        {
                            ililce = "";
                        }

                        if (var["Posta_kodu"] != null)
                        {
                            postakod = var["Posta_kodu"].InnerText;
                        }
                        else
                        {
                            postakod = "";
                        }

                        if (var["Telefon"] != null)
                        {
                            tel = var["Telefon"].InnerText;
                        }
                        else
                        {
                            tel = "";
                        }

                        if (var["Ulke_kodu"] != null)
                        {
                            ulkekod = var["Ulke_kodu"].InnerText;
                        }
                        else
                        {
                            ulkekod = "";
                        }

                        if (var["Tip"] != null)
                        {
                            tip = var["Tip"].InnerText;
                        }
                        else
                        {
                            tip = "";
                        }

                        if (var["No"] != null)
                        {
                            no = var["No"].InnerText;
                        }
                        else
                        {
                            no = "";
                        }

                        if (var["Kimlik_turu"] != null)
                        {
                            kmltur = var["Kimlik_turu"].InnerText;
                        }
                        else
                        {
                            kmltur = "";
                        }


                        firma[0].Add(adi);
                        firma[1].Add(cadde);
                        firma[2].Add(faks);
                        firma[3].Add(ililce);
                        firma[4].Add(postakod);
                        firma[5].Add(tel);
                        firma[6].Add(ulkekod);
                        firma[7].Add(tip);
                        firma[8].Add(no);
                        firma[9].Add(kmltur);
                    }
                    cevaptanGelenFirma = firma;

                }
            }

            #endregion

            #region Özetbeyanlar
            XmlNodeList XmlNodeListOzbyObj1 = dd.GetElementsByTagName("Ozetbeyanlar");
            if (XmlNodeListOzbyObj1[0] != null)
            {

                if (XmlNodeListOzbyObj1[0].ChildNodes.Count > 0)
                {
                    List<object>[] ozby;

                    ozby = new List<object>[6];

                    for (int i = 0; i < ozby.Length; i++)
                    {

                        if (ozby[i] == null) ozby[i] = new List<object>();
                    }

                    XmlNodeList xOzet = XmlNodeListOzbyObj1[0].ChildNodes;

                    string Aciklama;
                    string Ambar_ici;
                    string Baska_rejim;
                    string Ozetbeyan_islem_kapsami;
                    string Ozetbeyan_no;


                    foreach (XmlNode var in xOzet)
                    {
                        Ozetbeyan_no = var["Ozetbeyan_no"].InnerText;
                        if (var["Ozetbeyan_islem_kapsami"].InnerText == "3")
                            Ozetbeyan_islem_kapsami = "3";
                        else Ozetbeyan_islem_kapsami = "2";
                        Ambar_ici = var["Ambar_ici"].InnerText;
                        Baska_rejim = var["Baska_rejim"].InnerText;
                        Aciklama = var["Aciklama"].InnerText;

                        ozby[0].Add(Ozetbeyan_no);
                        ozby[1].Add(Ozetbeyan_islem_kapsami);
                        ozby[2].Add(Ambar_ici);
                        ozby[3].Add(Baska_rejim);
                        ozby[4].Add(Aciklama);

                        XmlNode tasimasenetleri = var.ChildNodes[0];
                        List<object>[] acmalar;
                        acmalar = new List<object>[2];


                        if (tasimasenetleri != null)
                        {
                            for (int s = 0; s < acmalar.Length; s++)
                            {
                                if (acmalar[s] == null) acmalar[s] = new List<object>();
                            }



                            string Tasima_senedi_no = "";
                            XmlNodeList XmlNodeListTasimaSenediNo = dd.GetElementsByTagName("Tasima_senedi_no");
                            for (int i = 0; i < XmlNodeListTasimaSenediNo.Count; i++)
                            {
                                if (XmlNodeListTasimaSenediNo[i] != null)
                                {
                                    Tasima_senedi_no = XmlNodeListTasimaSenediNo[i].InnerText;
                                    acmalar[0].Add(Tasima_senedi_no);
                                }

                            }
                            foreach (XmlNode lsttasimasenetleri in tasimasenetleri)
                            {
                                XmlNode tasimasatirlari = lsttasimasenetleri.ChildNodes[1];

                                List<object>[] satirlar;
                                satirlar = new List<object>[3];
                                for (int s = 0; s < satirlar.Length; s++)
                                {
                                    if (satirlar[s] == null) satirlar[s] = new List<object>();
                                }


                                if (tasimasatirlari != null)
                                {
                                    foreach (XmlNode tasimasatir in tasimasatirlari)
                                    {
                                        if (tasimasatir.ChildNodes.Count > 0)
                                        {
                                            string Tasima_satir_no = tasimasatir["Tasima_satir_no"].InnerText;
                                            string Ambar_kodu = tasimasatir["Ambar_kodu"].InnerText;
                                            decimal Acilacak_miktar = Convert.ToDecimal(tasimasatir["Acilacak_miktar"].InnerText.ToString().Replace(".", ","));


                                            satirlar[0].Add(Acilacak_miktar);
                                            satirlar[1].Add(Ambar_kodu);
                                            satirlar[2].Add(Tasima_satir_no);
                                        }

                                    }
                                    acmalar[1].Add(satirlar);


                                }

                                else
                                {

                                }
                                ozby[5].Add(acmalar);
                            }
                        }
                    }
                    cevaptanGelenOZBY = ozby;

                }
                else
                    cevaptanGelenOZBY = null;

            }

            #endregion

            #region Kalemler

            XmlNodeList XmlNodeListKalemObj1 = dd.GetElementsByTagName("Kalemler");
            if (XmlNodeListKalemObj1[0] != null)
            {
                if (XmlNodeListKalemObj1[0].ChildNodes.Count > 0)
                {
                    List<object>[] kalemler;

                    kalemler = new List<object>[79];
                    for (int i = 0; i < kalemler.Length; i++)
                    {

                        if (kalemler[i] == null) kalemler[i] = new List<object>();
                    }



                    XmlNodeList xKalem = XmlNodeListKalemObj1[0].ChildNodes;
                    //    test.kalem[] skalemler = new test.kalem[XmlNodeListKalemObj1[0].ChildNodes.Count];

                    string Kalem_no;
                    string Uluslararasi_anlasma;// "CARTATD"
                    string Ozellik;// "CARTBED"
                    string Ek_kod;// "CARTCODADD"
                    string YurtIci_Diger_Aciklama;// "CARTDEVAJU" 
                    string Sigorta_miktarinin_dovizi;//"CARTDEVASS" 
                    string Yurt_disi_harcamalar_miktarinin_dovizi;//"CARTDEVFAE"
                    string Navlun_miktarinin_dovizi;// "CARTDEVFRT"
                    string Fatura_miktarinin_dovizi;// "CARTDEVPFN"
                    string Teslim_sekli;//"CARTNCT"
                    string Mensei_ulke;//"CARTPYSORI"
                    string Muafiyetler_1;//"CARTSRG1"
                    string Muafiyetler_2;//"CARTSRG2"
                    string Muafiyetler_3;// "CARTSRG3"
                    string Miktar_birimi;// "CARTUNTADD"
                    string Algilama_birimi_1;// "CARTUNTPER" 
                    string Algilama_birimi_2;// "CARTUNTPER2"
                    string Algilama_birimi_3;//"CARTUNTPER3"
                    string Tamamlayici_olcu_birimi;// "CARTUNTSTA"
                    string Gtip;// "CESP"
                    string Satir_no;// "LARTDOCNO"
                    string Tarifedeki_tanimi;//"LARTDESCOM1"
                    decimal Yurt_ici_harcamalar;//"MARTAJU" 
                    decimal Yurt_disi_harcamalar_miktari;//"MARTFAE"
                    decimal Fatura_miktari;// "MARTPFN" 
                    decimal Sigorta_miktari;//"MARTASS" 
                    decimal Navlun_miktari;//"MARTFRT" 
                    decimal Sinir_gecis_ucreti;// "MARTVALFRN"
                    decimal Istatistiki_kiymet;// "MARTVALSTA"
                    decimal Miktar;//"QARTADD"
                    decimal Istatistiki_miktar;//"QARTDECSTA"
                    decimal Brut_agirlik;//"QARTPDSBRT"
                    decimal Net_agirlik;// "QARTPDSNET"
                    decimal Algilama_miktari_1;//"QARTPER"
                    decimal Algilama_miktari_2;//"QARTPER2"
                    decimal Algilama_miktari_3;// "QARTPER3"
                    string Mahrece_iade;//"CARTPR1"
                    string Ikincil_islem;// "CARTPR2"
                    string Imalatci_firma_bilgisi; //"CARTAUTMAN" 
                    string Ticari_tanimi; //"LARTDESCOM2" 
                    string Marka;//LARTMARCLS
                    string konteyner_Bilgi;//LARTMARCLS
                    string Numara;//LARTNUMCLS
                    string Cinsi;//CARTNNATCLS
                    decimal Adedi;//NARTNNBRCLS
                    string Kdv_orani;
                    string Kullanilmis;
                    string aciklama44 = "";
                    string imFirmaVergiNo;
                    decimal Yurtici_Banka;//"txtmartfrbnq" 
                    decimal Yurtici_Cevre;//"txtmartconaen"
                    decimal Yurtici_Depolama;// "txtmartfrsto" 
                    decimal Yurtici_Diger;//"txtmartaut" 
                    decimal Yurtici_Kkdf;//"txtmartfsur" 
                    decimal Yurtici_Kultur;// "txtmartmincul"
                    decimal Yurtici_Liman;// "txtmartfrpor"
                    decimal Yurtici_Tahliye;//"txtmartfreva"

                    string Muafiyet_Aciklamasi;

                    decimal YurtDisi_Komisyon;
                    decimal YurtDisi_Depolama;
                    decimal YurtDisi_Royalti;
                    decimal YurtDisi_Banka;
                    decimal YurtDisi_Diger;

                    string YurtDisi_Komisyon_Dovizi;
                    string YurtDisi_Depolama_Dovizi;
                    string YurtDisi_Royalti_Dovizi;
                    string YurtDisi_Banka_Dovizi;
                    string YurtDisi_Diger_Dovizi;

                    string YurtDisi_Diger_Aciklama;

                    string Kalem_Islem_Niteligi;
                    string Giris_Cikis_Amaci;
                    string Giris_Cikis_Amaci_Aciklama;
                    string Referans_Tarihi;
                    string STMIlKodu;
                    //string OdemeSekli;

                    foreach (XmlNode var in xKalem)
                    {
                        Gtip = var["Gtip"].InnerText;
                        if (var["Imalatci_firma_bilgisi"].InnerText != "")
                            Imalatci_firma_bilgisi = var["Imalatci_firma_bilgisi"].InnerText;
                        else Imalatci_firma_bilgisi = "";
                        if (var["Kalem_no"].InnerText != "")
                            Kalem_no = var["Kalem_sira_no"].InnerText;
                        else Kalem_no = "";
                        if (var["Mensei_ulke"].InnerText != "")
                            Mensei_ulke = var["Mensei_ulke"].InnerText;
                        else Mensei_ulke = "";
                        if (var["Brut_agirlik"].InnerText != "")
                            Brut_agirlik = Convert.ToDecimal(var["Brut_agirlik"].InnerText.ToString().Replace(".", ","));
                        else Brut_agirlik = 0;
                        if (var["Net_agirlik"].InnerText != "")
                            Net_agirlik = Convert.ToDecimal(var["Net_agirlik"].InnerText.ToString().Replace(".", ","));
                        else Net_agirlik = 0;
                        Tamamlayici_olcu_birimi = var["Tamamlayici_olcu_birimi"].InnerText;
                        if (var["Istatistiki_miktar"].InnerText != "")
                            Istatistiki_miktar = Convert.ToDecimal(var["Istatistiki_miktar"].InnerText.ToString().Replace(".", ","));
                        else Istatistiki_miktar = 0;
                        if (var["Uluslararasi_anlasma"] != null)
                        {
                            Uluslararasi_anlasma = var["Uluslararasi_anlasma"].InnerText;
                        }
                        else
                        {
                            Uluslararasi_anlasma = "";
                        }

                        if (var["Algilama_birimi_1"] != null)
                        {
                            Algilama_birimi_1 = var["Algilama_birimi_1"].InnerText;
                        }
                        else
                        {
                            Algilama_birimi_1 = "";
                        }

                        if (var["Algilama_birimi_2"] != null)
                        {
                            Algilama_birimi_2 = var["Algilama_birimi_2"].InnerText;
                        }
                        else
                        {
                            Algilama_birimi_2 = "";
                        }
                        if (var["Algilama_birimi_3"] != null)
                        {
                            Algilama_birimi_3 = var["Algilama_birimi_3"].InnerText;
                        }
                        else
                        {
                            Algilama_birimi_3 = "";
                        }


                        if (var["Algilama_miktari_1"].InnerText != "")
                            Algilama_miktari_1 = Convert.ToDecimal(var["Algilama_miktari_1"].InnerText.ToString().Replace(".", ","));
                        else Algilama_miktari_1 = 0;
                        if (var["Algilama_miktari_2"].InnerText != "")
                            Algilama_miktari_2 = Convert.ToDecimal(var["Algilama_miktari_2"].InnerText.ToString().Replace(".", ","));
                        else Algilama_miktari_2 = 0;
                        if (var["Algilama_miktari_3"].InnerText != "")
                            Algilama_miktari_3 = Convert.ToDecimal(var["Algilama_miktari_3"].InnerText.ToString().Replace(".", ","));
                        else Algilama_miktari_3 = 0;
                        if (var["Muafiyetler_1"] != null)
                        {
                            Muafiyetler_1 = var["Muafiyetler_1"].InnerText;
                        }
                        else
                        {
                            Muafiyetler_1 = "";
                        }
                        if (var["Muafiyetler_2"] != null)
                        {
                            Muafiyetler_2 = var["Muafiyetler_2"].InnerText;
                        }
                        else
                        {
                            Muafiyetler_2 = "";
                        }
                        if (var["Muafiyetler_3"] != null)
                        {
                            Muafiyetler_3 = var["Muafiyetler_3"].InnerText;
                        }
                        else
                        {
                            Muafiyetler_3 = "";
                        }

                        if (var["Teslim_sekli"] != null)
                        {
                            Teslim_sekli = var["Teslim_sekli"].InnerText;
                        }
                        else
                        {
                            Teslim_sekli = "";
                        }
                        if (var["Ek_kod"] != null)
                        {
                            Ek_kod = var["Ek_kod"].InnerText;
                        }
                        else
                        {
                            Ek_kod = "";
                        }
                        if (var["Ozellik"] != null)
                        {
                            Ozellik = var["Ozellik"].InnerText;
                        }
                        else
                        {
                            Ozellik = "";
                        }

                        if (var["Fatura_miktari"] != null)
                        {
                            Fatura_miktari = Convert.ToDecimal(var["Fatura_miktari"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Fatura_miktari = 0;
                        }

                        if (var["Fatura_miktarinin_dovizi"] != null)
                        {
                            Fatura_miktarinin_dovizi = var["Fatura_miktarinin_dovizi"].InnerText;
                        }
                        else
                        {
                            Fatura_miktarinin_dovizi = "";
                        }
                        if (var["Sinir_gecis_ucreti"] != null)
                        {
                            Sinir_gecis_ucreti = Convert.ToDecimal(var["Sinir_gecis_ucreti"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Sinir_gecis_ucreti = 0;
                        }
                        if (var["Navlun_miktari"] != null)
                        {
                            Navlun_miktari = Convert.ToDecimal(var["Navlun_miktari"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Navlun_miktari = 0;
                        }
                        if (var["Navlun_miktarinin_dovizi"] != null)
                        {
                            Navlun_miktarinin_dovizi = var["Navlun_miktarinin_dovizi"].InnerText;
                        }
                        else
                        {
                            Navlun_miktarinin_dovizi = "";
                        }
                        if (var["Istatistiki_kiymet"] != null)
                        {
                            Istatistiki_kiymet = Convert.ToDecimal(var["Istatistiki_kiymet"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Istatistiki_kiymet = 0;
                        }
                        if (var["Sigorta_miktarinin_dovizi"] != null)
                        {
                            Sigorta_miktarinin_dovizi = var["Sigorta_miktarinin_dovizi"].InnerText;
                        }
                        else
                        {
                            Sigorta_miktarinin_dovizi = "";
                        }
                        if (var["Sigorta_miktari"] != null)
                        {
                            Sigorta_miktari = Convert.ToDecimal(var["Sigorta_miktari"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Sigorta_miktari = 0;
                        }
                        if (var["Navlun_miktarinin_dovizi"] != null)
                        {
                            Navlun_miktarinin_dovizi = var["Navlun_miktarinin_dovizi"].InnerText;
                        }
                        else
                        {
                            Navlun_miktarinin_dovizi = "";
                        }
                        if (var["Yurt_disi_harcamalar_miktari"] != null)
                        {
                            Yurt_disi_harcamalar_miktari = Convert.ToDecimal(var["Yurt_disi_harcamalar_miktari"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Yurt_disi_harcamalar_miktari = 0;
                        }
                        if (var["Yurt_disi_harcamalar_miktarinin_dovizi"] != null)
                        {
                            Yurt_disi_harcamalar_miktarinin_dovizi = var["Yurt_disi_harcamalar_miktarinin_dovizi"].InnerText;
                        }
                        else
                        {
                            Yurt_disi_harcamalar_miktarinin_dovizi = "";
                        }
                        if (var["Yurtici_Diger_Aciklama"] != null)
                        {
                            YurtIci_Diger_Aciklama = var["Yurtici_Diger_Aciklama"].InnerText;
                        }
                        else
                        {
                            YurtIci_Diger_Aciklama = "";
                        }
                        if (var["Yurt_ici_harcamalar"] != null)
                        {
                            Yurt_ici_harcamalar = Convert.ToDecimal(var["Yurt_ici_harcamalar"].InnerText.ToString().Replace(".", ",")); ;
                        }
                        else
                        {
                            Yurt_ici_harcamalar = 0;
                        }
                        if (var["Tarifedeki_tanimi"] != null)
                        {
                            Tarifedeki_tanimi = var["Tarifedeki_tanimi"].InnerText;
                        }
                        else
                        {
                            Tarifedeki_tanimi = "";
                        }
                        if (var["Ticari_tanimi"] != null)
                        {
                            Ticari_tanimi = var["Ticari_tanimi"].InnerText;
                        }
                        else
                        {
                            Ticari_tanimi = "";
                        }
                        if (var["Marka"] != null)
                        {
                            Marka = var["Marka"].InnerText;
                        }
                        else
                        {
                            Marka = "";
                        }
                        if (var["Numara"] != null)
                        {
                            Numara = var["Numara"].InnerText;
                        }
                        else
                        {
                            Numara = "";
                        }
                        if (var["Cinsi"] != null)
                        {
                            Cinsi = var["Cinsi"].InnerText;
                        }
                        else
                        {
                            Cinsi = "";
                        }
                        if (var["Miktar_birimi"] != null)
                        {
                            Miktar_birimi = var["Miktar_birimi"].InnerText;
                        }
                        else
                        {
                            Miktar_birimi = "";
                        }
                        if (var["Mahrece_iade"] != null)
                        {
                            Mahrece_iade = var["Mahrece_iade"].InnerText;
                        }
                        else
                        {
                            Mahrece_iade = "";
                        }
                        if (var["Ikincil_islem"] != null)
                        {
                            Ikincil_islem = var["Ikincil_islem"].InnerText;
                        }
                        else
                        {
                            Ikincil_islem = "";
                        }
                        if (var["Adedi"] != null)
                        {
                            Adedi = Convert.ToDecimal(var["Adedi"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Adedi = 0;
                        }
                        if (var["Satir_no"] != null)
                        {
                            Satir_no = var["Satir_no"].InnerText;
                        }
                        else
                        {
                            Satir_no = "";
                        }
                        if (var["Miktar"] != null)
                        {
                            Miktar = Convert.ToDecimal(var["Miktar"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Miktar = 0;
                        }


                        if (var["Kdv_orani"] != null)
                        {
                            Kdv_orani = var["Kdv_orani"].InnerText;

                        }
                        else
                        {
                            Kdv_orani = "";
                        }
                        if (var["Kullanilmis_esya"] != null)
                        {
                            Kullanilmis = var["Kullanilmis_esya"].InnerText;
                        }
                        else
                        {
                            Kullanilmis = "";
                        }


                        if (var["Aciklama_44"] != null)
                            aciklama44 = var["Aciklama_44"].InnerText;

                        if (var["Imalatci_Vergino"] != null)
                        {
                            imFirmaVergiNo = var["Imalatci_Vergino"].InnerText;
                        }
                        else
                        {
                            imFirmaVergiNo = "";
                        }
                        if (var["Yurtici_Banka"] != null)
                        {
                            Yurtici_Banka = Convert.ToDecimal(var["Yurtici_Banka"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Yurtici_Banka = 0;
                        }
                        if (var["Yurtici_Cevre"] != null)
                        {
                            Yurtici_Cevre = Convert.ToDecimal(var["Yurtici_Cevre"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            Yurtici_Cevre = 0;
                        }
                        if (var["Yurtici_Depolama"] != null)
                        {
                            Yurtici_Depolama = Convert.ToDecimal(var["Yurtici_Depolama"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            Yurtici_Depolama = 0;
                        }
                        if (var["Yurtici_Diger"] != null)
                        {
                            Yurtici_Diger = Convert.ToDecimal(var["Yurtici_Diger"].InnerText.ToString().Replace(".", ","));



                        }
                        else
                        {
                            Yurtici_Diger = 0;
                        }
                        if (var["Yurtici_Kkdf"] != null)
                        {
                            Yurtici_Kkdf = Convert.ToDecimal(var["Yurtici_Kkdf"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Yurtici_Kkdf = 0;
                        }
                        if (var["Yurtici_Kultur"] != null)
                        {
                            Yurtici_Kultur = Convert.ToDecimal(var["Yurtici_Kultur"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            Yurtici_Kultur = 0;
                        }
                        if (var["Yurtici_Liman"] != null)
                        {
                            Yurtici_Liman = Convert.ToDecimal(var["Yurtici_Liman"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Yurtici_Liman = 0;
                        }
                        if (var["Yurtici_Tahliye"] != null)
                        {
                            Yurtici_Tahliye = Convert.ToDecimal(var["Yurtici_Tahliye"].InnerText.ToString().Replace(".", ","));
                        }
                        else
                        {
                            Yurtici_Tahliye = 0;
                        }

                        if (var["konteyner_Bilgi"] != null)
                        {
                            konteyner_Bilgi = var["konteyner_Bilgi"].InnerText;
                        }
                        else
                        {
                            konteyner_Bilgi = "";
                        }

                        if (var["Muafiyet_Aciklamasi"] != null)
                        {
                            Muafiyet_Aciklamasi = var["Muafiyet_Aciklamasi"].InnerText;
                        }
                        else
                        {
                            Muafiyet_Aciklamasi = "";
                        }

                        if (var["Giris_Cikis_Amaci"] != null)
                        {
                            Giris_Cikis_Amaci = var["Giris_Cikis_Amaci"].InnerText;
                        }
                        else
                        {
                            Giris_Cikis_Amaci = "";
                        }

                        if (var["Giris_Cikis_Amaci_Aciklama"] != null)
                        {
                            Giris_Cikis_Amaci_Aciklama = var["Giris_Cikis_Amaci_Aciklama"].InnerText;
                        }
                        else
                        {
                            Giris_Cikis_Amaci_Aciklama = "";
                        }
                        if (var["Kalem_Islem_Niteligi"] != null)
                        {
                            Kalem_Islem_Niteligi = var["Kalem_Islem_Niteligi"].InnerText;
                        }
                        else
                        {
                            Kalem_Islem_Niteligi = "";
                        }



                        if (var["YurtDisi_Komisyon_Dovizi"] != null)
                        {
                            YurtDisi_Komisyon_Dovizi = var["YurtDisi_Komisyon_Dovizi"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Komisyon_Dovizi = "";
                        }
                        if (var["YurtDisi_Depolama_Dovizi"] != null)
                        {
                            YurtDisi_Depolama_Dovizi = var["YurtDisi_Depolama_Dovizi"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Depolama_Dovizi = "";
                        }
                        if (var["YurtDisi_Royalti_Dovizi"] != null)
                        {
                            YurtDisi_Royalti_Dovizi = var["YurtDisi_Royalti_Dovizi"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Royalti_Dovizi = "";
                        }
                        if (var["YurtDisi_Banka_Dovizi"] != null)
                        {
                            YurtDisi_Banka_Dovizi = var["YurtDisi_Banka_Dovizi"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Banka_Dovizi = "";
                        }
                        if (var["YurtDisi_Diger_Dovizi"] != null)
                        {
                            YurtDisi_Diger_Dovizi = var["YurtDisi_Diger_Dovizi"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Diger_Dovizi = "";
                        }
                        if (var["YurtDisi_Diger_Aciklama"] != null)
                        {
                            YurtDisi_Diger_Aciklama = var["YurtDisi_Diger_Aciklama"].InnerText;
                        }
                        else
                        {
                            YurtDisi_Diger_Aciklama = "";
                        }


                        if (var["YurtDisi_Komisyon"] != null)
                        {
                            YurtDisi_Komisyon = Convert.ToDecimal(var["YurtDisi_Komisyon"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            YurtDisi_Komisyon = 0;
                        }
                        if (var["YurtDisi_Depolama"] != null)
                        {
                            YurtDisi_Depolama = Convert.ToDecimal(var["YurtDisi_Depolama"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            YurtDisi_Depolama = 0;
                        }
                        if (var["YurtDisi_Royalti"] != null)
                        {
                            YurtDisi_Royalti = Convert.ToDecimal(var["YurtDisi_Royalti"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            YurtDisi_Royalti = 0;
                        }
                        if (var["YurtDisi_Banka"] != null)
                        {
                            YurtDisi_Banka = Convert.ToDecimal(var["YurtDisi_Banka"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            YurtDisi_Banka = 0;
                        }
                        if (var["YurtDisi_Diger"] != null)
                        {
                            YurtDisi_Diger = Convert.ToDecimal(var["YurtDisi_Diger"].InnerText.ToString().Replace(".", ","));

                        }
                        else
                        {
                            YurtDisi_Diger = 0;
                        }
                        if (var["Referans_Tarihi"] != null)
                        {
                            Referans_Tarihi = var["Referans_Tarihi"].InnerText.ToString();
                        }
                        else
                        {
                            Referans_Tarihi = "";
                        }

                        if (var["STM_IlKodu"] != null)
                        {
                            STMIlKodu = var["STM_IlKodu"].InnerText;
                        }
                        else
                        {
                            STMIlKodu = "";
                        }

                        //if (var["OdemeSekli"] != null)
                        //{
                        //    OdemeSekli = var["OdemeSekli"].InnerText;
                        //}
                        //else
                        //{
                        //    OdemeSekli = "";
                        //}


                        //test.kalem kalemObj = new test.kalem();
                        //kalemObj.Gtip = Gtip;
                        //kalemObj.Imalatci_firma_bilgisi= Imalatci_firma_bilgisi;

                        kalemler[0].Add(Kalem_no);
                        kalemler[1].Add(Uluslararasi_anlasma);
                        kalemler[2].Add(Ozellik);
                        kalemler[3].Add(Ek_kod);
                        kalemler[4].Add(YurtIci_Diger_Aciklama);
                        kalemler[5].Add(Sigorta_miktarinin_dovizi);
                        kalemler[6].Add(Yurt_disi_harcamalar_miktarinin_dovizi);
                        kalemler[7].Add(Navlun_miktarinin_dovizi);
                        kalemler[8].Add(Fatura_miktarinin_dovizi);
                        kalemler[9].Add(Teslim_sekli);
                        kalemler[10].Add(Mensei_ulke);
                        kalemler[11].Add(Muafiyetler_1);
                        kalemler[12].Add(Muafiyetler_2);
                        kalemler[13].Add(Muafiyetler_3);
                        kalemler[14].Add(Miktar_birimi);
                        kalemler[15].Add(Algilama_birimi_1);
                        kalemler[16].Add(Algilama_birimi_2);
                        kalemler[17].Add(Algilama_birimi_3);
                        kalemler[18].Add(Tamamlayici_olcu_birimi);
                        kalemler[19].Add(Gtip);
                        kalemler[20].Add(Satir_no);
                        kalemler[21].Add(Tarifedeki_tanimi);
                        kalemler[22].Add(Yurt_ici_harcamalar);
                        kalemler[23].Add(Yurt_disi_harcamalar_miktari);
                        kalemler[24].Add(Fatura_miktari);
                        kalemler[25].Add(Sinir_gecis_ucreti);
                        kalemler[26].Add(Istatistiki_kiymet);
                        kalemler[27].Add(Miktar);
                        kalemler[28].Add(Istatistiki_miktar);
                        kalemler[29].Add(Brut_agirlik);
                        kalemler[30].Add(Net_agirlik);
                        kalemler[31].Add(Algilama_miktari_1);
                        kalemler[32].Add(Algilama_miktari_2);
                        kalemler[33].Add(Algilama_miktari_3);
                        kalemler[34].Add(Mahrece_iade);
                        kalemler[35].Add(Ikincil_islem);
                        kalemler[36].Add(Imalatci_firma_bilgisi);
                        kalemler[37].Add(Ticari_tanimi);
                        kalemler[38].Add(Marka);
                        kalemler[39].Add(Numara);
                        kalemler[40].Add(Cinsi);
                        kalemler[41].Add(Adedi);
                        kalemler[42].Add(Sigorta_miktari);
                        kalemler[43].Add(Navlun_miktari);
                        kalemler[44].Add(Kdv_orani);
                        kalemler[45].Add("Yaratılmış");
                        kalemler[46].Add(Kullanilmis);

                        XmlNode tamamlayici = var["tamamlayici_bilgi"];
                        List<object>[] tamamlayicibilgi = new List<object>[2];

                        //   List<test.tamamlayici> tamamlayicibilgi = new List<test.tamamlayici>();
                        //    List<test.tcgbacmakapatma> acmakapatma = new List<test.tcgbacmakapatma>();
                        if (tamamlayici != null)
                        {


                            foreach (XmlNode lsttamamlayici in tamamlayici)
                            {


                                for (int s = 0; s < tamamlayicibilgi.Length; s++)
                                {

                                    if (tamamlayicibilgi[s] == null) tamamlayicibilgi[s] = new List<object>();
                                }


                                string t_bilgi = lsttamamlayici["Tamamlayici_bilgi"].InnerText;
                                string t_oran = lsttamamlayici["Tamamlayici_bilgi_orani"].InnerText;


                                tamamlayicibilgi[0].Add(t_bilgi);
                                tamamlayicibilgi[1].Add(t_oran);

                            }

                        }
                        kalemler[47].Add(tamamlayicibilgi);

                        XmlNode acmalar = var["tcgbacmakapatma_bilgi"];

                        List<object>[] acmakapatma = new List<object>[3];

                        if (acmalar != null)
                        {
                            foreach (XmlNode lsttacmalar in acmalar)
                            {
                                for (int s = 0; s < acmakapatma.Length; s++)
                                {
                                    if (acmakapatma[s] == null) acmakapatma[s] = new List<object>();
                                }

                                //   int Kalem_No =Convert.ToInt32(lsttacmalar.ChildNodes[0].InnerText);
                                string K_beyanname_no = lsttacmalar["Kapatilan_beyanname_no"].InnerText;
                                int Kapatilan_Kalem_No = Convert.ToInt32(lsttacmalar["Kapatilan_kalem_no"].InnerText.ToString());
                                decimal Kapatilan_Miktar = Convert.ToDecimal(lsttacmalar["Kapatilan_miktar"].InnerText.ToString().Replace(".", ","));

                                acmakapatma[0].Add(K_beyanname_no);
                                acmakapatma[1].Add(Kapatilan_Kalem_No);
                                acmakapatma[2].Add(Kapatilan_Miktar);
                            }
                        }

                        kalemler[48].Add(acmakapatma);
                        kalemler[49].Add(aciklama44);
                        kalemler[50].Add(imFirmaVergiNo);

                        List<object>[] marka = new List<object>[10];
                        XmlNode markaBilgi = var["marka_model_bilgi"];
                        if (markaBilgi != null)
                        {
                            foreach (XmlNode lstMarkabilgi in markaBilgi)
                            {
                                for (int s = 0; s < marka.Length; s++)
                                {

                                    if (marka[s] == null) marka[s] = new List<object>();
                                }

                                string m_model_yili;
                                if (lstMarkabilgi["Model_Yili"] != null)
                                {
                                    m_model_yili = lstMarkabilgi["Model_Yili"].InnerText;
                                }
                                else
                                {
                                    m_model_yili = "";
                                }
                                string m_turu;
                                if (lstMarkabilgi["Marka_Turu"] != null)
                                {
                                    m_turu = lstMarkabilgi["Marka_Turu"].InnerText;
                                }
                                else
                                {
                                    m_turu = "";
                                }
                                string m_tescil_no;
                                if (lstMarkabilgi["Marka_Tescil_No"] != null)
                                {
                                    m_tescil_no = lstMarkabilgi["Marka_Tescil_No"].InnerText;
                                }
                                else
                                {
                                    m_tescil_no = "";
                                }

                                string m_adi;
                                if (lstMarkabilgi["Marka_Adi"] != null)
                                {
                                    m_adi = lstMarkabilgi["Marka_Adi"].InnerText;
                                }
                                else
                                {
                                    m_adi = "";
                                }

                                string m_kiymeti;
                                if (lstMarkabilgi["Marka_Kiymeti"] != null)
                                {
                                    m_kiymeti = lstMarkabilgi["Marka_Kiymeti"].InnerText.ToString().Replace(".", ",");
                                }
                                else
                                {
                                    m_kiymeti = "";
                                }
                                string m_ref_no;
                                if (lstMarkabilgi["Referans_No"] != null)
                                {
                                    m_ref_no = lstMarkabilgi["Referans_No"].InnerText;
                                }
                                else
                                {
                                    m_ref_no = "";
                                }


                                string m_model;
                                if (lstMarkabilgi["Model"] != null)
                                {
                                    m_model = lstMarkabilgi["Model"].InnerText;
                                }
                                else
                                {
                                    m_model = "";
                                }
                                string m_motor_hacmi;
                                if (lstMarkabilgi["Motor_hacmi"] != null)
                                {
                                    m_motor_hacmi = lstMarkabilgi["Motor_hacmi"].InnerText;
                                }
                                else
                                {
                                    m_motor_hacmi = "";
                                }
                                string m_silindir_adedi;
                                if (lstMarkabilgi["Silindir_adedi"] != null)
                                {
                                    m_silindir_adedi = lstMarkabilgi["Silindir_adedi"].InnerText;
                                }
                                else
                                {
                                    m_silindir_adedi = "";
                                }
                                string m_renk;
                                if (lstMarkabilgi["Renk"] != null)
                                {
                                    m_renk = lstMarkabilgi["Renk"].InnerText;
                                }
                                else
                                {
                                    m_renk = "";
                                }


                                marka[0].Add(m_turu);
                                marka[1].Add(m_tescil_no);
                                marka[2].Add(m_adi);
                                marka[3].Add(m_kiymeti);
                                marka[4].Add(m_ref_no);
                                marka[5].Add(m_model_yili);
                                marka[6].Add(m_model);
                                marka[7].Add(m_motor_hacmi);
                                marka[8].Add(m_silindir_adedi);
                                marka[9].Add(m_renk);


                            }
                        }
                        kalemler[51].Add(marka);

                        kalemler[52].Add(Yurtici_Banka);
                        kalemler[53].Add(Yurtici_Cevre);
                        kalemler[54].Add(Yurtici_Depolama);
                        kalemler[55].Add(Yurtici_Diger);
                        kalemler[56].Add(Yurtici_Kkdf);
                        kalemler[57].Add(Yurtici_Kultur);
                        kalemler[58].Add(Yurtici_Liman);
                        kalemler[59].Add(Yurtici_Tahliye);

                        List<object>[] konteyner = new List<object>[2];
                        XmlNode konteynerBilgi = var["konteyner_Bilgi"];
                        if (konteynerBilgi != null)
                        {
                            foreach (XmlNode lstKonteynerbilgi in konteynerBilgi)
                            {
                                for (int s = 0; s < konteyner.Length; s++)
                                {

                                    if (konteyner[s] == null) konteyner[s] = new List<object>();
                                }

                                string m_konteyner_ulke;

                                if (lstKonteynerbilgi["Ulke_Kodu"] != null)
                                {
                                    m_konteyner_ulke = lstKonteynerbilgi["Ulke_Kodu"].InnerText;
                                }
                                else
                                {
                                    m_konteyner_ulke = "";
                                }
                                string m_konteyner_no;
                                if (lstKonteynerbilgi["Konteyner_No"] != null)
                                {
                                    m_konteyner_no = lstKonteynerbilgi["Konteyner_No"].InnerText;
                                }
                                else
                                {
                                    m_konteyner_no = "";
                                }


                                konteyner[0].Add(m_konteyner_ulke);
                                konteyner[1].Add(m_konteyner_no);

                            }
                        }
                        kalemler[60].Add(konteyner);

                        kalemler[61].Add(Muafiyet_Aciklamasi);
                        kalemler[62].Add(Giris_Cikis_Amaci);
                        kalemler[63].Add(Giris_Cikis_Amaci_Aciklama);
                        kalemler[64].Add(YurtDisi_Komisyon);
                        kalemler[65].Add(YurtDisi_Depolama);
                        kalemler[66].Add(YurtDisi_Royalti);
                        kalemler[67].Add(YurtDisi_Banka);
                        kalemler[68].Add(YurtDisi_Diger);
                        kalemler[69].Add(YurtDisi_Diger_Aciklama);
                        kalemler[70].Add(Kalem_Islem_Niteligi);
                        kalemler[71].Add(YurtDisi_Komisyon_Dovizi);
                        kalemler[72].Add(YurtDisi_Depolama_Dovizi);
                        kalemler[73].Add(YurtDisi_Royalti_Dovizi);
                        kalemler[74].Add(YurtDisi_Banka_Dovizi);
                        kalemler[75].Add(YurtDisi_Diger_Dovizi);
                        kalemler[76].Add(Referans_Tarihi);
                        kalemler[77].Add(STMIlKodu);
                        List<object>[] odemeSekli = new List<object>[3];
                        XmlNode odemeSekliBilgi = var["OdemeSekilleri"];
                        if (odemeSekliBilgi != null)
                        {
                            foreach (XmlNode lstodemeSekliBilgi in odemeSekliBilgi)
                            {
                                for (int s = 0; s < odemeSekli.Length; s++)
                                {

                                    if (odemeSekli[s] == null) odemeSekli[s] = new List<object>();
                                }

                                string m_odemeSekliKodu;

                                if (lstodemeSekliBilgi["OdemeSekliKodu"] != null)
                                {
                                    m_odemeSekliKodu = lstodemeSekliBilgi["OdemeSekliKodu"].InnerText;
                                }
                                else
                                {
                                    m_odemeSekliKodu = "";
                                }
                                decimal m_OdemeTutari;
                                if (lstodemeSekliBilgi["OdemeTutari"] != null)
                                {
                                    m_OdemeTutari = Convert.ToDecimal(lstodemeSekliBilgi["OdemeTutari"].InnerText.Replace(".", ","));
                                }
                                else
                                {
                                    m_OdemeTutari = 0;
                                }
                                string m_TBFId;

                                if (lstodemeSekliBilgi["TBFID"] != null)
                                {
                                    m_TBFId = lstodemeSekliBilgi["TBFID"].InnerText;
                                }
                                else
                                {
                                    m_TBFId = "";
                                }


                                odemeSekli[0].Add(m_odemeSekliKodu);
                                odemeSekli[1].Add(m_OdemeTutari);
                                odemeSekli[2].Add(m_TBFId);

                            }
                        }
                        kalemler[78].Add(odemeSekli);
                    }
                    cevaptanGelenKalemler = kalemler;

                }


            }

            #endregion

            #region Kıymet
            try
            {
                XmlNodeList XmlNodeListKiymetObj1 = dd.GetElementsByTagName("KiymetBildirim");
                if (XmlNodeListKiymetObj1[0] != null)
                {


                    if (XmlNodeListKiymetObj1[0].ChildNodes.Count > 0)
                    {
                        List<object>[] kiymet;
                        kiymet = new List<object>[18];
                        for (int i = 0; i < kiymet.Length; i++)
                        {

                            if (kiymet[i] == null) kiymet[i] = new List<object>();
                        }

                        XmlNodeList xKiymet = XmlNodeListKiymetObj1[0].ChildNodes;
                        //    test.kalem[] skalemler = new test.kalem[XmlNodeListKalemObj1[0].ChildNodes.Count];

                        string Edim;
                        string Emsal;
                        string AliciSatici;
                        string Munasebet;
                        string Kisitlamalar;
                        string Royalti;
                        string Taahutname;
                        string SaticiyaIntikal;
                        string FaturaTarihiSayisi;
                        string GumrukIdaresiKarari;
                        string AliciSaticiAyrintilar;
                        string KisitlamalarAyrintilar;
                        string RoyaltiKosullar;
                        string SaticiyaIntikalKosullar;
                        string SehirYer;
                        string SozlesmeTarihiSayisi;
                        string TeslimSekli;

                        foreach (XmlNode var in xKiymet)
                        {
                            AliciSatici = var["AliciSatici"].InnerText;
                            AliciSaticiAyrintilar = var["AliciSaticiAyrintilar"].InnerText;
                            Edim = var["Edim"].InnerText;
                            Emsal = var["Emsal"].InnerText;
                            FaturaTarihiSayisi = var["FaturaTarihiSayisi"].InnerText;
                            GumrukIdaresiKarari = var["GumrukIdaresiKarari"].InnerText;
                            Kisitlamalar = var["Kisitlamalar"].InnerText;
                            KisitlamalarAyrintilar = var["KisitlamalarAyrintilar"].InnerText;
                            Munasebet = var["Munasebet"].InnerText;
                            Royalti = var["Royalti"].InnerText;
                            RoyaltiKosullar = var["RoyaltiKosullar"].InnerText;
                            SaticiyaIntikal = var["SaticiyaIntikal"].InnerText;
                            SaticiyaIntikalKosullar = var["SaticiyaIntikalKosullar"].InnerText;
                            SehirYer = var["SehirYer"].InnerText;
                            SozlesmeTarihiSayisi = var["SozlesmeTarihiSayisi"].InnerText;
                            Taahutname = var["Taahutname"].InnerText;
                            TeslimSekli = var["TeslimSekli"].InnerText;

                            kiymet[0].Add(AliciSatici);
                            kiymet[1].Add(AliciSaticiAyrintilar);
                            kiymet[2].Add(Edim);
                            kiymet[3].Add(Emsal);
                            kiymet[4].Add(FaturaTarihiSayisi);
                            kiymet[5].Add(GumrukIdaresiKarari);
                            kiymet[6].Add(Kisitlamalar);
                            kiymet[7].Add(KisitlamalarAyrintilar);
                            kiymet[8].Add(Munasebet);
                            kiymet[9].Add(Royalti);
                            kiymet[10].Add(RoyaltiKosullar);
                            kiymet[11].Add(SaticiyaIntikal);
                            kiymet[12].Add(SaticiyaIntikalKosullar);
                            kiymet[13].Add(SehirYer);
                            kiymet[14].Add(SozlesmeTarihiSayisi);
                            kiymet[15].Add(Taahutname);
                            kiymet[16].Add(TeslimSekli);


                            XmlNode KiymetKalemlerBilgi = var["KiymetKalemler"];
                            List<object>[] KiymetKalemler = new List<object>[19];
                            if (KiymetKalemlerBilgi != null)
                            {
                                foreach (XmlNode lstKiymetKalemler in KiymetKalemlerBilgi)
                                {
                                    for (int s = 0; s < KiymetKalemler.Length; s++)
                                    {

                                        if (KiymetKalemler[s] == null) KiymetKalemler[s] = new List<object>();
                                    }


                                    int kiymet_kalem_BeyannameKalemNo = Convert.ToInt32(lstKiymetKalemler["BeyannameKalemNo"].InnerText);
                                    decimal kiymet_kalem_DigerOdemeler = Convert.ToDecimal(lstKiymetKalemler["DigerOdemeler"].InnerText.Replace(".", ","));
                                    string kiymet_kalem_DigerOdemelerNiteligi;
                                    if (lstKiymetKalemler["DigerOdemelerNiteligi"] != null)
                                    {
                                        kiymet_kalem_DigerOdemelerNiteligi = lstKiymetKalemler["DigerOdemelerNiteligi"].InnerText;
                                    }
                                    else
                                    {
                                        kiymet_kalem_DigerOdemelerNiteligi = "";
                                    }
                                    decimal kiymet_kalem_DolayliIntikal = Convert.ToDecimal(lstKiymetKalemler["DolayliIntikal"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_DolayliOdeme = Convert.ToDecimal(lstKiymetKalemler["DolayliOdeme"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_GirisSonrasiNakliye = Convert.ToDecimal(lstKiymetKalemler["GirisSonrasiNakliye"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_IthalaKatilanMalzeme = Convert.ToDecimal(lstKiymetKalemler["IthalaKatilanMalzeme"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_IthalaUretimAraclar = Convert.ToDecimal(lstKiymetKalemler["IthalaUretimAraclar"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_IthalaUretimTuketimMalzemesi = Convert.ToDecimal(lstKiymetKalemler["IthalaUretimTuketimMalzemesi"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_KapAmbalajBedeli = Convert.ToDecimal(lstKiymetKalemler["KapAmbalajBedeli"].InnerText.Replace(".", ","));
                                    int kiymet_kalem_KiymetKalemNo = Convert.ToInt32(lstKiymetKalemler["KiymetKalemNo"].InnerText);
                                    decimal kiymet_kalem_Komisyon = Convert.ToDecimal(lstKiymetKalemler["Komisyon"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_Nakliye = Convert.ToDecimal(lstKiymetKalemler["Nakliye"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_PlanTaslak = Convert.ToDecimal(lstKiymetKalemler["PlanTaslak"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_RoyaltiLisans = Convert.ToDecimal(lstKiymetKalemler["RoyaltiLisans"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_Sigorta = Convert.ToDecimal(lstKiymetKalemler["Sigorta"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_TeknikYardim = Convert.ToDecimal(lstKiymetKalemler["TeknikYardim"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_Tellaliye = Convert.ToDecimal(lstKiymetKalemler["Tellaliye"].InnerText.Replace(".", ","));
                                    decimal kiymet_kalem_VergiHarcFon = Convert.ToDecimal(lstKiymetKalemler["VergiHarcFon"].InnerText.Replace(".", ","));

                                    KiymetKalemler[0].Add(kiymet_kalem_BeyannameKalemNo);
                                    KiymetKalemler[1].Add(kiymet_kalem_DigerOdemeler);
                                    KiymetKalemler[2].Add(kiymet_kalem_DigerOdemelerNiteligi);
                                    KiymetKalemler[3].Add(kiymet_kalem_DolayliIntikal);
                                    KiymetKalemler[4].Add(kiymet_kalem_DolayliOdeme);
                                    KiymetKalemler[5].Add(kiymet_kalem_GirisSonrasiNakliye);
                                    KiymetKalemler[6].Add(kiymet_kalem_IthalaKatilanMalzeme);
                                    KiymetKalemler[7].Add(kiymet_kalem_IthalaUretimAraclar);
                                    KiymetKalemler[8].Add(kiymet_kalem_IthalaUretimTuketimMalzemesi);
                                    KiymetKalemler[9].Add(kiymet_kalem_KapAmbalajBedeli);
                                    KiymetKalemler[10].Add(kiymet_kalem_KiymetKalemNo);
                                    KiymetKalemler[11].Add(kiymet_kalem_Komisyon);
                                    KiymetKalemler[12].Add(kiymet_kalem_Nakliye);
                                    KiymetKalemler[13].Add(kiymet_kalem_PlanTaslak);
                                    KiymetKalemler[14].Add(kiymet_kalem_RoyaltiLisans);
                                    KiymetKalemler[15].Add(kiymet_kalem_Sigorta);
                                    KiymetKalemler[16].Add(kiymet_kalem_TeknikYardim);
                                    KiymetKalemler[17].Add(kiymet_kalem_Tellaliye);
                                    KiymetKalemler[18].Add(kiymet_kalem_VergiHarcFon);
                                }

                            }

                            kiymet[17].Add(KiymetKalemler);
                        }
                        cevaptanGelenKiymet = kiymet;

                    }
                }
            }
            catch (Exception EX)
            {


            }

            #endregion

            XmlNodeList XMLbelgeler = dd.GetElementsByTagName("Dokumanlar");
            XmlNodeList XMLvergiler = dd.GetElementsByTagName("Vergiler");
            XmlNodeList XMLsorularacevap = dd.GetElementsByTagName("Sorular_cevaplar");

            #region SoruCevap
            {
                if (XMLsorularacevap[0] != null)
                {
                    if (XMLsorularacevap[0].ChildNodes != null)
                    {
                        if (XMLsorularacevap[0].ChildNodes.Count > 0)
                        {
                            XmlNodeList xSrCvp = XMLsorularacevap[0].ChildNodes;

                            List<Soru_Cevap> sorucevap = new List<Soru_Cevap>();
                            string soru_no;
                            string cevap;
                            int Kalem_no;

                            //List<object>[] Soru_Cevap;

                            //Soru_Cevap = new List<object>[3];

                            //for (int i = 0; i < Soru_Cevap.Length; i++)
                            //{

                            //    if (Soru_Cevap[i] == null) Soru_Cevap[i] = new List<object>();
                            //}


                            foreach (XmlNode var in xSrCvp)
                            {
                                Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                                soru_no = var.ChildNodes[1].InnerText;
                                cevap = var.ChildNodes[2].InnerText;

                                Soru_Cevap sorcvbObj = new Soru_Cevap();

                                sorcvbObj.Soru_no = soru_no;
                                sorcvbObj.Kalem_no = Kalem_no;
                                sorcvbObj.Cevap = cevap;

                                sorucevap.Add(sorcvbObj);
                            }
                            cevaptanGelenSoruCevap = sorucevap;

                        }
                    }
                }
            }

            #endregion

            #region Belgeler
            {
                if (XMLbelgeler[0] != null)
                {
                    if (XMLbelgeler[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xBelge = XMLbelgeler[0].ChildNodes;

                        List<Belge> sBelgeler = new List<Belge>();

                        string Dogrulama;
                        string Kod;
                        string Referans;
                        string Tamamlama_tarih = "";
                        int Kalem_no;
                        foreach (XmlNode var in xBelge)
                        {
                            Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                            Kod = var.ChildNodes[1].InnerText;
                            //  Aciklama = var.ChildNodes[2].InnerText;
                            Dogrulama = var.ChildNodes[2].InnerText;
                            Referans = var.ChildNodes[4].InnerText;
                            if (var.ChildNodes[3] != null)
                                Tamamlama_tarih = var.ChildNodes[3].InnerText;

                            Belge belgeObj = new Belge();
                            //   belgeObj.Aciklama = Aciklama;
                            belgeObj.Dogrulama = Dogrulama;
                            belgeObj.Kalem_no = Kalem_no;
                            belgeObj.Kod = Kod;
                            belgeObj.Referans = Referans;
                            if (Tamamlama_tarih != "")
                                belgeObj.Belge_tarihi = Tamamlama_tarih;
                            else belgeObj.Belge_tarihi = "";

                            sBelgeler.Add(belgeObj);
                        }
                        cevaptanGelenBelgeler = sBelgeler;
                    }
                }
            }

            #endregion

            #region Vergiler
            {
                if (XMLvergiler[0] != null)
                {
                    if (XMLvergiler[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xVergi = XMLvergiler[0].ChildNodes;

                        List<Vergi> sVergiler = new List<Vergi>();
                        //  string Aciklama;
                        string Kod;
                        string Miktar;
                        string Odeme_sekli = "";
                        string Oran;
                        string Matrah;
                        int Kalem_no;

                        foreach (XmlNode var in xVergi)
                        {
                            Kalem_no = Convert.ToInt32(var.ChildNodes[0].InnerText);
                            Kod = var.ChildNodes[1].InnerText;
                            Miktar = var.ChildNodes[2].InnerText;
                            Oran = var.ChildNodes[3].InnerText;
                            Matrah = var.ChildNodes[4].InnerText;
                            if (var.ChildNodes[4] != null)
                                Odeme_sekli = var.ChildNodes[4].InnerText;


                            Vergi vergiObj = new Vergi();
                            //  vergiObj.Aciklama = Aciklama;
                            vergiObj.Kod = Kod;
                            vergiObj.Kalem_no = Kalem_no;
                            vergiObj.Miktar = Miktar.Replace(".", ",");
                            if (Odeme_sekli != "")
                                vergiObj.Odeme_sekli = Odeme_sekli;
                            vergiObj.Oran = Oran;
                            vergiObj.Matrah = Matrah;


                            sVergiler.Add(vergiObj);
                        }

                        cevaptanGelenVergiler = sVergiler;
                    }
                }
            }

            #endregion
        }
    }





}