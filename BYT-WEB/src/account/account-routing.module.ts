import { NgModule } from '@angular/core';
import { Routes,RouterModule } from '@angular/router';
import { GirisComponent } from './giris/giris.component';



@NgModule({
    
  imports: [
    RouterModule.forChild([
        {
            
            path: '',
            component: GirisComponent,
            children: [
                { path: 'giris', component: GirisComponent},
               
             
            ]
        },
       
    ])
],
exports: [RouterModule]
})
export class AccountRoutingModule { }
