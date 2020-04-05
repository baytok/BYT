
import { Component, Injector, Optional, Inject, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { YeniYetkiComponent } from '../yeniYetki/yeniYetki.component';
import { DegistirYetkiComponent } from '../degistirYetki/degistirYetki.component';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import {
  YetkiDto, ServisDto
 } from '../../../../shared/service-proxies/service-proxies';
 import {AppServisDurumKodlari} from '../../../../shared/AppEnums';
 import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../../shared/service-proxies/service-proxies';
 import { MatDialog } from '@angular/material/dialog';
 import { MatSnackBar } from "@angular/material/snack-bar";
@Component({
  selector: 'app-yetkiler',
  templateUrl: './yetkiler.component.html',
  styleUrls: ['./yetkiler.component.css']
})
export class YetkilerComponent    implements OnInit {

  yetkiDataSource: YetkiDto[]=[];

  constructor( 
    injector: Injector,
    private beyanServis: BeyannameServiceProxy,
    private _dialog: MatDialog,
    private _fb: FormBuilder,
    private snackBar: MatSnackBar,
    ) {
    // super(injector);
  }
 
  ngOnInit() {
    this.getAllYetki();

  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getAllYetki(){
     this.beyanServis.getAllYetki()
    .subscribe( (result: YetkiDto[])=>{
          this.yetkiDataSource=result;
         
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
 
   }
  yenileYetkiler(){
  this.getAllYetki();
  }
  yeniYetki(){
    this.showCreateOrEditKullaniciDialog();
  }

  silYetki(yetki: YetkiDto){
    if(confirm(yetki.yetkiAdi+ '- Yetkiyi Silmek İstediğinizden Eminmisiniz?')){
      const promise = this.beyanServis
      .removeYetki(yetki.id)
      .toPromise();
      promise.then(
      result => {
      
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       
        this.yenileYetkiler();
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );

    
    }
  }

  degistirYetki(yetki: YetkiDto){
    this.showCreateOrEditKullaniciDialog(yetki);
  
  }



  showCreateOrEditKullaniciDialog(yetki?: YetkiDto): void {
    let sonucDialog;
    if (yetki=== undefined || yetki.id === undefined || yetki.id <= 0) {
      sonucDialog = this._dialog.open(YeniYetkiComponent,{
        width: '700px',
        height:'600px',
      
      });
    } else {
      sonucDialog = this._dialog.open(DegistirYetkiComponent,{
        width: '700px',
        height:'600px',
        data: {id:yetki.id,
          yetkiAdi: yetki.yetkiAdi,
          aciklama:yetki.aciklama,          
          aktif:yetki.aktif,
         
        }
      });
    }

    sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenileYetkiler();
        }
    });
}
 

  
}
