
import { Component, OnInit, Inject, Injector,ViewChild,ElementRef,Injectable } from "@angular/core";
import { Router } from "@angular/router";
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
  NbTransitGumrukDto,
  NbTeminatDto,
  NbMuhurDto,
  NbRotaDto,
  ServisDto,
  BeyanIslemDurumlari
} from "../../../../shared/service-proxies/service-proxies";

@Injectable() 

@Component({
  selector: 'app-nbdetay',
  templateUrl: './nbdetay.component.html',
  styleUrls: ['./nbdetay.component.css'],
  
})
export class NbDetayComponent implements OnInit {

  nctsBeyanForm: FormGroup;
  guzergahForm: FormGroup;  
  teminatForm: FormGroup;  
  transitForm: FormGroup;  
  muhurForm: FormGroup;  
  submitted: boolean = false;  
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  nctsBeyanInternalNo = this._beyanSession.nctsBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  beyanDurum: BeyanIslemDurumlari=new BeyanIslemDurumlari();
  _nctsBeyan: NctsBeyanDto = new NctsBeyanDto();  
  _gumrukAllList =this.referansService.getNctsGumrukAllJSON();
  _gumrukList =this.referansService.getNctsTrGumrukJSON();
  _sinirgumrukList =this.referansService.getNctsSinirGumrukJSON();
  _ulkeList = this.referansService.getUlkeDilJSON();
  _dilList = this.referansService.getDilJSON();
  _dovizList = this.referansService.getTrDovizCinsiJSON();
  _teminatTipiList = this.referansService.getTrTeminatTipiSON();
  constructor(
    private referansService:ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private _userRoles:UserRoles,
    private snackBar: MatSnackBar,
    private _fb: FormBuilder,
    private  girisService: GirisService,  
    private router: Router,
  
  ) {
    (
      this.nctsBeyanForm = this._fb.group(
        {
          nctsBeyanInternalNo:[],
        }),
      this.guzergahForm = this._fb.group({
       guzergahArry: this._fb.array([this.getGuzergah()]),
      }),
      this.muhurForm = this._fb.group({
        muhurArry: this._fb.array([this.getMuhur()]),
       }),
       this.transitForm = this._fb.group({
        transitArry: this._fb.array([this.getTransit()]),
       }),
       this.teminatForm = this._fb.group({
        teminatArry: this._fb.array([this.getTeminat()]),
       })
    )
  
  }

  ngOnInit() {
    
    if(!this._userRoles.canNctsBeyanRoles())
    {
      this.openSnackBar("Beyanname Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    if (
      this._beyanSession.islemInternalNo == undefined ||
      this._beyanSession.islemInternalNo == null
    ) {
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Detay Bilgiler Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl("/app/nctsbeyan");
    }
    this.nctsBeyanForm.disable();
    this.guzergahForm.disable();
    this.teminatForm.disable();
     this.transitForm.disable();
     this.muhurForm.disable();
  
    this.getBeyanname();
  
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }


  getBeyanname() {  
   
     this.beyanServis.getNctsBeyan(this._beyanSession.islemInternalNo).subscribe(
      result => {       
        this._nctsBeyan = new NctsBeyanDto();      
        this._nctsBeyan.initalBeyan(result);
    
        if (this._nctsBeyan == null) {
           this.openSnackBar(this._beyanSession.islemInternalNo + "  Bulunamadı", "Tamam");
           this.nctsBeyanInternalNo="";
           this.beyanStatu= "" ;
           this._beyanSession.islemInternalNo = "";
           this._beyanSession.nctsBeyanInternalNo= "" ;
           this._beyanSession.beyanStatu= "" ;        
      
          return;
        }
        else{
        
          this._beyanSession.islemInternalNo = this._beyanSession.islemInternalNo;    
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
     this.beyanStatu= this._nctsBeyan.tescilStatu;
     this._beyanSession.beyanStatu= this._nctsBeyan.tescilStatu;
       this.beyanServis.getNbRota(this._beyanSession.islemInternalNo).subscribe(
        (result: NbRotaDto[]) => {
          this.initGuzergahFormArray(result);
          this.guzergahForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.beyanServis.getNbMuhur(this._beyanSession.islemInternalNo).subscribe(
        (result: NbMuhurDto[]) => {
          this.initMuhurFormArray(result);
          this.muhurForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.beyanServis.getNbTransitGumruk(this._beyanSession.islemInternalNo).subscribe(
        (result: NbTransitGumrukDto[]) => {
          this.initTransitFormArray(result);
          this.transitForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.beyanServis.getNbTeminat(this._beyanSession.islemInternalNo).subscribe(
        (result: NbTeminatDto[]) => {
          this.initTeminatFormArray(result);
          this.teminatForm.disable();
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
      this.nctsBeyanForm.disable();  

  }
  
 
  get BeyanStatu():boolean {
   
    if(this.beyanStatu==='undefined' || this.beyanStatu===null)
    return false;
    if(this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
     return true;
    else return false;
  }
 
 
  getBeyannameIslemleri() {
   
    this.nctsBeyanForm.enable();
    this.guzergahForm.enable();
    this.teminatForm.enable();
    this.transitForm.enable();  
    this.muhurForm.enable();  
    this.nctsBeyanForm.markAllAsTouched();
    this.guzergahForm.markAllAsTouched();
    this.transitForm.markAllAsTouched();
     this.teminatForm.markAllAsTouched();
    this.muhurForm.markAllAsTouched();  
   
   
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

    }       this.setTransit(); 
            this.setGuzergah(); 
            this.setMuhur(); 
            this.setTeminat(); 
            this.openSnackBar("İşlem Gerçekleştirildi", "Tamam");         
            
         
            this.nctsBeyanForm.disable();
            this.guzergahForm.disable();
            this.teminatForm.disable();
            this.transitForm.disable();
            this.muhurForm.disable();
          
  }

 //#region Teminat
  initTeminatFormArray(teminat: NbTeminatDto[]) {
 

    const formArray = this.teminatForm.get("teminatArry") as FormArray;
    formArray.clear();
   
    for (let klm of teminat) {
      let formGroup: FormGroup = new FormGroup({
        teminatTipi: new FormControl(klm.teminatTipi, [Validators.required,]),
        grnNo: new FormControl(klm.grnNo, [Validators.required, Validators.maxLength(16)]),
        digerRefNo: new FormControl(klm.digerRefNo, [ Validators.maxLength(35)]),
        erisimKodu: new FormControl(klm.digerRefNo, [ Validators.maxLength(4)]),
        dovizCinsi: new FormControl(klm.dovizCinsi, [ Validators.required,]),
        tutar: new FormControl(klm.tutar, [  Validators.required,ValidationService.decimalValidation,]),
        ecGecerliDegil: new FormControl(klm.ecGecerliDegil, [ValidationService.numberValidator,]),
        ulkeGecerliDegil: new FormControl(klm.ulkeGecerliDegil, [ Validators.maxLength(4)]),
        nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
      });
  
      formArray.push(formGroup);
    }
    this.teminatForm.setControl("teminatArry", formArray);
  }
  
  getTeminat() {
    return this._fb.group({
      teminatTipi: new FormControl("", [Validators.required,]),
      grnNo: new FormControl("", [Validators.required, Validators.maxLength(16)]),
      digerRefNo: new FormControl("", [ Validators.maxLength(35)]),
      erisimKodu: new FormControl("", [ Validators.maxLength(4)]),
      dovizCinsi: new FormControl("", [ Validators.required,]),
      tutar: new FormControl(0, [  Validators.required,ValidationService.decimalValidation,]),
      ecGecerliDegil: new FormControl(0, [ValidationService.numberValidator,]),
      ulkeGecerliDegil: new FormControl("", [ Validators.maxLength(4)]),
      nctsBeyanInternalNo: new FormControl(this.nctsBeyanInternalNo, [
        Validators.required,
      ]),
    });
  }
  
  get teminatBilgileri() {
    return this.teminatForm.get("teminatArry") as FormArray;
  }
  
  addTeminatField() {
    this.teminatBilgileri.push(this.getTeminat());
  }
  
  deleteTeminatField(index: number) {
    this.teminatBilgileri.removeAt(index);
  }
  
  setTeminat() {
    if (this.teminatBilgileri.length > 0) {
      for (let klm of this.teminatBilgileri.value) {
    
        klm.tutar =
          typeof klm.tutar == "string"
            ? parseFloat(klm.tutar)
            : klm.tutar;
        klm.ecGecerliDegil =
          typeof klm.ecGecerliDegil == "string" 
          ? parseFloat(klm.ecGecerliDegil) 
          : klm.ecGecerliDegil;
      
      }

      this.initTeminatFormArray(this.teminatBilgileri.value);
  
      if (this.teminatBilgileri.invalid) {
        const invalid = [];
        const controls = this.teminatBilgileri.controls;
  
        for (const name in controls) {
          if (controls[name].invalid) {
            invalid.push(name);
          }
        }
  
        if (invalid.length > 0) {
          alert(
            "ERROR!! :-)\n\n Teminat Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
              JSON.stringify(invalid, null, 4)
          );
  
          return;
        }
      }
    }
  
    if (this.teminatBilgileri.length >= 0) {
      const promiseTeminat = this.beyanServis
        .restoreNbTeminat(
          this.teminatBilgileri.value,
          this.nctsBeyanInternalNo,
        
        )
        .toPromise();
        promiseTeminat.then(
        (result) => {
          // const servisSonuc = new ServisDto();
          // servisSonuc.init(result);
         
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );
    }
  }
  //#endregion Teminat

//#region Guzergah

initGuzergahFormArray(guzergah: NbRotaDto[]) {
 

  const formArray = this.guzergahForm.get("guzergahArry") as FormArray;
  formArray.clear();
  for (let klm of guzergah) {
    let formGroup: FormGroup = new FormGroup({
      ulkeKodu: new FormControl(klm.ulkeKodu, [
        Validators.required,
      ]),
  
        nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
    });

    formArray.push(formGroup);
  }
  this.guzergahForm.setControl("guzergahArry", formArray);
}

getGuzergah() {
  return this._fb.group({
    ulkeKodu: new FormControl("", [Validators.required]),
    
    nctsBeyanInternalNo: new FormControl(this.nctsBeyanInternalNo, [
      Validators.required,
    ]),
  });
}

get guzergahBilgileri() {
  return this.guzergahForm.get("guzergahArry") as FormArray;
}

addGuzergahField() {
  this.guzergahBilgileri.push(this.getGuzergah());
}

deleteGuzergahField(index: number) {
  this.guzergahBilgileri.removeAt(index);
}

setGuzergah() {
  if (this.guzergahBilgileri.length > 0) {
   
    this.initGuzergahFormArray(this.guzergahBilgileri.value);

    if (this.guzergahBilgileri.invalid) {
      const invalid = [];
      const controls = this.guzergahBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Güzergah Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

        return;
      }
    }
  }

  if (this.guzergahBilgileri.length >= 0) {
    const promiseRota = this.beyanServis
      .restoreNbRota(
        this.guzergahBilgileri.value,
        this.nctsBeyanInternalNo,
      
      )
      .toPromise();
      promiseRota.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Guzergah

//#region Muhur

initMuhurFormArray(muhur: NbMuhurDto[]) {
 
  const formArray = this.muhurForm.get("muhurArry") as FormArray;
  formArray.clear();
  for (let klm of muhur) {
    let formGroup: FormGroup = new FormGroup({
      dil: new FormControl(klm.dil, [
        Validators.required,
      ]),
      muhurNo: new FormControl(klm.muhurNo, [
        Validators.maxLength(50),
        Validators.required,
      ]),
        nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
    });

    formArray.push(formGroup);
  }
  this.muhurForm.setControl("muhurArry", formArray);
}

getMuhur() {
  return this._fb.group({
    dil: new FormControl("", [Validators.required]),
    muhurNo: new FormControl("", [Validators.required, Validators.maxLength(50)]),
    
    nctsBeyanInternalNo: new FormControl(this.nctsBeyanInternalNo, [
      Validators.required,
    ]),
  });
}

get muhurBilgileri() {
  return this.muhurForm.get("muhurArry") as FormArray;
}

addMuhurField() {
  this.muhurBilgileri.push(this.getMuhur());
}

deleteMuhurField(index: number) {
  this.muhurBilgileri.removeAt(index);
}

setMuhur() {
  if (this.muhurBilgileri.length > 0) {
   
    this.initMuhurFormArray(this.muhurBilgileri.value);

    if (this.muhurBilgileri.invalid) {
      const invalid = [];
      const controls = this.muhurBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Mühür Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

        return;
      }
    }
  }

  if (this.muhurBilgileri.length >= 0) {
    const promiseMuhur = this.beyanServis
      .restoreNbMuhur(
        this.muhurBilgileri.value,
        this.nctsBeyanInternalNo,
      
      )
      .toPromise();
      promiseMuhur.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Muhur

//#region Transit

initTransitFormArray(transit: NbTransitGumrukDto[]) {
 

  const formArray = this.transitForm.get("transitArry") as FormArray;
  formArray.clear();
  for (let klm of transit) {
    let formGroup: FormGroup = new FormGroup({
      gumruk: new FormControl(klm.gumruk, [
        Validators.required,
      ]),
      varisTarihi: new FormControl(klm.varisTarihi, [Validators.required, Validators.maxLength(20)]),
      nctsBeyanInternalNo: new FormControl(klm.nctsBeyanInternalNo),
    });

    formArray.push(formGroup);
  }
  this.transitForm.setControl("transitArry", formArray);
}

getTransit() {
  return this._fb.group({
    gumruk: new FormControl("", [
      Validators.required,
    ]),
    varisTarihi: new FormControl("", [Validators.required, Validators.maxLength(20)]),
    nctsBeyanInternalNo: new FormControl(this.nctsBeyanInternalNo, [
      Validators.required,
    ]),
  });
}

get transitBilgileri() {
  return this.transitForm.get("transitArry") as FormArray;
}

addTransitField() {
  this.transitBilgileri.push(this.getTransit());
}

deleteTransitField(index: number) {
  this.transitBilgileri.removeAt(index);
}

setTransit() {
  if (this.transitBilgileri.length > 0) {
   
    this.initTransitFormArray(this.transitBilgileri.value);

    if (this.transitBilgileri.invalid) {
      const invalid = [];
      const controls = this.transitBilgileri.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Transit Gümrük Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

        return;
      }
    }
  }

  if (this.transitBilgileri.length >= 0) {
    const promiseTransit = this.beyanServis
      .restoreNbTransitGumruk(
        this.transitBilgileri.value,
        this.nctsBeyanInternalNo,
      
      )
      .toPromise();
      promiseTransit.then(
      (result) => {
        // const servisSonuc = new ServisDto();
        // servisSonuc.init(result);
       
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
//#endregion Transit
 

onCancel() {
    this.submitted = false;
    this.nctsBeyanForm.disable();
  }
  
  
}

