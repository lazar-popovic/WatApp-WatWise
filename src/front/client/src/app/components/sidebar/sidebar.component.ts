import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { User } from '../../Models/User';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  url = 'Overview';
  menuProsumer = [
    'Overview',
    'Devices',
    'Consumption',
    'Production'
  ];
  menuEmployee = [
    'Overview',
    'Map',
    'Users',
    'Consumption'
  ];
  menuAdmin = [
    'Employees'
  ];
  currentTime;
  user = new User();
  role = 3;
  constructor (public datepipe: DatePipe, private route: Router, private authService: AuthService){
    let currentDateTime = this.datepipe.transform((new Date), 'h:mm dd/MM/yyyy');
    this.currentTime = currentDateTime;
    this.url = this.route.url.split('/')[2];
    this.role = authService.roleId;
  }

  select(element: EventTarget | null, role: number) {
    if(element == null)
      return;
    let active = document.querySelector(".sidebar-item-active") as HTMLDivElement;

    if(active!=null) {
      active.className = "sidebar-item";
    }

    (element as HTMLDivElement).className = "sidebar-item-active";
    if(role == 1)
      this.route.navigateByUrl(`/prosumer/${(element as HTMLDivElement).innerHTML.toLowerCase()}`);
    else if(role == 2)
      this.route.navigateByUrl(`/dso/${(element as HTMLDivElement).innerHTML.toLowerCase()}`);
  }

  logOut() {
    this.authService.logOut();
    this.route.navigateByUrl('/login');
  }

  clickHandler(event: MouseEvent, role: number) {
    this.select(event.target, role);
  }



  isMobile():boolean
  {
    if(window.innerWidth < 800){
      console.log("Telefon")
      return true;
    }
      console.log("Nije Telefon")
      return false;
  }


  menuOpen = false;
  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}


