
 import { mergeMap as _observableMergeMap, catchError as _observableCatch,map, filter,tap } from 'rxjs/operators';
// import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken,Injector } from "@angular/core";
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpResponseBase
} from "@angular/common/http";
import { Router } from "@angular/router";
import { KullaniciModel, KullaniciSonucModel } from './giris-service-proxies';
import { strict } from 'assert';
export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");
export const LoggedToken = new InjectionToken<string[]>('LoggedToken ');
@Injectable({
    providedIn: 'root'
  })
  
export class GirisService {
     private http: HttpClient;
     @Inject(LoggedToken) _loggedToken; 
     private baseUrl: string;
      kullaniciModel=new KullaniciModel;
      kullaniciSonuc: KullaniciSonucModel;
     
    constructor(
     
       @Inject(HttpClient) http: HttpClient,       
       private router:Router,
       @Optional()@Inject(API_BASE_URL) baseUrl?: string            
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
    public setLoginToken(kullanici:string,token:string, kullaniciAdi:string){
      // localStorage.setItem('bytServis_access_token', token);
      // localStorage.setItem('kullanici', kullanici);
      // localStorage.setItem('kullaniciAdi', kullaniciAdi);
      localStorage.setItem('kullaniciInfo', JSON.stringify({ token: token, name: kullaniciAdi, user:kullanici }))
     
      const Kullanici = new InjectionToken<string[]>('Kullanici');
      const injector = Injector.create([
        {provide: Kullanici, multi: true, useValue: kullanici},
        {provide: Kullanici, multi: true, useValue: token},]);

      const locales: string[] = injector.get(Kullanici);
  
       this._loggedToken=locales;
       
     
     
    }
    setKullaniciCikis() {
      
    // localStorage.removeItem('bytServis_access_token');
    // localStorage.removeItem('kullanici');
    // localStorage.removeItem('kullaniciAdi');
    // localStorage.removeItem('kullaniciInfo');
    this.router.navigateByUrl('/giris');
    }
    public get loggedIn(): boolean{
      return localStorage.getItem('kullaniciInfo') !==  null;
    }
    public get loggedKullanici(): string{
      var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
        var user = currentUser.user;
      return user ;
    }

    public get loggedToken(): string{
      var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
      var token = currentUser.token;
        return token ;
     
    }
    public get getShownLoginName():string{
      var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
      var name = currentUser.name;
        return name ;
    }

  }