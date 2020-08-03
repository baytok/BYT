import { Component, OnInit, Inject, Injector,ViewChild,ElementRef,Injectable } from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray
} from "@angular/forms";
import {
  ReferansService
} from "../../../../shared/helpers/ReferansService";

import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { GirisService } from '../../../../account/giris/giris.service';

import { MatSnackBar } from "@angular/material/snack-bar";
import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
import {
  ValidationService
} from "../../../../shared/service-proxies/ValidationService";
import {
  IghbDto,
  IghbListeDto,
  ServisDto
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";
import {
 rejim
} from "../../../../shared/helpers/referencesList";


@Component({
  selector: 'app-ighb',
  templateUrl: './ighb.component.html',
  styleUrls: ['./ighb.component.css'],
  
})
export class IghbComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  ighbForm: FormGroup;
  tcgbForm: FormGroup;
  submitted: boolean = false;  
  ighbInternalNo:string;
  beyanStatu:string;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _ighb: IghbDto = new IghbDto();
  _gumrukList =this.referansService.getGumrukJSON();
  
  constructor(
    private referansService:ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private _userRoles:UserRoles,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _fb: FormBuilder,
    private  girisService: GirisService,  
  
  ) {
    
   
  }
  get focus() {
    return this.ighbForm.controls;
  }
  buildForm(): void {
    this.ighbForm = this.formBuilder.group(
      {
       
        ighbInternalNo:[],        
        refNo: new FormControl("", [Validators.required, Validators.maxLength(30)]),
        izinliGondericiVergiNo: new FormControl("",[Validators.required,Validators.maxLength(15)]),
        plakaBilgisi: new FormControl("",[Validators.required,Validators.maxLength(100)]),
        tesisKodu: new FormControl("",[Validators.required,Validators.maxLength(30)]),
        gumrukKodu:new FormControl("", [Validators.required,Validators.maxLength(9)]),
        kullaniciKodu: new FormControl(""),
	      tescilStatu: [],
        tescilTarihi:[],     
      },
      (this.tcgbForm = this._fb.group({
        tcgbArry: this._fb.array([this.getTcgb()]),
      })),
    )
  }
 
  ngOnInit() {
  
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Beyanname Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.ighbForm.disable();

   
    if (this._beyanSession.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value=this._beyanSession.islemInternalNo;
      this.getIghbFromIslem(this._beyanSession.islemInternalNo);
     
    }
  
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  getIghbFromIslem(islemInternalNo:string) {  
  
    this.beyanServis.getIghb(islemInternalNo).subscribe(
      result => {
        this._ighb = new IghbDto();
        this._ighb.init(result);
        if (this._ighb == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.ighbInternalNo="";
          this.beyanStatu= "" ;
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.ighbInternalNo= "" ;
          this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
         
          this._beyanSession.islemInternalNo = islemInternalNo;    
          this._beyanSession.ighbInternalNo= this._ighb.ighbInternalNo ;
          this._beyanSession.beyanStatu= this._ighb.tescilStatu ;
          this.ighbInternalNo=this._ighb.ighbInternalNo;
          this.beyanStatu= this._ighb.tescilStatu ;
          this.loadighbForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getIghb(islemInternalNo) {  
   
     this.beyanServis.getIghb(islemInternalNo.value).subscribe(
      result => {       
        this._ighb = new IghbDto();
      
        this._ighb.init(result);
    
        if (this._ighb == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.ighbInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.ighbInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;         
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = islemInternalNo.value;    
          this._beyanSession.ighbInternalNo= this._ighb.ighbInternalNo ;
          this._beyanSession.beyanStatu= this._ighb.tescilStatu ;
          this.ighbInternalNo=this._ighb.ighbInternalNo;
          this.beyanStatu= this._ighb.tescilStatu ;
          this.loadighbForm();        
        
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadighbForm()
    {
      this.ighbForm.reset();
      this.tcgbForm.reset();
      this._beyanSession.ighbInternalNo= this._ighb.ighbInternalNo;
       this.ighbForm.setValue({      
        ighbInternalNo: this._ighb.ighbInternalNo,
	      gumrukKodu:this._ighb.gumrukKodu,
        kullaniciKodu:this._ighb.kullaniciKodu,
        refNo:this._ighb.refNo,
        izinliGondericiVergiNo:this._ighb.izinliGondericiVergiNo,
        plakaBilgisi:this._ighb.plakaBilgisi,
        tesisKodu:this._ighb.tesisKodu,
        tescilStatu: this._ighb.tescilStatu,
        tescilTarihi:this._ighb.tescilTarihi,  
        
     
       });
  
      
       this.beyanServis.getIghbListe(this._beyanSession.islemInternalNo).subscribe(
        (result: IghbListeDto[]) => {
          this.inittcgbFormArray(result);
          this.tcgbForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.ighbForm.disable();
   
  }
  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if (this.beyanStatu === 'Olusturuldu' || this.beyanStatu === 'Güncellendi')
     return true;
    else return false;
  }

  yeniIghbBasvuru() {
    this.ighbInternalNo='Boş';
    this.beyanStatu='';
    this.ighbForm.reset();
    this.ighbForm.enable();
    this.islemInput.nativeElement.value="";
    this.ighbForm.markAllAsTouched();
    this.submitted = false;   
    this.tcgbForm.reset();
    this.tcgbForm.enable();

    const formTcgbArray = this.tcgbForm.get("tcgbArry") as FormArray;
    formTcgbArray.clear();
    this.tcgbForm.setControl("tcgbArry", formTcgbArray);

  } 
  duzeltIghbBasvuru() {
   
    this.ighbForm.enable();
    this.tcgbForm.enable();
    this.ighbForm.markAllAsTouched();
    this.tcgbForm.markAllAsTouched();
   
    if (this.ighbForm.invalid) {
      const invalid = [];
      const controls = this.ighbForm.controls;
      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }
      
      alert(
        "ERROR!! \n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:"  + JSON.stringify(invalid, null, 4)
      );
     
    }
  }
  silIghbBasvuru(islemInternalNo){
    if (
      confirm(islemInternalNo.value+ " Başvuruyu Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeIghb(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.ighbForm.reset();
          this._beyanSession.islemInternalNo="";
          this._beyanSession.ighbInternalNo="";
          this.ighbInternalNo="";
          this._beyanSession.beyanStatu="";
          this.beyanStatu="";
          this.islemInput.nativeElement.value="";
          islemInternalNo.value="";
          this.ighbForm.disable();
          this.tcgbForm.disable();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  onighbFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.ighbForm.invalid) {
      const invalid = [];
      const controls = this.ighbForm.controls;
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
   
    this.ighbForm.get("ighbInternalNo").setValue(this.ighbInternalNo);    
    this.ighbForm.get("kullaniciKodu").setValue(this.girisService.loggedKullanici);
    
  
    let yeniislemInternalNo: string;
    let yeniIghb=new IghbDto();
    yeniIghb.init(this.ighbForm.value);
    
      const promise = this.beyanServis
        .restoreIghb(yeniIghb,this.ighbInternalNo)
        .toPromise();
      promise.then(
        result => {
        
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;
          
           
          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this._ighb.ighbInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this.setTcgb();   
            this.getIghbFromIslem(yeniislemInternalNo);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");         
          
          }
            this.ighbForm.disable();
            this.tcgbForm.disable();
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.ighbForm.value, null, 4)
    // );

  
  }

   //#region Tcgb

   inittcgbFormArray(tcgb: IghbListeDto[]) {
    const formArray = this.tcgbForm.get("tcgbArry") as FormArray;
    formArray.clear();
    for (let klm of tcgb) {
       let formGroup: FormGroup = new FormGroup({
       tcgbNumarasi: new FormControl(klm.tcgbNumarasi, [
          Validators.required,Validators.maxLength(20)
       ]),
       
        ighbInternalNo: new FormControl(klm.ighbInternalNo),
      
      });

      formArray.push(formGroup);
    }
    this.tcgbForm.setControl("tcgbArry", formArray);
  }

  getTcgb() {
    return this._fb.group({
      tcgbNumarasi: new FormControl("", [Validators.required,Validators.maxLength(20),]),
     
      ighbInternalNo: new FormControl(this.ighbInternalNo, [
        Validators.required,
      ]),
    
    });
  }

  get tcgbBilgileri() {
    return this.tcgbForm.get("tcgbArry") as FormArray;
  }

  addTcgbField() {
    this.tcgbBilgileri.push(this.getTcgb());
  }

  deleteTcgbField(index: number) {
    this.tcgbBilgileri.removeAt(index);
  }

  setTcgb() {
    if (this.tcgbBilgileri.length > 0) {
      for (let klm of this.tcgbBilgileri.value) {
        klm.ighbInternalNo =  this._ighb.ighbInternalNo;
       
      }

      this.inittcgbFormArray(this.tcgbBilgileri.value);

      if (this.tcgbBilgileri.invalid) {
        const invalid = [];
        const controls = this.tcgbBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Tcgb verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );

          return;
        }
      }
    }
 
  
    if (this.tcgbBilgileri.length >= 0) {
      const promise = this.beyanServis
        .restoreIghbListe(
          this.tcgbBilgileri.value,
          this.ighbInternalNo
        )
        .toPromise();
      promise.then(
        (result) => {

         // this.tcgbBilgileri.reset();
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
          // this.tcgbForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }
  //#endregion 
  onCancel() {
    this.submitted = false;
    this.ighbForm.disable();
  }
  
  
}
