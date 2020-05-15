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
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import {
  KullaniciDto, MusteriDto,YetkiDto,KullaniciYetkiDto, ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../../shared/service-proxies/service-proxies";
import * as _ from 'lodash';
import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
import { MatDialog } from "@angular/material/dialog";
import { MatInput } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-yeniKullanici',
  templateUrl: './yeniKullanici.component.html',
  styleUrls: ['./yeniKullanici.component.css']
})
export class YeniKullaniciComponent implements OnInit {
  kullaniciForm:FormGroup;
  submitted: boolean = false;  
  kullaniciDataSource: KullaniciDto[]=[];
  musteriDataSource: MusteriDto[]=[];
  yetkiDataSource: YetkiDto[]=[];
  checkedRolesMap: { [key: string]: boolean } = {};
  defaultRoleCheckedStatus = false;
  kullaniciYetkileri:KullaniciYetkiDto[];
  constructor(
    public dialogRef: MatDialogRef<YeniKullaniciComponent>,
    private _fb: FormBuilder,
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
  ) { }

  ngOnInit() {
    this.buildForm();   
    this.getAktifYetkiler();
    this.getAktifMusteriler();
   
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  get focus() {
    return this.kullaniciForm.controls;
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
  getAktifYetkiler()
  {
      this.beyanServis.getAllAktifYetkiler()
     .subscribe( (result: YetkiDto[])=>{
           this.yetkiDataSource=result;
        
      }, (err)=>{
        this.beyanServis.errorHandel(err);    
      });
    
  }
  get firmaAd(): string {
    let vergiNo= this.kullaniciForm ? this.kullaniciForm.get('vergiNo').value : '';
    if(vergiNo==='')
    return '';
    let selected = this.musteriDataSource.find(c=> c.vergiNo == vergiNo);
    this.kullaniciForm.get("firmaAd").setValue(selected.firmaAd);
 
    return selected.firmaAd;
  }
  buildForm(): void {
    this.kullaniciForm = this._fb.group(
      {     
        kullaniciKod: new FormControl("", [Validators.required, Validators.maxLength(15),]),
        ad:new FormControl("", [Validators.required, Validators.maxLength(30),]),
        soyad:new FormControl("", [Validators.required, Validators.maxLength(30),]),
        vergiNo: new FormControl("", [Validators.required,Validators.maxLength(15)]),
        firmaAd:new FormControl("", [Validators.required,Validators.maxLength(150)]), 
        kullaniciSifre:new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        kullaniciSifreTekrarla:new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
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
        ,
        //yetkiArry: this._fb.array([])
      },{
        validator: MustMatch('kullaniciSifre', 'kullaniciSifreTekrarla')
    }
    )
  }
 
  isRoleChecked(yetkiAdi: string): boolean {
    // just return default role checked status
    // it's better to use a setting
    return this.defaultRoleCheckedStatus;
  }

  onRoleChange(yetki: YetkiDto, $event) {
    this.checkedRolesMap[yetki.yetkiKodu] = $event.checked;
    
  }

  getCheckedRoles(): string[] {
    const yetkiler: string[] = [];
    
    _.forEach(this.checkedRolesMap, function(value, key) {
      if (value) {
        yetkiler.push(key);
      }
    });
    return yetkiler;
    
  }

  save(){
    this.submitted = true;    
   
    // stop here if form is invalid
    if (this.kullaniciForm.invalid) {
      const invalid = [];
      const controls = this.kullaniciForm.controls;
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
    
    let yeniKullanici=new KullaniciDto();
    yeniKullanici.init(this.kullaniciForm.value);    

    this.kullaniciYetkileri = [] as any;
    for(let role of this.getCheckedRoles())
    { 
      let kullaniciYetki= new KullaniciYetkiDto();
      console.log(role);
      kullaniciYetki.yetkiKodu=role;
      kullaniciYetki.kullaniciKod=yeniKullanici.kullaniciKod;
      kullaniciYetki.aktif=true;
      this.kullaniciYetkileri.push(kullaniciYetki);  
    }
      const promise = this.beyanServis
        .setKullanici(yeniKullanici)
        .toPromise();
      promise.then(
        result => {
         
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);       
         
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.kullaniciForm.disable();
        },
        err => {
         this.beyanServis.errorHandel(err);    
        }
      );

      const promises = this.beyanServis
      .setYetkiKullanici(this.kullaniciYetkileri,yeniKullanici.kullaniciKod)
      .toPromise();
     promises.then(
      result => {
       
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       
       
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      
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
