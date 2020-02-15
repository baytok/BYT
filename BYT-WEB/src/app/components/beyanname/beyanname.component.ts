import { Component, OnInit, Inject,Injector } from '@angular/core';
import { BeyannameServiceProxy,SessionServiceProxy} from '../../../shared/service-proxies/service-proxies';
import { MatSnackBar,MatDialog} from '@angular/material';

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemlerDto,
  TarihceDto,
  ServisDto
 } from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'app-beyanname',
  templateUrl: './beyanname.component.html',
  styleUrls: ['./beyanname.component.scss']
})
export class BeyannameComponent implements OnInit {
 
   
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar ,
   
  ) { }
  guidOf= this.session.guidOf;
  islemInternalNo= this.session.islemInternalNo;
  _beyanname:BeyannameDto;
  _kalemler:KalemlerDto[];
  ngOnInit() {
    this.getBeyanname(this.session.islemInternalNo);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getBeyanname(islemInternalNo:string){
    console.log(islemInternalNo);
    this.beyanServis.getBeyanname(islemInternalNo)
    .subscribe( (result)=>{  
      console.log(result);
      const _beyannameBilgileri = new BeyannameBilgileriDto();
      _beyannameBilgileri.init(result);  
      console.log(_beyannameBilgileri);

      this._beyanname =_beyannameBilgileri.Beyanname;

       if(this._beyanname==null )  
       {
        this.openSnackBar(islemInternalNo+ "  Bulunamadı",'Tamam'); 
         return;
       }
           
       this._kalemler =_beyannameBilgileri.Kalemler;
 
    
    }, (err)=>{
     console.log(err);
   });


  }

}
