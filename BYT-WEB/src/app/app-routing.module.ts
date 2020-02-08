import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { IslemComponent } from './components/islem/islem.component';


const routes: Routes = [
  {
    path:"about",component:AboutComponent
  }
  ,
  {
    path:"",component:IslemComponent
    //TODO: login sayfası yapılınca login sayfasına yönlenecek
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
