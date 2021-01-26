import {
  mergeMap as _observableMergeMap,
  catchError as _observableCatch
} from "rxjs/operators";
import {
  Observable,
  throwError as _observableThrow,
  of as _observableOf
} from "rxjs";
import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import {
  HttpClient,
  HttpParams,
  HttpHeaders,
  HttpResponse,
  HttpResponseBase,
  HttpErrorResponse,
  JsonpInterceptor
} from "@angular/common/http";
import { DecimalPipe } from "@angular/common";
import { parse } from 'querystring';
export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");
import { GirisService } from '../../account/giris/giris.service';
import { Router } from "@angular/router";
@Injectable()
export class BeyannameServiceProxy {
  
  
  private http: HttpClient;
  private baseUrl: string;

  protected jsonParseReviver:
    | ((key: string, value: any) => any)
    | undefined = undefined;

  constructor(
    private router:Router,
    private girisService:GirisService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string,
   
   
  ) {
    this.http = http;
    this.baseUrl = baseUrl ? baseUrl : "";
  
  }
 errorHandel(err:any)
 {
  if (err instanceof HttpErrorResponse) {
    if (err.status == 401) {
      console.log(err);
      localStorage.removeItem('kullaniciInfo');   
      this.router.navigateByUrl('/giris');
   
  }
  }
 }

  notAuthorizeRole()
  {

      localStorage.removeItem('kullaniciInfo');   
      this.router.navigateByUrl('/giris');
   
 
  }
  getAllKullanici() { 
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var kullaniciId = currentUser.user;
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Kullanicilar/KullaniciHizmeti/"+kullaniciId,httpOptions
    )
  }

  setKullanici(kullanici: KullaniciDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
  

    return this.http.post<any>(
      this.baseUrl + "KullaniciOlustur/KullaniciHizmeti", 
      kullanici,httpOptions  
      );
  }
  restoreKullanici(kullanici: KullaniciDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };

      return this.http.put<any>(
        this.baseUrl + "KullaniciDegistir/KullaniciHizmeti", 
        kullanici,httpOptions);
  }
  removeKullanici(kullaniciId) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
    return this.http.delete<any>(
      this.baseUrl + "KullaniciSil/KullaniciHizmeti/"+kullaniciId, httpOptions        
      );
  }
  getAllYetki() { 
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Yetkiler/KullaniciHizmeti/",httpOptions
    )
  }
  getAllAktifYetkiler() {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var kullaniciId = currentUser.user;
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "AktifYetkiler/KullaniciHizmeti/"+kullaniciId,httpOptions 
    );
  }
  getAllKullaniciAktifYetkiler(kullaniciKod) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "AktifKullaniciYetkiler/KullaniciHizmeti/"+kullaniciKod,httpOptions 
    );
  }
  setYetki(yetki: YetkiDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
  

    return this.http.post<any>(
      this.baseUrl + "YetkiOlustur/KullaniciHizmeti", 
      yetki,httpOptions  
      );
  }
  restoreYetki(yetki: YetkiDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };

      return this.http.put<any>(
        this.baseUrl + "YetkiDegistir/KullaniciHizmeti", 
        yetki,httpOptions);
  }
  removeYetki(yetkiId) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
    return this.http.delete<any>(
      this.baseUrl + "YetkiSil/KullaniciHizmeti/"+yetkiId, httpOptions        
      );
  }
  setYetkiKullanici(yetkiKullanici: KullaniciYetkiDto[], kullaniciKod:string) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
  

    return this.http.post<any>(
      this.baseUrl + "KullaniciYetkiOlustur/KullaniciHizmeti/"+kullaniciKod, 
      yetkiKullanici,httpOptions  
      );
  }
  restoreYetkiKullanici(yetkiKullanici: KullaniciYetkiDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };

      return this.http.put<any>(
        this.baseUrl + "KullaniciYetkiDegistir/KullaniciHizmeti", 
        yetkiKullanici,httpOptions);
  }
  getAllAktifMusteriler() {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var kullaniciId = currentUser.user;
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
   return this.http.get(
      this.baseUrl + "AktifMusteriler/KullaniciHizmeti/"+kullaniciId,httpOptions 
    );
  }
  getAllMusteri() {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Musteriler/KullaniciHizmeti/",httpOptions 
    );
  }
  setMusteri(musteri: MusteriDto) {
    
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "MusteriOlustur/KullaniciHizmeti", 
      musteri,httpOptions  
      );
  }
  restoreMusteri(musteri: MusteriDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.put<any>(
        this.baseUrl + "MusteriDegistir/KullaniciHizmeti", 
        musteri,httpOptions  
        );
  }
  removeMusteri(musteriId) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.delete<any>(
      this.baseUrl + "MusteriSil/KullaniciHizmeti/"+musteriId, httpOptions        
      );
  }

  getAllAktifFirmalar(musteriNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var kullaniciId = currentUser.user;
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "AktifFirmalar/KullaniciHizmeti/"+musteriNo+"/"+kullaniciId,httpOptions 
    );
  }
  getAllFirma() {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var kullaniciId = currentUser.user;
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Firmalar/KullaniciHizmeti/"+kullaniciId,httpOptions 
    );
  }
  setFirma(firma: FirmaDto) {
    
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "FirmaOlustur/KullaniciHizmeti", 
      firma,httpOptions  
      );
  }
  restoreFirma(firma: FirmaDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.put<any>(
        this.baseUrl + "FirmaDegistir/KullaniciHizmeti", 
        firma,httpOptions  
        );
  }
  removeFirma(firmaiId) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.delete<any>(
      this.baseUrl + "FirmaSil/KullaniciHizmeti/"+firmaiId, httpOptions        
      );
  }

  getAllIslem(Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "IslemHizmeti/KullaniciIleSorgulama/" + Kullanici,httpOptions
    );
  }
  getAllIslemFromRefNo(refNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "IslemHizmeti/RefNoIleSorgulama/" + refNo,httpOptions
    );
  }
  getTarihce(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(this.baseUrl + "TarihceHizmeti/" + IslemInternalNo, httpOptions);
  }
  KontrolGonderimi(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameKontrolGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  MesaiGonderimi(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/MesaiGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  IghbGonderimi(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/IGHBGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  
  BeyannameTescilMesajiHazirla(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  BeyannameTescilGonderimi(IslemInternalNo, Kullanici, guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici+"/"+guid,null,httpOptions  
        );
   
    
  }
  OzetBeyanTescilMesajiHazirla(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  OzetBeyanTescilGonderimi(IslemInternalNo, Kullanici, guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
  
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici+"/"+guid,null,httpOptions  
        );
   
    
  }
  NctsTescilMesajiHazirla(IslemInternalNo, Kullanici) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Ncts/NctsTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici,null,httpOptions  
        );
   
    
  }
  NctsTescilGonderimi(IslemInternalNo, Kullanici, guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
    console.log(guid);
      return this.http.post<any>(
        this.baseUrl + "Servis/Ncts/NctsTescilGonderim/" +
        IslemInternalNo + "/" + Kullanici+"/"+guid,null,httpOptions  
        );
   
    
  }
  
  getBeyannameServisSonucSorgula(Guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/BeyannameSorgulamaHizmeti/" + Guid,null,
      httpOptions
    );
  }
  getOzetBeyanServisSonucSorgula(Guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/OzetBeyanSorgulamaHizmeti/" + Guid,null,
      httpOptions
    );
  }
 
  getNctsServisSonucSorgula(Guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/NctsSorgulamaHizmeti/" + Guid,null,
      httpOptions
    );
  }
  
  getMesaiServisSonucSorgula(Guid) {
    
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/MesaiSorgulamaHizmeti/" + Guid,null,
      httpOptions
    );


  }

  getIghbServisSonucSorgula(Guid) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/IghbSorgulamaHizmeti/" + Guid,null,
      httpOptions
    );

  
  }

  getBeyannameSonucSorgula(Guid, IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl +
        "Servis/Beyanname/BeyannameSonucHizmeti/" +
        Guid +
        "/" +
        IslemInternalNo,httpOptions
    );
  }
  getMesaiSonucSorgula(Guid, IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl +
        "Servis/Beyanname/MesaiSonucHizmeti/" +
        Guid +
        "/" +
        IslemInternalNo,httpOptions
    );
  }
  getIghbSonucSorgula(Guid, IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl +
        "Servis/Beyanname/IghbSonucHizmeti/" +
        Guid +
        "/" +
        IslemInternalNo,httpOptions
    );
  }
  getOzetBeyanSonucSorgula(Guid, IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl +
        "Servis/OzetBeyan/OzetBeyanSonucHizmeti/" +
        Guid +
        "/" +
        IslemInternalNo,httpOptions
    );
  }

  getKalem(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Servis/Kalemler/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  removeKalem(kalemInternalNo,beyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KalemSil/"+kalemInternalNo+"/"+beyanInternalNo, httpOptions        
      );
  }
  restoreKalem(kalem: KalemDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KalemOlustur/", 
        kalem,httpOptions  
        );
  }
  getOdeme(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Odeme/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreOdeme(odeme: OdemeDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/OdemeSekliOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        odeme,httpOptions  
        );
  }
  getKonteyner(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Konteyner/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreKonteyner(konteyner: KonteynerDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KonteynerOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        konteyner,httpOptions  
        );
  } 
  getTamamlayici(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TamamlayiciBilgi/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreTamamlayici(tamamlayici: TamamlayiciBilgiDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/TamamlayiciBilgiOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        tamamlayici,httpOptions  
        );
  }
  getMarka(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Marka/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreMarka(tamamlayici: MarkaDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/MarkaOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        tamamlayici,httpOptions  
        );
  }
  getDbBeyannameAcma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/BeyannameAcma/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreDbBeyannameAcma(acma: BeyannameAcmaDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BeyannameAcmaOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        acma,httpOptions  
        );
  }
  getVergi(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Vergi/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreVergi(vergi: VergiDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/VergiOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        vergi,httpOptions  
        );
  }
  getBelge(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Belge/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreBelge(belge: BelgeDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BelgeOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        belge,httpOptions  
        );
  }
  getSoruCevap(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/SoruCevap/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreSoruCevap(belge: SoruCevapDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/SoruCevapOlustur/"+kalemInternalNo+"/"+beyanInternalNo, 
        belge,httpOptions  
        );
  }
  getKiymet(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KiymetBildirim/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreKiymet(kiymet: KiymetDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KiymetBildirimOlustur/", 
        kiymet,httpOptions  
        );
  }
  removeKiymet(kiymetInternalNo,beyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KiymetSil/"+kiymetInternalNo+"/"+beyanInternalNo, httpOptions        
      );
  }
  getKiymetKalem(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KiymetKalem/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreKiymetKalem(kalem: KiymetKalemDto[], kiymetInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/KiymetKalemOlustur/"+kiymetInternalNo+"/"+beyanInternalNo, 
        kalem,httpOptions  
        );
  }
  getDbOzetBeyanAcma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/OzetBeyanAcma/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreDbOzetBeyanAcma(ozetBeyanAcma: DbOzetBeyanAcmaDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/OzetBeyanAcmaOlustur/", 
        ozetBeyanAcma,httpOptions  
        );
  }
  getDbTasimaSenet(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasimaSenet/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreDbTasimaSenet(tasimaSenet:DbOzetBeyanAcmaTasimaSenetDto[],ozetBeyanInternalNo:string , beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/TasimaSenetOlustur/"+ozetBeyanInternalNo+"/"+beyanInternalNo, 
        tasimaSenet,httpOptions  
        );
  }
  getDbTasimaSatir(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasimaSatir/Beyanname/" + IslemInternalNo, httpOptions
    );
  }
  restoreDbTasimaSatir(tasimaSatir: DbOzetBeyanAcmaTasimaSatirDto[],ozetBeyanInternalNo:string ,tasimaSenetInternalNo:string, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/TasimaSatirOlustur/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo+"/"+beyanInternalNo, 
        tasimaSatir,httpOptions  
        );
  }
  removeDbOzetBeyanAcma(ozetBeyanInternalNo,beyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/OzetBeyanAcmaSil/"+ozetBeyanInternalNo+"/"+beyanInternalNo, httpOptions        
      );
  }
  getDbTeminat(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Teminat/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreDbTeminat(teminat: DbTeminatDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/TeminatOlustur/"+beyanInternalNo, 
        teminat,httpOptions  
        );
  }
  getDbFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Firma/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreDbFirma(firma: DbFirmaDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/FirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  getDbBeyanname(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
     return  this.http.get(
      this.baseUrl + "Servis/Beyanname/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  setDbBeyanname(beyanname: BeyannameDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    // const options = new RequestOptions({
    //   headers: this.getAuthorizedHeaders(),
    //   responseType: ResponseContentType.Json,
    //   withCredentials: false
    // });
    // return this.http.post( this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BeyannameOlustur/",
    // JSON.stringify({
    //   cmd: cmd,
    //   data: beyanname}), options)
      
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BeyannameOlustur/", 
        beyanname, httpOptions  
        );
  }
  getBeyannameKopyalama(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
    return this.http.post(
      this.baseUrl + "Servis/Beyanname/BeyannameKopyalama/" + IslemInternalNo,null,httpOptions
    );
  }
  removeBeyanname(IslemInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameSilme/"+IslemInternalNo, httpOptions        
      );
  }
  getOzetBeyan(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
     return  this.http.get(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  setOzetBeyan(beyanname: OzetBeyanDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
   
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/BeyannameOlustur/", 
        beyanname, httpOptions  
        );
  }
  getOzetBeyanKopyalama(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
    return this.http.post(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyanKopyalama/" + IslemInternalNo,null,httpOptions
    );
  }
  removeOzetBeyan(IslemInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyanSilme/"+IslemInternalNo, httpOptions        
      );
  }
  getObTasitUgrakUlke(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasitUgrakUlke/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObTasitUgrakUlke(tasit: TasitUgrakUlkeDto[],  ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasitUgrakUlkeOlustur/"+ozetBeyanInternalNo, 
        tasit,httpOptions  
        );
  }
  getObTasimaSenet(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Servis/TasimaSenet/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  removeObTasimaSenet(tasimaSenetInternalNo,ozetBeyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSenetSil/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo, httpOptions        
      );
  }
  restoreObTasimaSenet(senet: ObTasimaSenetDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSenetOlustur/", 
        senet,httpOptions  
        );
  }
  getObTasimaSatir(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasimaSatir/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObTasimaSatir(satir: ObTasimaSatirDto[], tasimaSenetInternalNo:string , ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSatirOlustur/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo, 
        satir,httpOptions  
        );
  }
  getObIhracat(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Ihracat/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObIhracat(ihracat: ObIhracatDto[], tasimaSenetInternalNo:string , ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/IhracatOlustur/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo, 
        ihracat,httpOptions  
        );
  }
  getObUgrakUlke(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/UgrakUlke/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObUgrakUlke(ulke: ObUgrakUlkeDto[], tasimaSenetInternalNo:string , ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/UgrakUlkeOlustur/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo, 
        ulke,httpOptions  
        );
  }
  getObSatirEsya(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasimaSatirEsya/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObSatirEsya(esya: ObSatirEsyaDto[], tasimaSatirInternalNo:string, tasimaSenetInternalNo:string , ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSatirEsyaOlustur/"+tasimaSatirInternalNo+"/"+tasimaSenetInternalNo+"/"+ozetBeyanInternalNo, 
        esya,httpOptions  
        );
  }
  getObTasiyiciFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasiyiciFirma/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObTasiyiciFirma(firma: ObTasiyiciFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasiyiciFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  resmoveObTasiyiciFirma(ozetBeyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasiyiciFirmaSil/"+ozetBeyanInternalNo, httpOptions        
      );
  } 
  getObOzetBeyanAcma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/OzetBeyanAcma/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObOzetBeyanAcma(ozetBeyanAcma: ObOzetBeyanAcmaDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/OzetBeyanAcmaOlustur/", 
        ozetBeyanAcma,httpOptions  
        );
  }
  getObOzetBeyanAcmaTasimaSenet(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/OzetBeyanAcmaTasimaSenet/OzetBeyan/" + IslemInternalNo, httpOptions
    );
  }
  restoreObOzetBeyanAcmaTasimaSenet(tasimaSenet:ObOzetBeyanAcmaTasimaSenetDto[], ozetBeyanAcmaBeyanInternalNo:string, ozetBeyanInternalNo:string ) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSenetOlustur/"+ozetBeyanInternalNo+"/"+ozetBeyanAcmaBeyanInternalNo, 
        tasimaSenet,httpOptions  
        );
  }
  getObOzetBeyanAcmaTasimaSatir(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/OzetBeyanAcmaTasimaSatir/OzetBeyan/" + IslemInternalNo, httpOptions
    );
  }
  restoreObOzetBeyanAcmaTasimaSatir(tasimaSatir: DbOzetBeyanAcmaTasimaSatirDto[],tasimaSenetInternalNo:string,OzetBeyanAcmaBeyanInternalNo:string, ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TasimaSatirOlustur/"+tasimaSenetInternalNo+"/"+OzetBeyanAcmaBeyanInternalNo+"/"+ozetBeyanInternalNo, 
        tasimaSatir,httpOptions  
        );
  }
  removeObOzetBeyanAcma(ozetBeyanAcmaBeyanInternalNo,ozetBeyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/OzetBeyanAcmaSil/"+ozetBeyanAcmaBeyanInternalNo+"/"+ozetBeyanInternalNo, httpOptions        
      );
  }
  getObTeminat(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Teminat/OzetBeyan/" + IslemInternalNo,httpOptions
    );
  }
  restoreObTeminat(teminat: ObTeminatDto[], ozetBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/OzetBeyan/OzetBeyanOlusturma/TeminatOlustur/"+ozetBeyanInternalNo, 
        teminat,httpOptions  
        );
  }

  getNctsBeyan(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
     return  this.http.get(
      this.baseUrl + "Servis/NctsBeyan/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  setNctsBeyan(beyanname: NctsBeyanDto) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
 
    const httpOptions = {
     headers: headers_object
    };
   
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/BeyannameOlustur/", 
        beyanname, httpOptions  
        );
  }
  getNctsKopyalama(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
     

    const httpOptions = {
     headers: headers_object
    };
    return this.http.post(
      this.baseUrl + "Servis/NctsBeyan/NctsBeyanKopyalama/" + IslemInternalNo,null,httpOptions
    );
  }
  removeNctsBeyan(IslemInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/NctsBeyan/NctsBeyanSilme/"+IslemInternalNo, httpOptions        
      );
  }
  getNbBeyanSahibi(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/BeyanSahibi/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbTasiyiciFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TasiyiFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbAsilSorumluFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/AsilSorumluFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbAliciFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/AliciFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbGondericiFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/GondericiFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbGuvenliAliciFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/GuvenliAliciFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  getNbGuvenliGondericiFirma(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/GuvenliGondericiFirma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbBeyanSahibi(firma: NbBeyanSahibiDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/BeyanSahibiOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbTasiyiciFirma(firma: NbTasiyiciFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/TasiyiciFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbAsilSorumluFirma(firma: NbAsilSorumluFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/AsilSorumluFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbGondericiFirma(firma: NbGondericiFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/GondericiFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbAliciFirma(firma: NbAliciFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/AliciFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbGuvenliGondericiFirma(firma: NbGuvenliGondericiFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/GuvenliGondericiFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbGuvenliAliciFirma(firma: NbGuvenliAliciFirmaDto, beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/GuvenliAliciFirmaOlustur/"+beyanInternalNo, 
        firma,httpOptions  
        );
  }
  getNbTeminat(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Teminat/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbTeminat(teminat: NbTeminatDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/TeminatOlustur/"+beyanInternalNo, 
        teminat,httpOptions  
        );
  }
  getNbMuhur(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Muhur/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbMuhur(muhur: NbMuhurDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/MuhurOlustur/"+beyanInternalNo, 
        muhur,httpOptions  
        );
  }
  getNbRota(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Rota/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbRota(rota: NbRotaDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/RotaOlustur/"+beyanInternalNo, 
        rota,httpOptions  
        );
  }
  getNbTransitGumruk(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/TransitGumruk/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbTransitGumruk(transit: NbTransitGumrukDto[], beyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
   
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/TransitGumrukOlustur/"+beyanInternalNo, 
        transit,httpOptions  
        );
  }
  getNbKalem(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Servis/Kalem/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  removeNbKalem(kalemInternalNo,nctsBeyanInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemSil/"+kalemInternalNo+"/"+nctsBeyanInternalNo, httpOptions        
      );
  }
  restoreNbKalem(kalem: NbKalemDto) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemOlustur/", 
        kalem,httpOptions  
        );
  }
  getNbKonteyner(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Konteyner/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbKonteyner(konteyner: NbKonteynerDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KonteynerOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        konteyner,httpOptions  
        );
  }
  getNbHassasEsya(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/HassasEsya/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbHassasEsya(esya: NbHassasEsyaDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/HassasEsyaOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        esya,httpOptions  
        );
  }
  getNbKap(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Kap/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbKap(kap: NbKapDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KapOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        kap,httpOptions  
        );
  }
  getNbEkBilgi(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/EkBilgi/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbEkBilgi(ekBilgi: NbEkBilgiDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/EkBilgiOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        ekBilgi,httpOptions  
        );
  }
  getNbBelgeler(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Belgeler/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbBelgeler(belgeler: NbBelgelerDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/BelgelerOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        belgeler,httpOptions  
        );
  }
  getNbOncekiBelgeler(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/OncekiBelgeler/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbOncekiBelgeler(belgeler: NbOncekiBelgelerDto[], kalemInternalNo:string , nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/OncekiBelgelerOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        belgeler,httpOptions  
        );
  }
  getNbKalemAliciFirma(IslemInternalNo, KalemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KalemAliciFirma/Ncts/" + IslemInternalNo+"/"+KalemInternalNo,httpOptions
    );
  }
  getNbKalemGondericiFirma(IslemInternalNo, KalemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KalemGondericiFirma/Ncts/" + IslemInternalNo+"/"+KalemInternalNo,httpOptions
    );
  }
  getNbKalemGuvenliAliciFirma(IslemInternalNo, KalemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KalemGuvenliAliciFirma/Ncts/" + IslemInternalNo+"/"+KalemInternalNo,httpOptions
    );
  }
  getNbKalemGuvenliGondericiFirma(IslemInternalNo, KalemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/KalemGuvenliGondericiFirma/Ncts/" + IslemInternalNo+"/"+KalemInternalNo,httpOptions
    );
  }
  restoreNbKalemGondericiFirma(firma: NbKalemGondericiFirmaDto, kalemInternalNo:string, nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemGondericiFirmaOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbKalemAliciFirma(firma: NbKalemAliciFirmaDto,kalemInternalNo:string, nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemAliciFirmaOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbKalemGuvenliGondericiFirma(firma: NbKalemGuvenliGondericiFirmaDto, kalemInternalNo:string, nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemGuvenliGondericiFirmaOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        firma,httpOptions  
        );
  }
  restoreNbKalemGuvenliAliciFirma(firma: NbKalemGuvenliAliciFirmaDto, kalemInternalNo:string, nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/KalemGuvenliAliciFirmaOlustur/"+kalemInternalNo+"/"+nctsBeyanInternalNo, 
        firma,httpOptions  
        );
  }
  getNbObAcama(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/ObAcma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbObAcma(acma: NbObAcmaDto[],  nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/ObAcmaOlustur/"+nctsBeyanInternalNo, 
        acma,httpOptions  
        );
  }
  getNbAbAcama(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/AbAcma/Ncts/" + IslemInternalNo,httpOptions
    );
  }
  restoreNbAbAcma(acma: NbAbAcmaDto[],  nctsBeyanInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/NctsBeyan/NctsOlusturma/abAcmaOlustur/"+nctsBeyanInternalNo, 
        acma,httpOptions  
        );
  }
  getNbKalemler(NctsBeyanInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/NbKalemler/Ncts/" + NctsBeyanInternalNo,httpOptions
    );
  }

  getMesai(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Mesai/Beyanname/" + IslemInternalNo,httpOptions
    );
  }
  restoreMesai(mesai: MesaiDto,  mesaiInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/MesaiOlustur/"+mesaiInternalNo, 
        mesai,httpOptions  
        );
  }
  
  removeMesai(mesaiInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/MesaiSil/"+mesaiInternalNo, httpOptions        
      );
  }

  getIghb(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/Ighb/Beyanname/" + IslemInternalNo,httpOptions
    );
  }


  restoreIghb(ighb: IghbDto,  ighbInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/IghbOlustur/"+ighbInternalNo, 
        ighb,httpOptions  
        );
  }
  removeIghb(ighbInternalNo) {
  
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
         
    const httpOptions = {
     headers: headers_object
    };
      return this.http.delete<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/IghbSil/"+ighbInternalNo, httpOptions        
      );
  }

  restoreIghbListe(ighb: IghbListeDto[],  ighbInternalNo:string) {
   
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
  
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/IghbListeOlustur/"+ighbInternalNo, ighb,
        httpOptions  
        );
  }
  getIghbListe(IslemInternalNo) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "Servis/IghbListe/Beyanname/" + IslemInternalNo,httpOptions
    );
  }

  getOzetBeyanAlanlar(rejim) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(this.baseUrl + "OzetBeyanAlan/BilgiHizmeti/" + rejim, httpOptions);
  }

  getIstatistik(KullaniciKod) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
    
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(this.baseUrl + "Istatistik/BilgiHizmeti/" + KullaniciKod, httpOptions);
  }
}
@Injectable()
export class SessionServiceProxy {
  
  private http: HttpClient;
  private baseUrl: string;
  public guidOf: string;
  public refNo: string;
  public islemInternalNo: string;
  public beyanInternalNo: string;
  public ozetBeyanInternalNo: string;
  public nctsBeyanInternalNo:string;
  public mesaiInternalNo:string;
  public ighbInternalNo:string
  public beyanStatu: string;
  public token: string;
  public ozetBeyanNo:string;
  public beyannameNo:string;
  public Kalemler: KalemDto[];
  protected jsonParseReviver:
    | ((key: string, value: any) => any)
    | undefined = undefined;

  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.http = http;
    this.baseUrl = baseUrl ? baseUrl : "";
  }

  /**
   * @return Success
   */
  // getCurrentLoginInformations(): Observable<GetCurrentLoginInformationsOutput> {
  //     let url_ = this.baseUrl + "/api/services/app/Session/GetCurrentLoginInformations";
  //     url_ = url_.replace(/[?&]$/, "");

  //     let options_ : any = {
  //         observe: "response",
  //         responseType: "blob",
  //         headers: new HttpHeaders({
  //             "Accept": "text/plain"
  //         })
  //     };

  //}
}

export class IslemDto implements IIslemlerDto {
  musteriNo: string | undefined;
  firmaNo: string | undefined;
  refNo: string | undefined;
  guidof: string | undefined;
  gonderimSayisi: number;
  beyanInternalNo: string | undefined;
  beyanNo: string | undefined;
  beyanTipi: string | undefined;
  id: number;
  islemDurumu: string | undefined;
  islemInternalNo: string | undefined;
  islemSonucu: string | undefined;
  islemTipi: string | undefined;
  islemZamani: Date;
  kullanici: string | undefined;
  olusturmaZamani: Date;

  constructor(data?: IIslemlerDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.musteriNo = data["musteriNo"];
      this.firmaNo = data["firmaNo"];
      this.refNo = data["refNo"];
      this.guidof = data["guidof"];
      this.gonderimSayisi = data["gonderimSayisi"];
      this.beyanInternalNo = data["beyanInternalNo"];
      this.beyanNo = data["beyanNo"];
      this.beyanTipi = data["beyanTipi"];
      this.id = data["id"];
      this.islemDurumu = data["islemDurumu"];
      this.islemInternalNo = data["islemInternalNo"];
      this.islemSonucu = data["islemSonucu"];
      this.islemTipi = data["islemTipi"];
      this.islemZamani = data["islemZamani"];
      this.kullanici = data["kullanici"];
      this.olusturmaZamani = data["olusturmaZamani"];
    }
  }

  static fromJS(data: any): IslemDto {
    data = typeof data === "object" ? data : {};
    let result = new IslemDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["musteriNo"] = this.musteriNo;
    data["firmaNo"] = this.firmaNo;
    data["refNo"] = this.refNo;
    data["guidof"] = this.guidof;
    data["gonderimSayisi"] = this.gonderimSayisi;
    data["beyanInternalNo"] = this.beyanInternalNo;
    data["beyanNo"] = this.beyanNo;
    data["beyanTipi"] = this.beyanTipi;
    data["id"] = this.id;
    data["islemDurumu"] = this.islemDurumu;
    data["islemInternalNo"] = this.islemInternalNo;
    data["islemSonucu"] = this.islemSonucu;
    data["islemTipi"] = this.islemTipi;
    data["islemZamani"] = this.islemZamani;
    data["kullanici"] = this.kullanici;
    data["olusturmaZamani"] = this.olusturmaZamani;
    return data;
  }

  clone(): IslemDto {
    const json = this.toJSON();
    let result = new IslemDto();
    result.init(json);
    return result;
  }
}

export interface IIslemlerDto {
  musteriNo: string | undefined;
  firmaNo: string | undefined;
  refNo: string | undefined;
  guidof: string | undefined;
  gonderimSayisi: number;
  beyanInternalNo: string | undefined;
  beyanNo: string | undefined;
  beyanTipi: string | undefined;
  id: number;
  islemDurumu: string | undefined;
  islemInternalNo: string | undefined;
  islemSonucu: string | undefined;
  islemTipi: string | undefined;
  islemZamani: Date;
  kullanici: string | undefined;
  olusturmaZamani: Date;
}

export class TarihceDto implements ITarihceDto {
  id: number;
  RefNo: string;
  IslemInternalNo: string;
  BeyanInternalNo: string;
  Kullanici: string;
  Guid: string;
  GonderimNo: number;
  BeyanNo: string;
  TicaretTipi: string;
  IslemTipi: string;
  IslemDurumu: string;
  IslemSonucu: string;
  Gumruk: string;
  Rejim: string;
  OlusturmaZamani:Date;
  GonderilecekVeri:string;
  GonderilenVeri: string;
  GondermeZamani:Date;
  SonucVeri: string;
  SonucZamani: Date;
  ServistekiVeri: string;
  ImzaliVeri:string;
 

  constructor(data?: ITarihceDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.RefNo = data["RefNo"];
      this.IslemInternalNo = data["IslemInternalNo"];
      this.BeyanInternalNo = data["BeyanInternalNo"];
      this.Kullanici = data["Kullanici"];
      this.Guid = data["Guid"];
      this.GonderimNo = data["GonderimNo"];
      this.BeyanNo = data["BeyanNo"];
      this.TicaretTipi = data["TicaretTipi"];
      this.IslemTipi = data["IslemTipi"];
      this.IslemDurumu = data["IslemDurumu"];
      this.IslemSonucu = data["IslemSonucu"];
      this.Gumruk = data["Gumruk"];
      this.Rejim = data["Rejim"];
      this.OlusturmaZamani = data["OlusturmaZamani"];
      this.GonderilecekVeri = data["GonderilecekVeri"];
      this.GondermeZamani = data["GondermeZamani"];
      this.GonderilenVeri = data["GonderilenVeri"];
      this.SonucVeri = data["SonucVeri"];
      this.SonucZamani = data["SonucZamani"];
      this.ServistekiVeri = data["ServistekiVeri"];
      this.ImzaliVeri= data["ImzaliVeri"];
    }
  }

  static fromJS(data: any): TarihceDto {
    data = typeof data === "object" ? data : {};
    let result = new TarihceDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    data["RefNo"] = this.RefNo;
    data["IslemInternalNo"] = this.IslemInternalNo;
    data["BeyanInternalNo"] = this.BeyanInternalNo;
    data["Kullanici"] = this.Kullanici;
    data["Guid"] = this.Guid;
    data["GonderimNo"] = this.GonderimNo;
    data["BeyanNo"] = this.BeyanNo;
    data["TicaretTipi"] = this.TicaretTipi;
    data["IslemTipi"] = this.IslemTipi;
    data["IslemDurumu"] = this.IslemDurumu;
    data["IslemSonucu"] = this.IslemSonucu;
    data["Gumruk"] = this.Gumruk;
    data["Rejim"] = this.Rejim;
     data["OlusturmaZamani"]=this.OlusturmaZamani ;
     data["GonderilecekVeri"]= this.GonderilecekVeri ;
     data["GondermeZamani"]= this.GondermeZamani;
     data["GonderilenVeri"]= this.GonderilenVeri;
     data["SonucVeri"] = this.SonucVeri;
     data["SonucZamani"]=this.SonucZamani;
     data["ServistekiVeri"]= this.ServistekiVeri;
     data["ImzaliVeri"]= this.ImzaliVeri;
    return data;
  }

  clone(): TarihceDto {
    const json = this.toJSON();
    let result = new TarihceDto();
    result.init(json);
    return result;
  }
}

export interface ITarihceDto {
  id: number;
  RefNo: string;
  IslemInternalNo: string;
  Kullanici: string;
  Guid: string;
  GonderimNo: number;
  BeyanNo: string;
  TicaretTipi: string;
  IslemTipi: string;
  IslemDurumu: string;
  IslemSonucu: string;
  Gumruk: string;
  Rejim: string;
  OlusturmaZamani:Date;
  GonderilecekVeri:string;
  GonderilenVeri: string;
  GondermeZamani:Date;
  SonucVeri: string;
  SonucZamani: Date;
  ServistekiVeri: string;
  ImzaliVeri:string;

}

export class SonucDto implements ISonucDto {
  veri: object[] | undefined;
  mesaj: string | undefined;
  hatalar: [];
  islem: boolean;

  constructor(data?: ISonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.veri = data["Veri"];
      this.mesaj = data["Mesaj"];
      this.islem = data["Islem"];
      this.hatalar = data["Hatalar"];
    }
  }

  static fromJS(data: any): SonucDto {
    data = typeof data === "object" ? data : {};
    let result = new SonucDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["Veri"] = this.veri;
    data["Mesaj"] = this.mesaj;
    data["Islem"] = this.islem;
    data["Hatalar"] = this.hatalar;
    return data;
  }

  clone(): SonucDto {
    const json = this.toJSON();
    let result = new SonucDto();
    result.init(json);
    return result;
  }
}

export interface ISonucDto {
  veri: object[] | undefined;
  mesaj: string | undefined;
  hatalar: [];
  islem: boolean;
}

export class AuthenticateModel implements IAuthenticateModel {
  userNameOrEmailAddress: string | "11111111100";
  password: string | undefined;
  rememberClient: boolean;

  constructor(data?: IAuthenticateModel) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.userNameOrEmailAddress = data["userNameOrEmailAddress"];
      this.password = data["password"];
      this.rememberClient = data["rememberClient"];
    }
  }

  static fromJS(data: any): AuthenticateModel {
    data = typeof data === "object" ? data : {};
    let result = new AuthenticateModel();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["userNameOrEmailAddress"] = this.userNameOrEmailAddress;
    data["password"] = this.password;
    data["rememberClient"] = this.rememberClient;
    return data;
  }

  clone(): AuthenticateModel {
    const json = this.toJSON();
    let result = new AuthenticateModel();
    result.init(json);
    return result;
  }
}

export interface IAuthenticateModel {
  userNameOrEmailAddress: string | undefined;
  password: string | undefined;
  rememberClient: boolean;
}

export class AuthenticateResultModel implements IAuthenticateResultModel {
  accessToken: string | undefined;
  encryptedAccessToken: string | undefined;
  expireInSeconds: number;
  userId: number;

  constructor(data?: IAuthenticateResultModel) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.accessToken = data["accessToken"];
      this.encryptedAccessToken = data["encryptedAccessToken"];
      this.expireInSeconds = data["expireInSeconds"];
      this.userId = data["userId"];
    }
  }

  static fromJS(data: any): AuthenticateResultModel {
    data = typeof data === "object" ? data : {};
    let result = new AuthenticateResultModel();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["accessToken"] = this.accessToken;
    data["encryptedAccessToken"] = this.encryptedAccessToken;
    data["expireInSeconds"] = this.expireInSeconds;
    data["userId"] = this.userId;
    return data;
  }

  clone(): AuthenticateResultModel {
    const json = this.toJSON();
    let result = new AuthenticateResultModel();
    result.init(json);
    return result;
  }
}

export interface IAuthenticateResultModel {
  accessToken: string | undefined;
  encryptedAccessToken: string | undefined;
  expireInSeconds: number;
  userId: number;
}
export class ServisDto {
  ServisDurumKodu: number;
  Hatalar: HatalarDto[];
  Bilgiler: BilgilerDto[];
  SonucVeriler:string;
  
  Sonuc: string;
  constructor(data?: ServisDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
     
      if (Array.isArray(data["bilgiler"])) {
        this.Bilgiler = [] as any;
        for (let item of data["bilgiler"]) this.Bilgiler.push(item);
      }
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }
     
      this.ServisDurumKodu = data["servisDurumKodlari"];
      this.Sonuc=this.getSonuc();

      if (this.Bilgiler!=undefined && Array.isArray( this.Bilgiler))
      this.SonucVeriler= JSON.stringify(this.Bilgiler[0].sonucVeriler);
    
    }
  }

  static fromJS(data: any): ServisDto {
    data = typeof data === "object" ? data : {};
    let result = new ServisDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    if (Array.isArray(this.Bilgiler)) {
      data["bilgiler"] = [];
      for (let item of this.Bilgiler) data["bilgiler"].push(item);
    }
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    } 
    data["servisDurumKodlari"] = this.ServisDurumKodu;
   
    return data;
  }

  clone(): ServisDto {
    const json = this.toJSON();
    let result = new ServisDto();
    result.init(json);
    return result;
  }
  getSonuc():string
  {
    let result={};

   
   if(this.ServisDurumKodu===1)
   {
    if (this.Bilgiler!=undefined && Array.isArray( this.Bilgiler)) {
    
      for (let item of this.Bilgiler) 
      {
      result={
        Guid:item.guid,
        İşlemTipi: item.islemTipi,
        ReferansNo:item.referansNo,
        Sonuç:item.sonuc
      }     
      }
    }

   }
   else
   {
    if (this.Hatalar!=undefined && this.Hatalar) {
    
      for (let item of this.Hatalar) 
      {
        result={
          HataKodu:item.hataKodu,
          HataAçıklaması:item.hataAciklamasi
        }
    
      }
    }
    }
   
    return JSON.stringify(result, null, 4);
  
  }
}

export class KullaniciServisDto {
  ServisDurumKodu: number;
  Hatalar: HatalarDto[];
  Bilgiler: BilgilerDto[];
  KullaniciBilgiler: KullaniciBilgileriDto;
  SonucVeriler:object;
  
  Sonuc: string;
  constructor(data?: KullaniciServisDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
     
      if (Array.isArray(data["bilgiler"])) {
        this.Bilgiler = [] as any;
        for (let item of data["bilgiler"]) this.Bilgiler.push(item);
      }
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }
      this.KullaniciBilgiler= data["kullaniciBilgileri"]; 
    
      this.ServisDurumKodu = data["servisDurumKodlari"];
      this.Sonuc=this.getSonuc();     
      //this.SonucVeriler= this.Bilgiler[0].sonucVeriler;
     
    }
  }

  static fromJS(data: any): KullaniciServisDto {
    data = typeof data === "object" ? data : {};
    let result = new KullaniciServisDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    if (Array.isArray(this.Bilgiler)) {
      data["bilgiler"] = [];
      for (let item of this.Bilgiler) data["bilgiler"].push(item);
    }
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }   
     data["kullaniciBilgileri"] = this.KullaniciBilgiler;
     data["servisDurumKodlari"] = this.ServisDurumKodu;
   
    return data;
  }

  clone(): KullaniciServisDto {
    const json = this.toJSON();
    let result = new KullaniciServisDto();
    result.init(json);
    return result;
  }
  getSonuc():string
  {
    let result={};
  
   if(this.ServisDurumKodu===1)
   {
   
    if (this.KullaniciBilgiler!=null){ 
     
        result={
          KullaniciKod:this.KullaniciBilgiler.kullaniciKod,
          KullaniciAdi:this.KullaniciBilgiler.kullaniciAdi,
          Token: this.KullaniciBilgiler.token,
          Yetkiler:this.KullaniciBilgiler.yetkiler       
        }     
      
      }
  }
   else
   {
    
    if (this.Hatalar) {
     
      for (let item of this.Hatalar) 
      {
        result={
          HataKodu:item.hataKodu,
          HataAçıklaması:item.hataAciklamasi
        }
    
      }
    }
    }
   
    return JSON.stringify(result, null, 4);
  
  }
}

export class IstatistikDto {
  KontrolGonderimSayisi: number;
  TescilGonderimSayisi: number;
  BeyannameSayisi: number;
  TescilBeyannameSayisi: number;
  SonucBeklenenSayisi:number;
  Sonuc: string;
  constructor(data?: IstatistikDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {     
      this.KontrolGonderimSayisi= data["kontrolGonderimSayisi"];
      this.TescilGonderimSayisi= data["tescilGonderimSayisi"];
      this.BeyannameSayisi= data["beyannameSayisi"];
      this.TescilBeyannameSayisi= data["tescilBeyannameSayisi"];
      this.SonucBeklenenSayisi=data["sonucBeklenenSayisi"];
    }
  }

  static fromJS(data: any): IstatistikDto {
    data = typeof data === "object" ? data : {};
    let result = new IstatistikDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    this.TescilGonderimSayisi= data["tescilGonderimSayisi"];
    this.BeyannameSayisi= data["beyannameSayisi"];
    this.TescilBeyannameSayisi= data["tescilBeyannameSayisi"];
     data["kontrolGonderimSayisi"] = this.KontrolGonderimSayisi;
     data["tescilGonderimSayisi"]=this.TescilGonderimSayisi;
     data["beyannameSayisi"]=this.BeyannameSayisi;
     data["tescilBeyannameSayisi"]=this.TescilBeyannameSayisi;
     data["sonucBeklenenSayisi"]=this.SonucBeklenenSayisi;
   
    return data;
  }

  clone(): IstatistikDto {
    const json = this.toJSON();
    let result = new IstatistikDto();
    result.init(json);
    return result;
  }
  
    
}


export class KullaniciDto {
  id: number;
  kullaniciKod:string;
  kullaniciSifre:string;
  ad: string;
  soyad: string;
  vergiNo:string;
  firmaAd:string;
  mailAdres:string;
  telefon:string;
  aktif:boolean;
  sonIslemZamani:Date;
  musteriNo: string;
  firmaNo: string;

  constructor(data?: KullaniciDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {         
      this.id = data["id"];
      this.kullaniciKod = data["kullaniciKod"];
      this.kullaniciSifre = data["kullaniciSifre"];
      this.ad = data["ad"];
      this.soyad = data["soyad"];
      this.vergiNo = data["vergiNo"];
      this.firmaAd = data["firmaAd"];
      this.mailAdres = data["mailAdres"];
      this.aktif = data["aktif"];
      this.telefon = data["telefon"];
      this.sonIslemZamani = data["sonIslemZamani"];
      this.musteriNo = data["musteriNo"];
      this.firmaNo = data["firmaNo"];
    }
  }

  static fromJS(data: any): KullaniciDto {
    data = typeof data === "object" ? data : {};
    let result = new KullaniciDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
  
    data["kullaniciKod"]=this.kullaniciKod ;
    data["kullaniciSifre"]= this.kullaniciSifre ;
    data["ad"]= this.ad ;
    data["soyad"]= this.soyad ;
    data["vergiNo"]= this.vergiNo ;
    data["firmaAd"]=this.firmaAd ;
    data["id"] = this.id;
    data["mailAdres"]= this.mailAdres;
    data["aktif"]=this.aktif ;
    data["telefon"]=this.telefon ;
    data["sonIslemZamani"]=this.sonIslemZamani;
    data["musteriNo"]=this.musteriNo;
    data["firmaNo"]= this.firmaNo ;
    return data;
  }
  clone(): KullaniciDto {
    const json = this.toJSON();
    let result = new KullaniciDto();
    result.init(json);
    return result;
  }
}

export class MusteriDto {
  id: number;
  musteriNo: string;
  adres: string;
  vergiNo:string;
  musteriAd:string;
  mailAdres:string;
  telefon:string;
  aktif:boolean;
  sonIslemZamani:Date;

  constructor(data?: KullaniciDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {         
      this.id = data["id"];
      this.musteriNo = data["musteriNo"]!=null || data["musteriNo"]!=undefined ?data["musteriNo"] :"";
      this.adres = data["adres"];
      this.vergiNo = data["vergiNo"];
      this.musteriAd = data["musteriAd"];
      this.mailAdres = data["mailAdres"];
      this.aktif = data["aktif"];
      this.telefon = data["telefon"];
      this.sonIslemZamani = data["sonIslemZamani"];
    }
  }

  static fromJS(data: any): KullaniciDto {
    data = typeof data === "object" ? data : {};
    let result = new KullaniciDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};  
   
    data["adres"]= this.adres ;
    data["vergiNo"]= this.vergiNo ;
    data["musteriAd"]=this.musteriAd ;
    data["id"] = this.id;
    data["musteriNo"] = this.musteriNo;
    data["mailAdres"]= this.mailAdres;
    data["aktif"]=this.aktif ;
    data["telefon"]=this.telefon ;
    data["sonIslemZamani"]=this.sonIslemZamani;
    return data;
  }
  clone(): KullaniciDto {
    const json = this.toJSON();
    let result = new KullaniciDto();
    result.init(json);
    return result;
  }
}

export class FirmaDto {
  id: number;
  musteriNo: string;
  firmaNo: string;
  adres: string;
  vergiNo:string;
  firmaAd:string;
  mailAdres:string;
  telefon:string;
  aktif:boolean;
  sonIslemZamani:Date;

  constructor(data?: KullaniciDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {         
      this.id = data["id"];
      this.musteriNo = data["musteriNo"]!=null ?data["musteriNo"] :"";
      this.firmaNo = data["firmaNo"]!=null ?data["firmaNo"] :"";
      this.adres = data["adres"];
      this.vergiNo = data["vergiNo"];
      this.firmaAd = data["firmaAd"];
      this.mailAdres = data["mailAdres"];
      this.aktif = data["aktif"];
      this.telefon = data["telefon"];
      this.sonIslemZamani = data["sonIslemZamani"];
    }
  }

  static fromJS(data: any): KullaniciDto {
    data = typeof data === "object" ? data : {};
    let result = new KullaniciDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};  
   
    data["adres"]= this.adres ;
    data["vergiNo"]= this.vergiNo ;
    data["firmaAd"]=this.firmaAd ;
    data["id"] = this.id;
    data["musteriNo"] = this.musteriNo;
    data["firmaNo"] = this.firmaNo;
    data["mailAdres"]= this.mailAdres;
    data["aktif"]=this.aktif ;
    data["telefon"]=this.telefon ;
    data["sonIslemZamani"]=this.sonIslemZamani;
    return data;
  }
  clone(): KullaniciDto {
    const json = this.toJSON();
    let result = new KullaniciDto();
    result.init(json);
    return result;
  }
}

export class YetkiDto {
  id:number;
  yetkiKodu: string;
  yetkiAdi: string;
  aciklama:string;  
  aktif:boolean;
 
  constructor(data?: YetkiDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    
    if (data) {         
      this.id = data["id"];
      this.yetkiKodu = data["yetkiKodu"];
      this.yetkiAdi = data["yetkiAdi"];
      this.aciklama = data["aciklama"];
      this.aktif = data["aktif"];
    
    }
  }

  static fromJS(data: any): YetkiDto {
    data = typeof data === "object" ? data : {};
    let result = new YetkiDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};  
     data["id"]=this.id;
    data["yetkiAdi"]= this.yetkiAdi ;
    data["aciklama"]= this.aciklama ;  
    data["yetkiKodu"] = this.yetkiKodu;  
    data["aktif"]=this.aktif ;
   
    return data;
  }
  clone(): YetkiDto {
    const json = this.toJSON();
    let result = new YetkiDto();
    result.init(json);
    return result;
  }
}

export class KullaniciYetkiDto {
  id: number;
  kullaniciKod: string;
  yetkiKodu:string;  
  aktif:boolean;
 

  constructor(data?: KullaniciYetkiDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {         
      this.id = data["id"];
      this.kullaniciKod = data["kullaniciKod"];
      this.yetkiKodu = data["yetkiKodu"];
      this.aktif = data["aktif"];
    
    }
  }

  static fromJS(data: any): KullaniciYetkiDto {
    data = typeof data === "object" ? data : {};
    let result = new KullaniciYetkiDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};  
   
    data["kullaniciKod"]= this.kullaniciKod ;
    data["yetkiKodu"]= this.yetkiKodu ;  
    data["id"] = this.id;  
    data["aktif"]=this.aktif ;
   
    return data;
  }
  clone(): KullaniciYetkiDto {
    const json = this.toJSON();
    let result = new KullaniciYetkiDto();
    result.init(json);
    return result;
  }
}


export class BeyannameSonucDto {
  Hatalar: SonucHatalarDto[];
  Belgeler: SonucBelgelerDto[];
  Vergiler: SonucVergilerDto[];
  Sorular: SonucSorularDto[];
  SoruCevaplar: SonucSoruCevaplarDto[];
  ToplamVergiler: SonucToplamVergilerDto[];
  ToplananVergiler: SonucToplananVergilerDto[];
  HesapDetaylari: SonucHesapDetaylariDto[];
  IstatistikiKiymet: SonucIstatistikiKiymetDto[];
  GumrukKiymet: SonucGumrukKiymetDto[];
  DovizKuruAlis: string;
  DovizKuruSatis: string;
  CiktiSeriNo: string;
  KalanKontor: string;
  MuayeneMemuru: string;

  constructor(data?: BeyannameSonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      if (Array.isArray(data["belgeler"])) {
        this.Belgeler = [] as any;
        for (let item of data["belgeler"]) this.Belgeler.push(item);
      }
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }
      if (Array.isArray(data["vergiler"])) {
        this.Vergiler = [] as any;
        for (let item of data["vergiler"]) this.Vergiler.push(item);
      }
      if (Array.isArray(data["sorular"])) {
        this.Sorular = [] as any;
        for (let item of data["sorular"]) this.Sorular.push(item);
      }
      if (Array.isArray(data["soruCevap"])) {
        this.SoruCevaplar = [] as any;
        for (let item of data["soruCevap"]) this.SoruCevaplar.push(item);
      }

      if (Array.isArray(data["toplamVergiler"])) {
        this.ToplamVergiler = [] as any;
        for (let item of data["toplamVergiler"]) this.ToplamVergiler.push(item);
      }

      if (Array.isArray(data["toplananVergiler"])) {
        this.ToplananVergiler = [] as any;
        for (let item of data["toplananVergiler"])
          this.ToplananVergiler.push(item);
      }

      if (Array.isArray(data["hesapDetaylari"])) {
        this.HesapDetaylari = [] as any;
        for (let item of data["hesapDetaylari"]) this.HesapDetaylari.push(item);
      }

      if (Array.isArray(data["istatistikiKiymetler"])) {
        this.IstatistikiKiymet = [] as any;
        for (let item of data["istatistikiKiymetler"])
          this.IstatistikiKiymet.push(item);
      }

      if (Array.isArray(data["gumrukKiymetleri"])) {
        this.GumrukKiymet = [] as any;
        for (let item of data["gumrukKiymetleri"]) this.GumrukKiymet.push(item);
      }

      this.DovizKuruAlis = data["dovizKuruAlis"];
      this.DovizKuruSatis = data["dovizKuruSatis"];
      this.CiktiSeriNo = data["ciktiSeriNo"];
      this.KalanKontor = data["kalanKontor"];
      this.MuayeneMemuru = data["muayeneMemuru"];
    }
  }

  static fromJS(data: any): BeyannameSonucDto {
    data = typeof data === "object" ? data : {};
    let result = new BeyannameSonucDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    if (Array.isArray(this.Belgeler)) {
      data["belgeler"] = [];
      for (let item of this.Belgeler) data["belgeler"].push(item);
    }
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
    if (Array.isArray(this.Vergiler)) {
      data["vergiler"] = [];
      for (let item of this.Vergiler) data["vergiler"].push(item);
    }
    if (Array.isArray(this.Sorular)) {
      data["sorular"] = [];
      for (let item of this.Sorular) data["sorular"].push(item);
    }
    if (Array.isArray(this.SoruCevaplar)) {
      data["soruCevap"] = [];
      for (let item of this.SoruCevaplar) data["soruCevap"].push(item);
    }
    if (Array.isArray(this.ToplamVergiler)) {
      data["toplamVergiler"] = [];
      for (let item of this.ToplamVergiler) data["toplamVergiler"].push(item);
    }
    if (Array.isArray(this.ToplananVergiler)) {
      data["toplananVergiler"] = [];
      for (let item of this.ToplananVergiler)
        data["toplananVergiler"].push(item);
    }

    if (Array.isArray(this.HesapDetaylari)) {
      data["hesapDetaylari"] = [];
      for (let item of this.HesapDetaylari) data["hesapDetaylari"].push(item);
    }

    if (Array.isArray(this.GumrukKiymet)) {
      data["gumrukKiymetleri"] = [];
      for (let item of this.GumrukKiymet) data["gumrukKiymetleri"].push(item);
    }

    if (Array.isArray(this.IstatistikiKiymet)) {
      data["istatistikiKiymetler"] = [];
      for (let item of this.IstatistikiKiymet)
        data["istatistikiKiymetler"].push(item);
    }

    data["dovizKuruAlis"] = this.DovizKuruAlis;
    data["dovizKuruSatis"] = this.DovizKuruSatis;
    data["ciktiSeriNo"] = this.CiktiSeriNo;
    data["kalanKontor"] = this.KalanKontor;
    data["muayeneMemuru"] = this.MuayeneMemuru;
    return data;
  }

  clone(): BeyannameSonucDto {
    const json = this.toJSON();
    let result = new BeyannameSonucDto();
    result.init(json);
    return result;
  }
}
export class MesaiSonucDto {
  Hatalar: SonucHatalarDto[];  
  MesaiId: string;
 

  constructor(data?: MesaiSonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }   

      this.MesaiId = data["mesaiId"];
     
    }
  }

  static fromJS(data: any): MesaiSonucDto {
    data = typeof data === "object" ? data : {};
    let result = new MesaiSonucDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

   
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
  
    data["mesaiId"]= this.MesaiId

    
    return data;
  }

  clone(): MesaiSonucDto {
    const json = this.toJSON();
    let result = new MesaiSonucDto();
    result.init(json);
    return result;
  }
}

export class IghbSonucDto {
  Hatalar: SonucHatalarDto[];  
 // MesaiId: string;
 

  constructor(data?: IghbSonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }   

     // this.MesaiId = data["mesaiId"];
     
    }
  }

  static fromJS(data: any): IghbSonucDto {
    data = typeof data === "object" ? data : {};
    let result = new IghbSonucDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

   
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
  
   // data["mesaiId"]= this.MesaiId

    
    return data;
  }

  clone(): IghbSonucDto {
    const json = this.toJSON();
    let result = new IghbSonucDto();
    result.init(json);
    return result;
  }
}


export class OzetBeyanSonucDto {
  Hatalar: SonucHatalarDto[];  
  KalemSayisi: string;
  TescilNo: string;
  TescilTarihi:string;

  constructor(data?: OzetBeyanSonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }   

      this.KalemSayisi = data["kalemSayisi"];
      this.TescilNo = data["tescilNo"];
      this.TescilTarihi = data["tescilTarihi"];
    
    }
  }

  static fromJS(data: any): OzetBeyanSonucDto {
    data = typeof data === "object" ? data : {};
    let result = new OzetBeyanSonucDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

   
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
  
    data["kalemSayisi"]= this.KalemSayisi
     data["tescilNo"]=this.TescilNo;
    data["tescilTarihi"]= this.TescilTarihi;
    
    return data;
  }

  clone(): OzetBeyanSonucDto {
    const json = this.toJSON();
    let result = new OzetBeyanSonucDto();
    result.init(json);
    return result;
  }
}

export class HatalarDto {
  hataKodu: number;
  hataAciklamasi: string;
}

export class SonucHatalarDto {
  kalemNo: number;
  hataKodu: number;
  hataAciklamasi: string;
}
export class SonucBelgelerDto {
  kalemNo: number;
  belgeKodu: string;
  belgeAciklamasi: string;
  dogrulama: string;
  referans: string;
  belgeTarihi: string;
}

export class SonucVergilerDto {
  kalemNo: number;
  vergiKodu: string;
  vergiAciklamasi: string;
  oran: string;
  miktar: string;
  natrah: string;
  odemeSekli: string;
}

export class SonucSorularDto {
  kalemNo: number;
  soruKodu: string;
  soruAciklamasi: string;
}

export class SonucSoruCevaplarDto {
  kalemNo: number;
  soruKodu: string;
  soruCevap: string;
}

export class SonucToplamVergilerDto {
  vergiKodu: string;
  vergiAciklamasi: string;
  OdemeSekli: string;
  miktar: string;
}

export class SonucToplananVergilerDto {
  miktar: string;
  odemeSekli: string;
}

export class SonucHesapDetaylariDto {
  miktar: string;
  aciklama: string;
}

export class SonucIstatistikiKiymetDto {
  kalemNo: number;
  miktar: string;
}

export class SonucGumrukKiymetDto {
  kalemNo: number;
  miktar: string;
}

export class BilgilerDto {
  guid: string;
  islemTipi: string;
  referansNo: string;
  sonuc: string;
  sonucVeriler: object;
}
export class KullaniciBilgileriDto {
  kullaniciKod: string;
  kullaniciAdi: string;
  token: string;
  yetkiler: KullaniciYetkileri[];
 
}
export class KullaniciYetkileri {
  yetkiKodu: string;
  yetkiAdi: string; 
}

export enum BeyanDurum {
  
  _0 = "Olusturuldu",
  _1 = "Guncellendi",
  _2 = "Islemde", 
  _3 = "Islem Sonucu Hatali",
  _4 = "Islem Sonucu Basarili",
  _5 = "Kontrol Gonderildi",
  _6 = "Tescil Gonderildi",
  _7 = "OzetBeyan Gonderildi",
  _8 = "Ncts Gonderildi",
  _9 = "Mesai Gonderildi",
  _10 = "Ighb Gonderildi",
  _11 = "Tescil Mesaji Olusturuldu",
  _12 = "Imzalandi",
  _13 = "Kontrol Basarili",
  _14 = "Kontrol Hatali",
  _15 = "Tescil Edildi",
  _16 = "Tescil Hatali",


}

export class BeyanIslemDurumlari {
  static Progress: string = BeyanDurum._0 || BeyanDurum._1 || BeyanDurum._2 || BeyanDurum._13 || BeyanDurum._14 || BeyanDurum._16 ;
  static Error: string = BeyanDurum._3;
  static Success: string = BeyanDurum._4;
  static Lock: string = BeyanDurum._5 || BeyanDurum._6 || BeyanDurum._7 ||BeyanDurum._8 ||BeyanDurum._9|| BeyanDurum._10|| BeyanDurum._11||BeyanDurum._12;
  
  islem(durumKod:string):string {  
   
       if(durumKod==BeyanDurum._0 || durumKod==BeyanDurum._1 || durumKod==BeyanDurum._2 || durumKod==BeyanDurum._13 || durumKod==BeyanDurum._14 || durumKod==BeyanDurum._16 )
       return BeyanIslemDurumlari.Progress;
       else if(durumKod==BeyanDurum._3)
       return BeyanIslemDurumlari.Error;
       else if(durumKod==BeyanDurum._4)
       return BeyanIslemDurumlari.Success;
       else if(durumKod==BeyanDurum._5 || durumKod==BeyanDurum._6 || durumKod==BeyanDurum._7 || durumKod==BeyanDurum._8 || durumKod==BeyanDurum._9|| durumKod==BeyanDurum._10|| durumKod==BeyanDurum._11|| durumKod==BeyanDurum._12)
       return BeyanIslemDurumlari.Lock;
       else BeyanIslemDurumlari.Lock;
  }
}

export enum ServisDurumKodlari {
  _0 = 0,
  _1 = 1,
  _2 = 2,
  _3 = 3,
  _4 = 4

  // 0= "İşlem sırasında beklenmeyen bir hata oluştu.",
  // 1 = "İşlem Başarılı.",
  // 2 = "Beklenmeyen hata.",
  // 3 = "Beyanname kayıt aşamasında hata oluştu.",
  // 4 = "Kalem kayıt aşamasında hata oluştu."
}
@Injectable()
export class BeyannameBilgileriDto {
  Beyanname: BeyannameDto;
  Kalemler: KalemDto[];
  BeyannameNo: string;

  init(data?: any) {
    if (data) {
    
      this.Beyanname = data["beyanname"];
      if (Array.isArray(data["kalemler"])) {
        this.Kalemler = [] as any;
        for (let item of data["kalemler"]) this.Kalemler.push(item);
      }

      this.BeyannameNo = data["beyanNo"];
    }
  }
}

export class BeyannameDto {
  beyanInternalNo: string;
  beyannameNo: string;
  rejim: string;
  aciklamalar: string;
  aliciSaticiIliskisi: string;
  aliciVergiNo: string;
  antrepoKodu: string;
  asilSorumluVergiNo: string;
  bankaKodu: string;
  basitlestirilmisUsul: string;
  beyanSahibiVergiNo: string;
  birlikKayitNumarasi: string;
  birlikKriptoNumarasi: string;
  cikistakiAracinKimligi: string;
  cikistakiAracinTipi: string;
  cikistakiAracinUlkesi: string;
  cikisUlkesi: string;
  esyaninBulunduguYer: string;
  gidecegiSevkUlkesi: string;
  gidecegiUlke: string;
  girisGumrukIdaresi: string;
  gondericiVergiNo: string;
  gumruk: string;
  isleminNiteligi: string;
  kapAdedi: number;
  konteyner: string;
  kullanici: string;
  limanKodu: string;
  mail1: string;
  mail2: string;
  mail3: string;
  mobil1: string;
  mobil2: string;
  musavirVergiNo: string;
  odemeAraci: string;
  musavirReferansNo: string;
  referansTarihi: string;
  refNo: string;
  sinirdakiAracinKimligi: string;
  sinirdakiAracinTipi: string;
  sinirdakiAracinUlkesi: string;
  sinirdakiTasimaSekli: string;
  tasarlananGuzergah: string;
  telafiEdiciVergi: number;
  tescilStatu: string;
  tescilTarihi: string;
  teslimSekli: string;
  teslimSekliYeri: string;
  ticaretUlkesi: string;
  toplamFatura: number;
  toplamFaturaDovizi: string;
  toplamNavlun: number;
  toplamNavlunDovizi: string;
  toplamSigorta: number;
  toplamSigortaDovizi: string;
  toplamYurtDisiHarcamalar: number;
  toplamYurtDisiHarcamalarDovizi: string;
  toplamYurtIciHarcamalar: number;
  varisGumrukIdaresi: string;
  yukBelgeleriSayisi: number;
  yuklemeBosaltmaYeri: string;
  olsuturulmaTarihi:string;
  sonIslemZamani:string;

  constructor(data?: BeyannameDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  initalBeyan(data?: any) {
    if (data) {
      this.beyanInternalNo=data["beyanInternalNo"]!=null ? data["beyanInternalNo"] : "";
      this.beyannameNo=data["beyannameNo"]!=null ? data["beyannameNo"]:"";
      this.rejim=data["rejim"]!=null ? data["rejim"] : "";
      this.aciklamalar=data["aciklamalar"]!=null ? data["aciklamalar"] : "";
      this.aliciSaticiIliskisi=data["aliciSaticiIliskisi"]!=null ? data["aliciSaticiIliskisi"] : "";
      this.aliciVergiNo=data["aliciVergiNo"]!=null ? data["aliciVergiNo"] : "";
      this.antrepoKodu=data["antrepoKodu"]!=null ? data["antrepoKodu"] : "";
      this.asilSorumluVergiNo=data["asilSorumluVergiNo"]!=null ? data["asilSorumluVergiNo"] : "";
      this.bankaKodu=data["bankaKodu"]!=null ? data["bankaKodu"] : "";
      this.basitlestirilmisUsul=data["basitlestirilmisUsul"]!=null ? data["basitlestirilmisUsul"] : "";
      this.beyanSahibiVergiNo=data["beyanSahibiVergiNo"]!=null ? data["beyanSahibiVergiNo"] : "";
      this.birlikKayitNumarasi=data["birlikKayitNumarasi"]!=null ? data["birlikKayitNumarasi"] : "";
      this.birlikKriptoNumarasi=data["birlikKriptoNumarasi"]!=null ? data["birlikKriptoNumarasi"] : "";
      this.cikistakiAracinKimligi=data["cikistakiAracinKimligi"]!=null ? data["cikistakiAracinKimligi"] : "";
      this.cikistakiAracinTipi=data["cikistakiAracinTipi"]!=null ? data["cikistakiAracinTipi"] : "";
      this.cikistakiAracinUlkesi=data["cikistakiAracinUlkesi"]!=null ? data["cikistakiAracinUlkesi"] : "";
      this.cikisUlkesi=data["cikisUlkesi"]!=null ? data["cikisUlkesi"] : "";
      this.esyaninBulunduguYer=data["esyaninBulunduguYer"]!=null ? data["esyaninBulunduguYer"] : "";
      this.gidecegiSevkUlkesi=data["gidecegiSevkUlkesi"]!=null ? data["gidecegiSevkUlkesi"] : "";
      this.gidecegiUlke=data["gidecegiUlke"]!=null ? data["gidecegiUlke"] : "";
      this.girisGumrukIdaresi=data["girisGumrukIdaresi"]!=null ? data["girisGumrukIdaresi"] : "";
      this.gondericiVergiNo=data["gondericiVergiNo"]!=null ? data["gondericiVergiNo"] : "";
      this.gumruk=data["gumruk"]!=null ? data["gumruk"] : "";
      this.isleminNiteligi=data["isleminNiteligi"]!=null ? data["isleminNiteligi"] : "";
      this.kapAdedi=data["kapAdedi"]!=null ?parseInt(data["kapAdedi"]):0;
      this.konteyner=data["konteyner"]!=null ? data["konteyner"] : "";
      this.kullanici=data["kullanici"]!=null ? data["kullanici"] : "";
      this.limanKodu=data["limanKodu"]!=null ? data["limanKodu"] : "";
      this.mail1=data["mail1"]!=null ? data["mail1"] : "";
      this.mail2=data["mail2"]!=null ? data["mail2"] : "";
      this.mail3=data["maimail3l1"]!=null ? data["mail3"] : "";
      this.mobil1=data["mobil1"]!=null ? data["mobil1"] : "";
      this.mobil2=data["mobil2"]!=null ? data["mobil2"] : "";
      this.musavirVergiNo=data["musavirVergiNo"]!=null ? data["musavirVergiNo"] : "";
      this.odemeAraci=data["odemeAraci"]!=null ? data["odemeAraci"] : "";
      this.musavirReferansNo=data["musavirReferansNo"]!=null ? data["musavirReferansNo"] : "";
      this.referansTarihi=data["referansTarihi"]!=null ? data["referansTarihi"] : "";
      this.refNo=data["refNo"]!=null ? data["refNo"] : "";
      this.sinirdakiAracinKimligi=data["sinirdakiAracinKimligi"]!=null ? data["sinirdakiAracinKimligi"] : "";
      this.sinirdakiAracinTipi=data["sinirdakiAracinTipi"]!=null ? data["sinirdakiAracinTipi"] : "";
      this.sinirdakiAracinUlkesi=data["sinirdakiAracinUlkesi"]!=null ? data["sinirdakiAracinUlkesi"] : "";
      this.sinirdakiTasimaSekli=data["sinirdakiTasimaSekli"]!=null ? data["sinirdakiTasimaSekli"] : "";
      this.tasarlananGuzergah=data["tasarlananGuzergah"]!=null ? data["tasarlananGuzergah"] : "";
      this.telafiEdiciVergi=data["telafiEdiciVergi"]!=null ?parseFloat(data["telafiEdiciVergi"]):0;
      this.tescilStatu=data["tescilStatu"]!=null ? data["tescilStatu"] : "";
      this.tescilTarihi=data["tescilTarihi"]!=null && data["tescilTarihi"]!='' ? data["tescilTarihi"] : "0001-01-01T00:00:00";
      this.teslimSekli=data["teslimSekli"]!=null ? data["teslimSekli"] : "";
      this.teslimSekliYeri=data["teslimSekliYeri"]!=null ? data["teslimSekliYeri"] : "";
      this.ticaretUlkesi=data["ticaretUlkesi"]!=null ? data["ticaretUlkesi"] : "";
      this.toplamFatura=data["toplamFatura"]!=null ?parseFloat(data["toplamFatura"]):0;
      this.toplamFaturaDovizi=data["toplamFaturaDovizi"]!=null ? data["toplamFaturaDovizi"] : "";
      this.toplamNavlun=data["toplamNavlun"]!=null ?parseFloat(data["toplamNavlun"]):0;
      this.toplamNavlunDovizi=data["toplamNavlunDovizi"]!=null ? data["toplamNavlunDovizi"] : "";
      this.toplamSigorta=data["toplamSigorta"]!=null ?parseFloat(data["toplamSigorta"]):0;
      this.toplamSigortaDovizi=data["toplamSigortaDovizi"]!=null ? data["toplamSigortaDovizi"] : "";
      this.toplamYurtDisiHarcamalar=data["toplamYurtDisiHarcamalar"]!=null ?parseFloat(data["toplamYurtDisiHarcamalar"]):0;
      this.toplamYurtDisiHarcamalarDovizi=data["toplamYurtDisiHarcamalarDovizi"]!=null ? data["toplamYurtDisiHarcamalarDovizi"] : "";
      this.toplamYurtIciHarcamalar=data["toplamYurtIciHarcamalar"]!=null ?parseFloat(data["toplamYurtIciHarcamalar"]):0;
      this.varisGumrukIdaresi=data["varisGumrukIdaresi"]!=null ? data["varisGumrukIdaresi"] : "";
      this.yukBelgeleriSayisi=data["yukBelgeleriSayisi"]!=null ?parseFloat(data["yukBelgeleriSayisi"]):0;
      this.yuklemeBosaltmaYeri=data["yuklemeBosaltmaYeri"]!=null ? data["yuklemeBosaltmaYeri"] : "";
      this.olsuturulmaTarihi= "0001-01-01T00:00:00";
      this.sonIslemZamani=  "0001-01-01T00:00:00";
    }
  }

  init(data?: any) {
    if (data) {
      this.beyanInternalNo=data["beyanInternalNo"];
      this.beyannameNo=data["beyannameNo"];
      this.rejim=data["rejim"];
      this.aciklamalar=data["aciklamalar"];
      this.aliciSaticiIliskisi=data["aliciSaticiIliskisi"];
      this.aliciVergiNo=data["aliciVergiNo"];
      this.antrepoKodu=data["antrepoKodu"];
      this.asilSorumluVergiNo=data["asilSorumluVergiNo"];
      this.bankaKodu=data["bankaKodu"];
      this.basitlestirilmisUsul=data["basitlestirilmisUsul"];;
      this.beyanSahibiVergiNo=data["beyanSahibiVergiNo"];
      this.birlikKayitNumarasi=data["birlikKayitNumarasi"];
      this.birlikKriptoNumarasi=data["birlikKriptoNumarasi"];
      this.cikistakiAracinKimligi=data["cikistakiAracinKimligi"];
      this.cikistakiAracinTipi=data["cikistakiAracinTipi"];
      this.cikistakiAracinUlkesi=data["cikistakiAracinUlkesi"];
      this.cikisUlkesi=data["cikisUlkesi"];
      this.esyaninBulunduguYer=data["esyaninBulunduguYer"];
      this.gidecegiSevkUlkesi=data["gidecegiSevkUlkesi"];
      this.gidecegiUlke=data["gidecegiUlke"];
      this.girisGumrukIdaresi=data["girisGumrukIdaresi"];
      this.gondericiVergiNo=data["gondericiVergiNo"];
      this.gumruk=data["gumruk"];
      this.isleminNiteligi=data["isleminNiteligi"];
      this.kapAdedi=data["kapAdedi"]!=null ?parseInt(data["kapAdedi"]):0;
      this.konteyner=data["konteyner"];
      this.kullanici=data["kullanici"];
      this.limanKodu=data["limanKodu"];
      this.mail1=data["mail1"];
      this.mail2=data["mail2"];
      this.mail3=data["mail3"];
      this.mobil1=data["mobil1"];
      this.mobil2=data["mobil2"];
      this.musavirVergiNo=data["musavirVergiNo"];
      this.odemeAraci=data["odemeAraci"];
      this.musavirReferansNo=data["musavirReferansNo"];
      this.referansTarihi=data["referansTarihi"];
      this.refNo=data["refNo"];
      this.sinirdakiAracinKimligi=data["sinirdakiAracinKimligi"];
      this.sinirdakiAracinTipi=data["sinirdakiAracinTipi"];
      this.sinirdakiAracinUlkesi=data["sinirdakiAracinUlkesi"];
      this.sinirdakiTasimaSekli=data["sinirdakiTasimaSekli"];
      this.tasarlananGuzergah=data["tasarlananGuzergah"];
      this.telafiEdiciVergi=data["telafiEdiciVergi"]!=null ?parseFloat(data["telafiEdiciVergi"]):0;
      this.tescilStatu=data["tescilStatu"];
      this.tescilTarihi=data["tescilTarihi"];
      this.teslimSekli=data["teslimSekli"];
      this.teslimSekliYeri=data["teslimSekliYeri"];
      this.ticaretUlkesi=data["ticaretUlkesi"];
      this.toplamFatura=data["toplamFatura"]!=null ?parseFloat(data["toplamFatura"]):0;
      this.toplamFaturaDovizi= data["toplamFaturaDovizi"];
      this.toplamNavlun=data["toplamNavlun"]!=null ?parseFloat(data["toplamNavlun"]):0;
      this.toplamNavlunDovizi=data["toplamNavlunDovizi"];
      this.toplamSigorta=data["toplamSigorta"]!=null ?parseFloat(data["toplamSigorta"]):0;
      this.toplamSigortaDovizi=data["toplamSigortaDovizi"];
      this.toplamYurtDisiHarcamalar=data["toplamYurtDisiHarcamalar"]!=null ?parseFloat(data["toplamYurtDisiHarcamalar"]):0;
      this.toplamYurtDisiHarcamalarDovizi=data["toplamYurtDisiHarcamalarDovizi"];
      this.toplamYurtIciHarcamalar=data["toplamYurtIciHarcamalar"]!=null ?parseFloat(data["toplamYurtIciHarcamalar"]):0;
      this.varisGumrukIdaresi=data["varisGumrukIdaresi"];
      this.yukBelgeleriSayisi=data["yukBelgeleriSayisi"]!=null ?parseFloat(data["yukBelgeleriSayisi"]):0;
      this.yuklemeBosaltmaYeri=data["yuklemeBosaltmaYeri"];
      
    }
  }

  static fromJS(data: any): BeyannameDto {
    data = typeof data === "object" ? data : {};
    let result = new BeyannameDto();

    result.init(data);
    return result;
  }


}
export class KalemDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  kalemSiraNo: number;
  gtip: string;
  aciklama44: string;
  adet: number;
  algilamaBirimi1: string;
  algilamaBirimi2: string;
  algilamaBirimi3: string;
  algilamaMiktari1: number;
  algilamaMiktari2: number;
  algilamaMiktari3: number;
  brutAgirlik: number;
  cins: string;
  ekKod: string;
  faturaMiktari: number;
  faturaMiktariDovizi: string;
  girisCikisAmaci: string;
  girisCikisAmaciAciklama: string;
  ikincilIslem: string;
  imalatciFirmaBilgisi: string;
  imalatciVergiNo: string;
  istatistikiKiymet: number;
  istatistikiMiktar: number;
  kalemIslemNiteligi: string;
  kullanilmisEsya: string;
  mahraceIade: string;
  marka: string;
  menseiUlke: string;
  miktar: number;
  miktarBirimi: string;
  muafiyetAciklamasi: string;
  muafiyetler1: string;
  muafiyetler2: string;
  muafiyetler3: string;
  muafiyetler4: string;
  muafiyetler5: string;
  navlunMiktari: number;
  navlunMiktariDovizi: string;
  netAgirlik: number;
  numara: string;
  ozellik: string;
  referansTarihi: string;
  satirNo: string;
  sigortaMiktari: number;
  sigortaMiktariDovizi: string;
  sinirGecisUcreti: number;
  stmIlKodu: string;
  tamamlayiciOlcuBirimi: string;
  tarifeTanimi: string;
  teslimSekli: string;
  ticariTanimi: string;
  uluslararasiAnlasma: string;
  yurtDisiDemuraj: number;
  yurtDisiDemurajDovizi: string;
  yurtDisiDiger: number;
  yurtDisiDigerAciklama: string;
  yurtDisiDigerDovizi: string;
  yurtDisiFaiz: number;
  yurtDisiFaizDovizi: string;
  yurtDisiKomisyon: number;
  yurtDisiKomisyonDovizi: string;
  yurtDisiRoyalti: number;
  yurtDisiRoyaltiDovizi: string;
  yurtIciBanka: number;
  yurtIciCevre: number;
  yurtIciDepolama: number;
  yurtIciDiger: number;
  yurtIciDigerAciklama: string;
  yurtIciKkdf: number;
  yurtIciKultur: number;
  yurtIciLiman: number;
  yurtIciTahliye: number;

  constructor(data?: KalemDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  initKalem(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) { 
     
         this.beyanInternalNo=data["beyanInternalNo"]!=null ? data["beyanInternalNo"] : "";
         this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"]:"";
         this.kalemSiraNo=data["kalemSiraNo"]!=null ? parseInt(data["kalemSiraNo"]):0;
         this.gtip=data["gtip"]!=null ?data["gtip"]:"";        
         this.aciklama44=data["aciklama44"]!=null ? data["aciklama44"]:"";
         this.adet=data["adet"]!=null ? parseInt(data["adet"]):0;
         this.algilamaBirimi1=data["algilamaBirimi1"]!=null ?data["algilamaBirimi1"]:"";
         this.algilamaBirimi2=data["algilamaBirimi2"]!=null ?data["algilamaBirimi2"]:"";
         this.algilamaBirimi3=data["algilamaBirimi3"]!=null ?data["algilamaBirimi3"]:"";
         this.algilamaMiktari1=data["algilamaMiktari1"]!=null ?parseFloat(data["algilamaMiktari1"]):0;
         this.algilamaMiktari2=data["algilamaMiktari2"]!=null ?parseFloat(data["algilamaMiktari2"]):0;
         this.algilamaMiktari3=data["algilamaMiktari3"]!=null ?parseFloat(data["algilamaMiktari3"]):0;
         this.brutAgirlik=data["brutAgirlik"]!=null ? parseFloat(data["brutAgirlik"]):0;
         this.cins=data["cins"]!=null ?data["cins"]:"";
         this.ekKod=data["ekKod"]!=null ?data["ekKod"]:"";
         this.faturaMiktari=data["faturaMiktari"]!=null ? parseFloat(data["faturaMiktari"]):0;
         this.faturaMiktariDovizi=data["faturaMiktariDovizi"]!=null ?data["faturaMiktariDovizi"]:"";
         this.girisCikisAmaci=data["girisCikisAmaci"]!=null ?data["girisCikisAmaci"]:"";
         this.girisCikisAmaciAciklama=data["girisCikisAmaciAciklama"]!=null ?data["girisCikisAmaciAciklama"]:"";
         this.ikincilIslem=data["ikincilIslem"]!=null ? data["ikincilIslem"]:"";
         this.imalatciFirmaBilgisi=data["imalatciFirmaBilgisi"]!=null ?data["imalatciFirmaBilgisi"]:"";
         this.imalatciVergiNo=data["imalatciVergiNo"]!=null ?data["imalatciVergiNo"]:"";
         this.istatistikiKiymet=data["istatistikiKiymet"]!=null ? parseFloat(data["istatistikiKiymet"]):0;
         this.istatistikiMiktar=data["istatistikiMiktar"]!=null ? parseFloat(data["istatistikiMiktar"]):0;
         this.kalemIslemNiteligi=data["kalemIslemNiteligi"]!=null ?data["kalemIslemNiteligi"]:"";
         this.kullanilmisEsya=data["kullanilmisEsya"]!=null ?data["kullanilmisEsya"]:"";
         this.mahraceIade=data["mahraceIade"]!=null ?data["mahraceIade"]:"";
         this.marka=data["marka"]!=null ?data["marka"]:"";
         this.menseiUlke=data["menseiUlke"]!=null ?data["menseiUlke"]:"";
         this.miktar=data["miktar"]!=null ? parseFloat(data["miktar"]):0;
         this.miktarBirimi=data["miktarBirimi"]!=null ?data["miktarBirimi"]:"";
         this.muafiyetAciklamasi=data["muafiyetAciklamasi"]!=null ?data["muafiyetAciklamasi"]:"";
         this.muafiyetler1=data["muafiyetler1"]!=null ?data["muafiyetler1"]:"";
         this.muafiyetler2=data["muafiyetler2"]!=null ?data["muafiyetler2"]:"";
         this.muafiyetler3=data["muafiyetler3"]!=null ?data["muafiyetler3"]:"";
         this.muafiyetler4=data["muafiyetler4"]!=null ?data["muafiyetler4"]:"";
         this.muafiyetler5=data["muafiyetler5"]!=null ?data["muafiyetler5"]:"";
         this.navlunMiktari=data["navlunMiktari"]!=null ? parseFloat(data["navlunMiktari"]):0;
         this.navlunMiktariDovizi=data["navlunMiktariDovizi"]!=null ?data["navlunMiktariDovizi"]:"";
         this.netAgirlik=data["netAgirlik"]!=null ? parseFloat(data["netAgirlik"]):0;
         this.numara=data["numara"]!=null ?data["numara"]:"";
         this.ozellik=data["ozellik"]!=null ?data["ozellik"]:"";
         this.satirNo=data["satirNo"]!=null ?data["satirNo"]:"";
         this.sigortaMiktari=data["sigortaMiktari"]!=null ? parseFloat(data["sigortaMiktari"]):0;
         this.sigortaMiktariDovizi=data["sigortaMiktariDovizi"]!=null ?data["sigortaMiktariDovizi"]:"";
         this.sinirGecisUcreti=data["sinirGecisUcreti"]!=null ? parseFloat(data["sinirGecisUcreti"]):0;
         this.stmIlKodu=data["stmIlKodu"]!=null ?data["stmIlKodu"]:"";
         this.tamamlayiciOlcuBirimi=data["tamamlayiciOlcuBirimi"]!=null ?data["tamamlayiciOlcuBirimi"]:"";
         this.teslimSekli=data["teslimSekli"]!=null ?data["teslimSekli"]:"";
         this.ticariTanimi=data["ticariTanimi"]!=null ?data["ticariTanimi"]:"";
         this.uluslararasiAnlasma=data["uluslararasiAnlasma"]!=null ?data["uluslararasiAnlasma"]:"";
         this.yurtDisiDemuraj=data["yurtDisiDemuraj"]!=null ? parseFloat(data["yurtDisiDemuraj"]):0;
         this.yurtDisiDemurajDovizi=data["yurtDisiDemurajDovizi"]!=null ?data["yurtDisiDemurajDovizi"]:"";
         this.yurtDisiDiger=data["yurtDisiDiger"]!=null ? parseFloat(data["yurtDisiDiger"]):0;
         this.yurtDisiDigerAciklama=data["yurtDisiDigerAciklama"]!=null ?data["yurtDisiDigerAciklama"]:"";
         this.yurtDisiDigerDovizi=data["yurtDisiDigerDovizi"]!=null ?data["yurtDisiDigerDovizi"]:"";
         this.yurtDisiFaiz=data["yurtDisiFaiz"]!=null ? parseFloat(data["yurtDisiFaiz"]):0;
         this.yurtDisiFaizDovizi=data["yurtDisiFaizDovizi"]!=null ?data["yurtDisiFaizDovizi"]:"";
         this.yurtDisiKomisyon=data["yurtDisiKomisyon"]!=null ? parseFloat(data["yurtDisiKomisyon"]):0;
         this.yurtDisiKomisyonDovizi=data["yurtDisiKomisyonDovizi"]!=null ?data["yurtDisiKomisyonDovizi"]:"";
         this.yurtDisiRoyalti=data["yurtDisiRoyalti"]!=null ? parseFloat(data["yurtDisiRoyalti"]):0;
         this.yurtDisiRoyaltiDovizi=data["yurtDisiRoyaltiDovizi"]!=null ?data["yurtDisiRoyaltiDovizi"]:"";
         this.yurtIciBanka=data["yurtIciBanka"]!=null ? parseFloat(data["yurtIciBanka"]):0;
         this.yurtIciCevre=data["yurtIciCevre"]!=null ? parseFloat(data["yurtIciCevre"]):0;
         this.yurtIciDepolama=data["yurtIciDepolama"]!=null ? parseFloat(data["yurtIciDepolama"]):0;
         this.yurtIciDiger=data["yurtIciDiger"]!=null ? parseFloat(data["yurtIciDiger"]):0;
         this.yurtIciDigerAciklama=data["yurtIciDigerAciklama"]!=null ?data["yurtIciDigerAciklama"]:"";
         this.yurtIciKkdf=data["yurtIciKkdf"]!=null ? parseFloat(data["yurtIciKkdf"]):0;
         this.yurtIciKultur=data["yurtIciKultur"]!=null ? parseFloat(data["yurtIciKultur"]):0;
         this.yurtIciLiman=data["yurtIciLiman"]!=null ? parseFloat(data["yurtIciLiman"]):0;
         this.yurtIciTahliye=data["yurtIciTahliye"]!=null ? parseFloat(data["yurtIciTahliye"]):0;

         
    }
  }

  

  static fromJS(data: any): KalemDto {
    data = typeof data === "object" ? data : {};
    let result = new KalemDto();

    result.init(data);
    return result;
  }
}
export class OdemeDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  odemeTutari: number;
  odemeSekliKodu: string;
  tbfid:string;

  constructor(data?: OdemeDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): OdemeDto {
    data = typeof data === "object" ? data : {};
    let result = new OdemeDto();

    result.init(data);
    return result;
  }
}
export class KonteynerDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  konteynerNo: string;
  ulkeKodu:string;

  constructor(data?: KonteynerDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): KonteynerDto {
    data = typeof data === "object" ? data : {};
    let result = new KonteynerDto();

    result.init(data);
    return result;
  }
}
export class TamamlayiciBilgiDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  gtip: string;
  bilgi:string;
  oran:string;
  constructor(data?: TamamlayiciBilgiDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): TamamlayiciBilgiDto {
    data = typeof data === "object" ? data : {};
    let result = new TamamlayiciBilgiDto();

    result.init(data);
    return result;
  }
}
export class MarkaDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  markaAdi: string;
  markaKiymeti:number;
  markaTescilNo:string;
  markaTuru:string;
  model:string;
  motorGucu:number;
  motorHacmi:string;
  motorNo:string;
  motorTipi:string;
  modelYili:string
  renk:string
  referansNo:string
  silindirAdet:number;
  vites:string;

  constructor(data?: MarkaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): MarkaDto {
    data = typeof data === "object" ? data : {};
    let result = new MarkaDto();

    result.init(data);
    return result;
  }
}
export class BeyannameAcmaDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  beyannameNo: string;
  kalemNo:number;
  miktar:number;
  aciklama:string;
 
  constructor(data?: BeyannameAcmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): BeyannameAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new BeyannameAcmaDto();

    result.init(data);
    return result;
  }
}
export class VergiDto {
  
  beyanInternalNo: string;
  kalemInternalNo: string;
  kalemNo:number;
  vergiKodu: number;
  vergiAciklamasi:string;
  miktar:number;
  oran:string;
  matrah:number;
  odemeSekli:string;

 
  constructor(data?: VergiDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): VergiDto {
    data = typeof data === "object" ? data : {};
    let result = new VergiDto();

    result.init(data);
    return result;
  }
}
export class BelgeDto {
 
  beyanInternalNo: string;
  kalemInternalNo: string;
  kalemNo:number;
  belgeKodu: string;
  belgeAciklamasi:string;
  dogrulama:string;
  referans:string;
  belgeTarihi:string;
 
  constructor(data?: BelgeDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): BelgeDto {
    data = typeof data === "object" ? data : {};
    let result = new BelgeDto();

    result.init(data);
    return result;
  }
}
export class SoruCevapDto {
  islemInternalNo: string;
  beyanInternalNo: string;
  kalemInternalNo: string;
  kalemNo:number;
  soruKodu: string;
  soruCevap:string;
  soruAciklamasi:string;
  tip:string;
  
  constructor(data?: SoruCevapDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): SoruCevapDto {
    data = typeof data === "object" ? data : {};
    let result = new SoruCevapDto();

    result.init(data);
    return result;
  }
}
export class KiymetDto {
  id:number;
  beyanInternalNo: string;
  kiymetInternalNo: string;
  aliciSatici: string;
  aliciSaticiAyrintilar:string;
  edim:string;
  emsal:string;
  faturaTarihiSayisi:string;
  gumrukIdaresiKarari:string;
  kisitlamalar:string;
  kisitlamalarAyrintilar:string;
  munasebet:string;
  royalti:string;
  royaltiKosullar:string;
  saticiyaIntikal:string;
  saticiyaIntikalKosullar:string;
  sehirYer:string;
  sozlesmeTarihiSayisi:string;
  taahhutname:string;
  teslimSekli:string;

  constructor(data?: KiymetDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): KiymetDto {
    data = typeof data === "object" ? data : {};
    let result = new KiymetDto();

    result.init(data);
    return result;
  }
}
export class KiymetKalemDto {
  id:number;
  beyanInternalNo: string;
  kiymetInternalNo: string;
  beyannameKalemNo: number;
  kiymetKalemNo:number;
  digerOdemeler:number;
  digerOdemelerNiteligi:string;
  dolayliIntikal:number;
  dolayliOdeme:number;
  girisSonrasiNakliye:number;
  ithalaKatilanMalzeme:number;
  ithalaUretimAraclar:number;
  ithalaUretimTuketimMalzemesi:number;
  kapAmbalajBedeli:number;
  komisyon:number;
  nakliye:number;
  planTaslak:number;
  royaltiLisans:number;
  sigorta:number;
  teknikYardim:number;
  tellaliye:number;
  vergiHarcFon:number;

  constructor(data?: KiymetKalemDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): KiymetKalemDto {
    data = typeof data === "object" ? data : {};
    let result = new KiymetKalemDto();

    result.init(data);
    return result;
  }
}
export class DbTeminatDto {
  beyanInternalNo: string;
  teminatSekli: string;
  teminatOrani:number;
  globalTeminatNo:string;
  bankaMektubuTutari:number;
  nakdiTeminatTutari:number;
  digerTutar:number;
  digerTutarReferansi:string;
  aciklama:string;

  constructor(data?: DbTeminatDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): DbTeminatDto {
    data = typeof data === "object" ? data : {};
    let result = new DbTeminatDto();

    result.init(data);
    return result;
  }
}
export class DbFirmaDto {
  beyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  faks:string;
  ilIlce:string;
  kimlikTuru:string;
  no:string;
  postaKodu:string;
  telefon:string;
  tip:string;
  ulkeKodu:string;

  constructor(data?: DbFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): DbFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new DbFirmaDto();

    result.init(data);
    return result;
  }
}
export class DbOzetBeyanAcmaDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  ozetBeyanNo: string;
  islemKapsami:string;
  ambar:string;
  baskaRejim:string;
  aciklama:string;
 

  constructor(data?: DbOzetBeyanAcmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): DbOzetBeyanAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new DbOzetBeyanAcmaDto();

    result.init(data);
    return result;
  }
}
export class DbOzetBeyanAcmaTasimaSenetDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  tasimaSenediNo: string;

  
 

  constructor(data?: DbOzetBeyanAcmaTasimaSenetDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): DbOzetBeyanAcmaTasimaSenetDto {
    data = typeof data === "object" ? data : {};
    let result = new DbOzetBeyanAcmaTasimaSenetDto();

    result.init(data);
    return result;
  }
}
export class DbOzetBeyanAcmaTasimaSatirDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  ambarKodu:string;
  miktar:number;
  tasimaSatirNo:number;
  
 

  constructor(data?: DbOzetBeyanAcmaTasimaSatirDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): DbOzetBeyanAcmaTasimaSatirDto {
    data = typeof data === "object" ? data : {};
    let result = new DbOzetBeyanAcmaTasimaSatirDto();

    result.init(data);
    return result;
  }
}
export class OzetBeyanDto { 
  
	ozetBeyanInternalNo:string;
	ozetBeyanNo:string;
	beyanSahibiVergiNo:string;
	beyanTuru:string;
	diger:string;
	dorseNo1:string;
	dorseNo1Uyrugu:string;
	dorseNo2:string;
	dorseNo2Uyrugu:string;
	ekBelgeSayisi:number;
	emniyetGuvenlik:string;
	grupTasimaSenediNo:string;
	gumrukIdaresi:string;
	kullaniciKodu:string;
	kurye:string;
	limanYerAdiBos:string;
	limanYerAdiYuk:string;
	oncekiBeyanNo:string;
	plakaSeferNo:string;
  referansNumarasi:string;
  refNo:string;
	rejim:string;
	tasimaSekli:string;
	tasitinAdi:string;
	tasiyiciVergiNo:string;
	tirAtaKarneNo:string;
	ulkeKodu:string;
	ulkeKoduYuk:string;
	ulkeKoduBos:string;
	yuklemeBosaltmaYeri:string;
	varisCikisGumrukIdaresi:string;
	varisTarihSaati:string;
	xmlRefId:string;
  tescilStatu:string;
  tescilTarihi:string;
	olsuturulmaTarihi:string;
  sonIslemZamani:string;
  
  constructor(data?: OzetBeyanDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  initalBeyan(data?: any) {
    if (data) {
     
      this.ozetBeyanInternalNo=data["ozetBeyanInternalNo"]!=null ? data["ozetBeyanInternalNo"] : "";
      this.ozetBeyanNo=data["ozetBeyanNo"]!=null ? data["ozetBeyanNo"] : "";
      this.beyanSahibiVergiNo=data["beyanSahibiVergiNo"]!=null ? data["beyanSahibiVergiNo"] : "";
      this.beyanTuru=data["beyanTuru"]!=null ? data["beyanTuru"] : "";
      this.diger=data["diger"]!=null ? data["diger"] : "";
      this.dorseNo1=data["dorseNo1"]!=null ? data["dorseNo1"] : "";;
      this.dorseNo1Uyrugu=data["dorseNo1Uyrugu"]!=null ? data["dorseNo1Uyrugu"] : "";
      this.dorseNo2=data["dorseNo2"]!=null ? data["dorseNo2"] : "";
      this.dorseNo2Uyrugu=data["dorseNo2Uyrugu"]!=null ? data["dorseNo2Uyrugu"] : "";
      this.ekBelgeSayisi=data["ekBelgeSayisi"]!=null ? data["ekBelgeSayisi"] : 0;
      this.emniyetGuvenlik=data["emniyetGuvenlik"]!=null ? data["emniyetGuvenlik"] : "";
      this.grupTasimaSenediNo=data["grupTasimaSenediNo"]!=null ? data["grupTasimaSenediNo"] : "";
      this.gumrukIdaresi=data["gumrukIdaresi"]!=null ? data["gumrukIdaresi"] : "";
      this.kullaniciKodu=data["kullaniciKodu"]!=null ? data["kullaniciKodu"] : "";
      this.kurye=data["kurye"]!=null ? data["kurye"] : "";
      this.limanYerAdiBos=data["limanYerAdiBos"]!=null ? data["limanYerAdiBos"] : "";
      this.limanYerAdiYuk=data["limanYerAdiYuk"]!=null ? data["limanYerAdiYuk"] : "";
      this.oncekiBeyanNo=data["oncekiBeyanNo"]!=null ? data["oncekiBeyanNo"] : "";
      this.plakaSeferNo=data["plakaSeferNo"]!=null ? data["plakaSeferNo"] : "";
      this.referansNumarasi=data["referansNumarasi"]!=null ? data["referansNumarasi"] : "";
      this.refNo=data["refNo"]!=null ? data["refNo"] : "";
      this.rejim=data["rejim"]!=null ? data["rejim"] : "";
      this.tasimaSekli=data["tasimaSekli"]!=null ? data["tasimaSekli"] : "";
      this.tasitinAdi=data["tasitinAdi"]!=null ? data["tasitinAdi"] : "";
      this.tasiyiciVergiNo=data["tasiyiciVergiNo"]!=null ? data["tasiyiciVergiNo"] : "";
      this.tirAtaKarneNo=data["tirAtaKarneNo"]!=null ? data["tirAtaKarneNo"] : "";
      this.ulkeKodu=data["ulkeKodu"]!=null ? data["ulkeKodu"] : "";
      this.ulkeKoduYuk=data["ulkeKoduYuk"]!=null ? data["ulkeKoduYuk"] : "";
      this.ulkeKoduBos=data["ulkeKoduBos"]!=null ? data["ulkeKoduBos"] : "";
      this.yuklemeBosaltmaYeri=data["yuklemeBosaltmaYeri"]!=null ? data["yuklemeBosaltmaYeri"] : "";
      this.varisCikisGumrukIdaresi=data["varisCikisGumrukIdaresi"]!=null ? data["varisCikisGumrukIdaresi"] : "";
      this.varisTarihSaati=data["varisTarihSaati"]!=null ?  data["varisTarihSaati"] : "";
      this.xmlRefId=data["xmlRefId"]!=null ? data["xmlRefId"] : "";
      this.tescilStatu=data["tescilStatu"]!=null ? data["tescilStatu"] : "";
      this.tescilTarihi=data[""]!=null ? data[""] : "0001-01-01T00:00:00";
      this.olsuturulmaTarihi= "0001-01-01T00:00:00";
      this.sonIslemZamani=  "0001-01-01T00:00:00";
    }
  }

   static fromJS(data: any): OzetBeyanDto {
    data = typeof data === "object" ? data : {};
    let result = new OzetBeyanDto();

    result.initalBeyan(data);
    return result;
  }

}
export class TasitUgrakUlkeDto {
  ozetBeyanInternalNo: string;
  hareketTarihSaati: string;
  limanYerAdi: string;
  ulkeKodu: string;  

  constructor(data?: TasitUgrakUlkeDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): TasitUgrakUlkeDto {
    data = typeof data === "object" ? data : {};
    let result = new TasitUgrakUlkeDto();

    result.init(data);
    return result;
  }
}
export class ObTasimaSenetDto {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo:string;
  tasimaSenediNo:string;
  acentaAdi: string;
  acentaVergiNo: string; 
  aktarmaYapilacak: string;  
  aktarmaTipi: string;   
  aliciAdi: string;  
  aliciVergiNo: string;  
  ambarHarici: string;  
  bildirimTarafiAdi: string;  
  bildirimTarafiVergiNo: string;  
  duzenlendigiUlke: string;  
  emniyetGuvenlik: string;  
  esyaninBulunduguYer: string;  
  faturaDoviz: string;  
  faturaToplami: number;  
  gondericiAdi: string;  
  gondericiVergiNo: string;  
  grup: string;  
  konteyner: string;  
  navlunDoviz: string;  
  navlunTutari: number;  
  odemeSekli: string;  
  oncekiSeferNumarasi: string;  
  oncekiSeferTarihi: string;  
  ozetBeyanNo: string;  
  roro: string;  
  senetSiraNo: number;  

  constructor(data?: ObTasimaSenetDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) { 
     
         this.ozetBeyanInternalNo=data["ozetBeyanInternalNo"]!=null ? data["ozetBeyanInternalNo"] : "";
         this.tasimaSenetInternalNo=data["tasimaSenetInternalNo"]!=null ?data["tasimaSenetInternalNo"]:"";
         this.senetSiraNo=data["senetSiraNo"]!=null ? parseInt(data["senetSiraNo"]):0;
         this.tasimaSenediNo=data["tasimaSenediNo"]!=null ?data["tasimaSenediNo"]:"";        
         this.faturaToplami=data["faturaToplami"]!=null ? parseInt(data["faturaToplami"]):0;     
         this.navlunTutari=data["navlunTutari"]!=null ?parseFloat(data["navlunTutari"]):0;       
         this.acentaAdi=data["acentaAdi"]!=null ?data["acentaAdi"]:"";   
         this.acentaVergiNo=data["acentaVergiNo"]!=null ?data["acentaVergiNo"]:"";   
         this.aktarmaYapilacak=data["aktarmaYapilacak"]!=null ?data["aktarmaYapilacak"]:"";   
         this.aktarmaTipi=data["aktarmaTipi"]!=null ?data["aktarmaTipi"]:"";    
         this.aliciAdi=data["aliciAdi"]!=null ?data["aliciAdi"]:"";    
         this.aliciVergiNo=data["aliciVergiNo"]!=null ?data["aliciVergiNo"]:"";     
         this.ambarHarici=data["ambarHarici"]!=null ?data["ambarHarici"]:"";     
         this.bildirimTarafiAdi=data["bildirimTarafiAdi"]!=null ?data["bildirimTarafiAdi"]:"";    
         this.bildirimTarafiVergiNo=data["bildirimTarafiVergiNo"]!=null ?data["bildirimTarafiVergiNo"]:"";   
         this.duzenlendigiUlke=data["duzenlendigiUlke"]!=null ?data["duzenlendigiUlke"]:"";   
         this.emniyetGuvenlik=data["emniyetGuvenlik"]!=null ?data["emniyetGuvenlik"]:"";     
         this.esyaninBulunduguYer=data["esyaninBulunduguYer"]!=null ?data["esyaninBulunduguYer"]:"";    
         this.faturaDoviz=data["faturaDoviz"]!=null ?data["faturaDoviz"]:"";     
         this.gondericiAdi=data["gondericiAdi"]!=null ?data["gondericiAdi"]:"";     
         this.gondericiVergiNo=data["gondericiVergiNo"]!=null ?data["gondericiVergiNo"]:"";   
         this.grup=data["grup"]!=null ?data["grup"]:"";   
         this.konteyner=data["konteyner"]!=null ?data["konteyner"]:"";   
         this.navlunDoviz=data["navlunDoviz"]!=null ?data["navlunDoviz"]:"";             
         this.odemeSekli=data["odemeSekli"]!=null ?data["odemeSekli"]:"";     
         this.oncekiSeferNumarasi=data["oncekiSeferNumarasi"]!=null ?data["oncekiSeferNumarasi"]:"";   
         this.oncekiSeferTarihi=data["oncekiSeferTarihi"]!=null ?data["oncekiSeferTarihi"]:"";   
         this.ozetBeyanNo=data["ozetBeyanNo"]!=null ?data["ozetBeyanNo"]:"";   
         this.roro=data["roro"]!=null ?data["roro"]:"";   
       
         
    }
  }


  static fromJS(data: any): ObTasimaSenetDto {
    data = typeof data === "object" ? data : {};
    let result = new ObTasimaSenetDto();

    result.init(data);
    return result;
  }
}
export class ObUgrakUlkeDto {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo:string;
  limanYerAdi: string;
  ulkeKodu: string;  

  constructor(data?: ObUgrakUlkeDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObUgrakUlkeDto {
    data = typeof data === "object" ? data : {};
    let result = new ObUgrakUlkeDto();

    result.init(data);
    return result;
  }
}
export class ObIhracatDto {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo:string;
  brutAgirlik: number;
  kapAdet: number;
  numara:string;
  parcali:string;
  tip:string;


  constructor(data?: ObIhracatDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObIhracatDto {
    data = typeof data === "object" ? data : {};
    let result = new ObIhracatDto();

    result.init(data);
    return result;
  }
}
export class ObTasimaSatirDto {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  tasimaSatirInternalNo: string;
  brutAgirlik: number;  
  kapAdet:number;
  kapCinsi:string;
  konteynerTipi:string;
  markaNo:string;  
  muhurNumarasi:string;  
  netAgirlik:number;  
  olcuBirimi:string;  
  satirNo:number;  
  konteynerYukDurumu:string;  

  constructor(data?: ObTasimaSatirDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObTasimaSatirDto {
    data = typeof data === "object" ? data : {};
    let result = new ObTasimaSatirDto();

    result.init(data);
    return result;
  }
}
export class ObSatirEsyaDto {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  tasimaSatirInternalNo: string;
  bmEsyaKodu: string;
  brutAgirlik: number;  
  esyaKodu:string;
  esyaninTanimi:string;
  kalemFiyati:number;
  kalemFiyatiDoviz:string;
  kalemSiraNo:number;
  netAgirlik:number;
  olcuBirimi:string;

  constructor(data?: ObSatirEsyaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObSatirEsyaDto {
    data = typeof data === "object" ? data : {};
    let result = new ObSatirEsyaDto();

    result.init(data);
    return result;
  }
}
export class ObTasiyiciFirmaDto {
  ozetBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  faks:string;
  ilIlce:string;
  kimlikTuru:string;
  no:string;
  postaKodu:string;
  telefon:string;
  tip:string;
  ulkeKodu:string;

  constructor(data?: ObTasiyiciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.ozetBeyanInternalNo=data["ozetBeyanInternalNo"]!=null ?data["ozetBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.faks=data["faks"]!=null ?data["faks"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.kimlikTuru=data["kimlikTuru"]!=null ?data["kimlikTuru"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.telefon=data["telefon"]!=null ?data["telefon"]:"";
        this.tip=data["tip"]!=null ?data["tip"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): ObTasiyiciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new ObTasiyiciFirmaDto();

    result.init(data);
    return result;
  }
}
export class ObOzetBeyanAcmaDto {
  id:number;
  ozetBeyanInternalNo: string;
  ozetBeyanAcmaBeyanInternalNo: string;
  ozetBeyanNo: string;
  islemKapsami:string;
  ambar:string;
  baskaRejim:string;
  dahiliNoAcma:string;
  aciklama:string;
 

  constructor(data?: ObOzetBeyanAcmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObOzetBeyanAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new ObOzetBeyanAcmaDto();

    result.init(data);
    return result;
  }
}
export class ObOzetBeyanAcmaTasimaSenetDto {
  id:number;
  ozetBeyanInternalNo: string;
  ozetBeyanAcmaBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  tasimaSenediNo: string;
  dahiliNoAcilanSenet:string;
  
 

  constructor(data?: ObOzetBeyanAcmaTasimaSenetDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObOzetBeyanAcmaTasimaSenetDto {
    data = typeof data === "object" ? data : {};
    let result = new ObOzetBeyanAcmaTasimaSenetDto();

    result.init(data);
    return result;
  }
}
export class ObOzetBeyanAcmaTasimaSatirDto {
  id:number;
  ozetBeyanInternalNo: string;
  ozetBeyanAcmaBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  acmaSatirNo:number;
  ambarKodu:string;
  ambardakiMiktar:number;
  acilacakMiktar:number;
  markaNo:string;
  esyaCinsi:string;
  birim:string;
  toplamMiktar:number;
  kapatilanMiktar:number;
  olcuBirimi:string;

  
 

  constructor(data?: ObOzetBeyanAcmaTasimaSatirDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObOzetBeyanAcmaTasimaSatirDto {
    data = typeof data === "object" ? data : {};
    let result = new ObOzetBeyanAcmaTasimaSatirDto();

    result.init(data);
    return result;
  }
}
export class ObTeminatDto {
  ozetBeyanInternalNo: string;
  teminatSekli: string;
  teminatOrani:number;
  globalTeminatNo:string;
  bankaMektubuTutari:number;
  nakdiTeminatTutari:number;
  digerTutar:number;
  digerTutarReferansi:string;
  aciklama:string;

  constructor(data?: ObTeminatDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?:[]) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObTeminatDto {
    data = typeof data === "object" ? data : {};
    let result = new ObTeminatDto();

    result.init(data);
    return result;
  }
}
export class NctsBeyanDto { 
  
  refNo:string;
	nctsBeyanInternalNo:string;
  beyannameNo:string;
  hareketGumruk:string;
  varisGumruk:string;
  tescilStatu:string;
  tescilTarihi:string;
	olsuturulmaTarihi:string;
  sonIslemZamani:string;  
  varisUlke:string;  
  cikisUlke:string;
  esyaKabulYerKod:string;
  esyaKabulYerDil:string;
	esyaKabulYer:string;
	esyaOnayYer:string;
  yuklemeYeri:string  
  esyaYer:string;
  yer:string;
  yerTarihDil:string;
  bosaltmaYer:string;
  yukBosYerDil:string;
  dahildeTasimaSekli:string;  
  sinirTasimaSekli:string;
  cikisTasimaSekli:string;  
	cikisTasitKimligi:string;
	cikisTasitKimligiDil:string;
	cikisTasitUlke:string;
  sinirTasitKimligi:string;  
	sinirTasitKimligiDil:string;
  sinirTasitUlke:string;  
  konteyner:boolean;  
	kalemSayisi:number;
	toplamKapSayisi:number;
  kalemToplamBrutKG:number;  	
	rejim:string;
  beyanTipi:string;  
	beyanTipiDil:string;
	odemeAraci:string;
	refaransNo:string;
	guvenliBeyan:number; 
  konveyansRefNo:string; 
  dorse1:string;
  dorse2:string;
  damgaVergi:number;
  musavirKimlikNo:string;
  kullanici:string;
  temsilci:string;
	temsilKapasite:string;
	temsilKapasiteDil:string;
	varisGumrukYetkilisi:string;
	kontrolSonuc:string;
  sureSinir:string;
  tanker:boolean;
  sinirGumruk:string;

  constructor(data?: NctsBeyanDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  initalBeyan(data?: any) {
    if (data) {
      this.refNo=data["refNo"]!=null ? data["refNo"] : "";
      this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ? data["nctsBeyanInternalNo"] : "";
      this.beyannameNo=data["beyannameNo"]!=null ? data["beyannameNo"] : "";
      this.tescilStatu=data["tescilStatu"]!=null ? data["tescilStatu"] : "";
      this.tescilTarihi=data[""]!=null ? data["tescilTarihi"] : "0001-01-01T00:00:00";
      this.olsuturulmaTarihi= "0001-01-01T00:00:00";
      this.sonIslemZamani=  "0001-01-01T00:00:00";
      this.varisUlke=data["varisUlke"]!=null ? data["varisUlke"] : "";
      this.cikisUlke=data["cikisUlke"]!=null ? data["cikisUlke"] : "";
      this.esyaKabulYerKod=data["esyaKabulYerKod"]!=null ? data["esyaKabulYerKod"] : "";
      this.esyaKabulYerDil=data["esyaKabulYerDil"]!=null ? data["esyaKabulYerDil"] : "";
      this.esyaKabulYer=data["esyaKabulYer"]!=null ? data["esyaKabulYer"] : "";
      this.esyaOnayYer=data["esyaOnayYer"]!=null ? data["esyaOnayYer"] : "";
      this.yuklemeYeri=data["yuklemeYeri"]!=null ? data["yuklemeYeri"] : "";    
      this.esyaYer=data["esyaYer"]!=null ? data["esyaYer"] : "";
      this.varisGumruk=data["varisGumruk"]!=null ? data["varisGumruk"] : "";
      this.hareketGumruk=data["hareketGumruk"]!=null ? data["hareketGumruk"] : "";
      this.sinirGumruk=data["sinirGumruk"]!=null ? data["sinirGumruk"] : "";
      this.dahildeTasimaSekli=data["dahildeTasimaSekli"]!=null ? data["dahildeTasimaSekli"] : "";
      this.cikisTasimaSekli=data["cikisTasimaSekli"]!=null ? data["cikisTasimaSekli"] : "";  
      this.cikisTasitKimligi=data["emniyetGuvenlik"]!=null ? data["cikisTasitKimligi"] : "";
      this.cikisTasitKimligiDil=data["cikisTasitKimligiDil"]!=null ? data["cikisTasitKimligiDil"] : "";
      this.cikisTasitUlke=data["cikisTasitUlke"]!=null ? data["cikisTasitUlke"] : "";
      this.sinirTasitKimligi=data["sinirTasitKimligi"]!=null ? data["sinirTasitKimligi"] : "";
      this.sinirTasitKimligiDil=data["sinirTasitKimligiDil"]!=null ? data["sinirTasitKimligiDil"] : "";
      this.sinirTasitUlke=data["sinirTasitUlke"]!=null ? data["sinirTasitUlke"] : "";
      this.sinirTasimaSekli=data["sinirTasimaSekli"]!=null ? data["sinirTasimaSekli"] : "";
      this.konteyner=data["konteyner"]!=null ? data["konteyner"] : false;
      this.kalemSayisi=data["kalemSayisi"]!=null ? parseInt(data["kalemSayisi"]) : 0;
      this.toplamKapSayisi=data["toplamKapSayisi"]!=null ? parseInt(data["toplamKapSayisi"]) : 0;
      this.kalemToplamBrutKG=data["kalemToplamBrutKG"]!=null ? parseFloat(data["kalemToplamBrutKG"]) : 0;    
      this.rejim=data["rejim"]!=null ? data["rejim"] : "";
      this.yer=data["yer"]!=null ? data["yer"] : "";
      this.yerTarihDil=data["yerTarihDil"]!=null ? data["yerTarihDil"] : "";
      this.beyanTipi=data["beyanTipi"]!=null ? data["beyanTipi"] : "";
      this.beyanTipiDil=data["beyanTipiDil"]!=null ? data["beyanTipiDil"] : "";
      this.odemeAraci=data["odemeAraci"]!=null ? data["odemeAraci"] : "";
      this.refaransNo=data["refaransNo"]!=null ? data["refaransNo"] : "";
      this.guvenliBeyan=data["guvenliBeyan"]!=null ? parseInt(data["guvenliBeyan"]) : 0;
      this.konveyansRefNo=data["konveyansRefNo"]!=null ? data["konveyansRefNo"] : "";
      this.bosaltmaYer=data["bosaltmaYer"]!=null ? data["bosaltmaYer"] : "";
      this.yukBosYerDil=data["yukBosYerDil"]!=null ?  data["yukBosYerDil"] : "";
      this.dorse1=data["dorse1"]!=null ? data["dorse1"] : "";
      this.dorse2=data["dorse2"]!=null ? data["dorse2"] : "";
      this.damgaVergi=data["damgaVergi"]!=null ?parseFloat(data["damgaVergi"]):0;
      this.musavirKimlikNo=data["musavirKimlikNo"]!=null ? data["musavirKimlikNo"] : "";
      this.kullanici=data["kullanici"]!=null ? data["kullanici"] : "";      
      this.temsilci=data["temsilci"]!=null ? data["temsilci"] : "";
      this.temsilKapasite=data["temsilKapasite"]!=null ? data["temsilKapasite"] : "";
      this.temsilKapasiteDil=data["temsilKapasiteDil"]!=null ? data["temsilKapasiteDil"] : "";
      this.varisGumrukYetkilisi=data["varisGumrukYetkilisi"]!=null ? data["varisGumrukYetkilisi"] : "";
      this.kontrolSonuc=data["kontrolSonuc"]!=null ? data["kontrolSonuc"] : "";
      this.tanker=data["tanker"]!=null ? data["tanker"] : false;
      this.sureSinir=data["sureSinir"]!=null ? data["sureSinir"] :  "";
      
    }
  }

   static fromJS(data: any): NctsBeyanDto {
    data = typeof data === "object" ? data : {};
    let result = new NctsBeyanDto();

    result.initalBeyan(data);
    return result;
  }

}
export class NbBeyanSahibiDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  no:string;
 
  constructor(data?: NbBeyanSahibiDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";     
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
      
    }
  }


  static fromJS(data: any): NbBeyanSahibiDto {
    data = typeof data === "object" ? data : {};
    let result = new NbBeyanSahibiDto();

    result.init(data);
    return result;
  }
}
export class NbTasiyiciFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbTasiyiciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbTasiyiciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbTasiyiciFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbGondericiFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbGondericiFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbGondericiFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbGondericiFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbAliciFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbAliciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbAliciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbAliciFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbAsilSorumluFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbAsilSorumluFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbAsilSorumluFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbAsilSorumluFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbGuvenliGondericiFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbGuvenliGondericiFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbGuvenliGondericiFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbGuvenliGondericiFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbGuvenliAliciFirmaDto {
  nctsBeyanInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbGuvenliAliciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbGuvenliAliciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbGuvenliAliciFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbTeminatDto {
  nctsBeyanInternalNo: string;
  teminatTipi: string;
  grnNo:string;
  digerRefNo:string;
  erisimKodu:string;
  dovizCinsi:string;
  tutar:number;
  ecGecerliDegil:number;
  ulkeGecerliDegil:string;
 
  constructor(data?: NbTeminatDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.teminatTipi=data["teminatTipi"]!=null ?data["teminatTipi"] :"";
        this.grnNo=data["grnNo"]!=null ?data["grnNo"]:"";     
        this.digerRefNo=data["digerRefNo"]!=null ?data["digerRefNo"]:"";
        this.erisimKodu=data["erisimKodu"]!=null ?data["erisimKodu"]:"";
        this.dovizCinsi=data["dovizCinsi"]!=null ?data["dovizCinsi"]:"";
        this.ulkeGecerliDegil=data["ulkeGecerliDegil"]!=null ?data["ulkeGecerliDegil"]:"";
        this.tutar=data["tutar"]!=null ? parseFloat( data["tutar"]):0;
        this.ecGecerliDegil=data["ecGecerliDegil"]!=null ? parseInt(data["ecGecerliDegil"]):0;
        
    }
  }


  static fromJS(data: any): NbTeminatDto {
    data = typeof data === "object" ? data : {};
    let result = new NbTeminatDto();

    result.init(data);
    return result;
  }
}
export class NbTransitGumrukDto {
  nctsBeyanInternalNo: string;
  gumruk: string;
  varisTarihi:string;
  
 
  constructor(data?: NbTransitGumrukDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.gumruk=data["gumruk"]!=null ?data["gumruk"] :"";
        this.varisTarihi=data["varisTarihi"]!=null ?data["varisTarihi"]:"";     
      
      
    }
  }


  static fromJS(data: any): NbTransitGumrukDto {
    data = typeof data === "object" ? data : {};
    let result = new NbTransitGumrukDto();

    result.init(data);
    return result;
  }
}
export class NbMuhurDto {
  nctsBeyanInternalNo: string;
  muhurNo: string;
  dil:string;
 
 
  constructor(data?: NbMuhurDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.muhurNo=data["muhurNo"]!=null ?data["muhurNo"] :"";
        this.dil=data["dil"]!=null ?data["dil"]:"";     
      
      
    }
  }


  static fromJS(data: any): NbMuhurDto {
    data = typeof data === "object" ? data : {};
    let result = new NbMuhurDto();

    result.init(data);
    return result;
  }
}
export class NbRotaDto {
  nctsBeyanInternalNo: string;
  ulkeKodu: string;
 
 
  constructor(data?: NbRotaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"] :"";
       
    }
  }


  static fromJS(data: any): NbRotaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbRotaDto();

    result.init(data);
    return result;
  }
}
export class NbKalemDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  kalemSiraNo: number;  
	gtip: string;
	rejimKodu: string;
	ticariTanim: string;
	ticariTanimDil: string;
	kiymet: number;
	kiymetDoviz: string;
	burutAgirlik: number;
	netAgirlik: number;
	varisUlkesi: string;
	cikisUlkesi: string;
	tptChMOdemeKod: string;
	konsimentoRef: string;
	undg: string;
	ihrBeyanNo: string;
	ihrBeyanTip: string;
	ihrBeyanParcali: string;
 
  constructor(data?: NbKalemDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.kalemSiraNo=data["kalemSiraNo"]!=null ?data["kalemSiraNo"] :0;
        this.gtip=data["gtip"]!=null ?data["gtip"] :"";
        this.rejimKodu=data["rejimKodu"]!=null ?data["rejimKodu"] :"";
        this.ticariTanim=data["ticariTanim"]!=null ?data["ticariTanim"] :"";
        this.ticariTanimDil=data["ticariTanimDil"]!=null ?data["ticariTanimDil"] :"";
        this.kiymet=data["kiymet"]!=null ?parseFloat(data["kiymet"]) :0;
        this.kiymetDoviz=data["kiymetDoviz"]!=null ?data["kiymetDoviz"] :"";
        this.burutAgirlik=data["burutAgirlik"]!=null ?parseFloat(data["burutAgirlik"]) :0;
        this.netAgirlik=data["netAgirlik"]!=null ?parseFloat(data["netAgirlik"]) :0;
        this.varisUlkesi=data["varisUlkesi"]!=null ?data["varisUlkesi"] :"";
        this.cikisUlkesi=data["cikisUlkesi"]!=null ?data["cikisUlkesi"] :"";
        this.tptChMOdemeKod=data["tptChMOdemeKod"]!=null ?data["tptChMOdemeKod"] :"";
        this.konsimentoRef=data["konsimentoRef"]!=null ?data["konsimentoRef"] :"";
        this.undg=data["undg"]!=null ?data["undg"] :"";
        this.ihrBeyanNo=data["ihrBeyanNo"]!=null ?data["ihrBeyanNo"] :"";
        this.ihrBeyanTip=data["ihrBeyanTip"]!=null ?data["ihrBeyanTip"] :"";
        this.ihrBeyanParcali=data["ihrBeyanParcali"]!=null ?data["ihrBeyanParcali"] :"";

       
    }
  }


  static fromJS(data: any): NbKalemDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKalemDto();

    result.init(data);
    return result;
  }
}
export class NbKonteynerDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  konteynerNo: string;
  ulke: string;
 
  constructor(data?: NbKonteynerDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.konteynerNo=data["konteynerNo"]!=null ?data["konteynerNo"] :"";
        this.ulke=data["ulke"]!=null ?data["ulke"] :"";
    }
  }


  static fromJS(data: any): NbKonteynerDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKonteynerDto();

    result.init(data);
    return result;
  }
}
export class NbKalemGondericiFirmaDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbKalemGondericiFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbKalemGondericiFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKalemGondericiFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbKalemAliciFirmaDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbKalemAliciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbKalemAliciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKalemAliciFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbKalemGuvenliGondericiFirmaDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbKalemGuvenliGondericiFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbKalemGuvenliGondericiFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKalemGuvenliGondericiFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbKalemGuvenliAliciFirmaDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  adUnvan: string;
  caddeSokakNo:string;
  ilIlce:string;
  dil:string;
  no:string;
  postaKodu:string;
  ulkeKodu:string;

  constructor(data?: NbKalemGuvenliAliciFirmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.adUnvan=data["adUnvan"]!=null ?data["adUnvan"] :"";
        this.caddeSokakNo=data["caddeSokakNo"]!=null ?data["caddeSokakNo"]:"";
        this.dil=data["dil"]!=null ?data["dil"]:"";
        this.ilIlce=data["ilIlce"]!=null ?data["ilIlce"]:"";
        this.no=data["no"]!=null ?data["no"]:"";
        this.postaKodu=data["postaKodu"]!=null ?data["postaKodu"]:"";
        this.ulkeKodu=data["ulkeKodu"]!=null ?data["ulkeKodu"]:"";

    }
  }


  static fromJS(data: any): NbKalemGuvenliAliciFirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKalemGuvenliAliciFirmaDto();

    result.init(data);
    return result;
  }
}
export class NbHassasEsyaDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  kod: number;
  miktar: number;
 
  constructor(data?: NbHassasEsyaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.kod=data["kod"]!=null ?  parseInt(data["kod"]) :0;
        this.miktar=data["miktar"]!=null ? parseFloat( data["miktar"]) :0;
    }
  }


  static fromJS(data: any): NbHassasEsyaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbHassasEsyaDto();

    result.init(data);
    return result;
  }
}
export class NbKapDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  markaNo:string;
  kapTipi:string;
  markaDil:string;
  kapAdet: number;
  parcaSayisi: number;
 
  constructor(data?: NbKapDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.markaNo=data["no"]!=null ?data["markaNo"] :"";
        this.markaDil=data["markaDil"]!=null ?data["markaDil"] :"";
        this.kapTipi=data["kapTipi"]!=null ?data["kapTipi"] :"";
        this.kapAdet=data["kapAdet"]!=null ?  parseInt(data["kapAdet"]) :0;
        this.parcaSayisi=data["parcaSayisi"]!=null ? parseInt( data["parcaSayisi"]) :0;
    }
  }


  static fromJS(data: any): NbKapDto {
    data = typeof data === "object" ? data : {};
    let result = new NbKapDto();

    result.init(data);
    return result;
  }
}
export class NbEkBilgiDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  ekBilgiKod:string;
  ekBilgi:string;
  dil:string;
  ec2Ihr: number;
  ulkeKodu: string;
 
  constructor(data?: NbEkBilgiDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.ekBilgiKod=data["ekBilgiKod"]!=null ?data["ekBilgiKod"] :"";
        this.dil=data["dil"]!=null ?data["dil"] :"";
        this.ekBilgi=data["tip"]!=null ?data["ekBilgi"] :"";
        this.ulkeKodu=data["ulkeKodu"]!=null ? data["ulkeKodu"] :"";
        this.ec2Ihr=data["ec2Ihr"]!=null ? parseInt( data["ec2Ihr"]) :0;
    }
  }


  static fromJS(data: any): NbEkBilgiDto {
    data = typeof data === "object" ? data : {};
    let result = new NbEkBilgiDto();

    result.init(data);
    return result;
  }
}
export class NbBelgelerDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  belgeTipi:string;
  refNo:string;
  belgeDil:string;
  tamamlayiciOlcu: string;
  tamamlayiciOlcuDil: string;
 
  constructor(data?: NbBelgelerDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.belgeTipi=data["belgeTipi"]!=null ?data["belgeTipi"] :"";
        this.belgeDil=data["belgeDil"]!=null ?data["belgeDil"] :"";
        this.refNo=data["refNo"]!=null ?data["refNo"] :"";
        this.tamamlayiciOlcu=data["tamamlayiciOlcu"]!=null ? data["tamamlayiciOlcu"] :"";
        this.tamamlayiciOlcuDil=data["tamamlayiciOlcuDil"]!=null ? data["tamamlayiciOlcuDil"] :"";
    }
  }


  static fromJS(data: any): NbBelgelerDto {
    data = typeof data === "object" ? data : {};
    let result = new NbBelgelerDto();

    result.init(data);
    return result;
  }
}
export class NbOncekiBelgelerDto {
  nctsBeyanInternalNo: string;
  kalemInternalNo: string;
  belgeTipi:string;
  refNo:string;
  belgeDil:string;
  tamamlayiciBilgi: string;
  tamamlayiciBilgiDil: string;
 
  constructor(data?: NbOncekiBelgelerDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
        this.kalemInternalNo=data["kalemInternalNo"]!=null ?data["kalemInternalNo"] :"";
        this.belgeTipi=data["belgeTipi"]!=null ?data["belgeTipi"] :"";
        this.belgeDil=data["belgeDil"]!=null ?data["belgeDil"] :"";
        this.refNo=data["refNo"]!=null ?data["refNo"] :"";
        this.tamamlayiciBilgi=data["tamamlayiciBilgi"]!=null ? data["tamamlayiciBilgi"] :"";
        this.tamamlayiciBilgiDil=data["tamamlayiciBilgiDil"]!=null ? data["tamamlayiciBilgiDil"] :"";
    }
  }


  static fromJS(data: any): NbOncekiBelgelerDto {
    data = typeof data === "object" ? data : {};
    let result = new NbOncekiBelgelerDto();

    result.init(data);
    return result;
  }
}
export class NbObAcmaDto {
  nctsBeyanInternalNo: string;  
  islemKapsami:string;
  ozetBeyanNo:string;
  ambarIci:string;
  ambarKodu: string;
  tasimaSenetNo: string;
  tasimaSatirNo: number;
  miktar: number;
 
  constructor(data?: NbObAcmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";
      
        this.islemKapsami=data["islemKapsami"]!=null ?data["islemKapsami"] :"";
        this.ozetBeyanNo=data["belgozetBeyanNoeDil"]!=null ?data["ozetBeyanNo"] :"";
        this.ambarIci=data["ambarIci"]!=null ?data["ambarIci"] :"";
        this.ambarKodu=data["ambarKodu"]!=null ? data["ambarKodu"] :"";
        this.tasimaSenetNo=data["tasimaSenetNo"]!=null ? data["tamamlayiciBilgiDil"] :"";
        this.tasimaSatirNo=data["titlNum"]!=null ? parseInt( data["tasimaSatirNo"]) :0;
        this.miktar=data["miktar"]!=null ? parseFloat( data["miktar"] ):0;
    }
  }


  static fromJS(data: any): NbObAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbObAcmaDto();

    result.init(data);
    return result;
  }
}
export class NbAbAcmaDto {
  nctsBeyanInternalNo: string; 
  beyannameNo:string;
  kalemNo:number;
  acilanKalemNo:number;
  aciklama: string;
  miktar: number;
  teslimSekli: string;
  dovizCinsi: string;
  kiymet: number;
  odemeSekli: string;
  isleminNiteligi: string;
  ticaretUlkesi: string;
  constructor(data?: NbAbAcmaDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.nctsBeyanInternalNo=data["nctsBeyanInternalNo"]!=null ?data["nctsBeyanInternalNo"] :"";     
        this.beyannameNo=data["beyannameNo"]!=null ?data["beyannameNo"] :"";
        this.aciklama=data["aciklama"]!=null ?data["aciklama"] :"";
        this.kalemNo=data["kalemNo"]!=null ? parseInt( data["kalemNo"]) :0;
        this.acilanKalemNo=data["acilanKalemNo"]!=null ? parseInt( data["acilanKalemNo"]) :0;
        this.miktar=data["miktar"]!=null ? parseFloat( data["miktar"]) :0;
        this.teslimSekli=data["teslimSekli"]!=null ?data["teslimSekli"] :"";
        this.dovizCinsi=data["dovizCinsi"]!=null ?data["dovizCinsi"] :"";
        this.kiymet= data["miktar"]!=null ? parseFloat( data["miktar"]) :0;
        this.odemeSekli=data["odemeSekli"]!=null ?data["odemeSekli"] :"";
        this.isleminNiteligi=data["isleminNiteligi"]!=null ?data["isleminNiteligi"] :"";
        this.ticaretUlkesi=data["ticaretUlkesi"]!=null ?data["ticaretUlkesi"] :"";
    }
  }


  static fromJS(data: any): NbAbAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new NbAbAcmaDto();

    result.init(data);
    return result;
  }
}
export class MesaiDto {
  mesaiInternalNo: string; 
  refNo:string;
  mesaiID: string;
  tescilStatu: string;
  aracAdedi: number;
  gumrukKodu: string;
  adres: string;
  beyannameNo: string;
  digerNo: string;
  esyaninBulunduguYer: string;
  esyaninBulunduguYerAdi: string;
  esyaninBulunduguYerKodu: string;
  firmaVergiNo: string;
  kullaniciKodu: string;
  globalHesaptanOdeme: string;
  gumrukSahasinda: string;
  irtibatAdSoyad: string;
  irtibatTelefonNo: string;
  islemTipi: string;
  odemeYapacakFirmaVergiNo:string;
  nCTSSayisi:number;
  oZBYSayisi:number;
  uzaklik:number;
  baslangicZamani:string;
  tescilTarihi:string;

  constructor(data?: MesaiDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.mesaiInternalNo=data["mesaiInternalNo"]!=null ?data["mesaiInternalNo"] :"";     
        this.beyannameNo=data["beyannameNo"]!=null ?data["beyannameNo"] :"";
        this.refNo=data["refNo"]!=null ?data["refNo"] :"";        
        this.aracAdedi=data["miktar"]!=null ? parseInt( data["aracAdedi"]) :0;
        this.mesaiID=data["mesaiID"]!=null ?data["mesaiID"] :"";
        this.gumrukKodu=data["gumrukKodu"]!=null ?data["gumrukKodu"] :"";
        this.adres=data["adres"]!=null ?data["adres"] :"";
        this.digerNo=data["digerNo"]!=null ?data["digerNo"] :"";
        this.esyaninBulunduguYer=data["esyaninBulunduguYer"]!=null ?data["esyaninBulunduguYer"] :"";
        this.esyaninBulunduguYerAdi=data["esyaninBulunduguYerAdi"]!=null ?data["esyaninBulunduguYerAdi"] :"";
        this.esyaninBulunduguYerKodu=data["esyaninBulunduguYerKodu"]!=null ?data["esyaninBulunduguYerKodu"] :"";
        this.firmaVergiNo=data["firmaVergiNo"]!=null ?data["firmaVergiNo"] :"";
        this.kullaniciKodu=data["kullaniciKodu"]!=null ?data["kullaniciKodu"] :"";
        this.globalHesaptanOdeme=data["globalHesaptanOdeme"]!=null ?data["globalHesaptanOdeme"] :"";
        this.gumrukSahasinda=data["gumrukSahasinda"]!=null ?data["gumrukSahasinda"] :"";
        this.irtibatAdSoyad=data["irtibatAdSoyad"]!=null ?data["irtibatAdSoyad"] :"";
        this.irtibatTelefonNo=data["irtibatTelefonNo"]!=null ?data["irtibatTelefonNo"] :"";
        this.islemTipi=data["islemTipi"]!=null ?data["islemTipi"] :"";
        this.odemeYapacakFirmaVergiNo=data["odemeYapacakFirmaVergiNo"]!=null ?data["odemeYapacakFirmaVergiNo"] :"";
        this.nCTSSayisi=data["nCTSSayisi"]!=null ? parseInt( data["nCTSSayisi"]) :0;
        this.oZBYSayisi=data["oZBYSayisi"]!=null ? parseInt( data["oZBYSayisi"]) :0;
        this.uzaklik=data["uzaklik"]!=null ? parseInt( data["uzaklik"]) :0;
        this.baslangicZamani=data["baslangicZamani"]!=null &&  data["baslangicZamani"]!='' ? data["baslangicZamani"] : "0001-01-01T00:00:00";
        this.tescilStatu=data["tescilStatu"]!=null ? data["tescilStatu"] : "";
        this.tescilTarihi=data["tescilTarihi"]!=null &&  data["tescilTarihi"]!='' ? data["tescilTarihi"] : "0001-01-01T00:00:00";
   
    }
  }


  static fromJS(data: any): MesaiDto {
    data = typeof data === "object" ? data : {};
    let result = new MesaiDto();

    result.init(data);
    return result;
  }
}
export class IghbDto {
  ighbInternalNo: string; 
  refNo:string;
  kullaniciKodu:string;
  gumrukKodu:string;
  izinliGondericiVergiNo: string;
  plakaBilgisi: string;
  tesisKodu: string;
  tescilStatu: string;
  tescilTarihi:string;
  constructor(data?: IghbDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.ighbInternalNo=data["ighbInternalNo"]!=null ?data["ighbInternalNo"] :"";     
        this.refNo=data["refNo"]!=null ?data["refNo"] :"";
        this.kullaniciKodu=data["kullaniciKodu"]!=null ?data["kullaniciKodu"] :"";
        this.gumrukKodu=data["gumrukKodu"]!=null ?data["gumrukKodu"] :"";
        this.izinliGondericiVergiNo=data["izinliGondericiVergiNo"]!=null ?data["izinliGondericiVergiNo"] :"";
        this.plakaBilgisi=data["plakaBilgisi"]!=null ?data["plakaBilgisi"] :"";
        this.tesisKodu=data["tesisKodu"]!=null ?data["tesisKodu"] :"";
        this.tescilStatu=data["tescilStatu"]!=null ? data["tescilStatu"] : "";
        this.tescilTarihi=data["tescilTarihi"]!=null &&  data["tescilTarihi"]!='' ? data["tescilTarihi"] : "0001-01-01T00:00:00";
   
      
    }
  }


  static fromJS(data: any): IghbDto {
    data = typeof data === "object" ? data : {};
    let result = new IghbDto();

    result.init(data);
    return result;
  }
}
export class IghbListeDto {
  ighbInternalNo: string; 
   tcgbNumarasi:string;
  
  constructor(data?: IghbListeDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
   
        this.ighbInternalNo=data["ighbInternalNo"]!=null ?data["ighbInternalNo"] :"";     
        this.tcgbNumarasi=data["tcgbNumarasi"]!=null ?data["tcgbNumarasi"] :"";
       
    }
  }


  static fromJS(data: any): IghbListeDto {
    data = typeof data === "object" ? data : {};
    let result = new IghbListeDto();

    result.init(data);
    return result;
  }
}

export class ObBeyanAlanDto {
 tip:string;
 ozetBeyanNo:string;
 beyanSahibiVergiNo:string; 
 beyanTuru:string;
 diger:string;
 dorseNo1:string; 
 dorseNo1Uyrugu:string;
 dorseNo2:string;
 dorseNo2Uyrugu:string;
 ekBelgeSayisi:string;
 emniyetGuvenlik:string;
 grupTasimaSenediNo:string;
 gumrukIdaresi:string;
 kullaniciKodu:string;
 kurye:string;
 limanYerAdiBos:string;
 limanYerAdiYuk:string;
 oncekiBeyanNo:string;
 plakaSeferNo:string;
 referansNumarasi:string;
 refNo:string;
 rejim:string;
 tasimaSekli:string;
 tasitinAdi:string;
 tasiyiciVergiNo:string;
 tirAtaKarneNo:string;
 ulkeKodu:string;
 ulkeKoduYuk:string;
 ulkeKoduBos:string;
 yuklemeBosaltmaYeri:string;
 varisCikisGumrukIdaresi:string;
 varisTarihSaati:string;
 xmlRefId:string;
 tescilStatu:string;
 tescilTarihi:string; 
  
  constructor(data?: ObBeyanAlanDto) {
    if (data) {
    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  init(data?: any) {
  
    if (data) {    
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }


  static fromJS(data: any): ObBeyanAlanDto {
    data = typeof data === "object" ? data : {};
    let result = new ObBeyanAlanDto();

    result.init(data);
    return result;
  }
}
export class ReferansDto { 
  kod: string;
  aciklama: string; 
}

