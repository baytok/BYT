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
  ozetBeyanForm: FormGroup;
  _ozetBeyanlar:OzetBeyanAcmaDto[];

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
        ambar: [false],     
       
       
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

    this.getozetBeyanlar(this._beyanSession.islemInternalNo);
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
    this.getozetBeyanlar(this._beyanSession.islemInternalNo);
  }
  getozetBeyanlar(islemInternalNo: string) {
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
    
    });
    this.ozetBeyanInternalNo = ozetBeyanInternalNo;

    //Kıymet kalemi için 
    // this.beyanServis.getOdeme(this._beyanSession.islemInternalNo).subscribe(
    //   (result: OdemeDto[]) => {
    //     this._odemeler = result.filter(
    //       (x) =>
    //         x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
    //     );
    //     this.initOdemeFormArray(this._odemeler);
    //     this.odemeForm.disable();
    //   },
    //   (err) => {
    //     this.beyanServis.errorHandel(err);
    //   }
    // );


    this.ozetBeyanForm.disable();
  }

  yeniOzetBeyan(){
    this.ozetBeyanInternalNo = "Boş";
    this.ozetBeyanForm.reset();
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
  }
  duzeltOzetBeyan(){
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
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
    this.ozetBeyanForm.setValue({ 

      beyanInternalNo:this._beyanSession.beyanInternalNo,
      ozetBeyanInternalNo:this.ozetBeyanInternalNo,
      ozetBeyanNo: this.ozetBeyanForm.get("ozetBeyanNo").value!=null?this.ozetBeyanForm.get("ozetBeyanNo").value:"",
      islemKapsami: this.ozetBeyanForm.get("islemKapsami").value!=null?this.ozetBeyanForm.get("islemKapsami").value:"",
      aciklama: this.ozetBeyanForm.get("aciklama").value!=null?this.ozetBeyanForm.get("aciklama").value:"",
      baskaRejim:_baskaRejim,
      ambar:_ambar,
      })
   

 
    let yeniOzetBeyanInternalNo: string;
    let yeniOzetBeyanAcma= new OzetBeyanAcmaDto();
    yeniOzetBeyanAcma.init(this.ozetBeyanForm.value);
    console.log(yeniOzetBeyanAcma);
    const promiseKalem = this.beyanServis.restoreOzetBeyanAcma(yeniOzetBeyanAcma).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kiymetServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yeniOzetBeyanInternalNo = kiymetServisSonuc.ReferansNo;
      
        if (yeniOzetBeyanInternalNo != null) {
          this.ozetBeyanInternalNo = yeniOzetBeyanInternalNo;
         
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
  onReset() {
    this.submitted = false;
  }
}
