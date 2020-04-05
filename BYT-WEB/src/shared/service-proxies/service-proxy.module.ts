import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


import * as ApiServiceProxies from './service-proxies';
import * as ApiServiceRoles from './UserRoles';
import * as ApiValidationService from './ValidationService';


@NgModule({
  providers: [
        ApiServiceProxies.BeyannameServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceRoles.UserRoles,
        ApiValidationService.ValidationService  
    ]
})
export class ServiceProxyModule { }
