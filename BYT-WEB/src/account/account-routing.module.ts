import { NgModule } from '@angular/core';
import { Routes,RouterModule } from '@angular/router';
import { GirisComponent } from './giris/giris.component';
import { AccountComponent } from './account.component';
const routes: Routes = [
   
     { path: '', component: GirisComponent },
  ];
 
@NgModule({
    imports: [
      RouterModule.forRoot(routes)
    ],
    exports: [
        RouterModule
    ]
})
export class AccountRoutingModule { }
