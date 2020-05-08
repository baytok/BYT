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

import {IhracatTipi} from "../../../../shared/helpers/referencesList";
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
  BeyannameBilgileriDto,
  ObTasimaSenetDto,
  ObTasimaSatirDto,
  ObIhracatDto,
  ObUgrakUlkeDto,
  ObSatirEsyaDto,
  ServisDto,
} from "../../../../shared/service-proxies/service-proxies";

import {
  NativeDateAdapter,
  DateAdapter,
  MAT_DATE_FORMATS,
} from "@angular/material/core";
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

export const PICK_FORMATS = {
  parse: {
    dateInput: { month: "short", year: "numeric", day: "numeric" },
  },
  display: {
    // dateInput: { month: 'short', year: 'numeric', day: 'numeric' },
    dateInput: "input",
    // monthYearLabel: { month: 'short', year: 'numeric', day: 'numeric' },
    monthYearLabel: "inputMonth",
    dateA11yLabel: { year: "numeric", month: "long", day: "numeric" },
    monthYearA11yLabel: { year: "numeric", month: "long" },
  },
};
@Injectable()
class PickDateAdapter extends NativeDateAdapter {
  parse(value: any): Date | null {
    if (typeof value === "string" && value.indexOf("/") > -1) {
      const str = value.split("/");
      const year = Number(str[2]);
      const month = Number(str[1]) - 1;
      const date = Number(str[0]);
      return new Date(year, month, date);
    }
    const timestamp = typeof value === "number" ? value : Date.parse(value);
    return isNaN(timestamp) ? null : new Date(timestamp);
  }
  format(date: Date, displayFormat: string): string {
    if (displayFormat == "input") {
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();
      return this._to2digit(day) + "/" + this._to2digit(month) + "/" + year;
    } else if (displayFormat == "inputMonth") {
      let month = date.getMonth() + 1;
      let year = date.getFullYear();
      return this._to2digit(month) + "/" + year;
    } else {
      return date.toDateString();
    }
  }

  private _to2digit(n: number) {
    return ("00" + n).slice(-2);
  }
}
@Component({
  selector: "app-tasimasenet",
  templateUrl: "./tasimasenet.component.html",
  styleUrls: ["./tasimasenet.component.css"],
  providers: [
    { provide: DateAdapter, useClass: PickDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS },
  ],
})
export class TasimaSenetComponent implements OnInit {
  public form: FormGroup;
  tasimaSenetInternalNo: string;
  tasimaSatirInternalNo: string;
  senetSiraNo: number;
  satirNo: number;
  sentNo:string
  senetForm: FormGroup;
  ihracatForm: FormGroup;
  ulkeForm: FormGroup;
  satirForm: FormGroup;
  satirEsyaForm: FormGroup; 
  _esyalar:ObSatirEsyaDto[]; 
  title = 'Taşıma Eşya Bilgileri';  
  closeResult: string;

  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  ozetBeyanInternalNo = this._beyanSession.ozetBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _senetler: ObTasimaSenetDto[];
  _satirlar: ObTasimaSatirDto[];
  _ihracatlar: ObIhracatDto[];
  _ulkeler: ObUgrakUlkeDto[];
  _ulkeList = this.referansService.getUlkeJSON();
  _dovizList = this.referansService.getdovizCinsiJSON();
  _cinsList = this.referansService.getkapCinsiJSON();
  _olcuList = this.referansService.getolcuJSON();
  _ihracTipiList=IhracatTipi;
  @ViewChild("SenetList", { static: true })
  private selectionList: MatSelectionList;
  @ViewChild("BeyannameNo", { static: true }) private _beyannameNo: ElementRef;

  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router,
    private modalService: NgbModal,
  ) {
    (this.senetForm = this._fb.group({
      ozetBeyanInternalNo: [],
      tasimaSenetInternalNo: [],
      senetSiraNo: [],
      tasimaSenediNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      acentaAdi: new FormControl("", [Validators.maxLength(150)]),
      acentaVergiNo: new FormControl("", [Validators.maxLength(20)]),
      aliciAdi: new FormControl("", [Validators.maxLength(150)]),
      aliciVergiNo: new FormControl("", [Validators.maxLength(20)]),
      ambarHarici: [false],
      bildirimTarafiAdi: new FormControl("", [Validators.maxLength(150)]),
      bildirimTarafiVergiNo: new FormControl("", [Validators.maxLength(20)]),
      duzenlendigiUlke: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      emniyetGuvenlik: [false],
      esyaninBulunduguYer: new FormControl("", [Validators.maxLength(16)]),
      faturaDoviz: new FormControl("", [Validators.maxLength(9)]),
      faturaToplami: new FormControl("", [ValidationService.decimalValidation]),
      gondericiAdi: new FormControl("", [Validators.maxLength(150)]),
      gondericiVergiNo: new FormControl("", [Validators.maxLength(20)]),
      grup: [false],
      konteyner: [false],
      navlunDoviz: new FormControl("", [Validators.maxLength(9)]),
      navlunTutari: new FormControl("", [ValidationService.decimalValidation]),
      odemeSekli: new FormControl("", [Validators.maxLength(9)]),
      oncekiSeferNumarasi: new FormControl("", [Validators.maxLength(20)]),
      oncekiSeferTarihi: new FormControl("", [
        Validators.maxLength(12),
        ValidationService.tarihValidation,
      ]),
      ozetBeyanNo: new FormControl("", [Validators.maxLength(20)]),
      roro: [false],
      aktarmaYapilacak: [false],
      aktarmaTipi: new FormControl("", [Validators.maxLength(20)]),
    })),
      (this.satirForm = this._fb.group({
        satirArry: this._fb.array([this.getSatir()]),
      }));
      (this.ihracatForm = this._fb.group({
        ihracatArry: this._fb.array([this.getIhracat()]),
      })),
      (this.ulkeForm = this._fb.group({
        ulkeArry: this._fb.array([this.getUlke()]),
      })),
      (this.satirEsyaForm = this._fb.group({
        satirEsyaArry: this._fb.array([this.getSatirEsya()]),
      }));
  }
  get focus() {
    return this.senetForm.controls;
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
      this.router.navigateByUrl("/app/ozetbeyan");
    }
    this.getSenetler(this._beyanSession.islemInternalNo);

    this._beyannameNo.nativeElement.focus();
    this.selectionList.selectionChange.subscribe(
      (s: MatSelectionListChange) => {
        this.selectionList.deselectAll();
        s.option.selected = true;
      }
    );
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
    if ((this.tasimaSenetInternalNo=='' || this.tasimaSenetInternalNo=='Boş' || this.tasimaSenetInternalNo===null || this.tasimaSenetInternalNo==='undefined') && ( this.beyanStatu === "Olusturuldu" || this.beyanStatu === "Güncellendi"))
      return true;
     else
       return false;
   
  }
  disableItem() {
    this.senetForm.disable();
    this.satirForm.disable();
     this.ihracatForm.disable();
     this.ulkeForm.disable();
  }
  enableItem() {
    this.senetForm.enable();
    this.satirForm.enable();
    this.ihracatForm.enable();
    this.ulkeForm.enable();
  }
  resetItem() {

    this.tasimaSenetInternalNo = "Boş";
    this.tasimaSatirInternalNo = "Boş";
    this.senetSiraNo = 0;
    this.sentNo="";

    this.senetForm.reset();
    this.satirForm.reset();
    this.ihracatForm.reset();
    this.ulkeForm.reset();

    const formSatirArray = this.satirForm.get("satirArry") as FormArray;
    formSatirArray.clear();
    this.satirForm.setControl("satirArry", formSatirArray);

    const formIhracatArray = this.ihracatForm.get("ihracatArry") as FormArray;
    formIhracatArray.clear();
    this.ihracatForm.setControl("ihracatArry", formIhracatArray);

    const formUlekArray = this.ulkeForm.get("ulkeArry") as FormArray;
    formUlekArray.clear();
    this.ulkeForm.setControl("ulkeArry", formUlekArray);
  }
  getSenetler(islemInternalNo: string) {
    this.beyanServis.getObTasimaSenet(islemInternalNo).subscribe(
      (result: ObTasimaSenetDto[]) => {
        this._senetler = result;
        this.disableItem();
        if (this._senetler.length > 0) this.getSenet(1);
        else this.resetItem();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getSenet(senetSiraNo) {
    this.tasimaSenetInternalNo = this._senetler[
      senetSiraNo - 1
    ].tasimaSenetInternalNo;
    this.senetSiraNo = this._senetler[senetSiraNo - 1].senetSiraNo;
    this.sentNo= this._senetler[senetSiraNo - 1].tasimaSenediNo;
    this.senetForm.setValue({
      ozetBeyanInternalNo: this._senetler[senetSiraNo - 1].ozetBeyanInternalNo,
      tasimaSenetInternalNo: this._senetler[senetSiraNo - 1]
        .tasimaSenetInternalNo,
      senetSiraNo: this._senetler[senetSiraNo - 1].senetSiraNo,
      tasimaSenediNo: this._senetler[senetSiraNo - 1].tasimaSenediNo,
      acentaAdi: this._senetler[senetSiraNo - 1].acentaAdi,
      acentaVergiNo: this._senetler[senetSiraNo - 1].acentaVergiNo,
      aliciAdi: this._senetler[senetSiraNo - 1].aliciAdi,
      aliciVergiNo: this._senetler[senetSiraNo - 1].aliciVergiNo,
      ambarHarici:
        this._senetler[senetSiraNo - 1].ambarHarici === "EVET" ? true : false,
      bildirimTarafiAdi: this._senetler[senetSiraNo - 1].bildirimTarafiAdi,
      bildirimTarafiVergiNo: this._senetler[senetSiraNo - 1]
        .bildirimTarafiVergiNo,
      duzenlendigiUlke: this._senetler[senetSiraNo - 1].duzenlendigiUlke,
      emniyetGuvenlik:
        this._senetler[senetSiraNo - 1].emniyetGuvenlik === "EVET"
          ? true
          : false,
      esyaninBulunduguYer: this._senetler[senetSiraNo - 1].esyaninBulunduguYer,
      faturaDoviz: this._senetler[senetSiraNo - 1].faturaDoviz,
      faturaToplami: this._senetler[senetSiraNo - 1].faturaToplami,
      gondericiAdi: this._senetler[senetSiraNo - 1].gondericiAdi,
      gondericiVergiNo: this._senetler[senetSiraNo - 1].gondericiVergiNo,
      grup: this._senetler[senetSiraNo - 1].grup === "EVET" ? true : false,
      konteyner:
        this._senetler[senetSiraNo - 1].konteyner === "EVET" ? true : false,
      navlunDoviz: this._senetler[senetSiraNo - 1].navlunDoviz,
      navlunTutari: this._senetler[senetSiraNo - 1].navlunTutari,
      odemeSekli: this._senetler[senetSiraNo - 1].odemeSekli,
      oncekiSeferNumarasi: this._senetler[senetSiraNo - 1].oncekiSeferNumarasi,
      oncekiSeferTarihi: this._senetler[senetSiraNo - 1].oncekiSeferTarihi,
      ozetBeyanNo: this._senetler[senetSiraNo - 1].ozetBeyanNo,
      roro: this._senetler[senetSiraNo - 1].roro === "EVET" ? true : false,
      aktarmaYapilacak:
        this._senetler[senetSiraNo - 1].aktarmaYapilacak === "EVET"
          ? true
          : false,
      aktarmaTipi: this._senetler[senetSiraNo - 1].aktarmaTipi,
    });

    this.beyanServis
      .getObTasimaSatir(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: ObTasimaSatirDto[]) => {
          this._satirlar = result.filter(
            (x) =>
              x.tasimaSenetInternalNo ===
              this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
          );
          this.initSatirFormArray(this._satirlar);
          this.satirForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    this.beyanServis.getObIhracat(this._beyanSession.islemInternalNo).subscribe(
      (result: ObIhracatDto[]) => {
        this._ihracatlar = result.filter(
          (x) =>
            x.tasimaSenetInternalNo === this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
        );
        this.initIhracatFormArray(this._ihracatlar);
        this.ihracatForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getObUgrakUlke(this._beyanSession.islemInternalNo).subscribe(
      (result: ObUgrakUlkeDto[]) => {
        this._ulkeler = result.filter(
          (x) =>
            x.tasimaSenetInternalNo === this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
        );
        this.initUlkeFormArray(this._ulkeler);
        this.ulkeForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.senetForm.disable();
  }

  yukleSenetler() {
    this.getSenetler(this._beyanSession.islemInternalNo);
  }

  yeniSenet() {
    this.tasimaSenetInternalNo = "Boş";
    this.tasimaSatirInternalNo = "Boş";
    this.senetSiraNo = 0;
    this.sentNo="";
    this.enableItem();
    this.resetItem();
    this.senetForm.markAllAsTouched();
    this.satirForm.markAllAsTouched();
    this.ulkeForm.markAllAsTouched();
    this.ihracatForm.markAllAsTouched();
  }

  duzeltSenet() {
    this.enableItem();
    this.senetForm.markAllAsTouched();
    this.satirForm.markAllAsTouched();
    this.ihracatForm.markAllAsTouched();
    this.ulkeForm.markAllAsTouched();
  }

  silSenet(tasimaSenetInternalNo: string,tasimaSenetNo:string) {
    if (
      confirm(
        tasimaSenetNo + "- Taşıma Senedini Silmek İstediğinizden Eminmisiniz?"
      )
    ) {
      const promise = this.beyanServis
        .removeObTasimaSenet(tasimaSenetInternalNo, this._beyanSession.ozetBeyanInternalNo)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.disableItem();
          this.resetItem();
          this.yukleSenetler();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }

  onsenetFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.senetForm.invalid) {
      const invalid = [];
      const controls = this.senetForm.controls;
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
  
    this.senetForm
      .get("ozetBeyanInternalNo")
      .setValue(this._beyanSession.ozetBeyanInternalNo);
    
    this.senetForm.get("senetSiraNo").setValue(this.senetSiraNo);
    this.senetForm
      .get("tasimaSenetInternalNo")
      .setValue(this.tasimaSenetInternalNo);

    let faturaToplami = this.senetForm.get("faturaToplami").value;
    this.senetForm
      .get("faturaToplami")
      .setValue(
        typeof faturaToplami == "string"
          ? parseFloat(faturaToplami)
          : faturaToplami
      );

    let navlunTutari = this.senetForm.get("navlunTutari").value;
    this.senetForm
      .get("navlunTutari")
      .setValue(
        typeof navlunTutari == "string"
          ? parseFloat(navlunTutari)
          : navlunTutari
      );

    let ambarHarici =
      this.senetForm.get("ambarHarici").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("ambarHarici").setValue(ambarHarici);

    let emniyetGuvenlik =
      this.senetForm.get("emniyetGuvenlik").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("emniyetGuvenlik").setValue(emniyetGuvenlik);

    let grup = this.senetForm.get("grup").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("grup").setValue(grup);

    let roro = this.senetForm.get("roro").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("roro").setValue(roro);

    let konteyner =
      this.senetForm.get("konteyner").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("konteyner").setValue(konteyner);

    let aktarmaYapilacak =
      this.senetForm.get("aktarmaYapilacak").value === true ? "EVET" : "HAYIR";
    this.senetForm.get("aktarmaYapilacak").setValue(aktarmaYapilacak);

    let yenitasimaSenetInternalNo: string;
    let yeniKalem = new ObTasimaSenetDto();
   
    yeniKalem.init(this.senetForm.value);
   
    const promiseKalem = this.beyanServis
      .restoreObTasimaSenet(yeniKalem)
      .toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kalemServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenitasimaSenetInternalNo = kalemServisSonuc.ReferansNo;

        if (yenitasimaSenetInternalNo != null) {
          this.tasimaSenetInternalNo = yenitasimaSenetInternalNo;
          this.setSatir();
           this.setIhracat();
           this.setUlke();        
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");               
          this.disableItem();
          this.yukleSenetler();
        }
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
  gtipLeave(gtip) {
    console.log("gtip leave:" + gtip);
  }

  

//#region Satır

initSatirFormArray(satir: ObTasimaSatirDto[]) {
  const formArray = this.satirForm.get("satirArry") as FormArray;
  formArray.clear();
  for (let klm of satir) {
    let formGroup: FormGroup = new FormGroup({
      brutAgirlik: new FormControl(klm.brutAgirlik, [
        ValidationService.decimalValidation,
      ]),
      kapAdedi: new FormControl(klm.kapAdedi, [
        ValidationService.numberValidator,
      ]),
      kapCinsi: new FormControl(klm.kapCinsi, [Validators.maxLength(9)]),
      konteynerTipi: new FormControl(klm.konteynerTipi, [
        Validators.maxLength(9),
      ]),
      markaNo: new FormControl(klm.markaNo, [Validators.maxLength(60)]),
      muhurNumarasi: new FormControl(klm.muhurNumarasi, [
        Validators.maxLength(35),
      ]),
      netAgirlik: new FormControl(klm.netAgirlik, [
        ValidationService.decimalValidation,
      ]),
      olcuBirimi: new FormControl(klm.olcuBirimi, [Validators.maxLength(9)]),
      satirNo: new FormControl(klm.satirNo, [
        ValidationService.numberValidator,
      ]),
      konteynerYukDurumu: new FormControl("", [Validators.maxLength(9)]),
      ozetBeyanInternalNo: new FormControl(
        klm.ozetBeyanInternalNo,
        [Validators.required]
      ),
      tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo, [
        Validators.required,
      ]),
      tasimaSatirInternalNo: new FormControl(klm.tasimaSatirInternalNo, [
        Validators.required,
      ]),
    });

    formArray.push(formGroup);
  }
  this.satirForm.setControl("satirArry", formArray);
}

getSatir() {
  return this._fb.group({
    brutAgirlik: new FormControl(0, [ValidationService.decimalValidation]),
    kapAdedi: new FormControl(0, [ValidationService.numberValidator]),
    kapCinsi: new FormControl("", [Validators.maxLength(9)]),
    konteynerTipi: new FormControl("", [Validators.maxLength(9)]),
    markaNo: new FormControl("", [Validators.maxLength(60)]),
    muhurNumarasi: new FormControl("", [Validators.maxLength(35)]),
    netAgirlik: new FormControl(0, [ValidationService.decimalValidation]),
    olcuBirimi: new FormControl("", [Validators.maxLength(9)]),
    satirNo: new FormControl(0, [ValidationService.numberValidator]),
    konteynerYukDurumu: new FormControl("", [Validators.maxLength(9)]),
    ozetBeyanInternalNo: new FormControl(this._beyanSession.ozetBeyanInternalNo, [
      Validators.required,
    ]),
    tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
      Validators.required,
    ]),
    tasimaSatirInternalNo: new FormControl(this.tasimaSatirInternalNo, [
      Validators.required,
    ]),
  });
}

get satirBilgileri() {
  return this.satirForm.get("satirArry") as FormArray;
}

addSatirField() {
  this.satirBilgileri.push(this.getSatir());
}

deleteSatirField(index: number) {
  this.satirBilgileri.removeAt(index);
}

setSatir() {
  if (this.satirBilgileri.length > 0) {
    for (let klm of this.satirBilgileri.value) {
      klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
      klm.tasimaSatirInternalNo = this.tasimaSatirInternalNo;
      klm.brutAgirlik =
        typeof klm.brutAgirlik == "string"
          ? parseFloat(klm.brutAgirlik)
          : klm.brutAgirlik;
      klm.kapAdedi =
        typeof klm.kapAdedi == "string"
          ? parseFloat(klm.kapAdedi)
          : klm.kapAdedi;

      klm.netAgirlik =
        typeof klm.netAgirlik == "string"
          ? parseFloat(klm.netAgirlik)
          : klm.netAgirlik;

      klm.satirNo =
        typeof klm.satirNo == "string"
          ? parseFloat(klm.satirNo)
          : klm.satirNo;
    }
    this.initSatirFormArray(this.satirBilgileri.value);

    if (this.satirBilgileri.invalid) {
      const invalid = [];
      const controls = this.satirBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Taşıma Satır Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );
        return;
      }
    }
  }

  if (this.satirBilgileri.length >= 0) {
    const promiseSatir = this.beyanServis
      .restoreObTasimaSatir(
        this.satirBilgileri.value,
        this.tasimaSenetInternalNo,
        this._beyanSession.ozetBeyanInternalNo
      )
      .toPromise();
    promiseSatir.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.ihracatForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Satır
//#region ihracat

initIhracatFormArray(ihracat: ObIhracatDto[]) {
  const formArray = this.ihracatForm.get("ihracatArry") as FormArray;
  formArray.clear();
  for (let klm of ihracat) {
    let formGroup: FormGroup = new FormGroup({
       brutAgirlik: new FormControl(klm.brutAgirlik, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      kapAdet: new FormControl(klm.kapAdet, [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      numara: new FormControl(klm.numara, [ Validators.required,Validators.maxLength(20)]),
      parcali: new FormControl(klm.parcali, [ Validators.required,Validators.maxLength(9)]),
      tip: new FormControl(klm.tip, [ Validators.required,Validators.maxLength(9)]),
      ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
      tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
    });

    formArray.push(formGroup);
  }
  this.ihracatForm.setControl("ihracatArry", formArray);
}

getIhracat() {
  return this._fb.group({
    brutAgirlik: new FormControl(0, [
      Validators.required,
      ValidationService.decimalValidation,
    ]),
    kapAdet: new FormControl(0, [
      Validators.required,
      ValidationService.numberValidator,
    ]),
    numara: new FormControl("", [ Validators.required,Validators.maxLength(20)]),
    parcali: new FormControl("", [ Validators.required,Validators.maxLength(9)]),
    tip: new FormControl("", [ Validators.required,Validators.maxLength(9)]),
    ozetBeyanInternalNo: new FormControl(this._beyanSession.ozetBeyanInternalNo, [
      Validators.required,
    ]),
    tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
      Validators.required,
    ]),
  });
}

get ihracatBilgileri() {
  return this.ihracatForm.get("ihracatArry") as FormArray;
}

addIhracatField() {
  this.ihracatBilgileri.push(this.getIhracat());
}

deleteIhracatField(index: number) {
  this.ihracatBilgileri.removeAt(index);
}

setIhracat() {
  if (this.ihracatBilgileri.length > 0) {
    for (let klm of this.ihracatBilgileri.value) {
      klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
      klm.brutAgirlik =
        typeof klm.brutAgirlik == "string"
          ? parseFloat(klm.brutAgirlik)
          : klm.brutAgirlik;

          klm.kapAdet =
          typeof klm.kapAdet == "string"
            ? parseFloat(klm.kapAdet)
            : klm.kapAdet;
    }

    this.initIhracatFormArray(this.ihracatBilgileri.value);

    if (this.ihracatBilgileri.invalid) {
      const invalid = [];
      const controls = this.ihracatBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n İhracat Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }
console.log(this.ihracatBilgileri.value);
  if (this.ihracatBilgileri.length >= 0) {
    const promiseIhracat = this.beyanServis
      .restoreObIhracat(
        this.ihracatBilgileri.value,
        this.tasimaSenetInternalNo,
        this._beyanSession.ozetBeyanInternalNo
      )
      .toPromise();
      promiseIhracat.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.ihracatForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion İhracat
//#region Ulke

initUlkeFormArray(ulke: ObUgrakUlkeDto[]) {
  const formArray = this.ulkeForm.get("ulkeArry") as FormArray;
  formArray.clear();
  for (let klm of ulke) {
    let formGroup: FormGroup = new FormGroup({
      limanYerAdi: new FormControl(klm.limanYerAdi, [
        Validators.required,
        Validators.maxLength(30),
      ]),
      ulkeKodu: new FormControl(klm.ulkeKodu, [
        Validators.required,
        Validators.maxLength(9),
      ]),
      ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
      tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
    });

    formArray.push(formGroup);
  }
  this.ulkeForm.setControl("ulkeArry", formArray);
}

getUlke() {
  return this._fb.group({
    limanYerAdi: new FormControl("", [
      Validators.required,
      Validators.maxLength(30),
    ]),
    ulkeKodu: new FormControl("", [
      Validators.required,
      Validators.maxLength(9),
    ]),
    ozetBeyanInternalNo: new FormControl(this._beyanSession.ozetBeyanInternalNo, [
      Validators.required,
    ]),
    tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
      Validators.required,
    ]),
  });
}

get ulkeBilgileri() {
  return this.ulkeForm.get("ulkeArry") as FormArray;
}

addUlkeField() {
  this.ulkeBilgileri.push(this.getUlke());
}

deleteUlkeField(index: number) {
  // if (this.odemeBilgileri.length !== 1) {
  this.ulkeBilgileri.removeAt(index);
}

setUlke() {
  if (this.ulkeBilgileri.length > 0) {
    for (let klm of this.ulkeBilgileri.value) {
      klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
    }
    this.initUlkeFormArray(this.ulkeBilgileri.value);

    if (this.ulkeBilgileri.invalid) {
      const invalid = [];
      const controls = this.ulkeBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Ülke Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );
        return;
      }
    }
  }

  if (this.ulkeBilgileri.length >= 0) {
    const promiseUlke = this.beyanServis
      .restoreObUgrakUlke(
        this.ulkeBilgileri.value,
        this.tasimaSenetInternalNo,
        this._beyanSession.ozetBeyanInternalNo
      )
      .toPromise();
      promiseUlke.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.ihracatForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Ulke

//#region Eşya

get SatirStatu(): boolean {
  
  if (this.beyanStatu === "undefined" || this.beyanStatu === null)
    return false;
  if ( this.tasimaSatirInternalNo!=='Boş' && this.tasimaSatirInternalNo!==null && this.tasimaSatirInternalNo!=='' && this.tasimaSenetInternalNo!=='undefined' && ( this.beyanStatu === "Olusturuldu" || this.beyanStatu === "Güncellendi"))
    return true;
  else return false;
}
getSatirEsyaBilgileri(content, index:number) {
  console.log( this.satirBilgileri.value);
  this.tasimaSatirInternalNo = this.satirBilgileri.controls[index].get("tasimaSatirInternalNo").value;  
  this.satirNo=this.satirBilgileri.controls[index].get("satirNo").value;
  this.beyanServis.getObSatirEsya(this._beyanSession.islemInternalNo).subscribe(
    (result: ObSatirEsyaDto[]) => {
     
      this._esyalar = result.filter(
        (x) =>
          x.tasimaSatirInternalNo === this.tasimaSatirInternalNo
      );
      this.initSatirEsyaFormArray(this._esyalar);
     this.satirEsyaForm.disable();
    
    },
    (err) => {
      this.beyanServis.errorHandel(err);
    }
  );
  this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
    this.closeResult = `Closed with: ${result}`;
  }, (reason) => {
    this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
  });

}

private getDismissReason(reason: any): string {
  console.log(reason);
  if (reason === ModalDismissReasons.ESC) {
    return 'by pressing ESC';
  } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
    return 'by clicking on a backdrop';
  } else if (reason === "Save Click") {
    this.setSatirEsya();
  } else {
         return  `with: ${reason}`;
  }
 
}

esyaSatirIslemOzetBeyan()
{
  this.satirEsyaForm.enable();
}

initSatirEsyaFormArray(tasimaSatirEsya: ObSatirEsyaDto[]) {
  const formArray = this.satirEsyaForm.get("satirEsyaArry") as FormArray;
  formArray.clear();
  for (let klm of tasimaSatirEsya) {
    let formGroup: FormGroup = new FormGroup({
      bmEsyaKodu: new FormControl(klm.bmEsyaKodu, [
        Validators.maxLength(15),
      ]),    
      esyaKodu: new FormControl(klm.esyaKodu, [
         Validators.maxLength(12),
      ]),   
      esyaninTanimi: new FormControl(klm.esyaninTanimi, [
        Validators.maxLength(150),
      ]),
      brutAgirlik: new FormControl(klm.brutAgirlik, [
         ValidationService.decimalValidation
      ]),     
      netAgirlik: new FormControl(klm.netAgirlik, [
        ValidationService.decimalValidation
     ]),     
      kalemFiyati: new FormControl(klm.kalemFiyati, [
         ValidationService.decimalValidation
      ]),   
      kalemFiyatiDoviz: new FormControl(klm.kalemFiyatiDoviz, [
        Validators.maxLength(9),
      ]), 
      kalemSiraNo: new FormControl(klm.kalemSiraNo, [
        ValidationService.numberValidator
      ]),  
      olcuBirimi: new FormControl(klm.olcuBirimi, [
        Validators.maxLength(9),
      ]),
      ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo, [
        Validators.required,
      ]),
      tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo, [
        Validators.required,
      ]),
      tasimaSatirInternalNo: new FormControl(klm.tasimaSatirInternalNo, [
        Validators.required,
      ]),
    });

    formArray.push(formGroup);
  }
  this.satirEsyaForm.setControl("satirEsyaArry", formArray);

}

getSatirEsya() {
  return this._fb.group({     
    bmEsyaKodu: new FormControl("", [
      Validators.maxLength(15),
    ]),    
    esyaKodu: new FormControl("", [
       Validators.maxLength(12),
    ]),   
    esyaninTanimi: new FormControl("", [
      Validators.maxLength(150),
    ]),
    brutAgirlik: new FormControl(0, [
       ValidationService.decimalValidation
    ]),     
    netAgirlik: new FormControl(0, [
      ValidationService.decimalValidation
   ]),     
    kalemFiyati: new FormControl(0, [
       ValidationService.decimalValidation
    ]),   
    kalemFiyatiDoviz: new FormControl("", [
      Validators.maxLength(9),
    ]), 
    kalemSiraNo: new FormControl(0, [
      ValidationService.numberValidator
    ]),  
    olcuBirimi: new FormControl(0, [
      Validators.maxLength(9),
    ]),
    ozetBeyanInternalNo: new FormControl(this._beyanSession.ozetBeyanInternalNo, [
      Validators.required,
    ]),
    tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
      Validators.required,
    ]),
    tasimaSatirInternalNo: new FormControl(this.tasimaSatirInternalNo, [
      Validators.required,
    ]),
  });
}

get satirEsyaBilgileri() {
  return this.satirEsyaForm.get("satirEsyaArry") as FormArray;
}

addSatirEsyaField() {
  this.satirEsyaBilgileri.push(this.getSatirEsya());
}

deleteSatirEsyaField(index: number) {
  this.satirEsyaBilgileri.removeAt(index);
}

setSatirEsya() {
  if (this.satirEsyaBilgileri.length > 0) {
    for (let klm of this.satirEsyaBilgileri.value) {
      klm.ozetBeyanInternalNo = this.ozetBeyanInternalNo;
      klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
      klm.miktar =
        typeof klm.miktar == "string"
          ? parseInt(klm.miktar)
          : klm.miktar;

    
    }
    this.initSatirEsyaFormArray(this.satirEsyaBilgileri.value);
  
    if (this.satirEsyaBilgileri.invalid) {
      const invalid = [];
      const controls = this.satirEsyaBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Taşıma Satırı Eşya Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );
        return;
      }
    }
  }
 
  if (this.satirEsyaBilgileri.length >= 0) {
    const promiseEsya = this.beyanServis
      .restoreObSatirEsya(
        this.satirEsyaBilgileri.value,
        this.tasimaSatirInternalNo,
        this.tasimaSenetInternalNo,
        this._beyanSession.ozetBeyanInternalNo
      )
      .toPromise();
      promiseEsya.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
         this.satirEsyaForm.disable();
         this.satirEsyaForm.reset();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}

//#endregion Eşya
  onReset() {
    this.submitted = false;
  }
  
}
