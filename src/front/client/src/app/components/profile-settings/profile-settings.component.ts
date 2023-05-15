import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth-service.service';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent {

  busy: Subscription | undefined;

  wanted: number = 1;

  constructor(private userService: UserService, private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  click(element: EventTarget | null) {
    if(element == null)
      return;

    let active = document.querySelector(".profile-settings-menu-active-item") as HTMLDivElement;

    if(active != null)
      active.className = "profile-settings-menu-item";

    if ((element as HTMLDivElement).className == "profile-settings-menu-item")
      (element as HTMLDivElement).className = "profile-settings-menu-active-item";
  }

  handler(event: MouseEvent) {
    this.click(event.target);
  }

  updatePassword(data: any) {
    this.busy = this.userService.updatePassword(data, this.authService.userId).subscribe((result: any) => {
      if(result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl('profile')
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    })
  }

  updateUser(data:any) {
    this.userService.updateUser(data, this.authService.userId).subscribe((result: any) => {
      if(result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl('profile')
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    })
  }
}
