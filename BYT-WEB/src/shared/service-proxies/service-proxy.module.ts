import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


import * as ApiServiceProxies from './service-proxies';

@NgModule({
  providers: [
        ApiServiceProxies.BeyannameServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.BeyannameBilgileriProxy,
    ]
})
export class ServiceProxyModule { }
