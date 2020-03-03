import { Component, OnInit, ViewChild, Inject, Injector, ElementRef } from "@angular/core";
import { MatListOption, MatSelectionList,MatSelectionListChange } from "@angular/material/list";
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import { MustMatch } from "../../../shared/helpers/must-match.validator";
import {
  ulke,
  teslimSekli,
  dovizCinsi
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../shared/service-proxies/service-proxies";
import { MatSnackBar, MatDialog } from "@angular/material";

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemlerDto,
  ServisDto
} from "../../../shared/service-proxies/service-proxies";

@Component({
  selector: "app-kalem",
  templateUrl: "./kalem.component.html",
  styleUrls: ["./kalem.component.scss"]
})
export class KalemComponent implements OnInit {

  
  kalemForm: FormGroup;
  ornekForm: FormGroup;
  submitted: boolean = false;
  guidOf = this.session.guidOf;
  islemInternalNo = this.session.islemInternalNo;
  beyanInternalNo = this.session.beyanInternalNo;
  beyanStatu = this.session.beyanStatu;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _kalemler: KalemlerDto[];
  _ulkeList = ulke;
  _teslimList = teslimSekli;
  _dovizList = dovizCinsi;
  @ViewChild('KalemList', {static: true}) private selectionList: MatSelectionList;
  @ViewChild('BeyannameNo', {static: true}) private _beyannameNo: ElementRef;
  
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _fb: FormBuilder
  ) {
    (this.kalemForm = this.formBuilder.group({
      //Genel Bilgiler
      beyanInternalNo: [],
      kalemInternalNo: [],
      kalemSiraNo: [],
      gtip: []
      //  aciklama44: [],
      //  adet: [],
      //  algilamaBirimi1:[],
      //  algilamaBirimi2: [],
      //  algilamaBirimi3: [],
      //  algilamaMiktari1: [],
      //  algilamaMiktari2: [],
      //  algilamaMiktari3: [],
      //  beyanInternalNo: [],
      //  brutAgirlik: [],
      //  cins: [],
      //  ekKod: [],
      //  faturaMiktari:[],
      //  faturaMiktariDovizi:[],
      //  girisCikisAmaci: [],
      //  girisCikisAmaciAciklama:[],
      //  ikincilIslem:[],
      //  imalatciFirmaBilgisi: [],
      //  imalatciVergiNo: [],
      //  istatistikiKiymet: [],
      //  istatistikiMiktar: [],
      //
      //  kalemIslemNiteligi: [],
      //  kullanilmisEsya: [],
      //  mahraceIade: [],
      //  marka: [],
      //  menseiUlke: [],
      //  miktar: [],
      //  miktarBirimi: [],
      //  muafiyetAciklamasi:[],
      //  muafiyetler1: [],
      //  muafiyetler2:[],
      //  muafiyetler3: [],
      //  muafiyetler4: [],
      //  muafiyetler5: [],
      //  navlunMiktari: [],
      //  navlunMiktariDovizi:[],
      //  netAgirlik: [],
      //  numara: [],
      //  ozellik: [],
      //  referansTarihi:[],
      //  satirNo:[],
      //  sigortaMiktari: [],
      //  sigortaMiktariDovizi:[],
      //  sinirGecisUcreti: [],
      //  stmIlKodu: [],
      //  tamamlayiciOlcuBirimi:[],
      //  tarifeTanimi:[],
      //  teslimSekli: [],
      //  ticariTanimi:[],
      //  uluslararasiAnlasma: [],
      //  yurtDisiDemuraj: [],
      //  yurtDisiDemurajDovizi: [],
      //  yurtDisiDiger: [],
      //  yurtDisiDigerAciklama:[],
      //  yurtDisiDigerDovizi: [],
      //  yurtDisiFaiz: [],
      //  yurtDisiFaizDovizi: [],
      //  yurtDisiKomisyon: [],
      //  yurtDisiKomisyonDovizi: [],
      //  yurtDisiRoyalti: [],
      //  yurtDisiRoyaltiDovizi: [],
      //  yurtIciBanka:[],
      //  yurtIciCevre: [],
      //  yurtIciDepolama: [],
      //  yurtIciDiger:[],
      //  yurtIciDigerAciklama: [],
      //  yurtIciKkdf: [],
      //  yurtIciKultur:[],
      //  yurtIciLiman:[],
      //  yurtIciTahliye:[],

      //     kullanici: new FormControl(""),
      //     gumruk: new FormControl("", [Validators.required]),
      //     rejim: ["", Validators.required],
      //     aciklamalar: new FormControl("", [Validators.maxLength(350)]),
      //     aliciSaticiIliskisi: [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ],
      //     aliciVergiNo: new FormControl("", [Validators.maxLength(20)]),
      //     gondericiVergiNo: new FormControl("", [Validators.maxLength(20)]),
      //     musavirVergiNo:  new FormControl("", [Validators.maxLength(20)]),
      //     beyanSahibiVergiNo: new FormControl("", [
      //       Validators.required,
      //       Validators.maxLength(20)
      //     ]),
      //     asilSorumluVergiNo: new FormControl("", [Validators.maxLength(20)]),

      //     basitlestirilmisUsul: new FormControl("", [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     ticaretUlkesi:  new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(3),
      //       Validators.maxLength(30),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     cikisUlkesi: new FormControl("",  [
      //       Validators.required,
      //       Validators.minLength(2),
      //       Validators.maxLength(9),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     gidecegiSevkUlkesi:  new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(3),
      //       Validators.maxLength(30),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     gidecegiUlke:  new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(3),
      //       Validators.maxLength(30),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     birlikKayitNumarasi: new FormControl("", [Validators.maxLength(30)]),
      //     birlikKriptoNumarasi: new FormControl("", [Validators.maxLength(30)]),
      //     isleminNiteligi:new FormControl("", [Validators.required,Validators.maxLength(9)]),
      //     kapAdedi: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     musavirReferansNo: new FormControl("", [Validators.required,Validators.maxLength(12)]),
      //     referansTarihi: [],
      //     tescilStatu: [],
      //     tescilTarihi:[],
      //     refNo: new FormControl("", [Validators.maxLength(30)]),
      //     //Finansal Bilgiler
      //     bankaKodu: new FormControl("", [
      //       Validators.maxLength(16),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     teslimSekli: new FormControl("", [Validators.required,Validators.maxLength(9)]),
      //     teslimSekliYeri:  new FormControl("", [Validators.maxLength(40)]),
      //     telafiEdiciVergi:new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     toplamFatura: new FormControl("", [Validators.required,Validators.pattern("^[0-9]*$")]),
      //     toplamFaturaDovizi:new FormControl("", [Validators.required]),
      //     toplamNavlun: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     toplamNavlunDovizi: [],
      //     toplamSigorta: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     toplamSigortaDovizi: [],
      //     toplamYurtDisiHarcamalar:new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     toplamYurtDisiHarcamalarDovizi: [],
      //     toplamYurtIciHarcamalar:new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     odemeAraci:[],

      //     //Taşıma Bilgileri
      //     antrepoKodu: new FormControl("", [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     limanKodu:new FormControl("", [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     sinirdakiAracinUlkesi: new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(3),
      //       Validators.maxLength(30),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     cikistakiAracinUlkesi: new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(3),
      //       Validators.maxLength(30),
      //       Validators.pattern("^[a-zA-Z0-9]*$")
      //     ]),
      //     cikistakiAracinKimligi: new FormControl("", [
      //       Validators.required,
      //       Validators.maxLength(35)
      //     ]),
      //     cikistakiAracinTipi: new FormControl("",[
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     sinirdakiAracinKimligi: new FormControl("", [
      //       Validators.required,
      //       Validators.maxLength(35)
      //     ]),
      //     sinirdakiAracinTipi: new FormControl("", [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     sinirdakiTasimaSekli: new FormControl("", [
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     konteyner: [false, Validators.requiredTrue],
      //     girisGumrukIdaresi:new FormControl("",[
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     varisGumrukIdaresi:new FormControl("",[
      //       Validators.maxLength(9),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     tasarlananGuzergah: new FormControl("", [Validators.maxLength(250)]),
      //     yukBelgeleriSayisi: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      //     yuklemeBosaltmaYeri: new FormControl("", [Validators.maxLength(40)]),
      //     esyaninBulunduguYer: new FormControl("", [Validators.maxLength(40)]),
      //     mobil1: new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(8),
      //       Validators.maxLength(12),
      //       Validators.pattern("^[0-9]*$")
      //     ]),
      //     mail1: new FormControl("", [
      //       Validators.required,
      //       Validators.minLength(5),
      //       Validators.maxLength(80),
      //       Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")
      //     ])
    })),
      (this.ornekForm = this._fb.group({
        companyName: ["", [Validators.required, Validators.maxLength(25)]],
        countryName: [""],
        city: [""],
        zipCode: [""],
        street: [""],
        units: this._fb.array([this.getUnit()])
      }));
  }
  get focus() {
    return this.kalemForm.controls;
  }
  ngOnInit() {
    this._beyannameNo.nativeElement.focus();
    this.selectionList.selectionChange.subscribe((s: MatSelectionListChange) => { 
      this.selectionList.deselectAll();
      s.option.selected = true;
  });

  
    if (
      this.session.islemInternalNo == undefined ||
      this.session.islemInternalNo == null
    )
      this.openSnackBar(
        this.session.islemInternalNo + " ait Kalem Bulunamadı",
        "Tamam"
      );
    else this._kalemler = this.session.Kalemler;
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getKalem(kalemNo) {
    this.kalemForm.setValue({
      beyanInternalNo: this._kalemler[kalemNo - 1].beyanInternalNo,
      kalemInternalNo: this._kalemler[kalemNo - 1].kalemInternalNo,
      gtip: this._kalemler[kalemNo - 1].gtip,
      kalemSiraNo: this._kalemler[kalemNo - 1].kalemSiraNo

      // aciklama44: "Aciklama44",
      // adet: 10,
      // algilamaBirimi1: "",
      // algilamaBirimi2: "",
      // algilamaBirimi3: "",
      // algilamaMiktari1: 0,
      // algilamaMiktari2: 0,
      // algilamaMiktari3: 0,
      // brutAgirlik: 100,
      // cins: "BI",
      // ekKod: "",
      // faturaMiktari: 100,
      // faturaMiktariDovizi: "USD",
      // girisCikisAmaci: "",
      // girisCikisAmaciAciklama: "",
      // ikincilIslem: "",
      // imalatciFirmaBilgisi: "HAYIR",
      // imalatciVergiNo: "",
      // istatistikiKiymet: 600,
      // istatistikiMiktar: 10,
      // kalemInternalNo: "11111111100DBG000011|1",
      // kalemIslemNiteligi: "",
      // kullanilmisEsya: "",
      // mahraceIade: "",
      // marka: "iphone",
      // menseiUlke: "400",
      // miktar: 10,
      // miktarBirimi: "C6",
      // muafiyetAciklamasi: "",
      // muafiyetler1: "",
      // muafiyetler2: "",
      // muafiyetler3: "",
      // muafiyetler4: "",
      // muafiyetler5: "",
      // navlunMiktari: 0,
      // navlunMiktariDovizi: "",
      // netAgirlik: 100,
      // numara: "12345",
      // ozellik: "88",
      // referansTarihi: "",
      // satirNo: "",
      // sigortaMiktari: 0,
      // sigortaMiktariDovizi: "",
      // sinirGecisUcreti: 0,
      // stmIlKodu: "",
      // tamamlayiciOlcuBirimi: "C62",
      // tarifeTanimi: "",
      // teslimSekli: "FOB",
      // ticariTanimi: "TicariTanimi",
      // uluslararasiAnlasma: "",
      // yurtDisiDemuraj: 0,
      // yurtDisiDemurajDovizi: "",
      // yurtDisiDiger: 0,
      // yurtDisiDigerAciklama: "",
      // yurtDisiDigerDovizi: "",
      // yurtDisiFaiz: 0,
      // yurtDisiFaizDovizi: "",
      // yurtDisiKomisyon: 0,
      // yurtDisiKomisyonDovizi: "",
      // yurtDisiRoyalti: 0,
      // yurtDisiRoyaltiDovizi: "",
      // yurtIciBanka: 0,
      // yurtIciCevre: 0,
      // yurtIciDepolama: 0,
      // yurtIciDiger: 0,
      // yurtIciDigerAciklama: "",
      // yurtIciKkdf: 0,
      // yurtIciKultur: 0,
      // yurtIciLiman: 0,
      // yurtIciTahliye: 0
    });

    this.kalemForm.disable();
  }

  yeniKalem() {
    this.kalemForm.reset();
  }

  duzeltKalem() {
    this.kalemForm.enable();
  }

  silKalem() {
    this.kalemForm.enable();
  }

  onkalemFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.kalemForm.invalid) {
      console.log("User Registration Form Submit", this.kalemForm.value);
      return;
    }

    // display form values on success
    alert("SUCCESS!! :-)\n\n" + JSON.stringify(this.kalemForm.value, null, 4));
  }
  onReset() {
    this.submitted = false;
  }
 

  private getUnit() {
    const numberPatern = "^[0-9.,]+$";
    return this._fb.group({
      unitName: ["", Validators.required],
      qty: [1, [Validators.required, Validators.pattern(numberPatern)]],
      unitPrice: ["", [Validators.required, Validators.pattern(numberPatern)]],
      unitTotalPrice: [{ value: "", disabled: true }]
    });
  }

  private addUnit() {
    const control = <FormArray>this.kalemForm.controls["units"];
    control.push(this.getUnit());
  }

  private removeUnit(i: number) {
    const control = <FormArray>this.kalemForm.controls["units"];
    control.removeAt(i);
  }
}
