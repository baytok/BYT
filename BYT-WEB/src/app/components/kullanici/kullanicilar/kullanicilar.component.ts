import { Component, Injector, Optional, Inject, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { YeniKullaniciComponent } from '../yeniKullanici/yeniKullanici.component';
import { DegistirKullaniciComponent } from '../degistirKullanici/degistirKullanici.component';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import {
  KullaniciDto, ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
 import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../../shared/service-proxies/service-proxies';
 import { MatDialog } from '@angular/material/dialog';
 import { MatSnackBar } from "@angular/material/snack-bar";
 import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
@Component({
  selector: 'app-kullanicilar',
  templateUrl: './kullanicilar.component.html',
  styleUrls: ['./kullanicilar.component.css']
})
export class KullanicilarComponent    implements OnInit {

  kullaniciDataSource: KullaniciDto[]=[];

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
    if(!this._userRoles.canBeyannameRoles())
    {
      this.openSnackBar("Bu Sayfasını Görmeye Yetkiniz Yoktur.", "Tamam");
      this.beyanServis.notAuthorizeRole();    
    }
    this.getAllKullanici();

  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getAllKullanici(){
     this.beyanServis.getAllKullanici()
    .subscribe( (result: KullaniciDto[])=>{
          this.kullaniciDataSource=result;
         
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
 
   }
  yenileKullanicilar(){
  this.getAllKullanici();
  }
  yeniKullanici(){
    this.showCreateOrEditKullaniciDialog();
  }

  silKullanici(kullanici: KullaniciDto){
    if(confirm(kullanici.kullaniciKod+ '- Kullanıcısını Silmek İstediğinizden Eminmisiniz?')){
      const promise = this.beyanServis
      .removeKullanici(kullanici.id)
      .toPromise();
      promise.then(
      result => {
        console.log(result);
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       

        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );

      this.yenileKullanicilar();
    }
  }

  degistirKullanici(kullanici: KullaniciDto){
    this.showCreateOrEditKullaniciDialog(kullanici);
    this.yenileKullanicilar();
  }



  showCreateOrEditKullaniciDialog(kullanici?: KullaniciDto): void {
    let sonucDialog;
    if (kullanici=== undefined || kullanici.id === undefined || kullanici.id <= 0) {
      sonucDialog = this._dialog.open(YeniKullaniciComponent,{
        width: '700px',
        height:'600px',
      
      });
    } else {
      sonucDialog = this._dialog.open(DegistirKullaniciComponent,{
        width: '700px',
        height:'600px',
        data: {id:kullanici.id,
          kullaniciKod: kullanici.kullaniciKod,
          ad:kullanici.ad,
          soyad:kullanici.soyad,
          vergiNo:kullanici.vergiNo,
          firmaAd:kullanici.firmaAd,
          aktif:kullanici.aktif,
          telefon:kullanici.telefon,
          mailAdres:kullanici.mailAdres,
          kullaniciSifre:kullanici.kullaniciSifre,
          kullaniciSifreTekrarla:""
        }
      });
    }

    sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenileKullanicilar();
        }
    });
}
 

  
}
