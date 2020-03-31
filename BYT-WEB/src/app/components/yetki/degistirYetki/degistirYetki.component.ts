
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
  YetkiDto,ServisDto
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
  yetkiAdi: string;
  aciklama: string;
  aktif:boolean;
 
}
@Component({
  selector: 'app-degistirYetki',
  templateUrl: './degistirYetki.component.html',
  styleUrls: ['./degistirYetki.component.css']
})

export class DegistirYetkiComponent implements OnInit {
  yetkiForm:FormGroup;
  submitted: boolean = false;  
  yetkiDataSource: YetkiDto[]=[];

  constructor(
    private _fb: FormBuilder,
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) { 
    this.yetkiForm = this._fb.group(
      {    
        id:[], 
        yetkiAdi: new FormControl("", [Validators.required, Validators.maxLength(50),]),
        aciklama:new FormControl("", [Validators.required,Validators.maxLength(500)]), 
        aktif: [true],
     
      }
    )

  }

  ngOnInit() {
 
    this.loadYetkiForm();
  
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  get focus() {
    return this.yetkiForm.controls;
  } 
 
  loadYetkiForm(){
    this.yetkiForm.setValue({
      id: this.data.id,
      yetkiAdi: this.data.yetkiAdi,
      aciklama:this.data.aciklama,
      aktif:this.data.aktif,
     
    });
  }
 
  save(){
    this.submitted = true;

    // stop here if form is invalid
    if (this.yetkiForm.invalid) {
      const invalid = [];
      const controls = this.yetkiForm.controls;
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
    
    let yetki=new YetkiDto();
    yetki.init(this.yetkiForm.value);
  
      const promise = this.beyanServis
        .restoreYetki(yetki)
        .toPromise();
      promise.then(
        result => {
        
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);   
          
         this.openSnackBar(servisSonuc.Sonuc, "Tamam");
         this.yetkiForm.disable();
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.kullaniciForm.value, null, 4)
    // );
   
  
  }

}
