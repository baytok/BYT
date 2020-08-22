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
  BeyannameServiceProxy,
  SessionServiceProxy,
 
} from "../../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import { ReferansService } from "../../../../shared/helpers/ReferansService";
import {
   NbKalemDto,
   NbKonteynerDto,
   NbKalemGondericiFirmaDto,
   NbKalemAliciFirmaDto,
   NbKalemGuvenliGondericiFirmaDto,
   NbKalemGuvenliAliciFirmaDto,
   NbHassasEsyaDto,
   NbKapDto,
   NbEkBilgiDto,
   NbBelgelerDto,
   NbOncekiBelgelerDto,
   ServisDto,
   BeyanIslemDurumlari
} from "../../../../shared/service-proxies/service-proxies";
import {
  nctsrejim,
 
} from "../../../../shared/helpers/referencesList";
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
  selector: "app-tasimasenet",
  templateUrl: "./nbkalem.component.html",
  styleUrls: ["./nbkalem.component.css"],
  providers: [
    { provide: DateAdapter, useClass: PickDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS },
  ],
})
export class NbKalemComponent implements OnInit {
  public form: FormGroup;
  kalemInternalNo: string;
  kalemSiraNo: number;
  sentNo:string
  kalemForm: FormGroup;
  aliciFirmaForm:FormGroup;
  gondericiFirmaForm:FormGroup;
  guvenlialiciFirmaForm:FormGroup;
  guvenligondericiFirmaForm:FormGroup;
  konteynerForm: FormGroup;
  hassasEsyaForm: FormGroup;
  kapForm: FormGroup;
  ekBilgiForm: FormGroup;
  belgeForm: FormGroup;
  oncekiBelgeForm: FormGroup;
  closeResult: string;
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  nctsBeyanInternalNo = this._beyanSession.nctsBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  beyanDurum: BeyanIslemDurumlari=new BeyanIslemDurumlari();
  _kalemler: NbKalemDto[];
  _konteynerler: NbKonteynerDto[];
  _hassasEsyalar: NbHassasEsyaDto[];
  _kaplar: NbKapDto[];
  _ekBilgiler: NbEkBilgiDto[];
  _belgeler: NbBelgelerDto[];
  _oncekiBelgeler: NbOncekiBelgelerDto[];
   _ulkeList = this.referansService.getUlkeDilJSON();
  _dilList = this.referansService.getDilJSON();
  _dovizList = this.referansService.getTrDovizCinsiJSON();
  _rejimList = nctsrejim;
  _odemeList = this.referansService.getNctsOdemeJSON();
  _ekBilgiList= this.referansService.getNctsEkBilgiJSON();
  _belgeList= this.referansService.getNctsBelgeTipJSON();
  _oncekiBelgeList= this.referansService.getNctsOncekiBelgeTipJSON();
  _kapCinsi=this.referansService.getNctsKapTipJSON();
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
    (this.kalemForm = this._fb.group({
      nctsBeyanInternalNo: [],
      kalemInternalNo: [],
      kalemSiraNo: [],
      gtip: new FormControl("", [
        Validators.required,
        Validators.maxLength(16),
       
      ]),
      rejimKodu: new FormControl("", [ Validators.maxLength(4)]),
      ticariTanim: new FormControl("", [Validators.required,Validators.maxLength(210)]),
      ticariTanimDil: new FormControl("", [Validators.maxLength(4)]),
      kiymet: new FormControl("", [ Validators.required,ValidationService.decimalValidation]),
      kiymetDoviz: new FormControl("", [ Validators.required,Validators.maxLength(4)]),
      burutAgirlik: new FormControl("", [Validators.required,ValidationService.decimalValidation]),
      netAgirlik: new FormControl("", [ValidationService.decimalValidation]),

      varisUlkesi: new FormControl("", [
         Validators.maxLength(4),
      ]),
      cikisUlkesi: new FormControl("", [
        Validators.maxLength(4),
     ]),    
      tptChMOdemeKod: new FormControl("", [Validators.maxLength(4)]),
      konsimentoRef: new FormControl("", [Validators.maxLength(70)]),
      undg: new FormControl("", [Validators.maxLength(4)]),
      ihrBeyanNo: new FormControl("", [Validators.maxLength(20)]),
      ihrBeyanTip: new FormControl("", [Validators.maxLength(9)]),
      ihrBeyanParcali: new FormControl("", [Validators.maxLength(9)]),
    })),      
       this.aliciFirmaForm = this._fb.group({
        adUnvan: new FormControl("", [
       
          Validators.maxLength(150),
        ]),
        caddeSokakNo: new FormControl("", [
         
          Validators.maxLength(150),
        ]),
   
        ilIlce: new FormControl("", [
        
          Validators.maxLength(35),
        ]),
        dil: new FormControl("", [
        
          Validators.maxLength(4),
        ]),
        no: new FormControl("", [
        
          Validators.maxLength(15),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        postaKodu: new FormControl("", [
         
          Validators.maxLength(10),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        ulkeKodu: new FormControl("", [
        
          Validators.maxLength(4),
        ]),
  
        nctsBeyanInternalNo: new FormControl(
          this.nctsBeyanInternalNo,
          []
        ),
        kalemInternalNo: new FormControl(
          this.kalemInternalNo,
          []
        ),
      }),
       this.gondericiFirmaForm = this._fb.group({
        adUnvan: new FormControl("", [
      
          Validators.maxLength(150),
        ]),
        caddeSokakNo: new FormControl("", [
        
          Validators.maxLength(150),
        ]),
   
        ilIlce: new FormControl("", [
         
          Validators.maxLength(35),
        ]),
        dil: new FormControl("", [
        
          Validators.maxLength(4),
        ]),
        no: new FormControl("", [
        
          Validators.maxLength(15),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        postaKodu: new FormControl("", [
         
          Validators.maxLength(10),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        ulkeKodu: new FormControl("", [
     
          Validators.maxLength(4),
        ]),
  
        nctsBeyanInternalNo: new FormControl(
          this.nctsBeyanInternalNo,
          []
        ),
        kalemInternalNo: new FormControl(
          this.kalemInternalNo,
          []
        ),
      })
      this.guvenlialiciFirmaForm = this._fb.group({
        adUnvan: new FormControl("", [
         
          Validators.maxLength(150),
        ]),
        caddeSokakNo: new FormControl("", [
        
          Validators.maxLength(150),
        ]),
   
        ilIlce: new FormControl("", [
       
          Validators.maxLength(35),
        ]),
        dil: new FormControl("", [
       
          Validators.maxLength(4),
        ]),
        no: new FormControl("", [
        
          Validators.maxLength(15),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        postaKodu: new FormControl("", [
      
          Validators.maxLength(10),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        ulkeKodu: new FormControl("", [
     
          Validators.maxLength(4),
        ]),
  
        nctsBeyanInternalNo: new FormControl(
          this.nctsBeyanInternalNo,
          []
        ),
        kalemInternalNo: new FormControl(
          this.kalemInternalNo,
          []
        ),
      }),
       this.guvenligondericiFirmaForm = this._fb.group({
        adUnvan: new FormControl("", [
        
          Validators.maxLength(150),
        ]),
        caddeSokakNo: new FormControl("", [
        
          Validators.maxLength(150),
        ]),
   
        ilIlce: new FormControl("", [
      
          Validators.maxLength(35),
        ]),
        dil: new FormControl("", [
       
          Validators.maxLength(4),
        ]),
        no: new FormControl("", [
        
          Validators.maxLength(15),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        postaKodu: new FormControl("", [
        
          Validators.maxLength(10),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        ulkeKodu: new FormControl("", [
       
          Validators.maxLength(4),
        ]),
  
        nctsBeyanInternalNo: new FormControl(
          this.nctsBeyanInternalNo,
          []
        ),
        kalemInternalNo: new FormControl(
          this.kalemInternalNo,
          []
        ),
      }),
      (this.konteynerForm = this._fb.group({
        konteynerArry: this._fb.array([this.getKonteyner()]),
      })),
      (this.hassasEsyaForm = this._fb.group({
        hassasEsyaArry: this._fb.array([this.getHassasEsya()]),
      })), 
       (this.kapForm = this._fb.group({
        kapArry: this._fb.array([this.getKap()]),
      })),
      (this.ekBilgiForm = this._fb.group({
        ekBilgiArry: this._fb.array([this.getEkBilgi()]),
      })),
      (this.belgeForm = this._fb.group({
        belgeArry: this._fb.array([this.getBelge()]),
      })),
      (this.oncekiBelgeForm = this._fb.group({
        oncekiBelgeArry: this._fb.array([this.getOncekiBelge()]),
      }))

      
        
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
      this.router.navigateByUrl("/app/nctsbeyan");
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

  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;

    if(this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
    return true;
    else return false;
  }
  get BeyanSilDuzeltStatu():boolean {
  
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if ((this.kalemInternalNo!='' && this.kalemInternalNo!='Boş' && this.kalemInternalNo!=null && this.kalemInternalNo!='undefined') && (this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress))
      return true;
     else
       return false;
   
  }
  disableItem() {
    this.kalemForm.disable();
    this.konteynerForm.disable();
    this.hassasEsyaForm.disable();
    this.kapForm.disable();
    this.ekBilgiForm.disable();
    this.aliciFirmaForm.disable();
    this.gondericiFirmaForm.disable();
    this.guvenlialiciFirmaForm.disable();
    this.guvenligondericiFirmaForm.disable();
    this.belgeForm.disable();
    this.oncekiBelgeForm.disable();
  }
  enableItem() {
    this.kalemForm.enable();
    this.konteynerForm.enable();
    this.hassasEsyaForm.enable();
    this.kapForm.enable();
    this.ekBilgiForm.enable();
    this.aliciFirmaForm.enable();
    this.gondericiFirmaForm.enable();
    this.guvenlialiciFirmaForm.enable();
    this.guvenligondericiFirmaForm.enable();
    this.belgeForm.enable();
    this.oncekiBelgeForm.enable();
  }
  resetItem() {

    this.kalemInternalNo = "Boş";
    this.kalemSiraNo = 0;
    this.sentNo="";
     this.kalemForm.reset();   
     this.konteynerForm.reset();
     this.hassasEsyaForm.reset();
     this.kapForm.reset();
     this.ekBilgiForm.reset();
     this.aliciFirmaForm.reset();
     this.gondericiFirmaForm.reset();
     this.guvenlialiciFirmaForm.reset();
     this.guvenligondericiFirmaForm.reset();
     this.belgeForm.reset();
     this.oncekiBelgeForm.reset();
     const formkonteynerArray = this.konteynerForm.get("konteynerArry") as FormArray;
     formkonteynerArray.clear();
     this.konteynerForm.setControl("konteynerArry", formkonteynerArray);
     const formHassasEsyaArray = this.hassasEsyaForm.get("hassasEsyaArry") as FormArray;
     formHassasEsyaArray.clear();
     this.hassasEsyaForm.setControl("hassasEsyaArry", formHassasEsyaArray);
     const formKapArray = this.kapForm.get("kapArry") as FormArray;
     formKapArray.clear();
     this.kapForm.setControl("kapArry", formKapArray);
     const formEkBilgiArray = this.ekBilgiForm.get("ekBilgiArry") as FormArray;
     formEkBilgiArray.clear();
     this.ekBilgiForm.setControl("ekBilgiArry", formEkBilgiArray);
     const formBelgeArray = this.belgeForm.get("belgeArry") as FormArray;
     formBelgeArray.clear();
     this.belgeForm.setControl("belgeArry", formBelgeArray);
     const formOncekiBelgeArray = this.oncekiBelgeForm.get("oncekiBelgeArry") as FormArray;
     formOncekiBelgeArray.clear();
     this.oncekiBelgeForm.setControl("oncekiBelgeArry", formOncekiBelgeArray);
    
  }
  getKalemler(islemInternalNo: string) {
    this.beyanServis.getNbKalem(islemInternalNo).subscribe(
      (result: NbKalemDto[]) => {
        this._kalemler = result;
        this.disableItem();
        if (this._kalemler.length > 0) this.getKalem(1);
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
  getKalem(kalemSiraNo) {
   
    this.kalemInternalNo = this._kalemler[kalemSiraNo - 1].kalemInternalNo;
    this.kalemSiraNo = this._kalemler[kalemSiraNo - 1].kalemSiraNo;
 
    this.kalemForm.setValue({
      nctsBeyanInternalNo: this._kalemler[kalemSiraNo - 1].nctsBeyanInternalNo,
      kalemInternalNo: this._kalemler[kalemSiraNo - 1].kalemInternalNo,
      kalemSiraNo: this._kalemler[kalemSiraNo - 1].kalemSiraNo,
      gtip:this._kalemler[kalemSiraNo - 1].gtip,
      rejimKodu: this._kalemler[kalemSiraNo - 1].rejimKodu,
      ticariTanim: this._kalemler[kalemSiraNo - 1].ticariTanim,
      ticariTanimDil: this._kalemler[kalemSiraNo - 1].ticariTanimDil,
      kiymet: this._kalemler[kalemSiraNo - 1].kiymet,
      kiymetDoviz: this._kalemler[kalemSiraNo - 1].kiymetDoviz,
      burutAgirlik: this._kalemler[kalemSiraNo - 1].burutAgirlik,
      netAgirlik: this._kalemler[kalemSiraNo - 1].netAgirlik,
      varisUlkesi: this._kalemler[kalemSiraNo - 1].varisUlkesi,
      cikisUlkesi: this._kalemler[kalemSiraNo - 1].cikisUlkesi,
      tptChMOdemeKod: this._kalemler[kalemSiraNo - 1].tptChMOdemeKod,
      konsimentoRef: this._kalemler[kalemSiraNo - 1].konsimentoRef,
      undg:this._kalemler[kalemSiraNo - 1].undg,
      ihrBeyanNo: this._kalemler[kalemSiraNo - 1].ihrBeyanNo,
      ihrBeyanTip: this._kalemler[kalemSiraNo - 1].ihrBeyanTip,
      ihrBeyanParcali: this._kalemler[kalemSiraNo - 1].ihrBeyanParcali,

      
    });

    this.beyanServis.getNbKalemAliciFirma(this._beyanSession.islemInternalNo, this.kalemInternalNo).subscribe(
      (result: NbKalemAliciFirmaDto) => {
       
        this.LoadAliciFirma(result);
        this.aliciFirmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbKalemGondericiFirma(this._beyanSession.islemInternalNo, this.kalemInternalNo).subscribe(
      (result: NbKalemGondericiFirmaDto) => {
       
        this.LoadGondericiFirma(result);
        this.gondericiFirmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbKalemGuvenliAliciFirma(this._beyanSession.islemInternalNo, this.kalemInternalNo).subscribe(
      (result: NbKalemGuvenliAliciFirmaDto) => {
       
        this.LoadGuvenliAliciFirma(result);
        this.guvenlialiciFirmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbKalemGuvenliGondericiFirma(this._beyanSession.islemInternalNo, this.kalemInternalNo).subscribe(
      (result: NbKalemGuvenliGondericiFirmaDto) => {
       
        this.LoadGuvenliGondericiFirma(result);
        this.guvenligondericiFirmaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbKonteyner(this._beyanSession.islemInternalNo).subscribe(
      (result: NbKonteynerDto[]) => {
        this._konteynerler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initKonteynerFormArray(this._konteynerler);
        this.konteynerForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbHassasEsya(this._beyanSession.islemInternalNo).subscribe(
      (result: NbHassasEsyaDto[]) => {
        this._hassasEsyalar = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initHassasEsyaFormArray(this._hassasEsyalar);
        this.hassasEsyaForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbKap(this._beyanSession.islemInternalNo).subscribe(
      (result: NbKapDto[]) => {
        this._kaplar = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initKapFormArray(this._kaplar);
        this.kapForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbEkBilgi(this._beyanSession.islemInternalNo).subscribe(
      (result: NbEkBilgiDto[]) => {
        this._ekBilgiler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initEkBilgiFormArray(this._ekBilgiler);
        this.ekBilgiForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbBelgeler(this._beyanSession.islemInternalNo).subscribe(
      (result: NbBelgelerDto[]) => {
        this._belgeler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initBelgeFormArray(this._belgeler);
        this.belgeForm.disable();
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );

    this.beyanServis.getNbOncekiBelgeler(this._beyanSession.islemInternalNo).subscribe(
      (result: NbOncekiBelgelerDto[]) => {
        this._oncekiBelgeler = result.filter(
          (x) =>
            x.kalemInternalNo === this._kalemler[kalemSiraNo - 1].kalemInternalNo
        );
        
        this.initOncekiBelgeFormArray(this._oncekiBelgeler);
        this.oncekiBelgeForm.disable();
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
    this.kalemSiraNo = 0;
    this.sentNo="";
    this.enableItem();
    this.resetItem();
    this.kalemForm.markAllAsTouched();
    this.konteynerForm.markAllAsTouched();
    this.aliciFirmaForm.markAllAsTouched();
    this.gondericiFirmaForm.markAllAsTouched();
    this.guvenlialiciFirmaForm.markAllAsTouched();
    this.guvenligondericiFirmaForm.markAllAsTouched();
    this.hassasEsyaForm.markAllAsTouched();
    this.kapForm.markAllAsTouched();
    this.ekBilgiForm.markAllAsTouched();
    this.belgeForm.markAllAsTouched();
    this.oncekiBelgeForm.markAllAsTouched();
  }

  duzeltKalem() {
    this.enableItem();
    this.konteynerForm.markAllAsTouched();
    this.aliciFirmaForm.markAllAsTouched();
    this.gondericiFirmaForm.markAllAsTouched();
    this.guvenlialiciFirmaForm.markAllAsTouched();
    this.guvenligondericiFirmaForm.markAllAsTouched();
    this.hassasEsyaForm.markAllAsTouched();
    this.kapForm.markAllAsTouched();
    this.ekBilgiForm.markAllAsTouched();
    this.belgeForm.markAllAsTouched();
    this.oncekiBelgeForm.markAllAsTouched();
  }

  silKalem(kalemInternalNo: string,tasimaSenetNo:string) {
    if (
      confirm(
        tasimaSenetNo + "- Taşıma Senedini Silmek İstediğinizden Eminmisiniz?"
      )
    ) {
      const promise = this.beyanServis
        .removeNbKalem(kalemInternalNo, this._beyanSession.nctsBeyanInternalNo)
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
      .get("nctsBeyanInternalNo")
      .setValue(this._beyanSession.nctsBeyanInternalNo);
    
    this.kalemForm.get("kalemSiraNo").setValue(this.kalemSiraNo);
    this.kalemForm
      .get("kalemInternalNo")
      .setValue(this.kalemInternalNo);

    let kiymet = this.kalemForm.get("kiymet").value;
    this.kalemForm
      .get("kiymet")
      .setValue(
        typeof kiymet == "string"
          ? parseFloat(kiymet)
          : kiymet
      );

    let burutAgirlik = this.kalemForm.get("burutAgirlik").value;
    this.kalemForm
      .get("burutAgirlik")
      .setValue(
        typeof burutAgirlik == "string"
          ? parseFloat(burutAgirlik)
          : burutAgirlik
      );

      let netAgirlik = this.kalemForm.get("netAgirlik").value;
    this.kalemForm
      .get("netAgirlik")
      .setValue(
        typeof netAgirlik == "string"
          ? parseFloat(netAgirlik)
          : netAgirlik
      );
   

    let yenikalemInternalNo: string;
    let yeniKalem = new NbKalemDto();
   
    yeniKalem.init(this.kalemForm.value);
   
    const promiseKalem = this.beyanServis
      .restoreNbKalem(yeniKalem)
      .toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kalemServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenikalemInternalNo = kalemServisSonuc.ReferansNo;

        if (yenikalemInternalNo != null) {
          this.kalemInternalNo = yenikalemInternalNo;
          this.setAliciFirma();   
          this.setGondericiFirma();   
          this.setGuvenliAliciFirma();   
          this.setGuvenliGondericiFirma();  
          this.setKonteyner();
          this.setKap();
          this.setEkBilgi();
          this.setBelge();
          this.setOncekiBelge();
          this.setHassasEsya();
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
  gtipLeave(gtip) {
    console.log("gtip leave:" + gtip);
  }

    LoadAliciFirma(firma:NbKalemAliciFirmaDto)
    {
      this.aliciFirmaForm.reset();
      if(firma!=null)
       this.aliciFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        kalemInternalNo:firma.kalemInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
    setAliciFirma()
    {
      if (this.aliciFirmaForm.invalid) {
        const invalid = [];
        const controls = this.aliciFirmaForm.controls;
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
  
      this.aliciFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
      this.aliciFirmaForm.get("kalemInternalNo").setValue(this.kalemInternalNo);    
     
      const promise = this.beyanServis
      .restoreNbKalemAliciFirma(
        this.aliciFirmaForm.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promise.then(
      (result) => {
  
        this.aliciFirmaForm.disable();
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
    }
    LoadGondericiFirma(firma:NbKalemGondericiFirmaDto)
    {
      this.gondericiFirmaForm.reset();
      if(firma!=null)
       this.gondericiFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        kalemInternalNo:firma.kalemInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
    setGondericiFirma()
    {
      if (this.gondericiFirmaForm.invalid) {
        const invalid = [];
        const controls = this.gondericiFirmaForm.controls;
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
  
      this.gondericiFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
      this.gondericiFirmaForm.get("kalemInternalNo").setValue(this.kalemInternalNo);    
     
      const promise = this.beyanServis
      .restoreNbKalemGondericiFirma(
        this.gondericiFirmaForm.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promise.then(
      (result) => {
  
        this.gondericiFirmaForm.disable();
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
    }
    LoadGuvenliAliciFirma(firma:NbKalemGuvenliAliciFirmaDto)
    {
      this.guvenlialiciFirmaForm.reset();
      if(firma!=null)
       this.guvenlialiciFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        kalemInternalNo:firma.kalemInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
    setGuvenliAliciFirma()
    {
      if (this.guvenlialiciFirmaForm.invalid) {
        const invalid = [];
        const controls = this.guvenlialiciFirmaForm.controls;
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
  
      this.guvenlialiciFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
      this.guvenlialiciFirmaForm.get("kalemInternalNo").setValue(this.kalemInternalNo);    
     
      const promise = this.beyanServis
      .restoreNbKalemGuvenliAliciFirma(
        this.guvenlialiciFirmaForm.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promise.then(
      (result) => {
  
        this.guvenlialiciFirmaForm.disable();
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
    }
    LoadGuvenliGondericiFirma(firma:NbKalemGuvenliGondericiFirmaDto)
    {
      this.guvenligondericiFirmaForm.reset();
      if(firma!=null)
       this.guvenligondericiFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        kalemInternalNo:firma.kalemInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
    setGuvenliGondericiFirma()
    {
      if (this.guvenligondericiFirmaForm.invalid) {
        const invalid = [];
        const controls = this.guvenligondericiFirmaForm.controls;
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
  
      this.guvenligondericiFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
      this.guvenligondericiFirmaForm.get("kalemInternalNo").setValue(this.kalemInternalNo);    
     
      const promise = this.beyanServis
      .restoreNbKalemGuvenliGondericiFirma(
        this.guvenligondericiFirmaForm.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promise.then(
      (result) => {
  
        this.guvenligondericiFirmaForm.disable();
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
    }


//#region Konteyner

initKonteynerFormArray(ihracat: NbKonteynerDto[]) {
  const formArray = this.konteynerForm.get("konteynerArry") as FormArray;
  formArray.clear();
  for (let klm of ihracat) {
    let formGroup: FormGroup = new FormGroup({
      
      konteynerNo: new FormControl(klm.konteynerNo, [Validators.required,Validators.maxLength(50)]),
      ulke: new FormControl(klm.ulke, [Validators.maxLength(4)]),
     
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.konteynerForm.setControl("konteynerArry", formArray);
}

getKonteyner() {
  return this._fb.group({
 
    konteynerNo: new FormControl("", [Validators.required,Validators.maxLength(50)]),
    ulke: new FormControl("", [ Validators.maxLength(4)]),
  
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
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

      
      }
    }
  }

  if (this.konteynerBilgileri.length >= 0) {
    const promiseIhracat = this.beyanServis
      .restoreNbKonteyner(
        this.konteynerBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseIhracat.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Konteyner

//#region HassasEsya

initHassasEsyaFormArray(hesya: NbHassasEsyaDto[]) {
  const formArray = this.hassasEsyaForm.get("hassasEsyaArry") as FormArray;
  formArray.clear();
  for (let klm of hesya) {
    let formGroup: FormGroup = new FormGroup({
      
      kod: new FormControl(klm.kod, [Validators.required, ValidationService.numberValidator]),
      miktar: new FormControl(klm.miktar, [Validators.required, ValidationService.decimalValidation]),
     
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.hassasEsyaForm.setControl("hassasEsyaArry", formArray);
}

getHassasEsya() {
  return this._fb.group({
 
    kod: new FormControl(0, [Validators.required, ValidationService.numberValidator]),
    miktar: new FormControl(0, [Validators.required, ValidationService.decimalValidation]),
  
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
    kalemInternalNo: new FormControl(this.kalemInternalNo, [
      Validators.required,
    ]),
  });
}

get hassasEsyaBilgileri() {
  return this.hassasEsyaForm.get("hassasEsyaArry") as FormArray;
}

addHassasEsyaField() {
  this.hassasEsyaBilgileri.push(this.getHassasEsya());
}

deleteHassasEsyaField(index: number) {
  this.hassasEsyaBilgileri.removeAt(index);
}

setHassasEsya() {
  if (this.hassasEsyaBilgileri.length > 0) {
    for (let klm of this.hassasEsyaBilgileri.value) {
      klm.kalemInternalNo = this.kalemInternalNo;
      klm.kod =
      typeof klm.kod == "string"
        ? parseInt(klm.kod)
        : klm.kod;

      klm.miktar =
      typeof klm.miktar == "string"
        ? parseFloat(klm.miktar)
        : klm.miktar;
    }

    this.initHassasEsyaFormArray(this.hassasEsyaBilgileri.value);

    if (this.hassasEsyaBilgileri.invalid) {
      const invalid = [];
      const controls = this.hassasEsyaBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Hassas Eşya Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.hassasEsyaBilgileri.length >= 0) {
    const promiseHassasEsya = this.beyanServis
      .restoreNbHassasEsya(
        this.hassasEsyaBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseHassasEsya.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion HassasEsya

//#region Kap

initKapFormArray(kap: NbKapDto[]) {
  const formArray = this.kapForm.get("kapArry") as FormArray;
  formArray.clear();
  for (let klm of kap) {
    let formGroup: FormGroup = new FormGroup({
      
      markaNo: new FormControl(klm.markaNo, [ Validators.maxLength(42)]),
      markaDil: new FormControl(klm.markaDil, [Validators.maxLength(4)]),
      kapTipi: new FormControl(klm.kapTipi, [Validators.required,Validators.maxLength(3)]),
      kapAdet: new FormControl(klm.kapAdet, [Validators.required,ValidationService.numberValidator]),
      parcaSayisi: new FormControl(klm.parcaSayisi, [Validators.required,ValidationService.numberValidator]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.kapForm.setControl("kapArry", formArray);
}

getKap() {
  return this._fb.group({
 
      markaNo: new FormControl("", [ Validators.maxLength(42)]),
      markaDil: new FormControl("", [Validators.maxLength(4)]),
      kapTipi: new FormControl("", [Validators.required, Validators.maxLength(3)]),
      kapAdet: new FormControl(0, [Validators.required,ValidationService.numberValidator]),
      parcaSayisi: new FormControl(0, [Validators.required,ValidationService.numberValidator]),
  
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
    kalemInternalNo: new FormControl(this.kalemInternalNo, [
      Validators.required,
    ]),
  });
}

get kapBilgileri() {
  return this.kapForm.get("kapArry") as FormArray;
}

addKapField() {
  this.kapBilgileri.push(this.getKap());
}

deleteKapField(index: number) {
  this.kapBilgileri.removeAt(index);
}

setKap() {
  if (this.kapBilgileri.length > 0) {
    for (let klm of this.kapBilgileri.value) {
      klm.kalemInternalNo = this.kalemInternalNo;
      klm.parcaSayisi =
      typeof klm.parcaSayisi == "string"
        ? parseInt(klm.parcaSayisi)
        : klm.parcaSayisi;

      klm.kapAdet =
      typeof klm.kapAdet == "string"
        ? parseFloat(klm.kapAdet)
        : klm.kapAdet;
    }

    this.initKapFormArray(this.kapBilgileri.value);

    if (this.kapBilgileri.invalid) {
      const invalid = [];
      const controls = this.kapBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Kap Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.kapBilgileri.length >= 0) {
    const promiseKap = this.beyanServis
      .restoreNbKap(
        this.kapBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseKap.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Kap

//#region EkBilgi

initEkBilgiFormArray(ekBilgi: NbEkBilgiDto[]) {
  const formArray = this.ekBilgiForm.get("ekBilgiArry") as FormArray;
  formArray.clear();
  for (let klm of ekBilgi) {
    let formGroup: FormGroup = new FormGroup({
      
      ekBilgi: new FormControl(klm.ekBilgi, [Validators.required, Validators.maxLength(70)]),
      dil: new FormControl(klm.dil, [Validators.maxLength(4)]),
      ekBilgiKod: new FormControl(klm.ekBilgiKod, [Validators.required,Validators.maxLength(5)]),
      ec2Ihr: new FormControl(klm.ec2Ihr, [ValidationService.numberValidator]),
      ulkeKodu: new FormControl(klm.ulkeKodu, [Validators.maxLength(4)]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.ekBilgiForm.setControl("ekBilgiArry", formArray);
}

getEkBilgi() {
  return this._fb.group({
 
    ekBilgi: new FormControl("", [Validators.required, Validators.maxLength(70)]),
    dil: new FormControl("", [Validators.maxLength(4)]),
    ekBilgiKod: new FormControl("", [Validators.required,Validators.maxLength(5)]),
    ec2Ihr: new FormControl(0, [ValidationService.numberValidator]),
    ulkeKodu: new FormControl("", [Validators.maxLength(4)]),
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
    kalemInternalNo: new FormControl(this.kalemInternalNo, [
      Validators.required,
    ]),
  });
}

get ekBilgiBilgileri() {
  return this.ekBilgiForm.get("ekBilgiArry") as FormArray;
}

addEkBilgiField() {
  this.ekBilgiBilgileri.push(this.getEkBilgi());
}

deleteEkBilgiField(index: number) {
  this.ekBilgiBilgileri.removeAt(index);
}

setEkBilgi() {
  if (this.ekBilgiBilgileri.length > 0) {
    for (let klm of this.ekBilgiBilgileri.value) {
      klm.kalemInternalNo = this.kalemInternalNo;
      klm.ec2Ihr =
      typeof klm.ec2Ihr == "string"
        ? parseInt(klm.ec2Ihr)
        : klm.ec2Ihr;

     
    }

    this.initEkBilgiFormArray(this.ekBilgiBilgileri.value);

    if (this.ekBilgiBilgileri.invalid) {
      const invalid = [];
      const controls = this.ekBilgiBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Ek Bilgi Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.ekBilgiBilgileri.length >= 0) {
    const promiseEkBilgi = this.beyanServis
      .restoreNbEkBilgi(
        this.ekBilgiBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseEkBilgi.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion EkBilgi

//#region Belgeler

initBelgeFormArray(belge: NbBelgelerDto[]) {
  const formArray = this.belgeForm.get("belgeArry") as FormArray;
  formArray.clear();
  for (let klm of belge) {
    let formGroup: FormGroup = new FormGroup({
      
      belgeTipi: new FormControl(klm.belgeTipi, [Validators.required, Validators.maxLength(4)]),
      belgeDil: new FormControl(klm.belgeDil, [Validators.maxLength(4)]),
      refNo: new FormControl(klm.refNo, [Validators.maxLength(70)]),
      tamamlayiciOlcu: new FormControl(klm.tamamlayiciOlcu, [Validators.maxLength(26)]),
      tamamlayiciOlcuDil: new FormControl(klm.tamamlayiciOlcuDil, [Validators.maxLength(4)]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.belgeForm.setControl("belgeArry", formArray);
}

getBelge() {
  return this._fb.group({
 
    belgeTipi: new FormControl("", [Validators.required, Validators.maxLength(4)]),
    belgeDil: new FormControl("", [Validators.maxLength(4)]),
    refNo: new FormControl("", [Validators.maxLength(70)]),
    tamamlayiciOlcu: new FormControl("", [Validators.maxLength(26)]),
    tamamlayiciOlcuDil: new FormControl("", [Validators.maxLength(4)]),
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
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

      
      }
    }
  }

  if (this.belgeBilgileri.length >= 0) {
    const promiseBelge = this.beyanServis
      .restoreNbBelgeler(
        this.belgeBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseBelge.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Belgeler

//#region Önceki Belgeler

initOncekiBelgeFormArray(belge: NbOncekiBelgelerDto[]) {
  const formArray = this.oncekiBelgeForm.get("oncekiBelgeArry") as FormArray;
  formArray.clear();
  for (let klm of belge) {
    let formGroup: FormGroup = new FormGroup({
      
      belgeTipi: new FormControl(klm.belgeTipi, [Validators.required, Validators.maxLength(4)]),
      belgeDil: new FormControl(klm.belgeDil, [Validators.maxLength(4)]),
      refNo: new FormControl(klm.refNo, [Validators.required,Validators.maxLength(70)]),
      tamamlayiciBilgi: new FormControl(klm.tamamlayiciBilgi, [Validators.maxLength(26)]),
      tamamlayiciBilgiDil: new FormControl(klm.tamamlayiciBilgiDil, [Validators.maxLength(4)]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      kalemInternalNo: new FormControl(klm.kalemInternalNo),
    });

    formArray.push(formGroup);
  }
  this.oncekiBelgeForm.setControl("oncekiBelgeArry", formArray);
}

getOncekiBelge() {
  return this._fb.group({
 
    belgeTipi: new FormControl("", [Validators.required, Validators.maxLength(4)]),
    belgeDil: new FormControl("", [Validators.maxLength(4)]),
    refNo: new FormControl("", [Validators.required,Validators.maxLength(70)]),
    tamamlayiciBilgi: new FormControl("", [Validators.maxLength(26)]),
    tamamlayiciBilgiDil: new FormControl("", [Validators.maxLength(4)]),
    nctsBeyanInternalNo: new FormControl(this._beyanSession.nctsBeyanInternalNo, [
      Validators.required,
    ]),
    kalemInternalNo: new FormControl(this.kalemInternalNo, [
      Validators.required,
    ]),
  });
}

get oncekiBelgeBilgileri() {
  return this.oncekiBelgeForm.get("oncekiBelgeArry") as FormArray;
}

addOncekiBelgeField() {
  this.oncekiBelgeBilgileri.push(this.getOncekiBelge());
}

deleteOncekiBelgeField(index: number) {
  this.oncekiBelgeBilgileri.removeAt(index);
}

setOncekiBelge() {
  if (this.oncekiBelgeBilgileri.length > 0) {
    for (let klm of this.oncekiBelgeBilgileri.value) {
      klm.kalemInternalNo = this.kalemInternalNo;
    
    }

    this.initOncekiBelgeFormArray(this.oncekiBelgeBilgileri.value);

    if (this.oncekiBelgeBilgileri.invalid) {
      const invalid = [];
      const controls = this.oncekiBelgeBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Önceki Belge Bilgileri verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

      
      }
    }
  }

  if (this.oncekiBelgeBilgileri.length >= 0) {
    const promiseBelge = this.beyanServis
      .restoreNbOncekiBelgeler(
        this.oncekiBelgeBilgileri.value,
        this.kalemInternalNo,
        this._beyanSession.nctsBeyanInternalNo
      )
      .toPromise();
      promiseBelge.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
        // this.konteynerForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Önceki Belgeler


  onReset() {
    this.submitted = false;
  }
  
}
