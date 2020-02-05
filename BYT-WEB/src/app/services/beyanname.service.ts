import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BeyannameService {

  constructor(
    @Inject('apiUrl') private _apiUrl,
    private http:HttpClient
  ) {
    
   }

  getAllIslem(Kullanici){
    return this.http.get(this._apiUrl+'/api/IslemHizmeti/' + Kullanici);
  }
}