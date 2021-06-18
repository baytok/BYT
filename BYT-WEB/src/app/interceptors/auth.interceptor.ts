import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    var currentUser = JSON.parse(localStorage.getItem('kullaniciInfo'));
    var token = currentUser.token;
    let newRequest : HttpRequest<any>
    newRequest=request.clone({
      headers:request.headers.set("Authorization","Bearer "+ token)
    })
    return next.handle(newRequest);
  }
}
