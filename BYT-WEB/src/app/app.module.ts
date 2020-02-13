import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AboutComponent } from './components/about/about.component';
import { NaviComponent } from './components/navi/navi.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { SonucservisComponent } from './components/sonucservis/sonucservis.component';
import { FormsModule } from '@angular/forms';
import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { SharedModule } from '../shared/shared.module';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AppConsts } from '../shared/AppConsts';
import { GirisComponent } from '../account/giris/giris.component';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { NgxSpinnerModule } from "ngx-spinner";


@NgModule({
   declarations: [
      AppComponent,
      AboutComponent,
      NaviComponent,
      HeaderComponent,
      FooterComponent,
      IslemComponent,
      BeyannameComponent,
      SonucservisComponent,
      GirisComponent
    
   ],
   imports: [
      SharedModule.forRoot(),
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      BrowserAnimationsModule,
      FormsModule,
      ServiceProxyModule,
      NgxSpinnerModule,
      NgxLoadingModule.forRoot({
         animationType: ngxLoadingAnimationTypes.threeBounce,
         backdropBackgroundColour: 'rgba(0,0,0,0.2)', 
         backdropBorderRadius: '10px',
         primaryColour: '#8883e6', 
         secondaryColour: '#8883e6', 
         tertiaryColour: '#8883e6'
     })
    
   ],
   entryComponents: [
      SonucservisComponent
   ],
   providers: [
      { provide: API_BASE_URL, useValue: "https://localhost:44345/api/BYT/" }
   ],
    bootstrap: [
       AppComponent
    ]
})

export class AppModule { }
export function getRemoteServiceBaseUrl(): string {
 return AppConsts.remoteServiceBaseUrl;
  
}
