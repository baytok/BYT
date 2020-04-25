import { Component, OnInit } from "@angular/core";
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
import { ulke } from "../../../shared/helpers/referencesList";
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
  KiymetDto,
  KiymetKalemDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
import { ReferansService } from "../../../shared/helpers/ReferansService";

@Component({
  selector: "app-kiymet",
  templateUrl: "./kiymet.component.html",
  styleUrls: ["./kiymet.component.css"],
})
export class KiymetComponent implements OnInit {
  kiymetInternalNo: string;
  kiymetForm: FormGroup;
  kalemForm: FormGroup;
  _kiymetler: KiymetDto[];
  _kalemler: KiymetKalemDto[];
  _teslimList = this.referansService.getteslimSekliJSON();
  _aliciSaticiList = this.referansService.getaliciSaticiJSON();
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;

  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router
  ) {
    (this.kiymetForm = this._fb.group({
      id: [],
      beyanInternalNo: [],
      kiymetInternalNo: [],

      aliciSatici: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      aliciSaticiAyrintilar: new FormControl("", [Validators.maxLength(300)]),
      edim: new FormControl("", [Validators.maxLength(9)]),
      emsal: new FormControl("", [Validators.maxLength(9)]),
      faturaTarihiSayisi: new FormControl("", [
        Validators.required,
        Validators.maxLength(300),
      ]),
      gumrukIdaresiKarari: new FormControl("", [
        Validators.required,
        Validators.maxLength(300),
      ]),
      kisitlamalar: new FormControl("", [Validators.maxLength(9)]),
      kisitlamalarAyrintilar: new FormControl("", [Validators.maxLength(9)]),
      munasebet: new FormControl("", [Validators.maxLength(9)]),
      royalti: new FormControl("", [Validators.maxLength(9)]),
      royaltiKosullar: new FormControl("", [Validators.maxLength(300)]),
      saticiyaIntikal: new FormControl("", [Validators.maxLength(9)]),
      saticiyaIntikalKosullar: new FormControl("", [Validators.maxLength(300)]),
      sehirYer: new FormControl("", [
        Validators.required,
        Validators.maxLength(300),
      ]),
      sozlesmeTarihiSayisi: new FormControl("", [
        Validators.required,
        Validators.maxLength(300),
      ]),
      taahhutname: [true],

      teslimSekli: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
    })),
      (this.kalemForm = this._fb.group({
        kalemArry: this._fb.array([this.getKalem()]),
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
    ) {
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kıymet Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl("/app/beyanname");
    }

    this.getKiymetler(this._beyanSession.islemInternalNo);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  get focus() {
    return this.kiymetForm.controls;
  }

  yukleKiymet() {
    this.getKiymetler(this._beyanSession.islemInternalNo);
  }
  getKiymetler(islemInternalNo: string) {
    this.beyanServis.getKiymet(islemInternalNo).subscribe(
      (result: KiymetDto[]) => {
        this._kiymetler = result;
        this.kiymetForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getKiymet(kiymetInternalNo) {
    let siraNo = null;
    for (let i in this._kiymetler) {
      if (this._kiymetler[i].kiymetInternalNo === kiymetInternalNo) siraNo = i;
    }

    this.kiymetForm.setValue({
      beyanInternalNo: this._kiymetler[siraNo].beyanInternalNo,
      kiymetInternalNo: this._kiymetler[siraNo].kiymetInternalNo,
      aliciSatici: this._kiymetler[siraNo].aliciSatici,
      aliciSaticiAyrintilar: this._kiymetler[siraNo].aliciSaticiAyrintilar,
      edim: this._kiymetler[siraNo].edim,
      emsal: this._kiymetler[siraNo].emsal,
      faturaTarihiSayisi: this._kiymetler[siraNo].faturaTarihiSayisi,
      gumrukIdaresiKarari: this._kiymetler[siraNo].gumrukIdaresiKarari,
      kisitlamalar: this._kiymetler[siraNo].kisitlamalar,
      kisitlamalarAyrintilar: this._kiymetler[siraNo].kisitlamalarAyrintilar,
      munasebet: this._kiymetler[siraNo].munasebet,
      royalti: this._kiymetler[siraNo].royalti,
      royaltiKosullar: this._kiymetler[siraNo].royaltiKosullar,
      saticiyaIntikal: this._kiymetler[siraNo].saticiyaIntikal,
      saticiyaIntikalKosullar: this._kiymetler[siraNo].saticiyaIntikalKosullar,
      sehirYer: this._kiymetler[siraNo].sehirYer,
      sozlesmeTarihiSayisi: this._kiymetler[siraNo].sozlesmeTarihiSayisi,
      taahhutname:
        this._kiymetler[siraNo].taahhutname === "Evet" ? true : false,
      teslimSekli: this._kiymetler[siraNo].teslimSekli,
      id: this._kiymetler[siraNo].id,
    });
    this.kiymetInternalNo = kiymetInternalNo;

    this.beyanServis
      .getKiymetKalem(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: KiymetKalemDto[]) => {
          this._kalemler = result.filter(
            (x) => x.kiymetInternalNo === this.kiymetInternalNo
          );
          this.initkalemFormArray(this._kalemler);

          this.kalemForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

    this.kiymetForm.disable();
  }

  yeniKiymet() {
    this.kiymetInternalNo = "Boş";
    this.kiymetForm.reset();
    this.kiymetForm.enable();
    this.kalemForm.reset();
    this.kalemForm.enable();
    this.kiymetForm.markAllAsTouched();
    this.kalemForm.markAllAsTouched();
  }
  duzeltKiymet() {
    this.kiymetForm.enable();
    this.kiymetForm.markAllAsTouched();
    this.kalemForm.enable();
    this.kalemForm.markAllAsTouched();
  }
  silKiymet(kiymetInternalNo) {
    if (
      confirm(kiymetInternalNo + "- kiymeti Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeKiymet(kiymetInternalNo, this._beyanSession.beyanInternalNo)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.kiymetForm.disable();
          this.kiymetForm.reset();
          this.kalemForm.disable();
          this.kalemForm.reset();
          this.yukleKiymet();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }

  onkiymetFormSubmit() {
    this.submitted = true;

    if (this.kiymetForm.invalid) {
      const invalid = [];
      const controls = this.kiymetForm.controls;
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

    let tahhut =
      this.kiymetForm.get("taahhutname").value === true ? "Evet" : "Hayir";
    let ID = this.kiymetForm.get("id").value;
    this.kiymetForm.setValue({
      beyanInternalNo: this._beyanSession.beyanInternalNo,
      kiymetInternalNo: this.kiymetInternalNo,
      aliciSatici:
        this.kiymetForm.get("aliciSatici").value != null
          ? this.kiymetForm.get("aliciSatici").value
          : "",
      aliciSaticiAyrintilar:
        this.kiymetForm.get("aliciSaticiAyrintilar").value != null
          ? this.kiymetForm.get("aliciSaticiAyrintilar").value
          : "",
      edim:
        this.kiymetForm.get("edim").value != null
          ? this.kiymetForm.get("edim").value
          : "",
      emsal:
        this.kiymetForm.get("emsal").value != null
          ? this.kiymetForm.get("emsal").value
          : "",
      faturaTarihiSayisi:
        this.kiymetForm.get("faturaTarihiSayisi").value != null
          ? this.kiymetForm.get("faturaTarihiSayisi").value
          : "",
      gumrukIdaresiKarari:
        this.kiymetForm.get("gumrukIdaresiKarari").value != null
          ? this.kiymetForm.get("gumrukIdaresiKarari").value
          : "",
      kisitlamalar:
        this.kiymetForm.get("kisitlamalar").value != null
          ? this.kiymetForm.get("kisitlamalar").value
          : "",
      kisitlamalarAyrintilar:
        this.kiymetForm.get("kisitlamalarAyrintilar").value != null
          ? this.kiymetForm.get("kisitlamalarAyrintilar").value
          : "",
      munasebet:
        this.kiymetForm.get("munasebet").value != null
          ? this.kiymetForm.get("munasebet").value
          : "",
      royalti:
        this.kiymetForm.get("royalti").value != null
          ? this.kiymetForm.get("royalti").value
          : "",
      royaltiKosullar:
        this.kiymetForm.get("royaltiKosullar").value != null
          ? this.kiymetForm.get("royaltiKosullar").value
          : "",
      saticiyaIntikal:
        this.kiymetForm.get("saticiyaIntikal").value != null
          ? this.kiymetForm.get("saticiyaIntikal").value
          : "",
      saticiyaIntikalKosullar:
        this.kiymetForm.get("saticiyaIntikalKosullar").value != null
          ? this.kiymetForm.get("saticiyaIntikalKosullar").value
          : "",
      sehirYer:
        this.kiymetForm.get("sehirYer").value != null
          ? this.kiymetForm.get("sehirYer").value
          : "",
      sozlesmeTarihiSayisi:
        this.kiymetForm.get("sozlesmeTarihiSayisi").value != null
          ? this.kiymetForm.get("sozlesmeTarihiSayisi").value
          : "",
      teslimSekli:
        this.kiymetForm.get("teslimSekli").value != null
          ? this.kiymetForm.get("teslimSekli").value
          : "",
      taahhutname: tahhut,
      id: ID != null ? ID : 0,
    });
    // this.kiymetForm
    //   .get("beyanInternalNo")
    //   .setValue(this._beyanSession.beyanInternalNo);

    // this.kiymetForm.get("kiymetInternalNo").setValue(this.kiymetInternalNo);
    // this.kiymetForm.get("taahhutname").setValue(tahhut);

    let yenikiymetInternalNo: string;
    let yeniKiymet = new KiymetDto();
    yeniKiymet.init(this.kiymetForm.value);

    const promiseKalem = this.beyanServis.restoreKiymet(yeniKiymet).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kiymetServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenikiymetInternalNo = kiymetServisSonuc.ReferansNo;

        if (yenikiymetInternalNo != null) {
          this.kiymetInternalNo = yenikiymetInternalNo;
          this.setKalem();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.kiymetForm.disable();
          this.yukleKiymet();
        }
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }

  kisitlamaSelect(kisitlama) {
    if (kisitlama == "Evet") {
      this.kiymetForm.controls["kisitlamalarAyrintilar"].setValidators([
        Validators.required,
      ]);
      this.kiymetForm.controls[
        "kisitlamalarAyrintilar"
      ].updateValueAndValidity();
    } else {
      this.kiymetForm.controls["kisitlamalarAyrintilar"].clearValidators();
      this.kiymetForm.controls[
        "kisitlamalarAyrintilar"
      ].updateValueAndValidity();
    }
  }

  saticiyaIntikalSelect(intikal) {
    if (intikal == "Evet") {
      this.kiymetForm.controls["saticiyaIntikalKosullar"].setValidators([
        Validators.required,
      ]);
      this.kiymetForm.controls[
        "saticiyaIntikalKosullar"
      ].updateValueAndValidity();
    } else {
      this.kiymetForm.controls["saticiyaIntikalKosullar"].clearValidators();
      this.kiymetForm.controls[
        "saticiyaIntikalKosullar"
      ].updateValueAndValidity();
    }
  }

  royaltiSelect(royalti) {
    if (royalti == "Evet") {
      this.kiymetForm.controls["royaltiKosullar"].setValidators([
        Validators.required,
      ]);
      this.kiymetForm.controls[
        "royaltiKosullar"
      ].updateValueAndValidity();
    } else {
      this.kiymetForm.controls["royaltiKosullar"].clearValidators();
      this.kiymetForm.controls[
        "royaltiKosullar"
      ].updateValueAndValidity();
    }
  }
  initkalemFormArray(kalem: KiymetKalemDto[]) {
    const formArray = this.kalemForm.get("kalemArry") as FormArray;
    formArray.clear();
    for (let klm of kalem) {
      let formGroup: FormGroup = new FormGroup({
        beyannameKalemNo: new FormControl(klm.beyannameKalemNo, [
          Validators.required,
          ValidationService.numberValidator,
        ]),
        kiymetKalemNo: new FormControl(klm.kiymetKalemNo, [
          Validators.required,
          ValidationService.numberValidator,
        ]),
        digerOdemeler: new FormControl(klm.digerOdemeler, [
          ValidationService.decimalValidation,
        ]),
        digerOdemelerNiteligi: new FormControl(klm.digerOdemelerNiteligi, [
          Validators.maxLength(100),
        ]),
        dolayliIntikal: new FormControl(klm.dolayliIntikal, [
          ValidationService.decimalValidation,
        ]),
        dolayliOdeme: new FormControl(klm.dolayliOdeme, [
          ValidationService.decimalValidation,
        ]),
        girisSonrasiNakliye: new FormControl(klm.girisSonrasiNakliye, [
          ValidationService.decimalValidation,
        ]),
        ithalaKatilanMalzeme: new FormControl(klm.ithalaKatilanMalzeme, [
          ValidationService.decimalValidation,
        ]),
        ithalaUretimAraclar: new FormControl(klm.ithalaUretimAraclar, [
          ValidationService.decimalValidation,
        ]),
        ithalaUretimTuketimMalzemesi: new FormControl(
          klm.ithalaUretimTuketimMalzemesi,
          [ValidationService.decimalValidation]
        ),
        kapAmbalajBedeli: new FormControl(klm.kapAmbalajBedeli, [
          ValidationService.decimalValidation,
        ]),
        komisyon: new FormControl(klm.komisyon, [
          ValidationService.decimalValidation,
        ]),
        nakliye: new FormControl(klm.nakliye, [
          ValidationService.decimalValidation,
        ]),
        planTaslak: new FormControl(klm.planTaslak, [
          ValidationService.decimalValidation,
        ]),
        royaltiLisans: new FormControl(klm.royaltiLisans, [
          ValidationService.decimalValidation,
        ]),
        sigorta: new FormControl(klm.sigorta, [
          ValidationService.decimalValidation,
        ]),
        teknikYardim: new FormControl(klm.teknikYardim, [
          ValidationService.decimalValidation,
        ]),
        tellaliye: new FormControl(klm.tellaliye, [
          ValidationService.decimalValidation,
        ]),
        vergiHarcFon: new FormControl(klm.vergiHarcFon, [
          ValidationService.decimalValidation,
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kiymetInternalNo: new FormControl(klm.kiymetInternalNo),
      });

      formArray.push(formGroup);
    }
    this.kalemForm.setControl("kalemArry", formArray);
  }

  getKalem() {
    return this._fb.group({
      beyannameKalemNo: new FormControl(0, [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      kiymetKalemNo: new FormControl(0, [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      digerOdemeler: new FormControl(0, [ValidationService.decimalValidation]),
      digerOdemelerNiteligi: new FormControl("", [Validators.maxLength(100)]),
      dolayliIntikal: new FormControl(0, [ValidationService.decimalValidation]),
      dolayliOdeme: new FormControl(0, [ValidationService.decimalValidation]),
      girisSonrasiNakliye: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      ithalaKatilanMalzeme: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      ithalaUretimAraclar: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      ithalaUretimTuketimMalzemesi: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      kapAmbalajBedeli: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      komisyon: new FormControl(0, [ValidationService.decimalValidation]),
      nakliye: new FormControl(0, [ValidationService.decimalValidation]),
      planTaslak: new FormControl(0, [ValidationService.decimalValidation]),
      royaltiLisans: new FormControl(0, [ValidationService.decimalValidation]),
      sigorta: new FormControl(0, [ValidationService.decimalValidation]),
      teknikYardim: new FormControl(0, [ValidationService.decimalValidation]),
      tellaliye: new FormControl(0, [ValidationService.decimalValidation]),
      vergiHarcFon: new FormControl(0, [ValidationService.decimalValidation]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kiymetInternalNo: new FormControl(this.kiymetInternalNo, [
        Validators.required,
      ]),
    });
  }

  get kalemBilgileri() {
    return this.kalemForm.get("kalemArry") as FormArray;
  }

  addKalemField() {
    this.kalemBilgileri.push(this.getKalem());
  }

  deleteKalemField(index: number) {
    this.kalemBilgileri.removeAt(index);
  }

  setKalem() {
    if (this.kalemBilgileri.length > 0) {
      for (let klm of this.kalemBilgileri.value) {
        klm.kiymetInternalNo = this.kiymetInternalNo;
        klm.kiymetKalemNo = 0;
        klm.beyannameKalemNo =
          typeof klm.beyannameKalemNo == "string"
            ? parseInt(klm.beyannameKalemNo)
            : klm.beyannameKalemNo;

        klm.digerOdemeler =
          typeof klm.digerOdemeler == "string"
            ? parseFloat(klm.digerOdemeler)
            : klm.digerOdemeler;

        klm.dolayliIntikal =
          typeof klm.dolayliIntikal == "string"
            ? parseFloat(klm.dolayliIntikal)
            : klm.dolayliIntikal;

        klm.dolayliOdeme =
          typeof klm.dolayliOdeme == "string"
            ? parseFloat(klm.dolayliOdeme)
            : klm.dolayliOdeme;

        klm.girisSonrasiNakliye =
          typeof klm.girisSonrasiNakliye == "string"
            ? parseFloat(klm.girisSonrasiNakliye)
            : klm.girisSonrasiNakliye;

        klm.ithalaKatilanMalzeme =
          typeof klm.ithalaKatilanMalzeme == "string"
            ? parseFloat(klm.ithalaKatilanMalzeme)
            : klm.ithalaKatilanMalzeme;

        klm.ithalaUretimAraclar =
          typeof klm.ithalaUretimAraclar == "string"
            ? parseFloat(klm.ithalaUretimAraclar)
            : klm.ithalaUretimAraclar;

        klm.ithalaUretimTuketimMalzemesi =
          typeof klm.ithalaUretimTuketimMalzemesi == "string"
            ? parseFloat(klm.ithalaUretimTuketimMalzemesi)
            : klm.ithalaUretimTuketimMalzemesi;

        klm.kapAmbalajBedeli =
          typeof klm.kapAmbalajBedeli == "string"
            ? parseFloat(klm.kapAmbalajBedeli)
            : klm.kapAmbalajBedeli;

        klm.komisyon =
          typeof klm.komisyon == "string"
            ? parseFloat(klm.komisyon)
            : klm.komisyon;

        klm.nakliye =
          typeof klm.nakliye == "string"
            ? parseFloat(klm.nakliye)
            : klm.nakliye;

        klm.planTaslak =
          typeof klm.planTaslak == "string"
            ? parseFloat(klm.planTaslak)
            : klm.planTaslak;

        klm.royaltiLisans =
          typeof klm.royaltiLisans == "string"
            ? parseFloat(klm.royaltiLisans)
            : klm.royaltiLisans;

        klm.sigorta =
          typeof klm.sigorta == "string"
            ? parseFloat(klm.sigorta)
            : klm.sigorta;

        klm.teknikYardim =
          typeof klm.teknikYardim == "string"
            ? parseFloat(klm.teknikYardim)
            : klm.teknikYardim;

        klm.tellaliye =
          typeof klm.tellaliye == "string"
            ? parseFloat(klm.tellaliye)
            : klm.tellaliye;

        klm.vergiHarcFon =
          typeof klm.vergiHarcFon == "string"
            ? parseFloat(klm.vergiHarcFon)
            : klm.vergiHarcFon;
      }
      this.initkalemFormArray(this.kalemBilgileri.value);

      if (this.kalemBilgileri.invalid) {
        const invalid = [];
        const controls = this.kalemBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Kiymet Kalem Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.kalemBilgileri.length >= 0) {
      const promiseMarka = this.beyanServis
        .restoreKiymetKalem(
          this.kalemBilgileri.value,
          this.kiymetInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseMarka.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
          // this.odemeForm.disable();
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
