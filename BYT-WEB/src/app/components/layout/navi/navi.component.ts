import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-sidenav-list',
  templateUrl: './navi.component.html',
  styleUrls: ['./navi.component.scss']
})


export class NaviComponent implements OnInit {

  @Output() sidenavClose = new EventEmitter();

  constructor() { }
 
  ngOnInit() {
    
  }
  public onSidenavClose = () => {
    this.sidenavClose.emit();
  }
  onToggleSidenav(){}
  
}

