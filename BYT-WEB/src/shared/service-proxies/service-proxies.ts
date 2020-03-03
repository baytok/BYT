﻿import {
  mergeMap as _observableMergeMap,
  catchError as _observableCatch
} from "rxjs/operators";
import {
  Observable,
  throwError as _observableThrow,
  of as _observableOf
} from "rxjs";
import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpResponseBase
} from "@angular/common/http";
import { DecimalPipe } from "@angular/common";
export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");

@Injectable()
export class BeyannameServiceProxy {
  private http: HttpClient;
  private baseUrl: string;
  protected jsonParseReviver:
    | ((key: string, value: any) => any)
    | undefined = undefined;

  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.http = http;
    this.baseUrl = baseUrl ? baseUrl : "";
  }

  getAllIslem(Kullanici) {
    return this.http.get(
      this.baseUrl + "IslemHizmeti/KullaniciIleSorgulama/" + Kullanici
    );
  }
  getAllIslemFromRefNo(refNo) {
    return this.http.get(
      this.baseUrl + "IslemHizmeti/RefNoIleSorgulama/" + refNo
    );
  }

  getTarihce(IslemInternalNo) {
    return this.http.get(this.baseUrl + "TarihceHizmeti/" + IslemInternalNo);
  }

  KontrolGonderimi(IslemInternalNo, Kullanici) {
    return this.http.post<any>(
      this.baseUrl +
        "Servis/Beyanname/KontrolHizmeti/" +
        IslemInternalNo +
        "/" +
        Kullanici,
      { title: " POST Request" }
    );
  }

  getSonucSorgula(Guid) {
    return this.http.post<any>(
      this.baseUrl + "Servis/SorgulamaHizmeti/" + Guid,
      { title: " POST Request " }
    );
  }
  getBeyannameSonucSorgula(Guid, IslemInternalNo) {
    return this.http.get(
      this.baseUrl +
        "Servis/Beyanname/BeyannameSonucHizmeti/" +
        Guid +
        "/" +
        IslemInternalNo
    );
  }

  getBeyanname(IslemInternalNo) {
    return this.http.get(
      this.baseUrl + "Servis/Beyanname/Beyanname/" + IslemInternalNo
    );
  }

  setBeyanname(beyanname: BeyannameDto) {

    // const options = new RequestOptions({
    //   headers: this.getAuthorizedHeaders(),
    //   responseType: ResponseContentType.Json,
    //   withCredentials: false
    // });
    // return this.http.post( this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BeyannameOlustur/",
    // JSON.stringify({
    //   cmd: cmd,
    //   data: beyanname}), options)
      
      return this.http.post<any>(
        this.baseUrl + "Servis/Beyanname/BeyannameOlusturma/BeyannameOlustur/", 
        beyanname  
        );
  }

  //   getBeyannameKopyalama(IslemInternalNo){
  //     return this.http.post<any>(this.baseUrl+"Servis/Beyanname/BeyannameOlusturma/BeyannameKopyalama/" + IslemInternalNo, { title: ' POST Request ' });
  //  }

  getBeyannameKopyalama(IslemInternalNo) {
    return this.http.post<any>(
      this.baseUrl + "Servis/Beyanname/BeyannameKopyalama/" + IslemInternalNo,
      { title: " POST Request " }
    );
  }
}
@Injectable()
export class SessionServiceProxy {
  private http: HttpClient;
  private baseUrl: string;
  public guidOf: string;
  public refNo: string;
  public islemInternalNo: string;
  public beyanInternalNo: string;
  public beyanStatu: string;

  public Kalemler: KalemlerDto[];
  protected jsonParseReviver:
    | ((key: string, value: any) => any)
    | undefined = undefined;

  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.http = http;
    this.baseUrl = baseUrl ? baseUrl : "";
  }

  /**
   * @return Success
   */
  // getCurrentLoginInformations(): Observable<GetCurrentLoginInformationsOutput> {
  //     let url_ = this.baseUrl + "/api/services/app/Session/GetCurrentLoginInformations";
  //     url_ = url_.replace(/[?&]$/, "");

  //     let options_ : any = {
  //         observe: "response",
  //         responseType: "blob",
  //         headers: new HttpHeaders({
  //             "Accept": "text/plain"
  //         })
  //     };

  //}
}

export class IslemDto implements IIslemlerDto {
  refNo: string | undefined;
  guidof: string | undefined;
  gonderimSayisi: number;
  beyanInternalNo: string | undefined;
  beyanNo: string | undefined;
  beyanTipi: string | undefined;
  id: number;
  islemDurumu: string | undefined;
  islemInternalNo: string | undefined;
  islemSonucu: string | undefined;
  islemTipi: string | undefined;
  islemZamani: Date;
  kullanici: string | undefined;
  olusturmaZamani: Date;

  constructor(data?: IIslemlerDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.refNo = data["refNo"];
      this.guidof = data["guidof"];
      this.gonderimSayisi = data["gonderimSayisi"];
      this.beyanInternalNo = data["beyanInternalNo"];
      this.beyanNo = data["beyanNo"];
      this.beyanTipi = data["beyanTipi"];
      this.id = data["id"];
      this.islemDurumu = data["islemDurumu"];
      this.islemInternalNo = data["islemInternalNo"];
      this.islemSonucu = data["islemSonucu"];
      this.islemTipi = data["islemTipi"];
      this.islemZamani = data["islemZamani"];
      this.kullanici = data["kullanici"];
      this.olusturmaZamani = data["olusturmaZamani"];
    }
  }

  static fromJS(data: any): IslemDto {
    data = typeof data === "object" ? data : {};
    let result = new IslemDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["refNo"] = this.refNo;
    data["guidof"] = this.guidof;
    data["gonderimSayisi"] = this.gonderimSayisi;
    data["beyanInternalNo"] = this.beyanInternalNo;
    data["beyanNo"] = this.beyanNo;
    data["beyanTipi"] = this.beyanTipi;
    data["id"] = this.id;
    data["islemDurumu"] = this.islemDurumu;
    data["islemInternalNo"] = this.islemInternalNo;
    data["islemSonucu"] = this.islemSonucu;
    data["islemTipi"] = this.islemTipi;
    data["islemZamani"] = this.islemZamani;
    data["kullanici"] = this.kullanici;
    data["olusturmaZamani"] = this.olusturmaZamani;
    return data;
  }

  clone(): IslemDto {
    const json = this.toJSON();
    let result = new IslemDto();
    result.init(json);
    return result;
  }
}

export interface IIslemlerDto {
  refNo: string | undefined;
  guidof: string | undefined;
  gonderimSayisi: number;
  beyanInternalNo: string | undefined;
  beyanNo: string | undefined;
  beyanTipi: string | undefined;
  id: number;
  islemDurumu: string | undefined;
  islemInternalNo: string | undefined;
  islemSonucu: string | undefined;
  islemTipi: string | undefined;
  islemZamani: Date;
  kullanici: string | undefined;
  olusturmaZamani: Date;
}

export class TarihceDto implements ITarihceDto {
  id: number;
  RefNo: string;
  IslemInternalNo: string;
  BeyanInternalNo: string;
  Kullanici: string;
  Guid: string;
  GonderimNo: number;
  BeyanNo: string;
  TicaretTipi: string;
  IslemTipi: string;
  IslemDurumu: string;
  IslemSonucu: string;
  Gumruk: string;
  Rejim: string;
  GonderilenVeri: string;
  SonucVeri: string;
  SonucZamani: Date;
  ServistekiVeri: string;

  constructor(data?: ITarihceDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.RefNo = data["RefNo"];
      this.IslemInternalNo = data["IslemInternalNo"];
      this.BeyanInternalNo = data["BeyanInternalNo"];
      this.Kullanici = data["Kullanici"];
      this.Guid = data["Guid"];
      this.GonderimNo = data["GonderimNo"];
      this.BeyanNo = data["BeyanNo"];
      this.TicaretTipi = data["TicaretTipi"];
      this.IslemTipi = data["IslemTipi"];
      this.IslemDurumu = data["IslemDurumu"];
      this.IslemSonucu = data["IslemSonucu"];
      this.Gumruk = data["Gumruk"];
      this.Rejim = data["Rejim"];
      this.GonderilenVeri = data["GonderilenVeri"];
      this.SonucVeri = data["SonucVeri"];
      this.SonucZamani = data["SonucZamani"];
      this.ServistekiVeri = data["ServistekiVeri"];
    }
  }

  static fromJS(data: any): TarihceDto {
    data = typeof data === "object" ? data : {};
    let result = new TarihceDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    data["RefNo"] = this.RefNo;
    data["IslemInternalNo"] = this.IslemInternalNo;
    data["BeyanInternalNo"] = this.BeyanInternalNo;
    data["Kullanici"] = this.Kullanici;
    data["Guid"] = this.Guid;
    data["GonderimNo"] = this.GonderimNo;
    data["BeyanNo"] = this.BeyanNo;
    data["TicaretTipi"] = this.TicaretTipi;
    data["IslemTipi"] = this.IslemTipi;
    data["IslemDurumu"] = this.IslemDurumu;
    data["IslemSonucu"] = this.IslemSonucu;
    data["Gumruk"] = this.Gumruk;
    data["Rejim"] = this.Rejim;
    data["GonderilenVeri"] = this.GonderilenVeri;
    data["SonucVeri"] = this.SonucVeri;
    data["SonucZamani"] = this.SonucZamani;
    data["ServistekiVeri"] = this.ServistekiVeri;
    return data;
  }

  clone(): TarihceDto {
    const json = this.toJSON();
    let result = new TarihceDto();
    result.init(json);
    return result;
  }
}

export interface ITarihceDto {
  id: number;
  RefNo: string;
  IslemInternalNo: string;
  Kullanici: string;
  Guid: string;
  GonderimNo: number;
  BeyanNo: string;
  TicaretTipi: string;
  IslemTipi: string;
  IslemDurumu: string;
  IslemSonucu: string;
  Gumruk: string;
  Rejim: string;
  GonderilenVeri: string;
  SonucVeri: string;
  SonucZamani: Date;
  ServistekiVeri: string;
}

export class SonucDto implements ISonucDto {
  veri: object[] | undefined;
  mesaj: string | undefined;
  hatalar: [];
  islem: boolean;

  constructor(data?: ISonucDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.veri = data["Veri"];
      this.mesaj = data["Mesaj"];
      this.islem = data["Islem"];
      this.hatalar = data["Hatalar"];
    }
  }

  static fromJS(data: any): SonucDto {
    data = typeof data === "object" ? data : {};
    let result = new SonucDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["Veri"] = this.veri;
    data["Mesaj"] = this.mesaj;
    data["Islem"] = this.islem;
    data["Hatalar"] = this.hatalar;
    return data;
  }

  clone(): SonucDto {
    const json = this.toJSON();
    let result = new SonucDto();
    result.init(json);
    return result;
  }
}

export interface ISonucDto {
  veri: object[] | undefined;
  mesaj: string | undefined;
  hatalar: [];
  islem: boolean;
}

export class AuthenticateModel implements IAuthenticateModel {
  userNameOrEmailAddress: string | "11111111100";
  password: string | undefined;
  rememberClient: boolean;

  constructor(data?: IAuthenticateModel) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.userNameOrEmailAddress = data["userNameOrEmailAddress"];
      this.password = data["password"];
      this.rememberClient = data["rememberClient"];
    }
  }

  static fromJS(data: any): AuthenticateModel {
    data = typeof data === "object" ? data : {};
    let result = new AuthenticateModel();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["userNameOrEmailAddress"] = this.userNameOrEmailAddress;
    data["password"] = this.password;
    data["rememberClient"] = this.rememberClient;
    return data;
  }

  clone(): AuthenticateModel {
    const json = this.toJSON();
    let result = new AuthenticateModel();
    result.init(json);
    return result;
  }
}

export interface IAuthenticateModel {
  userNameOrEmailAddress: string | undefined;
  password: string | undefined;
  rememberClient: boolean;
}

export class AuthenticateResultModel implements IAuthenticateResultModel {
  accessToken: string | undefined;
  encryptedAccessToken: string | undefined;
  expireInSeconds: number;
  userId: number;

  constructor(data?: IAuthenticateResultModel) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.accessToken = data["accessToken"];
      this.encryptedAccessToken = data["encryptedAccessToken"];
      this.expireInSeconds = data["expireInSeconds"];
      this.userId = data["userId"];
    }
  }

  static fromJS(data: any): AuthenticateResultModel {
    data = typeof data === "object" ? data : {};
    let result = new AuthenticateResultModel();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};
    data["accessToken"] = this.accessToken;
    data["encryptedAccessToken"] = this.encryptedAccessToken;
    data["expireInSeconds"] = this.expireInSeconds;
    data["userId"] = this.userId;
    return data;
  }

  clone(): AuthenticateResultModel {
    const json = this.toJSON();
    let result = new AuthenticateResultModel();
    result.init(json);
    return result;
  }
}

export interface IAuthenticateResultModel {
  accessToken: string | undefined;
  encryptedAccessToken: string | undefined;
  expireInSeconds: number;
  userId: number;
}
export class ServisDto {
  ServisDurumKodu: number;
  Hatalar: HatalarDto[];
  Bilgiler: BilgilerDto[];

  constructor(data?: ServisDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      console.log(data);
      if (Array.isArray(data["bilgiler"])) {
        this.Bilgiler = [] as any;
        for (let item of data["bilgiler"]) this.Bilgiler.push(item);
      }
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }

      this.ServisDurumKodu = data["servisDurumKodlari"];
    }
  }

  static fromJS(data: any): ServisDto {
    data = typeof data === "object" ? data : {};
    let result = new ServisDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    if (Array.isArray(this.Bilgiler)) {
      data["bilgiler"] = [];
      for (let item of this.Bilgiler) data["bilgiler"].push(item);
    }
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
    data["servisDurumKodlari"] = this.ServisDurumKodu;
    return data;
  }

  clone(): ServisDto {
    const json = this.toJSON();
    let result = new ServisDto();
    result.init(json);
    return result;
  }
}

export class BeyannameSonucDto {
  Hatalar: SonucHatalarDto[];
  Belgeler: SonucBelgelerDto[];
  Vergiler: SonucVergilerDto[];
  Sorular: SonucSorularDto[];
  SoruCevaplar: SonucSoruCevaplarDto[];
  ToplamVergiler: SonucToplamVergilerDto[];
  ToplananVergiler: SonucToplananVergilerDto[];
  HesapDetaylari: SonucHesapDetaylariDto[];
  IstatistikiKiymet: SonucIstatistikiKiymetDto[];
  GumrukKiymet: SonucGumrukKiymetDto[];
  DovizKuruAlis: string;
  DovizKuruSatis: string;
  CiktiSeriNo: string;
  KalanKontor: string;
  MuayeneMemuru: string;

  constructor(data?: ServisDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      if (Array.isArray(data["belgeler"])) {
        this.Belgeler = [] as any;
        for (let item of data["belgeler"]) this.Belgeler.push(item);
      }
      if (Array.isArray(data["hatalar"])) {
        this.Hatalar = [] as any;
        for (let item of data["hatalar"]) this.Hatalar.push(item);
      }
      if (Array.isArray(data["vergiler"])) {
        this.Vergiler = [] as any;
        for (let item of data["vergiler"]) this.Vergiler.push(item);
      }
      if (Array.isArray(data["sorular"])) {
        this.Sorular = [] as any;
        for (let item of data["sorular"]) this.Sorular.push(item);
      }
      if (Array.isArray(data["soruCevap"])) {
        this.SoruCevaplar = [] as any;
        for (let item of data["soruCevap"]) this.SoruCevaplar.push(item);
      }

      if (Array.isArray(data["toplamVergiler"])) {
        this.ToplamVergiler = [] as any;
        for (let item of data["toplamVergiler"]) this.ToplamVergiler.push(item);
      }

      if (Array.isArray(data["toplananVergiler"])) {
        this.ToplananVergiler = [] as any;
        for (let item of data["toplananVergiler"])
          this.ToplananVergiler.push(item);
      }

      if (Array.isArray(data["hesapDetaylari"])) {
        this.HesapDetaylari = [] as any;
        for (let item of data["hesapDetaylari"]) this.HesapDetaylari.push(item);
      }

      if (Array.isArray(data["istatistikiKiymetler"])) {
        this.IstatistikiKiymet = [] as any;
        for (let item of data["istatistikiKiymetler"])
          this.IstatistikiKiymet.push(item);
      }

      if (Array.isArray(data["gumrukKiymetleri"])) {
        this.GumrukKiymet = [] as any;
        for (let item of data["gumrukKiymetleri"]) this.GumrukKiymet.push(item);
      }

      this.DovizKuruAlis = data["dovizKuruAlis"];
      this.DovizKuruSatis = data["dovizKuruSatis"];
      this.CiktiSeriNo = data["ciktiSeriNo"];
      this.KalanKontor = data["kalanKontor"];
      this.MuayeneMemuru = data["muayeneMemuru"];
    }
  }

  static fromJS(data: any): ServisDto {
    data = typeof data === "object" ? data : {};
    let result = new ServisDto();

    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === "object" ? data : {};

    if (Array.isArray(this.Belgeler)) {
      data["belgeler"] = [];
      for (let item of this.Belgeler) data["belgeler"].push(item);
    }
    if (Array.isArray(this.Hatalar)) {
      data["hatalar"] = [];
      for (let item of this.Hatalar) data["hatalar"].push(item);
    }
    if (Array.isArray(this.Vergiler)) {
      data["vergiler"] = [];
      for (let item of this.Vergiler) data["vergiler"].push(item);
    }
    if (Array.isArray(this.Sorular)) {
      data["sorular"] = [];
      for (let item of this.Sorular) data["sorular"].push(item);
    }
    if (Array.isArray(this.SoruCevaplar)) {
      data["soruCevap"] = [];
      for (let item of this.SoruCevaplar) data["soruCevap"].push(item);
    }
    if (Array.isArray(this.ToplamVergiler)) {
      data["toplamVergiler"] = [];
      for (let item of this.ToplamVergiler) data["toplamVergiler"].push(item);
    }
    if (Array.isArray(this.ToplananVergiler)) {
      data["toplananVergiler"] = [];
      for (let item of this.ToplananVergiler)
        data["toplananVergiler"].push(item);
    }

    if (Array.isArray(this.HesapDetaylari)) {
      data["hesapDetaylari"] = [];
      for (let item of this.HesapDetaylari) data["hesapDetaylari"].push(item);
    }

    if (Array.isArray(this.GumrukKiymet)) {
      data["gumrukKiymetleri"] = [];
      for (let item of this.GumrukKiymet) data["gumrukKiymetleri"].push(item);
    }

    if (Array.isArray(this.IstatistikiKiymet)) {
      data["istatistikiKiymetler"] = [];
      for (let item of this.IstatistikiKiymet)
        data["istatistikiKiymetler"].push(item);
    }

    data["dovizKuruAlis"] = this.DovizKuruAlis;
    data["dovizKuruSatis"] = this.DovizKuruSatis;
    data["ciktiSeriNo"] = this.CiktiSeriNo;
    data["kalanKontor"] = this.KalanKontor;
    data["muayeneMemuru"] = this.MuayeneMemuru;
    return data;
  }

  clone(): ServisDto {
    const json = this.toJSON();
    let result = new ServisDto();
    result.init(json);
    return result;
  }
}

export class HatalarDto {
  hataKodu: number;
  hataAciklamasi: string;
}

export class SonucHatalarDto {
  kalemNo: number;
  hataKodu: number;
  hataAciklamasi: string;
}
export class SonucBelgelerDto {
  kalemNo: number;
  belgeKodu: string;
  belgeAciklamasi: string;
  dogrulama: string;
  referans: string;
  belgeTarihi: string;
}

export class SonucVergilerDto {
  kalemNo: number;
  vergiKodu: string;
  vergiAciklamasi: string;
  oran: string;
  miktar: string;
  natrah: string;
  odemeSekli: string;
}

export class SonucSorularDto {
  kalemNo: number;
  soruKodu: string;
  soruAciklamasi: string;
}

export class SonucSoruCevaplarDto {
  kalemNo: number;
  soruKodu: string;
  soruCevap: string;
}

export class SonucToplamVergilerDto {
  vergiKodu: string;
  vergiAciklamasi: string;
  OdemeSekli: string;
  miktar: string;
}

export class SonucToplananVergilerDto {
  miktar: string;
  odemeSekli: string;
}

export class SonucHesapDetaylariDto {
  miktar: string;
  aciklama: string;
}

export class SonucIstatistikiKiymetDto {
  kalemNo: number;
  miktar: string;
}

export class SonucGumrukKiymetDto {
  kalemNo: number;
  miktar: string;
}

export class BilgilerDto {
  guid: string;
  islemTipi: string;
  referansNo: string;
  sonuc: string;
  sonucVeriler: object;
}
export enum ServisDurumKodlari {
  _0 = 0,
  _1 = 1,
  _2 = 2,
  _3 = 3,
  _4 = 4

  // 0= "İşlem sırasında beklenmeyen bir hata oluştu.",
  // 1 = "İşlem Başarılı.",
  // 2 = "Beklenmeyen hata.",
  // 3 = "Beyanname kayıt aşamasında hata oluştu.",
  // 4 = "Kalem kayıt aşamasında hata oluştu."
}
@Injectable()
export class BeyannameBilgileriDto {
  Beyanname: BeyannameDto;
  Kalemler: KalemlerDto[];
  BeyannameNo: string;

  init(data?: any) {
    if (data) {
      console.log(data);
      this.Beyanname = data["beyanname"];
      if (Array.isArray(data["kalemler"])) {
        this.Kalemler = [] as any;
        for (let item of data["kalemler"]) this.Kalemler.push(item);
      }

      this.BeyannameNo = data["beyanNo"];
    }
  }
}

export class BeyannameDto {
  beyanInternalNo: string;
  beyannameNo: string;
  rejim: string;
  aciklamalar: string;
  aliciSaticiIliskisi: string;
  aliciVergiNo: string;
  antrepoKodu: string;
  asilSorumluVergiNo: string;
  bankaKodu: string;
  basitlestirilmisUsul: string;
  beyanSahibiVergiNo: string;
  birlikKayitNumarasi: string;
  birlikKriptoNumarasi: string;
  cikistakiAracinKimligi: string;
  cikistakiAracinTipi: string;
  cikistakiAracinUlkesi: string;
  cikisUlkesi: string;
  esyaninBulunduguYer: string;
  gidecegiSevkUlkesi: string;
  gidecegiUlke: string;
  girisGumrukIdaresi: string;
  gondericiVergiNo: string;
  gumruk: string;
  isleminNiteligi: string;
  kapAdedi: number;
  konteyner: string;
  kullanici: string;
  limanKodu: string;
  mail1: string;
  mail2: string;
  mail3: string;
  mobil1: string;
  mobil2: string;
  musavirVergiNo: string;
  odemeAraci: string;
  musavirReferansNo: string;
  referansTarihi: string;
  refNo: string;
  sinirdakiAracinKimligi: string;
  sinirdakiAracinTipi: string;
  sinirdakiAracinUlkesi: string;
  sinirdakiTasimaSekli: string;
  tasarlananGuzergah: string;
  telafiEdiciVergi: number;
  tescilStatu: string;
  tescilTarihi: string;
  teslimSekli: string;
  teslimSekliYeri: string;
  ticaretUlkesi: string;
  toplamFatura: number;
  toplamFaturaDovizi: string;
  toplamNavlun: number;
  toplamNavlunDovizi: string;
  toplamSigorta: number;
  toplamSigortaDovizi: string;
  toplamYurtDisiHarcamalar: number;
  toplamYurtDisiHarcamalarDovizi: string;
  toplamYurtIciHarcamalar: number;
  varisGumrukIdaresi: string;
  yukBelgeleriSayisi: number;
  yuklemeBosaltmaYeri: string;

  constructor(data?: BeyannameDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.beyanInternalNo=data["beyanInternalNo"];
      this.beyannameNo=data["beyannameNo"];
      this.rejim=data["rejim"];
      this.aciklamalar=data["aciklamalar"];
      this.aliciSaticiIliskisi=data["aliciSaticiIliskisi"];
      this.aliciVergiNo=data["aliciVergiNo"];
      this.antrepoKodu=data["antrepoKodu"];
      this.asilSorumluVergiNo=data["asilSorumluVergiNo"];
      this.bankaKodu=data["bankaKodu"];
      this.basitlestirilmisUsul=data["basitlestirilmisUsul"];;
      this.beyanSahibiVergiNo=data["beyanSahibiVergiNo"];
      this.birlikKayitNumarasi=data["birlikKayitNumarasi"];
      this.birlikKriptoNumarasi=data["birlikKriptoNumarasi"];
      this.cikistakiAracinKimligi=data["cikistakiAracinKimligi"];
      this.cikistakiAracinTipi=data["cikistakiAracinTipi"];
      this.cikistakiAracinUlkesi=data["cikistakiAracinUlkesi"];
      this.cikisUlkesi=data["cikisUlkesi"];
      this.esyaninBulunduguYer=data["esyaninBulunduguYer"];
      this.gidecegiSevkUlkesi=data["gidecegiSevkUlkesi"];
      this.gidecegiUlke=data["gidecegiUlke"];
      this.girisGumrukIdaresi=data["girisGumrukIdaresi"];
      this.gondericiVergiNo=data["gondericiVergiNo"];
      this.gumruk=data["gumruk"];
      this.isleminNiteligi=data["isleminNiteligi"];
      this.kapAdedi=data["kapAdedi"];
      this.konteyner=data["konteyner"];
      this.kullanici=data["kullanici"];
      this.limanKodu=data["limanKodu"];
      this.mail1=data["mail1"];
      this.mail2=data["mail2"];
      this.mail3=data["mail3"];
      this.mobil1=data["mobil1"];
      this.mobil2=data["mobil2"];
      this.musavirVergiNo=data["musavirVergiNo"];
      this.odemeAraci=data["odemeAraci"];
      this.musavirReferansNo=data["musavirReferansNo"];
      this.referansTarihi=data["referansTarihi"];
      this.refNo=data["refNo"];
      this.sinirdakiAracinKimligi=data["sinirdakiAracinKimligi"];
      this.sinirdakiAracinTipi=data["sinirdakiAracinTipi"];
      this.sinirdakiAracinUlkesi=data["sinirdakiAracinUlkesi"];
      this.sinirdakiTasimaSekli=data["sinirdakiTasimaSekli"];
      this.tasarlananGuzergah=data["tasarlananGuzergah"];
      this.telafiEdiciVergi=data["telafiEdiciVergi"];
      this.tescilStatu=data["tescilStatu"];
      this.tescilTarihi=data["tescilTarihi"];
      this.teslimSekli=data["teslimSekli"];
      this.teslimSekliYeri=data["teslimSekliYeri"];
      this.ticaretUlkesi=data["ticaretUlkesi"];
      this.toplamFatura=data["toplamFatura"];
      this.toplamFaturaDovizi= data["toplamFaturaDovizi"];
      this.toplamNavlun=data["toplamNavlun"];
      this.toplamNavlunDovizi=data["toplamNavlunDovizi"];
      this.toplamSigorta=data["toplamSigorta"];
      this.toplamSigortaDovizi=data["toplamSigortaDovizi"];
      this.toplamYurtDisiHarcamalar=data["toplamYurtDisiHarcamalar"];
      this.toplamYurtDisiHarcamalarDovizi=data["toplamYurtDisiHarcamalarDovizi"];
      this.toplamYurtIciHarcamalar=data["toplamYurtIciHarcamalarrejim"];
      this.varisGumrukIdaresi=data["varisGumrukIdaresi"];
      this.yukBelgeleriSayisi=data["yukBelgeleriSayisi"];
      this.yuklemeBosaltmaYeri=data["yuklemeBosaltmaYeri"];
      
    }
  }

  static fromJS(data: any): BeyannameDto {
    data = typeof data === "object" ? data : {};
    let result = new BeyannameDto();

    result.init(data);
    return result;
  }


}
export class KalemlerDto {
  beyanInternalNo: string;
  kalemInternalNo: string;
  kalemSiraNo: number;
  gtip: string;
  aciklama44: string;
  adet: number;
  algilamaBirimi1: string;
  algilamaBirimi2: string;
  algilamaBirimi3: string;
  algilamaMiktari1: number;
  algilamaMiktari2: number;
  algilamaMiktari3: number;
  brutAgirlik: number;
  cins: string;
  ekKod: string;
  faturaMiktari: number;
  faturaMiktariDovizi: string;
  girisCikisAmaci: string;
  girisCikisAmaciAciklama: string;
  ikincilIslem: string;
  imalatciFirmaBilgisi: string;
  imalatciVergiNo: string;
  istatistikiKiymet: number;
  istatistikiMiktar: number;
  kalemIslemNiteligi: string;
  kullanilmisEsya: string;
  mahraceIade: string;
  marka: string;
  menseiUlke: string;
  miktar: number;
  miktarBirimi: string;
  muafiyetAciklamasi: string;
  muafiyetler1: string;
  muafiyetler2: string;
  muafiyetler3: string;
  muafiyetler4: string;
  muafiyetler5: string;
  navlunMiktari: number;
  navlunMiktariDovizi: string;
  netAgirlik: number;
  numara: string;
  ozellik: string;
  referansTarihi: string;
  satirNo: string;
  sigortaMiktari: number;
  sigortaMiktariDovizi: string;
  sinirGecisUcreti: number;
  stmIlKodu: string;
  tamamlayiciOlcuBirimi: string;
  tarifeTanimi: string;
  teslimSekli: string;
  ticariTanimi: string;
  uluslararasiAnlasma: string;
  yurtDisiDemuraj: number;
  yurtDisiDemurajDovizi: string;
  yurtDisiDiger: number;
  yurtDisiDigerAciklama: string;
  yurtDisiDigerDovizi: string;
  yurtDisiFaiz: number;
  yurtDisiFaizDovizi: string;
  yurtDisiKomisyon: number;
  yurtDisiKomisyonDovizi: string;
  yurtDisiRoyalti: number;
  yurtDisiRoyaltiDovizi: string;
  yurtIciBanka: number;
  yurtIciCevre: number;
  yurtIciDepolama: number;
  yurtIciDiger: number;
  yurtIciDigerAciklama: string;
  yurtIciKkdf: number;
  yurtIciKultur: number;
  yurtIciLiman: number;
  yurtIciTahliye: number;
}
