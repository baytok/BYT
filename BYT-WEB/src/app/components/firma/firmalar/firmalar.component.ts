import { Component, Injector, Optional, Inject, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { YeniFirmaComponent } from '../yeniFirma/yeniFirma.component';
import { DegistirFirmaComponent } from '../degistirFirma/degistirFirma.component';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import {
  FirmaDto, ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
 import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../../shared/service-proxies/service-proxies';
 import { MatDialog } from '@angular/material/dialog';
 import { MatSnackBar } from "@angular/material/snack-bar";
 import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
import { appModuleAnimation } from '../../../../shared/animations/routerTransition';
 @Component({
  selector: 'app-firmalar',
  animations: [appModuleAnimation()],
  templateUrl: './firmalar.component.html',
  styleUrls: ['./firmalar.component.css']
})
export class FirmalarComponent    implements OnInit {

  firmaDataSource: FirmaDto[]=[];

  constructor( 
    injector: Injector,
    private beyanServis: BeyannameServiceProxy,
    private _dialog: MatDialog,
    private _fb: FormBuilder,
    private _userRoles:UserRoles,
    private snackBar: MatSnackBar,
    ) {
    // super(injector);
  }
 
  ngOnInit() {
    if(!this._userRoles.canFirmaRoles())
    {
      this.openSnackBar("Bu Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.getAllFirma();

  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getAllFirma(){
     this.beyanServis.getAllFirma()
    .subscribe( (result: FirmaDto[])=>{
          this.firmaDataSource=result;
          
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
 
   }
   yenileFirmalar(){
  this.getAllFirma();
  }
  yeniFirma(){
    this.showCreateOrEditFirmaDialog();
  }

  silFirma(firma: FirmaDto){
    if(confirm(firma.vergiNo+ '- Firmasını Silmek İstediğinizden Eminmisiniz?')){
      const promise = this.beyanServis
      .removeFirma(firma.id)
      .toPromise();
      promise.then(
      result => {
       
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       
        this.yenileFirmalar();
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );

   
    }
  }

  degistirFirma(firma: FirmaDto){
    this.showCreateOrEditFirmaDialog(firma);
   
  }



  showCreateOrEditFirmaDialog(firma?: FirmaDto): void {
    let sonucDialog;
    if (firma=== undefined || firma.id === undefined || firma.id <= 0) {
      sonucDialog = this._dialog.open(YeniFirmaComponent,{
        width: '700px',
        height:'600px',
      
      });
    } else {
      sonucDialog = this._dialog.open(DegistirFirmaComponent,{
        width: '700px',
        height:'600px',
        data: {
          id:firma.id,
          musteriNo:firma.musteriNo,
          firmaNo:firma.firmaNo,
          adres:firma.adres,
          vergiNo:firma.vergiNo,
          firmaAd:firma.firmaAd,
          aktif:firma.aktif,
          telefon:firma.telefon,
          mailAdres:firma.mailAdres,
      
        }
      });
    }

    sonucDialog.afterClosed().subscribe(result => {
        if (result) {
         
            this.yenileFirmalar();
        }
    });
}
 

  
}
