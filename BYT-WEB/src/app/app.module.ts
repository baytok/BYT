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
import { KalemComponent } from './components/kalem/kalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { YeniKullaniciComponent } from './components/kullanici/yeniKullanici/yeniKullanici.component';
import { DegistirKullaniciComponent } from './components/kullanici/degistirKullanici/degistirKullanici.component';
import { YeniMusteriComponent } from './components/musteri/yeniMusteri/yeniMusteri.component';
import { DegistirMusteriComponent } from './components/musteri/degistirMusteri/degistirMusteri.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { BeyannameSonucservisComponent } from './components/beyannamesonucservis/beyannamesonucservis.component';
import { FormsModule,ReactiveFormsModule  } from '@angular/forms';
import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { SharedModule } from '../shared/shared.module';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AppConsts } from '../shared/AppConsts';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {
   BeyannameServiceProxy,
   SessionServiceProxy
 } from "../shared/service-proxies/service-proxies";


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
      KalemComponent,
      KullanicilarComponent,
      MusterilerComponent,
      YeniKullaniciComponent,
      DegistirKullaniciComponent,
      YeniMusteriComponent,
      DegistirMusteriComponent
   
    
   ],
   imports: [
      SharedModule.forRoot(),
      CommonModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      NgbModule,
      ServiceProxyModule,   
      ReactiveFormsModule,
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
      BeyannameSonucservisComponent,DegistirKullaniciComponent,DegistirMusteriComponent
      

   ],
   providers: [
      { provide: API_BASE_URL, useValue: "https://localhost:44345/api/BYT/" },
       SessionServiceProxy
   ],
    bootstrap: [
        AppComponent
   ]
})

export class AppModule { }
export function getRemoteServiceBaseUrl(): string {
 return AppConsts.remoteServiceBaseUrl;
  
}
