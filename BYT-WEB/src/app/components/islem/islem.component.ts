import { Component, ViewChild, OnInit, ViewEncapsulation } from "@angular/core";
import {
  BeyannameServiceProxy,
  SessionServiceProxy,
} from "../../../shared/service-proxies/service-proxies";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BeyannameSonucServisComponent } from "../DetayliBeyan/beyannamesonucservis/beyannamesonucservis.component";
import { OzetBeyanSonucServisComponent } from "../OzetBeyan/ozetbeyansonucservis/ozetbeyansonucservis.component";

import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from "@angular/animations";
import { Router } from "@angular/router";
import { GirisService } from "../../../account/giris/giris.service";
import { SelectionModel } from "@angular/cdk/collections";
import { NgxChildProcessModule } from "ngx-childprocess";

export interface Element {
  checked: boolean;
  name: string;
  position: number;
  weight: number;
  symbol: string;
  highlighted?: boolean;
  hovered?: boolean;
}
export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

import {
  IslemDto,
  TarihceDto,
  ServisDto,
} from "../../../shared/service-proxies/service-proxies";
import { NctsBeyanComponent } from '../Ncts/nctsbeyan/nctsbeyan.component';
import { NctsSonucServisComponent } from '../Ncts/nctssonucservis/nctssonucservis.component';
import { MesaiSonucServisComponent } from '../DetayliBeyan/mesaisonucservis/mesaisonucservis.component';
import { IghbSonucServisComponent } from '../DetayliBeyan/ighbsonucservis/ighbsonucservis.component';

@Component({
  selector: "app-islem",
  templateUrl: "./islem.component.html",
  styleUrls: ["./islem.component.scss"],
  animations: [
    trigger("detailExpand", [
      state("collapsed", style({ height: "0px", minHeight: "0" })),
      state("expanded", style({ height: "*" })),
      transition(
        "expanded <=> collapsed",
        animate("225ms cubic-bezier(0.4, 0.0, 0.2, 1)")
      ),
    ]),
  ],
  encapsulation: ViewEncapsulation.None,
})
export class IslemComponent implements OnInit {
  kullanici = "";
  public loading = false;

  islemlerDataSource: IslemDto[] = [];
  tarihceDataSource = new MatTableDataSource();
  displayedColumnsIslem: string[] = [
    "beyanTipi",
    "islemTipi",
    "islemDurumu",
    "islemZamani",
    "islemInternalNo",
  ];
  displayedColumnsTarihce: string[] = [
    "islemInternalNo",
    "gonderimNo",
    "islemTipi",
    "islemDurumu",
    "gondermeZamani",
    "sonucZamani",
    "guid",
  ];
  expandedElement: TarihceDto | null;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  selectionIslem = new SelectionModel<IslemDto>(false, []);
  selectionTarihce = new SelectionModel<TarihceDto>(false, []);

  constructor(
    private beyanServis: BeyannameServiceProxy,
    private girisService: GirisService,
    private _beyanSession: SessionServiceProxy,
    private snackBar: MatSnackBar,
    private _dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit() {
    this.kullanici = this.girisService.loggedKullanici;

    this.yenileIslemler();

    this.tarihceDataSource.paginator = this.paginator;
    this.tarihceDataSource.sort = this.sort;
  }

  yenileIslemler(): void {
    this.getAllIslem();
  }

  yenileTarihce(): void {
    this.getTarihce(this._beyanSession.islemInternalNo);
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }
  getAllIslem() {
    this.beyanServis.getAllIslem(this.kullanici).subscribe(
      (result: IslemDto[]) => {
        this.islemlerDataSource = result;
        // this._beyanSession.guidOf=result[0].guidof;
        // this._beyanSession.islemInternalNo= result[0].islemInternalNo;
        // this._beyanSession.refNo=result[0].refNo;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getIslemFromRefNo(refNo) {
    this.beyanServis.getAllIslemFromRefNo(refNo.value).subscribe(
      (result: IslemDto[]) => {
        this.openSnackBar(refNo.value, "Tamam");
        this.islemlerDataSource = result;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }
  getkontrolGonderMenu(beyanTipi: string) {
    if (beyanTipi == "DetayliBeyan")
     return true;
    else  return false;
  }
  getMesaiMenu(beyanTipi: string) {
    if (beyanTipi == "Mesai")
     return true;
    else  return false;
  }

  getIghbMenu(beyanTipi: string) {
    if (beyanTipi == "Ighb")
     return true;
    else  return false;
  }
  getTescilMesajiMenu(beyanTipi: string) {
    if (beyanTipi == "Ncts" || beyanTipi == "DetayliBeyan" || beyanTipi == "OzetBeyan")
     return true;
    else  return false;
  }
  
  getTarihce(IslemInternalNo: string) {
    this._beyanSession.islemInternalNo = IslemInternalNo;
    this.beyanServis.getTarihce(IslemInternalNo).subscribe(
      (result: TarihceDto[]) => {
     
        this.tarihceDataSource.data = result;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
  }

  getMessageSonucSorgula(guid: string,islemTipi: string) {
    this.loading = true;
  
    const Servissonuc = new ServisDto();
    if ( islemTipi.trim() == "0")
    {
    const promise = this.beyanServis.getOzetBeyanServisSonucSorgula(guid).toPromise();
    promise.then(
      (result) => {
        Servissonuc.init(result);
        this.loading = false;
        this.openSnackBar(Servissonuc.Sonuc, "Tamam");
        this.yenileTarihce();

        this.loading = false;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    }
   else if ( islemTipi.trim() == "1" || islemTipi.trim() == "2")
    {
    const promise = this.beyanServis.getBeyannameServisSonucSorgula(guid).toPromise();
    promise.then(
      (result) => {
        Servissonuc.init(result);
        this.loading = false;
        this.openSnackBar(Servissonuc.Sonuc, "Tamam");
        this.yenileTarihce();

        this.loading = false;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    }
    else if ( islemTipi.trim() == "4" )
    {
    const promise = this.beyanServis.getNctsServisSonucSorgula(guid).toPromise();
    promise.then(
      (result) => {
        Servissonuc.init(result);
        this.loading = false;
        this.openSnackBar(Servissonuc.Sonuc, "Tamam");
        this.yenileTarihce();

        this.loading = false;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    }
   else if ( islemTipi.trim() == "5" )
    { 
    const promise = this.beyanServis.getIghbServisSonucSorgula(guid).toPromise();
    promise.then(
      (result) => {
        Servissonuc.init(result);
        this.loading = false;
        this.openSnackBar(Servissonuc.Sonuc, "Tamam");
        this.yenileTarihce();

        this.loading = false;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    }
    else if ( islemTipi.trim() == "6" )
    {
    const promise = this.beyanServis.getMesaiServisSonucSorgula(guid).toPromise();
    promise.then(
      (result) => {
        Servissonuc.init(result);
        this.loading = false;
        this.openSnackBar(Servissonuc.Sonuc, "Tamam");
        this.yenileTarihce();

        this.loading = false;
      },
      (err) => {
        this.beyanServis.errorHandel(err);
      }
    );
    }
  }
  getBeyanname(islemInternalNo: string, beyanTipi: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (beyanTipi == "DetayliBeyan")
      this.router.navigateByUrl("/app/dbbeyan");
    else if (beyanTipi == "OzetBeyan") this.router.navigateByUrl("/app/ozetbeyan");
    else if (beyanTipi == "Ncts") this.router.navigateByUrl("/app/nctsbeyan");
    else if (beyanTipi == "Mesai") this.router.navigateByUrl("/app/mesai");
    else if (beyanTipi == "Ighb") this.router.navigateByUrl("/app/ighb");
  }
  getBeyannameSonuc(guid: string, islemInternalNo: string, beyan: string) {
    this.showSonucDialog(0, guid, islemInternalNo,beyan);
  }
  sendingKontrolMessages(islemInternalNo: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (confirm("Kontrol Gönderimi Yapamak İstediğinizden Eminmisiniz?")) {
      this.loading = true;

      const promise = this.beyanServis
        .KontrolGonderimi(islemInternalNo, this.kullanici)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  sendingMesaiMessages(islemInternalNo: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (confirm("Mesai Başvuru Gönderimi Yapamak İstediğinizden Eminmisiniz?")) {
      this.loading = true;

      const promise = this.beyanServis
        .MesaiGonderimi(islemInternalNo, this.kullanici)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  sendingIghbMessages(islemInternalNo: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (confirm("Ighb Gönderimi Yapamak İstediğinizden Eminmisiniz?")) {
      this.loading = true;

      const promise = this.beyanServis
        .IghbGonderimi(islemInternalNo, this.kullanici)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  sendingTescilMessages(islemInternalNo: string, guid: string, beyan: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (confirm("Tescil Gönderimi Yapamak İstediğinizden Eminmisiniz?")) {
      this.loading = true;
 
    
    if(beyan.trim()==="0") {
    
      const promise = this.beyanServis
        .OzetBeyanTescilGonderimi(islemInternalNo, this.kullanici, guid)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
    else  if(beyan.trim()==='2') {     
      const promise = this.beyanServis
        .BeyannameTescilGonderimi(islemInternalNo, this.kullanici, guid)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
    else if(beyan.trim()==="4") {
    
      const promise = this.beyanServis
        .NctsTescilGonderimi(islemInternalNo, this.kullanici, guid)
        .toPromise();
      promise.then(
        (result) => {
          const servisSonuc = new ServisDto();
          servisSonuc.init(result);
          var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
          this._beyanSession.guidOf = beyanServisSonuc.Guid;
          this.loading = false;
          this.getAllIslem();
          this.getTarihce(islemInternalNo);

          this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
        },
        (err) => {
          this.beyanServis.errorHandel(err);
        }
      );
    }
  }
  }
  sendingTescilMessagesSet(islemInternalNo: string, beyan: string) {
    this._beyanSession.islemInternalNo = islemInternalNo;
    if (confirm("Tescil Mesajı Hazırlanacak, Eminmisiniz?")) {
      this.loading = true;
      if (beyan == "DetayliBeyan") {
        const promise = this.beyanServis
          .BeyannameTescilMesajiHazirla(islemInternalNo, this.kullanici)
          .toPromise();
        promise.then(
          (result) => {
            const servisSonuc = new ServisDto();
            servisSonuc.init(result);
            var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
            this._beyanSession.guidOf = beyanServisSonuc.Guid;
            this.loading = false;
            this.getAllIslem();
            this.getTarihce(islemInternalNo);

            this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
          },
          (err) => {
            this.beyanServis.errorHandel(err);
          }
        );
      }
      if (beyan == "OzetBeyan") {
        const promise = this.beyanServis
          .OzetBeyanTescilMesajiHazirla(islemInternalNo, this.kullanici)
          .toPromise();
        promise.then(
          (result) => {
            const servisSonuc = new ServisDto();
            servisSonuc.init(result);
            var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
            this._beyanSession.guidOf = beyanServisSonuc.Guid;
            this.loading = false;
            this.getAllIslem();
            this.getTarihce(islemInternalNo);

            this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
          },
          (err) => {
            this.beyanServis.errorHandel(err);
          }
        );
      }
      if (beyan == "Ncts") {
        const promise = this.beyanServis
          .NctsTescilMesajiHazirla(islemInternalNo, this.kullanici)
          .toPromise();
        promise.then(
          (result) => {
            const servisSonuc = new ServisDto();
            servisSonuc.init(result);
            var beyanServisSonuc = JSON.parse(servisSonuc.getSonuc());
            this._beyanSession.guidOf = beyanServisSonuc.Guid;
            this.loading = false;
            this.getAllIslem();
            this.getTarihce(islemInternalNo);
            this.openSnackBar(servisSonuc.getSonuc(), "Tamam");
          },
          (err) => {
            this.beyanServis.errorHandel(err);
          }
        );
      }
    }
  }

  showSonucDialog(id?: number, guid?: string, islemInternalNo?: string, beyan?:string): void {
    let sonucDialog;
    if ((beyan.trim()==='1' || beyan.trim()==='2') && (id === undefined || id <= 0)) {
      sonucDialog = this._dialog.open(BeyannameSonucServisComponent, {
        width: "700px",
        height: "600px",
        data: { guidOf: guid, islemInternalNo: islemInternalNo },
      });    
     
    } 
    else if ((beyan.trim()==='0' ))  {
      sonucDialog = this._dialog.open(OzetBeyanSonucServisComponent, {
        width: "700px",
        height: "600px",
        data: { guidOf: guid, islemInternalNo: islemInternalNo },
      });  
    } 
     else if ((beyan.trim()==='4' ) )  {
      sonucDialog = this._dialog.open(NctsSonucServisComponent, {
        width: "700px",
        height: "600px",
        data: { guidOf: guid, islemInternalNo: islemInternalNo },
      });        
    }

    else if ((beyan.trim()==='5' ) )  {
      sonucDialog = this._dialog.open(IghbSonucServisComponent, {
        width: "700px",
        height: "600px",
        data: { guidOf: guid, islemInternalNo: islemInternalNo },
      });        
    }

    else if ((beyan.trim()==='6' ) )  {
      sonucDialog = this._dialog.open(MesaiSonucServisComponent, {
        width: "700px",
        height: "600px",
        data: { guidOf: guid, islemInternalNo: islemInternalNo },
      });        
    }

    sonucDialog.afterClosed().subscribe((result) => {
        if (result) {
          this.yenileIslemler();
        }
      });
  }

  rowClick(index) {
    //TODO: satır seçildiğinde birşey yapmak istersek
  }
  getMoreInformationIslem(row): string {
    return (
      "Referans No : " +
      row.refNo +
      " \n  Gönderim No :" +
      row.gonderimSayisi +
      " \n Oluşturma Zaman:" +
      row.olusturmaZamani +
      " \n Sonuç: " +
      row.islemSonucu
    );
  }
  applyTarihceFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tarihceDataSource.filter = filterValue.trim().toLowerCase();
  }
  ngAfterViewInit() {
    this.tarihceDataSource.paginator = this.paginator;
  }

  callBytIslemler() {
    var fileName = "BYT.UI.exe";
    var path = "C:\\Projeler\\GitHub\\Repositories\\BYT\\BYT.UI\\bin\\Debug";
    var filepath =
      "C:\\Projeler\\GitHub\\Repositories\\BYT\\BYT.UI\\bin\\Debug\\BYT.UI.exe";
    const params: string[] = ["H", "He"];

    var shell = new ActiveXObject("WScript.shell");
    shell.run(filepath, params);
    shell.Quit;
  }
  loadScripts() {
    // This array contains all the files/CDNs
    const dynamicScripts = ["assets//Js//loadByt.js"];
    for (let i = 0; i < dynamicScripts.length; i++) {
      const node = document.createElement("script");
      node.src = dynamicScripts[i];
      node.type = "text/javascript";
      node.async = false;
      document.getElementsByTagName("head")[0].appendChild(node);
    }
  }
}
