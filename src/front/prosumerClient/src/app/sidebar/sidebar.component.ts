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
  

}
