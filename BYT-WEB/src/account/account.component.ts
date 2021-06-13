import { Component, ViewContainerRef, OnInit, ViewEncapsulation, Injector } from '@angular/core';
import { GirisService } from './giris/giris.service';


@Component({
    templateUrl: 'account.component.html',
    selector: 'account-root',
    encapsulation: ViewEncapsulation.None
})
export class AccountComponent  implements OnInit {

    versionText: string;
    currentYear: number;

    private viewContainerRef: ViewContainerRef;

    public constructor(
        injector: Injector,
        private _loginService: GirisService
    ) {
     
        this.currentYear = new Date().getFullYear();
      
    }

   

    ngOnInit(): void {
  
    }
}
