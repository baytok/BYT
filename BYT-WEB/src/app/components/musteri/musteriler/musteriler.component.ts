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
 @Component({
  selector: 'app-musteriler',
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
    private snackBar: MatSnackBar,
    ) {
    // super(injector);
  }
 
  ngOnInit() {
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
       console.log(err);
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
        console.log(result);
        const servisSonuc = new ServisDto();
        servisSonuc.init(result);       

        this.openSnackBar(servisSonuc.Sonuc, "Tamam");
      },
      err => {
        console.log(err);
      }
    );

      this.yenileMusteriler();
    }
  }

  degistirMusteri(musteri: MusteriDto){
    this.showCreateOrEditMusteriDialog(musteri);
    this.yenileMusteriler();
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
