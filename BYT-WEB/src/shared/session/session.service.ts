import { Injectable } from '@angular/core';
import { AuthenticateModel, AuthenticateResultModel } from '../../account/giris/giris-service-proxies';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

public _user: AuthenticateModel;

constructor() { }


}
