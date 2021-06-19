import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AboutComponent } from './components/about/about.component';
import { NaviComponent } from './components/layout/navi/navi.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { GenelComponent } from './components/genel/genel.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { UserAreaComponent } from './components/layout/user-area/user-area.component';
import { IslemComponent } from './components/islem/islem.component';
import { DbBeyannameComponent } from './components/DetayliBeyan/dbbeyanname/dbbeyanname.component';
import { DbKalemComponent } from './components/DetayliBeyan/dbkalem/dbkalem.component';
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
import { DbFirmaComponent } from './components/DetayliBeyan/dbfirma/dbfirma.component';
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
import { NctsBeyanComponent } from './components/Ncts/nctsbeyan/nctsbeyan.component';
import { NbAcmaComponent } from './components/Ncts/nbacma/nbacma.component';
import { NbKalemComponent } from './components/Ncts/nbkalem/nbkalem.component';
import { NbDetayComponent } from './components/Ncts/nbdetay/nbdetay.component';
import { IghbComponent } from './components/DetayliBeyan/ighb/ighb.component';
import { MesaiComponent } from './components/DetayliBeyan/mesai/mesai.component';
import { IghbSonucServisComponent } from './components/DetayliBeyan/ighbsonucservis/ighbsonucservis.component';
import { MesaiSonucServisComponent } from './components/DetayliBeyan/mesaisonucservis/mesaisonucservis.component';
import { NctsSonucServisComponent } from './components/Ncts/nctssonucservis/nctssonucservis.component';
import { DolasimComponent } from './components/DetayliBeyan/dolasim/dolasim.component';
import { FirmalarComponent } from './components/firma/firmalar/Firmalar.component';
import { YeniFirmaComponent } from './components/firma/yeniFirma/yeniFirma.component';
import { DegistirFirmaComponent } from './components/firma/degistirFirma/degistirFirma.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';



@NgModule({
   declarations: [
      AppComponent,
      AboutComponent,
      NaviComponent,
      HeaderComponent,
      FooterComponent,
      UserAreaComponent,
      IslemComponent,
      DbBeyannameComponent,
      BeyannameSonucServisComponent,    
      OzetBeyanSonucServisComponent,  
      NctsSonucServisComponent,
      MesaiSonucServisComponent,
      IghbSonucServisComponent,
      GenelComponent,
      DbKalemComponent,
      KullanicilarComponent,
      MusterilerComponent,
      FirmalarComponent,
      YeniFirmaComponent,
      DegistirFirmaComponent,
      YeniKullaniciComponent,
      DegistirKullaniciComponent,
      YeniMusteriComponent,
      DegistirMusteriComponent,
      YetkilerComponent,
      DegistirYetkiComponent,
      YeniYetkiComponent,
      KiymetComponent,
      DbTeminatComponent,
      DbFirmaComponent,
      DbOzetbeyanAcmaComponent,
      OzetbeyanComponent,
      TasimaSenetComponent,
      ObOzetBeyanAcmaComponent,
      ObTeminatComponent,
      TasiyiciFirmaComponent,
      NctsBeyanComponent,
      NbAcmaComponent,
      NbKalemComponent,
      NbDetayComponent,
      IghbComponent,
      MesaiComponent,
      DolasimComponent,
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
         
     }),
     
        
   ],
   entryComponents: [
      BeyannameSonucServisComponent,OzetBeyanSonucServisComponent,DegistirKullaniciComponent,DegistirMusteriComponent,AppConsts,
      IghbSonucServisComponent,MesaiSonucServisComponent,NctsSonucServisComponent
      

   ],
   providers: [    
      {provide:HTTP_INTERCEPTORS, useClass:AuthInterceptor,multi:true},
      SessionServiceProxy,AccountModule,ReferansService,
    
   ],
    bootstrap: [
        AppComponent,
   ]
})

export class AppModule { }
export function getRemoteServiceBaseUrl(): string {
 return AppConsts.remoteServiceBaseUrl;
  
}
