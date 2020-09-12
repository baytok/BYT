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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BYT.WS.Controllers.Servis.OzetBeyan
{
    [Route("api/BYT/Servis/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OzetBeyanSorgulamaHizmetiController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;
        private OzetBeyanDataContext _beyannameContext;
        private readonly ServisCredential _servisCredential;
        public IConfiguration Configuration { get; }
        public OzetBeyanSorgulamaHizmetiController(IslemTarihceDataContext islemTarihcecontext, OzetBeyanDataContext beyannameContex, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            Configuration = configuration;
            _islemTarihceContext = islemTarihcecontext;
            _beyannameContext = beyannameContex;
            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }



        [HttpPost("{Guid}")]
        public async Task<ServisDurum> GetSonuc(string Guid)
        {
          
                ServisDurum _servisDurum = new ServisDurum();
                try
                {

                    SonucHizmeti.GumrukWSSoapClient sonuc = ServiceHelper.GetSonucWSClient(_servisCredential.username, _servisCredential.password);
                    SonucHizmeti.ArrayOfXElement snc = new SonucHizmeti.ArrayOfXElement();

                    snc = await sonuc.IslemSonucGetir4Async(Guid.Trim());

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(snc.Nodes[1].FirstNode.ToString());
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR", false);
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

                    try
                    {
                        var _tarihce = await _islemTarihceContext.Tarihce.FirstOrDefaultAsync(v => v.Guid == Guid);
                        var _islem = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.Guidof == _tarihce.Guid );
                        var _beyanname = await _beyannameContext.ObBeyan.FirstOrDefaultAsync(v => v.OzetBeyanInternalNo == _islem.BeyanInternalNo);

                      //  gidenXml= "<SonucBilgisi><BasariliMi>false</BasariliMi><TescilNo>20550100EX000451</TescilNo><TescilTarihi>21/08/2020 14:33:29</TescilTarihi><KalemSayisi>1</KalemSayisi><Hatalar /></SonucBilgisi>";
                      //  gidenXml = "<SonucBilgisi><BasariliMi>false</BasariliMi><TescilNo/><Hatalar><HataBilgisi><HataAciklamasi>Esya kodu hatalidir(440390009000)</HataAciklamasi ></HataBilgisi></Hatalar></SonucBilgisi>";

                        var sonucObj = SonuclariTopla(gidenXml, Guid, _islem.IslemInternalNo, _tarihce.GonderimNo, _islem.BeyanInternalNo);

                        if (sonucObj.Result != null)
                        {

                            _tarihce.IslemDurumu = "Sonuclandi";
                            _tarihce.SonucZamani = DateTime.Now;
                            _tarihce.SonIslemZamani = DateTime.Now;
                            _tarihce.SonucVeri = gidenXml;
                            _tarihce.ServistekiVeri = gelenXml;
                            _tarihce.BeyanNo = sonucObj.Result.TescilNo;

                            _islemTarihceContext.Entry(_tarihce).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();


                            _islem.IslemDurumu = "Sonuclandi";
                            _islem.IslemZamani = DateTime.Now;
                            _islem.SonIslemZamani = DateTime.Now;
                            _tarihce.BeyanNo = sonucObj.Result.TescilNo;


                            _islemTarihceContext.Entry(_islem).State = EntityState.Modified;
                            await _islemTarihceContext.SaveChangesAsync();

                            if (!string.IsNullOrEmpty(sonucObj.Result.TescilNo))
                            {
                                _beyanname.OzetBeyanNo = sonucObj.Result.TescilNo;
                                if (!string.IsNullOrWhiteSpace(sonucObj.Result.TescilTarihi))
                                    _beyanname.TescilTarihi = Convert.ToDateTime(sonucObj.Result.TescilTarihi);
                                _beyanname.SonIslemZamani = DateTime.Now;
                                _beyanname.TescilStatu = "Tescil Edildi";
                            }
                            else
                            {
                                if (sonucObj.Result.Hatalar.Count > 0)
                                {
                                    _beyanname.SonIslemZamani = DateTime.Now;
                                    _beyanname.TescilStatu = "Tescil Hatali";
                                }
                            }

                            _beyannameContext.Entry(_beyanname).State = EntityState.Modified;
                            await _beyannameContext.SaveChangesAsync();

                        }
                        else
                        {
                            _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                            List<Internal.Hata> lsthtt = new List<Internal.Hata>();

                            Hata ht = new Hata { HataKodu = 1, HataAciklamasi = "Sonuç Bilgileri Oluşmamış" };
                            lsthtt.Add(ht);
                            _servisDurum.Hatalar = lsthtt;

                            return _servisDurum;
                        }

                    }
                    catch (Exception ex)
                    {

                        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
                        List<Internal.Hata> lsthtt = new List<Internal.Hata>();

                        Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
                        lsthtt.Add(ht);
                        _servisDurum.Hatalar = lsthtt;
                        var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
                        return _servisDurum;
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

                    var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "Sorgulama İşlemler Gerçekleştirildi" };


                    return _servisDurum;


                }
                catch (Exception ex)
                {

                    return null;
                }



            }

       private async Task<OzetBeyanXmlSonuc> SonuclariTopla(string XML, string GuidOf, string InternalNo, int GonderimNo, string BeyanInternalNo)
        {
            var options = new DbContextOptionsBuilder<BeyannameSonucDataContext>()
            .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
            .Options;
            var _beyannameSonucTarihcecontext = new BeyannameSonucDataContext(options);
            XmlDocument xd = new XmlDocument();
            OzetBeyanXmlSonuc sonucObj = new OzetBeyanXmlSonuc();



            using (var transaction = _beyannameSonucTarihcecontext.Database.BeginTransaction())
            {
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
                    XmlNodeList XmlNodeListObj1 = xd.GetElementsByTagName("TescilNo");
                    XmlNodeList XmlNodeListObj3 = xd.GetElementsByTagName("TescilTarihi");
                    XmlNodeList XmlNodeListObj4 = xd.GetElementsByTagName("KalemSayisi");
                    XmlNodeList XMLhatalar = xd.GetElementsByTagName("Hatalar");


                    if (XmlNodeListObj1[0] != null)
                    {
                        if (XmlNodeListObj1[0].ChildNodes[0] != null)
                            sonucObj.TescilNo = XmlNodeListObj1[0].ChildNodes[0].Value;

                    }


                    if (XmlNodeListObj3[0] != null)
                    {
                        if (XmlNodeListObj3[0].ChildNodes[0] != null)
                            sonucObj.TescilTarihi = XmlNodeListObj3[0].ChildNodes[0].Value;
                    }

                    if (XmlNodeListObj4[0] != null)
                    {
                        if (XmlNodeListObj4[0].ChildNodes[0] != null)
                            sonucObj.KalemSayisi = XmlNodeListObj4[0].ChildNodes[0].Value;
                    }

                    #region Hatalar

                    if (XMLhatalar[0].ChildNodes.Count > 0)
                    {
                        XmlNodeList xHata = XMLhatalar[0].ChildNodes;

                        List<ObSonucHatalar> shatalar = new List<ObSonucHatalar>();

                        int hata_kodu;
                        string hata_aciklamasi;
                        int i = 0;
                        foreach (XmlNode var in xHata)
                        {
                            i++;
                            hata_kodu = i;
                            hata_aciklamasi = var.ChildNodes[0].InnerText;

                            ObSonucHatalar hataObj = new ObSonucHatalar();
                            hataObj.HataKodu = hata_kodu;
                            hataObj.HataAciklamasi = hata_aciklamasi;

                            shatalar.Add(hataObj);

                            ObSonucHatalar _hata = new ObSonucHatalar();

                            _hata.Guid = GuidOf;
                            _hata.GonderimNo = GonderimNo;
                            _hata.IslemInternalNo = InternalNo;
                            _hata.HataKodu = hata_kodu;
                            _hata.HataAciklamasi = hata_aciklamasi;
                            _hata.SonIslemZamani = DateTime.Now;
                            _beyannameSonucTarihcecontext.Entry(_hata).State = EntityState.Added;


                        }
                        await _beyannameSonucTarihcecontext.SaveChangesAsync();
                        sonucObj.Hatalar = shatalar;

                    }

                    #endregion



                    ObSonuc _obozbySouc = new ObSonuc();
                    _obozbySouc.IslemInternalNo = InternalNo;
                    _obozbySouc.Guid = GuidOf;
                    _obozbySouc.GonderimNo = GonderimNo;
                    _obozbySouc.SonIslemZamani = DateTime.Now;

                    if (XMLhatalar[0].ChildNodes.Count == 0)
                    {
                        _obozbySouc.KalemSayisi = sonucObj.KalemSayisi;
                        _obozbySouc.TescilNo = sonucObj.TescilNo;
                        _obozbySouc.TescilTarihi = sonucObj.TescilTarihi;
                        _obozbySouc.Durum = "Tescil Edildi";

                    }
                    else
                        _obozbySouc.Durum = "Tescil Hatali";

                    _beyannameSonucTarihcecontext.Entry(_obozbySouc).State = EntityState.Added;
                    await _beyannameSonucTarihcecontext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception EX)
                {
                    transaction.Rollback();
                    return null;
                }
            }
            return sonucObj;



        }

      
    }





}