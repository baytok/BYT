
import { Component, OnInit } from '@angular/core';

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
 
} from "@angular/forms";

import {
  teminat,
  
} from "../../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";

import {
  ObTeminatDto,
  ServisDto,
} from "../../../../shared/service-proxies/service-proxies";
@Component({
  selector: 'app-obteminat',
  templateUrl: './obteminat.component.html',
  styleUrls: ['./obteminat.component.css']
})
export class ObTeminatComponent implements OnInit {

  teminatForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  ozetBeyanInternalNo = this._beyanSession.ozetBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  _teminatList = teminat;
  constructor(  private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,    
    private router:Router,
    ) 
    {
      (this.teminatForm = this._fb.group({
        teminatArry: this._fb.array([this.getTeminat()]),
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
          this._beyanSession.islemInternalNo + " ait Teminat Bulunamadı",
          "Tamam"
        );
        this.router.navigateByUrl('/app/ozetbeyan');
      }
        this.getTeminatBilgileri();
       
    }

    openSnackBar(message: string, action: string) {
      this.snackBar.open(message, action, {
        duration: 2000,
      });
    }
    get BeyanStatu():boolean {
    
      if(this.beyanStatu==='undefined' || this.beyanStatu===null)
      return false;
      if (this.beyanStatu === 'Olusturuldu' || this.beyanStatu === 'Güncellendi')
       return true;
      else return false;
    }
 islemTeminat(){
  this.teminatForm.enable();
  this.teminatForm.markAllAsTouched();
 }
  yeniTeminat(){
    
    this.teminatForm.reset();   
    const formTeminatArray = this.teminatForm.get("teminatArry") as FormArray;
    formTeminatArray.clear();
    this.teminatForm.setControl("teminatArry", formTeminatArray);
    this.teminatForm.markAllAsTouched();

  }

  onTeminatFormSubmit(){
   
    if (this.teminatBilgileri.length > 0) {
      for (let klm of this.teminatBilgileri.value) {
       
        klm.teminatOrani = typeof(klm.teminatOrani)=="string" ? parseFloat(klm.teminatOrani) : klm.teminatOrani;
        klm.nakdiTeminatTutari = typeof(klm.nakdiTeminatTutari)=="string" ? parseFloat(klm.nakdiTeminatTutari) : klm.nakdiTeminatTutari;
        klm.bankaMektubuTutari = typeof(klm.bankaMektubuTutari)=="string" ? parseFloat(klm.bankaMektubuTutari) : klm.bankaMektubuTutari;
        klm.digerTutar = typeof(klm.digerTutar)=="string" ? parseFloat(klm.digerTutar) :  klm.digerTutar; 
       
      }

      this.initTeminatFormArray(this.teminatBilgileri.value);

      if (this.teminatBilgileri.invalid) {
        const invalid = [];
        const controls = this.teminatBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Ödeme Şekli Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
         
          return;
        }
      }
    }
  
    if (this.teminatBilgileri.length >= 0) {
      const promiseOdeme = this.beyanServis
        .restoreObTeminat(
          this.teminatBilgileri.value,
          this._beyanSession.ozetBeyanInternalNo
        )
        .toPromise();
      promiseOdeme.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.teminatForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }
  initTeminatFormArray(teminat: ObTeminatDto[]) {
    const formArray = this.teminatForm.get("teminatArry") as FormArray;
    formArray.clear();
    for (let klm of teminat) {
      let formGroup: FormGroup = new FormGroup({
        teminatSekli: new FormControl(klm.teminatSekli, [
          Validators.required,
        ]),
        teminatOrani: new FormControl(klm.teminatOrani, [
          Validators.required,
          Validators.maxLength(10),
          ValidationService.decimalValidation,
        ]),
        bankaMektubuTutari: new FormControl(klm.bankaMektubuTutari, [
          Validators.maxLength(10),
          ValidationService.decimalValidation,
        ]),
        nakdiTeminatTutari: new FormControl(klm.nakdiTeminatTutari, [
          Validators.maxLength(10),
          ValidationService.decimalValidation,
        ]),
        digerTutar: new FormControl(klm.digerTutar, [
          Validators.maxLength(10),
          ValidationService.decimalValidation,
        ]),
        digerTutarReferansi: new FormControl(klm.digerTutarReferansi, [
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        globalTeminatNo: new FormControl(klm.globalTeminatNo, [
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        aciklama: new FormControl(klm.aciklama, [
          Validators.maxLength(100),
         
        ]),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo,[ Validators.required,]),
    
      });

      formArray.push(formGroup);
    }
    this.teminatForm.setControl("teminatArry", formArray);
  }
  getTeminatBilgileri(){
    this.beyanServis.getObTeminat(this._beyanSession.islemInternalNo).subscribe(
      (result:ObTeminatDto[]) => {
       
        this.initTeminatFormArray(result);
        this.teminatForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getTeminat() {
    return this._fb.group({

      teminatSekli: new FormControl("", [
        Validators.required,
      ]),
      teminatOrani: new FormControl(0, [
        Validators.required,
        Validators.maxLength(10),
        ValidationService.decimalValidation,
      ]),
      bankaMektubuTutari: new FormControl(0, [
        Validators.maxLength(10),
        ValidationService.decimalValidation,
      ]),
      nakdiTeminatTutari: new FormControl(0, [
        Validators.maxLength(10),
        ValidationService.decimalValidation,
      ]),
      digerTutar: new FormControl(0, [
        Validators.maxLength(10),
        ValidationService.decimalValidation,
      ]),
      digerTutarReferansi: new FormControl("", [
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$")
      ]),
      globalTeminatNo: new FormControl("", [
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$")
      ]),
      aciklama: new FormControl("", [
        Validators.maxLength(100),
       
      ]),
      ozetBeyanInternalNo:new FormControl(this._beyanSession.ozetBeyanInternalNo, [
        Validators.required,
       
      ])
      
    });
  }

  get teminatBilgileri() {
    return this.teminatForm.get("teminatArry") as FormArray;
  }

  addTeminatField() {
    
    this.teminatBilgileri.push(this.getTeminat());
   
  }

  deleteTeminatField(index: number) {
    this.teminatBilgileri.removeAt(index);
  }



}

