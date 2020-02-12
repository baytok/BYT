import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
// import { ModalModule } from 'ngx-bootstrap';

import { AccountRoutingModule } from './account-routing.module';

import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';

import { SharedModule } from '../shared/shared.module';

import { AccountComponent } from './account.component';
import { GirisComponent } from './giris/giris.component';
import { GirisService } from './giris/giris.service';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,     
        SharedModule,
        ServiceProxyModule,
        AccountRoutingModule,
      
    ],
    declarations: [
        AccountComponent,
        GirisComponent,
       
    ],
    providers: [
        GirisService
    ],
    entryComponents: [
       
    ]
})
export class AccountModule {

}
