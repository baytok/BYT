import { Component, OnInit } from '@angular/core';

export interface DialogData {
  id: string;
  adSoyad: string;

}
@Component({
  selector: 'app-degistirKullanici',
  templateUrl: './degistirKullanici.component.html',
  styleUrls: ['./degistirKullanici.component.css']
})
export class DegistirKullaniciComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
