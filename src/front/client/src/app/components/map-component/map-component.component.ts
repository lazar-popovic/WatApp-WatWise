import {Component, OnInit} from '@angular/core';
import * as L from 'leaflet';
import {LocationService} from "../../services/location.service";

@Component({
  selector: 'app-map-component',
  templateUrl: './map-component.component.html',
  styleUrls: ['./map-component.component.css']
})
export class MapComponentComponent implements OnInit {

  private map: any;

  locations: any[] = [];
  users: any[] = [];
  showOverlay = false;
  constructor( private locationService: LocationService) { }
  ngOnInit(): void {
    this.map = L.map('map').setView([44.0128, 20.9114], 14);

    const latLng = L.latLng(44.0128, 20.9114);
    const point = this.map.latLngToContainerPoint(latLng);
    const z = this.map.getZoom();
    const x = Math.floor(point.x / 256);
    const y = Math.floor(point.y / 256);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
      maxZoom: 18
    }).addTo( this.map);
    console.log(`z: ${z}, x: ${x}, y: ${y}`);

    this.locationService.getLocations().subscribe(
      ( result: any) => {
        if( result.success)
        {
          this.locations = result.data;
          console.log( this.locations);
          this.placeMarkers();
        }
        else
        {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }

  placeMarkers() {
    for (const location of this.locations) {
      const marker = L.marker([location.latitude, location.longitude])
        .bindPopup(`<strong>Address:</strong> ${location.address} ${location.addressNumber}, ${location.city}`)
        .addTo(this.map);

      marker.on('click', () => {
        this.showOverlay = true;
        this.getUsersForLocation(location.id);
      });
    }
  }

  getUsersForLocation(locationId: number) {
    this.locationService.getUsersForLocationId( locationId).subscribe(
      ( result: any) => {
        if( result.success) {
          this.users = result.data;
          console.log( this.users);
        }
        else {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }

  closeOverlay(): void {
    this.showOverlay = false;
  }
}
