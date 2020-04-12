import { Component, OnInit } from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
} from "@angular/forms";

import {
  ulke,
  kimlikTuru,
  firmaTipi,
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../shared/service-proxies/UserRoles";

import { MatSnackBar } from "@angular/material/snack-bar";

import {
  FirmaDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
@Component({
  selector: "app-firma",
  templateUrl: "./firma.component.html",
  styleUrls: ["./firma.component.css"],
})
export class FirmaComponent implements OnInit {
  firmaForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  _ulkeList = ulke;
  _firmaTipiList = firmaTipi;
  _kimlikTuruList = kimlikTuru;
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder
  ) {
    this.firmaForm = this._fb.group({
      firmaArry: this._fb.array([this.getFirma()]),
    });
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
    )
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kalem Bulunamadı",
        "Tamam"
      );
    this.getFirmaBilgileri();
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  islemFirma() {
    this.firmaForm.enable();
    this.firmaForm.markAllAsTouched();
  }
  yeniFirma() {
    this.firmaForm.reset();
    const formTeminatArray = this.firmaForm.get("firmaArry") as FormArray;
    formTeminatArray.clear();
    this.firmaForm.setControl("firmaArry", formTeminatArray);
    this.firmaForm.markAllAsTouched();
  }

  onFirmaFormSubmit() {
    console.log(this.firmaBilgileri.value);
    if (this.firmaBilgileri.length > 0) {
      this.initFirmaFormArray(this.firmaBilgileri.value);

      if (this.firmaBilgileri.invalid) {
        const invalid = [];
        const controls = this.firmaBilgileri.controls;

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

    if (this.firmaBilgileri.length >= 0) {
      const promiseOdeme = this.beyanServis
        .restoreFirma(
          this.firmaBilgileri.value,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseOdeme.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.firmaForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }
  initFirmaFormArray(teminat: FirmaDto[]) {
    const formArray = this.firmaForm.get("firmaArry") as FormArray;
    formArray.clear();
    for (let klm of teminat) {
      let formGroup: FormGroup = new FormGroup({
        adUnvan: new FormControl(klm.adUnvan, [
          Validators.required,
          Validators.maxLength(150),
        ]),
        caddeSokakNo: new FormControl(klm.caddeSokakNo, [
          Validators.required,
          Validators.maxLength(150),
        ]),
        faks: new FormControl(klm.faks, [Validators.maxLength(15)]),
        ilIlce: new FormControl(klm.ilIlce, [
          Validators.required,
          Validators.maxLength(35),
        ]),
        kimlikTuru: new FormControl(klm.kimlikTuru, [Validators.maxLength(9)]),
        no: new FormControl(klm.no, [
          Validators.required,
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        postaKodu: new FormControl(klm.postaKodu, [
          Validators.maxLength(10),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        telefon: new FormControl(klm.telefon, [
          Validators.required,
          Validators.maxLength(100),
          Validators.pattern("^[0-9]*$"),
        ]),
        
        tip: new FormControl(klm.tip, [ Validators.required,Validators.maxLength(15)]),
        ulkeKodu: new FormControl(klm.ulkeKodu, [ Validators.required,Validators.maxLength(9)]),

        beyanInternalNo: new FormControl(klm.beyanInternalNo, [
          Validators.required,
        ]),
      });

      formArray.push(formGroup);
    }
    this.firmaForm.setControl("firmaArry", formArray);
  }
  getFirmaBilgileri() {
    this.beyanServis.getFirma(this._beyanSession.islemInternalNo).subscribe(
      (result: FirmaDto[]) => {
        this.initFirmaFormArray(result);
        this.firmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }

  getFirma() {
    return this._fb.group({
      adUnvan: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      faks: new FormControl("", [Validators.maxLength(15)]),
      ilIlce: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      kimlikTuru: new FormControl("", [ Validators.required,Validators.maxLength(9)]),
      no: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
        Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      telefon: new FormControl("", [
        Validators.required,
        Validators.maxLength(100),
        Validators.pattern("^[0-9]*$"),
      ]),

      tip: new FormControl("", [ Validators.required,Validators.maxLength(15)]),
      ulkeKodu: new FormControl("", [ Validators.required,Validators.maxLength(9)]),

      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
    });
  }

  get firmaBilgileri() {
    return this.firmaForm.get("firmaArry") as FormArray;
  }

  addFirmaField() {
    this.firmaBilgileri.push(this.getFirma());
  }

  deleteFirmaField(index: number) {
    this.firmaBilgileri.removeAt(index);
  }
}
