import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AboutComponent } from './components/about/about.component';
import { NaviComponent } from './components/layout/navi/navi.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { GenelComponent } from './components/genel/genel.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { BeyannameSonucservisComponent } from './components/beyannamesonucservis/beyannamesonucservis.component';
import { FormsModule } from '@angular/forms';
import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { SharedModule } from '../shared/shared.module';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AppConsts } from '../shared/AppConsts';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { AppSessionService } from 'src/shared/session/app-session.service';


@NgModule({
   declarations: [
      AppComponent,
      AboutComponent,
      NaviComponent,
      HeaderComponent,
      FooterComponent,
      IslemComponent,
      BeyannameComponent,
      BeyannameSonucservisComponent,      
      GenelComponent,
   
    
   ],
   imports: [
      SharedModule.forRoot(),
      CommonModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ServiceProxyModule,      
         NgxLoadingModule.forRoot({
         animationType: ngxLoadingAnimationTypes.threeBounce,
         backdropBackgroundColour: 'rgba(0,2,0,0.2)', 
         backdropBorderRadius: '10px',
         primaryColour: '#8883e6', 
         secondaryColour: '#8883e6', 
         tertiaryColour: '#8883e6'
         
     })
    
   ],
   entryComponents: [
      BeyannameSonucservisComponent
   ],
   providers: [
      { provide: API_BASE_URL, useValue: "https://localhost:44345/api/BYT/" },
       AppSessionService,
   ],
    bootstrap: [
        AppComponent
   ]
})

export class AppModule { }
export function getRemoteServiceBaseUrl(): string {
 return AppConsts.remoteServiceBaseUrl;
  
}
