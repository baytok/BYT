import { BrowserModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector, APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { PlatformLocation, registerLocaleData } from '@angular/common';
import { SharedModule } from '../../src/shared/shared.module';
import { ServiceProxyModule } from '../../src/shared/service-proxies/service-proxy.module';
import { RootRoutingModule } from './root-routing.module';
import { AppConsts } from '../../src/shared/AppConsts';
import { AppSessionService } from '../../src/shared/session/app-session.service';
import { API_BASE_URL } from '../../src/shared/service-proxies/service-proxies';
import { RootComponent } from './root.component';
import { AppModule } from '../app/app.module';
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
        SharedModule.forRoot(),
        ServiceProxyModule,
        RootRoutingModule,
        HttpClientModule,
        AppModule,
      
    ],
    declarations: [
        RootComponent
    ],
    providers: [
      
        { provide: API_BASE_URL, useFactory: getRemoteServiceBaseUrl },
      
    ],
    bootstrap: [RootComponent]
})

export class RootModule {

}

export function getBaseHref(platformLocation: PlatformLocation): string {
    const baseUrl = platformLocation.getBaseHrefFromDOM();
    if (baseUrl) {
        return baseUrl;
    }

    return '/';
}

function getDocumentOrigin() {
    if (!document.location.origin) {
        const port = document.location.port ? ':' + document.location.port : '';
        return document.location.protocol + '//' + document.location.hostname + port;
    }

    return document.location.origin;
}
