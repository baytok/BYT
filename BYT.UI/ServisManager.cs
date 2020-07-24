using BYT.UI.Internal;
using BYT.UI.Models;
using BYT.UI.Models.Dto;
using Microsoft.Build.Framework;
using Raven.Abstractions.Connection;
using Raven.Database.Bundles.ScriptedIndexResults;
using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace BYT.UI
{
   
    public class ServisManager
    {
      
        private HttpClient GetClient(string token)
        {
           

            HttpClient _client = new HttpClient();
#if (DEBUG)
            _client.BaseAddress = new Uri("https://localhost:44345/api/BYT/");
#endif

#if TEST
                        _client.BaseAddress = new Uri("http://localhost:44345/api/BYT/");
#endif

#if RELEASE
                        _client.BaseAddress = new Uri("http://localhost:44345/api/BYT/");
#endif

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromMinutes(60);
         
            //_client.DefaultRequestHeaders.Authorization =
            //new AuthenticationHeaderValue(
            //"Basic",
            //Convert.ToBase64String(
            //System.Text.ASCIIEncoding.ASCII.GetBytes(
            //    string.Format("{0}:{1}", "GumrukDolasimUser", "ZnSfs_3M"))));


            return _client;
        }
        

        public  Sonuc<ServisDurum> SonuclariGetir(string Guid)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent("application/json");
            string url = string.Format(_client.BaseAddress + "Servis/SorgulamaHizmeti/" + Guid );
            var response =  _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> RefIdAl(string Rejim)
        {

            HttpClient _client = GetClient("");
            string url = string.Format(_client.BaseAddress + "BilgiHizmeti/ReferansId/" + Rejim);
            try
            {
                var content = new StringContent("application/json");
                var response = _client.PostAsync(url, content);
                string responseString = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                    return mobileEsult;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public  List<Islem> IslemleriGetirFromKullanici( string Kullanici)
        {

            HttpClient _client = GetClient("");
            string url = string.Format(_client.BaseAddress + "IslemHizmeti/KullaniciIleSorgulama/" + Kullanici );
            try
            {
                var response =  _client.GetAsync(url);
                string responseString = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    var mobileEsult = JsonConvert.DeserializeObject<List<Islem>>(responseString);
                    return mobileEsult;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public List<Tarihce> TarihceGetir(string IslemInternalNo)
        {

            HttpClient _client = GetClient("");
            string url = string.Format(_client.BaseAddress + "TarihceHizmeti/" + IslemInternalNo);
            try
            {
                var response = _client.GetAsync(url);
                string responseString = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    var mobileEsult = JsonConvert.DeserializeObject<List<Tarihce>>(responseString);
                    return mobileEsult;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Sonuc<ServisDurum> BeyannameOlustur(string Kullanici)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent("application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/" + Kullanici);
            var response = _client.PutAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> BeyannameOlustur(DbBeyan beyan)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent(JsonConvert.SerializeObject(beyan), Encoding.UTF8,"application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/BeyannameOlustur");
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> KalemOlustur(DbKalem kalem)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent(JsonConvert.SerializeObject(kalem), Encoding.UTF8, "application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/KalemOlustur");
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> OdemeSekliOlustur(DbOdemeSekli odeme)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent(JsonConvert.SerializeObject(odeme), Encoding.UTF8, "application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/OdemeSekliOlustur");
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> MarkaOlustur(DbMarka marka)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent(JsonConvert.SerializeObject(marka), Encoding.UTF8, "application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/MarkaOlustur");
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> FirmaOlustur(DbFirma firma)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent(JsonConvert.SerializeObject(firma), Encoding.UTF8, "application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/BeyannameOlusturma/FirmaOlustur");
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public  Sonuc<ServisDurum> IslemOlustur(string Kullanici)
        {

            HttpClient _client = GetClient("");
            var content = new StringContent("application/json");
            string url = string.Format(_client.BaseAddress + "IslemHizmeti/IslemOlustur/" + Kullanici);
            var response =  _client.PutAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public async Task<object> KontrolGonderGet(string BeyanInternalNo, string Kullanici)
        {

            HttpClient _client = GetClient("");
            string url = string.Format(_client.BaseAddress + "Servis/KontrolHizmeti/"+ BeyanInternalNo + "/"+ Kullanici);
            var response = await _client.GetAsync(url);
            string responseString = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var mobileEsult = responseString;
                return mobileEsult;
            }
            return null;
        }
        public Sonuc<ServisDurum> KontrolGonderPost(string IslemInternalNo, string Kullanici)
        {

            HttpClient _client = GetClient("");
        
            var content = new StringContent("application/json");
            string url = string.Format(_client.BaseAddress + "Servis/Beyanname/KontrolHizmeti/" + IslemInternalNo + "/" + Kullanici);
            var response =  _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
                return mobileEsult;
            }
            return null;
        }
        public KullaniciServisDurum GirisPost(string Kullanici, string Sifre)
        {

            HttpClient _client = GetClient("");

            var content = new StringContent("application/json");
            string url = string.Format(_client.BaseAddress + "KullaniciGiris/KullaniciHizmeti/" + Kullanici + "/" + Sifre);
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<KullaniciServisDurum>(responseString);
                return mobileEsult;
            }
            return null;
        }

        public List<Islem> IslemleriGetir(string Kullanici,string token)
        {

            HttpClient _client = GetClient(token);
            string url = string.Format(_client.BaseAddress + "IslemHizmeti/IslemListesi/" + Kullanici);
            try
            {
                var response = _client.GetAsync(url);
                string responseString = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    var mobileEsult = JsonConvert.DeserializeObject<List<Islem>>(responseString);
                    return mobileEsult;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Tarihce TarihceDetaylariGetir(string Guid,string Token)
        {

            HttpClient _client = GetClient(Token);
            string url = string.Format(_client.BaseAddress + "IslemHizmeti/IslemDatay/" + Guid);
            try
            {
                var response = _client.GetAsync(url);
                string responseString = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    var mobileEsult = JsonConvert.DeserializeObject<Tarihce>(responseString);
                    return mobileEsult;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public ServisDurum TarihceGuncellePost(string imzaliVeri, string Guid,string Token)
        {

            HttpClient _client = GetClient(Token);

            var content = new StringContent(JsonConvert.SerializeObject(imzaliVeri), Encoding.UTF8, "application/json");
            string url = string.Format(_client.BaseAddress + "IslemHizmeti/DetayGuncelle/" + Guid );
            var response = _client.PostAsync(url, content);
            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                var mobileEsult = JsonConvert.DeserializeObject<ServisDurum>(responseString);
                return mobileEsult;
            }
            return null;
        }
        //public Sonuc<ServisDurum> BeyannameOlusturma(BeyannameBilgileri param)
        //{
        //    HttpClient _client = GetClient();
        //    string jsonData = JsonConvert.SerializeObject(param);
        //    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
        //    string url = string.Format(_client.BaseAddress + "api/BeyannameOlusturma/BeyannameOlustur", param);

        //    var response = _client.PostAsync(url, content).Result;
        //    string responseString = response.Content.ReadAsStringAsync().Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
        //        return mobileEsult;
        //    }
        //    else return null;

        //}

        //public void TarifeVeriTabaniOlustur()
        //{
        //    HttpClient _client = GetClient();

        //    var content = new StringContent("application/json");
        //    string url = string.Format(_client.BaseAddress + "api/Tarife/TarifeVeriTabaniOlustur");
        //    var response = _client.PostAsync(url, content).Result;
        //    string responseString = response.Content.ReadAsStringAsync().Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);

        //    }


        //}

        //public List<TarifeVergiOranlari> VergiSorgulama(string Tarife, string Tarih)
        //{
        //    HttpClient _client = GetClient();

        //    string url = string.Format(_client.BaseAddress + "api/Tarife/VergiSorgulama?Tarife={0}&&Tarih={1}", Tarife, Tarih);
        //    var response = _client.GetAsync(url).Result;
        //    string responseString = response.Content.ReadAsStringAsync().Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var mobileEsult = JsonConvert.DeserializeObject<List<TarifeVergiOranlari>>(responseString);
        //        return mobileEsult;
        //    }
        //    else return null;


        //}

        //public Sonuc<ServisDurum> BeyannameOlusturmaImzali(string param)
        //{
        //    HttpClient _client = GetClient();

        //    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
        //    string url = string.Format(_client.BaseAddress + "api/BeyannameOlusturma/BeyannameOlusturmaImzali", param);
        //    var response = _client.PostAsync(url, content).Result;
        //    string responseString = response.Content.ReadAsStringAsync().Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var mobileEsult = JsonConvert.DeserializeObject<Sonuc<ServisDurum>>(responseString);
        //        return mobileEsult;
        //    }
        //    else return null;

        //}
    }
}

