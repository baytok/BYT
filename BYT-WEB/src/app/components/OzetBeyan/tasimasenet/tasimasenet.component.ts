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
} from "../../../../shared/helpers/referencesList";
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
  OdemeDto,
  KonteynerDto,
  MarkaDto, 
  ServisDto,
 } from "../../../../shared/service-proxies/service-proxies";

import {
  NativeDateAdapter,
  DateAdapter,
  MAT_DATE_FORMATS,
} from "@angular/material/core";


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
  selector: 'app-tasimasenet',
  templateUrl: './tasimasenet.component.html',
  styleUrls: ['./tasimasenet.component.css'],
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
  senetForm: FormGroup;
  odemeForm: FormGroup;
  konteynerForm: FormGroup; 
  satirForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.ozetBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu; 
  _beyannameBilgileri: BeyannameBilgileriDto;
  _senetler: ObTasimaSenetDto[]; 
  _satirlar: ObTasimaSatirDto[];
  _odemeler: OdemeDto[];
  _konteynerler: KonteynerDto[];
  _ulkeList = this.referansService.getUlkeJSON();
  _dovizList = this.referansService.getdovizCinsiJSON(); 
  _cinsList = this.referansService.getkapCinsiJSON();
  _olcuList = this.referansService.getolcuJSON(); 
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
  
  ) {
     (this.senetForm = this._fb.group({
   
      ozetBeyanInternalNo: [],
      tasimaSenetInternalNo: [],
      senetSiraNo: [],
      tasimaSenediNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$")
      ]),
      acentaAdi: new FormControl("", [Validators.maxLength(150)]),
      acentaVergiNo: new FormControl("", [Validators.maxLength(20)]),
      aliciAdi: new FormControl("", [Validators.maxLength(150)]),
      aliciVergiNo: new FormControl("", [Validators.maxLength(20)]),
      ambarHarici:[false],
      bildirimTarafiAdi: new FormControl("", [Validators.maxLength(150)]),
      bildirimTarafiVergiNo: new FormControl("", [Validators.maxLength(20)]),
      duzenlendigiUlke: new FormControl("", [Validators.required, Validators.maxLength(9)]),
      emniyetGuvenlik:[false],
      esyaninBulunduguYer: new FormControl("", [Validators.maxLength(16)]),
      faturaDoviz: new FormControl("", [Validators.maxLength(9)]),
      faturaToplami: new FormControl("", [
          ValidationService.decimalValidation,
      ]),
      gondericiAdi: new FormControl("", [Validators.maxLength(150)]),
      gondericiVergiNo: new FormControl("", [Validators.maxLength(20)]),
      grup:[false],
      konteyner:[false],
      navlunDoviz: new FormControl("", [Validators.maxLength(9)]),
      navlunTutari: new FormControl("", [
        ValidationService.decimalValidation,
       ]),
      odemeSekli: new FormControl("", [Validators.maxLength(9)]),
      oncekiSeferNumarasi: new FormControl("", [Validators.maxLength(20)]),
      oncekiSeferTarihi: new FormControl("", [Validators.maxLength(12), ValidationService.tarihValidation]),
      ozetBeyanNo: new FormControl("", [Validators.maxLength(20)]),
      roro:[false],
      aktarmaYapilacak:[false],
      aktarmaTipi: new FormControl("", [Validators.maxLength(20)]),

      }))
      ,
       (this.satirForm = this._fb.group({
         satirArry: this._fb.array([this.getSatir()]),
       }))  
      // (this.odemeForm = this._fb.group({
      //   odemeArry: this._fb.array([this.getOdeme()]),
      // })),
      // (this.konteynerForm = this._fb.group({
      //   konteynerArry: this._fb.array([this.getKonteyner()]),
      // }))
     
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
    if (this.beyanStatu === 'Olusturuldu' || this.beyanStatu === 'Güncellendi')
     return true;
    else return false;
  }
  disableItem() {
    this.senetForm.disable();
    this.satirForm.disable();
    // this.odemeForm.disable();
    // this.konteynerForm.disable();  
   
   
  }
  enableItem() {
    this.senetForm.enable();
    this.satirForm.enable();
    // this.odemeForm.enable();
    // this.konteynerForm.enable(); 
  
   
  }
  resetItem() {
    this.senetForm.reset();
    this.satirForm.reset();
    // this.odemeForm.reset();
    // this.konteynerForm.reset();
  
  
     const formSatirArray = this.satirForm.get("satirArry") as FormArray;
     formSatirArray.clear();
     this.satirForm.setControl("satirArry", formSatirArray);

     
    // const formOdemeArray = this.odemeForm.get("odemeArry") as FormArray;
    // formOdemeArray.clear();
    // this.odemeForm.setControl("odemeArry", formOdemeArray);

    // const formKonteynerArray = this.konteynerForm.get(
    //   "konteynerArry"
    // ) as FormArray;
    // formKonteynerArray.clear();
    // this.konteynerForm.setControl("konteynerArry", formKonteynerArray);


  


  }
  getSenetler(islemInternalNo: string) {
    this.beyanServis.getObTasimaSenet(islemInternalNo).subscribe(
      (result: ObTasimaSenetDto[]) => {
        this._senetler = result;
        this.disableItem();
        if(this._senetler.length>0)
        this.getSenet(1);
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
   
    this.tasimaSenetInternalNo = this._senetler[senetSiraNo - 1].tasimaSenetInternalNo;
    this.senetSiraNo = this._senetler[senetSiraNo - 1].senetSiraNo;
      this.senetForm.setValue({
      ozetBeyanInternalNo: this._senetler[senetSiraNo - 1].ozetBeyanInternalNo,
      tasimaSenetInternalNo: this._senetler[senetSiraNo - 1].tasimaSenetInternalNo,
      senetSiraNo: this._senetler[senetSiraNo - 1].senetSiraNo,
      tasimaSenediNo:this._senetler[senetSiraNo - 1].tasimaSenediNo,
      acentaAdi:this._senetler[senetSiraNo - 1].acentaAdi ,
      acentaVergiNo:this._senetler[senetSiraNo - 1].acentaVergiNo ,
      aliciAdi:this._senetler[senetSiraNo - 1].aliciAdi ,
      aliciVergiNo:this._senetler[senetSiraNo - 1].aliciVergiNo ,
      ambarHarici:this._senetler[senetSiraNo - 1].ambarHarici=== "EVET" ? true : false,
      bildirimTarafiAdi: this._senetler[senetSiraNo - 1].bildirimTarafiAdi,
      bildirimTarafiVergiNo:  this._senetler[senetSiraNo - 1].bildirimTarafiVergiNo,
      duzenlendigiUlke: this._senetler[senetSiraNo - 1].duzenlendigiUlke,
      emniyetGuvenlik:this._senetler[senetSiraNo - 1].emniyetGuvenlik=== "EVET" ? true : false,
      esyaninBulunduguYer: this._senetler[senetSiraNo - 1].esyaninBulunduguYer,
      faturaDoviz: this._senetler[senetSiraNo - 1].faturaDoviz,
      faturaToplami: this._senetler[senetSiraNo - 1].faturaToplami,
      gondericiAdi: this._senetler[senetSiraNo - 1].gondericiAdi,
      gondericiVergiNo: this._senetler[senetSiraNo - 1].gondericiVergiNo,
      grup:this._senetler[senetSiraNo - 1].grup=== "EVET" ? true : false,
      konteyner:this._senetler[senetSiraNo - 1].konteyner=== "EVET" ? true : false,
      navlunDoviz: this._senetler[senetSiraNo - 1].navlunDoviz,
      navlunTutari: this._senetler[senetSiraNo - 1].navlunTutari,
      odemeSekli:this._senetler[senetSiraNo - 1].odemeSekli,
      oncekiSeferNumarasi: this._senetler[senetSiraNo - 1].oncekiSeferNumarasi,
      oncekiSeferTarihi: this._senetler[senetSiraNo - 1].oncekiSeferTarihi,
      ozetBeyanNo: this._senetler[senetSiraNo - 1].ozetBeyanNo,
      roro:this._senetler[senetSiraNo - 1].roro=== "EVET" ? true : false,
      aktarmaYapilacak:this._senetler[senetSiraNo - 1].aktarmaYapilacak=== "EVET" ? true : false,
      aktarmaTipi: this._senetler[senetSiraNo - 1].aktarmaTipi,
     
     
    });
    
      this.beyanServis.getObTasimaSatir(this._beyanSession.islemInternalNo).subscribe(
      (result: ObTasimaSatirDto[]) => {
        this._satirlar = result.filter(
          (x) =>
            x.tasimaSenetInternalNo === this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
        );
        this.initSatirFormArray(this._satirlar);
        this.satirForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      } );
    // this.beyanServis.getOdeme(this._beyanSession.islemInternalNo).subscribe(
    //   (result: OdemeDto[]) => {
    //     this._odemeler = result.filter(
    //       (x) =>
    //         x.tasimaSenetInternalNo === this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
    //     );
    //     this.initOdemeFormArray(this._odemeler);
    //     this.odemeForm.disable();
    //   },
    //   (err) => {
    //     this.beyanServis.errorHandel(err);
    //   }
    // );

    // this.beyanServis.getKonteyner(this._beyanSession.islemInternalNo).subscribe(
    //   (result: KonteynerDto[]) => {
    //     this._konteynerler = result.filter(
    //       (x) =>
    //         x.tasimaSenetInternalNo === this._senetler[senetSiraNo - 1].tasimaSenetInternalNo
    //     );
    //     this.initKonteynerFormArray(this._konteynerler);
    //     this.konteynerForm.disable();
    //   },
    //   (err) => {
    //     this.beyanServis.errorHandel(err);
    //   }
    // );   

   

    this.senetForm.disable();
  }

  yukleSenetler() {
    this.getSenetler(this._beyanSession.islemInternalNo);
  }

  yeniSenet() {
    this.tasimaSenetInternalNo = "Boş";
    this.tasimaSatirInternalNo="Boş";
    this.senetSiraNo = 0;
    this.enableItem();
    this.resetItem();
    this.senetForm.markAllAsTouched();
    this.satirForm.markAllAsTouched();
  }

  duzeltSenet() {
    this.enableItem();
    this.senetForm.markAllAsTouched();
    this.satirForm.markAllAsTouched();
    this.odemeForm.markAllAsTouched();
    this.konteynerForm.markAllAsTouched();
   
  
  }

  silSenet(tasimaSenetInternalNo: string) {
    if (
      confirm(tasimaSenetInternalNo + "- kalemi Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeKalem(tasimaSenetInternalNo, this._beyanSession.beyanInternalNo)
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
      .get("beyanInternalNo")
      .setValue(this._beyanSession.beyanInternalNo);
    this.senetForm.get("senetSiraNo").setValue(this.senetSiraNo); 
    this.senetForm.get("tasimaSenetInternalNo").setValue(this.tasimaSenetInternalNo);
    let ikincilIslem =
      this.senetForm.get("ikincilIslem").value === true ? "EVET" : "";
    this.senetForm.get("ikincilIslem").setValue(ikincilIslem);

    let imalatciFirmaBilgisi =
      this.senetForm.get("imalatciFirmaBilgisi").value === true ? "EVET" : "";
    this.senetForm.get("imalatciFirmaBilgisi").setValue(imalatciFirmaBilgisi);

    let mahraceIade =
      this.senetForm.get("mahraceIade").value === true ? "EVET" : "";
    this.senetForm.get("mahraceIade").setValue(mahraceIade);

    let yenitasimaSenetInternalNo: string;
    let yeniKalem = new ObTasimaSenetDto();
    yeniKalem.init(this.senetForm.value);

    const promiseKalem = this.beyanServis.restoreObTasimaSenet(yeniKalem).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kalemServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenitasimaSenetInternalNo = kalemServisSonuc.ReferansNo;

        if (yenitasimaSenetInternalNo != null) {
          this.tasimaSenetInternalNo = yenitasimaSenetInternalNo;
          this.setSatir();
          // this.setOdeme();
          // this.setKonteyner();
        
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
  onReset() {
    this.submitted = false;
  }
  gtipLeave(gtip){
    console.log("gtip leave:"+gtip);
  }

//#region Satır
  initSatirFormArray(satir: ObTasimaSatirDto[]) {
    const formArray = this.satirForm.get("satirArry") as FormArray;
    formArray.clear();
    for (let klm of satir) {
      let formGroup: FormGroup = new FormGroup({
      brutAgirlik: new FormControl(klm.brutAgirlik, [ValidationService.decimalValidation,]),
      kapAdedi: new FormControl(klm.kapAdedi, [ValidationService.numberValidator,]),
      kapCinsi: new FormControl(klm.kapCinsi, [Validators.maxLength(9)]),
      konteynerTipi: new FormControl(klm.konteynerTipi, [Validators.maxLength(9),]),
      markaNo: new FormControl(klm.markaNo, [Validators.maxLength(60)]),
      muhurNumarasi: new FormControl(klm.muhurNumarasi, [Validators.maxLength(35)]),
      netAgirlik: new FormControl(klm.netAgirlik, [ValidationService.decimalValidation,]),
      olcuBirimi: new FormControl(klm.olcuBirimi, [Validators.maxLength(9)]),
      satirNo: new FormControl(klm.satirNo, [ValidationService.numberValidator]),
      konteynerYukDurumu: new FormControl("", [Validators.maxLength(9)]),
      ozetBeyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
        Validators.required,
      ]),
      tasimaSatirInternalNo: new FormControl(this.tasimaSatirInternalNo, [
        Validators.required,
      ])      
      });

      formArray.push(formGroup);
    }
    this.satirForm.setControl("satirArry", formArray);
  }

  getSatir() {
    return this._fb.group({
      brutAgirlik: new FormControl(0, [ValidationService.decimalValidation,]),
      kapAdedi: new FormControl(0, [ValidationService.numberValidator,]),
      kapCinsi: new FormControl("", [Validators.maxLength(9)]),
      konteynerTipi: new FormControl("", [Validators.maxLength(9),]),
      markaNo: new FormControl("", [Validators.maxLength(60)]),
      muhurNumarasi: new FormControl("", [Validators.maxLength(35)]),
      netAgirlik: new FormControl(0, [ValidationService.decimalValidation,]),
      olcuBirimi: new FormControl("", [Validators.maxLength(9)]),
      satirNo: new FormControl(0, [ValidationService.numberValidator]),
      konteynerYukDurumu: new FormControl("", [Validators.maxLength(9)]),
      ozetBeyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
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
        klm.silindirAdet =
          typeof klm.silindirAdet == "string"
            ? parseFloat(klm.silindirAdet)
            : klm.silindirAdet;

        klm.motorGucu =
          typeof klm.motorGucu == "string"
            ? parseFloat(klm.motorGucu)
            : klm.motorGucu;

        klm.markaKiymeti =
          typeof klm.markaKiymeti == "string"
            ? parseFloat(klm.markaKiymeti)
            : klm.markaKiymeti;
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
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseSatir.then(
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
 // #endregion Satır
  // //#region Ödeme Şekli

  // initOdemeFormArray(odeme: OdemeDto[]) {
  //   const formArray = this.odemeForm.get("odemeArry") as FormArray;
  //   formArray.clear();
  //   for (let klm of odeme) {
  //     let formGroup: FormGroup = new FormGroup({
  //       odemeSekliKodu: new FormControl(klm.odemeSekliKodu, [
  //         Validators.required,
  //       ]),
  //       odemeTutari: new FormControl(klm.odemeTutari, [
  //         Validators.required,
  //         Validators.maxLength(10),
  //         ValidationService.decimalValidation,
  //       ]),
  //       tbfid: new FormControl(klm.tbfid, [
  //         Validators.required,
  //         Validators.maxLength(30),
  //         Validators.pattern("^[a-zA-Z0-9]*$"),
  //       ]),
  //       beyanInternalNo: new FormControl(klm.beyanInternalNo),
  //       tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
  //     });

  //     formArray.push(formGroup);
  //   }
  //   this.odemeForm.setControl("odemeArry", formArray);
  // }

  // getOdeme() {
  //   return this._fb.group({
  //     odemeSekliKodu: new FormControl("", [Validators.required]),
  //     odemeTutari: new FormControl(0, [
  //       Validators.required,
  //       Validators.maxLength(10),
  //       ValidationService.decimalValidation,
  //     ]),
  //     tbfid: new FormControl("", [
  //       Validators.required,
  //       Validators.maxLength(30),
  //       Validators.pattern("^[a-zA-Z0-9]*$"),
  //     ]),
  //     beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
  //       Validators.required,
  //     ]),
  //     tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
  //       Validators.required,
  //     ]),
  //   });
  // }

  // get odemeBilgileri() {
  //   return this.odemeForm.get("odemeArry") as FormArray;
  // }

  // addOdemeField() {
  //   this.odemeBilgileri.push(this.getOdeme());
  // }

  // deleteOdemeField(index: number) {
  //   this.odemeBilgileri.removeAt(index);
  // }

  // setOdeme() {
  //   if (this.odemeBilgileri.length > 0) {
  //     for (let klm of this.odemeBilgileri.value) {
  //       klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
  //       klm.odemeTutari =
  //         typeof klm.odemeTutari == "string"
  //           ? parseFloat(klm.odemeTutari)
  //           : klm.odemeTutari;
  //     }

  //     this.initOdemeFormArray(this.odemeBilgileri.value);

  //     if (this.odemeBilgileri.invalid) {
  //       const invalid = [];
  //       const controls = this.odemeBilgileri.controls;

  //       for (const name in controls) {
  //         if (controls[name].invalid) {
  //           invalid.push(name);
  //         }
  //       }

  //       if (invalid.length > 0) {
  //         alert(
  //           "ERROR!! :-)\n\n Ödeme Şekli Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
  //             JSON.stringify(invalid, null, 4)
  //         );

  //         return;
  //       }
  //     }
  //   }

  //   if (this.odemeBilgileri.length >= 0) {
  //     const promiseOdeme = this.beyanServis
  //       .restoreOdeme(
  //         this.odemeBilgileri.value,
  //         this.tasimaSenetInternalNo,
  //         this._beyanSession.beyanInternalNo
  //       )
  //       .toPromise();
  //     promiseOdeme.then(
  //       (result) => {
  //         // const servisSonuc = new ServisDto();
  //         // servisSonuc.init(result);
  //         // this.odemeForm.disable();
  //       },
  //       (err) => {
  //         this.openSnackBar(err, "Tamam");
  //       }
  //     );
  //   }
  // }
  // //#endregion Ödeme
  // //#region Konteyner

  // initKonteynerFormArray(konteyner: KonteynerDto[]) {
  //   const formArray = this.konteynerForm.get("konteynerArry") as FormArray;
  //   formArray.clear();
  //   for (let klm of konteyner) {
  //     let formGroup: FormGroup = new FormGroup({
  //       konteynerNo: new FormControl(klm.konteynerNo, [
  //         Validators.required,
  //         Validators.maxLength(35),
  //       ]),
  //       ulkeKodu: new FormControl(klm.ulkeKodu, [
  //         Validators.required,
  //         Validators.maxLength(9),
  //       ]),
  //       beyanInternalNo: new FormControl(klm.beyanInternalNo),
  //       tasimaSenetInternalNo: new FormControl(klm.tasimaSenetInternalNo),
  //     });

  //     formArray.push(formGroup);
  //   }
  //   this.konteynerForm.setControl("konteynerArry", formArray);
  // }

  // getKonteyner() {
  //   return this._fb.group({
  //     konteynerNo: new FormControl("", [
  //       Validators.required,
  //       Validators.maxLength(35),
  //     ]),
  //     ulkeKodu: new FormControl("", [
  //       Validators.required,
  //       Validators.maxLength(9),
  //     ]),
  //     beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
  //       Validators.required,
  //     ]),
  //     tasimaSenetInternalNo: new FormControl(this.tasimaSenetInternalNo, [
  //       Validators.required,
  //     ]),
  //   });
  // }

  // get konteynerBilgileri() {
  //   return this.konteynerForm.get("konteynerArry") as FormArray;
  // }

  // addKonteynerField() {
  //   this.konteynerBilgileri.push(this.getKonteyner());
  // }

  // deleteKonteynerField(index: number) {
  //   // if (this.odemeBilgileri.length !== 1) {
  //   this.konteynerBilgileri.removeAt(index);
  // }

  // setKonteyner() {
  //   if (this.konteynerBilgileri.length > 0) {
  //     for (let klm of this.konteynerBilgileri.value) {
  //       klm.tasimaSenetInternalNo = this.tasimaSenetInternalNo;
  //     }
  //     this.initKonteynerFormArray(this.konteynerBilgileri.value);

  //     if (this.konteynerBilgileri.invalid) {
  //       const invalid = [];
  //       const controls = this.konteynerBilgileri.controls;

  //       for (const name in controls) {
  //         if (controls[name].invalid) {
  //           invalid.push(name);
  //         }
  //       }

  //       if (invalid.length > 0) {
  //         alert(
  //           "ERROR!! :-)\n\n Konteyner Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
  //             JSON.stringify(invalid, null, 4)
  //         );
  //         return;
  //       }
  //     }
  //   }

  //   if (this.konteynerBilgileri.length >= 0) {
  //     const promiseKonteyner = this.beyanServis
  //       .restoreKonteyner(
  //         this.konteynerBilgileri.value,
  //         this.tasimaSenetInternalNo,
  //         this._beyanSession.beyanInternalNo
  //       )
  //       .toPromise();
  //     promiseKonteyner.then(
  //       (result) => {
  //         // const servisSonuc = new ServisDto();
  //         // servisSonuc.init(result);
  //         // this.odemeForm.disable();
  //       },
  //       (err) => {
  //         this.openSnackBar(err, "Tamam");
  //       }
  //     );
  //   }
  // }
  // //#endregion Konteyner
 
  
}

