import { Component, OnInit, Inject,Injector } from '@angular/core';
import {MatSnackBar,MatDialog,MatDialogRef,MAT_DIALOG_DATA} from '@angular/material';
import { BeyannameServiceProxy} from '../../../shared/service-proxies/service-proxies';

import {
  BeyannameSonucDto,
  SonucBelgelerDto,
  SonucHatalarDto
  
 } from '../../../shared/service-proxies/service-proxies';
export interface DialogData {
  guidOf: string;
  islemInternalNo: string;

}
@Component({
  selector: 'app-sonucservis',
  templateUrl: './beyannamesonucservis.component.html',
  styleUrls: ['./beyannamesonucservis.component.scss']
  
})

export class BeyannameSonucservisComponent 
implements OnInit {
  step = 0;
  constructor(
    injector: Injector,
    public dialogRef: MatDialogRef<BeyannameSonucservisComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private beyanServis: BeyannameServiceProxy,
    
  ) { 
   
  }
  guidOf= this.data.guidOf;
  islemInternalNo= this.data.islemInternalNo;
  _sonucHatalar:SonucHatalarDto[];
  _sonucBelgeler:SonucBelgelerDto[];
  ngOnInit() {
    this.beyanServis.getBeyannameSonucSorgula(this.islemInternalNo,this.guidOf)
    .subscribe( (result)=>{  
     
     const sonuc = new BeyannameSonucDto();
      sonuc.init(result);     
      this._sonucBelgeler=sonuc.Belgeler;
      this._sonucHatalar =sonuc.Hatalar;
    }, (err)=>{
     console.log(err);
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
