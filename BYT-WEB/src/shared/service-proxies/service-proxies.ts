
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class BeyannameServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    getAllIslem(Kullanici){
        return this.http.get(this.baseUrl+'IslemHizmeti/KullaniciIleSorgulama/' + Kullanici);
      }
      getAllIslemFromRefId(refId){
        return this.http.get(this.baseUrl+'IslemHizmeti/RefIdIleSorgulama/' + refId);
      }
    
      getTarihce(IslemInternalNo){
        return this.http.get(this.baseUrl+'TarihceHizmeti/' + IslemInternalNo);
      }
    
      KontrolGonderimi(IslemInternalNo,Kullanici){
        return this.http.get(this.baseUrl+"Servis/Beyanname/KontrolHizmeti/" + IslemInternalNo + "/" + Kullanici);
      }

   
    }

  


@Injectable()
export class SessionServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
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
    refId: string | undefined;
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
    islemZamani: Date ;
    kullanici: string | undefined;
    olusturmaZamani:Date ;

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
            this.refId = data["refId"];
            this.guidof = data["guidof"];
            this.gonderimSayisi = data["gonderimSayisi"];
            this.beyanInternalNo= data["beyanInternalNo"];
            this.beyanNo= data["beyanNo"];
            this.beyanTipi= data["beyanTipi"];
            this.id= data["id"];
            this.islemDurumu= data["islemDurumu"];
            this.islemInternalNo= data["islemInternalNo"];
            this.islemSonucu= data["islemSonucu"];
            this.islemTipi= data["islemTipi"];
            this.islemZamani= data["islemZamani"];
            this.kullanici= data["kullanici"];
            this.olusturmaZamani= data["olusturmaZamani"];
        }
    }

    static fromJS(data: any): IslemDto {
        data = typeof data === 'object' ? data : {};
        let result = new IslemDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["refId"] = this.refId;
        data["guidof"] = this.guidof;
        data["gonderimSayisi"] = this.gonderimSayisi;
        data["beyanInternalNo"]=this.beyanInternalNo;
        data["beyanNo"]=this.beyanNo;
        data["beyanTipi"]=this.beyanTipi;
        data["id"]= this.id;
        data["islemDurumu"]= this.islemDurumu;
        data["islemInternalNo"]=this.islemInternalNo;
        data["islemSonucu"]=this.islemSonucu;
        data["islemTipi"]= this.islemTipi;
        data["islemZamani"]=this.islemZamani;
        data["kullanici"]= this.kullanici;
        data["olusturmaZamani"]= this.olusturmaZamani;
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
    refId: string | undefined;
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
    islemZamani: Date ;
    kullanici: string | undefined;
    olusturmaZamani:Date ;

}


export class TarihceDto implements ITarihceDto {
    id: number;
    RefId: string;
    IslemInternalNo: string;
    BeyanInternalNo: string;
    Kullanici: string;
    Guid: string;
    GonderimNo:number;
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
            this.RefId = data["RefId"];
            this.IslemInternalNo = data["IslemInternalNo"];
            this.BeyanInternalNo=data["BeyanInternalNo"];
            this.Kullanici = data["Kullanici"];
            this.Guid=data["Guid"];
            this.GonderimNo = data["GonderimNo"];
            this.BeyanNo = data["BeyanNo"];
            this.TicaretTipi = data["TicaretTipi"];
            this.IslemTipi=data["IslemTipi"];
            this.IslemDurumu = data["IslemDurumu"];
            this.IslemSonucu = data["IslemSonucu"];
            this.Gumruk = data["Gumruk"];
            this.Rejim=data["Rejim"];
            this.GonderilenVeri=data["GonderilenVeri"];
            this.SonucVeri=data["SonucVeri"];
            this.SonucZamani=data["SonucZamani"];
            this.ServistekiVeri=data["ServistekiVeri"];
        }
    }

    static fromJS(data: any): TarihceDto {
        data = typeof data === 'object' ? data : {};
        let result = new TarihceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
     
        data["RefId"]=this.RefId;
        data["IslemInternalNo"]=this.IslemInternalNo;
        data["BeyanInternalNo"]=this.BeyanInternalNo;
        data["Kullanici"]=this.Kullanici;
        data["Guid"]=this.Guid;
        data["GonderimNo"]=this.GonderimNo;
        data["BeyanNo"]= this.BeyanNo;
        data["TicaretTipi"]=this.TicaretTipi;
        data["IslemTipi"]=this.IslemTipi;
        data["IslemDurumu"]=this.IslemDurumu;
        data["IslemSonucu"]=this.IslemSonucu;
        data["Gumruk"]=this.Gumruk;
        data["Rejim"]=this.Rejim;
        data["GonderilenVeri"]=this.GonderilenVeri;
        data["SonucVeri"]=this.SonucVeri;
        data["SonucZamani"]= this.SonucZamani;
        data["ServistekiVeri"]=this.ServistekiVeri;
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
    RefId: string;
    IslemInternalNo: string;
    Kullanici: string;
    Guid: string;
    GonderimNo:number;
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
            this.hatalar=data["Hatalar"];
        }
    }

    static fromJS(data: any): SonucDto {
        data = typeof data === 'object' ? data : {};
        let result = new SonucDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
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
    veri: object [] | undefined;
    mesaj: string | undefined;
    hatalar: [];
    islem: boolean;
}

export class AuthenticateModel implements IAuthenticateModel {
    userNameOrEmailAddress: string | undefined;
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
        data = typeof data === 'object' ? data : {};
        let result = new AuthenticateModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
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
        data = typeof data === 'object' ? data : {};
        let result = new AuthenticateResultModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
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
