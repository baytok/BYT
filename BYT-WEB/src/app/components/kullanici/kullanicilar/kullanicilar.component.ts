import { Component, Injector, Optional, Inject, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { YeniKullaniciComponent } from '../yeniKullanici/yeniKullanici.component';
import { DegistirKullaniciComponent } from '../degistirKullanici/degistirKullanici.component';
import {
  ServisDto,KullaniciDto
 } from '../../../../shared/service-proxies/service-proxies';
 import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../../shared/service-proxies/service-proxies';
 import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-kullanicilar',
  templateUrl: './kullanicilar.component.html',
  styleUrls: ['./kullanicilar.component.css']
})
export class KullanicilarComponent    implements OnInit {
  kullaniciDataSource: KullaniciDto[]=[];
  displayedColumnsKullanici: string[] = ['KullaniciKod','Ad','Soyad','VergiNo','FirmaAd','islem'];
  constructor( 
    injector: Injector,
    private beyanServis: BeyannameServiceProxy,
    private _dialog: MatDialog,
    ) {
    // super(injector);
  }

  ngOnInit() {
    this.getAllKullanici();
  }
  getAllKullanici(){
     this.beyanServis.getAllKullanici()
    .subscribe( (result: KullaniciDto[])=>{
          this.kullaniciDataSource=result;
         
     }, (err)=>{
       console.log(err);
     });
 
   }
  yenileKullanicilar(){
  this.getAllKullanici();
  }
  yeniKullanici(){
    this.showCreateOrEditTenantDialog();
  }

  silKullanici(kullanici:string){

  }

  degistirKullanici(kullanici: KullaniciDto){
    this.showCreateOrEditTenantDialog(kullanici.id);
  }



  showCreateOrEditTenantDialog(id?: number): void {
    let sonucDialog;
    if (id === undefined || id <= 0) {
      sonucDialog = this._dialog.open(YeniKullaniciComponent,{
        width: '700px',
        height:'600px',
        data: {id:id, adSoyad:""}
      });
    } else {
      sonucDialog = this._dialog.open(DegistirKullaniciComponent,{
        width: '700px',
        height:'600px',
        data: {id:id, adSoyad:""}
      });
    }

    sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenileKullanicilar();
        }
    });
}
 

  
}
