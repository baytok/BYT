import { Component, OnInit, Inject, Injector } from "@angular/core";
import {
  BeyannameServiceProxy,
  SessionServiceProxy
} from "../../../shared/service-proxies/service-proxies";
import { MatSnackBar, MatDialog } from "@angular/material";

import {
  BeyannameBilgileriDto,
  BeyannameDto,
  KalemlerDto,
  ServisDto
} from "../../../shared/service-proxies/service-proxies";

@Component({
  selector: "app-beyanname",
  templateUrl: "./beyanname.component.html",
  styleUrls: ["./beyanname.component.scss"]
})
export class BeyannameComponent implements OnInit {
  constructor(
    private beyanServis: BeyannameServiceProxy,
    private session: SessionServiceProxy,
    private snackBar: MatSnackBar
  ) {}
  guidOf = this.session.guidOf;
  islemInternalNo = this.session.islemInternalNo;
  _beyannameBilgileri: BeyannameBilgileriDto;
  _beyanname: BeyannameDto=new BeyannameDto();
  _kalemler: KalemlerDto[];
  ngOnInit() {
    this.getBeyanname(this.session.islemInternalNo);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  getBeyanname(islemInternalNo: string) {
    this.beyanServis.getBeyanname(islemInternalNo).subscribe(
      result => {
        this._beyannameBilgileri = new BeyannameBilgileriDto();
        this._beyannameBilgileri.init(result);

        this._beyanname = this._beyannameBilgileri.Beyanname;
        console.log(this._beyanname);
        if (this._beyanname == null) {
           this.openSnackBar(islemInternalNo + "  Bulunamadı", "Tamam");
          return;
        }

        this._kalemler = this._beyannameBilgileri.Kalemler;
      },
      err => {
        console.log(err);
      }
    );
  }

  getBeyannameKopyalama(islemInternalNo: string) {
    if (confirm("Beyannameyi Kopyalamakta Eminmisiniz?")) {
      let yeniislemInternalNo: string;
      const promise = this.beyanServis
        .getBeyannameKopyalama(islemInternalNo)
        .toPromise();
      promise.then(
        result => {
          console.log(result);
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          yeniislemInternalNo = servisSonuc.Bilgiler[0].referansNo;
          console.log(yeniislemInternalNo);

          if (this._beyanname == null) {
            islemInternalNo = "";
            this.openSnackBar(yeniislemInternalNo, "Tamam");
          }
        },
        err => {
          console.log(err);
        }
      );
    }
  }
}
