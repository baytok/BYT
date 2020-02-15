import { Component, OnInit, Inject,Injector } from '@angular/core';
import {MatSnackBar,MatDialog,MatDialogRef,MAT_DIALOG_DATA} from '@angular/material';
import { BeyannameServiceProxy} from '../../../shared/service-proxies/service-proxies';

import {
  BeyannameSonucDto
  
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
  ngOnInit() {
    this.beyanServis.getBeyannameSonucSorgula(this.islemInternalNo,this.guidOf)
    .subscribe( (result)=>{  
    
     const sonuc_ = new BeyannameSonucDto();
      sonuc_.init(result);     
      console.log(sonuc_);
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
