import { Component, EventEmitter, Output } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import {ToastrNotifService} from "../../services/toastr-notif.service";
import { GeocodingService } from 'src/app/services/geocoding.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
  user: any = {
    email: '' as string,
    firstname: '' as string,
    lastname: '' as string,
    location: {
      address: '' as string,
      city: '' as string,
      number: '',
      neighborhood: '' as string
    }
  };

  searchAddress = "";
  busy: Subscription | undefined;
  items : any;
  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private userService: UserService,private router: Router, private geoService: GeocodingService ,private toastrNotifService: ToastrNotifService) { }

  storeUser()
  {
    console.log( this.user);
    this.busy = this.userService.createUser(this.user).subscribe((result: any) => {
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
    console.log( element);
    let street =  (element as HTMLDivElement).innerText.split(", ");
    this.user.location.address = street[0];
    this.user.location.neighborhood = street[1];
    this.user.location.city = street[2];
    console.log( this.user);
    (document.querySelector(".users-form-location") as HTMLInputElement).value = (element as HTMLDivElement).innerText;
  }

  getAddress() {
    if( this.searchAddress == "") {
      this.toastrNotifService.showErrors(["Enter address for search!"]);
    }
    else {
      this.geoService.getAddress(this.searchAddress).subscribe((result: any) => {
        (document.querySelector('#address-show') as HTMLDivElement).style.display = 'block';
        let city="";
        let neighbourhood="";
        let road="";
        this.items = result.body.map((item:any) => {

          if (item.address.city)
            city = item.address.city;
          else if (item.address.town)
            city = item.address.town;
          item.city = city;

          if (item.address.neighbourhood)
            neighbourhood = item.address.neighbourhood;
          else if (item.address.suburb)
            neighbourhood = item.address.suburb;
          else
            neighbourhood = item.address.city_district;

          item.city = city;
          item.neighborhood = neighbourhood;
          console.log( item);
          return item;
        });

        (document.querySelector('.users-form-location') as HTMLDivElement).style.marginBottom = "0px";
      }, (error: any) => {
        console.log(error);
      });
    }
  }

  refresh() : void {

  }
}
