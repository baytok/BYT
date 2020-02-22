import { Component, OnInit, Inject, Injector } from "@angular/core";
import { FormGroup, FormBuilder, Validators, FormControl, NgForm } from '@angular/forms';
import { MustMatch } from '../../../shared/helpers/must-match.validator';
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../shared/service-proxies/service-proxies";
import { MatSnackBar, MatDialog } from "@angular/material";

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
  submitted:boolean=false;
  registrationForm: FormGroup;
  guidOf = this.session.guidOf;
  islemInternalNo = this.session.islemInternalNo;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _beyanname: BeyannameDto=new BeyannameDto();
  _kalemler: KalemlerDto[];
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder
    
  ) {
    this.registrationForm = this.formBuilder.group({
      title: ['', Validators.required],    
      confirmPassword: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue],
      firstName: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(30),
        Validators.pattern('^[a-zA-Z ]*$')]),
      lastName: new FormControl('', []),
      addressGroup: this.formBuilder.group({
      address: new FormControl('', [
        Validators.required,
        Validators.maxLength(255)
      ]),
      city: new FormControl('', []),
      state: new FormControl('', []),
      pincode: new FormControl('', [
        // Validators.required,
        Validators.minLength(6),
        Validators.maxLength(8),
        Validators.pattern('^[a-zA-Z0-9]*$')])       
    }),
    phoneNumber: new FormControl('', [
      // Validators.required,
      Validators.minLength(8),
      Validators.maxLength(12),
      Validators.pattern('^[0-9]*$')]),
    email: new FormControl('', [
      // Validators.required,
      Validators.minLength(5),
      Validators.maxLength(80),
      Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")
    ]),
    password: new FormControl('', [
      // Validators.required,
      Validators.minLength(5),
      Validators.maxLength(12)        
    ])
  },
  {
    validator: MustMatch('password', 'confirmPassword')
});}
get f() { return this.registrationForm.controls; }
  ngOnInit() {
     if(this.session.islemInternalNo!=undefined)
     {
      
      this.getBeyanname(this.session.islemInternalNo);
    
     }
     registrationForm:FormGroup;
  }
  onRegistrationFormSubmit() {
    
    if(this.registrationForm.valid){      
      console.log("User Registration Form Submit", this.registrationForm.value);
    }
    
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getBeyanname(islemInternalNo) {  
  
    if(this.session.islemInternalNo===null || this.session.islemInternalNo===undefined)
    this.session.islemInternalNo=islemInternalNo.value;

    this.beyanServis.getBeyanname(this.session.islemInternalNo).subscribe(
      result => {
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);

        this._beyanname = this._beyannameBilgileri.Beyanname;
      

        if (this._beyanname == null) {
           this.openSnackBar(islemInternalNo.value + "  Bulunamadı", "Tamam");
          return;
        }
        
        this.registrationForm.setValue({
          title:"" ,    
          confirmPassword: "",
          acceptTerms: false,
          firstName:"dvlfövlşfö",
          lastName:"",         
          addressGroup:{  
          address:"gvbfg dfdfdfc",      
          city:"dvvvf",
          state:"dfdc",
          pincode:"0455"},
          phoneNumber:565656,
          email:"sdd@fgfv",
          password:"rfdvc"
          
        });
        this._kalemler = this._beyannameBilgileri.Kalemler;
      },
      err => {
        console.log(err);
      }
    );
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
            islemInternalNo.value=yeniislemInternalNo;
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

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registrationForm.invalid) {
        return;
    }

    // display form values on success
    alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.registrationForm.value, null, 4));
}

onReset() {
    this.submitted = false;
    this.registrationForm.reset();
    this.registrationForm.disable();
    
}
}
