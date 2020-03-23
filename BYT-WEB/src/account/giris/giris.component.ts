import { Component, OnInit,InjectionToken,Inject } from '@angular/core';
import { AppSessionService } from '../../shared/session/app-session.service';
import { GirisService } from './giris.service';
import { Router } from "@angular/router";
import {AppServisDurumKodlari} from '../../shared/AppEnums';
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../shared/service-proxies/service-proxies";
import {
  KullaniciDto,ServisDto
 } from '../../shared/service-proxies/service-proxies';
 import { MatSnackBar } from "@angular/material/snack-bar";
 export const LoggedToken = new InjectionToken<string[]>('LoggedToken ');
@Component({
  selector: 'app-giris',
  templateUrl: './giris.component.html',
  styleUrls: ['./giris.component.scss']
})
export class GirisComponent implements OnInit {
  submitting = false;
  @Inject(LoggedToken) loggedUserInfo;
  constructor(
    private _UserSession: AppSessionService,
    public   girisService: GirisService,     

    private router:Router,
    private snackBar: MatSnackBar,
    private beyanServis: BeyannameServiceProxy,
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
     
      const servisSonuc = new ServisDto();
      servisSonuc.init(result);
     
      var token = JSON.parse(servisSonuc.Sonuc).ReferansNo;
      var kullaniciAdi=JSON.parse(servisSonuc.Sonuc).Guid;
      
      this.girisService.setLoginToken(this.girisService.kullaniciModel.kullanici,token,kullaniciAdi);
       

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
