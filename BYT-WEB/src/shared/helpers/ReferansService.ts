
  import { Injectable } from '@angular/core';
  import { HttpClient } from '@angular/common/http'; 
  import { Observable,of } from 'rxjs';
  import * as rejimData from '../../shared/helpers/rejim.json';
  import * as bsData from '../../shared/helpers/bs.json';
  import * as gumrukData from '../../shared/helpers/gumruk.json';
  import * as ulkeData from '../../shared/helpers/ulke.json';
  import * as aliciSaticiData from '../../shared/helpers/aliciSatici.json';
  import * as tasimaSekliData from '../../shared/helpers/tasimaSekli.json';
  import * as isleminNiteligiData from '../../shared/helpers/isleminNiteligi.json';
  import * as aracTipiData from '../../shared/helpers/aracTipi.json';
  import * as teslimSekliData from '../../shared/helpers/teslimSekli.json';
  import * as dovizCinsiData from '../../shared/helpers/dovizCinsi.json';
@Injectable()
export class ReferansService  {
 
  private rejim: any = (rejimData as any).default;
  private bs: any = (bsData as any).default;
  private gumruk: any = (gumrukData as any).default;
  private ulke: any = (ulkeData as any).default;
  private aliciSatici: any = (aliciSaticiData as any).default;
  private tasimaSekli: any = (tasimaSekliData as any).default;
  private isleminNiteligi: any = (isleminNiteligiData as any).default;
  private aracTipi: any = (aracTipiData as any).default;
  private teslimSekli: any = (teslimSekliData as any).default;
  private dovizCinsi: any = (dovizCinsiData as any).default;
  constructor() {
   
}

public getRejimJSON(): Observable<any> {
    return this.rejim;
}
public getBsJSON(): Observable<any> {
  return this.bs;
}
public getGumrukJSON(): Observable<any> {
  return this.gumruk;
}
public getUlkeJSON(): Observable<any> {
  return this.ulke;
}
public getaliciSaticiJSON(): Observable<any> {
  return this.aliciSatici;
}
public gettasimaSekliJSON(): Observable<any> {
  return this.tasimaSekli;
}
public getisleminNiteligiJSON(): Observable<any> {
  return this.isleminNiteligi;
}
public getaracTipiJSON(): Observable<any> {
  return this.aracTipi;
}
public getteslimSekliJSON(): Observable<any> {
  return this.teslimSekli;
}
public getdovizCinsiJSON(): Observable<any> {
  return this.dovizCinsi;
}


}
