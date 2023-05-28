import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/Models/User';
import { environment } from 'src/app/environments/environment';
import { AuthService } from 'src/app/services/auth-service.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  user = new User();
  constructor(private authService:AuthService, private userService: UserService, private route: ActivatedRoute, private router: Router) {
    this.getUser();
  }

  getUser() {
    this.userService.getUser(this.authService.userId).subscribe((result: any) => {
      if(result.errors.length > 0) {
        this.router.navigateByUrl("profile");
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
}
