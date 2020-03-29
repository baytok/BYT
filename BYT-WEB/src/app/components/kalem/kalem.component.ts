import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  Injector,
  ElementRef
} from "@angular/core";
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange
} from "@angular/material/list";
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
  odeme
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../shared/service-proxies/service-proxies";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemDto,
  OdemeDto,
  ServisDto
} from "../../../shared/service-proxies/service-proxies";

@Component({
  selector: "app-kalem",
  templateUrl: "./kalem.component.html",
  styleUrls: ["./kalem.component.scss"]
})
export class KalemComponent implements OnInit {
  public form: FormGroup;
  public contactList: FormArray;
  get contactFormGroup() {
    return this.form.get("contacts") as FormArray;
  }
  kalemInternalNo:string;
  kalemNo:number;
  kalemForm: FormGroup;
  odemeForm: FormGroup;
  markaForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _kalemler: KalemDto[];
  _odemeler: OdemeDto[];
  _odeme: OdemeDto;
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
        Validators.pattern("^[0-9]*$")
      ]),
      aciklama44: new FormControl("", [Validators.maxLength(500)]),
      menseiUlke: new FormControl("", [
        Validators.required,
        Validators.maxLength(9)
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
      ticariTanimi: new FormControl("", [Validators.required,Validators.maxLength(350)]),

      // Eşya Bilgileri
      // referansTarihi:[],
      cins: new FormControl("", [Validators.required, Validators.maxLength(9)]),
      ekKod: new FormControl("", [Validators.maxLength(9)]),
      adet: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      marka: new FormControl("", [
        Validators.required,
        ,
        Validators.maxLength(70)
      ]),
      miktar: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      miktarBirimi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9)
      ]),
      netAgirlik: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      brutAgirlik: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      numara: new FormControl("", [
        Validators.required,
        Validators.maxLength(70)
      ]),
      satirNo: new FormControl("", [Validators.maxLength(20)]),
      istatistikiMiktar: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      tamamlayiciOlcuBirimi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9)
      ]),
      algilamaBirimi1: new FormControl("", [Validators.maxLength(9)]),
      algilamaBirimi2: new FormControl("", [Validators.maxLength(9)]),
      algilamaBirimi3: new FormControl("", [Validators.maxLength(9)]),
      algilamaMiktari1: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      algilamaMiktari2: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      algilamaMiktari3: new FormControl("", [Validators.pattern("^[0-9]*$")]),

      //Finansal Bilgiler
      teslimSekli: new FormControl("", [
        Validators.required,
        Validators.maxLength(9)
      ]),
      istatistikiKiymet: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      faturaMiktari: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$")
      ]),
      faturaMiktariDovizi: new FormControl("", [
        Validators.required,
        Validators.maxLength(9)
      ]),
      navlunMiktari: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      navlunMiktariDovizi: new FormControl("", [Validators.maxLength(9)]),
      sigortaMiktari: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      sigortaMiktariDovizi: new FormControl("", [Validators.maxLength(9)]),
      sinirGecisUcreti: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiDemuraj: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiDemurajDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiDiger: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiDigerAciklama: new FormControl("", [Validators.maxLength(100)]),
      yurtDisiDigerDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiFaiz: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiFaizDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiKomisyon: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiKomisyonDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtDisiRoyalti: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtDisiRoyaltiDovizi: new FormControl("", [Validators.maxLength(9)]),
      yurtIciBanka: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciCevre: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciDepolama: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciDiger: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciDigerAciklama: new FormControl("", [Validators.maxLength(100)]),
      yurtIciKkdf: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciKultur: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciLiman: new FormControl("", [Validators.pattern("^[0-9]*$")]),
      yurtIciTahliye: new FormControl("", [Validators.pattern("^[0-9]*$")])
    })),
    (this.odemeForm = this._fb.group({
        odemeArry: this._fb.array([this.getOdeme()])
    })),
    (this.markaForm = this._fb.group({
        companyName: ["", [Validators.required, Validators.maxLength(25)]],
        countryName: [""],
        city: [""],
        zipCode: [""],
        street: [""],
        units: this._fb.array([this.getMarka()])
    }));
  }
  get focus() {
    return this.kalemForm.controls;
  }

  get markaitem(): FormArray {
    return this.markaForm.get("units") as FormArray;
  }
  ngOnInit() {
    
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
  getKalemler(islemInternalNo: string) {
    this.beyanServis.getKalem(islemInternalNo).subscribe(
      (result: KalemDto[]) => {
        this._kalemler = result;
        this.kalemForm.disable();
      },
      err => {
        console.log(err);
      }
    );
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getKalem(kalemNo) {
    this.kalemInternalNo=this._kalemler[kalemNo - 1].kalemInternalNo;
    this.kalemNo=this._kalemler[kalemNo - 1].kalemSiraNo;
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
      yurtIciTahliye: this._kalemler[kalemNo - 1].yurtIciTahliye
    });

    this.beyanServis.getOdeme(this._beyanSession.islemInternalNo).subscribe(
      (result: OdemeDto[]) => {
        this._odemeler = result.filter(
          x => x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
        );
        this.initOdemeFormArray(this._odemeler);
      },
      err => {
        console.log(err);
      }
    );
  
    
    // for(var klm in this._odeme)
    // {
     
    
      // this.odemes.setValue([
      //   {
      //     odemeSekliKodu:this._odeme[klm].odemeSekliKodu,
      //     odemeTutari:this._odeme[klm].odemeTutari
      //   }
      // ]);

  // }
   
   

    this.kalemForm.disable();
  }

  initOdemeFormArray(odeme: OdemeDto[]) {
    const formArray = this.odemeForm.get("odemeArry") as FormArray;
    formArray.clear();
   
     for(var klm in odeme)
     {
     
      formArray.push(this.createOdemeForms(odeme[klm]));
     
     }
    this.odemeForm.setControl("odemeArry", formArray);
    console.log(this.odemeForm.value);
   
    // odeme.map(item => {
    //   formArray.push(this.createForms(item));
    // });
    // this.odemeForm.setControl("odemeArry", formArray);
    // console.log(formArray);
  }
  createOdemeForms(odeme): FormGroup {
    let formGroup: FormGroup = new FormGroup({
      odemeSekliKodu: new FormControl(odeme.odemeSekliKodu),
      odemeTutari: new FormControl(odeme.odemeTutari)
    });

    return formGroup;
  }

  yukleKalemler() {
    this.getKalemler(this._beyanSession.islemInternalNo);
  }

  yeniKalem() {
   
    this.kalemInternalNo='Boş';
    this.kalemNo=0;
    this.kalemForm.reset();
    this.kalemForm.enable();
    this.kalemForm.markAllAsTouched();
  }

  duzeltKalem() {
   
    this.kalemForm.enable();
    this.kalemForm.markAllAsTouched();
  }

  silKalem(kalemInternalNo:string) {
    if(confirm(kalemInternalNo+ '- kalemi Silmek İstediğinizden Eminmisiniz?')){
      const promise = this.beyanServis
      .removeKalem(kalemInternalNo,this._beyanSession.beyanInternalNo)
      .toPromise();
      promise.then(
      result => {
       
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       
        this.kalemForm.reset();
        this.kalemForm.disable();
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        console.log(err);
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
        "ERROR!! :-)\n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:"  + JSON.stringify(invalid, null, 4)
      );
      return;
    }
    // this.kalemForm.setValue({
    //   aciklama44: "açıklama",
    //   adet: 1,
    //   algilamaBirimi1: "",
    //   algilamaBirimi2: "",
    //   algilamaBirimi3: "",
    //   algilamaMiktari1: 0,
    //   algilamaMiktari2: 0,
    //   algilamaMiktari3: 0,
    //   beyanInternalNo: "11111111100DB000011",
    //   brutAgirlik: 1,
    //   cins: "BI",
    //   ekKod: "",
    //   faturaMiktari: 12,
    //   faturaMiktariDovizi: "USD",
    //   girisCikisAmaci: "",
    //   girisCikisAmaciAciklama: "",
    //   gtip: "851712000011",
    //   ikincilIslem: "",
    //   imalatciFirmaBilgisi: "HAYIR",
    //   imalatciVergiNo: "",
    //   istatistikiKiymet: 10,
    //   istatistikiMiktar: 1,
    //   kalemInternalNo: "11111111100DB000011|1",
    //   kalemIslemNiteligi: "",
    //   kalemSiraNo: 1,
    //   kullanilmisEsya: "",
    //   mahraceIade: "",
    //   marka: "dcx",
    //   menseiUlke: "001",
    //   miktar: 1,
    //   miktarBirimi: "MTK",
    //   muafiyetAciklamasi: "",
    //   muafiyetler1: "",
    //   muafiyetler2: "",
    //   muafiyetler3: "",
    //   muafiyetler4: "",
    //   muafiyetler5: "",
    //   navlunMiktari: 0,
    //   navlunMiktariDovizi: "",
    //   netAgirlik: 1,
    //   numara: "1",
    //   ozellik: "",
    //   satirNo: "",
    //   sigortaMiktari: 0,
    //   sigortaMiktariDovizi: "",
    //   sinirGecisUcreti: 0,
    //   stmIlKodu: "",
    //   tamamlayiciOlcuBirimi: "C62",
    //   teslimSekli: "FOB",
    //   ticariTanimi: "tanım",
    //   uluslararasiAnlasma: "",
    //   yurtDisiDemuraj: 0,
    //   yurtDisiDemurajDovizi: "",
    //   yurtDisiDiger: 0,
    //   yurtDisiDigerAciklama: "",
    //   yurtDisiDigerDovizi: "",
    //   yurtDisiFaiz: 0,
    //   yurtDisiFaizDovizi: "",
    //   yurtDisiKomisyon: 0,
    //   yurtDisiKomisyonDovizi: "",
    //   yurtDisiRoyalti: 0,
    //   yurtDisiRoyaltiDovizi: "",
    //   yurtIciBanka: 0,
    //   yurtIciCevre: 0,
    //   yurtIciDepolama: 0,
    //   yurtIciDiger: 0,
    //   yurtIciDigerAciklama: "",
    //   yurtIciKkdf: 0,
    //   yurtIciKultur: 0,
    //   yurtIciLiman: 0,
    //   yurtIciTahliye: 0
    //  })
    this.kalemForm.get("beyanInternalNo").setValue(this._beyanSession.beyanInternalNo);
    this.kalemForm.get("kalemSiraNo").setValue(this.kalemNo);
    console.log(this.kalemInternalNo);
    this.kalemForm.get("kalemInternalNo").setValue(this.kalemInternalNo);
    let yenikalemInternalNo: string;
    let yeniKalem=new KalemDto();
    yeniKalem.init(this.kalemForm.value);
   
   console.log(yeniKalem);
      const promise = this.beyanServis
        .restoreKalem(yeniKalem)
        .toPromise();
      promise.then(
        result => {
         
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          yenikalemInternalNo = servisSonuc.Bilgiler[0].referansNo;
        
          if (yenikalemInternalNo != null) {         
        
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");
            this.kalemForm.disable();
          }
        },
        err => {
        
          this.openSnackBar(err, "Tamam");
        }
      );
   
 
  }
  onReset() {
    this.submitted = false;
  }
 

  //Marka

  getMarka() {
    const numberPatern = "^[0-9.,]+$";
    return this._fb.group({
      unitName: ["", Validators.required],
      qty: [1, [Validators.required, Validators.pattern(numberPatern)]],
      unitPrice: ["", [Validators.required, Validators.pattern(numberPatern)]],
      unitTotalPrice: [{ value: "", disabled: true }]
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

  // Ödeme Şekli
  getOdeme() {
    const numberPatern = "^[0-9.,]+$";
    return this._fb.group({
      odemeSekliKodu: ["", Validators.required],
      odemeTutari: [1, [Validators.required, Validators.pattern(numberPatern)]]
    });
  }

  get odemes() {
    return this.odemeForm.get("odemeArry") as FormArray;
  }

  onOdemeFormSubmit(): void {
    for (let i = 0; i < this.odemes.length; i++) {
      console.log(this.odemes.at(i).value);
    }
  }
  addOdemeField() {
    // this.odemeArry.push(item);
    this.odemes.push(this._fb.control(false));
  }

  deleteOdemeField(index: number) {
    if (this.odemes.length !== 1) {
      this.odemes.removeAt(index);
    }
    console.log(this.odemes.length);
  }
}
