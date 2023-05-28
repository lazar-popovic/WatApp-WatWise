import { DatePipe } from '@angular/common';
import { Component, HostListener, Input } from '@angular/core';
import { User } from '../../Models/User';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';
import { environment } from 'src/app/environments/environment';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css','./sidebar.component.mobile.css']
})
export class SidebarComponent {
  url = 'Overview';
  menuProsumer = [
    'Overview',
    'Devices',
    'Consumption',
    'Production',
    'Scheduler'
  ];
  menuEmployee = [
    'Overview',
    'Map',
    'Prosumers',
    'Energy usage'
  ];
  menuAdmin = [
    'Employees'
  ];
  user = new User();
  currentTime;
  role = 3;
  constructor (public datepipe: DatePipe, private route: Router, private authService: AuthService, private userService: UserService){
    let currentDateTime = this.datepipe.transform((new Date), 'h:mm dd/MM/yyyy');
    this.currentTime = currentDateTime;
    this.url = this.route.url.split('/')[2];
    this.role = authService.roleId;
    this.getUser();

    let mobileLink = (document.querySelector('.link-' + this.url) as HTMLDivElement);
    console.log( mobileLink);
    if( mobileLink != null) {
      mobileLink.style.backgroundColor = '#3e3e3e';
    }
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
      this.route.navigateByUrl(`/dso/${(element as HTMLDivElement).innerHTML.toLowerCase().replace(" ","-")}`);
  }

  getUser() {
    this.userService.getUser(this.authService.userId).subscribe((result: any) => {
      if(result.errors.length > 0) {
        this.route.navigateByUrl("profile");
      } else {
        this.user.firstname = result.data.firstname;
        this.user.lastname = result.data.lastname;
        this.user.mail = result.data.email;
        this.user.roleId = result.data.roleId;
        this.user.role = result.data.role.roleName;
        this.user.profileImage = environment.pictureAppendix + result.data.profileImage;
        if(result.data.location != null) {
          this.user.address = result.data.location.address;
          this.user.num = result.data.location.addressNumber;
          this.user.city = result.data.location.city;
        }
      }
    });
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

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const targetElement = event.target as HTMLElement;
    const menuElement = document.querySelector('.menu-list');
    const burgerMenuElement = document.querySelector('.burger-menu');

    if (this.menuOpen == true && menuElement && !menuElement.contains(targetElement) && !burgerMenuElement?.contains(targetElement)) {
      this.toggleMenu();
    }
  }

  isThisPage( linkUrl: string) {
    if( this.url == linkUrl) {
      this.menuOpen = false;
    }
  }
}


