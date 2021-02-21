import {
  Component,
  OnInit,
  Inject,
  Injector,
  ViewChild,
  ElementRef,
  Injectable,
} from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
} from "@angular/forms";
import { ReferansService } from "../../../../shared/helpers/ReferansService";
import * as _ from "lodash";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { GirisService } from "../../../../account/giris/giris.service";

import { MatSnackBar } from "@angular/material/snack-bar";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import {
  DolasimDto,
  ServisDto,
  BeyanIslemDurumlari,
} from "../../../../shared/service-proxies/service-proxies";
import {
  NativeDateAdapter,
  DateAdapter,
  MAT_DATE_FORMATS,
  MatDateFormats,
} from "@angular/material/core";
import { rejim } from "../../../../shared/helpers/referencesList";
import { stringify } from "@angular/compiler/src/util";
import { getLocaleDateFormat } from "@angular/common";

@Component({
  selector: "app-dolasim",
  templateUrl: "./dolasim.component.html",
  styleUrls: ["./dolasim.component.css"],
})
export class DolasimComponent implements OnInit {
  @ViewChild("islemNo", { static: true }) private islemInput: ElementRef;
  dolasimAtrForm: FormGroup;
  signedMineAtrForm: FormGroup;
  faturaAtrForm: FormGroup;
  malAtrForm: FormGroup;
  evrakAtrForm: FormGroup;
  submitted: boolean = false;
  ihracatEditable: boolean = false;
  ithalatEditable: boolean = false;
  tabIndex: number = -1;
  atrNo: string;
  atrDisabled: boolean = false;
  euro1Disabled: boolean = false;
  formaDisabled: boolean = false;
  dolasimInternalNo: string;
  beyanStatu: string;
  beyanDurum: BeyanIslemDurumlari = new BeyanIslemDurumlari();
  dolasimId: string;
  editable: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _dolasim: DolasimDto = new DolasimDto();
  _gumrukList = this.referansService.getdolasimGumruk();
  _ulkeList = this.referansService.getdolasimUlke();
  _birimList = this.referansService.getdolasimBirimler();
  _dovizList = this.referansService.getdolasimDoviz();
  _evrakList = this.referansService.getdolasimEvrakTuru();
  _belgeList = this.referansService.getdolasimBelgeler();
  constructor(
    private referansService: ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private _userRoles: UserRoles,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _fb: FormBuilder,
    private girisService: GirisService
  ) {
    (this.dolasimAtrForm = this.formBuilder.group({
      dolasimInternalNo: [],
      beyannameNo: new FormControl("", [Validators.maxLength(30)]),
      refNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(30),
      ]),
      tcKimlikNo: new FormControl("", [Validators.maxLength(11)]),
      gumrukId: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      belgeNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
      ]),
      beyanTipi: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
      ]),
      ihracatciUlkeId: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      ihracUlkesiId: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      varisUlkesiId: new FormControl("", [
        Validators.required,
        ValidationService.numberValidator,
      ]),
      kapasiteRaporuVizeNo: new FormControl("", [
        ValidationService.numberValidator,
      ]),
      kapasiteRaporuTarihi: new FormControl("", [
        ValidationService.tarihValidation,
        Validators.maxLength(20),
      ]),
      ihracatciAdi: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      ihracatciAdres: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      ihracatciVergiNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(15),
      ]),
      ihracatciYer: new FormControl("", [
        Validators.required,
        Validators.maxLength(50),
      ]),
      malinGonderildigiSahisUlkeId: new FormControl("", [
        ValidationService.numberValidator,
      ]),
      malinGonderildigiSahisAdi: new FormControl("", [
        Validators.maxLength(50),
      ]),
      malinGonderildigiSahisAdres: new FormControl("", [
        Validators.maxLength(150),
      ]),
      taahhutnameSecimi: new FormControl("A", []),
      tasimaBelgesiNo: new FormControl("", [Validators.maxLength(50)]),
      tasimaBelgesiTarihi: new FormControl("", [
        ValidationService.tarihValidation,
        Validators.maxLength(20),
      ]),
      tasimayaIliskinBilgiler: new FormControl("", [Validators.maxLength(150)]),
      faturalar: [],
      mallar: [],
      evraklar: [],
      signedMime: [],
      tescilStatu: [],
      tescilTarihi: [],
    })),
      (this.signedMineAtrForm = this.formBuilder.group({
        Beyanname_no: [],
        Tescil_tarihi: [],
        Beyan_tipi: [],
        Ihracatci_vergi_no: [],
        Ihracatci_unvan: [],
        Ihracatci_adres: [],
        Ihracatci_ulke: [],
        Alici_adsoyad: [],
        Alici_adres: [],
        Alici_ulke: [],
        Ihrac_ulke_: [],
        Varis_ulke: [],
        Tasima_bilgileri: [],
        Gozlemler: [],
        Tasima_belgesi_no: [],
        Tasima_belgesi_tarihi: [],
        _Ticari_tanimlar: [],
        _Faturalar: [],
        Belge_no: [],
        Belge_seri_no: [],
        Belge_olusturma_tarihi: [],
        Mersis_no: [],
        Mesaj: [],
        Kullanici: [],
        Kurum: [],
        Ikinci_nusha: [],
        Ilk_nusha_id: [],
        Gumruk_kodu: [],
      })),
      (this.faturaAtrForm = this._fb.group({
        atrFaturaArry: this._fb.array([this.getFaturaAtr()]),
        atrFaturaSignedArry: this._fb.array([this.getFaturaSignedAtr()]),
      })),
   
      (this.malAtrForm = this._fb.group({
        atrMalArry: this._fb.array([this.getMalAtr()]),
        atrMalSignedArry: this._fb.array([this.getMalSignedAtr()]),
      })),
   
      (this.evrakAtrForm = this._fb.group({
        atrEvrakArry: this._fb.array([this.getEvrakAtr()]),
      }));
  }
  get focus() {
    return this.dolasimAtrForm.controls;
  }

  ngOnInit() {
    if (!this._userRoles.canDolasimRoles()) {
      this.openSnackBar(
        "Dolaşım Belgeleri Sayfasını Görmeye Yetkiniz Yoktur.",
        "Tamam"
      );
      this.beyanServis.notAuthorizeRole();
    }

    this.dolasimAtrForm.disable();
    this.faturaAtrForm.disable();
    this.evrakAtrForm.disable();
    this.malAtrForm.disable();

    if (this._beyanSession.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value = this._beyanSession.islemInternalNo;
      this.getDolasimFromIslem(this._beyanSession.islemInternalNo);
    }
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  atrIslemleri() {
    this.tabIndex = 0;
    this.atrDisabled = true;
    this.formaDisabled = false;
    this.euro1Disabled = false;
  }
  euro1Islemleri() {
    this.tabIndex = 1;
    this.euro1Disabled = true;
    this.atrDisabled = false;
    this.formaDisabled = false;
  }
  formaAIslemleri() {
    this.tabIndex = 2;
    this.formaDisabled = true;
    this.atrDisabled = false;
    this.euro1Disabled = false;
  }

  getDolasimFromIslem(islemInternalNo: string) {
    this.beyanServis.getDolasim(islemInternalNo).subscribe(
      (result) => {
        this._dolasim = new DolasimDto();
        this._dolasim.init(result);
        if (this._dolasim == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.dolasimInternalNo = "";
          this.beyanStatu = "";
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.dolasimInternalNo = "";
          this._beyanSession.beyanStatu = "";

          return;
        } else {
          this._beyanSession.islemInternalNo = islemInternalNo;
          this._beyanSession.dolasimInternalNo = this._dolasim.dolasimInternalNo;
          this._beyanSession.beyanStatu = this._dolasim.tescilStatu;
          this.dolasimInternalNo = this._dolasim.dolasimInternalNo;
          this.beyanStatu = this._dolasim.tescilStatu;
          this.loadDolasimForm();
        }
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getDolasim(islemInternalNo) {
    this.beyanServis.getDolasim(islemInternalNo.value).subscribe(
      (result) => {
        this._dolasim = new DolasimDto();

        this._dolasim.init(result);

        if (this._dolasim == null) {
          this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
          this.dolasimInternalNo = "";
          this.beyanStatu = "";
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.dolasimInternalNo = "";
          this._beyanSession.beyanStatu = "";

          return;
        } else {
          this._beyanSession.islemInternalNo = islemInternalNo.value;
          this._beyanSession.dolasimInternalNo = this._dolasim.dolasimInternalNo;
          this._beyanSession.beyanStatu = this._dolasim.tescilStatu;
          this.dolasimInternalNo = this._dolasim.dolasimInternalNo;
          this.beyanStatu = this._dolasim.tescilStatu;
          this.loadDolasimForm();
        }
      },
      (err: any) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  loadDolasimForm() {
    this.dolasimAtrForm.reset();
    this.evrakAtrForm.reset();
    this.malAtrForm.reset();
    this.faturaAtrForm.reset();
    this._beyanSession.dolasimInternalNo = this._dolasim.dolasimInternalNo;

    if (this._dolasim.dolasimInternalNo.includes("ATR")) {
      this.atrIslemleri();
      this.atrNo = this._dolasim.dolasimInternalNo;
      this.dolasimAtrForm.setValue({
        dolasimInternalNo: this._dolasim.dolasimInternalNo,
        refNo: this._dolasim.refNo,
        tcKimlikNo: this._dolasim.tcKimlikNo,
        gumrukId: this._dolasim.gumrukId,
        ihracatciUlkeId: this._dolasim.ihracatciUlkeId,
        ihracUlkesiId: this._dolasim.ihracUlkesiId,
        varisUlkesiId: this._dolasim.varisUlkesiId,
        kapasiteRaporuVizeNo: this._dolasim.kapasiteRaporuVizeNo,
        malinGonderildigiSahisUlkeId: this._dolasim
          .malinGonderildigiSahisUlkeId,
        belgeNo: this._dolasim.belgeNo,
        beyanTipi: this._dolasim.beyanTipi,
        ihracatciAdi: this._dolasim.ihracatciAdi,
        ihracatciAdres: this._dolasim.ihracatciAdres,
        ihracatciVergiNo: this._dolasim.ihracatciVergiNo,
        ihracatciYer: this._dolasim.ihracatciYer,
        kapasiteRaporuTarihi: this._dolasim.kapasiteRaporuTarihi,
        malinGonderildigiSahisAdi: this._dolasim.malinGonderildigiSahisAdi,
        malinGonderildigiSahisAdres: this._dolasim.malinGonderildigiSahisAdres,
        taahhutnameSecimi: this._dolasim.taahhutnameSecimi,
        tasimaBelgesiNo: this._dolasim.tasimaBelgesiNo,
        tasimaBelgesiTarihi: this._dolasim.tasimaBelgesiTarihi,
        tasimayaIliskinBilgiler: this._dolasim.tasimayaIliskinBilgiler,
        beyannameNo: this._dolasim.beyannameNo,
        tescilStatu: this._dolasim.tescilStatu,
        tescilTarihi: this._dolasim.tescilTarihi,
        faturalar: this._dolasim.faturalar,
        mallar: this._dolasim.mallar,
        evraklar: this._dolasim.evraklar,
        signedMime: this._dolasim.signedMime,
      });

      this.initMalAtrFormArray();
      this.initMalSignedAtrFormArray();
      this.initFaturaAtrFormArray();
      this.initFaturaSignedAtrFormArray();
      this.initEvrakAtrFormArray();
      
      this.dolasimAtrForm.disable();
      this.evrakAtrForm.disable();
      this.malAtrForm.disable();
      this.faturaAtrForm.disable();
    } else if (this._dolasim.dolasimInternalNo.includes("EURO1")) {
      console.log("EURO1");
      this.euro1Islemleri();
    }
  }
  get yeniBeyanMenu(): boolean {
    let yetkiVar: boolean = false;

    var currentUser = JSON.parse(localStorage.getItem("kullaniciInfo"));
    var _usersRoles = currentUser.roles;

    for (let itm in _usersRoles) {
      if (
        _usersRoles[itm].yetkiKodu == "DO" ||
        _usersRoles[itm].yetkiKodu == "FI"
      )
        yetkiVar = true;
    }

    return yetkiVar;
  }
  get BeyanStatu(): boolean {
    if (this.beyanStatu === "undefined" || this.beyanStatu === null)
      return false;
    if (this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
      return true;
    else return false;
  }

  atrBasvuru() {
    this.dolasimInternalNo = "Boş";
    this.beyanStatu = "";
    this.dolasimAtrForm.reset();
    this.dolasimAtrForm.enable();
    this.faturaAtrForm.reset();
    this.faturaAtrForm.enable();
    this.faturaAtrForm.markAllAsTouched();
    const formAtrFaturaArray = this.faturaAtrForm.get(
      "atrFaturaArry"
    ) as FormArray;
    formAtrFaturaArray.clear();
    this.faturaAtrForm.setControl("atrFaturaArry", formAtrFaturaArray);

    this.malAtrForm.reset();
    this.malAtrForm.enable();
    this.malAtrForm.markAllAsTouched();
    const formAtrMalArray = this.malAtrForm.get("atrMalArry") as FormArray;
    formAtrMalArray.clear();
    this.malAtrForm.setControl("atrMalArry", formAtrMalArray);

    this.evrakAtrForm.reset();
    this.evrakAtrForm.enable();
    this.evrakAtrForm.markAllAsTouched();
    const formAtrEvrakArray = this.evrakAtrForm.get(
      "atrEvrakArry"
    ) as FormArray;
    formAtrEvrakArray.clear();
    this.evrakAtrForm.setControl("atrEvrakArry", formAtrEvrakArray);

    this.islemInput.nativeElement.value = "";
    this.dolasimAtrForm.markAllAsTouched();
    this.submitted = false;
  }
  duzeltAtr() {
    this.dolasimAtrForm.enable();
    this.dolasimAtrForm.markAllAsTouched();
    this.evrakAtrForm.enable();
    this.evrakAtrForm.markAllAsTouched();
    this.malAtrForm.enable();
    this.malAtrForm.markAllAsTouched();
    this.faturaAtrForm.enable();
    this.faturaAtrForm.markAllAsTouched();
    if (this.dolasimAtrForm.invalid) {
      const invalid = [];
      const controls = this.dolasimAtrForm.controls;
      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      alert(
        "ERROR!! \n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:" +
          JSON.stringify(invalid, null, 4)
      );
    }
  }
  silAtr(islemInternalNo) {
    if (
      confirm(
        islemInternalNo.value + " Başvuruyu Silmek İstediğinizden Eminmisiniz?"
      )
    ) {
      const promise = this.beyanServis
        .removeDolasim(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.dolasimAtrForm.reset();
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.dolasimInternalNo = "";
          this.dolasimInternalNo = "";
          this._beyanSession.beyanStatu = "";
          this.beyanStatu = "";
          this.islemInput.nativeElement.value = "";
          islemInternalNo.value = "";
          this.atrNo = "";
          this.dolasimAtrForm.reset();
          this.faturaAtrForm.reset();
          this.evrakAtrForm.reset();
          this.malAtrForm.reset();
          this.dolasimAtrForm.disable();
          this.faturaAtrForm.disable();
          this.evrakAtrForm.disable();
          this.malAtrForm.disable();

          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }

  //#region Fatura

  initFaturaAtrFormArray() {
    var jsonArray = JSON.parse(this._dolasim.faturalar);

    const formArray = this.faturaAtrForm.get("atrFaturaArry") as FormArray;
    formArray.clear();
    for (let klm of jsonArray) {
      let formGroup: FormGroup = new FormGroup({
        FaturaNo: new FormControl(klm.FaturaNo, [
          Validators.required,
          Validators.maxLength(35),
        ]),
        FaturaTarihi: new FormControl(klm.FaturaTarihi, [
          Validators.required,
          ValidationService.tarihValidation,
        ]),
        FaturaTutari: new FormControl(klm.FaturaTutari, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        FaturaDovizId: new FormControl(klm.FaturaDovizId, [
          Validators.required,
          Validators.maxLength(9),
        ]),
      });

      formArray.push(formGroup);
    }
    this.faturaAtrForm.setControl("atrFaturaArry", formArray);
  }
  initFaturaSignedAtrFormArray() {
    var jsonArray = JSON.parse(this._dolasim.faturalar);

    const formArray = this.faturaAtrForm.get("atrFaturaSignedArry") as FormArray;
    formArray.clear();
    for (let klm of jsonArray) {
      let formGroup: FormGroup = new FormGroup({
        Fatura_no: new FormControl(klm.FaturaNo, [        
        ]),
        Fatura_tarihi: new FormControl(klm.FaturaTarihi, [        
        ]),
        Fatura_tutari: new FormControl(klm.FaturaTutari, [
        ]),
        Fatura_birim: new FormControl(this.getDoviz(klm.FaturaDovizId), [
        ]),
      });

      formArray.push(formGroup);
    }
    this.faturaAtrForm.setControl("atrFaturaSignedArry", formArray);
  }
  getDoviz(dovizId:number):string{
    let dovizBirim;
    _.forEach(this._dovizList, function (value) {
      if (value.Id == dovizId) {
        dovizBirim= value.Kodu;
      }})
      return dovizBirim;    
  }
  getFaturaAtr() {
    return this._fb.group({
      FaturaNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      FaturaTarihi: new FormControl("", [
        Validators.required,
        ValidationService.tarihValidation,
      ]),
      FaturaTutari: new FormControl(0, [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      FaturaDovizId: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
    });
  }
  getFaturaSignedAtr(){
    return this._fb.group({
      Fatura_no: new FormControl("", []),
      Fatura_tarihi: new FormControl("", []),
      Fatura_tutari: new FormControl("", []),
      Fatura_birim: new FormControl("", []),
    });
  }
  get atrFaturaBilgileri() {
    return this.faturaAtrForm.get("atrFaturaArry") as FormArray;
  }

  addAtrFaturaField() {
    this.atrFaturaBilgileri.push(this.getFaturaAtr());
  }

  deleteAtrFaturaField(index: number) {
    // if (this.odemeBilgileri.length !== 1) {
    this.atrFaturaBilgileri.removeAt(index);
  }
 
  //#endregion Fatura

  //#region Mallar

  initMalAtrFormArray() {
    var jsonArray = JSON.parse(this._dolasim.mallar);

    const formArray = this.malAtrForm.get("atrMalArry") as FormArray;
    formArray.clear();
    for (let klm of jsonArray) {
      let formGroup: FormGroup = new FormGroup({
        Bilgi: new FormControl(klm.Bilgi, [
          Validators.required,
          Validators.maxLength(50),
        ]),
        Agirlik: new FormControl(klm.Agirlik, [
          Validators.required,
          ValidationService.decimalValidation,
        ]),
        BirimId: new FormControl(klm.BirimId, [
          Validators.required,
          Validators.maxLength(9),
        ]),
      });

      formArray.push(formGroup);
    }
    this.malAtrForm.setControl("atrMalArry", formArray);
  }
  initMalSignedAtrFormArray() {
    var jsonArray = JSON.parse(this._dolasim.mallar);

    const formArray = this.malAtrForm.get("atrMalSignedArry") as FormArray;
    formArray.clear();
    for (let klm of jsonArray) {
      let formGroup: FormGroup = new FormGroup({
        Ticari_tanim: new FormControl(klm.Bilgi, [
        ]),
        Brut_agirlik: new FormControl(klm.Agirlik, [
        ]),
        Brut_agirlik_birim: new FormControl(this.getAgirlikBirim(klm.BirimId), [
        ]),
      });

      formArray.push(formGroup);
    }
    this.malAtrForm.setControl("atrMalSignedArry", formArray);
  }
  getAgirlikBirim(birimId:number):string{
    let birim;
    _.forEach(this._birimList, function (value) {     
      if (value.Id == birimId) {        
        birim= value.Kodu;
       
      }})
   
      return birim;    
  }
  getMalAtr() {
    return this._fb.group({
      Bilgi: new FormControl("", [
        Validators.required,
        Validators.maxLength(50),
      ]),
      Agirlik: new FormControl("", [
        Validators.required,
        ValidationService.decimalValidation,
      ]),
      BirimId: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
    });
  }
  getMalSignedAtr(){
    return this._fb.group({
      Ticari_tanim: new FormControl("", []),
          Brut_agirlik: new FormControl("", []),
          Brut_agirlik_birim: new FormControl("", []),
    });
  }
  get atrMalBilgileri() {
    return this.malAtrForm.get("atrMalArry") as FormArray;
  }

  addAtrMalField() {
    this.atrMalBilgileri.push(this.getMalAtr());
  }

  deleteAtrMalField(index: number) {
    // if (this.odemeBilgileri.length !== 1) {
    this.atrMalBilgileri.removeAt(index);
  }

  //#endregion Mallar

  //#region Evraklar

  initEvrakAtrFormArray() {
    var jsonArray = JSON.parse(this._dolasim.evraklar);

    const formArray = this.evrakAtrForm.get("atrEvrakArry") as FormArray;
    formArray.clear();
    for (let klm of jsonArray) {
      let formGroup: FormGroup = new FormGroup({
        EvrakTurId: new FormControl(klm.EvrakTurId, [Validators.maxLength(3)]),
        Uzanti: new FormControl(klm.Uzanti, [Validators.maxLength(10)]),
        EvrakBase64: new FormControl(klm.EvrakBase64, [Validators.required]),
      });

      formArray.push(formGroup);
    }
    this.evrakAtrForm.setControl("atrEvrakArry", formArray);
  }

  getEvrakAtr() {
    return this._fb.group({
      EvrakTurId: new FormControl("", [Validators.maxLength(3)]),
      Uzanti: new FormControl("", [Validators.maxLength(10)]),
      EvrakBase64: new FormControl("", [Validators.required]),
    });
  }

  get atrEvrakBilgileri() {
    return this.evrakAtrForm.get("atrEvrakArry") as FormArray;
  }

  addAtrEvrakField() {
    this.atrEvrakBilgileri.push(this.getEvrakAtr());
  }

  deleteAtrEvrakField(index: number) {
    // if (this.odemeBilgileri.length !== 1) {
    this.atrEvrakBilgileri.removeAt(index);
  }

  //#endregion Mallar

  onAtrFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.dolasimAtrForm.invalid) {
      const invalid = [];
      const controls = this.dolasimAtrForm.controls;
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

    this.dolasimAtrForm
      .get("dolasimInternalNo")
      .setValue(this.dolasimInternalNo);
    this.dolasimAtrForm
      .get("tcKimlikNo")
      .setValue(this.girisService.loggedKullanici);
    this.dolasimAtrForm.get("taahhutnameSecimi").setValue("A");
    this.dolasimAtrForm
      .get("mallar")
      .setValue(JSON.stringify(this.atrMalBilgileri.value));
    this.dolasimAtrForm
      .get("evraklar")
      .setValue(JSON.stringify(this.atrEvrakBilgileri.value));
    this.dolasimAtrForm
      .get("faturalar")
      .setValue(JSON.stringify(this.atrFaturaBilgileri.value));
    
    this.signedAtr();
    this.dolasimAtrForm
      .get("signedMime")
      .setValue(JSON.stringify(this.signedMineAtrForm.value));

    let yeniislemInternalNo: string;
    let yeniDolasim = new DolasimDto();
    yeniDolasim.init(this.dolasimAtrForm.value);

    const promise = this.beyanServis.restoreDolasim(yeniDolasim).toPromise();
    promise.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());

        yeniislemInternalNo = beyanServisSonuc.ReferansNo;

        if (yeniislemInternalNo != null) {
          this.islemInput.nativeElement.value = yeniislemInternalNo;
          this._dolasim.dolasimInternalNo = yeniislemInternalNo;
          this._beyanSession.islemInternalNo = yeniislemInternalNo;
          this.getDolasimFromIslem(yeniislemInternalNo);
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        }
        this.dolasimAtrForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.dolasimAtrForm.value, null, 4)
    // );
  }

  onCancel() {
    this.submitted = false;
    this.dolasimAtrForm.disable();
  }

  signedAtr() {
    let ihracatciUlkeId, ihracUlkeId, aliciUlke, varisUlkesiId;
    ihracatciUlkeId = this.dolasimAtrForm.get("ihracatciUlkeId").value;
    ihracUlkeId= this.dolasimAtrForm.get("ihracUlkesiId").value;
    aliciUlke=this.dolasimAtrForm.get("malinGonderildigiSahisUlkeId").value;
    varisUlkesiId=this.dolasimAtrForm.get("varisUlkesiId").value;
    _.forEach(this._ulkeList, function (value) {
      if (value.Id === ihracatciUlkeId) {
        ihracatciUlkeId = value.Kodu;
      }
    
      if (value.Id ===ihracUlkeId) {
        ihracUlkeId = value.Kodu;
      }
    
      if (value.Id ===aliciUlke ) {
        aliciUlke = value.Adi;
      }
     
      if (value.Id === varisUlkesiId) {
        varisUlkesiId = value.Kodu;
      }
    });

    let gumrukId=this.dolasimAtrForm.get("gumrukId").value;
    _.forEach(this._gumrukList, function (value) {
      if (value.Id === gumrukId) {
        gumrukId = value.Kodu;
      }
    });

    let d: string = new Date().toISOString();

        this.signedMineAtrForm.reset();
        this.signedMineAtrForm.setValue({
          Beyanname_no: this.dolasimAtrForm.get("beyannameNo").value,
          Tescil_tarihi:
            this.dolasimAtrForm.get("tescilTarihi").value != null
              ? this.dolasimAtrForm.get("tescilTarihi").value 
              : "0001-01-01T00:00:00",
          Beyan_tipi: this.dolasimAtrForm.get("beyanTipi").value,
          Ihracatci_vergi_no: this.dolasimAtrForm.get("ihracatciVergiNo").value,
          Ihracatci_unvan: this.dolasimAtrForm.get("ihracatciAdi").value,
          Ihracatci_adres: this.dolasimAtrForm.get("ihracatciAdres").value,
          Ihracatci_ulke: ihracatciUlkeId,
          Alici_adsoyad: this.dolasimAtrForm.get("malinGonderildigiSahisAdi")
            .value,
          Alici_adres: this.dolasimAtrForm.get("malinGonderildigiSahisAdres")
            .value,
          Alici_ulke: aliciUlke,
          Ihrac_ulke_: ihracUlkeId,
          Varis_ulke: "EU",
          Tasima_bilgileri: this.dolasimAtrForm.get("tasimayaIliskinBilgiler")
            .value,
          Gozlemler: "",
          Tasima_belgesi_no: this.dolasimAtrForm.get("tasimaBelgesiNo").value,
          Tasima_belgesi_tarihi:
            this.dolasimAtrForm.get("tasimaBelgesiTarihi").value != null
              ? this.dolasimAtrForm.get("tasimaBelgesiTarihi").value 
              : "0001-01-01T00:00:00",
          _Ticari_tanimlar: this.malAtrForm.get("atrMalSignedArry").value,
          _Faturalar: this.faturaAtrForm.get("atrFaturaSignedArry").value,
          Belge_no: "0321",
          Belge_seri_no: this.dolasimAtrForm.get("belgeNo").value,
          Belge_olusturma_tarihi: d,
          Mersis_no: "",
          Mesaj: "",
          Kullanici: this.dolasimAtrForm.get("tcKimlikNo").value,
          Kurum: "TOBB",
          Ikinci_nusha: "",
          Ilk_nusha_id: "",
          Gumruk_kodu: gumrukId,
        });
      }
    }
  

