import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { IslemComponent } from './components/islem/islem.component';
import { GirisComponent } from '../account/giris/giris.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
const routes: Routes = [
  {
    path:"about",component:AboutComponent
  }
  ,
  {
    path:"",component:GirisComponent
    
  },
  {
    path:"Islemler",component:IslemComponent
    
  },
  {
    path:"Beyanname",component:BeyannameComponent
    
  }/*
  ,
  {
    path:"**",component:NotFoundComponent
  }
*/
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
