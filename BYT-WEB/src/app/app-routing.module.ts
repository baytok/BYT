import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
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
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }