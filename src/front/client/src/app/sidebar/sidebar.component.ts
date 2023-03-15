import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { User } from '../Models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  url = 'Overview';
  menu = [
    'Overview',
    'Devices',
    'Consumption',
    'Production'
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
    let active = document.querySelector(".sidebar-item-active") as HTMLDivElement;
    
    if(active!=null) {
      active.className = "sidebar-item";
    }
    
    (element as HTMLDivElement).className = "sidebar-item-active";
    this.route.navigateByUrl(`/prosumer/${(element as HTMLDivElement).innerHTML.toLowerCase()}`);
  }
  

  clickHandler(event: MouseEvent) {
    this.select(event.target);
  }
}
