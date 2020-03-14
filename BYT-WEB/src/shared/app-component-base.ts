import { Injector, ElementRef } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { BeyannameServiceProxy ,SessionServiceProxy} from '../shared/service-proxies/service-proxies';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
   ServisDto
 } from '../shared/service-proxies/service-proxies';

export abstract class AppComponentBase  {
   beyanServis:BeyannameServiceProxy;
   session: SessionServiceProxy;
   snackBar: MatSnackBar;
   _dialog: MatDialog;
   
    constructor(
      injector: Injector,
   
     
        ) {
     
    }
  
    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
          duration: 2000,
        });
      }
    isGranted(permissionName: string): boolean {
        return true;
    }
}
