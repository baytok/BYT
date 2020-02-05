import { Component, OnInit } from '@angular/core';
import { BeyannameService } from '../../services/beyanname.service';

@Component({
  selector: 'app-islem',
  templateUrl: './islem.component.html',
  styleUrls: ['./islem.component.css']
})
export class IslemComponent implements OnInit {

  data={}
  constructor(private beyanServis: BeyannameService ) { }

  ngOnInit() {

    this.getAllIslem();
  }
  getAllIslem(){
    this.beyanServis.getAllIslem("11111111100")
      .subscribe( (res)=>{
       Object.keys(res).forEach( (key)=>{
        this.data[key]=res[key];
       });
      }, (err)=>{
        console.log(err);
      });
  
   }
}
