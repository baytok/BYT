import { Component, OnInit, Inject, Injector,ViewChild,ElementRef } from "@angular/core";
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
  rejim,
  bs,
  gumruk,
  ulke,
  aliciSatici,
  tasimaSekli,
  isleminNiteligi,
  aracTipi,
  teslimSekli,
  dovizCinsi
} from "../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../shared/service-proxies/service-proxies";
import { MatDialog } from "@angular/material/dialog";
import { MatInput } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemlerDto,
  ServisDto
} from "../../../shared/service-proxies/service-proxies";

@Component({
  selector: "app-beyanname",
  templateUrl: "./beyanname.component.html",
  styleUrls: ["./beyanname.component.scss"]
})
export class BeyannameComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  beyannameForm: FormGroup;
  submitted: boolean = false;
  guidOf = this.session.guidOf;
  islemInternalNo = this.session.islemInternalNo;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _beyanname: BeyannameDto = new BeyannameDto();
  _kalemler: KalemlerDto[];
  _rejimList = rejim;
  _bsList = bs;
  _gumrukList = gumruk;
  _ulkeList = ulke;
  _aliciSaticiList = aliciSatici;
  _tasimaSekliList = tasimaSekli;
  _isleminNiteligiList = isleminNiteligi;
  _aracTipiList = aracTipi;
  _teslimList = teslimSekli;
  _dovizList=dovizCinsi;


  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _fb: FormBuilder
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
        aliciSaticiIliskisi: [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ],
        aliciVergiNo: new FormControl("", [Validators.maxLength(20)]),
        gondericiVergiNo: new FormControl("", [Validators.maxLength(20)]),
        musavirVergiNo:  new FormControl("", [Validators.maxLength(20)]),
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
          Validators.required,
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
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
          Validators.pattern("^[a-zA-Z0-9]*$")
        ]),       
        birlikKayitNumarasi: new FormControl("", [Validators.maxLength(30)]),
        birlikKriptoNumarasi: new FormControl("", [Validators.maxLength(30)]),
        isleminNiteligi:new FormControl("", [Validators.required,Validators.maxLength(9)]),      
        kapAdedi: new FormControl("", [Validators.pattern("^[0-9]*$")]),
        musavirReferansNo: new FormControl("", [Validators.required,Validators.maxLength(12)]),     
        referansTarihi: [],
        tescilStatu: [],
        tescilTarihi:[],
        refNo: new FormControl("", [Validators.maxLength(30)]),
        //Finansal Bilgiler
        bankaKodu: new FormControl("", [
          Validators.maxLength(16),
          Validators.pattern("^[0-9]*$")
        ]),        
        teslimSekli: new FormControl("", [Validators.required,Validators.maxLength(9)]), 
        teslimSekliYeri:  new FormControl("", [Validators.maxLength(40)]),         
        telafiEdiciVergi:new FormControl("", [Validators.pattern("^[0-9]*$")]),
        toplamFatura: new FormControl("", [Validators.required,Validators.pattern("^[0-9]*$")]),
        toplamFaturaDovizi:new FormControl("", [Validators.required]), 
        toplamNavlun: new FormControl("", [Validators.pattern("^[0-9]*$")]),
        toplamNavlunDovizi: [],
        toplamSigorta: new FormControl("", [Validators.pattern("^[0-9]*$")]),
        toplamSigortaDovizi: [],
        toplamYurtDisiHarcamalar:new FormControl("", [Validators.pattern("^[0-9]*$")]),
        toplamYurtDisiHarcamalarDovizi: [],
        toplamYurtIciHarcamalar:new FormControl("", [Validators.pattern("^[0-9]*$")]),
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
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        sinirdakiAracinKimligi: new FormControl("", [
          Validators.required,
          Validators.maxLength(35)
        ]),
        sinirdakiAracinTipi: new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        sinirdakiTasimaSekli: new FormControl("", [
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        konteyner: [false],
        girisGumrukIdaresi:new FormControl("",[
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        varisGumrukIdaresi:new FormControl("",[
          Validators.maxLength(9),
          Validators.pattern("^[0-9]*$")
        ]),
        tasarlananGuzergah: new FormControl("", [Validators.maxLength(250)]),       
        yukBelgeleriSayisi: new FormControl("", [Validators.pattern("^[0-9]*$")]),    
        yuklemeBosaltmaYeri: new FormControl("", [Validators.maxLength(40)]),    
        esyaninBulunduguYer: new FormControl("", [Validators.maxLength(40)]),    
        mobil1: new FormControl("", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(12),
          Validators.pattern("^[0-9]*$")
        ]),
       
        mail1: new FormControl("", [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(80),
          Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")
        ])
      }
      
    )
  }
  ngOnInit() {
    if (this.session.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value=this.session.islemInternalNo;
      this.getBeyannameFromIslem(this.session.islemInternalNo);
     
    }
    this.buildForm();
    this.beyannameForm.disable();
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getBeyannameFromIslem(islemInternalNo:string) {  

    this.beyanServis.getBeyanname(islemInternalNo).subscribe(
      result => {
       
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);
        
        this._beyanname = this._beyannameBilgileri.Beyanname;       
        if (this._beyanname == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          return;
        }
        else{
          this._kalemler=this._beyannameBilgileri.Kalemler;
          this.session.islemInternalNo = islemInternalNo;
          this.session.Kalemler= this._kalemler;
          this.session.beyanInternalNo= this._beyanname.beyanInternalNo ;
          this.session.beyanStatu= this._beyanname.tescilStatu ;
        }
        this.loadBeyannameForm();
      
      },
      err => {
        console.log(err);
      }
    );
  }
  getBeyanname(islemInternalNo) {  

    this.beyanServis.getBeyanname(islemInternalNo.value).subscribe(
      result => {
       
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);
        
        this._beyanname = this._beyannameBilgileri.Beyanname;       
        if (this._beyanname == null) {
          this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
          return;
        }
        else{
          this._kalemler=this._beyannameBilgileri.Kalemler;
          this.session.islemInternalNo = islemInternalNo.value;
          this.session.Kalemler= this._kalemler;
          this.session.beyanInternalNo= this._beyanname.beyanInternalNo ;
          this.session.beyanStatu= this._beyanname.tescilStatu ;
          this.loadBeyannameForm();
          // islemInternalNo.value ="";
        }
    
      },
      err => {
        console.log(err);
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
        konteyner: this._beyanname.konteyner,
        
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
  getBeyannameKopyalama(islemInternalNo) {
    if (confirm("Beyannameyi Kopyalamakta Eminmisiniz?")) {
      let yeniislemInternalNo: string;
      const promise = this.beyanServis
        .getBeyannameKopyalama(islemInternalNo.value)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          yeniislemInternalNo = servisSonuc.Bilgiler[0].referansNo;
          console.log(yeniislemInternalNo);

          if (this._beyanname == null) {
            islemInternalNo.value = yeniislemInternalNo;
            this.openSnackBar(yeniislemInternalNo, "Tamam");
            this.getBeyanname(islemInternalNo);
          }
        },
        err => {
          console.log(err);
        }
      );
    }
  }
  yeniBeyanname() {
    this._beyanname.beyanInternalNo='';
    this._beyanname.tescilStatu='';
    this.beyannameForm.reset();
    this.beyannameForm.enable();
    this.submitted = false;
    this.islemInput.nativeElement.value="";
  } 
  duzeltBeyanname() {
   
    this.beyannameForm.enable();
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

    let yeniislemInternalNo: string;
    let yeniBeyanname=new BeyannameDto();
    yeniBeyanname.init(this.beyannameForm.value);
    console.log(yeniBeyanname);
      const promise = this.beyanServis
        .setBeyanname(yeniBeyanname)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          yeniislemInternalNo = servisSonuc.Bilgiler[0].referansNo;
          console.log(yeniislemInternalNo);

          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this.session.islemInternalNo=yeniislemInternalNo;
            this.openSnackBar(yeniislemInternalNo, "Tamam");
          
          }
        },
        err => {
          console.log(err);
        }
      );
    
    alert(
      "SUCCESS!! :-)\n\n" + JSON.stringify(this.beyannameForm.value, null, 4)
    );

    this.beyannameForm.disable();
  }
  onCancel() {
    this.submitted = false;
    this.beyannameForm.disable();
  }
  
  
}
