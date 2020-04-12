import { Component, OnInit } from '@angular/core';
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
 
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
@Component({
  selector: 'app-kiymet',
  templateUrl: './kiymet.component.html',
  styleUrls: ['./kiymet.component.css']
})
export class KiymetComponent implements OnInit {
  kiymetInternalNo: string;
  kiymetForm: FormGroup;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  constructor(  private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder) 
    {
      (this.kiymetForm = this._fb.group({
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
    )
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kalem Bulunamadı",
        "Tamam"
      );
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  get focus() {
    return this.kiymetForm.controls;
  }


  yukleKiymet(){

  }
  yeniKiymet(){}
  duzeltKiymet(){}
  silKiymet(kiymetInternalNo){}
  onkiymtFormSubmit(){}
}
