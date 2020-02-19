import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AppSessionService } from '../shared/session/app-session.service';
import { AccountComponent } from './account.component';
import { GirisComponent } from './giris/giris.component';
import { GirisService } from './giris/giris.service';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,     
        SharedModule,       
        AccountRoutingModule,
        
        
    ],
    declarations: [
        AccountComponent,
        GirisComponent,
       
    ],
    providers: [
        GirisService,AppSessionService
    ],
    entryComponents: [
       
    ],
     bootstrap: [
         AccountComponent
      ]
})
export class AccountModule {

}
