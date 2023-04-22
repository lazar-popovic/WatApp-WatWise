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
      (document.querySelector('#address-show') as HTMLDivElement).style.display = 'block';
      let content = '';
      for(let item of result.body) {
        let city;
        if (item.address.suburb == null)
          city = item.address.city;
        else
          city = item.address.suburb;
        content += "<div class='display-item'>"
                + item.addressDetails.split(', ')[0]
                + ", " 
                + city
                + ", " 
                + item.address.country 
                + "</div>";
      }
      (document.querySelector('#address-show') as HTMLDivElement).innerHTML = content;
      (document.querySelector('.users-form-location') as HTMLDivElement).style.marginBottom = "0px";
    }, (error: any) => {
      console.log(error);
    })
  }
}
