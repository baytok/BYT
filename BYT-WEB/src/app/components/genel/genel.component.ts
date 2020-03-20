import { Component, OnInit } from '@angular/core';
import { GirisService } from '../../../account/giris/giris.service';
import { Router } from "@angular/router";
@Component({
  selector: 'app-genel',
  templateUrl: './genel.component.html',
  styleUrls: ['./genel.component.scss']
})
export class GenelComponent implements OnInit {

  constructor(
    private  girisService: GirisService,
        private router:Router  
        ) { }

  ngOnInit() {
    if(!this.girisService.loggedIn)
    this.router.navigateByUrl('/giris');
  }

}
