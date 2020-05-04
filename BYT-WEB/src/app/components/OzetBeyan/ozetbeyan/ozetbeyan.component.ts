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
  OzetBeyanDto,
  TasitUgrakUlkeDto,
  ServisDto
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";
import {
 rejim
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
  templateUrl: './ozetbeyan.component.html',
  styleUrls: ['./ozetbeyan.component.css'],
  providers: [
    {provide: DateAdapter, useClass: PickDateAdapter},
    {provide: MAT_DATE_FORMATS, useValue: PICK_FORMATS}
 ]
})
export class OzetbeyanComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  ozetBeyanForm: FormGroup;
  tasitForm: FormGroup;
  submitted: boolean = false;
  ihracatEditable: boolean = false;
  ithalatEditable: boolean = false;
  ozetBeyanInternalNo:string;
  beyanStatu:string;
  editable: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _ozetBeyan: OzetBeyanDto = new OzetBeyanDto();
  _rejimList = rejim;
  _beyanTuruList = this.referansService.beyanTuruJSON();
  _gumrukList =this.referansService.getGumrukJSON();
  _ulkeList = this.referansService.getUlkeJSON();
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
    return this.ozetBeyanForm.controls;
  }
  buildForm(): void {
    this.ozetBeyanForm = this.formBuilder.group(
      {
       
      	ozetBeyanInternalNo:[],
	      ozetBeyanNo:[],
        beyanSahibiVergiNo: new FormControl("", [Validators.maxLength(20)]),
	      beyanTuru: new FormControl("", [Validators.required,Validators.maxLength(9)]),
      	diger:new FormControl("", [Validators.maxLength(500)]),
        dorseNo1:new FormControl("", [Validators.maxLength(25)]),
        dorseNo1Uyrugu:new FormControl("", [Validators.maxLength(9)]),
        dorseNo2:new FormControl("", [Validators.maxLength(25)]),
        dorseNo2Uyrugu:new FormControl("", [Validators.maxLength(9)]),
        ekBelgeSayisi : new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
      	emniyetGuvenlik: [false],
      	grupTasimaSenediNo:new FormControl("", [Validators.maxLength(20)]),
	      gumrukIdaresi:new FormControl("", [Validators.required,Validators.maxLength(9)]),
      	kullaniciKodu: new FormControl(""),
	      kurye:[false],
        limanYerAdiBos:new FormControl("", [Validators.maxLength(20)]),
        limanYerAdiYuk:new FormControl("", [Validators.maxLength(20)]),
        oncekiBeyanNo:new FormControl("", [Validators.maxLength(20)]),
        plakaSeferNo:new FormControl("", [Validators.maxLength(25)]),
        referansNumarasi:new FormControl("", [Validators.maxLength(25)]),
        rejim:new FormControl("", [Validators.required,Validators.maxLength(9)]),
        tasimaSekli:new FormControl("", [Validators.maxLength(9)]),
        tasitinAdi:new FormControl("", [Validators.maxLength(50)]),
        tasiyiciVergiNo:new FormControl("", [Validators.maxLength(20)]),
        tirAtaKarneNo:new FormControl("", [Validators.maxLength(20)]),
        ulkeKodu:new FormControl("", [Validators.maxLength(9)]),
        ulkeKoduYuk:new FormControl("", [Validators.maxLength(9)]),
        ulkeKoduBos:new FormControl("", [Validators.maxLength(9)]),
        yuklemeBosaltmaYeri:new FormControl("", [Validators.maxLength(20)]),
        varisCikisGumrukIdaresi:new FormControl("", [Validators.maxLength(9)]),
        varisTarihSaati: new FormControl("",[ ValidationService.tarihValidation]),
        xmlRefId:new FormControl("", [Validators.maxLength(35)]),
        tescilStatu: [],
        tescilTarihi:[],     
      },
      (this.tasitForm = this._fb.group({
        tasitArry: this._fb.array([this.getTasit()]),
      })),
    )
  }
 
  ngOnInit() {
  
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Beyanname Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.ozetBeyanForm.disable();

   
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
  //   this.ozetBeyanForm.controls['gondericiVergiNo'].setValidators([Validators.required]);
  //   this.ozetBeyanForm.controls['gondericiVergiNo'].updateValueAndValidity();
 
  //   this.ihracatEditable=true;
  //   this.ithalatEditable=false;
  //  }
  //   else{
  //     this.ozetBeyanForm.controls['birlikKayitNumarasi'].clearValidators();
  //     this.ozetBeyanForm.controls['birlikKayitNumarasi'].updateValueAndValidity();
   
  //      this.ihracatEditable=false;
  //      this.ithalatEditable=true;
  //   }

  //   if(ticaret !='')
  //    this.editable=true;
  }
  bsSelect(bs){
      
  //  if(bs=='2')
  //  {
  //   this.ozetBeyanForm.controls['referansTarihi'].setValidators([Validators.required]);
  //   this.ozetBeyanForm.controls['referansTarihi'].updateValueAndValidity();
  //  }
  //  else{
  //   this.ozetBeyanForm.controls['referansTarihi'].clearValidators();
  //   this.ozetBeyanForm.controls['referansTarihi'].updateValueAndValidity();
  //  }
  }

  getBeyannameFromIslem(islemInternalNo:string) {  
  
    this.beyanServis.getOzetBeyan(islemInternalNo).subscribe(
      result => {
        this._ozetBeyan = new OzetBeyanDto();
        this._ozetBeyan.initalBeyan(result);
        if (this._ozetBeyan == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.ozetBeyanInternalNo="";
          this.beyanStatu= "" ;        
      
          return;
        }
        else{
         
          this._beyanSession.islemInternalNo = islemInternalNo;
          this._beyanSession.ozetBeyanInternalNo= this._ozetBeyan.ozetBeyanInternalNo ;
          this._beyanSession.beyanStatu= this._ozetBeyan.tescilStatu ; 
          this.ozetBeyanInternalNo=this._ozetBeyan.ozetBeyanInternalNo;
          this.beyanStatu= this._ozetBeyan.tescilStatu ;
          this.loadozetBeyanForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getBeyanname(islemInternalNo) {  
   
      var result= this.beyanServis.getOzetBeyan(islemInternalNo.value).subscribe(
      result => {
       
        this._ozetBeyan = new OzetBeyanDto();
        this._ozetBeyan.initalBeyan(result);
       
        if (this._ozetBeyan == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.ozetBeyanInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.ozetBeyanInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = islemInternalNo.value;    
          this._beyanSession.ozetBeyanInternalNo= this._ozetBeyan.ozetBeyanInternalNo ;
          this._beyanSession.beyanStatu= this._ozetBeyan.tescilStatu ;
          this.ozetBeyanInternalNo=this._ozetBeyan.ozetBeyanInternalNo;
          this.beyanStatu= this._ozetBeyan.tescilStatu ;
          this.loadozetBeyanForm();
         
          // islemInternalNo.value ="";
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadozetBeyanForm()
    {

       this.ozetBeyanForm.setValue({
      
        ozetBeyanInternalNo: this._ozetBeyan.ozetBeyanInternalNo,
	      ozetBeyanNo:this._ozetBeyan.ozetBeyanNo,
        beyanSahibiVergiNo:this._ozetBeyan.beyanSahibiVergiNo,
	      beyanTuru:this._ozetBeyan.beyanTuru,
      	diger:this._ozetBeyan.diger,
        dorseNo1:this._ozetBeyan.dorseNo1,
        dorseNo1Uyrugu:this._ozetBeyan.dorseNo1Uyrugu,
        dorseNo2:this._ozetBeyan.dorseNo2,
        dorseNo2Uyrugu:this._ozetBeyan.dorseNo2Uyrugu,
        ekBelgeSayisi : this._ozetBeyan.ekBelgeSayisi,
      	emniyetGuvenlik: this._ozetBeyan.emniyetGuvenlik==="EVET"?true:false,
      	grupTasimaSenediNo:this._ozetBeyan.grupTasimaSenediNo,
	      gumrukIdaresi:this._ozetBeyan.gumrukIdaresi,
        kullaniciKodu:this._ozetBeyan.kullaniciKodu,
	      kurye:this._ozetBeyan.kurye==="EVET"?true:false,
        limanYerAdiBos:this._ozetBeyan.limanYerAdiBos,
        limanYerAdiYuk:this._ozetBeyan.limanYerAdiYuk,
        oncekiBeyanNo:this._ozetBeyan.oncekiBeyanNo,
        plakaSeferNo:this._ozetBeyan.plakaSeferNo,
        referansNumarasi:this._ozetBeyan.referansNumarasi,
        rejim:this._ozetBeyan.rejim,
        tasimaSekli:this._ozetBeyan.tasimaSekli,
        tasitinAdi:this._ozetBeyan.tasitinAdi,
        tasiyiciVergiNo:this._ozetBeyan.tasiyiciVergiNo,
        tirAtaKarneNo:this._ozetBeyan.tirAtaKarneNo,
        ulkeKodu:this._ozetBeyan.ulkeKodu,
        ulkeKoduYuk:this._ozetBeyan.ulkeKoduYuk,
        ulkeKoduBos:this._ozetBeyan.ulkeKoduBos,
        yuklemeBosaltmaYeri:this._ozetBeyan.yuklemeBosaltmaYeri,
        varisCikisGumrukIdaresi:this._ozetBeyan.varisCikisGumrukIdaresi,
        varisTarihSaati:this._ozetBeyan.varisTarihSaati,
        xmlRefId:this._ozetBeyan.xmlRefId,
        tescilStatu: this._ozetBeyan.tescilStatu,
        tescilTarihi:this._ozetBeyan.tescilTarihi,    
     
       });
  
       this.beyanServis.getObTasitUgrakUlke(this._beyanSession.islemInternalNo).subscribe(
        (result: TasitUgrakUlkeDto[]) => {
          this.initTasitFormArray(result);
          this.tasitForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.ozetBeyanForm.disable();

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
        .getOzetBeyanKopyalama(islemInternalNo.value)
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
    this.ozetBeyanInternalNo='Boş';
    this.beyanStatu='';
    this.ozetBeyanForm.reset();
    this.ozetBeyanForm.enable();
    this.islemInput.nativeElement.value="";
    this.ozetBeyanForm.markAllAsTouched();
    this.submitted = false;
    this.ihracatEditable = false;
    this.ithalatEditable = false;
    this.editable=false;
   
    this.tasitForm.reset();
    this.tasitForm.enable();

    const formTasitArray = this.tasitForm.get("tasitArry") as FormArray;
    formTasitArray.clear();
    this.tasitForm.setControl("tasitArry", formTasitArray);

  } 
  duzeltBeyanname() {
   
    this.ozetBeyanForm.enable();
    this.tasitForm.enable();
    this.rejimSelect(this.ozetBeyanForm.get("rejim").value);
    this.ozetBeyanForm.markAllAsTouched();
    this.tasitForm.markAllAsTouched();
   
    if (this.ozetBeyanForm.invalid) {
      const invalid = [];
      const controls = this.ozetBeyanForm.controls;
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
  onozetBeyanFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.ozetBeyanForm.invalid) {
      const invalid = [];
      const controls = this.ozetBeyanForm.controls;
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
   
    this.ozetBeyanForm.get("ozetBeyanInternalNo").setValue(this.ozetBeyanInternalNo);    
    this.ozetBeyanForm.get("kullaniciKodu").setValue(this.girisService.loggedKullanici);
    
    let kurye = this.ozetBeyanForm.get("kurye").value ===true ?"EVET":"HAYIR";
    this.ozetBeyanForm.get("kurye").setValue(kurye);
       
    let emniyet =this.ozetBeyanForm.get("emniyetGuvenlik").value ===true ?"EVET":"HAYIR";
    this.ozetBeyanForm.get("emniyetGuvenlik").setValue(emniyet);
  
    let yeniislemInternalNo: string;
    let yeniBeyanname=new OzetBeyanDto();
    yeniBeyanname.initalBeyan(this.ozetBeyanForm.value);
    console.log(yeniBeyanname);
      const promise = this.beyanServis
        .setOzetBeyan(yeniBeyanname)
        .toPromise();
      promise.then(
        result => {
        
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;
          this.setTasit();
           
          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this._ozetBeyan.ozetBeyanInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");            
          }
           this.ozetBeyanForm.disable();
            this.tasitForm.disable();
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.ozetBeyanForm.value, null, 4)
    // );

  
  }

   //#region Taşıtın Uğradığı Ülke

   initTasitFormArray(tasit: TasitUgrakUlkeDto[]) {
    const formArray = this.tasitForm.get("tasitArry") as FormArray;
    formArray.clear();
    for (let klm of tasit) {
       let formGroup: FormGroup = new FormGroup({
        ulkeKodu: new FormControl(klm.ulkeKodu, [
          Validators.required,Validators.maxLength(9)
        ]),
        limanYerAdi: new FormControl(klm.limanYerAdi, [
          Validators.required,
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]*$"),
        ]),
        hareketTarihSaati: new FormControl(klm.hareketTarihSaati, [
          Validators.required,
         ValidationService.tarihValidation,
        ]),
        ozetBeyanInternalNo: new FormControl(klm.ozetBeyanInternalNo),
      
      });

      formArray.push(formGroup);
    }
    this.tasitForm.setControl("tasitArry", formArray);
  }

  getTasit() {
    return this._fb.group({
      ulkeKodu: new FormControl("", [Validators.required,Validators.maxLength(9),]),
      limanYerAdi: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      hareketTarihSaati: new FormControl("", [
        Validators.required,
        ValidationService.tarihValidation,
      ]),
      ozetBeyanInternalNo: new FormControl(this._beyanSession.ozetBeyanInternalNo, [
        Validators.required,
      ]),
    
    });
  }

  get tasitBilgileri() {
    return this.tasitForm.get("tasitArry") as FormArray;
  }

  addTasitField() {
    this.tasitBilgileri.push(this.getTasit());
  }

  deleteTasitField(index: number) {
    this.tasitBilgileri.removeAt(index);
  }

  setTasit() {
    if (this.tasitBilgileri.length > 0) {
      for (let klm of this.tasitBilgileri.value) {
        klm.ozetBeyanInternalNo = this.ozetBeyanInternalNo;
       
      }

      this.initTasitFormArray(this.tasitBilgileri.value);

      if (this.tasitBilgileri.invalid) {
        const invalid = [];
        const controls = this.tasitBilgileri.controls;

        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }

        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Taşıt Uğrak Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );

          return;
        }
      }
    }

    if (this.tasitBilgileri.length >= 0) {
      const promiseOdeme = this.beyanServis
        .restoreObTasitUgrakUlke(
          this.tasitBilgileri.value,
          this._beyanSession.ozetBeyanInternalNo
        )
        .toPromise();
      promiseOdeme.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
          // this.tasitForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }
  //#endregion Taşıtın Uğradığı Ülke
  onCancel() {
    this.submitted = false;
    this.ozetBeyanForm.disable();
  }
  
  
}
