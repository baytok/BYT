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
    public class IGHBGonderimController : ControllerBase
    {
      
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;
        private BeyannameSonucDataContext _sonucContext;
        public IConfiguration Configuration { get; }
        public IGHBGonderimController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
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
        public async Task<ServisDurum> GetIgbh(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var ighbValues= await _beyannameContext.Ighb.FirstOrDefaultAsync(v => v.IghbInternalNo == islemValues.BeyanInternalNo);
                var ighbListValues = await _beyannameContext.IghbListe.Where(v => v.IghbInternalNo == islemValues.BeyanInternalNo).ToListAsync();

                IGHBHizmeti.Gumruk_Biztalk_IGHBClient ighbServis = ServiceHelper.GetIGHBWSClient(_servisCredential.username, _servisCredential.password);

                IGHBHizmeti.IGHBGelen gelen = new IGHBHizmeti.IGHBGelen();
                IGHBHizmeti.IGHB _ighb = new IGHBHizmeti.IGHB();
                List<IGHBHizmeti.TCGBBilgi> tcgbler = new List<IGHBHizmeti.TCGBBilgi>();
                _ighb.GumrukKodu = ighbValues.GumrukKodu;
                _ighb.IzinliGondericiVergiNo = ighbValues.IzinliGondericiVergiNo;
                _ighb.PlakaBilgisi = ighbValues.PlakaBilgisi;
                _ighb.TesisKodu = ighbValues.TesisKodu;
                foreach (var item in ighbListValues)
                {
                    IGHBHizmeti.TCGBBilgi tcgb = new IGHBHizmeti.TCGBBilgi();
                    tcgb.TCGBNumarasi = item.TcgbNumarasi;
                    tcgbler.Add(tcgb);
                }

                _ighb.TCGBBilgiListesi = tcgbler.ToArray();

            

                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                      .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

                gelen.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
                gelen.RefID = islemValues.RefNo;
                gelen.Sifre =  "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
                gelen.IGHB = _ighb;
                var values = await ighbServis.IzinliGondericiHazirBildirimiYapAsync(gelen);

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
                        islemValues.IslemDurumu = "IghbGonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("IB", "IBG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "Ighb";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = ighbValues.GumrukKodu;
                        _tarihce.Rejim = "EX";
                        _tarihce.IslemInternalNo = islemValues.IslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = IslemDurumu;
                        _tarihce.IslemSonucu = islemSonucu;
                        _tarihce.IslemTipi = "5";
                        _tarihce.TicaretTipi = "EX";
                        _tarihce.GonderilenVeri = _tarihce.GonderilecekVeri = SerializeToXML(gelen);
                        _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
                        await _islemTarihceContext.SaveChangesAsync();

                        ighbValues.SonIslemZamani = DateTime.Now;
                        ighbValues.TescilStatu = "Ighb Gonderildi";
                        _beyannameContext.Entry(ighbValues).State = EntityState.Modified;
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