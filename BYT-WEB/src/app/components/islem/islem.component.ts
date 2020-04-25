import { Component,ViewChild, OnInit,ViewEncapsulation,Inject,InjectionToken } from '@angular/core';
import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../shared/service-proxies/service-proxies';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BeyannameSonucservisComponent } from '../../components/beyannamesonucservis/beyannamesonucservis.component';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {animate, state, style, transition, trigger} from '@angular/animations';
import {AppServisDurumKodlari} from '../../../shared/AppEnums';
import { Router } from "@angular/router";
import { AppSessionService } from '../../../shared/session/app-session.service';
import { GirisService } from '../../../account/giris/giris.service';
import {SelectionModel} from '@angular/cdk/collections';
import { Subscribable } from 'rxjs/Observable';
import { Observable, Subscription } from 'rxjs';
export interface Element {
  checked: boolean;
  name: string;
  position: number;
  weight: number;
  symbol: string;
  highlighted?: boolean;
  hovered?: boolean;
}

import {
   IslemDto,
   TarihceDto,
   ServisDto
  } from '../../../shared/service-proxies/service-proxies';

  
  const ELEMENT_DATA: TarihceDto[] = [
   
  ];

@Component({
  selector: 'app-islem',
  templateUrl: './islem.component.html',
  styleUrls: ['./islem.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  encapsulation: ViewEncapsulation.None,
})

export class IslemComponent implements OnInit {

  kullanici="";
  public loading = false;
  islemlerDataSource: IslemDto []=[];
  tarihceDataSource = new MatTableDataSource<TarihceDto>(ELEMENT_DATA);
  displayedColumnsIslem: string[] = ['beyanTipi','islemTipi','islemDurumu','islemZamani','islemInternalNo'];
  displayedColumnsTarihce: string[] = ['islemInternalNo','gonderimNo','islemTipi','islemDurumu','gondermeZamani','sonucZamani', 'guid'];
  expandedElement: TarihceDto | null;
  @ViewChild('tarihceOrdersPaginator',{static: true}) paginator: MatPaginator;
   @ViewChild(MatSort, {static: false}) sort: MatSort;
   selectionIslem = new SelectionModel<IslemDto>(false, []);
   selectionTarihce = new SelectionModel<TarihceDto>(false, []);
   
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private  girisService: GirisService,  
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar ,
    private _dialog: MatDialog,
    private router:Router,
       ) {
        
     }
   
  ngOnInit() {
  
   this.kullanici=this.girisService.loggedKullanici;
 
   this.yenileIslemler();
 
  
  }
 
  yenileIslemler(): void {
    this.getAllIslem();
  }

  yenileTarihce(): void {
  
     this.getTarihce(this._beyanSession.islemInternalNo);
    
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getAllIslem(){
 
    this.beyanServis.getAllIslem(this.kullanici)
    .subscribe( (result: IslemDto[])=>{
          this.islemlerDataSource=result;
          // this._beyanSession.guidOf=result[0].guidof;
          // this._beyanSession.islemInternalNo= result[0].islemInternalNo;
          // this._beyanSession.refNo=result[0].refNo;
       
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
 
   }
   getIslemFromRefNo(refNo){
    
    this.beyanServis.getAllIslemFromRefNo(refNo.value)
    .subscribe( (result: IslemDto[])=>{
      this.openSnackBar(refNo.value,'Tamam')
      this.islemlerDataSource=result;
    
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });

   }
   
   getTarihce(IslemInternalNo:string){  

    this._beyanSession.islemInternalNo=IslemInternalNo;
    this.beyanServis.getTarihce(IslemInternalNo)
    .subscribe( (result: TarihceDto[])=>{   
   
       this.tarihceDataSource.data=result;
     
      }, (err)=>{
        this.beyanServis.errorHandel(err);    
     });
    
   }

   getMessageSonucSorgula(guid:string)
   {
    this.loading = true; 
    const Servissonuc = new ServisDto;
    const promise=this.beyanServis.getSonucSorgula(guid).toPromise();
    promise.then( (result)=>{
      
      Servissonuc.init(result);
      this.loading = false; 
      this.openSnackBar(Servissonuc.Sonuc ,'Tamam')
      this.yenileTarihce();    
     
      this.loading = false;
     }, (err)=>{
      this.beyanServis.errorHandel(err);    
     });
    
   }
   getBeyanname(islemInternalNo: string)
   {
    this._beyanSession.islemInternalNo=islemInternalNo;  
    this.router.navigateByUrl('/app/beyanname');
   }
   getBeyannameSonuc(guid:string,islemInternalNo: string)
   {
     this.showSonucDialog(0,guid,islemInternalNo);
   }
   sendingKontrolMessages(islemInternalNo:string){
    this._beyanSession.islemInternalNo=islemInternalNo;  
    if(confirm('Kontrol Gönderimi Yapamak İstediğinizden Eminmisiniz?')){
      this.loading = true; 
    
     const promise=this.beyanServis.KontrolGonderimi(islemInternalNo,this.kullanici).toPromise();
     promise.then( (result)=>{  
        const sonuc_ = new ServisDto();
        sonuc_.init(result);
         this._beyanSession.guidOf=sonuc_.Bilgiler[0].guid;
         this.loading = false; 
         this.getAllIslem();
         this.getTarihce(islemInternalNo);
              this.openSnackBar( sonuc_.ServisDurumKodu===AppServisDurumKodlari.Available ? this._beyanSession.guidOf +"-"+sonuc_.Bilgiler[0].sonuc
                :sonuc_.Hatalar[0].hataKodu+"-"+sonuc_.Hatalar[0].hataAciklamasi ,'Tamam');
      }, (err)=>{
        this.beyanServis.errorHandel(err);    
     });
    
    }
    
   }

   
   showSonucDialog(id?: number, guid?: string,  islemInternalNo?: string): void {
    let sonucDialog;
    if (id === undefined || id <= 0) {
     sonucDialog = this._dialog.open(BeyannameSonucservisComponent,{
      width: '700px',
      height:'600px',
      data: {guidOf:guid, islemInternalNo:islemInternalNo}
    });
    
      sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenileIslemler();
        }
    });
    }   
   }

   rowClick (index) {
   
  }
  getMoreInformationIslem(row): string {
    return 'Referans No : '+row.refNo+
    ' \n  Gönderim No :'+ row.gonderimSayisi+
    ' \n Oluşturma Zaman:'+ row.olusturmaZamani+
    ' \n Sonuç: '+row.islemSonucu;
  }
    applyTarihceFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tarihceDataSource.filter = filterValue.trim().toLowerCase();
   }
}


