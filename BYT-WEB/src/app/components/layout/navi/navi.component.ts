import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import {
  UserRoles
} from "../../../../shared/service-proxies/UserRoles";
@Component({
  selector: 'app-sidenav-list',
  templateUrl: './navi.component.html',
  styleUrls: ['./navi.component.scss']
})


export class NaviComponent implements OnInit {
 
  @Output() sidenavClose = new EventEmitter();

  constructor(
    private userRoles: UserRoles,
  ) { }
 
  ngOnInit() {
   
  }
  get BeyannameRole() {
        return this.userRoles.canBeyannameRoles();
  }
  get OzetBeyanRole() {
    return this.userRoles.canOzetBeyanRoles();
  }
  get NctsBeyanRole() {
    return this.userRoles.canNctsBeyanRoles();
  }
  get AdminRole() {
    return this.userRoles.canAdminRoles();
}
  public onSidenavClose = () => {
    this.sidenavClose.emit();
  }
  onToggleSidenav(){}
 
}

