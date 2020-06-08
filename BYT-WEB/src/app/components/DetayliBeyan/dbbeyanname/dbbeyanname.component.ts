import { Component, OnInit, ViewChild,ElementRef,Injectable } from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,

} from "@angular/forms";

import {
  ReferansService
} from "../../../../shared/helpers/ReferansService";

import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { GirisService } from '../../../../account/giris/giris.service';

import { MatSnackBar } from "@angular/material/snack-bar";
import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
import {
  ValidationService
} from "../../../../shared/service-proxies/ValidationService";
import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemDto,
  ServisDto
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";


export const PICK_FORMATS = {
  parse: {
    dateInput: {month: 'short', year: 'numeric', day: 'numeric'}
},
display: {
    // dateInput: { month: 'short', year: 'numeric', day: 'numeric' },
    dateInput: 'input',
    // monthYearLabel: { month: 'short', year: 'numeric', day: 'numeric' },
    monthYearLabel: 'inputMonth',
    dateA11yLabel: {year: 'numeric', month: 'long', day: 'numeric'},
    monthYearA11yLabel: {year: 'numeric', month: 'long'},
}
};
@Injectable() 
class PickDateAdapter extends NativeDateAdapter {
  parse(value: any): Date | null {
    if ((typeof value === 'string') && (value.indexOf('/') > -1)) {
      const str = value.split('/');
      const year = Number(str[2]);
      const month = Number(str[1]) - 1;
      const date = Number(str[0]);
      return new Date(year, month, date);
    }
    const timestamp = typeof value === 'number' ? value : Date.parse(value);
    return isNaN(timestamp) ? null : new Date(timestamp);
  }
format(date: Date, displayFormat: string): string {
   if (displayFormat == "input") {
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();
      return this._to2digit(day) + '/' + this._to2digit(month) + '/' + year;
   } else if (displayFormat == "inputMonth") {
      let month = date.getMonth() + 1;
      let year = date.getFullYear();
      return  this._to2digit(month) + '/' + year;
   } else {
       return date.toDateString();
   }
}

  private _to2digit(n: number) {
    return ('00' + n).slice(-2);
  }
}

export const ihracatRejim=[
  {rejim:'1000'},
  {rejim:'1021'},
  {rejim:'1023'},
  {rejim:'1040'},
  {rejim:'1042'},
  {rejim:'1072'},
  {rejim:'1091'},
  {rejim:'2100'},
  {rejim:'2123'},
  {rejim:'2141'},
  {rejim:'2151'},
  {rejim:'2152'},
  {rejim:'2153'},
  {rejim:'2172'},
  {rejim:'2191'},
  {rejim:'2300'},
  {rejim:'2340'},
  {rejim:'2341'},
  {rejim:'2342'},
  {rejim:'2351'},
  {rejim:'2352'},
  {rejim:'2353'},
  {rejim:'2600'},
  {rejim:'3141'},
  {rejim:'3151'},
  {rejim:'3152'},
  {rejim:'3153'},
  {rejim:'3158'},
  {rejim:'3171'},
  {rejim:'7200'},
  {rejim:'7272'},
  {rejim:'8100'},
  
  ]
export const ithalatRejim=[
  {rejim:'4000'},
  {rejim:'4010'},
  {rejim:'4051'},
  {rejim:'4053'},
  {rejim:'4058'},
  {rejim:'4071'},
  {rejim:'4072'},
  {rejim:'4091'},
  {rejim:'4100'},
  {rejim:'4121'},
  {rejim:'4123'},
  {rejim:'4171'},
  {rejim:'4191'},
  {rejim:'4200'},
  {rejim:'4210'},
  {rejim:'4251'},
  {rejim:'4253'},
  {rejim:'4258'},
  {rejim:'4271'},
  {rejim:'4291'},
  {rejim:'5100'},
  {rejim:'5121'},
  {rejim:'5123'},
  {rejim:'5141'},
  {rejim:'5171'},
  {rejim:'5191'},
  {rejim:'5200'},
  {rejim:'5221'},
  {rejim:'5223'},
  {rejim:'5271'},
  {rejim:'5291'},
  {rejim:'5300'},
  {rejim:'5321'},
  {rejim:'5323'},
  {rejim:'5341'},
  {rejim:'5351'},
  {rejim:'5352'},
  {rejim:'5353'},
  {rejim:'5358'},
  {rejim:'5371'},
  {rejim:'5391'},
  {rejim:'5800'},
  {rejim:'6121'},
  {rejim:'6123'},
  {rejim:'6321'},
  {rejim:'6323'},
  {rejim:'6326'},
  {rejim:'6521'},
  {rejim:'6523'},
  {rejim:'6771'},
  {rejim:'7100'},
  {rejim:'7121'},
  {rejim:'7123'},
  {rejim:'7141'},
  {rejim:'7151'},
  {rejim:'7153'},
  {rejim:'7158'},
  {rejim:'7171'},
  {rejim:'7191'}, 
  {rejim:'7241'},
  {rejim:'7252'},
  {rejim:'7300'},
  {rejim:'7373'},
  {rejim:'8200'},
  {rejim:'9100'},
  {rejim:'9171'},
  
  ]
@Component({
  selector: "app-beyanname",
  templateUrl: "./dbbeyanname.component.html",
  styleUrls: ["./dbbeyanname.component.scss"],
  providers: [
    {provide: DateAdapter, useClass: PickDateAdapter},
    {provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS}
 ]
})
export class DbBeyannameComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  beyannameForm: FormGroup;
  submitted: boolean = false;
  ihracatEditable: boolean = false;
  ithalatEditable: boolean = false;
  beyanInternalNo:string;
  beyanStatu:string;
  editable: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _beyanname: BeyannameDto = new BeyannameDto();
  _kalemler: KalemDto[];
  _rejimList = this.referansService.getRejimJSON();
  _bsList = this.referansService.getBsJSON();
  _gumrukList =this.referansService.getGumrukJSON();
  _ulkeList = this.referansService.getUlkeJSON();
  _aliciSaticiList = this.referansService.getaliciSaticiJSON();
  _tasimaSekliList = this.referansService.gettasimaSekliJSON();
  _isleminNiteligiList = this.referansService.getisleminNiteligiJSON();
  _aracTipiList = this.referansService.getaracTipiJSON();
  _teslimList = this.referansService.getteslimSekliJSON();
  _dovizList=this.referansService.getdovizCinsiJSON();


  constructor(
    private referansService:ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private _userRoles:UserRoles,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _fb: FormBuilder,
    private  girisService: GirisService,  
  
  ) {
    
   
  }
  get focus() {
    return this.beyannameForm.controls;
  }
  buildForm(): void {
    this.beyannameForm = this.formBuilder.group(
      {
        //Genel Bilgiler
        beyannameNo: [],
        beyanInternalNo: [],
        kullanici: new FormControl(""),
        gumruk: new FormControl("", [Validators.required]),
        rejim: ["", Validators.required],
        aciklamalar: new FormControl("", [Validators.maxLength(350)]),
        aliciSaticiIliskisi:new FormControl("",  [
          Validators.required,
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        aliciVergiNo: new FormControl("", [Validators.maxLength(20)]),
        gondericiVergiNo: new FormControl("", [Validators.maxLength(20)]),
        musavirVergiNo:  new FormControl("", [Validators.required,Validators.maxLength(20)]),
        beyanSahibiVergiNo: new FormControl("", [
          Validators.required,
          Validators.maxLength(20)
        ]),
        asilSorumluVergiNo: new FormControl("", [Validators.maxLength(20)]),
     
        basitlestirilmisUsul: new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        ticaretUlkesi:  new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        cikisUlkesi: new FormControl("",  [
         
          Validators.minLength(2),
          Validators.maxLength(9),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        gidecegiSevkUlkesi:  new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        gidecegiUlke:  new FormControl("", [
        
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),       
        birlikKayitNumarasi: new FormControl("", [
        
          Validators.maxLength(30)]),
        birlikKriptoNumarasi: new FormControl("", [
          
          Validators.maxLength(30)]),
        isleminNiteligi:new FormControl("", [Validators.required,Validators.maxLength(9)]),   
      //  kapAdedi: new FormControl("", [Validators.pattern("^[0-9]*$")]),   
        kapAdedi: new FormControl("", [ValidationService.numberValidator]),
        musavirReferansNo: new FormControl("", [Validators.required,Validators.maxLength(9)]),     
        referansTarihi: new FormControl("",[ ValidationService.tarihValidation]),
        tescilStatu: [],
        tescilTarihi:[],
        refNo: new FormControl("", [Validators.maxLength(30)]),
        //Finansal Bilgiler
        bankaKodu: new FormControl("", [
          Validators.required,
          Validators.maxLength(16),
          Validators.pattern("^[0-9]*$")
        ]),        
        teslimSekli: new FormControl("", [Validators.required,Validators.maxLength(9)]), 
        teslimSekliYeri:  new FormControl("", [Validators.required,Validators.maxLength(40)]),         
        telafiEdiciVergi:new FormControl("", [ValidationService.decimalValidation]),
       // toplamFatura: new FormControl("", [Validators.required,Validators.pattern('[0-9]+(\.[0-9]{0,2}?)?')]),
        toplamFatura: new FormControl("", [Validators.required,ValidationService.decimalValidation]),
        toplamFaturaDovizi:new FormControl("", [Validators.required]), 
        toplamNavlun: new FormControl("", [ValidationService.decimalValidation]),
        toplamNavlunDovizi: [],
        toplamSigorta: new FormControl("", [ValidationService.decimalValidation]),
        toplamSigortaDovizi: [],
        toplamYurtDisiHarcamalar:new FormControl("", [ValidationService.decimalValidation]),
        toplamYurtDisiHarcamalarDovizi: [],
        toplamYurtIciHarcamalar:new FormControl("", [ValidationService.decimalValidation]),
        odemeAraci:[],       
      
        //Taşıma Bilgileri
        antrepoKodu: new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),   
        limanKodu:new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),               
        sinirdakiAracinUlkesi: new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        cikistakiAracinUlkesi: new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),
        cikistakiAracinKimligi: new FormControl("", [
          Validators.required,
          Validators.maxLength(35)
        ]),
        cikistakiAracinTipi: new FormControl("",[
          Validators.required,
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        sinirdakiAracinKimligi: new FormControl("", [
          Validators.required,
          Validators.maxLength(35)
        ]),
        sinirdakiAracinTipi: new FormControl("", [
          Validators.required,
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        sinirdakiTasimaSekli: new FormControl("", [
          Validators.required,
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        konteyner: [false],
        girisGumrukIdaresi:new FormControl("",[
          Validators.required,
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        varisGumrukIdaresi:new FormControl("",[
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        tasarlananGuzergah: new FormControl("", [Validators.maxLength(250)]),       
        yukBelgeleriSayisi: new FormControl("", [ValidationService.numberValidator]),    
        yuklemeBosaltmaYeri: new FormControl("", [Validators.maxLength(40)]),    
        esyaninBulunduguYer: new FormControl("", [Validators.maxLength(40)]),    
        mobil1: new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          ValidationService.telefonValidation
        ]),
       
        mail1: new FormControl("", [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(80),
          ValidationService.emailValidator
        ])
      }
      
    )
  }
 
  ngOnInit() {
  
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Beyanname Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.beyannameForm.disable();

   
    if (this._beyanSession.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value=this._beyanSession.islemInternalNo;
      this.getBeyannameFromIslem(this._beyanSession.islemInternalNo);
     
    }
  
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  rejimSelect(rejim){
    let ticaret= ihracatRejim.findIndex(x=>x.rejim==rejim)>=0?'E':ithalatRejim.findIndex(x=>x.rejim==rejim)>=0?'I':'';
   
   if(ticaret=='E')
   {
    this.beyannameForm.controls['gondericiVergiNo'].setValidators([Validators.required]);
    this.beyannameForm.controls['gondericiVergiNo'].updateValueAndValidity();
    this.beyannameForm.controls['birlikKayitNumarasi'].setValidators([Validators.required]);
    this.beyannameForm.controls['birlikKayitNumarasi'].updateValueAndValidity();
    this.beyannameForm.controls['birlikKriptoNumarasi'].setValidators([Validators.required]);
    this.beyannameForm.controls['birlikKriptoNumarasi'].updateValueAndValidity();
    this.beyannameForm.controls['cikisUlkesi'].setValue("");
    this.beyannameForm.controls['cikisUlkesi'].clearValidators();
    this.beyannameForm.controls['cikisUlkesi'].updateValueAndValidity();
    this.beyannameForm.controls['gidecegiUlke'].setValidators([Validators.required,
      Validators.minLength(2),
      Validators.maxLength(9),
      Validators.pattern("^[a-zA-Z0-9]*$")]);
    this.beyannameForm.controls['gidecegiUlke'].updateValueAndValidity();
    this.ihracatEditable=true;
    this.ithalatEditable=false;
   }
    else{
      this.beyannameForm.controls['birlikKayitNumarasi'].clearValidators();
      this.beyannameForm.controls['birlikKayitNumarasi'].updateValueAndValidity();
      this.beyannameForm.controls['birlikKriptoNumarasi'].clearValidators();
      this.beyannameForm.controls['birlikKriptoNumarasi'].updateValueAndValidity();
      this.beyannameForm.controls['aliciVergiNo'].setValidators([Validators.required]);
      this.beyannameForm.controls['aliciVergiNo'].updateValueAndValidity();
      this.beyannameForm.controls['gidecegiUlke'].setValue("");
      this.beyannameForm.controls['gidecegiUlke'].clearValidators();
      this.beyannameForm.controls['gidecegiUlke'].updateValueAndValidity();
      this.beyannameForm.controls['cikisUlkesi'].setValidators([Validators.required,
        Validators.minLength(2),
        Validators.maxLength(9),
        Validators.pattern("^[a-zA-Z0-9]*$")]);
      this.beyannameForm.controls['cikisUlkesi'].updateValueAndValidity();
       this.ihracatEditable=false;
       this.ithalatEditable=true;
    }

    if(ticaret !='')
     this.editable=true;
  }
  bsSelect(bs){
      
   if(bs=='2')
   {
    this.beyannameForm.controls['referansTarihi'].setValidators([Validators.required]);
    this.beyannameForm.controls['referansTarihi'].updateValueAndValidity();
   }
   else{
    this.beyannameForm.controls['referansTarihi'].clearValidators();
    this.beyannameForm.controls['referansTarihi'].updateValueAndValidity();
  }
  }
  getBeyannameFromIslem(islemInternalNo:string) {  

    this.beyanServis.getDbBeyanname(islemInternalNo).subscribe(
      result => {
       
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);
        
        this._beyanname = this._beyannameBilgileri.Beyanname;     
    
        if (this._beyanname == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.beyanInternalNo="";
          this.beyanStatu= "" ;             
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.beyanInternalNo= "" ;
          this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
          this._kalemler=this._beyannameBilgileri.Kalemler;
          this._beyanSession.islemInternalNo = islemInternalNo;
          this._beyanSession.Kalemler= this._kalemler;
          this._beyanSession.beyanInternalNo= this._beyanname.beyanInternalNo ;
          this._beyanSession.beyanStatu= this._beyanname.tescilStatu ; 
          this.beyanInternalNo=this._beyanname.beyanInternalNo;
          this.beyanStatu= this._beyanname.tescilStatu ;
          this.loadBeyannameForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getBeyanname(islemInternalNo) {  

      var result= this.beyanServis.getDbBeyanname(islemInternalNo.value).subscribe(
      result => {
       
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);
        
        this._beyanname = this._beyannameBilgileri.Beyanname;   
     
        if (this._beyanname == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.beyanInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.beyanInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
          this._kalemler=this._beyannameBilgileri.Kalemler;
          this._beyanSession.islemInternalNo = islemInternalNo.value;
          this._beyanSession.Kalemler= this._kalemler;
          this._beyanSession.beyanInternalNo= this._beyanname.beyanInternalNo ;
          this._beyanSession.beyanStatu= this._beyanname.tescilStatu ;
          this.beyanInternalNo=this._beyanname.beyanInternalNo;
          this.beyanStatu= this._beyanname.tescilStatu ;
          this.loadBeyannameForm();
          // islemInternalNo.value ="";
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadBeyannameForm()
    {
      this.beyannameForm.setValue({
        beyanInternalNo: this._beyanname.beyanInternalNo,
        beyannameNo: this._beyanname.beyannameNo,
        rejim: this._beyanname.rejim,
        gumruk: this._beyanname.gumruk,
        aciklamalar: this._beyanname.aciklamalar,
        aliciSaticiIliskisi: this._beyanname.aliciSaticiIliskisi,
        aliciVergiNo: this._beyanname.aliciVergiNo,
        antrepoKodu: this._beyanname.antrepoKodu,
        asilSorumluVergiNo: this._beyanname.asilSorumluVergiNo,
        bankaKodu: this._beyanname.bankaKodu,
        basitlestirilmisUsul: this._beyanname.basitlestirilmisUsul,
        beyanSahibiVergiNo: this._beyanname.beyanSahibiVergiNo,
        kullanici: this._beyanname.kullanici,
        gondericiVergiNo: this._beyanname.gondericiVergiNo,    
        birlikKayitNumarasi: this._beyanname.birlikKayitNumarasi,
        birlikKriptoNumarasi: this._beyanname.birlikKriptoNumarasi,
        cikistakiAracinKimligi: this._beyanname.cikistakiAracinKimligi,
        cikistakiAracinTipi: this._beyanname.cikistakiAracinTipi,
        cikistakiAracinUlkesi: this._beyanname.cikistakiAracinUlkesi,
        cikisUlkesi: this._beyanname.cikisUlkesi,
        esyaninBulunduguYer: this._beyanname.esyaninBulunduguYer,
        gidecegiSevkUlkesi: this._beyanname.gidecegiSevkUlkesi,
        gidecegiUlke: this._beyanname.gidecegiUlke,
        girisGumrukIdaresi: this._beyanname.girisGumrukIdaresi,              
        isleminNiteligi: this._beyanname.isleminNiteligi,
        kapAdedi: this._beyanname.kapAdedi,
        konteyner: this._beyanname.konteyner==="EVET"?true:false,
        
        limanKodu: this._beyanname.limanKodu,
        mail1: this._beyanname.mail1,
        // mail2: this._beyanname.mail2,
        // mail3: this._beyanname.mail3,
        mobil1: this._beyanname.mobil1,
        // mobil2: this._beyanname.mobil2,
        musavirVergiNo: this._beyanname.musavirVergiNo,
        odemeAraci: this._beyanname.odemeAraci,
        musavirReferansNo: this._beyanname.musavirReferansNo,
        referansTarihi: this._beyanname.referansTarihi,
        refNo: this._beyanname.refNo,
        sinirdakiAracinKimligi: this._beyanname.sinirdakiAracinKimligi,
        sinirdakiAracinTipi: this._beyanname.sinirdakiAracinTipi,
        sinirdakiAracinUlkesi: this._beyanname.sinirdakiAracinUlkesi,
        sinirdakiTasimaSekli: this._beyanname.sinirdakiTasimaSekli,
        tasarlananGuzergah: this._beyanname.tasarlananGuzergah,
        telafiEdiciVergi: this._beyanname.telafiEdiciVergi,
        tescilStatu: this._beyanname.tescilStatu,
        tescilTarihi: this._beyanname.tescilTarihi,
        teslimSekli: this._beyanname.teslimSekli,
        teslimSekliYeri: this._beyanname.teslimSekliYeri,
        ticaretUlkesi: this._beyanname.ticaretUlkesi,
        toplamFatura: this._beyanname.toplamFatura,
        toplamFaturaDovizi: this._beyanname.toplamFaturaDovizi,
        toplamNavlun: this._beyanname.toplamNavlun,
        toplamNavlunDovizi: this._beyanname.toplamNavlunDovizi,
        toplamSigorta: this._beyanname.toplamSigorta,
        toplamSigortaDovizi: this._beyanname.toplamSigortaDovizi,
        toplamYurtDisiHarcamalar: this._beyanname.toplamYurtDisiHarcamalar,
        toplamYurtDisiHarcamalarDovizi: this._beyanname
          .toplamYurtDisiHarcamalarDovizi,
        toplamYurtIciHarcamalar: this._beyanname.toplamYurtIciHarcamalar,
        varisGumrukIdaresi: this._beyanname.varisGumrukIdaresi,
        yukBelgeleriSayisi: this._beyanname.yukBelgeleriSayisi,
        yuklemeBosaltmaYeri: this._beyanname.yuklemeBosaltmaYeri,
    
      });
  
      this.beyannameForm.disable();

  }
  get BeyanStatu():boolean {
  
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if (this.beyanStatu === 'Olusturuldu' || this.beyanStatu === 'Güncellendi')
     return true;
    else return false;
  }
  getBeyannameKopyalama(islemInternalNo) {
    if (confirm("Beyannameyi Kopyalamakta Eminmisiniz?")) {
      let yeniislemInternalNo: string;
      const promise = this.beyanServis
        .getBeyannameKopyalama(islemInternalNo.value)
        .toPromise();
      promise.then(
        result => {         
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);    
         
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;
         
          if (yeniislemInternalNo != "") {
            islemInternalNo.value = yeniislemInternalNo;
            this.islemInput.nativeElement.value=islemInternalNo.value;
            this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
            this.getBeyannameFromIslem(yeniislemInternalNo);
          }
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    }
  }
  yeniBeyanname() {
    
    this.beyannameForm.reset();
    this.beyannameForm.enable();   
    this.islemInput.nativeElement.value='';
    this.beyanInternalNo='Boş';   
    this.beyannameForm.markAllAsTouched();
    
    this.submitted = false;
    this.ihracatEditable = false;
    this.ithalatEditable = false;
    this.editable=false;

  } 
  duzeltBeyanname() {
   
    this.beyannameForm.enable();
    this.rejimSelect(this.beyannameForm.get("rejim").value);
    this.beyannameForm.markAllAsTouched();
   
    if (this.beyannameForm.invalid) {
      const invalid = [];
      const controls = this.beyannameForm.controls;
      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }
      
      alert(
        "ERROR!! \n\n Aşağıdaki nesnelerin verileri veya formatı yanlış:"  + JSON.stringify(invalid, null, 4)
      );
     
    }
  }
  silBeyanname(islemInternalNo){
    if (
      confirm(islemInternalNo.value+ " Beyannameyi Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeBeyanname(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.beyannameForm.reset();
          this._beyanSession.islemInternalNo="";
          this.islemInput.nativeElement.value="";
          this.beyannameForm.disable();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  onbeyannameFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.beyannameForm.invalid) {
      const invalid = [];
      const controls = this.beyannameForm.controls;
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
    this.beyannameForm.get("beyanInternalNo").setValue(this.beyanInternalNo);
    this.beyannameForm.get("kullanici").setValue(this.girisService.loggedKullanici);

    let konteyner =this.beyannameForm.get("konteyner").value ===true ?"EVET":"HAYIR";
    this.beyannameForm.get("konteyner").setValue(konteyner);
  
  
    let yeniislemInternalNo: string;
    let yeniBeyanname=new BeyannameDto();
    yeniBeyanname.initalBeyan(this.beyannameForm.value);
  
      const promise = this.beyanServis
        .setDbBeyanname(yeniBeyanname)
        .toPromise();
      promise.then(
        result => {
        
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;
        
           
          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this.beyanInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this.beyanInternalNo=yeniislemInternalNo;
            this.getBeyannameFromIslem(yeniislemInternalNo);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");
            this.beyannameForm.disable();
          }
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.beyannameForm.value, null, 4)
    // );

  
  }
  onCancel() {
    this.submitted = false;
    this.beyannameForm.disable();
  }
  
  
}
