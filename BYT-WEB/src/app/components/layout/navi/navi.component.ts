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
  get AdminRole() {
    return this.userRoles.canAdminRoles();
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
  get MesaiRole() {
    return this.userRoles.canMesaiRoles();
  }
  get IghbRole() {
    return this.userRoles.canIghbRoles();
  }
  get DolasimRole() {
    return this.userRoles.canDolasimRoles();
  }
  public onSidenavClose = () => {
    this.sidenavClose.emit();
  }
  onToggleSidenav(){}
 
}

