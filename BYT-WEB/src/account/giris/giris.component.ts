import { Component, OnInit } from '@angular/core';
import { SessionService } from '../../shared/session/session.service';
import { GirisService } from './giris.service';
import { Router } from "@angular/router";
@Component({
  selector: 'app-giris',
  templateUrl: './giris.component.html',
  styleUrls: ['./giris.component.scss']
})
export class GirisComponent implements OnInit {
  

  constructor(
    private _session:SessionService,
    public girisService: GirisService,
    private router:Router
     ) { }

  ngOnInit() {
    
  }

  login() {    
    
     this._session._user = this.girisService.authenticateModel;
     this.router.navigateByUrl('/Islemler');
  }

}
