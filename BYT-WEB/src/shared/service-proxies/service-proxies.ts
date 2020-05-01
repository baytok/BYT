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
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      
    const httpOptions = {
     headers: headers_object
    };
    return this.http.get(
      this.baseUrl + "Kullanicilar/KullaniciHizmeti/",httpOptions
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
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "AktifYetkiler/KullaniciHizmeti/",httpOptions 
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
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.get(
      this.baseUrl + "AktifMusteriler/KullaniciHizmeti/",httpOptions 
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

  TescilMesajiHazirla(IslemInternalNo, Kullanici) {
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

  TescilGonderimi(IslemInternalNo, Kullanici, imzaliVeri) {
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
        IslemInternalNo + "/" + Kullanici+"/"+imzaliVeri,null,httpOptions  
        );
   
    
  }

  getSonucSorgula(Guid) {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    var headers_object = new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': "Bearer "+token})
      

    const httpOptions = {
     headers: headers_object
    };

    return this.http.post<any>(
      this.baseUrl + "Servis/SorgulamaHizmeti/" + Guid,null,
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

  getBeyanname(IslemInternalNo) {
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
  getBeyannameAcma(IslemInternalNo) {
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
  restoreBeyannameAcma(acma: BeyannameAcmaDto[], kalemInternalNo:string , beyanInternalNo:string) {
   
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
  getOzetBeyanAcma(IslemInternalNo) {
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
  restoreOzetBeyanAcma(ozetBeyanAcma: OzetBeyanAcmaDto) {
   
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
  getTasimaSenet(IslemInternalNo) {
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
  restoreTasimaSenet(tasimaSenet: TasimaSenetDto[],ozetBeyanInternalNo:string , beyanInternalNo:string) {
   
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
  getTasimaSatir(IslemInternalNo) {
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
  restoreTasimaSatir(tasimaSatir: TasimaSatirDto[],ozetBeyanInternalNo:string ,tasimaSenetInternalNo:string, beyanInternalNo:string) {
   
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

  removeOzetBeyanAcma(ozetBeyanInternalNo,beyanInternalNo) {
  
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
  

  getTeminat(IslemInternalNo) {
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
  restoreTeminat(teminat: TeminatDto[], beyanInternalNo:string) {
   
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

  getFirma(IslemInternalNo) {
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
  restoreFirma(firma: FirmaDto[], beyanInternalNo:string) {
   
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

  setBeyanname(beyanname: BeyannameDto) {
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
  public beyanStatu: string;
  public token: string;

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
      this.SonucVeriler= this.Bilgiler[0].sonucVeriler;
     
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
  id: number;
  yetkiAdi: string;
  aciklama:string;  
  aktif:boolean;
 

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
      this.yetkiAdi = data["yetkiAdi"];
      this.aciklama = data["aciklama"];
      this.aktif = data["aktif"];
    
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
   
    data["yetkiAdi"]= this.yetkiAdi ;
    data["aciklama"]= this.aciklama ;  
    data["id"] = this.id;  
    data["aktif"]=this.aktif ;
   
    return data;
  }
  clone(): KullaniciDto {
    const json = this.toJSON();
    let result = new KullaniciDto();
    result.init(json);
    return result;
  }
}

export class KullaniciYetkiDto {
  id: number;
  kullaniciKod: string;
  yetkiId:number;  
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
      this.yetkiId = data["yetkiId"];
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
    data["yetkiId"]= this.yetkiId ;  
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

  static fromJS(data: any): ServisDto {
    data = typeof data === "object" ? data : {};
    let result = new ServisDto();

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

  clone(): ServisDto {
    const json = this.toJSON();
    let result = new ServisDto();
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
  id: number;
  yetkiAdi: string; 
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
export class TeminatDto {
  beyanInternalNo: string;
  teminatSekli: string;
  teminatOrani:number;
  globalTeminatNo:string;
  bankaMektubuTutari:number;
  nakdiTeminatTutari:number;
  digerTutar:number;
  digerTutarReferansi:string;
  aciklama:string;

  constructor(data?: TeminatDto) {
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


  static fromJS(data: any): TeminatDto {
    data = typeof data === "object" ? data : {};
    let result = new TeminatDto();

    result.init(data);
    return result;
  }
}
export class FirmaDto {
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

  constructor(data?: FirmaDto) {
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


  static fromJS(data: any): FirmaDto {
    data = typeof data === "object" ? data : {};
    let result = new FirmaDto();

    result.init(data);
    return result;
  }
}
export class OzetBeyanAcmaDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  ozetBeyanNo: string;
  islemKapsami:string;
  ambar:string;
  baskaRejim:string;
  aciklama:string;
 

  constructor(data?: OzetBeyanAcmaDto) {
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


  static fromJS(data: any): OzetBeyanAcmaDto {
    data = typeof data === "object" ? data : {};
    let result = new OzetBeyanAcmaDto();

    result.init(data);
    return result;
  }
}
export class TasimaSenetDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  tasimaSenediNo: string;

  
 

  constructor(data?: TasimaSenetDto) {
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


  static fromJS(data: any): TasimaSenetDto {
    data = typeof data === "object" ? data : {};
    let result = new TasimaSenetDto();

    result.init(data);
    return result;
  }
}
export class TasimaSatirDto {
  id:number;
  beyanInternalNo: string;
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  ambarKodu:string;
  miktar:number;
  tasimaSatirNo:string;
  
 

  constructor(data?: TasimaSatirDto) {
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


  static fromJS(data: any): TasimaSatirDto {
    data = typeof data === "object" ? data : {};
    let result = new TasimaSatirDto();

    result.init(data);
    return result;
  }
}
export class ReferansDto { 
  kod: string;
  aciklama: string; 
}

