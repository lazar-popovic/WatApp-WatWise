import { Component } from '@angular/core';
import { User } from 'src/app/Models/User';
import { AuthService } from 'src/app/services/auth-service.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {

  showProfileForm = true;
  dsoShow = true;
  user = new User();
  constructor(private authService:AuthService, private userService: UserService) {
    this.getUser();
  }

  getUser() {
    this.userService.getUser(this.authService.userId).subscribe((result: any) => {
      console.log(result.data);
      this.user.firstName = result.data.firstname;
      this.user.lastName = result.data.lastname;
      this.user.mail = result.data.email;
      this.user.role = result.data.roleId;
      this.user.address = result.data.location;
    });
  }
}
