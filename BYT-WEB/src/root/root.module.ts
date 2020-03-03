import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector, APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { RootRoutingModule } from './root-routing.module';
import { RootComponent } from './root.component';
import { HttpClientModule } from '@angular/common/http';

import * as _ from 'lodash';


@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        RootRoutingModule,
        HttpClientModule,
 
      
    ],
    declarations: [
        RootComponent
    ],
    providers: [
          
    ],
    bootstrap: [RootComponent]
})

export class RootModule {

}


