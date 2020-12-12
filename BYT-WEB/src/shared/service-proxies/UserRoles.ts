import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import { GirisService } from "../../account/giris/giris.service";
import * as _ from "lodash";
import { KullaniciYetkileri } from "./service-proxies";
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  CanActivate,
  Router,
} from "@angular/router";
import { Observable } from "rxjs";
import { strict } from "assert";
import { stringify } from "querystring";

@Injectable()
export class UserRoles {
  Yetkiler: KullaniciYetkileri[];
  constructor(private girisService: GirisService, public router: Router) {}

  canBeyannameRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "DB") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
  canOzetBeyanRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "OB") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
  canNctsBeyanRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "NB") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
  canMesaiRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "MB") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
  canIghbRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "IB") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
  canDolasimRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD" || role.yetkiKodu === "DO") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }

  canAdminRoles(): boolean {
    if (this.girisService.loggedRoles != null) {
      var yetki: boolean;
      this.Yetkiler = [] as any;
      for (let item of this.girisService.loggedRoles) this.Yetkiler.push(item);

      for (let role of this.Yetkiler) {
        if (role.yetkiKodu === "AD") {
          yetki = true;
          break;
        } else yetki = false;
      }
      //  this.router.navigate(['giris'])
    }
    return yetki;
  }
}
