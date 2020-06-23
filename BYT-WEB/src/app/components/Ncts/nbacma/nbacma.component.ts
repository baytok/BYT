import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  Injectable,
} from "@angular/core";
import {
  MatSelectionList,
  MatSelectionListChange,
} from "@angular/material/list";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
} from "@angular/forms";


import {
  BeyannameServiceProxy,
  SessionServiceProxy,
 
} from "../../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import { ReferansService } from "../../../../shared/helpers/ReferansService";
import {
  NbObAcmaDto,
  NbAbAcmaDto,
  ServisDto,
} from "../../../../shared/service-proxies/service-proxies";


@Injectable() 
@Component({
  selector: "app-nbacma",
  templateUrl: "./nbacma.component.html",
  styleUrls: ["./nbacma.component.css"],

})
export class NbAcmaComponent implements OnInit {
  public form: FormGroup;
  acmaForm: FormGroup; 
  obAcmaForm: FormGroup;
  abAcmaForm: FormGroup;
 
  closeResult: string;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  nctsBeyanInternalNo = this._beyanSession.nctsBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;

   _ulkeList = this.referansService.getUlkeDilJSON();
  _dilList = this.referansService.getDilJSON();
  _dovizList = this.referansService.getTrDovizCinsiJSON(); 
  _odemeList = this.referansService.getNctsOdemeJSON();
  _teslimSekliList= this.referansService.getNctsTeslimSekliJSON();
  _isleminNiteligiList= this.referansService.getNctsisleminNiteligiJSON();
 
  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router,
   
  ) {
    (this.acmaForm = this._fb.group({
      nctsBeyanInternalNo: [],
      
    })),      
      
       (this.obAcmaForm = this._fb.group({
        obArry: this._fb.array([this.getObAcma()]),
      })),
      (this.abAcmaForm = this._fb.group({
        abArry: this._fb.array([this.getAbAcma()]),
      }))
     
      
        
  }
  get focus() {
    return this.acmaForm.controls;
  }

  ngOnInit() {
    if (!this._userRoles.canBeyannameRoles()) {
      this.openSnackBar(
        "Beyanname Sayfasını Görmeye Yetkiniz Yoktur.",
        "Tamam"
      );
      this.beyanServis.notAuthorizeRole();
    }
    if (
      this._beyanSession.islemInternalNo == undefined ||
      this._beyanSession.islemInternalNo == null
    ) {
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kalem Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl("/app/nctsbeyan");
    }
    this.getBilgiler();

  }

  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if (  this.beyanStatu === "Olusturuldu" || this.beyanStatu === "Güncellendi")
    return true;
    else return false;
  }
  get BeyanSilDuzeltStatu():boolean {
  
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if ( this.beyanStatu === "Olusturuldu" || this.beyanStatu === "Güncellendi")
      return true;
     else
       return false;
   
  }



  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getBilgiler() {
   
    this.acmaForm.setValue({
      nctsBeyanInternalNo: this.nctsBeyanInternalNo     

      
    });

   
    this.beyanServis.getNbObAcama(this._beyanSession.islemInternalNo).subscribe(
      (result: NbObAcmaDto[]) => {
     
        this.initobAcmaFormArray(result);
        this.obAcmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbAbAcama(this._beyanSession.islemInternalNo).subscribe(
      (result: NbAbAcmaDto[]) => {
     
        this.initabAcmaFormArray(result);
        this.abAcmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );   
    this.acmaForm.disable();
  }
 

  getAcmaIslemleri(){
    // this.acmaForm.reset();     
    // this.obAcmaForm.reset();
    // this.abAcmaForm.reset();  
    // const formKapArray = this.obAcmaForm.get("kapArry") as FormArray;
    // formKapArray.clear();
    // this.obAcmaForm.setControl("kapArry", formKapArray);
    // const formEkBilgiArray = this.abAcmaForm.get("ekBilgiArry") as FormArray;
    // formEkBilgiArray.clear();
    // this.abAcmaForm.setControl("ekBilgiArry", formEkBilgiArray);

    this.acmaForm.enable();
    this.obAcmaForm.enable();
    this.abAcmaForm.enable();
    
    this.acmaForm.markAllAsTouched();
    this.obAcmaForm.markAllAsTouched();
    this.abAcmaForm.markAllAsTouched();
    
   
   
    if (this.acmaForm.invalid) {
      const invalid = [];
      const controls = this.acmaForm.controls;
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
  onacmaFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.acmaForm.invalid) {
      const invalid = [];
      const controls = this.acmaForm.controls;
      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      alert(
        "ERROR!! :-)\n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:" +
          JSON.stringify(invalid, null, 4)
      );
      return;
    }
  
    this.acmaForm
      .get("nctsBeyanInternalNo")
      .setValue(this._beyanSession.nctsBeyanInternalNo);    
    
          this.setObAcma();
          this.setAbAcma();        
          this.obAcmaForm.reset();     
          this.abAcmaForm.reset();      
         this.obAcmaForm.disable();
         this.abAcmaForm.disable();
         this.getBilgiler();
      
  }
 

//#region Ob Açma

initobAcmaFormArray(ozet: NbObAcmaDto[]) {
  const formArray = this.obAcmaForm.get("obArry") as FormArray;
  formArray.clear();
  for (let klm of ozet) {
    let formGroup: FormGroup = new FormGroup({
      
      islemKapsami: new FormControl(klm.islemKapsami, [Validators.required, Validators.maxLength(9)]),
      ambarIci: new FormControl(klm.ambarIci, [Validators.required,Validators.maxLength(9)]),
      ambarKodu: new FormControl(klm.ambarKodu, [Validators.maxLength(9)]),
      tasimaSenetNo: new FormControl(klm.tasimaSenetNo, [Validators.required, Validators.maxLength(20)]),
      ozetBeyanNo: new FormControl(klm.ozetBeyanNo, [Validators.required, Validators.maxLength(20)]),
      tasimaSatirNo: new FormControl(klm.tasimaSatirNo, [ValidationService.numberValidator]),
      miktar: new FormControl(klm.miktar, [Validators.required,ValidationService.decimalValidation]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
     
    });

    formArray.push(formGroup);
  }
  this.obAcmaForm.setControl("obArry", formArray);
}

getObAcma() {
  return this._fb.group({
 
     islemKapsami: new FormControl("", [Validators.required, Validators.maxLength(9)]),
      ambarIci: new FormControl("", [Validators.required,Validators.maxLength(9)]),
      ambarKodu: new FormControl("", [Validators.maxLength(9)]),
      tasimaSenetNo: new FormControl("", [Validators.required, Validators.maxLength(20)]),
      ozetBeyanNo: new FormControl("", [Validators.required, Validators.maxLength(20)]),
      tasimaSatirNo: new FormControl(0, [ValidationService.numberValidator]),
      miktar: new FormControl(0, [Validators.required,ValidationService.decimalValidation]),
      nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
   
  });
}

get obAcmaBilgileri() {
  return this.obAcmaForm.get("obArry") as FormArray;
}

addObAcmaField() {
  this.obAcmaBilgileri.push(this.getObAcma());
}

deleteObAcmaField(index: number) {
  this.obAcmaBilgileri.removeAt(index);
}

setObAcma() {
  if (this.obAcmaBilgileri.length > 0) {
    for (let klm of this.obAcmaBilgileri.value) {
 
      klm.miktar =
      typeof klm.miktar == "string"
        ? parseInt(klm.miktar)
        : klm.miktar;

      klm.tasimaSatirNo =
      typeof klm.tasimaSatirNo == "string"
        ? parseFloat(klm.tasimaSatirNo)
        : klm.tasimaSatirNo;
    }

    this.initobAcmaFormArray(this.obAcmaBilgileri.value);

    if (this.obAcmaBilgileri.invalid) {
      const invalid = [];
      const controls = this.obAcmaBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Özet Beyan Açma Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.obAcmaBilgileri.length >= 0) {
    const promiseOzet = this.beyanServis
      .restoreNbObAcma(
        this.obAcmaBilgileri.value,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseOzet.then(
      (result) => {
        const servisSonuc = new ServisDto();
         servisSonuc.init(result);
         this.openSnackBar(servisSonuc.Sonuc, "Tamam");   
        
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion ObAçma

//#region AbAçma

initabAcmaFormArray(antrepo: NbAbAcmaDto[]) {
  const formArray = this.abAcmaForm.get("abArry") as FormArray;
  formArray.clear();
  for (let klm of antrepo) {
    let formGroup: FormGroup = new FormGroup({
      
      beyannameNo: new FormControl(klm.beyannameNo, [Validators.required, Validators.maxLength(20)]),
      aciklama: new FormControl(klm.aciklama, [Validators.maxLength(200)]),
      kalemNo: new FormControl(klm.kalemNo, [Validators.required,ValidationService.numberValidator]),
      acilanKalemNo: new FormControl(klm.acilanKalemNo, [Validators.required,ValidationService.numberValidator]),
      miktar: new FormControl(klm.miktar, [Validators.required,ValidationService.decimalValidation]),
      teslimSekli: new FormControl(klm.teslimSekli, [Validators.maxLength(9)]),
      dovizCinsi: new FormControl(klm.dovizCinsi, [Validators.maxLength(9)]),
      kiymet: new FormControl(klm.kiymet, [ValidationService.decimalValidation]),
      odemeSekli: new FormControl(klm.odemeSekli, [Validators.maxLength(9)]),
      isleminNiteligi: new FormControl(klm.isleminNiteligi, [Validators.maxLength(9)]),
      ticaretUlkesi: new FormControl(klm.ticaretUlkesi, [Validators.maxLength(9)]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
    
    });

    formArray.push(formGroup);
  }
  this.abAcmaForm.setControl("abArry", formArray);
}

getAbAcma() {
  return this._fb.group({
 
    beyannameNo: new FormControl("", [Validators.required, Validators.maxLength(20)]),
    aciklama: new FormControl("", [Validators.maxLength(200)]),
    kalemNo: new FormControl(0, [Validators.required,ValidationService.numberValidator]),
    acilanKalemNo: new FormControl(0, [Validators.required,ValidationService.numberValidator]),
    miktar: new FormControl(0, [Validators.required,ValidationService.decimalValidation]),
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
    teslimSekli: new FormControl("", [Validators.maxLength(9)]),
    dovizCinsi: new FormControl("", [Validators.maxLength(9)]),
    kiymet: new FormControl(0, [ValidationService.decimalValidation]),
    odemeSekli: new FormControl("", [Validators.maxLength(9)]),
    isleminNiteligi: new FormControl("", [Validators.maxLength(9)]),
    ticaretUlkesi: new FormControl("", [Validators.maxLength(9)]),
  
  });
}

get abAcmaBilgileri() {
  return this.abAcmaForm.get("abArry") as FormArray;
}

addabAcmaField() {
  this.abAcmaBilgileri.push(this.getAbAcma());
}

deletebAcmaField(index: number) {
  this.abAcmaBilgileri.removeAt(index);
}

setAbAcma() {
  if (this.abAcmaBilgileri.length > 0) {
    for (let klm of this.abAcmaBilgileri.value) {
  
      klm.miktar =
      typeof klm.miktar == "string"
        ? parseInt(klm.miktar)
        : klm.miktar;

        klm.kalemNo =
        typeof klm.kalemNo == "string"
          ? parseInt(klm.kalemNo)
          : klm.kalemNo;

          klm.acilanKalemNo =
          typeof klm.acilanKalemNo == "string"
            ? parseInt(klm.acilanKalemNo)
            : klm.acilanKalemNo;

            klm.kiymet =
            typeof klm.kiymet == "string"
              ? parseInt(klm.kiymet)
              : klm.kiymet;
            
     
    }

    this.initabAcmaFormArray(this.abAcmaBilgileri.value);

    if (this.abAcmaBilgileri.invalid) {
      const invalid = [];
      const controls = this.abAcmaBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Beyanname Açma Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.abAcmaBilgileri.length >= 0) {
    const promisebeyanname = this.beyanServis
      .restoreNbAbAcma(
        this.abAcmaBilgileri.value,
       this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promisebeyanname.then(
      (result) => {
         const servisSonuc = new ServisDto();
         servisSonuc.init(result);
         this.openSnackBar(servisSonuc.Sonuc, "Tamam");   
     
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion AbAçma


  onReset() {
    this.submitted = false;
  }
  
}
