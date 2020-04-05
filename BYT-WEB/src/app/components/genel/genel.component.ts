import { Component, OnInit } from '@angular/core';
import { GirisService } from '../../../account/giris/giris.service';
import { Router } from "@angular/router";
import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../shared/service-proxies/service-proxies';
import { MatSnackBar } from "@angular/material/snack-bar";
import {
  UserRoles
} from "../../../shared/service-proxies/UserRoles";
import {
  IstatistikDto
 } from '../../../shared/service-proxies/service-proxies';
@Component({
  selector: 'app-genel',
  templateUrl: './genel.component.html',
  styleUrls: ['./genel.component.scss']
})
export class GenelComponent implements OnInit {
  kullanici="";
  beyannameSayisi:number;
  kontrolGonderimSayisi:number;
  sonucBekelenen:number;
  tescilEdilenBeyannameSayisi:number;
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private  girisService: GirisService,
        private router:Router  ,
        private _userRoles:UserRoles,
        private snackBar: MatSnackBar,
        ) { }

  ngOnInit() {
   
    this.kullanici=this.girisService.loggedKullanici; 
    this.getIstatistik();
  }

  getIstatistik() {
  
    const promise = this.beyanServis
    .getIstatistik(this.kullanici)
    .toPromise();
    promise.then(
    result => {
     
      const istatitik = new IstatistikDto();
      istatitik.init(result);    
      this.beyannameSayisi=istatitik.BeyannameSayisi;   
      this.kontrolGonderimSayisi=istatitik.KontrolGonderimSayisi;   
      this.tescilEdilenBeyannameSayisi=istatitik.TescilBeyannameSayisi;   
      this.sonucBekelenen=istatitik.SonucBeklenenSayisi;   
      
    //  this.openSnackBar(istatitik.KontrolGonderimSayisi.toString(), "Tamam");
    },
    err => {
      this.beyanServis.errorHandel(err);    
    }
     );
    }
     openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
          duration: 2000
        });
      }
      
  
}
