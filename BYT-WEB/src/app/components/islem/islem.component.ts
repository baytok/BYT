import { Component, OnInit } from '@angular/core';
import { BeyannameService } from '../../services/beyanname.service';
import {MatSnackBar,MatDialog} from '@angular/material';
import { SonucservisComponent } from '../../components/sonucservis/sonucservis.component';
import {MatTableDataSource} from '@angular/material/table';
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
  kullanici="11111111100";
  islemler: IslemDto []=[];
  tarihce: TarihceDto []=[];

  constructor(
    private beyanServis: BeyannameService,
    private snackBar: MatSnackBar ,
    private _dialog: MatDialog

    ) { }

    displayedColumnsTarihce: string[] = ['refId','islemTipi','islemDurumu','sonucZamani','gondermeZamani','gonderimNo'];
    dataSourceTarihce = new MatTableDataSource(ELEMENT_DATA);
    expandedElement: TarihceDto | null;
    
  ngOnInit() {

    this.yenile();
  }
  yenile(): void {
    this.getAllIslem();
}
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getAllIslem(){
    this.beyanServis.getAllIslem(this.kullanici)
    .subscribe( (result: IslemDto[])=>{
          this.islemler=result;
          // console.log(this.islemler);
     }, (err)=>{
       console.log(err);
     });

   }
   getIslemFromRefId(refId){
    this.beyanServis.getAllIslemFromRefId(refId.value)
    .subscribe( (result: IslemDto[])=>{
      this.openSnackBar(refId.value,'Tamam')
      this.islemler=result;
      // console.log(this.islemler);
     }, (err)=>{
       console.log(err);
     });

   }
   getTarihce(IslemInternalNo:string){
    console.log(IslemInternalNo);
    this.beyanServis.getTarihce(IslemInternalNo)
    .subscribe( (result: TarihceDto[])=>{
      this.dataSourceTarihce.data=result;
      this.tarihce=result;
       console.log(this.tarihce);
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
      width: '250px',
      data: {guidOf:guid}
    });
    
      sonucDialog.afterClosed().subscribe(result => {
        if (result) {
            this.yenile();
        }
    });
    }   
   }

    applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceTarihce.filter = filterValue.trim().toLowerCase();
   }
}

