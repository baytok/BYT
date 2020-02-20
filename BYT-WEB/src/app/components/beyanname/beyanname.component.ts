import { Component, OnInit, Inject,Injector } from '@angular/core';
import { BeyannameServiceProxy,SessionServiceProxy,BeyannameBilgileriProxy} from '../../../shared/service-proxies/service-proxies';
import { MatSnackBar,MatDialog} from '@angular/material';

import {  
  BeyannameDto,
  KalemlerDto,
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
    private beyanBilgileriServis: BeyannameBilgileriProxy,
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
   
    this.beyanServis.getBeyanname(islemInternalNo)
    .subscribe( (result)=>{  
     
      const _beyannameBilgileri = this.beyanBilgileriServis.init(result); 
      console.log(_beyannameBilgileri);
       if(this._beyanname==null )  
       {
        islemInternalNo="";
        this.openSnackBar(islemInternalNo+ "  Bulunamadı",'Tamam'); 
         return;
       }
     
      //  this._kalemler =_beyannameBilgileri.Kalemler;
 
    
    }, (err)=>{
     console.log(err);
   });
  }

  getBeyannameKopyalama(islemInternalNo:string){
   let yeniislemInternalNo:string;
   const promise= this.beyanServis.getBeyannameKopyalama(islemInternalNo).toPromise();
    promise.then( (result)=>{  
      console.log(result);
      const servisSonuc = new ServisDto();
      servisSonuc.init(result);  
      yeniislemInternalNo=servisSonuc.Bilgiler[0].referansNo;
      console.log(yeniislemInternalNo);

      if(this._beyanname==null )  
       {
        islemInternalNo="";
        this.openSnackBar(yeniislemInternalNo,'Tamam'); 
       
       }
     
    }, (err)=>{
     console.log(err);
   });


  }
  

}
