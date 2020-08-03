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
  MesaiDto,
  ServisDto
} from "../../../../shared/service-proxies/service-proxies";
import { NativeDateAdapter, DateAdapter, MAT_DATE_FORMATS, MatDateFormats} from "@angular/material/core";
import {
 rejim
} from "../../../../shared/helpers/referencesList";


@Component({
  selector: 'app-mesai',
  templateUrl: './mesai.component.html',
  styleUrls: ['./mesai.component.css'],
 
})
export class MesaiComponent implements OnInit {
  @ViewChild('islemNo', {static: true}) private islemInput: ElementRef;
  mesaiForm: FormGroup;
  submitted: boolean = false;
  ihracatEditable: boolean = false;
  ithalatEditable: boolean = false;
  mesaiInternalNo:string;
  beyanStatu:string;
  editable: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  _mesai: MesaiDto = new MesaiDto();
  _gumrukList =this.referansService.getGumrukJSON();


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
    return this.mesaiForm.controls;
  }
  buildForm(): void {
    this.mesaiForm = this.formBuilder.group(
      {
       
      	mesaiInternalNo:[],
	      mesaiId:[],
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
  
    if(!this._userRoles.canMesaiRoles())
    {
      this.openSnackBar("Mesai Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.buildForm();
    this.mesaiForm.disable();

   
    if (this._beyanSession.islemInternalNo != undefined) {
      this.islemInput.nativeElement.value=this._beyanSession.islemInternalNo;
      this.getMesaiFromIslem(this._beyanSession.islemInternalNo);
     
    }
  
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  getMesaiFromIslem(islemInternalNo:string) {  
  
    this.beyanServis.getMesai(islemInternalNo).subscribe(
      result => {
        this._mesai = new MesaiDto();
        this._mesai.init(result);
        if (this._mesai == null) {
          this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          this.mesaiInternalNo="";
          this.beyanStatu= "" ;
          this._beyanSession.islemInternalNo = "";
          this._beyanSession.mesaiInternalNo= "" ;
          this._beyanSession.beyanStatu= "" ;
      
          return;
        }
        else{
         
          this._beyanSession.islemInternalNo = islemInternalNo;    
          this._beyanSession.mesaiInternalNo= this._mesai.mesaiInternalNo ;
          this._beyanSession.beyanStatu= this._mesai.tescilStatu ;
          this.mesaiInternalNo=this._mesai.mesaiInternalNo;
          this.beyanStatu= this._mesai.tescilStatu ;
          this.loadmesaiForm();
        }
     
      
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );
  }
  getMesai(islemInternalNo) {  
   
     this.beyanServis.getMesai(islemInternalNo.value).subscribe(
      result => {       
        this._mesai = new MesaiDto();
      
        this._mesai.init(result);
    
        if (this._mesai == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
           this.mesaiInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.mesaiInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;
         
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = islemInternalNo.value;    
          this._beyanSession.mesaiInternalNo= this._mesai.mesaiInternalNo ;
          this._beyanSession.beyanStatu= this._mesai.tescilStatu ;
          this.mesaiInternalNo=this._mesai.mesaiInternalNo;
          this.beyanStatu= this._mesai.tescilStatu ;
          this.loadmesaiForm();
         
        
        }
    
      },
      (err: any) => {     
            this.beyanServis.errorHandel(err);    
      }
     
    );
  }
  loadmesaiForm()
    {
      this._beyanSession.mesaiInternalNo= this._mesai.mesaiInternalNo;
       this.mesaiForm.setValue({      
        mesaiInternalNo: this._mesai.mesaiInternalNo,
        mesaiId:this._mesai.mesaiID,
        refNo: this._mesai.refNo,
        aracAdedi:this._mesai.aracAdedi,
        gumrukKodu:this._mesai.gumrukKodu,
        kullaniciKodu:this._mesai.kullaniciKodu,
        adres:this._mesai.adres,
        beyannameNo:this._mesai.beyannameNo,
        digerNo:this._mesai.digerNo,
        esyaninBulunduguYer:this._mesai.esyaninBulunduguYer,
        esyaninBulunduguYerAdi:this._mesai.esyaninBulunduguYerAdi,
        esyaninBulunduguYerKodu:this._mesai.esyaninBulunduguYerKodu,
      	firmaVergiNo:this._mesai.firmaVergiNo,
        globalHesaptanOdeme:this._mesai.globalHesaptanOdeme,
        gumrukSahasinda:this._mesai.gumrukSahasinda,
        irtibatAdSoyad:this._mesai.irtibatAdSoyad,
        irtibatTelefonNo:this._mesai.irtibatTelefonNo,
        islemTipi:this._mesai.islemTipi,
        odemeYapacakFirmaVergiNo:this._mesai.odemeYapacakFirmaVergiNo,
        nCTSSayisi:this._mesai.nCTSSayisi,
        oZBYSayisi:this._mesai.oZBYSayisi,
        uzaklik:this._mesai.uzaklik,
        baslangicZamani:this._mesai.baslangicZamani,   
        tescilStatu:this._mesai.tescilStatu,
        tescilTarihi:this._mesai.tescilTarihi
     
       });
  
 
      this.mesaiForm.disable();
   
  }
  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if (this.beyanStatu === 'Olusturuldu' || this.beyanStatu === 'Güncellendi')
     return true;
    else return false;
  }

  mesaiBasvuru() {
    this.mesaiInternalNo='Boş';
    this.beyanStatu='';
    this.mesaiForm.reset();
    this.mesaiForm.enable();
    this.islemInput.nativeElement.value="";
    this.mesaiForm.markAllAsTouched();
    this.submitted = false;


  } 
  duzeltMesai() {
   
    this.mesaiForm.enable();  
    this.mesaiForm.markAllAsTouched();
  
    if (this.mesaiForm.invalid) {
      const invalid = [];
      const controls = this.mesaiForm.controls;
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
  silMesai(islemInternalNo){
    if (
      confirm(islemInternalNo.value+ " Başvuruyu Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeMesai(islemInternalNo.value)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.mesaiForm.reset();
          this._beyanSession.islemInternalNo="";
          this._beyanSession.mesaiInternalNo="";
          this.mesaiInternalNo="";
          this._beyanSession.beyanStatu="";
          this.beyanStatu="";
          this.islemInput.nativeElement.value="";
          islemInternalNo.value="";
          this.mesaiForm.disable();
     
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  onmesaiFormSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.mesaiForm.invalid) {
      const invalid = [];
      const controls = this.mesaiForm.controls;
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
   
    this.mesaiForm.get("mesaiInternalNo").setValue(this.mesaiInternalNo);    
    this.mesaiForm.get("kullaniciKodu").setValue(this.girisService.loggedKullanici);
    
    let yeniislemInternalNo: string;
    let yeniMesai=new MesaiDto();
    yeniMesai.init(this.mesaiForm.value);
  
      const promise = this.beyanServis
        .restoreMesai(yeniMesai,this.mesaiInternalNo)
        .toPromise();
      promise.then(
        result => {
         
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
        
          yeniislemInternalNo = beyanServisSonuc.ReferansNo;          
         
          if (yeniislemInternalNo != null) {
            this.islemInput.nativeElement.value=yeniislemInternalNo;
            this._mesai.mesaiInternalNo=yeniislemInternalNo;
            this._beyanSession.islemInternalNo=yeniislemInternalNo;
            this.getMesaiFromIslem(yeniislemInternalNo);
            this.openSnackBar(servisSonuc.Sonuc, "Tamam");         
          
          }
            this.mesaiForm.disable();
         
        },
        err => {
          this.beyanServis.errorHandel(err);    
        }
      );
    
    // alert(
    //   "SUCCESS!! :-)\n\n" + JSON.stringify(this.mesaiForm.value, null, 4)
    // );

  
  }


  
  onCancel() {
    this.submitted = false;
    this.mesaiForm.disable();
  }
  
  
}
