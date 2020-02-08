﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT.WS.AltYapi;
using BYT.WS.Data;
using BYT.WS.Internal;
using BYT.WS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BYT.WS.Controllers.api
{
   
    [ApiController]
    public class IslemHizmetiController : ControllerBase
    {
        private IslemTarihceDataContext _islemContext;

        private readonly ServisCredential _servisCredential;

        public IslemHizmetiController(IslemTarihceDataContext islemcontext, IOptions<ServisCredential> servisCredential)
        {
            _islemContext = islemcontext;

            _servisCredential = new ServisCredential();
            _servisCredential.username = servisCredential.Value.username;
            _servisCredential.password = servisCredential.Value.password;
        }

        [Route("api/BYT/[controller]")]
        [HttpGet]
        public async Task<Sonuc<ServisDurum>> Get()
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var islemValues = await _islemContext.Islem.ToListAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = "", Sonuc = "Sorgulama Başarılı", SonucVeriler = islemValues };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;

            }
            catch (Exception ex)
            {

                return null;
            }


        }
        [Route("api/BYT/[controller]/KullaniciIleSorgulama/{Kullanici}")]
        [HttpGet("{Kullanici}")]
        public async Task<List<Islem>> GetIslemlerFromKullanici(string Kullanici)
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var islemValues =  await _islemContext.Islem.Where(v => v.Kullanici == Kullanici).ToListAsync();
                //var result = new Sonuc<object>() { Veri = islemValues, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
               
                return islemValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }
        [Route("api/BYT/[controller]/RefIdIleSorgulama/{refId}")]
        [HttpGet("{refId}")]
        public async Task<List<Islem>> GetIslemlerFromRefId(string refId)
        {
            try
            {
                ServisDurum _servisDurum = new ServisDurum();

                var islemValues = await _islemContext.Islem.Where(v => v.RefId == refId).ToListAsync();

                //_servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                //List<Bilgi> lstBlg = new List<Bilgi>();
                //Bilgi blg = new Bilgi { IslemTipi = "Sorgulama", ReferansNo = refId, Sonuc = "Sorgulama Başarılı", SonucVeriler = islemValues };
                //lstBlg.Add(blg);
                //_servisDurum.Bilgiler = lstBlg;

                //var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return islemValues;

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemOlustur/{Kullanici}")]
        [HttpPut("{Kullanici}")]
        public async Task<Sonuc<ServisDurum>> PutIslem(string Kullanici)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {
                Islem _islem = new Islem();
                _islem.Kullanici = Kullanici;
                _islem.IslemTipi = "Kontrol";
                _islem.BeyanTipi = "DetayliBeyan";
                _islem.IslemDurumu = "Olusturuldu";
                _islem.RefId = "000002";
                _islem.IslemInternalNo = Kullanici+"G"+"000002"; //TODO: sequence sayı alıp 6 ya tamamlayalım
                _islem.OlusturmaZamani = DateTime.Now;
                _islem.GonderimSayisi=0;

                _islemContext.Entry(_islem).State = EntityState.Added;
                await _islemContext.SaveChangesAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = Kullanici, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [Route("api/BYT/[controller]/IslemOlustur")]
        [HttpPut()]
        public async Task<Sonuc<ServisDurum>> Put(Islem _islem)
        {
            ServisDurum _servisDurum = new ServisDurum();
            try
            {
               

                _islemContext.Entry(_islem).State = EntityState.Added;
                await _islemContext.SaveChangesAsync();

                _servisDurum.ServisDurumKodlari = ServisDurumKodlari.IslemBasarili;

                List<Bilgi> lstBlg = new List<Bilgi>();
                Bilgi blg = new Bilgi { IslemTipi = "İşlem Oluştur", ReferansNo = _islem.Kullanici, Sonuc = "İşlem Oluşturma Başarılı", SonucVeriler = null };
                lstBlg.Add(blg);
                _servisDurum.Bilgiler = lstBlg;

                var result = new Sonuc<ServisDurum>() { Veri = _servisDurum, Islem = true, Mesaj = "İşlemler Gerçekletirildi" };
                //string jsonData = JsonConvert.SerializeObject(result, Formatting.None);

                return result;
            }
            catch (Exception ex)
            {

                return null;
            }


        }
    }





}