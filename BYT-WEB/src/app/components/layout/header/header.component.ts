import { Component, OnInit,Output, EventEmitter,ViewEncapsulation } from '@angular/core';
import { GirisService } from '../../../../account/giris/giris.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit {
  shownLoginName = '';
  constructor( private  girisService: GirisService,  ) {
   
   }

  ngOnInit() {
    this.shownLoginName = this.girisService.getShownLoginName;
  } 
  logout(){
    this.girisService.setKullaniciCikis();
  }
  
}
