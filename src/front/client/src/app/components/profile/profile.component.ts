import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/Models/User';
import { environment } from 'src/app/environments/environment';
import { AuthService } from 'src/app/services/auth-service.service';
import { JWTService } from 'src/app/services/jwt.service';
import { UserService } from 'src/app/services/user.service';
import * as L from 'leaflet';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  user = new User();
  id: number = this.jwtService.userId;
  constructor(private authService:AuthService,
              private userService: UserService,
              private route: ActivatedRoute,
              private router: Router,
              private jwtService: JWTService) {
    this.getUser();
  }

  getUser() {
    this.userService.getUser(this.authService.userId).subscribe((result: any) => {
      if(result.errors.length > 0) {
        this.router.navigateByUrl("profile");
      } else {
        this.user.id = result.data.id;
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
          this.user.latitude = result.data.location.latitude;
          this.user.longitude = result.data.location.longitude;
        }

        this.createMap();
      }
    });
  }

  private map: any;
  createMap() : void {
    console.log( this.user);
    this.map = L.map('map', { dragging: false }).setView([this.user.latitude!, this.user.longitude!], 14);

    const latLng = L.latLng(this.user.latitude!, this.user.longitude!);
    console.log( latLng);
    const point = this.map.latLngToContainerPoint(latLng);
    console.log( point);
    const z = this.map.getZoom();
    const x = Math.floor(point.x / 256);
    const y = Math.floor(point.y / 256);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
      maxZoom: 18
    }).addTo( this.map);
    console.log(`z: ${z}, x: ${x}, y: ${y}`);

    let icon = L.icon({
      iconUrl: 'assets/pins/pin.png',
      iconSize: [26, 40],
      iconAnchor: [16, 32],
      popupAnchor: [0, -32]
    })

    const marker = L.marker([this.user.latitude!, this.user.longitude!], { icon: icon })
    .bindPopup(`<strong>Address:</strong> ${this.user.address} ${this.user.num}`)
    .addTo(this.map);
  }
}
