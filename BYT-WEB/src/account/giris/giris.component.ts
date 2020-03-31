import { Component, OnInit,InjectionToken,Inject } from '@angular/core';
import { AppSessionService } from '../../shared/session/app-session.service';
import { GirisService } from './giris.service';
import { Router } from "@angular/router";
import {AppServisDurumKodlari} from '../../shared/AppEnums';
import { KullaniciModel, KullaniciSonucModel, } from './giris-service-proxies';

import {
  KullaniciServisDto
 } from '../../shared/service-proxies/service-proxies';
 import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: 'app-giris',
  templateUrl: './giris.component.html',
  styleUrls: ['./giris.component.scss']
})
export class GirisComponent implements OnInit {
  submitting = false;

  constructor(
    private _UserSession: AppSessionService,
    public   girisService: GirisService,     

    private router:Router,
    private snackBar: MatSnackBar,

     ) { }

  ngOnInit() {
 
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  login() {    
    const promise = this.girisService
    .getKullaniciGiris(this.girisService.kullaniciModel.kullanici,this.girisService.kullaniciModel.sifre)
    .toPromise();
  promise.then(
    result => {
    
      const servisSonuc = new KullaniciServisDto();
      servisSonuc.init(result);
     
      var token = JSON.parse(servisSonuc.Sonuc).Token;
      var kullaniciKod=JSON.parse(servisSonuc.Sonuc).KullaniciKod;
      var kullaniciAdi=JSON.parse(servisSonuc.Sonuc).KullaniciAdi;
      var yetkiler= JSON.parse(servisSonuc.Sonuc).Yetkiler;
   
      this.girisService.setLoginInfo(kullaniciKod,token,kullaniciAdi,yetkiler);

      if (servisSonuc.ServisDurumKodu===AppServisDurumKodlari.Available ) {     
        this.submitting = true;   
        this.router.navigateByUrl('/app');
      }
      else 
      {  this.submitting = false;
        this.openSnackBar(servisSonuc.Sonuc , "Tamam");
      }
    },
    err => {
      console.log(err);
    }
  );
   
  }

}
