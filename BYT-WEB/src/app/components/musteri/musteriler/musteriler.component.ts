import { Component, Injector, Optional, Inject, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { YeniMusteriComponent } from '../yeniMusteri/yeniMusteri.component';
import { DegistirMusteriComponent } from '../degistirMusteri/degistirMusteri.component';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  NgForm
} from "@angular/forms";
import {
  MusteriDto, ServisDto
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
  selector: 'app-musteriler',
  animations: [appModuleAnimation()],
  templateUrl: './musteriler.component.html',
  styleUrls: ['./musteriler.component.css']
})
export class MusterilerComponent    implements OnInit {

  musteriDataSource: MusteriDto[]=[];

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
    this.getAllMusteri();

  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getAllMusteri(){
     this.beyanServis.getAllMusteri()
    .subscribe( (result: MusteriDto[])=>{
          this.musteriDataSource=result;
          
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
 
   }
  yenileMusteriler(){
  this.getAllMusteri();
  }
  yeniMusteri(){
    this.showCreateOrEditMusteriDialog();
  }

  silMusteri(musteri: MusteriDto){
    if(confirm(musteri.vergiNo+ '- Firmasını Silmek İstediğinizden Eminmisiniz?')){
      const promise = this.beyanServis
      .removeMusteri(musteri.id)
      .toPromise();
      promise.then(
      result => {
       
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       
        this.yenileMusteriler();
        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        this.beyanServis.errorHandel(err);    
      }
    );

   
    }
  }

  degistirMusteri(musteri: MusteriDto){
    this.showCreateOrEditMusteriDialog(musteri);
   
  }



  showCreateOrEditMusteriDialog(musteri?: MusteriDto): void {
    let sonucDialog;
    if (musteri=== undefined || musteri.id === undefined || musteri.id <= 0) {
      sonucDialog = this._dialog.open(YeniMusteriComponent,{
        width: '700px',
        height:'600px',
      
      });
    } else {
      sonucDialog = this._dialog.open(DegistirMusteriComponent,{
        width: '700px',
        height:'600px',
        data: {
          id:musteri.id,
          adres:musteri.adres,
          vergiNo:musteri.vergiNo,
          firmaAd:musteri.firmaAd,
          aktif:musteri.aktif,
          telefon:musteri.telefon,
          mailAdres:musteri.mailAdres,
      
        }
      });
    }

    sonucDialog.afterClosed().subscribe(result => {
        if (result) {
         
            this.yenileMusteriler();
        }
    });
}
 

  
}
