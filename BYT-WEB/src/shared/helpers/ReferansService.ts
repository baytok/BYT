
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
  import * as vergiKoduData from '../../shared/helpers/vergiKodu.json';
  import * as anlasmaData from '../../shared/helpers/anlasma.json';
  import * as muafiyetData from '../../shared/helpers/muafiyet.json';
  import * as olcuData from '../../shared/helpers/olcu.json';
  import * as belgeKodData from '../../shared/helpers/belgeKodu.json';
  import * as kapCinsiData from '../../shared/helpers/kapCinsi.json';
  import * as odemeSekliData from '../../shared/helpers/odemeSekli.json';
  import * as ozellikData from '../../shared/helpers/ozellik.json';
  import * as stmilData from '../../shared/helpers/stmil.json';
  import * as beyanTuruData from '../../shared/helpers/beyanTuru.json';
  import * as trgumrukAllData from '../../shared/helpers/trgumrukAll.json';
  import * as trgumrukData from '../../shared/helpers/trgumruk.json';
  import * as ulkeDilData from '../../shared/helpers/ulkeDil.json';
  import * as dilData from '../../shared/helpers/dil.json';
  import * as trsinirgumrukData from '../../shared/helpers/trsinirgumruk.json';
  import * as nctsodemeData from '../../shared/helpers/nctsodeme.json';
  import * as nctstasimaData from '../../shared/helpers/nctstasimaSekli.json';
  import * as trteminatData from '../../shared/helpers/trteminattipi.json';
  import * as trdovizData from '../../shared/helpers/trdoviz.json';
  
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
  private vergiKodu: any = (vergiKoduData as any).default;
  private anlasma: any = (anlasmaData as any).default;
  private muafiyet: any = (muafiyetData as any).default;
  private olcu: any = (olcuData as any).default;
  private belgeKod: any = (belgeKodData as any).default;
  private kapCinsi: any = (kapCinsiData as any).default;
  private odemeSekli: any = (odemeSekliData as any).default;
  private ozellik: any = (ozellikData as any).default;
  private stmil: any = (stmilData as any).default;
  private beyanTuru: any = (beyanTuruData as any).default;
  private trgumrukAll: any = (trgumrukAllData as any).default;
  private trgumruk: any = (trgumrukData as any).default;  
  private dil: any = (dilData as any).default;
  private ulkeDil: any = (ulkeDilData as any).default;
  private trsinirgumruk: any = (trsinirgumrukData as any).default;
  private nctsodeme: any = (nctsodemeData as any).default;
  private nctstasimaSekli: any = (nctstasimaData as any).default;
  private trdoviz: any = (trdovizData as any).default;
  private trteminat: any = (trteminatData as any).default;
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
public getvergiKoduJSON(): [] {
  return this.vergiKodu;
}
public getanlasmaJSON(): Observable<any> {
  return this.anlasma;
}
public getmuafiyetJSON(): Observable<any> {
  return this.muafiyet;
}
public getolcuJSON(): Observable<any> {
  return this.olcu;
}
public getbelgeKoduJSON(): [] {
  return this.belgeKod;
}
public getkapCinsiJSON(): Observable<any> {
  return this.kapCinsi;
}
public getodemeSekliJSON(): Observable<any> {
  return this.odemeSekli;
}
public getozellikJSON(): Observable<any> {
  return this.ozellik;
}
public getstmilJSON(): Observable<any> {
  return this.stmil;
}
public beyanTuruJSON(): Observable<any> {
  return this.beyanTuru;
}
public getTrGumrukAllJSON(): Observable<any> {
  return this.trgumrukAll;
}
public getTrGumrukJSON(): Observable<any> {
  return this.trgumruk;
}
public getDilJSON(): Observable<any> {
  return this.dil;
}
public getUlkeDilJSON(): Observable<any> {
  return this.ulkeDil;
}
public getTrSinirGumrukJSON(): Observable<any> {
  return this.trsinirgumruk;
}
public getNctsOdemeJSON(): Observable<any> {
  return this.nctsodeme;
}

public getNctsTasimaSekliJSON(): Observable<any> {
  return this.nctstasimaSekli;
}
public getTrTeminatTipiSON(): Observable<any> {
  return this.trteminat;
}

public getTrDovizCinsiJSON(): Observable<any> {
  return this.trdoviz;
}

}
