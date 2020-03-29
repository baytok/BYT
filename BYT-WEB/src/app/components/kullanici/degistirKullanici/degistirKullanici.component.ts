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
  KullaniciDto,MusteriDto,YetkiDto,ServisDto
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
  kullaniciKod: string;
  ad:string;
  soyad:string;
  vergiNo:string;
  firmaAd:string;
  aktif:boolean;
  telefon:string;
  mailAdres:string;
  kullaniciSifre:string;
  kullaniciSifreTekrarla:string;
}
@Component({
  selector: 'app-degistirKullanici',
  templateUrl: './degistirKullanici.component.html',
  styleUrls: ['./degistirKullanici.component.css']
})

export class DegistirKullaniciComponent implements OnInit {
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
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) { 
    this.kullaniciForm = this._fb.group(
      {    
        id:[], 
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
      },{
        validator: MustMatch('kullaniciSifre', 'kullaniciSifreTekrarla')
    }
    )

  }

  ngOnInit() {
 
    this.loadKullaniciForm();
   this.getAktifMusteriler();
   this.getAktifYetkiler();
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
           
      }, (err)=>{
        console.log(err);
      });
    
  }
  loadKullaniciForm(){
    this.kullaniciForm.setValue({
      id: this.data.id,
      kullaniciKod:this.data.kullaniciKod,
      ad:this.data.ad,
      soyad:this.data.soyad,
      aktif:this.data.aktif,
      firmaAd:this.data.firmaAd,
      vergiNo:this.data.vergiNo,
      telefon:this.data.telefon,
      mailAdres:this.data.mailAdres,
      kullaniciSifre:this.data.kullaniciSifre,
      kullaniciSifreTekrarla:""
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
    
    let Kullanici=new KullaniciDto();
    Kullanici.init(this.kullaniciForm.value);
  
      const promise = this.beyanServis
        .restoreKullanici(Kullanici)
        .toPromise();
      promise.then(
        result => {
        
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
