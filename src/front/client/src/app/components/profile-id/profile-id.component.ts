import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/Models/User';
import { AuthService } from 'src/app/services/auth-service.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile-id',
  templateUrl: './profile-id.component.html',
  styleUrls: ['./profile-id.component.css']
})
export class ProfileIDComponent{
  user = new User();
  id : any = '' ;

  constructor(private authService:AuthService, private userService: UserService, private route: ActivatedRoute, private router: Router) {
    this.getUser(this.route.snapshot.paramMap.get('id'));
  }

  getUser(id: string | null) {
    this.userService.getUser(id).subscribe((result: any) => {
      if(result.errors.length > 0) {
        this.router.navigateByUrl("profile");
      } else {
        this.user.firstName = result.data.firstname;
        this.user.lastName = result.data.lastname;
        this.user.mail = result.data.email;
        this.user.roleId = result.data.roleId;
        this.user.role = result.data.role.roleName;
        if(result.data.location != null) {
          this.user.address = result.data.location.address;
          this.user.num = result.data.location.addressNumber;
          this.user.city = result.data.location.city;
        }
      }
    });
  }

}
