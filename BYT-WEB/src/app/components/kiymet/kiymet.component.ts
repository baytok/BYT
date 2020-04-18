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
import { Router } from "@angular/router";
import {
  KiymetDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
import {
  ReferansService
} from "../../../shared/helpers/ReferansService";

@Component({
  selector: 'app-kiymet',
  templateUrl: './kiymet.component.html',
  styleUrls: ['./kiymet.component.css']
})
export class KiymetComponent implements OnInit {
  kiymetInternalNo: string;
  kiymetForm: FormGroup;
  _kiymetler:KiymetDto[];
  _teslimList = this.referansService.getteslimSekliJSON();
  _aliciSaticiList = this.referansService.getaliciSaticiJSON();
  submitted: boolean = false;
  guidOf = this._beyanSession.guidOf;
  islemInternalNo = this._beyanSession.islemInternalNo;
  beyanInternalNo = this._beyanSession.beyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  
  constructor(    
    private referansService:ReferansService,
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router:Router,
    ) 
    {
      (this.kiymetForm = this._fb.group({
        
        beyanInternalNo: [],
        kiymetInternalNo: [],
      
        aliciSatici:  new FormControl("", [
          Validators.required,
          Validators.maxLength(9),
        ]),
        aliciSaticiAyrintilar: new FormControl("", [
           Validators.maxLength(300),
        ]),
        edim: new FormControl("", [
           Validators.maxLength(9),
        ]),
        emsal: new FormControl("", [
           Validators.maxLength(9),
        ]),
        faturaTarihiSayisi: new FormControl("", [
           Validators.maxLength(300),
        ]),
        gumrukIdaresiKarari: new FormControl("", [
          Validators.required,
          Validators.maxLength(300),
        ]),
        kisitlamalar: new FormControl("", [
            Validators.maxLength(9),
        ]),
        kisitlamalarAyrintilar: new FormControl("", [
            Validators.maxLength(9),
        ]),
        munasebet: new FormControl("", [
           Validators.maxLength(9),
        ]),
        royalti: new FormControl("", [
            Validators.maxLength(9),
        ]),
        royaltiKosullar: new FormControl("", [
            Validators.maxLength(300),
        ]),
        saticiyaIntikal: new FormControl("", [
            Validators.maxLength(9),
        ]),
        saticiyaIntikalKosullar: new FormControl("", [
            Validators.maxLength(300),
        ]),
        sehirYer: new FormControl("", [
          Validators.required,
          Validators.maxLength(300),
        ]),
        sozlesmeTarihiSayisi: new FormControl("", [
          Validators.required,
          Validators.maxLength(300),
        ]),
        taahhutname: [false],
      
        teslimSekli: new FormControl("", [
          Validators.required,
          Validators.maxLength(9),
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
    ){
      this.openSnackBar(
        this._beyanSession.islemInternalNo + " ait Kıymet Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl('/app/beyanname');
    }

    this.getKiymetler(this._beyanSession.islemInternalNo);
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
    this.getKiymetler(this._beyanSession.islemInternalNo);
  }
  getKiymetler(islemInternalNo: string) {
    this.beyanServis.getKiymet(islemInternalNo).subscribe(
      (result: KiymetDto[]) => {
        this._kiymetler = result;
        this.kiymetForm.disable();
       
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getKiymet(kiymetInternalNo) {
    let siraNo=null;
    for (let i in this._kiymetler) {
     if (this._kiymetler[i].kiymetInternalNo === kiymetInternalNo)
       siraNo=i;
    }
  
    this.kiymetForm.setValue({    
      beyanInternalNo: this._kiymetler[siraNo].beyanInternalNo,
      kiymetInternalNo: this._kiymetler[siraNo].kiymetInternalNo,
        aliciSatici:  this._kiymetler[siraNo].aliciSatici,
        aliciSaticiAyrintilar: this._kiymetler[siraNo].aliciSaticiAyrintilar,
        edim: this._kiymetler[siraNo].edim,
        emsal: this._kiymetler[siraNo].emsal,
        faturaTarihiSayisi: this._kiymetler[siraNo].faturaTarihiSayisi,
        gumrukIdaresiKarari: this._kiymetler[siraNo].gumrukIdaresiKarari,
        kisitlamalar: this._kiymetler[siraNo].kisitlamalar,
        kisitlamalarAyrintilar: this._kiymetler[siraNo].kisitlamalarAyrintilar,
        munasebet: this._kiymetler[siraNo].munasebet,
        royalti: this._kiymetler[siraNo].royalti,
        royaltiKosullar: this._kiymetler[siraNo].royaltiKosullar,
        saticiyaIntikal: this._kiymetler[siraNo].saticiyaIntikal,
        saticiyaIntikalKosullar: this._kiymetler[siraNo].saticiyaIntikalKosullar,
        sehirYer: this._kiymetler[siraNo].sehirYer,
        sozlesmeTarihiSayisi: this._kiymetler[siraNo].sozlesmeTarihiSayisi,
        taahhutname: this._kiymetler[siraNo].taahhutname==="Evet"?true:false,
        teslimSekli: this._kiymetler[siraNo].teslimSekli,
    });
    this.kiymetInternalNo = kiymetInternalNo;

    //Kıymet kalemi için 
    // this.beyanServis.getOdeme(this._beyanSession.islemInternalNo).subscribe(
    //   (result: OdemeDto[]) => {
    //     this._odemeler = result.filter(
    //       (x) =>
    //         x.kalemInternalNo === this._kalemler[kalemNo - 1].kalemInternalNo
    //     );
    //     this.initOdemeFormArray(this._odemeler);
    //     this.odemeForm.disable();
    //   },
    //   (err) => {
    //     this.beyanServis.errorHandel(err);
    //   }
    // );


    this.kiymetForm.disable();
  }

  yeniKiymet(){
    this.kiymetInternalNo = "Boş";
    this.kiymetForm.reset();
    this.kiymetForm.enable();
    this.kiymetForm.markAllAsTouched();
  }
  duzeltKiymet(){
    this.kiymetForm.enable();
    this.kiymetForm.markAllAsTouched();
  }
  silKiymet(kiymetInternalNo){
    if (
      confirm(kiymetInternalNo + "- kiymeti Silmek İstediğinizden Eminmisiniz?")
    ) {
      const promise = this.beyanServis
        .removeKiymet(kiymetInternalNo, this._beyanSession.beyanInternalNo)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.kiymetForm.disable();
          this.kiymetForm.reset();
          this.yukleKiymet();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
    
  }

  onkiymetFormSubmit() {
    this.submitted = true;

    if (this.kiymetForm.invalid) {
      const invalid = [];
      const controls = this.kiymetForm.controls;
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

    let tahhut =this.kiymetForm.get("taahhutname").value ===true ?"Evet":"Hayir";
  
    this.kiymetForm.setValue({ 
      beyanInternalNo:this._beyanSession.beyanInternalNo,
      kiymetInternalNo:this.kiymetInternalNo,
      aliciSatici: this.kiymetForm.get("aliciSatici").value!=null?this.kiymetForm.get("aliciSatici").value:"",
      aliciSaticiAyrintilar:this.kiymetForm.get("aliciSaticiAyrintilar").value!=null?this.kiymetForm.get("aliciSaticiAyrintilar").value:"",
      edim:this.kiymetForm.get("edim").value!=null?this.kiymetForm.get("edim").value:"",
      emsal:this.kiymetForm.get("emsal").value!=null?this.kiymetForm.get("emsal").value:"",
      faturaTarihiSayisi:this.kiymetForm.get("faturaTarihiSayisi").value!=null?this.kiymetForm.get("faturaTarihiSayisi").value:"",
      gumrukIdaresiKarari:this.kiymetForm.get("gumrukIdaresiKarari").value!=null?this.kiymetForm.get("gumrukIdaresiKarari").value:"",
      kisitlamalar:this.kiymetForm.get("kisitlamalar").value!=null?this.kiymetForm.get("kisitlamalar").value:"",
      kisitlamalarAyrintilar:this.kiymetForm.get("kisitlamalarAyrintilar").value!=null?this.kiymetForm.get("kisitlamalarAyrintilar").value:"",
      munasebet:this.kiymetForm.get("munasebet").value!=null?this.kiymetForm.get("munasebet").value:"",
      royalti:this.kiymetForm.get("royalti").value!=null?this.kiymetForm.get("royalti").value:"",
      royaltiKosullar:this.kiymetForm.get("royaltiKosullar").value!=null?this.kiymetForm.get("royaltiKosullar").value:"",
      saticiyaIntikal:this.kiymetForm.get("saticiyaIntikal").value!=null?this.kiymetForm.get("saticiyaIntikal").value:"",
      saticiyaIntikalKosullar:this.kiymetForm.get("saticiyaIntikalKosullar").value!=null?this.kiymetForm.get("saticiyaIntikalKosullar").value:"",
      sehirYer:this.kiymetForm.get("sehirYer").value!=null?this.kiymetForm.get("sehirYer").value:"",
      sozlesmeTarihiSayisi:this.kiymetForm.get("sozlesmeTarihiSayisi").value!=null?this.kiymetForm.get("sozlesmeTarihiSayisi").value:"", 
      teslimSekli:this.kiymetForm.get("teslimSekli").value!=null?this.kiymetForm.get("teslimSekli").value:"",
      taahhutname:tahhut,
      })
    // this.kiymetForm
    //   .get("beyanInternalNo")
    //   .setValue(this._beyanSession.beyanInternalNo);
   
    // this.kiymetForm.get("kiymetInternalNo").setValue(this.kiymetInternalNo); 
    // this.kiymetForm.get("taahhutname").setValue(tahhut);
  

 
    let yenikiymetInternalNo: string;
    let yeniKiymet= new KiymetDto();
    yeniKiymet.init(this.kiymetForm.value);
    console.log(yeniKiymet);
    const promiseKalem = this.beyanServis.restoreKiymet(yeniKiymet).toPromise();
    promiseKalem.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        var kiymetServisSonuc = JSON.parse(servisSonuc.getSonuc());
        yenikiymetInternalNo = kiymetServisSonuc.ReferansNo;
      
        if (yenikiymetInternalNo != null) {
          this.kiymetInternalNo = yenikiymetInternalNo;
         
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
         this.kiymetForm.disable();
         this.yukleKiymet();
        }
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
  onReset() {
    this.submitted = false;
  }
}
