import { Component } from '@angular/core';
import { DatePipe } from '@angular/common';
import { User } from '../../Models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar-dso',
  templateUrl: './sidebar-dso.component.html',
  styleUrls: ['./sidebar-dso.component.css']
})
export class SidebarDsoComponent {
  url = 'Overview';
  menu = [
    'Overview',
    'Map',
    'Users',
    'Consumption'
  ];
  currentTime;
  user = new User();
  constructor (public datepipe: DatePipe, private route: Router){
    let currentDateTime = this.datepipe.transform((new Date), 'h:mm dd/MM/yyyy');
    this.currentTime = currentDateTime;
    this.url = this.route.url.split('/')[2];
  }
  
  select(element: EventTarget | null) {
    if(element == null) 
      return;
    let active = document.querySelector(".sidebar-dso-item-active") as HTMLDivElement;

    if(active!=null) {
      active.className = "sidebar-dso-item";
    }

    (element as HTMLDivElement).className = "sidebar-dso-item-active";
    this.route.navigateByUrl(`/dso/${(element as HTMLDivElement).innerHTML.toLowerCase()}`);
  }
  

  clickHandler(event: MouseEvent) {
    this.select(event.target);
  }
}
