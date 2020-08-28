import { Component, OnInit, Inject,Injector } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BeyannameServiceProxy} from '../../../../shared/service-proxies/service-proxies';

import {
  IghbSonucDto,
  SonucHatalarDto,
 
  
 } from '../../../../shared/service-proxies/service-proxies';
export interface DialogData {
  guidOf: string;
  islemInternalNo: string;

}
@Component({
  selector: 'app-sonucservis',
  templateUrl: './ighbsonucservis.component.html',
  styleUrls: ['./ighbsonucservis.component.scss']
  
})

export class IghbSonucServisComponent 
implements OnInit {
  step = 0;
  ngClass="";
  constructor(
    injector: Injector,
    public dialogRef: MatDialogRef<IghbSonucServisComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private beyanServis: BeyannameServiceProxy,
    
  ) { 
   
  }
  guidOf= this.data.guidOf;
  islemInternalNo= this.data.islemInternalNo;  
  dovizKuruAlis:string; 
  hataSayisi:number;  
  _sonucHatalar:SonucHatalarDto[];
 
  ngOnInit() {
    this.beyanServis.getIghbSonucSorgula(this.islemInternalNo,this.guidOf)
    .subscribe( (result)=>{  
    
     const sonuc = new IghbSonucDto();
      sonuc.init(result);     
   
      this._sonucHatalar =sonuc.Hatalar;    
     // this.dovizKuruAlis= sonuc.DovizKuruAlis;   
      this.hataSayisi=this._sonucHatalar.length;
  

      if(this._sonucHatalar.length>0) {
        this.step = 1;
        this.ngClass="matClassRed";
      }
    

    }, (err)=>{
      this.beyanServis.errorHandel(err);    
   });
  }
  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }
  close(result: any): void {
    this.dialogRef.close(result);
  }
}
