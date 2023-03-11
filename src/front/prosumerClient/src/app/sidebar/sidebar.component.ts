import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { User } from '../Models/User';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  currentTime;
  user = new User();
  constructor (public datepipe: DatePipe){
    let currentDateTime = this.datepipe.transform((new Date), 'h:mm dd/MM/yyyy');
    this.currentTime = currentDateTime;
  }
  
  select(element: EventTarget | null) {
    if(element == null) 
      return;
    let active = document.querySelector(".sidebar-item-active") as HTMLDivElement;

    if(active!=null) {
      active.className = "sidebar-item";
    }

    (element as HTMLDivElement).className = "sidebar-item-active";
  }
  

  clickHandler(event: MouseEvent) {
    this.select(event.target);
  }
}
