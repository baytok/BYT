import { Component, OnInit,InjectionToken,Inject } from '@angular/core';
import { AppSessionService } from '../../shared/session/app-session.service';
import { GirisService } from './giris.service';
import { Router } from "@angular/router";
import {AppServisDurumKodlari} from '../../shared/AppEnums';
import { KullaniciModel, KullaniciSonucModel, } from './giris-service-proxies';
import { BeyannameServiceProxy ,SessionServiceProxy} from '../../shared/service-proxies/service-proxies';
import {
  KullaniciServisDto
 } from '../../shared/service-proxies/service-proxies';
 import { MatSnackBar } from "@angular/material/snack-bar";
 import { accountModuleAnimation } from '../../shared/animations/routerTransition';
@Component({
  selector: 'app-giris',
  animations: [accountModuleAnimation()],
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
    private _beyanSession: SessionServiceProxy,
     ) { }

  ngOnInit() {
 
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  login() {    
    this.submitting = true;   
    const promise = this.girisService
    .getKullaniciGiris(this.girisService.kullaniciModel.kullanici,this.girisService.kullaniciModel.sifre)
    .toPromise();
  promise.then(
    result => {
  
      const servisSonuc = new KullaniciServisDto();
      servisSonuc.init(result);
      
      if (servisSonuc.ServisDurumKodu===AppServisDurumKodlari.Available ) {   
          var token = JSON.parse(servisSonuc.Sonuc).Token;
          this._beyanSession.token=token;
          var kullaniciKod=JSON.parse(servisSonuc.Sonuc).KullaniciKod;
          var kullaniciAdi=JSON.parse(servisSonuc.Sonuc).KullaniciAdi;
          var yetkiler= JSON.parse(servisSonuc.Sonuc).Yetkiler;

        this.girisService.setLoginInfo(kullaniciKod,token,kullaniciAdi,yetkiler);
        this.router.navigateByUrl('/app');
      }
      else 
      {  
         this.openSnackBar(servisSonuc.Sonuc , "Tamam");
      }
    },
    err => {
      console.log(err);
    }
  );
   
  }

}
