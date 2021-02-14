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
  DolasimDto,
  ServisDto,
  BeyanIslemDurumlari
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";
import {
 rejim
} from "../../../../shared/helpers/referencesList";


@Component({
  selector: 'app-dolasim',
  templateUrl: './dolasim.component.html',
  styleUrls: ['./dolasim.component.css'],
 
})
export class DolasimComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  dolasimForm: FormGroup;
  submitted: boolean = false;
  ihracatEditable: boolean = false;
  ithalatEditable: boolean = false;
  dolasimInternalNo:string;
  beyanStatu:string;  
  beyanDurum: BeyanIslemDurumlari=new BeyanIslemDurumlari();
  dolasimId:string;
  editable: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _dolasim: DolasimDto = new DolasimDto();
  _gumrukList =this.referansService.getdolasimGumruk();
  _ulkeList =this.referansService.getdolasimUlke();
  _birimList =this.referansService.getdolasimBirimler();
  _dovizList =this.referansService.getdolasimDoviz();
  _evrakList =this.referansService.getdolasimEvrakTuru();
  _belgeList =this.referansService.getdolasimBelgeler();
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
    return this.dolasimForm.controls;
  }
  buildForm(): void {
    this.dolasimForm = this.formBuilder.group(
      {
       
      	dolasimInternalNo:[],
	      dolasimId:[],
        refNo: new FormControl("", [Validators.required, Validators.maxLength(30)]),
        aracAdedi: new FormControl("",[Validators.required,Validators.maxLength(12), ValidationService.numberValidator]),
        gumrukKodu:new FormControl("", [Validators.required,Validators.maxLength(9)]),
        kullaniciKodu: new FormControl(""),
        adres:new FormControl("", [Validators.maxLength(150)]),	     
        beyannameNo:new FormControl("", [Validators.required,Validators.maxLength(20)]),
        digerNo:new FormControl("", [Validators.maxLength(20)]),
        esyaninBulunduguYer:new FormControl("", [Validators.required,Validators.maxLength(100)]),
        esyaninBulunduguYerAdi:new FormControl("", [Validators.maxLength(50)]),
        esyaninBulunduguYerKodu:new FormControl("", [Validators.maxLength(30)]),       
      	firmaVergiNo:new FormControl("", [Validators.required,Validators.maxLength(15)]),	  
        globalHesaptanOdeme:new FormControl("", [Validators.maxLength(15)]),
        gumrukSahasinda:new FormControl("", [Validators.maxLength(15)]),
        irtibatAdSoyad:new FormControl("", [Validators.required,Validators.maxLength(50)]),
        irtibatTelefonNo:new FormControl("", [Validators.required,ValidationService.telefonValidation]),      
        islemTipi:new FormControl("", [Validators.required,Validators.maxLength(20)]),
        odemeYapacakFirmaVergiNo:new FormControl("", [Validators.maxLength(20)]),
        nCTSSayisi:new FormControl("", [ValidationService.numberValidator]),
        oZBYSayisi:new FormControl("", [ValidationService.numberValidator]),
        uzaklik:new FormControl("", [ValidationService.numberValidator]),
        baslangicZamani:new FormControl("", [Validators.maxLength(20)]),        
        tescilStatu: [],
        tescilTarihi:[],     
      }      
    )
  }
 
  ngOnInit() {
  
    if(!this._userRoles.canDolasimRoles())
    {
      this.openSnackBar("Mesai Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.dolasimForm.disable();

   
    if (this._beyanSession.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value=this._beyanSession.islemInternalNo;
      this.getDolasimFromIslem(this._beyanSession.islemInternalNo);
     
    }
  
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  getDolasimFromIslem(islemInternalNo:string) {  
  
    this.beyanServis.getDolasim(islemInternalNo).subscribe(
      result => {
        this._dolasim = new DolasimDto();
        this._dolasim.init(result);
        if (this._dolasim == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.dolasimInternalNo="";
          this.beyanStatu= "" ;
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.dolasimInternalNo= "" ;
          this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
         
          this._beyanSession.islemInternalNo = islemInternalNo;    
          this._beyanSession.dolasimInternalNo= this._dolasim.dolasimInternalNo ;
          this._beyanSession.beyanStatu= this._dolasim.tescilStatu ;
          this.dolasimInternalNo=this._dolasim.dolasimInternalNo;
          this.beyanStatu= this._dolasim.tescilStatu ;
          this.loadDolasimForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getDolasim(islemInternalNo) {  
   
     this.beyanServis.getDolasim(islemInternalNo.value).subscribe(
      result => {       
        this._dolasim = new DolasimDto();
      
        this._dolasim.init(result);
    
        if (this._dolasim == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.dolasimInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.dolasimInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;
         
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = islemInternalNo.value;    
          this._beyanSession.dolasimInternalNo= this._dolasim.dolasimInternalNo ;
          this._beyanSession.beyanStatu= this._dolasim.tescilStatu ;
          this.dolasimInternalNo=this._dolasim.dolasimInternalNo;
          this.beyanStatu= this._dolasim.tescilStatu ;
          this.loadDolasimForm();
         
        
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadDolasimForm()
    {
      this._beyanSession.dolasimInternalNo= this._dolasim.dolasimInternalNo;
      // this.dolasimId = this._dolasim.dolasimId!=null ? this._dolasim.dolasimId: this._beyanSession.dolasimInternalNo;
        
      //  this.dolasimForm.setValue({      
      //   dolasimInternalNo: this._mesai.dolasimInternalNo,
      //   dolasimId:this._mesai.dolasimId,
      //   refNo: this._mesai.refNo,
      //   aracAdedi:this._mesai.aracAdedi,
      //   gumrukKodu:this._mesai.gumrukKodu,
      //   kullaniciKodu:this._mesai.kullaniciKodu,
      //   adres:this._mesai.adres,
      //   beyannameNo:this._mesai.beyannameNo,
      //   digerNo:this._mesai.digerNo,
      //   esyaninBulunduguYer:this._mesai.esyaninBulunduguYer,
      //   esyaninBulunduguYerAdi:this._mesai.esyaninBulunduguYerAdi,
      //   esyaninBulunduguYerKodu:this._mesai.esyaninBulunduguYerKodu,
      // 	firmaVergiNo:this._mesai.firmaVergiNo,
      //   globalHesaptanOdeme:this._mesai.globalHesaptanOdeme,
      //   gumrukSahasinda:this._mesai.gumrukSahasinda,
      //   irtibatAdSoyad:this._mesai.irtibatAdSoyad,
      //   irtibatTelefonNo:this._mesai.irtibatTelefonNo,
      //   islemTipi:this._mesai.islemTipi,
      //   odemeYapacakFirmaVergiNo:this._mesai.odemeYapacakFirmaVergiNo,
      //   nCTSSayisi:this._mesai.nCTSSayisi,
      //   oZBYSayisi:this._mesai.oZBYSayisi,
      //   uzaklik:this._mesai.uzaklik,
      //   baslangicZamani:this._mesai.baslangicZamani,   
      //   tescilStatu:this._mesai.tescilStatu,
      //   tescilTarihi:this._mesai.tescilTarihi
     
      //  });
  
 
      this.dolasimForm.disable();
   
  }
  get yeniBeyanMenu():boolean {

    let yetkiVar:boolean=false;

    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var _usersRoles = currentUser.roles;
 
      for (let itm in _usersRoles) { 
    
        if(_usersRoles[itm].yetkiKodu=="DO" || _usersRoles[itm].yetkiKodu=="FI" )
             yetkiVar=true;
        
        
      }
    
      return yetkiVar;
  
  }
  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if(this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
     return true;
    else return false;
  }

  dolasimBasvuru() {
    this.dolasimInternalNo='Boş';
    this.beyanStatu='';
    this.dolasimForm.reset();
    this.dolasimForm.enable();
    this.islemInput.nativeElement.value="";
    this.dolasimForm.markAllAsTouched();
    this.submitted = false;

  } 
  duzeltDolasim() {
   
    this.dolasimForm.enable();  
    this.dolasimForm.markAllAsTouched();
  
    if (this.dolasimForm.invalid) {
      const invalid = [];
      const controls = this.dolasimForm.controls;
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
  silDolasim(islemInternalNo){
    if (
      confirm(islemInternalNo.value+ " Başvuruyu Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeDolasim(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.dolasimForm.reset();
          this._beyanSession.islemInternalNo="";
          this._beyanSession.dolasimInternalNo="";
          this.dolasimInternalNo="";
          this._beyanSession.beyanStatu="";
          this.beyanStatu="";
          this.islemInput.nativeElement.value="";
          islemInternalNo.value="";
          this.dolasimForm.disable();
     
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  onDolasimFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.dolasimForm.invalid) {
      const invalid = [];
      const controls = this.dolasimForm.controls;
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
   
    this.dolasimForm.get("dolasimInternalNo").setValue(this.dolasimInternalNo);    
    this.dolasimForm.get("kullaniciKodu").setValue(this.girisService.loggedKullanici);
    
    let yeniislemInternalNo: string;
    let yeniDolasim=new DolasimDto();
    yeniDolasim.init(this.dolasimForm.value);
  
      const promise = this.beyanServis
        .restoreDolasim(yeniDolasim)
        .toPromise();
      promise.then(
        result => {
         
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
        
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;          
         
          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this._dolasim.dolasimInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this.getDolasimFromIslem(yeniislemInternalNo);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");         
          
          }
            this.dolasimForm.disable();
         
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.dolasimForm.value, null, 4)
    // );

  
  }


  
  onCancel() {
    this.submitted = false;
    this.dolasimForm.disable();
  }
  
  
}
