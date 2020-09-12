import { Component, OnInit, ViewEncapsulation } from "@angular/core";
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

import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import {
  ObOzetBeyanAcmaDto,
  ObOzetBeyanAcmaTasimaSenetDto,
  ObOzetBeyanAcmaTasimaSatirDto,
  ServisDto,
  BeyanIslemDurumlari
} from "../../../../shared/service-proxies/service-proxies";
import { ReferansService } from "../../../../shared/helpers/ReferansService";
import { NgbModal, ModalDismissReasons } from "@ng-bootstrap/ng-bootstrap";
@Component({
  selector: "app-obozetbeyanacma",
  templateUrl: "./obozetbeyanacma.component.html",
  styleUrls: ["./obozetbeyanacma.component.css"],
  encapsulation: ViewEncapsulation.None,
})
export class ObOzetBeyanAcmaComponent implements OnInit {
  ozetBeyanAcmaBeyanInternalNo: string;
  ozetBeyanNo:string;
  tasimaSenetInternalNo: string;
  tasimaSenediNo: string;
  ozetBeyanForm: FormGroup;
  tasimaSenetiForm: FormGroup;
  tasimaSatiriForm: FormGroup;
  _ozetBeyanlar: ObOzetBeyanAcmaDto[];
  _tasimaSenetleri: ObOzetBeyanAcmaTasimaSenetDto[];
  _tasimaSatirlari: ObOzetBeyanAcmaTasimaSatirDto[];
  _olcuList = this.referansService.getolcuJSON();
  _cinsList = this.referansService.getkapCinsiJSON();
  tasimaSatirGoster: boolean = false;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  ozetBeyanInternalNo = this._beyanSession.ozetBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  beyanDurum: BeyanIslemDurumlari=new BeyanIslemDurumlari();
  title = "Taşıma Satırları";
  closeResult: string;
  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router,
    private modalService: NgbModal
  ) {
    (this.ozetBeyanForm = this._fb.group({
      id: [],
      ozetBeyanAcmaBeyanInternalNo: [],
      ozetBeyanInternalNo: [],

      islemKapsami: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      ozetBeyanNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(30),
      ]),
     
      aciklama: new FormControl("", [Validators.maxLength(1500)]),
      baskaRejim: [false],
      ambar: [false],
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
    ) {
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Özet Beyan Açma Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl("/app/ozetbeyan");
    }
    this.ozetBeyanNo = this._beyanSession.ozetBeyanNo!=null ? this._beyanSession.ozetBeyanNo: this._beyanSession.ozetBeyanInternalNo;
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

  get BeyanStatu(): boolean {
    if (this.beyanStatu === "undefined" || this.beyanStatu === null)
      return false;

      if(this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
      return true;
    else return false;
  }
  get BeyanSilDuzeltStatu(): boolean {
    if (this.beyanStatu === "undefined" || this.beyanStatu === null)
      return false;
    if (
      this.ozetBeyanAcmaBeyanInternalNo != "Boş" &&
      this.ozetBeyanAcmaBeyanInternalNo != null &&
      this.ozetBeyanAcmaBeyanInternalNo != "" &&
      this.ozetBeyanAcmaBeyanInternalNo != "undefined" &&
      (this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
    )
      return true;
    else return false;
  }

  yukleOzetBeyan() {
    this.getOzetBeyanlar(this._beyanSession.islemInternalNo);
    this.ozetBeyanAcmaBeyanInternalNo = "";
  }
  getOzetBeyanlar(islemInternalNo: string) {
    this.beyanServis.getObOzetBeyanAcma(islemInternalNo).subscribe(
      (result: ObOzetBeyanAcmaDto[]) => {
        this._ozetBeyanlar = result;
        this.ozetBeyanForm.disable();
        this.tasimaSenetiForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getOzetBeyanBilgileri(ozetBeyanAcmaBeyanInternalNo) {
    let siraNo = null;
  
    for (let i in this._ozetBeyanlar) {
      if (
        this._ozetBeyanlar[i].ozetBeyanAcmaBeyanInternalNo ===
        ozetBeyanAcmaBeyanInternalNo
      )
        siraNo = i;
    }

    this.ozetBeyanForm.setValue({
      ozetBeyanAcmaBeyanInternalNo: this._ozetBeyanlar[siraNo]
        .ozetBeyanAcmaBeyanInternalNo,
      ozetBeyanInternalNo: this._ozetBeyanlar[siraNo].ozetBeyanInternalNo,
      ozetBeyanNo: this._ozetBeyanlar[siraNo].ozetBeyanNo,
      islemKapsami: this._ozetBeyanlar[siraNo].islemKapsami,
      baskaRejim:
        this._ozetBeyanlar[siraNo].baskaRejim === "Evet" ? true : false,
      ambar: this._ozetBeyanlar[siraNo].ambar === "Evet" ? true : false,
      aciklama: this._ozetBeyanlar[siraNo].aciklama,
      id: this._ozetBeyanlar[siraNo].id,
    });
    this.ozetBeyanAcmaBeyanInternalNo = ozetBeyanAcmaBeyanInternalNo;

    this.beyanServis
      .getObOzetBeyanAcmaTasimaSenet(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: ObOzetBeyanAcmaTasimaSenetDto[]) => {
          this._tasimaSenetleri = result.filter(
            (x) =>
              x.ozetBeyanAcmaBeyanInternalNo ===
              this.ozetBeyanAcmaBeyanInternalNo
          );
          this.initTasimaSenetiFormArray(this._tasimaSenetleri);
          this.tasimaSenetiForm.disable();
          this.tasimaSenetInternalNo = "";
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    this.tasimaSatiriForm.disable();
    this.tasimaSenetiForm.disable();
    this.ozetBeyanForm.disable();

    if (this.ozetBeyanForm.get("islemKapsami").value === "3")
      this.tasimaSatirGoster = true;
    else this.tasimaSatirGoster = false;
  }

  yeniOzetBeyan() {
    this.ozetBeyanAcmaBeyanInternalNo = "Boş";
    this.tasimaSenetInternalNo = "Boş";
    this.ozetBeyanForm.reset();
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
    this.tasimaSenetiForm.reset();
    this.tasimaSenetiForm.enable();
    this.tasimaSenetiForm.markAllAsTouched();
    this.tasimaSatirGoster = false;

    const formSenetArray = this.tasimaSenetiForm.get(
      "tasimaSenetiArry"
    ) as FormArray;
    formSenetArray.clear();
    this.tasimaSenetiForm.setControl("tasimaSenetiArry", formSenetArray);
  }
  duzeltOzetBeyan() {
    this.ozetBeyanForm.enable();
    this.ozetBeyanForm.markAllAsTouched();
    this.tasimaSenetiForm.enable();
    this.tasimaSenetiForm.markAllAsTouched();
    this.tasimaSatirGoster = false;
  }
  silOzetBeyan(ozetBeyanAcmaBeyanInternalNo) {
    if (
      confirm(
        ozetBeyanAcmaBeyanInternalNo +
          "- Özet Beyanı Silmek İstediğinizden Eminmisiniz?"
      )
    ) {
      const promise = this.beyanServis
        .removeObOzetBeyanAcma(
          this.ozetBeyanAcmaBeyanInternalNo,
          ozetBeyanAcmaBeyanInternalNo
        )
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.ozetBeyanForm.disable();
          this.ozetBeyanForm.reset();
          this.ozetBeyanAcmaBeyanInternalNo = "Boş";
          this.tasimaSenetInternalNo = "Boş";
          this.tasimaSenetiForm.disable();
          this.tasimaSenetiForm.reset();
          this.yukleOzetBeyan();
          this.tasimaSatirGoster = false;
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

    let _ambar =
      this.ozetBeyanForm.get("ambar").value === true ? "Evet" : "Hayir";
    let _baskaRejim =
      this.ozetBeyanForm.get("baskaRejim").value === true ? "Evet" : "Hayir";
    let ID = this.ozetBeyanForm.get("id").value;

    this.ozetBeyanForm.setValue({
      ozetBeyanInternalNo: this._beyanSession.ozetBeyanInternalNo,
      ozetBeyanAcmaBeyanInternalNo: this.ozetBeyanAcmaBeyanInternalNo,
      ozetBeyanNo:
        this.ozetBeyanForm.get("ozetBeyanNo").value != null
          ? this.ozetBeyanForm.get("ozetBeyanNo").value
          : "",
    
      islemKapsami:
        this.ozetBeyanForm.get("islemKapsami").value != null
          ? this.ozetBeyanForm.get("islemKapsami").value
          : "",
      aciklama:
        this.ozetBeyanForm.get("aciklama").value != null
          ? this.ozetBeyanForm.get("aciklama").value
          : "",
      baskaRejim: _baskaRejim,
      ambar: _ambar,
      id: ID != null ? ID : 0,
    });

    let yeniOzetBeyanInternalNo: string;
    let yeniOzetBeyanAcma = new ObOzetBeyanAcmaDto();
    yeniOzetBeyanAcma.init(this.ozetBeyanForm.value);

    const promiseKalem = this.beyanServis
      .restoreObOzetBeyanAcma(yeniOzetBeyanAcma)
      .toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kiymetServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yeniOzetBeyanInternalNo = kiymetServisSonuc.ReferansNo;

        if (yeniOzetBeyanInternalNo != null) {
          this.ozetBeyanAcmaBeyanInternalNo = yeniOzetBeyanInternalNo;
          this.setTasimaSeneti();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.ozetBeyanForm.disable();
          this.ozetBeyanForm.reset();
          this.yukleOzetBeyan();
        }
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }

  initTasimaSenetiFormArray(tasimaSenet: ObOzetBeyanAcmaTasimaSenetDto[]) {
    const formArray = this.tasimaSenetiForm.get(
      "tasimaSenetiArry"
    ) as FormArray;
    formArray.clear();
    for (let klm of tasimaSenet) {
      let formGroup: FormGroup = new FormGroup({
        id: new FormControl(klm.id, [Validators.required]),
        tasimaSenediNo: new FormControl(klm.tasimaSenediNo, [
          Validators.required,
          Validators.maxLength(50),
        ]),
        ozetBeyanAcmaBeyanInternalNo: new FormControl(
          klm.ozetBeyanAcmaBeyanInternalNo
        ),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
        tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
      });

      formArray.push(formGroup);
    }
    this.tasimaSenetiForm.setControl("tasimaSenetiArry", formArray);
  }
  get BeyanSatirStatu(): boolean {
    if (this.beyanStatu === "undefined" || this.beyanStatu === null)
      return false;
    if (
      this.tasimaSenetInternalNo != "Boş" &&
      this.tasimaSenetInternalNo != null &&
      this.tasimaSenetInternalNo != "" &&
      this.tasimaSenetInternalNo != "undefined" &&
      (this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
    )
      return true;
    else return false;
  }
  getTasimaSenetBilgileri(content, index: number) {
    let tasimaSenetInternalNo = this.tasimaSenetiBilgileri.controls[index].get(
      "tasimaSenetInternalNo"
    ).value;
    this.tasimaSenetInternalNo = tasimaSenetInternalNo;

    this.tasimaSenediNo = this.tasimaSenetiBilgileri.controls[index].get(
      "tasimaSenediNo"
    ).value;
    this.beyanServis
      .getObOzetBeyanAcmaTasimaSatir(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: ObOzetBeyanAcmaTasimaSatirDto[]) => {
          this._tasimaSatirlari = result.filter(
            (x) => x.tasimaSenetInternalNo === this.tasimaSenetInternalNo
          );
          this.initTasimaSatiriFormArray(this._tasimaSatirlari);
          this.tasimaSatiriForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    this.modalService
      .open(content, { ariaLabelledBy: "modal-basic-title" })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }
  getTasimaSeneti() {
    return this._fb.group({
      id: new FormControl(0, [Validators.required]),
      tasimaSenediNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(50),
      ]),
      ozetBeyanInternalNo: new FormControl(
        this._beyanSession.ozetBeyanInternalNo,
        [Validators.required]
      ),
      ozetBeyanAcmaBeyanInternalNo: new FormControl(
        this.ozetBeyanAcmaBeyanInternalNo,
        [Validators.required]
      ),
      tasimaSenetInternalNo: new FormControl("Boş", [Validators.required]),
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
        klm.ozetBeyanAcmaBeyanInternalNo = this.ozetBeyanAcmaBeyanInternalNo;
       // klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
      }

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
      const promiseTasimaSenet = this.beyanServis
        .restoreObOzetBeyanAcmaTasimaSenet(
          this.tasimaSenetiBilgileri.value,
          this.ozetBeyanAcmaBeyanInternalNo,
          this.ozetBeyanInternalNo
        )
        .toPromise();
      promiseTasimaSenet.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }

  tasimaSatirIslemOzetBeyan() {
    this.tasimaSatiriForm.enable();
  }
  initTasimaSatiriFormArray(tasimaSatir: ObOzetBeyanAcmaTasimaSatirDto[]) {
    const formArray = this.tasimaSatiriForm.get(
      "tasimaSatiriArry"
    ) as FormArray;
    formArray.clear();
    for (let klm of tasimaSatir) {
      let formGroup: FormGroup = new FormGroup({
        acmaSatirNo: new FormControl(klm.acmaSatirNo, [
          ValidationService.numberValidator,
        ]),
        ambarKodu: new FormControl(klm.ambarKodu, [Validators.maxLength(9)]),
        ambardakiMiktar: new FormControl(klm.ambardakiMiktar, [
          ValidationService.decimalValidation,
        ]),
        acilacakMiktar: new FormControl(klm.acilacakMiktar, [
          ValidationService.decimalValidation,
        ]),
        markaNo: new FormControl(klm.markaNo, [Validators.maxLength(60)]),
        esyaCinsi: new FormControl(klm.esyaCinsi, [Validators.maxLength(9)]),
        birim: new FormControl(klm.birim, [Validators.maxLength(9)]),
        toplamMiktar: new FormControl(klm.toplamMiktar, [
          ValidationService.decimalValidation,
        ]),
        kapatilanMiktar: new FormControl(klm.kapatilanMiktar, [
          ValidationService.decimalValidation,
        ]),
        olcuBirimi: new FormControl(klm.olcuBirimi, [Validators.maxLength(9)]),

        ozetBeyanAcmaBeyanInternalNo: new FormControl(
          klm.ozetBeyanAcmaBeyanInternalNo
        ),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
        tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
      });

      formArray.push(formGroup);
    }
    this.tasimaSatiriForm.setControl("tasimaSatiriArry", formArray);
  }

  getTasimaSatiri() {
    return this._fb.group({
      acmaSatirNo: new FormControl(0, [ValidationService.numberValidator]),
      ambarKodu: new FormControl("", [Validators.maxLength(9)]),

      ambardakiMiktar: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      acilacakMiktar: new FormControl(0, [ValidationService.decimalValidation]),
      markaNo: new FormControl("", [Validators.maxLength(60)]),
      esyaCinsi: new FormControl("", [Validators.maxLength(9)]),
      birim: new FormControl("", [Validators.maxLength(9)]),
      toplamMiktar: new FormControl(0, [ValidationService.decimalValidation]),
      kapatilanMiktar: new FormControl(0, [
        ValidationService.decimalValidation,
      ]),
      olcuBirimi: new FormControl("", [Validators.maxLength(9)]),

      ozetBeyanInternalNo: new FormControl(
        this._beyanSession.ozetBeyanInternalNo,
        [Validators.required]
      ),
      ozetBeyanAcmaBeyanInternalNo: new FormControl(
        this.ozetBeyanAcmaBeyanInternalNo,
        [Validators.required]
      ),
      tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
        Validators.required,
      ]),
    });
  }

  get tasimaSatiriBilgileri() {
    return this.tasimaSatiriForm.get("tasimaSatiriArry") as FormArray;
  }

  addTasimaSatiriField() {
    this.tasimaSatiriBilgileri.push(this.getTasimaSatiri());
  }

  deleteTasimaSatiriField(index: number) {
    this.tasimaSatiriBilgileri.removeAt(index);
  }

  setTasimaSatiri() {
    if (this.tasimaSatiriBilgileri.length > 0) {
      for (let klm of this.tasimaSatiriBilgileri.value) {
        klm.ozetBeyanAcmaBeyanInternalNo = this.ozetBeyanAcmaBeyanInternalNo;
        klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
        klm.ambardakiMiktar =
          typeof klm.ambardakiMiktar == "string"
            ? parseInt(klm.ambardakiMiktar)
            : klm.ambardakiMiktar;
        klm.acilacakMiktar =
          typeof klm.acilacakMiktar == "string"
            ? parseInt(klm.acilacakMiktar)
            : klm.acilacakMiktar;
        klm.toplamMiktar =
          typeof klm.toplamMiktar == "string"
            ? parseInt(klm.toplamMiktar)
            : klm.toplamMiktar;
        klm.kapatilanMiktar =
          typeof klm.kapatilanMiktar == "string"
            ? parseInt(klm.kapatilanMiktar)
            : klm.kapatilanMiktar;
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
        .restoreObOzetBeyanAcmaTasimaSatir(
          this.tasimaSatiriBilgileri.value,
          this.tasimaSenetInternalNo,
          this.ozetBeyanAcmaBeyanInternalNo,        
          this._beyanSession.ozetBeyanInternalNo
        )
        .toPromise();
      promiseMarka.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
          this.tasimaSatiriForm.disable();
          this.tasimaSatiriForm.reset();
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

  private getDismissReason(reason: any): string {
 
    if (reason === ModalDismissReasons.ESC) {
      return "by pressing ESC";
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return "by clicking on a backdrop";
    } else if (reason === "Save Click") {
      this.setTasimaSatiri();
    } else {
      return `with: ${reason}`;
    }
  }
}
