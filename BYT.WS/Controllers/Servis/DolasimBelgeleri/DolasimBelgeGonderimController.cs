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

namespace BYT.WS.Controllers.Servis.DolasimBelgeleri
{
    // [Route("api/BYT/Servis/Beyanname/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DolasimBelgeGonderimController : ControllerBase
    {
        private IslemTarihceDataContext _islemTarihceContext;

        private readonly ServisCredential _servisCredential;

        public IConfiguration Configuration { get; }
        public DolasimBelgeGonderimController(IslemTarihceDataContext islemTarihcecontext, IOptions<ServisCredential> servisCredential, IConfiguration configuration)
        {
            _islemTarihceContext = islemTarihcecontext;
            Configuration = configuration;


            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }



        [Route("api/BYT/Servis/Dolasim/[controller]/{IslemInternalNo}/{Kullanici}")]
        [HttpPost("{IslemInternalNo}/{Kullanici}")]
        public async Task<ServisDurum> GetBasvuru(string IslemInternalNo, string Kullanici)
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


                DolasimBelgeHizmeti.TobbMedosPortTypeClient _servis = ServiceHelper.GetDolasimBelgeWSClient(_servisCredential.username, _servisCredential.password);

                DolasimBelgeHizmeti.IYO321Request belge0321req = new DolasimBelgeHizmeti.IYO321Request();
                belge0321req.BelgeNo = "ç3211018";
                belge0321req.BeyanTipi = "TCGB";
                belge0321req.Evraklar = "";
                belge0321req.Faturalar = "";
                belge0321req.GumrukId = 66;
                belge0321req.IhracatciAdi = "Hasan ";
                belge0321req.IhracatciAdres = "Ankara";
                belge0321req.IhracatciUlkeId = 66;
                belge0321req.IhracatciVergiNo = "11111";
                belge0321req.IhracatciYer = "Ankara";
                belge0321req.IhracUlkesiId = 66;
                belge0321req.KapasiteRaporuTarihi = "";
                belge0321req.KapasiteRaporuVizeNo = 0;
                belge0321req.MalinGonderildigiSahisAdi = "Osman";
                belge0321req.MalinGonderildigiSahisAdres = "Antalya";
                belge0321req.MalinGonderildigiSahisUlkeId = 66;
                belge0321req.Mallar = "";
                belge0321req.Sifre = "747464";
                belge0321req.SignedMime = "";
                belge0321req.TaahhutnameSecimi = "Evet";
                belge0321req.TasimaBelgesiNo = "12345";
                belge0321req.TasimaBelgesiTarihi = "";
                belge0321req.TasimayaIliskinBilgiler = "";
                belge0321req.TcKimlikNo = "15781158208";
                belge0321req.VarisUlkesiId = 66;


                DolasimBelgeHizmeti.IYO321Response belge0321res = new DolasimBelgeHizmeti.IYO321Response();
                belge0321res = await _servis.IYO321Async(belge0321req);

                var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
                   .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
                   .Options;
                KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
                var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

         


                string guidOf = "", IslemDurumu = "", islemSonucu = "";



                // string InnerXml = "<RefID> E20 - 3037 </RefID ><Guid> 7fa7c6f4 - f837 - 4afb - 8f1b - 952987c9bd44 </Guid ><Durum> İşleminiz başlamıştır.Teşekkür ederiz.</Durum >";
                //string outerxml = "<Response><RefID>E20-3037</RefID><Guid>7fa7c6f4-f837-4afb-8f1b-952987c9bd44</Guid><Durum>İşleminiz başlamıştır.Teşekkür ederiz.</Durum></Response>";
                //string hataInnerXml = "<Message>Elektronik İmza ile kullanıcı kodu üzerindeki bilgiler uyumsuz.</Message>";
                //string hataOuterXml = "<Error><Message>Elektronik İmza ile kullanıcı kodu üzerindeki bilgiler uyumsuz.</Message></Error>";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(belge0321res.Veri);


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
                                IslemDurumu = "Islemde";
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
                        islemValues.IslemDurumu = "DolasimBelgeBasvuru";
                        islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("DO", "DOG");
                        islemValues.IslemZamani = DateTime.Now;
                        islemValues.SonIslemZamani = DateTime.Now;
                        islemValues.IslemSonucu = islemSonucu;
                        islemValues.Guidof = guidOf;
                        islemValues.IslemTipi = "DolBlgBsv";
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
                        _tarihce.IslemTipi = "7";
                        _tarihce.Rejim = "Bşv";
                        _tarihce.TicaretTipi = "Bşv";
                        _tarihce.GonderilenVeri = _tarihce.GonderilecekVeri = SerializeToXML(belge0321req);
                        _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
                        _tarihce.GonderimNo = islemValues.GonderimSayisi;
                        _tarihce.SonIslemZamani = DateTime.Now;

                        _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
                        await _islemTarihceContext.SaveChangesAsync();

                        //mesaiValues.SonIslemZamani = DateTime.Now;
                        //mesaiValues.TescilStatu = "Dolaşım Belgesi Basvuru";
                        //_beyannameContext.Entry(mesaiValues).State = EntityState.Modified;
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
                Bilgi blg = new Bilgi { IslemTipi = "Dolaşım Belgesi Başvuru Gönderimi", ReferansNo = guidOf, GUID = guidOf, Sonuc = "Dolaşım Belges, Başvuru Gönderimi Gerçekleşti", SonucVeriler = null };
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


        //[Route("api/BYT/Servis/Beyanname/[controller]/{IslemInternalNo}/{Kullanici}")]
        //[HttpPost("{IslemInternalNo}/{Kullanici}")]
        //public async Task<ServisDurum> GetMesaiIptal(string IslemInternalNo, string Kullanici)
        //{
        //    ServisDurum _servisDurum = new ServisDurum();
        //    var options = new DbContextOptionsBuilder<BeyannameDataContext>()
        //           .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //           .Options;
        //    var _beyannameContext = new BeyannameDataContext(options);
        //    try
        //    {
        //        var islemValues = await _islemTarihceContext.Islem.FirstOrDefaultAsync(v => v.IslemInternalNo == IslemInternalNo.Trim() && v.Kullanici == Kullanici.Trim());
        //        var mesaiValues = await _beyannameContext.Mesai.FirstOrDefaultAsync(v => v.MesaiInternalNo == islemValues.BeyanInternalNo);

        //        MesaiHizmeti.Gumruk_Biztalk_MesaiBasvuruSoapClient mesaiServis = ServiceHelper.GetMesaiBildirWsClient(_servisCredential.username, _servisCredential.password);

        //        MesaiHizmeti.MesaiBasvuruIptal mesai = new MesaiHizmeti.MesaiBasvuruIptal();


        //        var optionss = new DbContextOptionsBuilder<KullaniciDataContext>()
        //              .UseSqlServer(new SqlConnection(Configuration.GetConnectionString("BYTConnection")))
        //              .Options;
        //        KullaniciDataContext _kullaniciContext = new KullaniciDataContext(optionss);
        //        var kullaniciValues = await _kullaniciContext.Kullanici.Where(v => v.KullaniciKod == Kullanici).FirstOrDefaultAsync();

        //        mesai.KullaniciAdi = "15781158208"; // islemValues.Kullanici;
        //        mesai.RefID = islemValues.RefNo;
        //        mesai.Sifre = "19cd21ebad3e08b8f1955b6461bd2f41"; //Md5Helper.getMd5Hash(kullaniciValues.KullaniciSifre);
        //        mesai.IP = "";
        //        mesai.MesaiID = mesaiValues.MesaiID;


        //         var values = await mesaiServis.MesaiBasvuruIptalAsync(mesai);

        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(values.Root.OuterXml);
        //        string guidOf = "", IslemDurumu = "", islemSonucu = "";
        //        if (doc.HasChildNodes)
        //        {
        //            foreach (XmlNode n in doc.ChildNodes[0].ChildNodes)
        //            {

        //                if (n.Name == "Message")
        //                {
        //                    IslemDurumu = "Hata";
        //                    islemSonucu = n.InnerText;
        //                    break;
        //                }

        //                else
        //                {
        //                    if (n.Name == "Guid")
        //                    {
        //                        IslemDurumu = "Islemde";
        //                        guidOf = n.InnerText;


        //                    }
        //                    else if (n.Name == "Durum")
        //                    {
        //                        islemSonucu = n.InnerText;


        //                    }
        //                }

        //            }
        //        }


        //        if (string.IsNullOrWhiteSpace(guidOf))
        //            guidOf = "BYT:" + Guid.NewGuid().ToString();

        //        using (var transaction = _islemTarihceContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                islemValues.Kullanici = Kullanici;
        //                islemValues.IslemDurumu = "MesaiIptalGonderildi";
        //                islemValues.IslemInternalNo = islemValues.BeyanInternalNo.Replace("MB", "MBG");
        //                islemValues.IslemZamani = DateTime.Now;
        //                islemValues.SonIslemZamani = DateTime.Now;
        //                islemValues.IslemSonucu = islemSonucu;
        //                islemValues.Guidof = guidOf;
        //                islemValues.IslemTipi = "MesaiIptal";
        //                islemValues.GonderimSayisi++;


        //                _islemTarihceContext.Entry(islemValues).State = EntityState.Modified;
        //                await _islemTarihceContext.SaveChangesAsync();


        //                Tarihce _tarihce = new Tarihce();
        //                _tarihce.Guid = guidOf;
        //                _tarihce.Gumruk = mesaiValues.GumrukKodu;                       
        //                _tarihce.IslemInternalNo = islemValues.IslemInternalNo;
        //                _tarihce.Kullanici = Kullanici;
        //                _tarihce.RefNo = islemValues.RefNo;
        //                _tarihce.IslemDurumu = IslemDurumu;
        //                _tarihce.IslemSonucu = islemSonucu;
        //                _tarihce.IslemTipi = "6";
        //                 _tarihce.GonderilenVeri = _tarihce.GonderilecekVeri = SerializeToXML(mesai);
        //                _tarihce.GondermeZamani = _tarihce.OlusturmaZamani = DateTime.Now;
        //                _tarihce.GonderimNo = islemValues.GonderimSayisi;
        //                _tarihce.SonIslemZamani = DateTime.Now;

        //                _islemTarihceContext.Entry(_tarihce).State = EntityState.Added;
        //                await _islemTarihceContext.SaveChangesAsync();

        //                mesaiValues.SonIslemZamani = DateTime.Now;
        //                mesaiValues.TescilStatu = "Mesai Iptal Gonderildi";
        //                _beyannameContext.Entry(mesaiValues).State = EntityState.Modified;
        //                await _islemTarihceContext.SaveChangesAsync();
        //                await _beyannameContext.SaveChangesAsync();
        //                transaction.Commit();

        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.BeyannameKayitHatasi;
        //                List<Internal.Hata> lsthtt = new List<Internal.Hata>();

        //                Hata ht = new Hata { HataKodu = 1, HataAciklamasi = ex.Message };
        //                lsthtt.Add(ht);
        //                _servisDurum.Hatalar = lsthtt;
        //                var rresult = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekleştirilemedi" };
        //                return _servisDurum;
        //            }
        //        }



        //        _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

        //        List<Bilgi> lstBlg = new List<Bilgi>();
        //        List<Internal.Hata> lstht = new List<Internal.Hata>();

        //        if (IslemDurumu == "Hata")
        //        {
        //            Internal.Hata ht = new Internal.Hata { HataKodu = 1, HataAciklamasi = islemSonucu };
        //            lstht.Add(ht);
        //        }
        //        Bilgi blg = new Bilgi { IslemTipi = "Kontrol Gönderimi", ReferansNo = guidOf, GUID = guidOf, Sonuc = "Kontrol Gönderimi Gerçekleşti", SonucVeriler = null };
        //        lstBlg.Add(blg);


        //        _servisDurum.Bilgiler = lstBlg;
        //        _servisDurum.Hatalar = lstht;



        //        return _servisDurum;

        //    }
        //    catch (Exception ex)
        //    {

        //        List<Internal.Hata> lstht = new List<Internal.Hata>();
        //        Internal.Hata ht = new Internal.Hata { HataKodu = 1, HataAciklamasi = ex.ToString() };
        //        lstht.Add(ht);
        //        _servisDurum.Hatalar = lstht;

        //        return _servisDurum;
        //    }


        //}
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