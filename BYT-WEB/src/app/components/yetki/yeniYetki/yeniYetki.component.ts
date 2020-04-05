import { Component, OnInit } from '@angular/core';
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
  YetkiDto,  ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../../shared/service-proxies/service-proxies";
import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
import { MatDialog,MatDialogRef } from "@angular/material/dialog";
import { MatInput } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";
@Component({
  selector: 'app-yeniYetki',
  templateUrl: './yeniYetki.component.html',
  styleUrls: ['./yeniYetki.component.css']
})
export class YeniYetkiComponent implements OnInit {
  yetkiForm:FormGroup;
  submitted: boolean = false;  
  yetkiDataSource: YetkiDto[]=[];
 
  constructor(
    public dialogRef: MatDialogRef<YeniYetkiComponent>,
    private _fb: FormBuilder,
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles:UserRoles,
  ) { }

  ngOnInit() {
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Bu Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
   
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  get focus() {
    return this.yetkiForm.controls;
  }
 
  buildForm(): void {
    this.yetkiForm = this._fb.group(
      {     
        yetkiAdi: new FormControl("", [Validators.required, Validators.maxLength(50),]),
        aciklama:new FormControl("", [Validators.required, Validators.maxLength(500),]),
      
        aktif: [true],
     
      }
    )
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
    
    let yeniYetki=new YetkiDto();
    yeniYetki.init(this.yetkiForm.value);
  
      const promise = this.beyanServis
        .setYetki(yeniYetki)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
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
  close(result: any): void {
    this.dialogRef.close(result);
   
  }
}
