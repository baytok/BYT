import { Component,ViewChild, OnInit } from '@angular/core';
import { BeyannameService } from '../../../shared/services/beyanname.service';
import { SessionService } from  '../../../shared/session/session.service';
import { MatSnackBar,MatDialog} from '@angular/material';
import { SonucservisComponent } from '../../components/sonucservis/sonucservis.component';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {animate, state, style, transition, trigger} from '@angular/animations';

import {
   IslemDto,
   TarihceDto
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
  islemInternalNo="";
  kullanici="11111111100";
  islemlerDataSource: IslemDto []=[];
  tarihceDataSource = new MatTableDataSource(ELEMENT_DATA);
  displayedColumnsIslem: string[] = ['refId','islemTipi','beyanTipi','islemDurumu','islemSonucu','islemZamani','gonderimSayisi','islemInternalNo'];
  displayedColumnsTarihce: string[] = ['islemInternalNo','gonderimNo','islemTipi','islemDurumu','sonucZamani','gondermeZamani','guid'];
  expandedElement: TarihceDto | null;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  constructor(
    private beyanServis: BeyannameService,
    private _session:SessionService,
    private snackBar: MatSnackBar ,
    private _dialog: MatDialog

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

  yenileTarihce(IslemInternalNo:string): void {
     this.getTarihce(IslemInternalNo);
    
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
          // console.log(this.islemler);
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
    this.islemInternalNo=IslemInternalNo;
    this.beyanServis.getTarihce(IslemInternalNo)
    .subscribe( (result: TarihceDto[])=>{
      this.tarihceDataSource.data=result;
      }, (err)=>{
       console.log(err);
     });

   }

   getSonuc(guid:string)
   {
     this.showSonucDialog(0,guid);
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

