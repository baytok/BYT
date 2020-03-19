import { Component, OnInit } from '@angular/core';
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
@Component({
  selector: 'app-giris',
  templateUrl: './giris.component.html',
  styleUrls: ['./giris.component.scss']
})
export class GirisComponent implements OnInit {
 
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
    const promise = this.beyanServis
    .getKullaniciGiris(this.girisService.authenticateModel.userNameOrEmailAddress,this.girisService.authenticateModel.password)
    .toPromise();
  promise.then(
    result => {
     
      const servisSonuc = new ServisDto();
      servisSonuc.init(result);
    
     // console.log(servisSonuc.Bilgiler[0].referansNo);

      if (servisSonuc.ServisDurumKodu===AppServisDurumKodlari.Available ) {        
        this.openSnackBar(servisSonuc.Bilgiler[0].referansNo +"/"+servisSonuc.Bilgiler[0].sonuc, "Tamam");
        this._UserSession._user = this.girisService.authenticateModel;
  
        this.router.navigateByUrl('/app');
      }
      else this.openSnackBar(servisSonuc.Hatalar[0].hataKodu.toString() +"/"+servisSonuc.Hatalar[0].hataAciklamasi , "Tamam");
    },
    err => {
      console.log(err);
    }
  );
   
  }

}
