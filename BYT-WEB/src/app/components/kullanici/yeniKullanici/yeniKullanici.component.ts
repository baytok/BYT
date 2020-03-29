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
  KullaniciDto, MusteriDto,YetkiDto, ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../../shared/service-proxies/service-proxies";
import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
import { MatDialog } from "@angular/material/dialog";
import { MatInput } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";

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
  constructor(
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
        console.log(err);
      });
    
  }
  getAktifYetkiler()
  {
      this.beyanServis.getAllAktifYetkiler()
     .subscribe( (result: YetkiDto[])=>{
           this.yetkiDataSource=result;
           console.log(result);
      }, (err)=>{
        console.log(err);
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
        yetki: this._fb.array([])
      },{
        validator: MustMatch('kullaniciSifre', 'kullaniciSifreTekrarla')
    }
    )
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
  
      const promise = this.beyanServis
        .setKullanici(yeniKullanici)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);       

          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.kullaniciForm.disable();
        },
        err => {
          console.log(err);
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.kullaniciForm.value, null, 4)
    // );
  
  
  }

}
