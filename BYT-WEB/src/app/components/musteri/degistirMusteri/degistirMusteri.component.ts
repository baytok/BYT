import { Component, OnInit,Inject,Injector } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import { MustMatch } from "../../../../shared/helpers/must-match.validator";
import {
 MusteriDto,ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../../shared/service-proxies/service-proxies";
import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
import { MatInput } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
export interface DialogData {
  id: number;  
  vergiNo:string;
  firmaAd:string;
  adres:string;
  aktif:boolean;
  telefon:string;
  mailAdres:string;
 
}
@Component({
  selector: 'app-degistirMusteri',
  templateUrl: './degistirMusteri.component.html',
  styleUrls: ['./degistirMusteri.component.css']
})

export class DegistirMusteriComponent implements OnInit {
  musteriForm:FormGroup;
  submitted: boolean = false;  
  musteriDataSource: MusteriDto[]=[];
  constructor(
    public dialogRef: MatDialogRef<DegistirMusteriComponent>,
    private _fb: FormBuilder,
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) { 
    this.musteriForm = this._fb.group(
      {    
        id:[], 
      
        adres:new FormControl("", [Validators.required, Validators.maxLength(150),]),
        vergiNo: new FormControl("", [Validators.required,Validators.maxLength(15)]),
        firmaAd:new FormControl("", [Validators.required,Validators.maxLength(150)]), 
      
        aktif: [true],
     
        telefon: new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(12),
          Validators.pattern("^[0-9]*$")
        ]),
       
        mailAdres: new FormControl("", [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(80),
          Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")
        ])
      }
    )

  }

  ngOnInit() {
 
    this.loadMusteriForm();
    console.log(this.musteriForm);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  get focus() {
    return this.musteriForm.controls;
  } 
  
  loadMusteriForm(){
    this.musteriForm.setValue({
      id: this.data.id,     
      adres:this.data.adres,    
      aktif:this.data.aktif,
      firmaAd:this.data.firmaAd,
      vergiNo:this.data.vergiNo,
      telefon:this.data.telefon,
      mailAdres:this.data.mailAdres,
     
    });
  }
  save(){
    this.submitted = true;

    // stop here if form is invalid
    if (this.musteriForm.invalid) {
      const invalid = [];
      const controls = this.musteriForm.controls;
      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }
      
      alert(
        "ERROR!! :-)\n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:"  + JSON.stringify(invalid, null, 4)
      );
     
      return;
    }
    
    let musteri=new MusteriDto();
    musteri.init(this.musteriForm.value);
 
      const promise = this.beyanServis
        .restoreMusteri(musteri)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);       

          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.musteriForm.disable();
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.kullaniciForm.value, null, 4)
    // );

  
  }
  close(result: any): void {
    this.dialogRef.close(result);
   
  }
}
