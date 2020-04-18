import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { KalemComponent } from './components/kalem/kalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { YetkilerComponent } from './components/yetki/yetkiler/yetkiler.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { GenelComponent } from './components/genel/genel.component';
import { AppComponent } from './app.component';
import { AuthGuard } from '../account/giris/giris.guard';
import { KiymetComponent } from './components/kiymet/kiymet.component';
import { TeminatComponent } from './components/teminat/teminat.component';
import { FirmaComponent } from './components/firma/firma.component';
import { OzetbeyanacmaComponent } from './components/ozetbeyanacma/ozetbeyanacma.component';

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
              { path: 'beyanname', component: BeyannameComponent,canActivate : [AuthGuard] },   
              { path: 'genel', component: GenelComponent,canActivate : [AuthGuard] }, 
              { path: 'kalem', component: KalemComponent,canActivate : [AuthGuard] },      
              { path: 'kiymet', component: KiymetComponent,canActivate : [AuthGuard] },      
              { path: 'teminat', component: TeminatComponent,canActivate : [AuthGuard] },      
              { path: 'firma', component: FirmaComponent,canActivate : [AuthGuard] },
              { path: 'ozetbeyanacma', component: OzetbeyanacmaComponent,canActivate : [AuthGuard] },            
              { path: 'kullanicilar', component: KullanicilarComponent,canActivate : [AuthGuard] },      
              { path: 'musteriler', component: MusterilerComponent,canActivate : [AuthGuard] }, 
              { path: 'yetkiler', component: YetkilerComponent,canActivate : [AuthGuard] },                         
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }