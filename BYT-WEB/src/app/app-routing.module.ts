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

 @NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
          path: '',
          component: AppComponent,
          children: [
             { path: '', component: GenelComponent },         
              { path: 'islemler', component: IslemComponent },            
              { path: 'beyanname', component: BeyannameComponent },   
              { path: 'genel', component: GenelComponent }, 
              { path: 'kalem', component: KalemComponent },      
              { path: 'kullanicilar', component: KullanicilarComponent },      
              { path: 'musteriler', component: MusterilerComponent },             
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }