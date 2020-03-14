import { Component, OnInit } from '@angular/core';

export interface DialogData {
  id: string;
  adSoyad: string;

}
@Component({
  selector: 'app-yeniKullanici',
  templateUrl: './yeniKullanici.component.html',
  styleUrls: ['./yeniKullanici.component.css']
})
export class YeniKullaniciComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
