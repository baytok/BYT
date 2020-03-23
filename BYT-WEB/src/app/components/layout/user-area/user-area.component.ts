import { Component, OnInit,ViewEncapsulation } from '@angular/core';
import { GirisService } from '../../../../account/giris/giris.service';
@Component({
  selector: 'app-user-area',
  templateUrl: './user-area.component.html',
  styleUrls: ['./user-area.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class UserAreaComponent implements OnInit {
  shownLoginName = '';
  constructor( private  girisService: GirisService ) {
   
   }

  ngOnInit() {
    this.shownLoginName = this.girisService.getShownLoginName;
  } 
  logout(){
    this.girisService.setKullaniciCikis();
  }
}
