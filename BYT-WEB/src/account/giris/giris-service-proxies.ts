﻿
export class KullaniciModel implements IKullaniciModel {
    kullanici: string | undefined;
    sifre: string | undefined;
    rememberClient: boolean;

    constructor(data?: IKullaniciModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.kullanici = data["kullanici"];
            this.sifre = data["sifre"];
            this.rememberClient = data["rememberClient"];
        }
    }

    static fromJS(data: any): KullaniciModel {
        data = typeof data === 'object' ? data : {};
        let result = new KullaniciModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["kullanici"] = this.kullanici;
        data["sifre"] = this.sifre;
        data["rememberClient"] = this.rememberClient;
        return data; 
    }

    clone(): KullaniciModel {
        const json = this.toJSON();
        let result = new KullaniciModel();
        result.init(json);
        return result;
    }
}

export interface IKullaniciModel {
    kullanici: string | undefined;
    sifre: string | undefined;
    rememberClient: boolean;
}

export class KullaniciSonucModel implements IKullaniciSonucModel {
    token: string | undefined;
    expireInSeconds: 1800;
    kullanici: string;

    constructor(data?: IKullaniciSonucModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.token = data["token"];
            this.expireInSeconds = data["expireInSeconds"];
            this.kullanici = data["kullanici"];
        }
    }

    static fromJS(data: any): KullaniciSonucModel {
        data = typeof data === 'object' ? data : {};
        let result = new KullaniciSonucModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["token"] = this.token;
        data["expireInSeconds"] = this.expireInSeconds;
        data["kullanici"] = this.kullanici;
        return data; 
    }

    clone(): KullaniciSonucModel {
        const json = this.toJSON();
        let result = new KullaniciSonucModel();
        result.init(json);
        return result;
    }
}

export interface IKullaniciSonucModel {
    token: string | undefined;
    expireInSeconds: 1800;
    kullanici: string;
}
