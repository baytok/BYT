import { BrowserModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector, APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { RootRoutingModule } from './root-routing.module';
import { AppConsts } from '../../src/shared/AppConsts';
import { API_BASE_URL } from '../../src/shared/service-proxies/service-proxies';
import { RootComponent } from './root.component';
import { HttpClientModule } from '@angular/common/http';
import * as _ from 'lodash';



export function convertAbpLocaleToAngularLocale(locale: string): string {
    if (!AppConsts.localeMappings) {
        return locale;
    }

    const localeMapings = _.filter(AppConsts.localeMappings, { from: locale });
    if (localeMapings && localeMapings.length) {
        return localeMapings[0]['to'];
    }

    return locale;
}


export function getRemoteServiceBaseUrl(): string {
    return AppConsts.remoteServiceBaseUrl;
}


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
      
    ],
    bootstrap: [RootComponent]
})

export class RootModule {

}


