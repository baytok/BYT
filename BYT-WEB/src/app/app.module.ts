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
import { UserAreaComponent } from './components/layout/user-area/user-area.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/DetayliBeyan/beyanname/beyanname.component';
import { KalemComponent } from './components/DetayliBeyan/kalem/kalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { YeniKullaniciComponent } from './components/kullanici/yeniKullanici/yeniKullanici.component';
import { DegistirKullaniciComponent } from './components/kullanici/degistirKullanici/degistirKullanici.component';
import { YeniMusteriComponent } from './components/musteri/yeniMusteri/yeniMusteri.component';
import { DegistirMusteriComponent } from './components/musteri/degistirMusteri/degistirMusteri.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { YetkilerComponent } from './components/yetki/yetkiler.component';
import { DegistirYetkiComponent } from './components/yetki/degistirYetki/degistirYetki.component';
import { YeniYetkiComponent } from './components/yetki/yeniYetki/yeniYetki.component';
import { KiymetComponent } from './components/DetayliBeyan/kiymet/kiymet.component';
import { OzetbeyanComponent } from './components/OzetBeyan/ozetbeyan/ozetbeyan.component';
import { BeyannameSonucServisComponent } from './components/DetayliBeyan/beyannamesonucservis/beyannamesonucservis.component';
import { OzetBeyanSonucServisComponent } from './components/OzetBeyan/ozetbeyansonucservis/ozetbeyansonucservis.component';
import { FormsModule,ReactiveFormsModule  } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AppConsts } from '../shared/AppConsts';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import {AccountModule} from '../account/account.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {
    SessionServiceProxy
 } from "../shared/service-proxies/service-proxies";
import { DbTeminatComponent } from './components/DetayliBeyan/dbteminat/dbteminat.component';
import { FirmaComponent } from './components/DetayliBeyan/firma/firma.component';
import { DbOzetbeyanAcmaComponent } from './components/DetayliBeyan/dbozetbeyanacma/dbozetbeyanacma.component';
import { TasimaSenetComponent } from './components/OzetBeyan/tasimasenet/tasimasenet.component';
import { ObOzetBeyanAcmaComponent } from './components/OzetBeyan/obozetbeyanacma/obozetbeyanacma.component';
import { ObTeminatComponent } from './components/OzetBeyan/obteminat/obteminat.component';
import { TasiyiciFirmaComponent } from './components/OzetBeyan/tasiyicifirma/tasiyicifirma.component';
import {
   ReferansService
 } from "../shared/helpers/ReferansService";
import { TestComponent } from './components/test/test.component';
import {NgxChildProcessModule} from 'ngx-childprocess';

@NgModule({
   declarations: [
      AppComponent,
      AboutComponent,
      NaviComponent,
      HeaderComponent,
      FooterComponent,
      UserAreaComponent,
      IslemComponent,
      BeyannameComponent,
      BeyannameSonucServisComponent,    
      OzetBeyanSonucServisComponent,  
      GenelComponent,
      KalemComponent,
      KullanicilarComponent,
      MusterilerComponent,
      YeniKullaniciComponent,
      DegistirKullaniciComponent,
      YeniMusteriComponent,
      DegistirMusteriComponent,
      YetkilerComponent,
      DegistirYetkiComponent,
      YeniYetkiComponent,
      KiymetComponent,
      DbTeminatComponent,
      FirmaComponent,
      DbOzetbeyanAcmaComponent,
      OzetbeyanComponent,
      TasimaSenetComponent,
      ObOzetBeyanAcmaComponent,
      ObTeminatComponent,
      TasiyiciFirmaComponent,
      TestComponent
      
    
   ],
   imports: [
      SharedModule.forRoot(),
      CommonModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      NgbModule,
      NgxChildProcessModule,
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
      BeyannameSonucServisComponent,OzetBeyanSonucServisComponent,DegistirKullaniciComponent,DegistirMusteriComponent,AppConsts
      

   ],
   providers: [
      SessionServiceProxy,AccountModule,ReferansService
   ],
    bootstrap: [
        AppComponent,
   ]
})

export class AppModule { }
export function getRemoteServiceBaseUrl(): string {
 return AppConsts.remoteServiceBaseUrl;
  
}
