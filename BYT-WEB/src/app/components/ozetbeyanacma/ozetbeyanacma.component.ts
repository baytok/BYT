import { Component, OnInit } from '@angular/core';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from "@angular/material/list";
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm,
} from "@angular/forms";
import { MustMatch } from "../../../shared/helpers/must-match.validator";
import {
  ulke,
  
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../shared/service-proxies/UserRoles";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import {
  OzetBeyanAcmaDto,
  TasimaSenetDto,
  TasimaSatirDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
import {
  ReferansService
} from "../../../shared/helpers/ReferansService";

  @Component({
    selector: 'app-ozetbeyan',
    templateUrl: './ozetbeyanacma.component.html',
    styleUrls: ['./ozetbeyanacma.component.css']
  })
export class OzetbeyanacmaComponent implements OnInit {
  ozetBeyanInternalNo: string;
  tasimaSenetInternalNo: string;
  ozetBeyanForm: FormGroup;
  tasimaSenetiForm: FormGroup;
  tasimaSatiriForm: FormGroup; 
  _ozetBeyanlar:OzetBeyanAcmaDto[];
  _tasimaSenetleri:TasimaSenetDto[];
  _tasimaSatirlari:TasimaSatirDto[];
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  
  constructor(    
    private referansService:ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router:Router,
    ) 
    {
      (this.ozetBeyanForm = this._fb.group({
        id:[],
        beyanInternalNo: [],
        ozetBeyanInternalNo: [],
    
        islemKapsami: new FormControl("", [
          Validators.required,
           Validators.maxLength(9),
        ]),
        ozetBeyanNo: new FormControl("", [
          Validators.required,
          Validators.maxLength(30),
        ]),
        aciklama: new FormControl("", [
            Validators.maxLength(1500),
        ]),      
        baskaRejim: [false],
        ambar: [false]      
       
      })), 
      (this.tasimaSenetiForm = this._fb.group({
        tasimaSenetiArry: this._fb.array([this.getTasimaSeneti()]),
      })),
      (this.tasimaSatiriForm = this._fb.group({
        tasimaSatiriArry: this._fb.array([this.getTasimaSatiri()]),
      }));
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
    ){
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Özet Beyan Açma Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl('/app/beyanname');
    }

    this.getOzetBeyanlar(this._beyanSession.islemInternalNo);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  get focus() {
    return this.ozetBeyanForm.controls;
  }


  yukleOzetBeyan(){
    this.getOzetBeyanlar(this._beyanSession.islemInternalNo);
  }
  getOzetBeyanlar(islemInternalNo: string) {
    this.beyanServis.getOzetBeyanAcma(islemInternalNo).subscribe(
      (result: OzetBeyanAcmaDto[]) => {
        this._ozetBeyanlar = result;
        this.ozetBeyanForm.disable();
       
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getOzetBeyan(ozetBeyanInternalNo) {
    let siraNo=null;
    for (let i in this._ozetBeyanlar) {
     if (this._ozetBeyanlar[i].ozetBeyanInternalNo === ozetBeyanInternalNo)
       siraNo=i;
    }
   
    this.ozetBeyanForm.setValue({    
      beyanInternalNo: this._ozetBeyanlar[siraNo].beyanInternalNo,
      ozetBeyanInternalNo: this._ozetBeyanlar[siraNo].ozetBeyanInternalNo,
      ozetBeyanNo:  this._ozetBeyanlar[siraNo].ozetBeyanNo,
      islemKapsami: this._ozetBeyanlar[siraNo].islemKapsami,
      baskaRejim: this._ozetBeyanlar[siraNo].baskaRejim==="Evet"?true:false,
      ambar: this._ozetBeyanlar[siraNo].ambar==="Evet"?true:false,
      aciklama: this._ozetBeyanlar[siraNo].aciklama,
      id: this._ozetBeyanlar[siraNo].id,
    });
    this.ozetBeyanInternalNo = ozetBeyanInternalNo;

  
    this.beyanServis.getTasimaSenet(this._beyanSession.islemInternalNo).subscribe(
      (result: TasimaSenetDto[]) => {
       
        this._tasimaSenetleri = result.filter(
          (x) =>
            x.ozetBeyanInternalNo === this.ozetBeyanInternalNo
        );
        this.initTasimaSenetiFormArray(this._tasimaSenetleri);
        this.tasimaSenetiForm.disable();

        //Tasima Satir çağır   this.beyanServis.restoreTasimaSatir
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );


    this.ozetBeyanForm.disable();
  }

  yeniOzetBeyan(){
    this.ozetBeyanInternalNo = "Boş";
    this.tasimaSenetInternalNo = "Boş";
    this.ozetBeyanForm.reset();
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
    this.tasimaSenetiForm.reset();
    this.tasimaSenetiForm.enable();
    this.tasimaSenetiForm.markAllAsTouched();
    this.tasimaSatiriForm.reset();
    this.tasimaSatiriForm.enable();
    this.tasimaSatiriForm.markAllAsTouched();
  }
  duzeltOzetBeyan(){
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
    this.tasimaSenetiForm.enable();
    this.tasimaSenetiForm.markAllAsTouched();
    this.tasimaSatiriForm.enable();
    this.tasimaSatiriForm.markAllAsTouched();
  }
  silOzetBeyan(ozetBeyanInternalNo){
    if (
      confirm(ozetBeyanInternalNo + "- Özet Beyanı Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeOzetBeyanAcma(ozetBeyanInternalNo, this._beyanSession.beyanInternalNo)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.ozetBeyanForm.disable();
          this.ozetBeyanForm.reset();
          this.tasimaSatiriForm.disable();
          this.tasimaSatiriForm.reset();
          this.tasimaSenetiForm.disable();
          this.tasimaSenetiForm.reset();
          this.yukleOzetBeyan();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
    
  }

  onOzetBeyanFormSubmit() {
    this.submitted = true;

    if (this.ozetBeyanForm.invalid) {
      const invalid = [];
      const controls = this.ozetBeyanForm.controls;
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

    let _ambar =this.ozetBeyanForm.get("ambar").value ===true ?"Evet":"Hayir";
    let _baskaRejim =this.ozetBeyanForm.get("baskaRejim").value ===true ?"Evet":"Hayir";  
    let ID=this.ozetBeyanForm.get("id").value;
   
    this.ozetBeyanForm.setValue({
      beyanInternalNo:this._beyanSession.beyanInternalNo,
      ozetBeyanInternalNo:this.ozetBeyanInternalNo,
      ozetBeyanNo: this.ozetBeyanForm.get("ozetBeyanNo").value!=null?this.ozetBeyanForm.get("ozetBeyanNo").value:"",
      islemKapsami: this.ozetBeyanForm.get("islemKapsami").value!=null?this.ozetBeyanForm.get("islemKapsami").value:"",
      aciklama: this.ozetBeyanForm.get("aciklama").value!=null?this.ozetBeyanForm.get("aciklama").value:"",
      baskaRejim:_baskaRejim,
      ambar:_ambar,
      id:ID!=null?ID:0,
      })
   

    
    let yeniOzetBeyanInternalNo: string;
    let yeniOzetBeyanAcma= new OzetBeyanAcmaDto();
    yeniOzetBeyanAcma.init(this.ozetBeyanForm.value);
   
    const promiseKalem = this.beyanServis.restoreOzetBeyanAcma(yeniOzetBeyanAcma).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kiymetServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yeniOzetBeyanInternalNo = kiymetServisSonuc.ReferansNo;
      
        if (yeniOzetBeyanInternalNo != null) {
          this.ozetBeyanInternalNo = yeniOzetBeyanInternalNo;
         this.setTasimaSeneti();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.ozetBeyanForm.disable();
          this.yukleOzetBeyan();
        }
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }

  initTasimaSenetiFormArray(tasimaSenet: TasimaSenetDto[]) {
    const formArray = this.tasimaSenetiForm.get("tasimaSenetiArry") as FormArray;
    formArray.clear();
    for (let klm of tasimaSenet) {
      let formGroup: FormGroup = new FormGroup({
        tasimaSenediNo: new FormControl(klm.tasimaSenediNo, [
          Validators.required,
          Validators.maxLength(50),
        ]),     
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
        tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
      });

      formArray.push(formGroup);
    }
    this.tasimaSenetiForm.setControl("tasimaSenetiArry", formArray);
  
  }
  getTasimaSenet(tasimaSenetInternalNo) {
   
    this.tasimaSenetInternalNo = tasimaSenetInternalNo;
  
    this.beyanServis.getTasimaSatir(this._beyanSession.islemInternalNo).subscribe(
      (result: TasimaSatirDto[]) => {
       
        this._tasimaSatirlari = result.filter(
          (x) =>
            x.tasimaSenetInternalNo === this.tasimaSenetInternalNo
        );
        this.initTasimaSatiriFormArray(this._tasimaSatirlari);
        this.tasimaSenetiForm.disable();

      
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    this.tasimaSenetiForm.disable();
  }
  getTasimaSeneti() {
    return this._fb.group({     
      tasimaSenediNo: new FormControl("", [Validators.required,
        Validators.maxLength(50),]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      ozetBeyanInternalNo: new FormControl(this.ozetBeyanInternalNo, [
        Validators.required,
      ]),
      tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
        Validators.required,
      ]),
    });
  }

  get tasimaSenetiBilgileri() {
    return this.tasimaSenetiForm.get("tasimaSenetiArry") as FormArray;
  }

  addTasimaSenetiField() {
    this.tasimaSenetiBilgileri.push(this.getTasimaSeneti());
  }

  deleteTasimaSenetiField(index: number) {
    this.tasimaSenetiBilgileri.removeAt(index);
  }

  setTasimaSeneti() {
    if (this.tasimaSenetiBilgileri.length > 0) {
      for (let klm of this.tasimaSenetiBilgileri.value) {
        klm.ozetBeyanInternalNo = this.ozetBeyanInternalNo;
        klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
      }
      this.initTasimaSenetiFormArray(this.tasimaSenetiBilgileri.value);

      if (this.tasimaSenetiBilgileri.invalid) {
        const invalid = [];
        const controls = this.tasimaSenetiBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Taşıma Seneti Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.tasimaSenetiBilgileri.length >= 0) {
      const promiseMarka = this.beyanServis
        .restoreTasimaSenet(
          this.tasimaSenetiBilgileri.value,
          this.ozetBeyanInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseMarka.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
           this.tasimaSenetiForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }

  initTasimaSatiriFormArray(tasimaSatir: TasimaSatirDto[]) {
    const formArray = this.tasimaSatiriForm.get("tasimaSatiriArry") as FormArray;
    formArray.clear();
    for (let klm of tasimaSatir) {
      let formGroup: FormGroup = new FormGroup({
        ambarKodu: new FormControl(klm.ambarKodu, [
          Validators.maxLength(9),
        ]),    
        tasimaSatirNo: new FormControl(klm.tasimaSatirNo, [
          Validators.required,
          Validators.maxLength(9),
        ]),   
        miktar: new FormControl(klm.miktar, [
          Validators.required,
          ValidationService.decimalValidation
        ]),     
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
        tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
      });

      formArray.push(formGroup);
    }
    this.tasimaSatiriForm.setControl("tasimaSatiriArry", formArray);
  }

  getTasimaSatiri() {
    return this._fb.group({     
      ambarKodu: new FormControl("", [
        Validators.maxLength(9),
      ]),    
      tasimaSatirNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),   
      miktar: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation
      ]),     
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      ozetBeyanInternalNo: new FormControl(this.ozetBeyanInternalNo, [
        Validators.required,
      ]),
      tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
        Validators.required,
      ]),
    });
  }

  get tasimaSatiriBilgileri() {
    return this.tasimaSatiriForm.get("tasimaSatiriForm") as FormArray;
  }

  addTasimaSatiriField() {
    this.tasimaSenetiBilgileri.push(this.getTasimaSeneti());
  }

  deleteTasimaSatiriField(index: number) {
    this.tasimaSenetiBilgileri.removeAt(index);
  }

  setTasimaSatiri() {
    if (this.tasimaSatiriBilgileri.length > 0) {
      for (let klm of this.tasimaSatiriBilgileri.value) {
        klm.ozetBeyanInternalNo = this.ozetBeyanInternalNo;
        klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
        klm.miktar =
          typeof klm.miktar == "string"
            ? parseInt(klm.miktar)
            : klm.miktar;

      
      }
      this.initTasimaSatiriFormArray(this.tasimaSatiriBilgileri.value);

      if (this.tasimaSatiriBilgileri.invalid) {
        const invalid = [];
        const controls = this.tasimaSatiriBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Taşıma Satırı Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.tasimaSatiriBilgileri.length >= 0) {
      const promiseMarka = this.beyanServis
        .restoreTasimaSatir(
          this.tasimaSatiriBilgileri.value,
          this.ozetBeyanInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseMarka.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
           this.tasimaSenetiForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }

  onReset() {
    this.submitted = false;
  }
}
