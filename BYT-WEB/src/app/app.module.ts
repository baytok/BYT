import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AboutComponent } from './components/about/about.component';
import { NaviComponent } from './components/navi/navi.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { IslemComponent } from './components/islem/islem.component';
import { TarihceComponent } from './components/tarihce/tarihce.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    NaviComponent,
    HeaderComponent,
    FooterComponent,
    IslemComponent,
    TarihceComponent,
    BeyannameComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule

  ],
  providers: [
    {provide:'apiUrl',useValue:'https://localhost:44345/'}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
