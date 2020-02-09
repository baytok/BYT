import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AboutComponent } from './components/about/about.component';
import { NaviComponent } from './components/navi/navi.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { IslemComponent } from './components/islem/islem.component';
import { GirisComponent } from '../account/giris/giris.component';
import { BeyannameComponent } from './components/beyanname/beyanname.component';
import { SonucservisComponent } from './components/sonucservis/sonucservis.component';
import { 
  MatButtonModule,
  MatCheckboxModule,
  MatInputModule,
  MatCardModule,
  MatIconModule,
  MatSnackBarModule,
  MatMenuModule,
  MatExpansionModule,
  MatDialogModule,
  MatTableModule,
  MatPaginatorModule 
} from '@angular/material';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    NaviComponent,
    HeaderComponent,
    FooterComponent,
    IslemComponent,
    BeyannameComponent,
    SonucservisComponent,
    GirisComponent
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatSnackBarModule,
    FormsModule,
    MatMenuModule,
    MatExpansionModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule 
    

  ],
  entryComponents: [
    SonucservisComponent
  ],
  providers: [
    {provide:'apiUrl',useValue:'https://localhost:44345/api/BYT/'}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
