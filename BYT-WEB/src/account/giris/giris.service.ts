
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { AuthenticateModel, AuthenticateResultModel } from './giris-service-proxies';

@Injectable({
    providedIn: 'root'
  })
export class GirisService {
    
     authenticateModel=new AuthenticateModel;
     authenticateResult: AuthenticateResultModel;
    constructor(
    
    
    ) {
     
    }

}
