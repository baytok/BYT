import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { DbKalemComponent } from './components/DetayliBeyan/dbkalem/dbkalem.component';
import { KullanicilarComponent } from './components/kullanici/kullanicilar/kullanicilar.component';
import { MusterilerComponent } from './components/musteri/musteriler/musteriler.component';
import { YetkilerComponent } from './components/yetki/yetkiler.component';
import { IslemComponent } from './components/islem/islem.component';
import { DbBeyannameComponent } from './components/DetayliBeyan/dbbeyanname/dbbeyanname.component';
import { OzetbeyanComponent } from './components/OzetBeyan/ozetbeyan/ozetbeyan.component';
import { GenelComponent } from './components/genel/genel.component';
import { AppComponent } from './app.component';
import { AuthGuard } from '../account/giris/giris.guard';
import { KiymetComponent } from './components/DetayliBeyan/kiymet/kiymet.component';
import { DbTeminatComponent } from './components/DetayliBeyan/dbteminat/dbteminat.component';
import { DbFirmaComponent } from './components/DetayliBeyan/dbfirma/dbfirma.component';
import { DbOzetbeyanAcmaComponent } from './components/DetayliBeyan/dbozetbeyanacma/dbozetbeyanacma.component';
import { TasimaSenetComponent } from './components/OzetBeyan/tasimasenet/tasimasenet.component';
import { ObOzetBeyanAcmaComponent } from './components/OzetBeyan/obozetbeyanacma/obozetbeyanacma.component';
import { ObTeminatComponent } from './components/OzetBeyan/obteminat/obteminat.component';
import { TasiyiciFirmaComponent } from './components/OzetBeyan/tasiyicifirma/tasiyicifirma.component';
import { NctsBeyanComponent } from './components/Ncts/nctsbeyan/nctsbeyan.component';
import { NbAcmaComponent } from './components/Ncts/nbacma/nbacma.component';
import { NbKalemComponent } from './components/Ncts/nbkalem/nbkalem.component';
import { NbDetayComponent } from './components/Ncts/nbdetay/nbdetay.component';
import { TestComponent } from './components/test/test.component';
import { MesaiComponent } from './components/DetayliBeyan/mesai/mesai.component';
import { IghbComponent } from './components/DetayliBeyan/ighb/ighb.component';
import { DolasimComponent } from './components/DetayliBeyan/dolasim/dolasim.component';

 @NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
          path: '',
          component: AppComponent,
          children: [
              { path: '', component: GenelComponent,canActivate : [AuthGuard] },         
              { path: 'islemler', component: IslemComponent, canActivate : [AuthGuard] }, 
              { path: 'kullanicilar', component: KullanicilarComponent,canActivate : [AuthGuard] },      
              { path: 'musteriler', component: MusterilerComponent,canActivate : [AuthGuard] }, 
              { path: 'yetkiler', component: YetkilerComponent,canActivate : [AuthGuard] },
              { path: 'genel', component: GenelComponent,canActivate : [AuthGuard] }, 
              { path: 'dbbeyan', component: DbBeyannameComponent,canActivate : [AuthGuard] },  
              { path: 'dbkalem', component: DbKalemComponent,canActivate : [AuthGuard] },      
              { path: 'kiymet', component: KiymetComponent,canActivate : [AuthGuard] },      
              { path: 'dbteminat', component: DbTeminatComponent,canActivate : [AuthGuard] },      
              { path: 'dbfirma', component: DbFirmaComponent,canActivate : [AuthGuard] },
              { path: 'dbozetbeyanacma', component: DbOzetbeyanAcmaComponent,canActivate : [AuthGuard] },   
              { path: 'ozetbeyan', component: OzetbeyanComponent,canActivate : [AuthGuard] },      
              { path: 'tasimasenet', component: TasimaSenetComponent,canActivate : [AuthGuard] },    
              { path: 'obozetbeyanacma', component: ObOzetBeyanAcmaComponent,canActivate : [AuthGuard] },   
              { path: 'obteminat', component: ObTeminatComponent,canActivate : [AuthGuard] },    
              { path: 'tasiyicifirma', component: TasiyiciFirmaComponent,canActivate : [AuthGuard] },    
              { path: 'nctsbeyan', component: NctsBeyanComponent,canActivate : [AuthGuard] },       
              { path: 'nbdetay', component: NbDetayComponent,canActivate : [AuthGuard] },  
              { path: 'nbkalem', component: NbKalemComponent,canActivate : [AuthGuard] },    
              { path: 'nbacma', component: NbAcmaComponent,canActivate : [AuthGuard] },               
              { path: 'mesai', component: MesaiComponent,canActivate : [AuthGuard] },               
              { path: 'ighb', component: IghbComponent,canActivate : [AuthGuard] },       
              { path: 'dolasim', component: DolasimComponent,canActivate : [AuthGuard] },               
              { path: 'test', component: TestComponent },    
                                   
          ],
          
      }
  ]) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }