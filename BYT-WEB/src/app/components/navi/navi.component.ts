import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navi',
  templateUrl: './navi.component.html',
  styleUrls: ['./navi.component.scss']
})


export class NaviComponent implements OnInit {

  menuItems: MenuItem[] = [
    new MenuItem('HomePage',  'home', '/app/home'),

    new MenuItem('About',  'info', '/app/about'),

    new MenuItem('Beyanlar',  'menu', '', [
        new MenuItem('Detaylı Beyan', '', '', [
            new MenuItem('Genel Bilgiler', '', 'genelbilgiler'),
            new MenuItem('Kalem', '',  'https://aspnetboilerplate.com/Templates?ref=abptmpl')
        ]),
    new MenuItem('İşlemler',  'business', 'islemler')
    ])

   ];

  constructor() { }
 
  ngOnInit() {
    
  }

}

export class MenuItem {
  name = '';
  icon = '';
  route = '';
  items: MenuItem[];

  constructor(name: string, icon: string, route: string, childItems: MenuItem[] = null) {
      this.name = name;
       this.icon = icon;
      this.route = route;

      if (childItems) {
          this.items = childItems;
      } else {
          this.items = [];
      }
  }
}
