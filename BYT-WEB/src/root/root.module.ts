import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector, APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { RootRoutingModule } from './root-routing.module';
import { RootComponent } from './root.component';
import { HttpClientModule } from '@angular/common/http';
import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { LoggedToken } from '../shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { GirisService } from 'src/account/giris/giris.service';


@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        RootRoutingModule,
        HttpClientModule,
 
      
    ],
    declarations: [
        RootComponent
    ],
    providers: [
        { provide: API_BASE_URL, useValue: "https://localhost:44345/api/BYT/" }, 
        { provide: LoggedToken, useValue: "" },
         GirisService
    ],
    bootstrap: [RootComponent]
})

export class RootModule {

}


