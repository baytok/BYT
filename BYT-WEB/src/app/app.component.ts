import { Component,OnInit } from '@angular/core';
import { GirisService } from '../account/giris/giris.service';
import { SessionServiceProxy} from '../shared/service-proxies/service-proxies';
@Component({
  // selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'BYT-WEB';
  constructor(
    private  girisService: GirisService,  
    private _beyanSession: SessionServiceProxy,
  ) {
  
  
  }
  ngOnInit() {
    
  // console.log(this._beyanSession.token);
  }
  
  onResize(event) {
   
}
  
}

