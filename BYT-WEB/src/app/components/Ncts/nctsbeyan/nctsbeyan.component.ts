import { Component, OnInit, Inject, Injector,ViewChild,ElementRef,Injectable } from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray
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
  NctsBeyanDto,
  NbBeyanSahibiDto,
  NbTasiyiciFirmaDto,
  NbGondericiFirmaDto,
  NbAliciFirmaDto,
  NbAsilSorumluFirmaDto,
  NbGuvenliGondericiFirmaDto,
  NbGuvenliAliciFirmaDto,
  ServisDto
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";
import {
  nctsrejim,
  nctsBeyanTipi
} from "../../../../shared/helpers/referencesList";

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

@Component({
  selector: 'app-ozetbeyan',
  templateUrl: './nctsbeyan.component.html',
  styleUrls: ['./nctsbeyan.component.css'],
  providers: [
    {provide: DateAdapter, useClass: PickDateAdapter},
    {provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS}
 ]
})
export class NctsBeyanComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  nctsBeyanForm: FormGroup;
  asilSorumluFirmaForm: FormGroup;
  beyanSahibiForm:FormGroup;
  gondericiFirmaForm: FormGroup;
  aliciFirmaForm: FormGroup;  
  tasiyiciFirmaForm: FormGroup;  
  guvenliAliciFirmaForm: FormGroup;  
  guvenliGondericiFirmaForm: FormGroup;  
  submitted: boolean = false;
  nctsBeyanInternalNo:string;
  beyanStatu:string; 
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _nctsBeyan: NctsBeyanDto = new NctsBeyanDto();
  _rejimList = nctsrejim;
  _beyanTipiList = nctsBeyanTipi;
  _gumrukAllList =this.referansService.getNctsGumrukAllJSON();
  _gumrukList =this.referansService.getNctsTrGumrukJSON();
  _sinirgumrukList =this.referansService.getNctsSinirGumrukJSON();
  _ulkeList = this.referansService.getUlkeDilJSON();
  _dilList = this.referansService.getDilJSON();
  _odemeList = this.referansService.getNctsOdemeJSON();
  _tasimaSekliList = this.referansService.getNctsTasimaSekliJSON();
 


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
    this.beyanSahibiForm= this._fb.group({
      adUnvan: new FormControl("", [
         Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
         Validators.maxLength(150),
      ]),
 
      ilIlce: new FormControl("", [
           Validators.maxLength(35),
      ]),
     
      no: new FormControl("", [
            Validators.maxLength(15),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
     
      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    }),
    this.asilSorumluFirmaForm = this._fb.group({
      adUnvan: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
 
      ilIlce: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      dil: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),
      no: new FormControl("", [
        Validators.required,
        Validators.maxLength(15),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),
     
      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    }),
    this.aliciFirmaForm = this._fb.group({
      adUnvan: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
 
      ilIlce: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      dil: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),
      no: new FormControl("", [
        Validators.required,
        Validators.maxLength(15),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),

      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    }),
    this.gondericiFirmaForm = this._fb.group({
      adUnvan: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
 
      ilIlce: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      dil: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),
      no: new FormControl("", [
        Validators.required,
        Validators.maxLength(15),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(4),
      ]),

      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    })
    this.tasiyiciFirmaForm = this._fb.group({
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
      
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
     
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
      
        Validators.maxLength(4),
      ]),

      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    })
    this.guvenliGondericiFirmaForm = this._fb.group({
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
      
     
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      
      postaKodu: new FormControl("", [
      
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
      
        Validators.maxLength(4),
      ]),

      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    })
    this.guvenliAliciFirmaForm = this._fb.group({
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
      
      
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
      
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      ulkeKodu: new FormControl("", [
      
        Validators.maxLength(4),
      ]),

      nctsBeyanInternalNo: new FormControl(
        this.nctsBeyanInternalNo,
        []
      ),
    })
  }
  get focus() {
    return this.nctsBeyanForm.controls;
  }
  buildForm(): void {
    this.nctsBeyanForm = this.formBuilder.group(
      {
        refNo:[],
       	nctsBeyanInternalNo:[],
        beyannameNo:[],
        tescilStatu: [],
        tescilTarihi:[],            
        varisUlke: new FormControl("", [Validators.required, Validators.maxLength(4)]),
        cikisUlke: new FormControl("", [Validators.required, Validators.maxLength(4)]),
        varisGumruk: new FormControl("", [Validators.required, Validators.maxLength(9)]),
        hareketGumruk: new FormControl("", [Validators.required, Validators.maxLength(9)]),
        sinirGumruk: new FormControl("", [Validators.required, Validators.maxLength(9)]),
        yuklemeYeri: new FormControl("", [Validators.maxLength(20)]),
        yukBosYerDil:new FormControl("", [Validators.maxLength(4)]),  
        bosaltmaYer:new FormControl("", [Validators.maxLength(35)]),
        esyaKabulYerKod: new FormControl("", [Validators.maxLength(17)]),
        esyaKabulYerDil: new FormControl("", [Validators.maxLength(4)]),
        esyaKabulYer: new FormControl("", [Validators.maxLength(17)]),
        esyaOnayYer: new FormControl("", [Validators.maxLength(17)]),    
        esyaYer: new FormControl("", [Validators.maxLength(20)]),    
        dahildeTasimaSekli: new FormControl("",[Validators.maxLength(4)]),
        sinirTasimaSekli:new FormControl("", [Validators.maxLength(40)]),
        cikisTasimaSekli:new FormControl("", [Validators.maxLength(4)]),  
        cikisTasitKimligi:new FormControl("", [Validators.maxLength(40)]),  
        cikisTasitKimligiDil:new FormControl("", [Validators.maxLength(4)]),  
        cikisTasitUlke:new FormControl("", [Validators.maxLength(4)]),  
        sinirTasitKimligi:new FormControl("", [Validators.maxLength(40)]),    
        sinirTasitKimligiDil:new FormControl("", [Validators.maxLength(4)]),  
        sinirTasitUlke:new FormControl("", [Validators.maxLength(4)]),  
        konteyner:[false],
      	kalemSayisi: new FormControl(0,[ ValidationService.numberValidator]),
      	toplamKapSayisi: new FormControl(0,[ ValidationService.numberValidator]),
        kalemToplamBrutKG: new FormControl(0,[ ValidationService.decimalValidation]),        
        rejim:new FormControl("", [Validators.required,Validators.maxLength(4)]),  
        beyanTipi:new FormControl("", [Validators.required,Validators.maxLength(1)]),//SCI    
        beyanTipiDil:new FormControl("", [Validators.required,Validators.maxLength(4)]),  
        odemeAraci:new FormControl("", [Validators.maxLength(4)]),
        refaransNo:new FormControl("", [Validators.maxLength(10)]),
        guvenliBeyan: new FormControl("",[ ValidationService.numberValidator]),
        konveyansRefNo:new FormControl("", [Validators.maxLength(35)]), 
        dorse1:new FormControl("", [Validators.maxLength(50)]), 
        dorse2:new FormControl("", [Validators.maxLength(50)]), 
        damgaVergi: new FormControl("",[Validators.maxLength(15)]),       
        musavirKimlikNo: new FormControl("",[ Validators.maxLength(15)]),  
        yer:new FormControl("", [Validators.required,Validators.maxLength(15)]),// asıl sorumlu ??    
        yerTarihDil:new FormControl("", [Validators.required,Validators.maxLength(4)]),// asul sorumlu ??   
        kullanici: [],  
        temsilci:new FormControl("", [Validators.required,Validators.maxLength(50)]),  
        temsilKapasite:new FormControl("", [Validators.required,Validators.maxLength(9)]),  
        temsilKapasiteDil:new FormControl("", [Validators.maxLength(4)]),  
        varisGumrukYetkilisi:new FormControl("", [Validators.maxLength(17)]),  
        kontrolSonuc:new FormControl("", [Validators.maxLength(9)]),  
        sureSinir:new FormControl("", [Validators.maxLength(12),ValidationService.tarihValidation]), 
        tanker:[false],
         
  
      },
    
    )
  }
 
  ngOnInit() {
  
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Beyanname Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.nctsBeyanForm.disable();
    this.asilSorumluFirmaForm.disable();
    this.gondericiFirmaForm.disable();
    this.beyanSahibiForm.disable();
    this.aliciFirmaForm.disable();
    this.tasiyiciFirmaForm.disable();
    this.guvenliAliciFirmaForm.disable();
    this.guvenliGondericiFirmaForm.disable();

   
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
  //   let ticaret= ihracatRejim.findIndex(x=>x.rejim==rejim)>=0?'E':ithalatRejim.findIndex(x=>x.rejim==rejim)>=0?'I':'';
   
  //  if(ticaret=='E')
  //  {
  //   this.nctsBeyanForm.controls['gondericiVergiNo'].setValidators([Validators.required]);
  //   this.nctsBeyanForm.controls['gondericiVergiNo'].updateValueAndValidity();
 
  //   this.ihracatEditable=true;
  //   this.ithalatEditable=false;
  //  }
  //   else{
  //     this.nctsBeyanForm.controls['birlikKayitNumarasi'].clearValidators();
  //     this.nctsBeyanForm.controls['birlikKayitNumarasi'].updateValueAndValidity();
   
  //      this.ihracatEditable=false;
  //      this.ithalatEditable=true;
  //   }

  //   if(ticaret !='')
  //    this.editable=true;
  }
  bsSelect(bs){
      
  //  if(bs=='2')
  //  {
  //   this.nctsBeyanForm.controls['referansTarihi'].setValidators([Validators.required]);
  //   this.nctsBeyanForm.controls['referansTarihi'].updateValueAndValidity();
  //  }
  //  else{
  //   this.nctsBeyanForm.controls['referansTarihi'].clearValidators();
  //   this.nctsBeyanForm.controls['referansTarihi'].updateValueAndValidity();
  //  }
  }

  getBeyannameFromIslem(islemInternalNo:string) {  
  
    this.beyanServis.getNctsBeyan(islemInternalNo).subscribe(
      result => {
        this._nctsBeyan = new NctsBeyanDto();
        this._nctsBeyan.initalBeyan(result);
      
        if (this._nctsBeyan == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.nctsBeyanInternalNo="";
          this.beyanStatu= "" ;
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.nctsBeyanInternalNo= "" ;
          this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
       
          this._beyanSession.islemInternalNo = islemInternalNo;    
          this._beyanSession.nctsBeyanInternalNo= this._nctsBeyan.nctsBeyanInternalNo ;
          this._beyanSession.beyanStatu= this._nctsBeyan.tescilStatu ;
          this.nctsBeyanInternalNo=this._nctsBeyan.nctsBeyanInternalNo;
          this.beyanStatu= this._nctsBeyan.tescilStatu ;
     
          this.loadnctsBeyanForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getBeyanname(islemInternalNo) {  
   
     this.beyanServis.getNctsBeyan(islemInternalNo.value).subscribe(
      result => {       
        this._nctsBeyan = new NctsBeyanDto();
      
        this._nctsBeyan.initalBeyan(result);
    
        if (this._nctsBeyan == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.nctsBeyanInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.nctsBeyanInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;
         
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = islemInternalNo.value;    
          this._beyanSession.nctsBeyanInternalNo= this._nctsBeyan.nctsBeyanInternalNo ;
          this._beyanSession.beyanStatu= this._nctsBeyan.tescilStatu ;
          this.nctsBeyanInternalNo=this._nctsBeyan.nctsBeyanInternalNo;
          this.beyanStatu= this._nctsBeyan.tescilStatu ;
          this.loadnctsBeyanForm();
         
        
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadnctsBeyanForm()
    {
      this.nctsBeyanForm.reset();
     this._beyanSession.nctsBeyanInternalNo= this._nctsBeyan.nctsBeyanInternalNo;
     this.nctsBeyanInternalNo= this._nctsBeyan.nctsBeyanInternalNo;
       this.nctsBeyanForm.setValue({      
        nctsBeyanInternalNo: this._nctsBeyan.nctsBeyanInternalNo,
        beyannameNo:this._nctsBeyan.beyannameNo,
        refNo:this._nctsBeyan.refNo,
        hareketGumruk:this._nctsBeyan.hareketGumruk,
        varisGumruk:this._nctsBeyan.varisGumruk,
        tescilStatu:this._nctsBeyan.tescilStatu,
        tescilTarihi:this._nctsBeyan.tescilTarihi,
        varisUlke:this._nctsBeyan.varisUlke,  
        cikisUlke:this._nctsBeyan.cikisUlke,     
        esyaKabulYerKod:this._nctsBeyan.esyaKabulYerKod,
        esyaKabulYerDil:this._nctsBeyan.esyaKabulYerDil,
        esyaKabulYer:this._nctsBeyan.esyaKabulYer,
        esyaOnayYer:this._nctsBeyan.esyaOnayYer,
        yuklemeYeri:this._nctsBeyan.yuklemeYeri,
        esyaYer:this._nctsBeyan.esyaYer,
        yer:this._nctsBeyan.yer,
        yerTarihDil:this._nctsBeyan.yerTarihDil,
        bosaltmaYer:this._nctsBeyan.bosaltmaYer,
        yukBosYerDil:this._nctsBeyan.yukBosYerDil,
        dahildeTasimaSekli:this._nctsBeyan.dahildeTasimaSekli,
        sinirTasimaSekli:this._nctsBeyan.sinirTasimaSekli,
        cikisTasimaSekli:this._nctsBeyan.cikisTasimaSekli,
        cikisTasitKimligi:this._nctsBeyan.cikisTasitKimligi,
        cikisTasitKimligiDil:this._nctsBeyan.cikisTasitKimligiDil,
        cikisTasitUlke:this._nctsBeyan.cikisTasitUlke,
        sinirTasitKimligi:this._nctsBeyan.sinirTasitKimligi,  
        sinirTasitKimligiDil:this._nctsBeyan.sinirTasitKimligiDil,
        sinirTasitUlke:this._nctsBeyan.sinirTasitUlke,
        konteyner:this._nctsBeyan.konteyner,
        kalemSayisi:this._nctsBeyan.kalemSayisi,
        toplamKapSayisi:this._nctsBeyan.toplamKapSayisi,
        kalemToplamBrutKG:this._nctsBeyan.kalemToplamBrutKG,
        rejim:this._nctsBeyan.rejim,
        beyanTipi:this._nctsBeyan.beyanTipi,
        beyanTipiDil:this._nctsBeyan.beyanTipiDil,
        odemeAraci:this._nctsBeyan.odemeAraci,
        refaransNo:this._nctsBeyan.refaransNo,
        guvenliBeyan:this._nctsBeyan.guvenliBeyan, 
        konveyansRefNo:this._nctsBeyan.konveyansRefNo,
        dorse1:this._nctsBeyan.dorse1,
        dorse2:this._nctsBeyan.dorse2,
        damgaVergi:this._nctsBeyan.damgaVergi,
        musavirKimlikNo:this._nctsBeyan.musavirKimlikNo,
        kullanici:this._nctsBeyan.kullanici,
        temsilci:this._nctsBeyan.temsilci,
        temsilKapasite:this._nctsBeyan.temsilKapasite,
        temsilKapasiteDil:this._nctsBeyan.temsilKapasiteDil,
        varisGumrukYetkilisi:this._nctsBeyan.varisGumrukYetkilisi,
        kontrolSonuc:this._nctsBeyan.kontrolSonuc,
        sureSinir:this._nctsBeyan.sureSinir,
        tanker:this._nctsBeyan.tanker,
        sinirGumruk:this._nctsBeyan.sinirGumruk
      
     
       });
      
       this.beyanServis.getNbBeyanSahibi(this._beyanSession.islemInternalNo).subscribe(
        (result: NbBeyanSahibiDto) => {
          this.LoadBeyanSahibi(result);
          this.beyanSahibiForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
   
      
       this.beyanServis.getNbTasiyiciFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbTasiyiciFirmaDto) => {
          this.LoadTasiyiciFirma(result);
          this.tasiyiciFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.beyanServis.getNbAsilSorumluFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbAsilSorumluFirmaDto) => {
          this.LoadAsilSorumluFirma(result);
          this.asilSorumluFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.beyanServis.getNbAliciFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbAliciFirmaDto) => {
          this.LoadAliciFirma(result);
          this.aliciFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.beyanServis.getNbGondericiFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbGondericiFirmaDto) => {
          this.LoadGondericiFirma(result);
          this.gondericiFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.beyanServis.getNbGuvenliAliciFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbGuvenliAliciFirmaDto) => {
          this.LoadGuvenliAliciFirma(result);
          this.guvenliAliciFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.beyanServis.getNbGuvenliGondericiFirma(this._beyanSession.islemInternalNo).subscribe(
        (result: NbGuvenliGondericiFirmaDto) => {
          this.LoadGuvenliGondericiFirma(result);
          this.guvenliGondericiFirmaForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );

      this.nctsBeyanForm.disable();
   

  }
  
  LoadBeyanSahibi(firma:NbBeyanSahibiDto)
  { this.beyanSahibiForm.reset();
    if(firma!=null)
     this.beyanSahibiForm.setValue({  
   
      nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
      adUnvan: firma.adUnvan,
      caddeSokakNo:firma.caddeSokakNo,
      ilIlce:firma.ilIlce,     
      no:firma.no,
    
    });
  }
  LoadTasiyiciFirma(firma:NbTasiyiciFirmaDto)
    {
      this.tasiyiciFirmaForm.reset();
    if(firma!=null)
       this.tasiyiciFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
  LoadAsilSorumluFirma(firma:NbAsilSorumluFirmaDto)
    {
      this.asilSorumluFirmaForm.reset();
      if(firma!=null)
       this.asilSorumluFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
  LoadAliciFirma(firma:NbAliciFirmaDto)
    {
      this.aliciFirmaForm.reset();
      if(firma!=null)
       this.aliciFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
  LoadGondericiFirma(firma:NbGondericiFirmaDto)
    {
      this.gondericiFirmaForm.reset();
      if(firma!=null)
       this.gondericiFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
  LoadGuvenliAliciFirma(firma:NbGuvenliAliciFirmaDto)
    {
      this.guvenliAliciFirmaForm.reset();
      if(firma!=null)
       this.guvenliAliciFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
    }
  LoadGuvenliGondericiFirma(firma:NbGuvenliGondericiFirmaDto)
    { this.guvenliGondericiFirmaForm.reset();
      if(firma!=null)
       this.guvenliGondericiFirmaForm.setValue({  
        nctsBeyanInternalNo: firma.nctsBeyanInternalNo,
        adUnvan: firma.adUnvan,
        caddeSokakNo:firma.caddeSokakNo,
        ilIlce:firma.ilIlce,
        dil:firma.dil,
        no:firma.no,
        postaKodu:firma.postaKodu,
        ulkeKodu:firma.ulkeKodu
      });
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
        .getNctsKopyalama(islemInternalNo.value)
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
    this.nctsBeyanInternalNo='Boş';
    this.beyanStatu='';
    this.nctsBeyanForm.reset();
    this.nctsBeyanForm.enable();  
    this.nctsBeyanForm.markAllAsTouched();
    this.islemInput.nativeElement.value="";
    this.submitted = false;
   
    this.asilSorumluFirmaForm.reset();
    this.asilSorumluFirmaForm.enable();
    this.asilSorumluFirmaForm.markAllAsTouched();
    this.gondericiFirmaForm.reset();
    this.gondericiFirmaForm.enable();
    this.gondericiFirmaForm.markAllAsTouched();
    this.aliciFirmaForm.reset();
    this.aliciFirmaForm.enable();
    this.aliciFirmaForm.markAllAsTouched();
    this.tasiyiciFirmaForm.reset();
    this.tasiyiciFirmaForm.enable();
    this.tasiyiciFirmaForm.markAllAsTouched();
    this.guvenliAliciFirmaForm.reset();
    this.guvenliAliciFirmaForm.enable();
    this.guvenliAliciFirmaForm.markAllAsTouched();
    this.guvenliGondericiFirmaForm.reset();
    this.guvenliGondericiFirmaForm.enable();
    this.guvenliGondericiFirmaForm.markAllAsTouched();
    this.beyanSahibiForm.reset();
    this.beyanSahibiForm.enable();
    this.beyanSahibiForm.markAllAsTouched();
   

  } 
  duzeltBeyanname() {
   
    this.nctsBeyanForm.enable();
    this.asilSorumluFirmaForm.enable();
    this.gondericiFirmaForm.enable();
    this.aliciFirmaForm.enable();
    this.tasiyiciFirmaForm.enable();
    this.guvenliAliciFirmaForm.enable();
    this.guvenliGondericiFirmaForm.enable();
    this.beyanSahibiForm.enable();
   // this.rejimSelect(this.nctsBeyanForm.get("rejim").value);
    this.nctsBeyanForm.markAllAsTouched();
    this.asilSorumluFirmaForm.markAllAsTouched();
    this.gondericiFirmaForm.markAllAsTouched();
    this.aliciFirmaForm.markAllAsTouched();
    this.tasiyiciFirmaForm.markAllAsTouched();
    this.guvenliAliciFirmaForm.markAllAsTouched();
    this.guvenliGondericiFirmaForm.markAllAsTouched();
    this.beyanSahibiForm.markAllAsTouched();
   
    if (this.nctsBeyanForm.invalid) {
      const invalid = [];
      const controls = this.nctsBeyanForm.controls;
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
        .removeNctsBeyan(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.nctsBeyanForm.reset();
          this._beyanSession.islemInternalNo="";
          this._beyanSession.nctsBeyanInternalNo="";
          this.nctsBeyanInternalNo="";
          this._beyanSession.beyanStatu="";
          this.beyanStatu="";
          this.islemInput.nativeElement.value="";
          islemInternalNo.value="";
          this.nctsBeyanForm.disable();
          this.asilSorumluFirmaForm.reset();
          this.asilSorumluFirmaForm.disable();
          this.gondericiFirmaForm.reset();
          this.gondericiFirmaForm.disable();
          this.aliciFirmaForm.reset();
          this.aliciFirmaForm.disable();
          this.tasiyiciFirmaForm.reset();
          this.tasiyiciFirmaForm.disable();
          this.guvenliAliciFirmaForm.reset();
          this.guvenliAliciFirmaForm.disable();
          this.guvenliGondericiFirmaForm.reset();
          this.guvenliGondericiFirmaForm.disable();
          this.beyanSahibiForm.reset();
          this.beyanSahibiForm.disable();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  onnctsBeyanFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.nctsBeyanForm.invalid) {
      const invalid = [];
      const controls = this.nctsBeyanForm.controls;
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
   
    this.nctsBeyanForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
    this.nctsBeyanForm.get("kullanici").setValue(this.girisService.loggedKullanici);
 
    let yeniislemInternalNo: string;
    let yeniBeyanname=new NctsBeyanDto();
    yeniBeyanname.initalBeyan(this.nctsBeyanForm.value);
  
      const promise = this.beyanServis
        .setNctsBeyan(yeniBeyanname)
        .toPromise();
      promise.then(
        result => {
        
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
         
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;          
        
          if (yeniislemInternalNo != null) {
          
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this._nctsBeyan.nctsBeyanInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this._beyanSession.nctsBeyanInternalNo=yeniislemInternalNo;
            this.nctsBeyanInternalNo=yeniislemInternalNo;
            this.setBeyanSahibi();          
            this.setTasiyiciFirma();   
            this.setAsilSorumluFirma();   
            this.setGondericiFirma();             
            this.setAliciFirma();   
            this.setGuvenliAlici();   
            this.setGuvenliGonderici(); 
          
            this.getBeyannameFromIslem(yeniislemInternalNo);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");         
          
          }
         
            this.nctsBeyanForm.disable();
            this.asilSorumluFirmaForm.disable();
            this.gondericiFirmaForm.disable();
            this.aliciFirmaForm.disable();
            this.tasiyiciFirmaForm.disable();
            this.guvenliAliciFirmaForm.disable();
            this.guvenliGondericiFirmaForm.disable();
            this.beyanSahibiForm.disable();
          
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.nctsBeyanForm.value, null, 4)
    // );

  
  }
  setBeyanSahibi()
  {
    if (this.beyanSahibiForm.invalid) {
      const invalid = [];
      const controls = this.beyanSahibiForm.controls;
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

    this.beyanSahibiForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
   
    const promise = this.beyanServis
    .restoreNbBeyanSahibi(
      this.beyanSahibiForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.beyanSahibiForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
  }
  setTasiyiciFirma()
  {
    if (this.tasiyiciFirmaForm.invalid) {
      const invalid = [];
      const controls = this.tasiyiciFirmaForm.controls;
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

    this.tasiyiciFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
   
    const promise = this.beyanServis
    .restoreNbTasiyiciFirma(
      this.tasiyiciFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.tasiyiciFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
  }
  setAsilSorumluFirma()
  {
    if (this.asilSorumluFirmaForm.invalid) {
      const invalid = [];
      const controls = this.asilSorumluFirmaForm.controls;
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

    this.asilSorumluFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
   
    const promise = this.beyanServis
    .restoreNbAsilSorumluFirma(
      this.asilSorumluFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.asilSorumluFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
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
   
    const promise = this.beyanServis
    .restoreNbGondericiFirma(
      this.gondericiFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.gondericiFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
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
   
    const promise = this.beyanServis
    .restoreNbAliciFirma(
      this.aliciFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.aliciFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
  }
  setGuvenliGonderici()
  {
    if (this.guvenliGondericiFirmaForm.invalid) {
      const invalid = [];
      const controls = this.guvenliGondericiFirmaForm.controls;
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

    this.guvenliGondericiFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
   
    const promise = this.beyanServis
    .restoreNbGuvenliGondericiFirma(
      this.guvenliGondericiFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.guvenliGondericiFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
  }
  setGuvenliAlici()
  {
    if (this.guvenliAliciFirmaForm.invalid) {
      const invalid = [];
      const controls = this.guvenliAliciFirmaForm.controls;
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

    this.guvenliAliciFirmaForm.get("nctsBeyanInternalNo").setValue(this.nctsBeyanInternalNo);    
   
    const promise = this.beyanServis
    .restoreNbGuvenliAliciFirma(
      this.guvenliAliciFirmaForm.value,
      this._beyanSession.nctsBeyanInternalNo
    )
    .toPromise();
    promise.then(
    (result) => {

      this.guvenliAliciFirmaForm.reset();
     
    },
    (err) => {
      this.openSnackBar(err, "Tamam");
    }
  );
  }
  onCancel() {
    this.submitted = false;
    this.nctsBeyanForm.disable();
  }
  
  
}
