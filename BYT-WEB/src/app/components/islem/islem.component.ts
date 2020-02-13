import { Component,ViewChild, OnInit } from '@angular/core';
import { BeyannameServiceProxy ,SessionServiceProxy} from '../../../shared/service-proxies/service-proxies';
import { MatSnackBar,MatDialog} from '@angular/material';
import { SonucservisComponent } from '../../components/sonucservis/sonucservis.component';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {animate, state, style, transition, trigger} from '@angular/animations';
import {AppServisDurumKodlari} from '../../../shared/AppEnums';
import { NgxSpinnerService } from "ngx-spinner";



import {
   IslemDto,
   TarihceDto,
   ServisDto
  } from '../../../shared/service-proxies/service-proxies';
  // export interface PeriodicElement {
  //   name: string;
  //   position: number;
  //   weight: number;
  //   symbol: string;
  // }
  
  const ELEMENT_DATA: TarihceDto[] = [
    // {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
    // {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
    
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
})

export class IslemComponent implements OnInit {
 
  kullanici="11111111100";
 
  public loading = false;

  islemlerDataSource: IslemDto []=[];
  tarihceDataSource = new MatTableDataSource(ELEMENT_DATA);
  displayedColumnsIslem: string[] = ['refId','islemTipi','beyanTipi','islemDurumu','islemSonucu','islemZamani','gonderimSayisi','islemInternalNo'];
  displayedColumnsTarihce: string[] = ['islemInternalNo','gonderimNo','islemTipi','islemDurumu','sonucZamani','gondermeZamani','guid'];
  expandedElement: TarihceDto | null;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar ,
    private _dialog: MatDialog,
    private spinner: NgxSpinnerService
    ) { }

   
    
  ngOnInit() {
   
    // this.kullanici=this._session._user.userNameOrEmailAddress;
    this.tarihceDataSource.paginator = this.paginator;
    this.tarihceDataSource.sort = this.sort;
    this.yenileIslemler();
 
  }
  yenileIslemler(): void {
    this.getAllIslem();
  }

  yenileTarihce(): void {
  
     this.getTarihce(this.session.islemInternalNo);
    
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
          this.session.guidOf=result[0].guidof;
          this.session.islemInternalNo= result[0].islemInternalNo;
          this.session.refId=result[0].refId;
       
     }, (err)=>{
       console.log(err);
     });
 
   }
   getIslemFromRefId(refId){
    
    this.beyanServis.getAllIslemFromRefId(refId.value)
    .subscribe( (result: IslemDto[])=>{
      this.openSnackBar(refId.value,'Tamam')
      this.islemlerDataSource=result;
      // console.log(this.islemler);
     }, (err)=>{
       console.log(err);
     });

   }
   getTarihce(IslemInternalNo:string){
    // this.spinner.show();
    // this.loading = true;
    // setTimeout(() => { console.log("World!"); this.spinner.hide();  this.loading = false; }, 5000);
   
    this.session.islemInternalNo=IslemInternalNo;
    this.beyanServis.getTarihce(IslemInternalNo)
    .subscribe( (result: TarihceDto[])=>{
      this.tarihceDataSource.data=result;
      }, (err)=>{
       console.log(err);
     });
    
   }

   getMessageSonucSorgula(guid:string)
   {
    this.loading = true; 
  
    this.beyanServis.getSonucSorgula(guid)
    .subscribe( (result)=>{
      const sonuc_ = new ServisDto;
      sonuc_.init(result);
      
      this.openSnackBar(sonuc_.Bilgiler[0].referansNo +"-" +sonuc_.Bilgiler[0].sonuc ,'Tamam')
      this.yenileTarihce();    
     
      this.loading = false;
     }, (err)=>{
       console.log(err);
     });
    
   }

   getSonuc(guid:string)
   {
     this.showSonucDialog(0,guid);
   }
   sendingKontrolMessages(IslemInternalNo:string){
    this.session.islemInternalNo=IslemInternalNo;  
    if(confirm('Kontrol Gönderimi Yapamak İstediğinizden Eminmisiniz?')){
      this.spinner.show();
      this.beyanServis.KontrolGonderimi(IslemInternalNo,this.kullanici)
      .subscribe( (result)=>{  
        const sonuc_ = new ServisDto();
        sonuc_.init(result);
         this.session.guidOf=sonuc_.Bilgiler[0].guid;
         //  this.getIslemFromRefId(this.session.refId);
            setTimeout(() => {  this.spinner.hide(); }, 5000);
              this.openSnackBar( sonuc_.ServisDurumKodu===AppServisDurumKodlari.Available ? this.session.guidOf +"-"+sonuc_.Bilgiler[0].sonuc
                :sonuc_.Hatalar[0].hataKodu+"-"+sonuc_.Hatalar[0].hataAciklamasi ,'Tamam');
      }, (err)=>{
       console.log(err);
     });
    
    }
    
   }

   
   showSonucDialog(id?: number, guid?: string): void {
    let sonucDialog;
    if (id === undefined || id <= 0) {
     sonucDialog = this._dialog.open(SonucservisComponent,{
      width: '700px',
      height:'600px',
      data: {guidOf:guid}
    });
    
      sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenileIslemler();
        }
    });
    }   
   }

    applyTarihceFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tarihceDataSource.filter = filterValue.trim().toLowerCase();
   }
}


