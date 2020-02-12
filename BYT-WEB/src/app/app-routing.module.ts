import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { IslemComponent } from './components/islem/islem.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { GirisComponent } from '../account/giris/giris.component';

 const routes: Routes = [
   {
     path:"about",component:AboutComponent
   }
   ,
   {
     path:"islemler",component:IslemComponent
    
   },
   {
     path:"beyanname",component:BeyannameComponent
    
    }
   ,
      {
      path:'',component:GirisComponent
    
    }
   ,
    // { path: '', redirectTo: '../account/account.component', pathMatch: 'full' },
 ];

 @NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }