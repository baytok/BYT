import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { KalemComponent } from './components/kalem/kalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { GenelComponent } from './components/genel/genel.component';
import { AppComponent } from './app.component';
import { AuthGuard } from '../account/giris/giris.guard';

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
              { path: 'kullanicilar', component: KullanicilarComponent,canActivate : [AuthGuard] },      
              { path: 'musteriler', component: MusterilerComponent,canActivate : [AuthGuard] },             
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }