
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
import {
  KullaniciYetkileri,
 
 } from '../../shared/service-proxies/service-proxies';
import { KullaniciModel, KullaniciSonucModel, } from './giris-service-proxies';

import { strict } from 'assert';
export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");

@Injectable({
    providedIn: 'root'
  })
  
export class GirisService {
     private http: HttpClient;
      private baseUrl: string;
      kullaniciModel=new KullaniciModel;
      kullaniciSonuc=new KullaniciSonucModel;
     
    constructor(
 
       @Inject(HttpClient) http: HttpClient,       
       private router:Router,
       @Optional()@Inject(API_BASE_URL) baseUrl?: string            
    ) {
      this.http = http;
   
    // this.baseUrl = baseUrl ? baseUrl : "http://servis.byt.com/BYTServis/api/BYT/";
     this.baseUrl = baseUrl ? baseUrl : "https://localhost:44345/api/BYT/";
    }
  
    getKullaniciGiris(KullaniciKod:string, KullaniciSifre:string) {
      return this.http.get<any>(
        this.baseUrl +
          "KullaniciGiris/KullaniciHizmeti/"+KullaniciKod+"/"+KullaniciSifre    
      );
    
    }
    public setLoginInfo(kullaniciKod:string,token:string, kullaniciAdi:string, yetki:[]){
   
      this.kullaniciSonuc.token=token;
      this.kullaniciSonuc.kullaniciKod=kullaniciKod;
      this.kullaniciSonuc.kullaniciAdi=kullaniciAdi;

      localStorage.setItem('kullaniciInfo', JSON.stringify({ token: token, name: kullaniciAdi, user:kullaniciKod, roles:yetki }))
      
     
    }
    setKullaniciCikis() {
      
     localStorage.removeItem('kullaniciInfo');
   
    this.router.navigateByUrl('/giris');
    }
    public get loggedIn(): boolean{
      return localStorage.getItem('kullaniciInfo') !==  null;
    }
    public get loggedKullanici(): string{
     // return this.kullaniciSonuc.kullaniciKod;
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

    public get loggedRoles(): KullaniciYetkileri[]{
      var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
        var user = currentUser.roles;
      return user ;
    }

  }