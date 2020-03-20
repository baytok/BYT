
 import { mergeMap as _observableMergeMap, catchError as _observableCatch,map, filter,tap } from 'rxjs/operators';
// import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpResponseBase
} from "@angular/common/http";
import { KullaniciModel, KullaniciSonucModel } from './giris-service-proxies';
export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");
@Injectable({
    providedIn: 'root'
  })
export class GirisService {
     private http: HttpClient;
     private baseUrl: string;
     kullaniciModel=new KullaniciModel;
     kullaniciSonus: KullaniciSonucModel;
     
    constructor(
       @Inject(HttpClient) http: HttpClient,
       @Optional() @Inject(API_BASE_URL) baseUrl?: string        
    ) {
      this.http = http;
      this.baseUrl = baseUrl ? baseUrl : "https://localhost:44345/api/BYT/";
     
    }
  
    getKullaniciGiris(KullaniciKod:string, KullaniciSifre:string) {
      return this.http.get<any>(
        this.baseUrl +
          "KullaniciGiris/KullaniciHizmeti/"+KullaniciKod+"/"+KullaniciSifre    
      );
    
    }
    setKullaniciCikis() {
    localStorage.removeItem('bytServis_access_token');
    localStorage.removeItem('kullanici');
    }
    public get loggedIn(): boolean{
      return localStorage.getItem('bytServis_access_token') !==  null;
    }

}
