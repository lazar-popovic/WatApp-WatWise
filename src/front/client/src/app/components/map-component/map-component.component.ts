import {Component, OnInit} from '@angular/core';
import * as L from 'leaflet';
import {LocationService} from "../../services/location.service";
import { Router } from '@angular/router';

@Component({
  selector: 'app-map-component',
  templateUrl: './map-component.component.html',
  styleUrls: ['./map-component.component.css']
})
export class MapComponentComponent implements OnInit {

  private map: any;
  cities: any[] = [];
  neighborhoods: any[] = [];
  locations: any[] = [];
  selectedLocation: any;
  selectedCity: string = "All";
  selectedNeighborhood: string = "All";
  users: any[] = [];
  showOverlay = false;
  constructor( private locationService: LocationService, private router: Router) { }
  ngOnInit(): void {
    this.map = L.map('map').setView([44.0128, 20.9114], 14);

    this.getCities();

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
  }

  markers: any[] = [];

  placeMarkers() {
    const customIcon = L.icon({
      iconUrl: 'assets/pin.png',
      iconSize: [26, 40],
      iconAnchor: [16, 32],
      popupAnchor: [0, -32]
    });

    // First remove all existing markers from the map
    for (const marker of this.markers) {
      this.map.removeLayer(marker);
    }

    // Then add new markers to the map
    this.markers = []; // Initialize the markers array
    for (const location of this.locations) {
      const marker = L.marker([location.latitude, location.longitude], { icon: customIcon })
        .bindPopup(`<strong>Address:</strong> ${location.address} ${location.addressNumber}, ${location.city}`)
        .addTo(this.map);

      marker.on('click', () => {
        this.showOverlay = true;
        this.getUsersForLocation(location.id);
      });

      this.markers.push(marker); // Add the marker to the markers array
    }
  }

  getCities() {
    this.locationService.getCities( ).subscribe(
      ( result: any) => {
        if( result.success) {
          this.cities = result.data;
          this.selectedCity = this.cities[0];
          this.getNeighborhoods();
          console.log( result.data);
        }
        else {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }

  getNeighborhoods() {
    this.locationService.getNeighborhoodsForCity( this.selectedCity).subscribe(
      ( result: any) => {
        if( result.success) {
          this.neighborhoods = result.data;
          this.selectedNeighborhood = this.neighborhoods.length > 0 ? this.neighborhoods[0] : "All";
          this.getLocationsForNeighborhood();
          console.log( this.neighborhoods);
        }
        else {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }

  getLocationsForNeighborhood() {
    this.locationService.getLocationsForNeighborhood( this.selectedNeighborhood).subscribe(
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

  getUsersForLocation(locationId: number) {
    this.selectedLocation = this.locations.find( l => l.id == locationId)
    console.log( this.selectedLocation);
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

  goToUser( userId: number) {
    this.router.navigate(['/profile', userId]);
  }
}
