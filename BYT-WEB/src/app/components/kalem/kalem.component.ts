import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  Injector,
  ElementRef,
} from "@angular/core";
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
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../shared/service-proxies/UserRoles";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemDto,
  OdemeDto,
  KonteynerDto,
  TamamlayiciBilgiDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";

@Component({
  selector: "app-kalem",
  templateUrl: "./kalem.component.html",
  styleUrls: ["./kalem.component.scss"],
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
  _ulkeList = ulke;
  _teslimList = teslimSekli;
  _dovizList = dovizCinsi;
  _kullanilmisList = kullanilmisEsya;
  _girisCikisAmaciList = girisCikisAmaci;
  _anlasmaList = anlasma;
  _muafiyetList = muafiyet;
  _ozellikList = ozellik;
  _isleminNiteligiList = kalemIsleminNiteligi;
  _stmIlKodList = stmIlKod;
  _cinsList = cins;
  _olcuList = olcu;
  _algilamaList = algilama;
  _odemeList = odeme;
  @ViewChild("KalemList", { static: true })
  private selectionList: MatSelectionList;
  @ViewChild("BeyannameNo", { static: true }) private _beyannameNo: ElementRef;

  constructor(
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder
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
      ozellik: new FormControl("", [Validators.maxLength(9)]),
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
        beyanInternalNo: [],
        kalemInternalNo: [],
        companyName: ["", [Validators.required, Validators.maxLength(25)]],
        countryName: [""],
        city: [""],
        zipCode: [""],
        street: [""],
        units: this._fb.array([this.getMarka()]),
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
    )
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kalem Bulunamadı",
        "Tamam"
      );
  
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
  }
  enableItem() {
    this.kalemForm.enable();
    this.odemeForm.enable();
    this.konteynerForm.enable();
    this.tamamlayiciForm.enable();
  }
  resetItem() {
    this.kalemForm.reset();
    this.odemeForm.reset();
    this.konteynerForm.reset();
    this.tamamlayiciForm.reset();
    const formOdemeArray = this.odemeForm.get("odemeArry") as FormArray;
    formOdemeArray.clear();
    this.odemeForm.setControl("odemeArry", formOdemeArray);

    const formKonteynerArray = this.odemeForm.get("konteynerArry") as FormArray;
    formKonteynerArray.clear();
    this.konteynerForm.setControl("konteynerArry", formKonteynerArray);

    const formTamamlayiciArray = this.tamamlayiciForm.get(
      "tamamlayiciArry"
    ) as FormArray;
    formTamamlayiciArray.clear();
    this.tamamlayiciForm.setControl("tamamlayiciArry", formTamamlayiciArray);
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
      ikincilIslem: this._kalemler[kalemNo - 1].ikincilIslem,
      imalatciFirmaBilgisi: this._kalemler[kalemNo - 1].imalatciFirmaBilgisi,
      imalatciVergiNo: this._kalemler[kalemNo - 1].imalatciVergiNo,
      istatistikiKiymet: this._kalemler[kalemNo - 1].istatistikiKiymet,
      istatistikiMiktar: this._kalemler[kalemNo - 1].istatistikiMiktar,
      kalemIslemNiteligi: this._kalemler[kalemNo - 1].kalemIslemNiteligi,
      kullanilmisEsya: this._kalemler[kalemNo - 1].kullanilmisEsya,
      mahraceIade: this._kalemler[kalemNo - 1].mahraceIade,
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
    let yenikalemInternalNo: string;
    let yeniKalem = new KalemDto();
    yeniKalem.init(this.kalemForm.value);

    const promiseKalem = this.beyanServis.restoreKalem(yeniKalem).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        yenikalemInternalNo = servisSonuc.Bilgiler[0].referansNo;

        if (yenikalemInternalNo != null) {
          this.kalemInternalNo = yenikalemInternalNo;
          this.setOdeme();
          this.setKonteyner();
          this.setTamamlayici();
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

  //Marka
  get markaitem(): FormArray {
    return this.markaForm.get("units") as FormArray;
  }

  getMarka() {
    const numberPatern = "^[0-9.,]+$";
    return this._fb.group({
      unitName: ["", Validators.required],
      qty: [1, [Validators.required, Validators.pattern(numberPatern)]],
      unitPrice: ["", [Validators.required, Validators.pattern(numberPatern)]],
      unitTotalPrice: [{ value: "", disabled: true }],
    });
  }

  addMarka() {
    const control = <FormArray>this.markaForm.controls["units"];
    control.push(this.getMarka());
  }

  removeMarka(i: number) {
    const control = <FormArray>this.markaForm.controls["units"];
    control.removeAt(i);
  }

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
      beyanInternalNo: new FormControl("", []),
      kalemInternalNo: new FormControl("", []),
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
        klm.beyanInternalNo = this._beyanSession.beyanInternalNo;       
        klm.odemeTutari = typeof(klm.odemeTutari)=="string" ? parseFloat(klm.odemeTutari) : klm.odemeTutari;
       
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
     console.log(this.odemeBilgileri.value);
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
      beyanInternalNo: new FormControl("", []),
      kalemInternalNo: new FormControl("", []),
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
        klm.beyanInternalNo = this._beyanSession.beyanInternalNo;
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
      beyanInternalNo: new FormControl("", []),
      kalemInternalNo: new FormControl("", []),
    });
  }

  get tamamlayiciBilgileri() {
    return this.tamamlayiciForm.get("tamamlayiciArry") as FormArray;
  }

  addTamamlayiciField() {
    this.tamamlayiciBilgileri.push(this.getTamamlayici());
  }

  deleteTamamlayiciField(index: number) {
    // if (this.odemeBilgileri.length !== 1) {
    this.tamamlayiciBilgileri.removeAt(index);
  }

  setTamamlayici() {
    if (this.tamamlayiciBilgileri.length > 0) {
      for (let klm of this.tamamlayiciBilgileri.value) {
        klm.kalemInternalNo = this.kalemInternalNo;
        klm.beyanInternalNo = this._beyanSession.beyanInternalNo;
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
}
