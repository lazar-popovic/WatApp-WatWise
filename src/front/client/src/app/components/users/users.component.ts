import { Component } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
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
      address: 'bb',
      city: 'none',
      number: 0
    }
  };

  constructor(private userService: UserService,private router: Router) { }

  storeUser()
  {
    this.userService.createUser(this.user).subscribe((result: any) => {
      this.router.navigateByUrl('/dso/users');
    },(error: any) => {
      console.log(error);
    });
  }
}
