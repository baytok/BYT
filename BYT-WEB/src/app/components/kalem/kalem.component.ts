import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  Injector,
  ElementRef,
  Injectable,
} from "@angular/core";
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from "@angular/material/list";
import { Observable } from "rxjs";
import { map, filter } from "rxjs/operators";
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
  teslimSekli,
  dovizCinsi,
  kullanilmisEsya,
  girisCikisAmaci,
  anlasma,
  muafiyet,
  ozellik,
  kalemIsleminNiteligi,
  stmIlKod,
  cins,
  olcu,
  algilama,
  odeme,
  VergiKodu,
  BelgeKodu,
  SoruKodu,
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
import { ReferansService } from "../../../shared/helpers/ReferansService";
import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemDto,
  OdemeDto,
  KonteynerDto,
  TamamlayiciBilgiDto,
  MarkaDto,
  BeyannameAcmaDto,
  VergiDto,
  BelgeDto,
  SoruCevapDto,
  ServisDto,
  ReferansDto,
} from "../../../shared/service-proxies/service-proxies";

import {
  NativeDateAdapter,
  DateAdapter,
  MAT_DATE_FORMATS,
  MatDateFormats,
} from "@angular/material/core";
import { MatDatepickerModule } from "@angular/material/datepicker";

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
  selector: "app-kalem",
  templateUrl: "./kalem.component.html",
  styleUrls: ["./kalem.component.scss"],
  providers: [
    { provide: DateAdapter, useClass: PickDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS },
  ],
})
export class KalemComponent implements OnInit {
  public form: FormGroup;

  kalemInternalNo: string;
  kalemNo: number;
  kalemForm: FormGroup;
  odemeForm: FormGroup;
  konteynerForm: FormGroup;
  tamamlayiciForm: FormGroup;
  markaForm: FormGroup;
  beyannameAcmaForm: FormGroup;
  vergiForm: FormGroup;
  belgeForm: FormGroup;
  soruCevapForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _kalemler: KalemDto[];
  _odemeler: OdemeDto[];
  _konteynerler: KonteynerDto[];
  _tamamlayiciBilgiler: TamamlayiciBilgiDto[];
  _markalar: MarkaDto[];
  _acmalar: BeyannameAcmaDto[];
  _belgeler: BelgeDto[];
  _vergiler: VergiDto[];
  _soruCevaplar: SoruCevapDto[];
  _ulkeList = this.referansService.getUlkeJSON();
  _teslimList = this.referansService.getteslimSekliJSON();
  _dovizList = this.referansService.getdovizCinsiJSON();
  _kullanilmisList = kullanilmisEsya;
  _girisCikisAmaciList = girisCikisAmaci;
  _anlasmaList = this.referansService.getanlasmaJSON();
  _muafiyetList = this.referansService.getmuafiyetJSON();
  _ozellikList = ozellik;
  _isleminNiteligiList = kalemIsleminNiteligi;
  _stmIlKodList = stmIlKod;
  _cinsList = this.referansService.getkapCinsiJSON();
  _olcuList = this.referansService.getolcuJSON();
  _algilamaList = algilama;
  _vergiList = this.referansService.getvergiKoduJSON();
  _belgeList = this.referansService.getbelgeKoduJSON();
  _soruList = SoruKodu;
  _odemeList = this.referansService.getodemeSekliJSON();
  @ViewChild("KalemList", { static: true })
  private selectionList: MatSelectionList;
  @ViewChild("BeyannameNo", { static: true }) private _beyannameNo: ElementRef;

  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router
  ) {
    (this.kalemForm = this._fb.group({
      //Genel Bilgiler
      //  tarifeTanimi:[],
      beyanInternalNo: [],
      kalemInternalNo: [],
      kalemSiraNo: [],
      gtip: new FormControl("", [
        Validators.required,
        Validators.maxLength(12),
        Validators.pattern("^[0-9]*$"),
      ]),
      aciklama44: new FormControl("", [Validators.maxLength(500)]),
      menseiUlke: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      girisCikisAmaci: new FormControl("", [Validators.maxLength(9)]),
      girisCikisAmaciAciklama: new FormControl("", [Validators.maxLength(300)]),
      uluslararasiAnlasma: new FormControl("", [Validators.maxLength(9)]),
      ikincilIslem: [false],
      imalatciFirmaBilgisi: [false],
      mahraceIade: [false],
      kalemIslemNiteligi: new FormControl("", [Validators.maxLength(9)]),
      kullanilmisEsya: new FormControl("", [Validators.maxLength(9)]),
      ozellik: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      muafiyetler1: new FormControl("", [Validators.maxLength(9)]),
      muafiyetler2: new FormControl("", [Validators.maxLength(9)]),
      muafiyetler3: new FormControl("", [Validators.maxLength(9)]),
      muafiyetler4: new FormControl("", [Validators.maxLength(9)]),
      muafiyetler5: new FormControl("", [Validators.maxLength(9)]),
      imalatciVergiNo: new FormControl("", [Validators.maxLength(15)]),
      muafiyetAciklamasi: new FormControl("", [Validators.maxLength(500)]),
      stmIlKodu: new FormControl("", [Validators.maxLength(9)]),
      ticariTanimi: new FormControl("", [
        Validators.required,
        Validators.maxLength(350),
      ]),

      // Eşya Bilgileri
      // referansTarihi:[],
      cins: new FormControl("", [Validators.required, Validators.maxLength(9)]),
      ekKod: new FormControl("", [Validators.maxLength(9)]),
      adet: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      marka: new FormControl("", [
        Validators.required,
        ,
        Validators.maxLength(70),
      ]),
      miktar: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      miktarBirimi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      netAgirlik: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      brutAgirlik: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      numara: new FormControl("", [
        Validators.required,
        Validators.maxLength(70),
      ]),
      satirNo: new FormControl("", [Validators.maxLength(20)]),
      istatistikiMiktar: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      tamamlayiciOlcuBirimi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      algilamaBirimi1: new FormControl("", [Validators.maxLength(9)]),
      algilamaBirimi2: new FormControl("", [Validators.maxLength(9)]),
      algilamaBirimi3: new FormControl("", [Validators.maxLength(9)]),
      algilamaMiktari1: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      algilamaMiktari2: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      algilamaMiktari3: new FormControl("", [
        ValidationService.decimalValidation,
      ]),

      //Finansal Bilgiler
      teslimSekli: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      istatistikiKiymet: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      faturaMiktari: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      faturaMiktariDovizi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      navlunMiktari: new FormControl("", [ValidationService.decimalValidation]),
      navlunMiktariDovizi: new FormControl("", [Validators.maxLength(9)]),
      sigortaMiktari: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      sigortaMiktariDovizi: new FormControl("", [Validators.maxLength(9)]),
      sinirGecisUcreti: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      yurtDisiDemuraj: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      yurtDisiDemurajDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiDiger: new FormControl("", [ValidationService.decimalValidation]),
      yurtDisiDigerAciklama: new FormControl("", [Validators.maxLength(100)]),
      yurtDisiDigerDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiFaiz: new FormControl("", [ValidationService.decimalValidation]),
      yurtDisiFaizDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiKomisyon: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      yurtDisiKomisyonDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiRoyalti: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      yurtDisiRoyaltiDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtIciBanka: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciCevre: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciDepolama: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
      yurtIciDiger: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciDigerAciklama: new FormControl("", [Validators.maxLength(100)]),
      yurtIciKkdf: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciKultur: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciLiman: new FormControl("", [ValidationService.decimalValidation]),
      yurtIciTahliye: new FormControl("", [
        ValidationService.decimalValidation,
      ]),
    })),
      (this.odemeForm = this._fb.group({
        odemeArry: this._fb.array([this.getOdeme()]),
      })),
      (this.konteynerForm = this._fb.group({
        konteynerArry: this._fb.array([this.getKonteyner()]),
      })),
      (this.tamamlayiciForm = this._fb.group({
        tamamlayiciArry: this._fb.array([this.getTamamlayici()]),
      })),
      (this.markaForm = this._fb.group({
        markaArry: this._fb.array([this.getMarka()]),
      })),
      (this.beyannameAcmaForm = this._fb.group({
        acmaArry: this._fb.array([this.getAcma()]),
      })),
      (this.vergiForm = this._fb.group({
        vergiArry: this._fb.array([this.getVergi()]),
      })),
      (this.belgeForm = this._fb.group({
        belgeArry: this._fb.array([this.getBelge()]),
      })),
      (this.soruCevapForm = this._fb.group({
        soruCevapArry: this._fb.array([this.getSoruCevap()]),
      }));
  }
  get focus() {
    return this.kalemForm.controls;
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
      this.router.navigateByUrl("/app/beyanname");
    }
    this.getKalemler(this._beyanSession.islemInternalNo);
    this._beyannameNo.nativeElement.focus();
    this.selectionList.selectionChange.subscribe(
      (s: MatSelectionListChange) => {
        this.selectionList.deselectAll();
        s.option.selected = true;
      }
    );
  }
  disableItem() {
    this.kalemForm.disable();
    this.odemeForm.disable();
    this.konteynerForm.disable();
    this.tamamlayiciForm.disable();
    this.markaForm.disable();
    this.beyannameAcmaForm.disable();
    this.vergiForm.disable();
    this.belgeForm.disable();
    this.soruCevapForm.disable();
  }
  enableItem() {
    this.kalemForm.enable();
    this.odemeForm.enable();
    this.konteynerForm.enable();
    this.tamamlayiciForm.enable();
    this.markaForm.enable();
    this.beyannameAcmaForm.enable();
    this.vergiForm.enable();
    this.belgeForm.enable();
    this.soruCevapForm.enable();
  }
  resetItem() {
    this.kalemForm.reset();
    this.odemeForm.reset();
    this.konteynerForm.reset();
    this.tamamlayiciForm.reset();
    this.markaForm.reset();
    this.beyannameAcmaForm.reset();
    this.vergiForm.reset();
    this.belgeForm.reset();
    this.soruCevapForm.reset();

    const formOdemeArray = this.odemeForm.get("odemeArry") as FormArray;
    formOdemeArray.clear();
    this.odemeForm.setControl("odemeArry", formOdemeArray);

    const formKonteynerArray = this.konteynerForm.get(
      "konteynerArry"
    ) as FormArray;
    formKonteynerArray.clear();
    this.konteynerForm.setControl("konteynerArry", formKonteynerArray);

    const formTamamlayiciArray = this.tamamlayiciForm.get(
      "tamamlayiciArry"
    ) as FormArray;
    formTamamlayiciArray.clear();
    this.tamamlayiciForm.setControl("tamamlayiciArry", formTamamlayiciArray);

    const formMarkaArray = this.markaForm.get("markaArry") as FormArray;
    formMarkaArray.clear();
    this.markaForm.setControl("markaArry", formMarkaArray);

    const formAcmaArray = this.beyannameAcmaForm.get("acmaArry") as FormArray;
    formAcmaArray.clear();
    this.beyannameAcmaForm.setControl("acmaArry", formAcmaArray);

    const formVergiArray = this.vergiForm.get("vergiArry") as FormArray;
    formVergiArray.clear();
    this.vergiForm.setControl("vergiArry", formVergiArray);

    const formBelgeArray = this.belgeForm.get("belgeArry") as FormArray;
    formBelgeArray.clear();
    this.belgeForm.setControl("belgeArry", formBelgeArray);

    const formSoruCevapArray = this.soruCevapForm.get(
      "soruCevapArry"
    ) as FormArray;
    formSoruCevapArray.clear();
    this.soruCevapForm.setControl("soruCevapArry", formSoruCevapArray);
  }
  getKalemler(islemInternalNo: string) {
    this.beyanServis.getKalem(islemInternalNo).subscribe(
      (result: KalemDto[]) => {
        this._kalemler = result;
        this.disableItem();
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
  getKalem(kalemNo) {
    this.kalemInternalNo = this._kalemler[kalemNo - 1].kalemInternalNo;
    this.kalemNo = this._kalemler[kalemNo - 1].kalemSiraNo;
    this.kalemForm.setValue({
      beyanInternalNo: this._kalemler[kalemNo - 1].beyanInternalNo,
      kalemInternalNo: this._kalemler[kalemNo - 1].kalemInternalNo,
      gtip: this._kalemler[kalemNo - 1].gtip,
      kalemSiraNo: this._kalemler[kalemNo - 1].kalemSiraNo,
      aciklama44: this._kalemler[kalemNo - 1].aciklama44,
      adet: this._kalemler[kalemNo - 1].adet,
      algilamaBirimi1: this._kalemler[kalemNo - 1].algilamaBirimi1,
      algilamaBirimi2: this._kalemler[kalemNo - 1].algilamaBirimi2,
      algilamaBirimi3: this._kalemler[kalemNo - 1].algilamaBirimi3,
      algilamaMiktari1: this._kalemler[kalemNo - 1].algilamaMiktari1,
      algilamaMiktari2: this._kalemler[kalemNo - 1].algilamaMiktari2,
      algilamaMiktari3: this._kalemler[kalemNo - 1].algilamaMiktari3,
      brutAgirlik: this._kalemler[kalemNo - 1].brutAgirlik,
      cins: this._kalemler[kalemNo - 1].cins,
      ekKod: this._kalemler[kalemNo - 1].ekKod,
      faturaMiktari: this._kalemler[kalemNo - 1].faturaMiktari,
      faturaMiktariDovizi: this._kalemler[kalemNo - 1].faturaMiktariDovizi,
      girisCikisAmaci: this._kalemler[kalemNo - 1].girisCikisAmaci,
      girisCikisAmaciAciklama: this._kalemler[kalemNo - 1]
        .girisCikisAmaciAciklama,
      ikincilIslem:
        this._kalemler[kalemNo - 1].ikincilIslem === "EVET" ? true : false,
      imalatciFirmaBilgisi:
        this._kalemler[kalemNo - 1].imalatciFirmaBilgisi === "EVET"
          ? true
          : false,
      imalatciVergiNo: this._kalemler[kalemNo - 1].imalatciVergiNo,
      istatistikiKiymet: this._kalemler[kalemNo - 1].istatistikiKiymet,
      istatistikiMiktar: this._kalemler[kalemNo - 1].istatistikiMiktar,
      kalemIslemNiteligi: this._kalemler[kalemNo - 1].kalemIslemNiteligi,
      kullanilmisEsya: this._kalemler[kalemNo - 1].kullanilmisEsya,
      mahraceIade:
        this._kalemler[kalemNo - 1].mahraceIade === "EVET" ? true : false,
      marka: this._kalemler[kalemNo - 1].marka,
      menseiUlke: this._kalemler[kalemNo - 1].menseiUlke,
      miktar: this._kalemler[kalemNo - 1].miktar,
      miktarBirimi: this._kalemler[kalemNo - 1].miktarBirimi,
      muafiyetAciklamasi: this._kalemler[kalemNo - 1].muafiyetAciklamasi,
      muafiyetler1: this._kalemler[kalemNo - 1].muafiyetler1,
      muafiyetler2: this._kalemler[kalemNo - 1].muafiyetler2,
      muafiyetler3: this._kalemler[kalemNo - 1].muafiyetler3,
      muafiyetler4: this._kalemler[kalemNo - 1].muafiyetler4,
      muafiyetler5: this._kalemler[kalemNo - 1].muafiyetler5,
      navlunMiktari: this._kalemler[kalemNo - 1].navlunMiktari,
      navlunMiktariDovizi: this._kalemler[kalemNo - 1].navlunMiktariDovizi,
      netAgirlik: this._kalemler[kalemNo - 1].netAgirlik,
      numara: this._kalemler[kalemNo - 1].numara,
      ozellik: this._kalemler[kalemNo - 1].ozellik,
      //  referansTarihi:  this._kalemler[kalemNo - 1].referansTarihi,
      satirNo: this._kalemler[kalemNo - 1].satirNo,
      sigortaMiktari: this._kalemler[kalemNo - 1].sigortaMiktari,
      sigortaMiktariDovizi: this._kalemler[kalemNo - 1].sigortaMiktariDovizi,
      sinirGecisUcreti: this._kalemler[kalemNo - 1].sinirGecisUcreti,
      stmIlKodu: this._kalemler[kalemNo - 1].stmIlKodu,
      tamamlayiciOlcuBirimi: this._kalemler[kalemNo - 1].tamamlayiciOlcuBirimi,
      //tarifeTanimi:  this._kalemler[kalemNo - 1].
      teslimSekli: this._kalemler[kalemNo - 1].teslimSekli,
      ticariTanimi: this._kalemler[kalemNo - 1].ticariTanimi,
      uluslararasiAnlasma: this._kalemler[kalemNo - 1].uluslararasiAnlasma,
      yurtDisiDemuraj: this._kalemler[kalemNo - 1].yurtDisiDemuraj,
      yurtDisiDemurajDovizi: this._kalemler[kalemNo - 1].yurtDisiDemurajDovizi,
      yurtDisiDiger: this._kalemler[kalemNo - 1].yurtDisiDiger,
      yurtDisiDigerAciklama: this._kalemler[kalemNo - 1].yurtDisiDigerAciklama,
      yurtDisiDigerDovizi: this._kalemler[kalemNo - 1].yurtDisiDigerDovizi,
      yurtDisiFaiz: this._kalemler[kalemNo - 1].yurtDisiFaiz,
      yurtDisiFaizDovizi: this._kalemler[kalemNo - 1].yurtDisiFaizDovizi,
      yurtDisiKomisyon: this._kalemler[kalemNo - 1].yurtDisiKomisyon,
      yurtDisiKomisyonDovizi: this._kalemler[kalemNo - 1]
        .yurtDisiKomisyonDovizi,
      yurtDisiRoyalti: this._kalemler[kalemNo - 1].yurtDisiRoyalti,
      yurtDisiRoyaltiDovizi: this._kalemler[kalemNo - 1].yurtDisiRoyaltiDovizi,
      yurtIciBanka: this._kalemler[kalemNo - 1].yurtIciBanka,
      yurtIciCevre: this._kalemler[kalemNo - 1].yurtIciCevre,
      yurtIciDepolama: this._kalemler[kalemNo - 1].yurtIciDepolama,
      yurtIciDiger: this._kalemler[kalemNo - 1].yurtIciDiger,
      yurtIciDigerAciklama: this._kalemler[kalemNo - 1].yurtIciDigerAciklama,
      yurtIciKkdf: this._kalemler[kalemNo - 1].yurtIciKkdf,
      yurtIciKultur: this._kalemler[kalemNo - 1].yurtIciKultur,
      yurtIciLiman: this._kalemler[kalemNo - 1].yurtIciLiman,
      yurtIciTahliye: this._kalemler[kalemNo - 1].yurtIciTahliye,
    });

    this.beyanServis.getOdeme(this._beyanSession.islemInternalNo).subscribe(
      (result: OdemeDto[]) => {
        this._odemeler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initOdemeFormArray(this._odemeler);
        this.odemeForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getKonteyner(this._beyanSession.islemInternalNo).subscribe(
      (result: KonteynerDto[]) => {
        this._konteynerler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initKonteynerFormArray(this._konteynerler);
        this.konteynerForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis
      .getTamamlayici(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: TamamlayiciBilgiDto[]) => {
          this._tamamlayiciBilgiler = result.filter(
            (x) =>
              x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
          );
          this.initTamamlayiciFormArray(this._tamamlayiciBilgiler);
          this.tamamlayiciForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

    this.beyanServis.getMarka(this._beyanSession.islemInternalNo).subscribe(
      (result: MarkaDto[]) => {
        this._markalar = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initMarkaFormArray(this._markalar);
        this.markaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis
      .getBeyannameAcma(this._beyanSession.islemInternalNo)
      .subscribe(
        (result: BeyannameAcmaDto[]) => {
          this._acmalar = result.filter(
            (x) =>
              x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
          );
          this.initAcmaFormArray(this._acmalar);
          this.beyannameAcmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

    this.beyanServis.getVergi(this._beyanSession.islemInternalNo).subscribe(
      (result: VergiDto[]) => {
        this._vergiler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initVergiFormArray(this._vergiler);
        this.vergiForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getBelge(this._beyanSession.islemInternalNo).subscribe(
      (result: BelgeDto[]) => {
        this._belgeler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initBelgeFormArray(this._belgeler);
        this.belgeForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getSoruCevap(this._beyanSession.islemInternalNo).subscribe(
      (result: SoruCevapDto[]) => {
        this._soruCevaplar = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initSoruCevapFormArray(this._soruCevaplar);
        this.soruCevapForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.kalemForm.disable();
  }

  yukleKalemler() {
    this.getKalemler(this._beyanSession.islemInternalNo);
  }

  yeniKalem() {
    this.kalemInternalNo = "Boş";
    this.kalemNo = 0;
    this.enableItem();
    this.resetItem();
    this.kalemForm.markAllAsTouched();
  }

  duzeltKalem() {
    this.enableItem();
    this.kalemForm.markAllAsTouched();
    this.tamamlayiciForm.markAllAsTouched();
    this.odemeForm.markAllAsTouched();
    this.konteynerForm.markAllAsTouched();
    this.markaForm.markAllAsTouched();
    this.beyannameAcmaForm.markAllAsTouched();
    this.vergiForm.markAllAsTouched();
    this.belgeForm.markAllAsTouched();
    this.soruCevapForm.markAllAsTouched();
  }

  silKalem(kalemInternalNo: string) {
    if (
      confirm(kalemInternalNo + "- kalemi Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeKalem(kalemInternalNo, this._beyanSession.beyanInternalNo)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.disableItem();
          this.resetItem();
          this.yukleKalemler();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }

  onkalemFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.kalemForm.invalid) {
      const invalid = [];
      const controls = this.kalemForm.controls;
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

    this.kalemForm
      .get("beyanInternalNo")
      .setValue(this._beyanSession.beyanInternalNo);
    this.kalemForm.get("kalemSiraNo").setValue(this.kalemNo);
    this.kalemForm.get("kalemInternalNo").setValue(this.kalemInternalNo);
    let ikincilIslem =
      this.kalemForm.get("ikincilIslem").value === true ? "EVET" : "";
    this.kalemForm.get("ikincilIslem").setValue(ikincilIslem);

    let imalatciFirmaBilgisi =
      this.kalemForm.get("imalatciFirmaBilgisi").value === true ? "EVET" : "";
    this.kalemForm.get("imalatciFirmaBilgisi").setValue(imalatciFirmaBilgisi);

    let mahraceIade =
      this.kalemForm.get("mahraceIade").value === true ? "EVET" : "";
    this.kalemForm.get("mahraceIade").setValue(mahraceIade);

    let yenikalemInternalNo: string;
    let yeniKalem = new KalemDto();
    yeniKalem.init(this.kalemForm.value);

    const promiseKalem = this.beyanServis.restoreKalem(yeniKalem).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kalemServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenikalemInternalNo = kalemServisSonuc.ReferansNo;

        if (yenikalemInternalNo != null) {
          this.kalemInternalNo = yenikalemInternalNo;
          this.setOdeme();
          this.setKonteyner();
          this.setTamamlayici();
          this.setMarka();
          this.setAcma();
          this.setVergi();
          this.setBelge();
          this.setSoruCevap();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.disableItem();
          this.yukleKalemler();
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
  //#region Marka

  initMarkaFormArray(marka: MarkaDto[]) {
    const formArray = this.markaForm.get("markaArry") as FormArray;
    formArray.clear();
    for (let klm of marka) {
      let formGroup: FormGroup = new FormGroup({
        markaAdi: new FormControl(klm.markaAdi, [
          Validators.required,
          Validators.maxLength(500),
        ]),
        markaKiymeti: new FormControl(klm.markaKiymeti, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        markaTescilNo: new FormControl(klm.markaTescilNo, [
          Validators.maxLength(20),
        ]),
        markaTuru: new FormControl(klm.markaTuru, [
          Validators.required,
          Validators.maxLength(9),
        ]),
        model: new FormControl(klm.model, [Validators.maxLength(30)]),
        motorGucu: new FormControl(klm.motorGucu, [
          ValidationService.numberValidator,
        ]),
        motorHacmi: new FormControl(klm.motorHacmi, [Validators.maxLength(30)]),
        motorNo: new FormControl(klm.motorNo, [Validators.maxLength(30)]),
        motorTipi: new FormControl(klm.motorTipi, [Validators.maxLength(20)]),
        modelYili: new FormControl(klm.modelYili, [Validators.maxLength(30)]),
        renk: new FormControl(klm.renk, [Validators.maxLength(30)]),
        referansNo: new FormControl(klm.referansNo, [
          Validators.maxLength(100),
        ]),
        silindirAdet: new FormControl(klm.silindirAdet, [
          ValidationService.numberValidator,
        ]),
        vites: new FormControl(klm.vites, [Validators.maxLength(20)]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.markaForm.setControl("markaArry", formArray);
  }

  getMarka() {
    return this._fb.group({
      markaAdi: new FormControl("", [
        Validators.required,
        Validators.maxLength(500),
      ]),
      markaKiymeti: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      markaTescilNo: new FormControl("", [Validators.maxLength(20)]),
      markaTuru: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      model: new FormControl("", [Validators.maxLength(30)]),
      motorGucu: new FormControl(0, [ValidationService.numberValidator]),
      motorHacmi: new FormControl("", [Validators.maxLength(30)]),
      motorNo: new FormControl("", [Validators.maxLength(30)]),
      motorTipi: new FormControl("", [Validators.maxLength(20)]),
      modelYili: new FormControl("", [Validators.maxLength(30)]),
      renk: new FormControl("", [Validators.maxLength(30)]),
      referansNo: new FormControl("", [Validators.maxLength(100)]),
      silindirAdet: new FormControl(0, [ValidationService.numberValidator]),
      vites: new FormControl("", [Validators.maxLength(20)]),

      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get markaBilgileri() {
    return this.markaForm.get("markaArry") as FormArray;
  }

  addMarkaField() {
    this.markaBilgileri.push(this.getMarka());
  }

  deleteMarkaField(index: number) {
    this.markaBilgileri.removeAt(index);
  }

  setMarka() {
    if (this.markaBilgileri.length > 0) {
      for (let klm of this.markaBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
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
      this.initMarkaFormArray(this.markaBilgileri.value);

      if (this.markaBilgileri.invalid) {
        const invalid = [];
        const controls = this.markaBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Marka Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.markaBilgileri.length >= 0) {
      const promiseMarka = this.beyanServis
        .restoreMarka(
          this.markaBilgileri.value,
          this.kalemInternalNo,
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
  //#endregion Marka
  //#region Ödeme Şekli

  initOdemeFormArray(odeme: OdemeDto[]) {
    const formArray = this.odemeForm.get("odemeArry") as FormArray;
    formArray.clear();
    for (let klm of odeme) {
      let formGroup: FormGroup = new FormGroup({
        odemeSekliKodu: new FormControl(klm.odemeSekliKodu, [
          Validators.required,
        ]),
        odemeTutari: new FormControl(klm.odemeTutari, [
          Validators.required,
          Validators.maxLength(10),
          ValidationService.decimalValidation,
        ]),
        tbfid: new FormControl(klm.tbfid, [
          Validators.required,
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.odemeForm.setControl("odemeArry", formArray);
  }

  getOdeme() {
    return this._fb.group({
      odemeSekliKodu: new FormControl("", [Validators.required]),
      odemeTutari: new FormControl(0, [
        Validators.required,
        Validators.maxLength(10),
        ValidationService.decimalValidation,
      ]),
      tbfid: new FormControl("", [
        Validators.required,
        Validators.maxLength(30),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get odemeBilgileri() {
    return this.odemeForm.get("odemeArry") as FormArray;
  }

  addOdemeField() {
    this.odemeBilgileri.push(this.getOdeme());
  }

  deleteOdemeField(index: number) {
    this.odemeBilgileri.removeAt(index);
  }

  setOdeme() {
    if (this.odemeBilgileri.length > 0) {
      for (let klm of this.odemeBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
        klm.odemeTutari =
          typeof klm.odemeTutari == "string"
            ? parseFloat(klm.odemeTutari)
            : klm.odemeTutari;
      }

      this.initOdemeFormArray(this.odemeBilgileri.value);

      if (this.odemeBilgileri.invalid) {
        const invalid = [];
        const controls = this.odemeBilgileri.controls;

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

    if (this.odemeBilgileri.length >= 0) {
      const promiseOdeme = this.beyanServis
        .restoreOdeme(
          this.odemeBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseOdeme.then(
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
  //#endregion Ödeme
  //#region Konteyner

  initKonteynerFormArray(konteyner: KonteynerDto[]) {
    const formArray = this.konteynerForm.get("konteynerArry") as FormArray;
    formArray.clear();
    for (let klm of konteyner) {
      let formGroup: FormGroup = new FormGroup({
        konteynerNo: new FormControl(klm.konteynerNo, [
          Validators.required,
          Validators.maxLength(35),
        ]),
        ulkeKodu: new FormControl(klm.ulkeKodu, [
          Validators.required,
          Validators.maxLength(9),
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.konteynerForm.setControl("konteynerArry", formArray);
  }

  getKonteyner() {
    return this._fb.group({
      konteynerNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      ulkeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get konteynerBilgileri() {
    return this.konteynerForm.get("konteynerArry") as FormArray;
  }

  addKonteynerField() {
    this.konteynerBilgileri.push(this.getKonteyner());
  }

  deleteKonteynerField(index: number) {
    // if (this.odemeBilgileri.length !== 1) {
    this.konteynerBilgileri.removeAt(index);
  }

  setKonteyner() {
    if (this.konteynerBilgileri.length > 0) {
      for (let klm of this.konteynerBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
      }
      this.initKonteynerFormArray(this.konteynerBilgileri.value);

      if (this.konteynerBilgileri.invalid) {
        const invalid = [];
        const controls = this.konteynerBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Konteyner Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.konteynerBilgileri.length >= 0) {
      const promiseKonteyner = this.beyanServis
        .restoreKonteyner(
          this.konteynerBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseKonteyner.then(
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
  //#endregion Konteyner
  //#region Beyanname Açma
  initAcmaFormArray(acma: BeyannameAcmaDto[]) {
    const formArray = this.beyannameAcmaForm.get("acmaArry") as FormArray;
    formArray.clear();
    for (let klm of acma) {
      let formGroup: FormGroup = new FormGroup({
        beyannameNo: new FormControl(klm.beyannameNo, [Validators.required]),
        kalemNo: new FormControl(klm.kalemNo, [
          Validators.required,
          ValidationService.numberValidator,
        ]),
        miktar: new FormControl(klm.miktar, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        aciklama: new FormControl(klm.aciklama, [Validators.maxLength(100)]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.beyannameAcmaForm.setControl("acmaArry", formArray);
  }

  getAcma() {
    return this._fb.group({
      beyannameNo: new FormControl("", [Validators.required]),
      kalemNo: new FormControl(0, [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      miktar: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      aciklama: new FormControl("", [Validators.maxLength(100)]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get acmaBilgileri() {
    return this.beyannameAcmaForm.get("acmaArry") as FormArray;
  }

  addAcmaField() {
    this.acmaBilgileri.push(this.getAcma());
  }

  deleteAcmaField(index: number) {
    this.acmaBilgileri.removeAt(index);
  }

  setAcma() {
    if (this.acmaBilgileri.length > 0) {
      for (let klm of this.acmaBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
        klm.kalemNo =
          typeof klm.kalemNo == "string"
            ? parseFloat(klm.kalemNo)
            : klm.kalemNo;
        klm.miktar =
          typeof klm.miktar == "string" ? parseFloat(klm.miktar) : klm.miktar;
      }

      this.initAcmaFormArray(this.acmaBilgileri.value);

      if (this.acmaBilgileri.invalid) {
        const invalid = [];
        const controls = this.acmaBilgileri.controls;

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

          return;
        }
      }
    }

    if (this.acmaBilgileri.length >= 0) {
      const promiseAcma = this.beyanServis
        .restoreBeyannameAcma(
          this.acmaBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseAcma.then(
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
  //#endregion Beyanname Açma
  //#region Tamamlayici

  initTamamlayiciFormArray(tamamlayici: TamamlayiciBilgiDto[]) {
    const formArray = this.tamamlayiciForm.get("tamamlayiciArry") as FormArray;
    formArray.clear();
    for (let klm of tamamlayici) {
      let formGroup: FormGroup = new FormGroup({
        gtip: new FormControl(klm.gtip, [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(12),
          Validators.pattern("^[0-9]*$"),
        ]),
        bilgi: new FormControl(klm.bilgi, [
          Validators.required,
          Validators.maxLength(10),
          Validators.pattern("^[0-9]*$"),
        ]),
        oran: new FormControl(klm.oran, [
          Validators.required,
          Validators.maxLength(10),
          Validators.pattern("^[0-9]*$"),
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.tamamlayiciForm.setControl("tamamlayiciArry", formArray);
  }

  getTamamlayici() {
    return this._fb.group({
      gtip: new FormControl("", [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(12),
        Validators.pattern("^[0-9]*$"),
      ]),
      bilgi: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern("^[0-9]*$"),
      ]),
      oran: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern("^[0-9]*$"),
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get tamamlayiciBilgileri() {
    return this.tamamlayiciForm.get("tamamlayiciArry") as FormArray;
  }

  addTamamlayiciField() {
    this.tamamlayiciBilgileri.push(this.getTamamlayici());
  }

  deleteTamamlayiciField(index: number) {
    this.tamamlayiciBilgileri.removeAt(index);
  }

  setTamamlayici() {
    if (this.tamamlayiciBilgileri.length > 0) {
      for (let klm of this.tamamlayiciBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
      }
      this.initTamamlayiciFormArray(this.tamamlayiciBilgileri.value);

      if (this.tamamlayiciBilgileri.invalid) {
        const invalid = [];
        const controls = this.tamamlayiciBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Tamamlayıcı Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
          return;
        }
      }
    }

    if (this.tamamlayiciBilgileri.length >= 0) {
      const promiseKonteyner = this.beyanServis
        .restoreTamamlayici(
          this.tamamlayiciBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseKonteyner.then(
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
  //#endregion Tamamlayici
  //#region Vergi
  initVergiFormArray(vergi: VergiDto[]) {
    const formArray = this.vergiForm.get("vergiArry") as FormArray;
    formArray.clear();
    for (let klm of vergi) {
      let formGroup: FormGroup = new FormGroup({
        vergiKodu: new FormControl(klm.vergiKodu, [
          Validators.required,
          ValidationService.numberValidator,
        ]),
        vergiAciklamasi: new FormControl(klm.vergiAciklamasi, []),
        miktar: new FormControl(klm.miktar, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        matrah: new FormControl(klm.matrah, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        oran: new FormControl(klm.oran, [
          Validators.required,
          Validators.maxLength(5),
        ]),
        odemeSekli: new FormControl(klm.odemeSekli, [
          Validators.required,
          Validators.maxLength(3),
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.vergiForm.setControl("vergiArry", formArray);
  }

  getVergi() {
    return this._fb.group({
      vergiKodu: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      miktar: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      matrah: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      vergiAciklamasi: [],
      oran: new FormControl("", [Validators.required, Validators.maxLength(5)]),
      odemeSekli: new FormControl("", [
        Validators.required,
        Validators.maxLength(3),
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get vergiBilgileri() {
    return this.vergiForm.get("vergiArry") as FormArray;
  }

  addVergiField() {
    this.vergiBilgileri.push(this.getVergi());
  }

  deleteVergiField(index: number) {
    this.vergiBilgileri.removeAt(index);
  }
  vergiAd(vergi) {
    if (this.vergiBilgileri.length > 0) {
      for (let klm of this.vergiBilgileri.value) {
        if (klm.vergiKodu === vergi) {
          for (let blg of this._vergiList) {
            if (blg["kod"] === vergi) {
              klm.vergiAciklamasi = blg["aciklama"];
            }
          }
        }
      }
    }
  }
  setVergi() {
    if (this.vergiBilgileri.length > 0) {
      for (let klm of this.vergiBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
        klm.vergiKodu =
          typeof klm.vergiKodu == "string"
            ? parseFloat(klm.vergiKodu)
            : klm.vergiKodu;
        klm.miktar =
          typeof klm.miktar == "string" ? parseFloat(klm.miktar) : klm.miktar;
        klm.matrah =
          typeof klm.matrah == "string" ? parseFloat(klm.matrah) : klm.matrah;
      }

      this.initVergiFormArray(this.vergiBilgileri.value);

      if (this.vergiBilgileri.invalid) {
        const invalid = [];
        const controls = this.vergiBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Vergi Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );

          return;
        }
      }
    }

    if (this.vergiBilgileri.length >= 0) {
      const promiseVergi = this.beyanServis
        .restoreVergi(
          this.vergiBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseVergi.then(
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
  //#endregion Vergi
  //#region Belge
  initBelgeFormArray(belge: BelgeDto[]) {
    const formArray = this.belgeForm.get("belgeArry") as FormArray;
    formArray.clear();
    for (let klm of belge) {
      let formGroup: FormGroup = new FormGroup({
        belgeKodu: new FormControl(klm.belgeKodu, [
          Validators.required,
          Validators.maxLength(10),
        ]),
        dogrulama: new FormControl(klm.dogrulama, [
          Validators.required,
          Validators.maxLength(10),
        ]),
        referans: new FormControl(klm.referans, [Validators.maxLength(30)]),
        belgeTarihi: new FormControl(klm.belgeTarihi, [
          Validators.required,
          ValidationService.tarihValidation,
        ]),
        belgeAciklamasi: new FormControl(klm.belgeAciklamasi, []),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.belgeForm.setControl("belgeArry", formArray);
  }
  belgeAd(belge) {
    if (this.belgeBilgileri.length > 0) {
      for (let klm of this.belgeBilgileri.value) {
        if (klm.belgeKodu === belge) {
          for (let blg of this._belgeList) {
            if (blg["kod"] === belge) {
              klm.belgeAciklamasi = blg["aciklama"];
            }
          }
        }
      }
    }
  }
  getBelge() {
    return this._fb.group({
      belgeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
      ]),
      belgeAciklamasi: [],
      dogrulama: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
      ]),
      referans: new FormControl("", [Validators.maxLength(30)]),
      belgeTarihi: new FormControl("", [
        Validators.required,
        ValidationService.tarihValidation,
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get belgeBilgileri() {
    return this.belgeForm.get("belgeArry") as FormArray;
  }

  addBelgeField() {
    this.belgeBilgileri.push(this.getBelge());
  }

  deleteBelgeField(index: number) {
    this.belgeBilgileri.removeAt(index);
  }

  setBelge() {
    if (this.belgeBilgileri.length > 0) {
      for (let klm of this.belgeBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
      }

      this.initBelgeFormArray(this.belgeBilgileri.value);

      if (this.belgeBilgileri.invalid) {
        const invalid = [];
        const controls = this.belgeBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Belge Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );

          return;
        }
      }
    }

    if (this.belgeBilgileri.length >= 0) {
      const promiseBelge = this.beyanServis
        .restoreBelge(
          this.belgeBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseBelge.then(
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
  //#endregion Belge
  //#region Soru Cevap
  initSoruCevapFormArray(sorucevap: SoruCevapDto[]) {
    const formArray = this.soruCevapForm.get("soruCevapArry") as FormArray;
    formArray.clear();
    for (let klm of sorucevap) {
      let formGroup: FormGroup = new FormGroup({
        soruKodu: new FormControl(klm.soruKodu, [
          Validators.required,
          Validators.maxLength(10),
        ]),
        soruAciklamasi: new FormControl(klm.soruAciklamasi, []),
        soruCevap: new FormControl(klm.soruCevap, [
          Validators.maxLength(10),
          klm.tip === "Soru" ? Validators.required : Validators.nullValidator,
        ]),
        tip: new FormControl(klm.tip, [
          Validators.required,
          Validators.maxLength(10),
        ]),
        beyanInternalNo: new FormControl(klm.beyanInternalNo),
        kalemInternalNo: new FormControl(klm.kalemInternalNo),
      });

      formArray.push(formGroup);
    }
    this.soruCevapForm.setControl("soruCevapArry", formArray);
  }

  getSoruCevap() {
    return this._fb.group({
      soruKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
      ]),
      soruAciklamasi: [],
      soruCevap: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
      ]),
      tip: new FormControl("Soru", [
        Validators.required,
        Validators.maxLength(10),
      ]),
      beyanInternalNo: new FormControl(this._beyanSession.beyanInternalNo, [
        Validators.required,
      ]),
      kalemInternalNo: new FormControl(this.kalemInternalNo, [
        Validators.required,
      ]),
    });
  }

  get soruCevapBilgileri() {
    return this.soruCevapForm.get("soruCevapArry") as FormArray;
  }

  addSoruCevapField() {
    this.soruCevapBilgileri.push(this.getSoruCevap());
  }

  deleteSoruCevapField(index: number) {
    this.soruCevapBilgileri.removeAt(index);
  }

  setSoruCevap() {
    if (this.soruCevapBilgileri.length > 0) {
      for (let klm of this.soruCevapBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
      }

      this.initSoruCevapFormArray(this.soruCevapBilgileri.value);

      if (this.soruCevapBilgileri.invalid) {
        const invalid = [];
        const controls = this.soruCevapBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Soru ve Cevap Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );

          return;
        }
      }
    }

    if (this.soruCevapBilgileri.length >= 0) {
      const promiseSoruCevap = this.beyanServis
        .restoreSoruCevap(
          this.soruCevapBilgileri.value,
          this.kalemInternalNo,
          this._beyanSession.beyanInternalNo
        )
        .toPromise();
      promiseSoruCevap.then(
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
  //#endregion Soru Cevap
}
