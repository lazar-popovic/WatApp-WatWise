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

  items : any;

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

  pickAddressHandler(event: any) {
    let element = event.target;
    this.user.location.city = (element as HTMLDivElement).innerText.split(", ")[0];
    (document.querySelector(".users-form-location") as HTMLInputElement).value = (element as HTMLDivElement).innerText;
  }

  getAddress() {
    this.geoService.getAddress(this.user.location.address, this.user.location.number).subscribe((result: any) => {
      (document.querySelector('#address-show') as HTMLDivElement).style.display = 'block';
      let city;
      this.items = result.body.map((item:any) => {
        if (item.address.city)
          city = item.address.city;
        else if (item.address.town)
          city = item.address.town;
        else
          city = item.address.suburb;
        item.city = city;
        return item;
      });

      (document.querySelector('.users-form-location') as HTMLDivElement).style.marginBottom = "0px";
    }, (error: any) => {
      console.log(error);
    })
  }
}
