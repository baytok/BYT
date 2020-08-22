import { Component, OnInit } from "@angular/core";

import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from "@angular/forms";

import {
  kimlikTuru,
  firmaTipi,
} from "../../../../shared/helpers/referencesList";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../../shared/service-proxies/service-proxies";
import { ValidationService } from "../../../../shared/service-proxies/ValidationService";
import { UserRoles } from "../../../../shared/service-proxies/UserRoles";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ReferansService } from "../../../../shared/helpers/ReferansService";
import {
  ObTasiyiciFirmaDto,
  ServisDto,
  BeyanIslemDurumlari
} from "../../../../shared/service-proxies/service-proxies";
@Component({
  selector: "app-firma",
  templateUrl: "./tasiyicifirma.component.html",
  styleUrls: ["./tasiyicifirma.component.css"],
})
export class TasiyiciFirmaComponent implements OnInit {
  firmaForm: FormGroup;
  submitted: boolean = false;
  firmaSaiyisi: boolean= false;
  guidOf = this._beyanSession.guidOf;
  firma = new ObTasiyiciFirmaDto;
  islemInternalNo = this._beyanSession.islemInternalNo;
  ozetBeyanInternalNo = this._beyanSession.ozetBeyanInternalNo;
  beyanStatu = this._beyanSession.beyanStatu;
  beyanDurum: BeyanIslemDurumlari=new BeyanIslemDurumlari();
  _ulkeList = this.referansService.getUlkeJSON();
  _firmaTipiList = firmaTipi;
  _kimlikTuruList = kimlikTuru;
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _userRoles: UserRoles,
    private _fb: FormBuilder,
    private router: Router,
    private referansService: ReferansService
  ) {
    this.firmaForm = this._fb.group({
      adUnvan: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      caddeSokakNo: new FormControl("", [
        Validators.required,
        Validators.maxLength(150),
      ]),
      faks: new FormControl("", [Validators.maxLength(15)]),
      ilIlce: new FormControl("", [
        Validators.required,
        Validators.maxLength(35),
      ]),
      kimlikTuru: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),
      no: new FormControl("", [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      postaKodu: new FormControl("", [
        Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z0-9]*$"),
      ]),
      telefon: new FormControl("", [
        Validators.required,
        Validators.maxLength(100),
        Validators.pattern("^[0-9]*$"),
      ]),

      tip: new FormControl("", [Validators.required, Validators.maxLength(15)]),
      ulkeKodu: new FormControl("", [
        Validators.required,
        Validators.maxLength(9),
      ]),

      ozetBeyanInternalNo: new FormControl(
        this.ozetBeyanInternalNo,
        []
      ),
    });
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
        this._beyanSession.islemInternalNo + " ait Firma Bulunamadı",
        "Tamam"
      );
      this.router.navigateByUrl("/app/ozetbeyan");
    }
    this.getFirmaBilgileri();
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getFirmaBilgileri() {
    this.beyanServis
      .getObTasiyiciFirma(this._beyanSession.islemInternalNo)
      .subscribe(
        (result:ObTasiyiciFirmaDto) => {
        if(result!=null)
        {         
          this.firma = new ObTasiyiciFirmaDto();
          this.firma.init(result);       
          this.loadFirmaForm();
        } 
        else   this.firmaForm.disable();

        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
  }
  loadFirmaForm() {

    this.firmaSaiyisi=this.firma.adUnvan!=''?true:false;
    this.firmaForm.setValue({
      ozetBeyanInternalNo: this.ozetBeyanInternalNo,
      adUnvan: this.firma.adUnvan,
      caddeSokakNo: this.firma.caddeSokakNo,
      faks: this.firma.faks,
      ilIlce: this.firma.ilIlce,
      kimlikTuru: this.firma.kimlikTuru,
      no: this.firma.no,
      postaKodu: this.firma.postaKodu,
      telefon: this.firma.telefon,
      tip: this.firma.tip,
      ulkeKodu: this.firma.ulkeKodu,
    });

    this.firmaForm.disable();
  }

  get BeyanStatu(): boolean {
    if (this.beyanStatu === "undefined" || this.beyanStatu === null)
      return false;
      if(this.beyanDurum.islem(this.beyanStatu) == BeyanIslemDurumlari.Progress)
      return true;
    else return false;
  }

  yeniFirma() {
    this.firmaSaiyisi=false;
    this.firmaForm.enable();
    this.firmaForm.reset();
    this.firmaForm.markAllAsTouched();
    
  }
  duzeltFirma() {
    this.firmaForm.enable();
    this.firmaForm.markAllAsTouched();
  }

  silFirma() {
    if (
      confirm("Silmek İstediğinizden Eminmisiniz?")
    ) {
      this.submitted=true;
      const promiseFirma = this.beyanServis
        .resmoveObTasiyiciFirma(this.ozetBeyanInternalNo)
        .toPromise();
      promiseFirma.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          this.getFirmaBilgileri();
          this.openSnackBar(servisSonuc.Sonuc, "Tamam");
          this.firmaSaiyisi=false;
          this.firmaForm.disable();
        },
        (err) => {
          this.openSnackBar(err, "Tamam");
        }
      );

      this.firmaForm.reset();
      this.firmaForm.disable();
    }
  }
  onFirmaFormSubmit() {
    if (this.firmaForm.invalid) {
      const invalid = [];
      const controls = this.firmaForm.controls;

      for (const name in controls) {
        if (controls[name].invalid) {
          invalid.push(name);
        }
      }

      if (invalid.length > 0) {
        alert(
          "ERROR!! :-)\n\n Firma Bilgi verilerinin bazılarını değerleri veya formatı yanlış:" +
            JSON.stringify(invalid, null, 4)
        );

        return;
      }
    }
    this.submitted=true;
    this.firmaForm.get("ozetBeyanInternalNo").setValue(this.ozetBeyanInternalNo);    

      let yeniFirma= new ObTasiyiciFirmaDto;
      yeniFirma.init(this.firmaForm.value);
      console.log(yeniFirma);
      const promiseFirma = this.beyanServis
      .restoreObTasiyiciFirma(
        yeniFirma ,
        this.ozetBeyanInternalNo
      )
      .toPromise();
    promiseFirma.then(
      (result) => {
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);
        this.getFirmaBilgileri();
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
        this.firmaForm.disable();
      },
      (err) => {
        this.openSnackBar(err, "Tamam");
      }
    );
  }
}
