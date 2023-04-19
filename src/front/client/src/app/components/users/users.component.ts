import { Component } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import {ToastrNotifService} from "../../services/toastr-notif.service";
import { GeocodingService } from 'src/app/services/geocoding.service';
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

  constructor(private userService: UserService,private router: Router, private geoService: GeocodingService ,private toastrNotifService: ToastrNotifService) { }

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

  getAddress() {
    this.geoService.getAddress(this.user.location.address, this.user.location.number).subscribe((result: any) => {
      console.log(result.body);
    }, (error: any) => {
      console.log(error);
    })
  }
}
