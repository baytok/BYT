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
    public class MesaiGonderimController : ControllerBase
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
        public MesaiGonderimController(IslemTarihceDataContext islemTarihcecontext, BeyannameSonucDataContext sonucContext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
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
        public async Task<ServisDurum> GetMesaiBasvuru(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var mesaiValues = await _beyannameContext.Mesai.FirstOrDefaultAsync(v => v.MesaiInternalNo == islemValues.BeyanInternalNo);
               

                MesaiHizmeti.Gumruk_Biztalk_MesaiBasvuruSoapClient mesaiServis = ServiceHelper.GetMesaiBildirWsClient(_servisCredential.username, _servisCredential.password);

                MesaiHizmeti.MesaiBasvuru mesai = new MesaiHizmeti.MesaiBasvuru();


                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                      .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

                mesai.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
                mesai.RefID = islemValues.RefNo;
                mesai.Sifre =  "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
                mesai.IP = "";
                mesai.Adres = mesaiValues.Adres;
                mesai.AracAdedi = mesaiValues.AracAdedi;
                mesai.BaslangicZamani = mesaiValues.BaslangicZamani;
                mesai.BeyannameNo = mesaiValues.BeyannameNo;
                mesai.DigerNo = mesaiValues.DigerNo;
                mesai.EsyaninBulunduguYer = mesaiValues.EsyaninBulunduguYer;
                mesai.EsyaninBulunduguYerAdi = mesaiValues.EsyaninBulunduguYerAdi;
                mesai.EsyaninBulunduguYerKodu = mesaiValues.EsyaninBulunduguYerKodu;
                mesai.FirmaVergiNo = mesaiValues.FirmaVergiNo;
                mesai.GlobalHesaptanOdeme = mesaiValues.GlobalHesaptanOdeme;
                mesai.GumrukKodu = mesaiValues.GumrukKodu;
                mesai.GumrukSahasinda = mesaiValues.GumrukSahasinda;
                mesai.IrtibatAdSoyad = mesaiValues.IrtibatAdSoyad;
                mesai.IrtibatTelefonNo = mesaiValues.IrtibatTelefonNo;
                mesai.IslemTipi = mesaiValues.IslemTipi;
                mesai.NCTSSayisi = mesaiValues.NCTSSayisi;
                mesai.OdemeYapacakFirmaVergiNo = mesaiValues.OdemeYapacakFirmaVergiNo;
                mesai.OZBYSayisi = mesaiValues.OZBYSayisi;
                mesai.Uzaklik = mesaiValues.Uzaklik;

                var values = await mesaiServis.MesaiBasvurusuYapAsync(mesai);

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
                        islemValues.IslemDurumu = "MesaiBasvuruGonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("MB", "MBG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "MesaiBasvuru";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = mesaiValues.GumrukKodu;
                        _tarihce.IslemInternalNo = islemValues.IslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = IslemDurumu;
                        _tarihce.IslemSonucu = islemSonucu;
                        _tarihce.IslemTipi = "6";
                        _tarihce.GonderilenVeri = _tarihce.GonderilecekVeri = SerializeToXML(mesai);
                        _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
                        await _islemTarihceContext.SaveChangesAsync();

                        mesaiValues.SonIslemZamani = DateTime.Now;
                        mesaiValues.TescilStatu = "Mesai Basvuru Gonderildi";
                        _beyannameContext.Entry(mesaiValues).State = EntityState.Modified;
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
        [Route("api/BYT/Servis/Beyanname/[controller]/{IslemInternalNo}/{Kullanici}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}")]
        public async Task<ServisDurum> GetMesaiIptal(string IslemInternalNo, string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            var options = new DbContextOptionsBuilder<BeyannameDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
            var _beyannameContext = new BeyannameDataContext(options);
            try
            {
                var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
                var mesaiValues = await _beyannameContext.Mesai.FirstOrDefaultAsync(v => v.MesaiInternalNo == islemValues.BeyanInternalNo);

                MesaiHizmeti.Gumruk_Biztalk_MesaiBasvuruSoapClient mesaiServis = ServiceHelper.GetMesaiBildirWsClient(_servisCredential.username, _servisCredential.password);

                MesaiHizmeti.MesaiBasvuruIptal mesai = new MesaiHizmeti.MesaiBasvuruIptal();


                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                      .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                      .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

                mesai.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
                mesai.RefID = islemValues.RefNo;
                mesai.Sifre = "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
                mesai.IP = "";
                mesai.MesaiID = mesaiValues.MesaiID;
               

                 var values = await mesaiServis.MesaiBasvuruIptalAsync(mesai);

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
                        islemValues.IslemDurumu = "MesaiIptalGonderildi";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("MB", "MBG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "MesaiIptal";
                        islemValues.GonderimSayisi++;


                        _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
                        await _islemTarihceContext.SaveChangesAsync();


                        Tarihce _tarihce = new Tarihce();
                        _tarihce.Guid = guidOf;
                        _tarihce.Gumruk = mesaiValues.GumrukKodu;                       
                        _tarihce.IslemInternalNo = islemValues.IslemInternalNo;
                        _tarihce.Kullanici = Kullanici;
                        _tarihce.RefNo = islemValues.RefNo;
                        _tarihce.IslemDurumu = IslemDurumu;
                        _tarihce.IslemSonucu = islemSonucu;
                        _tarihce.IslemTipi = "6";
                         _tarihce.GonderilenVeri = _tarihce.GonderilecekVeri = SerializeToXML(mesai);
                        _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
                        await _islemTarihceContext.SaveChangesAsync();

                        mesaiValues.SonIslemZamani = DateTime.Now;
                        mesaiValues.TescilStatu = "Mesai Iptal Gonderildi";
                        _beyannameContext.Entry(mesaiValues).State = EntityState.Modified;
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