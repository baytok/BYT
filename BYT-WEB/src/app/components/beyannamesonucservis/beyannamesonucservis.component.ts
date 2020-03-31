import { Component, OnInit, Inject,Injector } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BeyannameServiceProxy} from '../../../shared/service-proxies/service-proxies';

import {
  BeyannameSonucDto,
  SonucBelgelerDto,
  SonucHatalarDto,
  SonucVergilerDto,
  SonucSorularDto,
  SonucSoruCevaplarDto,
  SonucToplamVergilerDto,
  SonucToplananVergilerDto,
  SonucHesapDetaylariDto,
  SonucIstatistikiKiymetDto,
  SonucGumrukKiymetDto
  
 } from '../../../shared/service-proxies/service-proxies';
export interface DialogData {
  guidOf: string;
  islemInternalNo: string;

}
@Component({
  selector: 'app-sonucservis',
  templateUrl: './beyannamesonucservis.component.html',
  styleUrls: ['./beyannamesonucservis.component.scss']
  
})

export class BeyannameSonucservisComponent 
implements OnInit {
  step = 0;
  ngClass="";
  constructor(
    injector: Injector,
    public dialogRef: MatDialogRef<BeyannameSonucservisComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private beyanServis: BeyannameServiceProxy,
    
  ) { 
   
  }
  guidOf= this.data.guidOf;
  islemInternalNo= this.data.islemInternalNo;
  
  dovizKuruAlis:string;
  dovizKuruSatis:string;
  muayeneMeuru:string;
  ciktiSeriNo:string;
  kalanKontor:string;

  belgeSayisi:number;
  hataSayisi:number;
  vergiSayisi:number;
  soruSayisi:number;
  soruCevapSayisi:number;
  toplamVergiSayisi:number;
  toplananVergiSayisi:number;
  hesapDetaySayisi:number;
  istatistikiKiymetSayisi:number;
  gumrukKiymetSayisi:number;
  _sonucHatalar:SonucHatalarDto[];
  _sonucBelgeler:SonucBelgelerDto[];
  _sonucVergiler:SonucVergilerDto[];
  _sonucSorular:SonucSorularDto[];
  _sonucSoruCevaplar: SonucSoruCevaplarDto[];
  _sonucToplamVergiler:SonucToplamVergilerDto[];
  _sonucToplananVergiler:SonucToplananVergilerDto[];
  _sonucHesapDetaylari:SonucHesapDetaylariDto[];
  _sonucIstatistikiKiymet:SonucIstatistikiKiymetDto[];
  _sonucGumrukKiymet:SonucGumrukKiymetDto[];
  ngOnInit() {
    this.beyanServis.getBeyannameSonucSorgula(this.islemInternalNo,this.guidOf)
    .subscribe( (result)=>{  
     
     const sonuc = new BeyannameSonucDto();
      sonuc.init(result);     
      this._sonucBelgeler=sonuc.Belgeler;
      this._sonucHatalar =sonuc.Hatalar;
      this._sonucVergiler=sonuc.Vergiler;
      this._sonucSorular=sonuc.Sorular;
      this._sonucSoruCevaplar=sonuc.SoruCevaplar;
      this._sonucToplamVergiler=sonuc.ToplamVergiler;
      this._sonucToplananVergiler=sonuc.ToplananVergiler;
      this._sonucHesapDetaylari=sonuc.HesapDetaylari;
      this._sonucIstatistikiKiymet=sonuc.IstatistikiKiymet;
      this._sonucGumrukKiymet=sonuc.GumrukKiymet;
      this.dovizKuruAlis= sonuc.DovizKuruAlis;
      this.dovizKuruSatis= sonuc.DovizKuruSatis;
      this.muayeneMeuru= sonuc.MuayeneMemuru;
      this.ciktiSeriNo= sonuc.CiktiSeriNo;
      this.kalanKontor= sonuc.KalanKontor;

      this.belgeSayisi=this._sonucBelgeler.length;
      this.hataSayisi=this._sonucHatalar.length;
      this.vergiSayisi= this._sonucVergiler.length;
      this.soruSayisi= this._sonucSorular.length;
      this.soruCevapSayisi=this._sonucSoruCevaplar.length;
      this.toplamVergiSayisi=this._sonucToplamVergiler.length;
      this.toplananVergiSayisi=this._sonucToplananVergiler.length; 
      this.hesapDetaySayisi=this._sonucHesapDetaylari.length;  
      this.istatistikiKiymetSayisi=this._sonucIstatistikiKiymet.length;
      this.gumrukKiymetSayisi=this._sonucGumrukKiymet.length;

      if(this._sonucHatalar.length>0) {
        this.step = 1;
        this.ngClass="matClassRed";
      }
      else if(this._sonucBelgeler.length>0) 
      {
        this.step = 2;
        this.ngClass="";
      }
     

    }, (err)=>{
      this.beyanServis.errorHandel(err);    
   });
  }
  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }
  close(result: any): void {
    this.dialogRef.close(result);
  }
}
