import { Component } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import {ToastrNotifService} from "../../services/toastr-notif.service";
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
  user: any = {
    email: '',
    firstname: '',
    lastname: '',
    location: {
      address: '',
      city: '',
      number: ''
    }
  };

  constructor(private userService: UserService,private router: Router, private toastrNotifService: ToastrNotifService) { }

  storeUser()
  {
    this.userService.createUser(this.user).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl('/dso/users');
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    },(error: any) => {
      console.log(error);
    });
  }
}
