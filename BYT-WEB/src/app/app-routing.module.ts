import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { KalemComponent } from './components/DetayliBeyan/kalem/kalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { YetkilerComponent } from './components/yetki/yetkiler.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/DetayliBeyan/beyanname/beyanname.component';
import { OzetbeyanComponent } from './components/OzetBeyan/ozetbeyan/ozetbeyan.component';
import { GenelComponent } from './components/genel/genel.component';
import { AppComponent } from './app.component';
import { AuthGuard } from '../account/giris/giris.guard';
import { KiymetComponent } from './components/DetayliBeyan/kiymet/kiymet.component';
import { DbTeminatComponent } from './components/DetayliBeyan/dbteminat/dbteminat.component';
import { FirmaComponent } from './components/DetayliBeyan/firma/firma.component';
import { DbOzetbeyanAcmaComponent } from './components/DetayliBeyan/dbozetbeyanacma/dbozetbeyanacma.component';
import { TasimaSenetComponent } from './components/OzetBeyan/tasimasenet/tasimasenet.component';
import { ObOzetBeyanAcmaComponent } from './components/OzetBeyan/obozetbeyanacma/obozetbeyanacma.component';
import { ObTeminatComponent } from './components/OzetBeyan/obteminat/obteminat.component';


import { TestComponent } from './components/test/test.component';

 @NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
          path: '',
          component: AppComponent,
          children: [
              { path: '', component: GenelComponent,canActivate : [AuthGuard] },         
              { path: 'islemler', component: IslemComponent, canActivate : [AuthGuard] }, 
              { path: 'kullanicilar', component: KullanicilarComponent,canActivate : [AuthGuard] },      
              { path: 'musteriler', component: MusterilerComponent,canActivate : [AuthGuard] }, 
              { path: 'yetkiler', component: YetkilerComponent,canActivate : [AuthGuard] },
              { path: 'genel', component: GenelComponent,canActivate : [AuthGuard] }, 
              { path: 'beyanname', component: BeyannameComponent,canActivate : [AuthGuard] },  
              { path: 'kalem', component: KalemComponent,canActivate : [AuthGuard] },      
              { path: 'kiymet', component: KiymetComponent,canActivate : [AuthGuard] },      
              { path: 'dbteminat', component: DbTeminatComponent,canActivate : [AuthGuard] },      
              { path: 'firma', component: FirmaComponent,canActivate : [AuthGuard] },
              { path: 'dbozetbeyanacma', component: DbOzetbeyanAcmaComponent,canActivate : [AuthGuard] },   
              { path: 'ozetbeyan', component: OzetbeyanComponent,canActivate : [AuthGuard] },      
              { path: 'tasimasenet', component: TasimaSenetComponent,canActivate : [AuthGuard] },    
              { path: 'obozetbeyanacma', component: ObOzetBeyanAcmaComponent,canActivate : [AuthGuard] },   
              { path: 'obteminat', component: ObTeminatComponent,canActivate : [AuthGuard] },    
              { path: 'test', component: TestComponent },    
                                   
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }