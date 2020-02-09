import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { IslemComponent } from './components/islem/islem.component';
import { GirisComponent } from '../account/giris/giris.component';

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
    
  }/*
  ,
  {
    path:"**",component:NotFoundComponent
  }
*/
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
