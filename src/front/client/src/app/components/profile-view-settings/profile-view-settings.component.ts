import { Component, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/Models/User';
import { AuthService } from 'src/app/services/auth-service.service';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { environment } from 'src/app/environments/environment';
@Component({
  selector: 'app-profile-view-settings',
  templateUrl: './profile-view-settings.component.html',
  styleUrls: ['./profile-view-settings.component.css']
})
export class ProfileViewSettingsComponent {
  
  @Output() profileDataEvent = new EventEmitter<any>();
  
  data: any = {
    email: '',
    firstname: '',
    lastname: ''
  }
  user = new User();

  constructor(private userService: UserService, private authService: AuthService, private router: Router) { 
    this.getUser();
  }

  emitData()
  {
    this.profileDataEvent.emit(this.data);
  }

  showUpload() {
    (document.querySelector(".profile-overview-overlay") as HTMLDivElement).style.display = "block";
  }

  getUser() {
    this.userService.getUser(this.authService.userId).subscribe((result: any) => {
      if(result.errors.length > 0) {
        this.router.navigateByUrl("profile");
      } else {
        this.user.firstName = result.data.firstname;
        this.user.lastName = result.data.lastname;
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
}
