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
 FirmaDto,MusteriDto,ServisDto
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
  musteriNo:string;
  firmaNo:string;
  vergiNo:string;
  firmaAd:string;
  adres:string;
  aktif:boolean;
  telefon:string;
  mailAdres:string;
 
}
@Component({
  selector: 'app-degistirFirma',
  templateUrl: './degistirFirma.component.html',
  styleUrls: ['./degistirFirma.component.css']
})

export class DegistirFirmaComponent implements OnInit {
  firmaForm:FormGroup;
  submitted: boolean = false;  
  musteriDataSource: MusteriDto[]=[];
  constructor(
    public dialogRef: MatDialogRef<DegistirFirmaComponent>,
    private _fb: FormBuilder,
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) { 
    this.firmaForm = this._fb.group(
      {    
        id:[], 
        musteriNo:new FormControl("", [Validators.required]),
        firmaNo:[Validators.required], 
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
 
    this.loadfirmaForm();
 this.getAktifMusteriler();
 this.firmaForm.markAllAsTouched();
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  get focus() {
    return this.firmaForm.controls;
  } 
  getAktifMusteriler()
  {
      this.beyanServis.getAllAktifMusteriler()
     .subscribe( (result: MusteriDto[])=>{
           this.musteriDataSource=result;
          
      }, (err)=>{
        this.beyanServis.errorHandel(err);    
      });
    
  }
  get musteriNo(): string {
    let musteriNo= this.firmaForm ? this.firmaForm.get('musteriNo').value : '';
    if(musteriNo==='')
    return '';
    let selected = this.musteriDataSource.find(c=> c.musteriNo == musteriNo);
    this.firmaForm.get("musteriNo").setValue(selected.musteriNo);
 
    return selected.musteriNo;
  }
  loadfirmaForm(){
    this.firmaForm.setValue({
      id: this.data.id,  
      musteriNo:this.data.musteriNo,
      firmaNo:this.data.firmaNo,   
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
    if (this.firmaForm.invalid) {
      const invalid = [];
      const controls = this.firmaForm.controls;
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
    
    let firma=new FirmaDto();
    firma.init(this.firmaForm.value);
 
      const promise = this.beyanServis
        .restoreFirma(firma)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);       

          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.firmaForm.disable();
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
