import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
    //  { path: '', redirectTo: '../src/account/account.module', pathMatch: 'full' }
    //  ,
     {
         path: '',
         loadChildren: () => import('src/account/account.module').then(m => m.AccountModule), // Lazy load account module
         data: { preload: true }
     },
     {
         path: 'app',
         loadChildren: () => import('src/app/app.module').then(m => m.AppModule), // Lazy load account module
         data: { preload: true }
     }
    //  ,
    //  {
    //     path: 'islemler',
    //     loadChildren: () => import('src/app/components/islem/islem.component').then(m => m.IslemComponent), // Lazy load account module
    //     data: { preload: true }
    // }
    
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class RootRoutingModule { }
