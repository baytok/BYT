import { Component, OnInit, Inject,Injector } from '@angular/core';
import {MatSnackBar,MatDialog,MatDialogRef,MAT_DIALOG_DATA} from '@angular/material';
import { AppComponent } from 'src/app/app.component';
export interface DialogData {
  guidOf: string;

}
@Component({
  selector: 'app-sonucservis',
  templateUrl: './sonucservis.component.html',
  styleUrls: ['./sonucservis.component.scss']
})

export class SonucservisComponent 
implements OnInit {
  step = 0;
  constructor(
    injector: Injector,
    public dialogRef: MatDialogRef<SonucservisComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
    
  ) { 
   
  }
  guidOf= this.data.guidOf;
  ngOnInit() {
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
