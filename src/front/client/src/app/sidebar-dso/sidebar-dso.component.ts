import { Component } from '@angular/core';
import { DatePipe } from '@angular/common';
import { User } from '../Models/User';


@Component({
  selector: 'app-sidebar-dso',
  templateUrl: './sidebar-dso.component.html',
  styleUrls: ['./sidebar-dso.component.css']
})
export class SidebarDsoComponent {

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
