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
    const customIcons = [
      L.icon({
        iconUrl: 'assets/pins/pin-2.png',
        iconSize: [26, 40],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      }),
      L.icon({
        iconUrl: 'assets/pins/pin-1.png',
        iconSize: [26, 40],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      }),
      L.icon({
        iconUrl: 'assets/pins/pin0.png',
        iconSize: [26, 40],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      }),
      L.icon({
        iconUrl: 'assets/pins/pin1.png',
        iconSize: [26, 40],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      }),
      L.icon({
        iconUrl: 'assets/pins/pin2.png',
        iconSize: [26, 40],
        iconAnchor: [16, 32],
        popupAnchor: [0, -32]
      })
    ];

    for (const marker of this.markers) {
      this.map.removeLayer(marker);
    }

    this.markers = [];
    for (const location of this.locations) {
      let icon = 0;
      console.log( location.totalPowerUsage );
      if( location.totalPowerUsage < -1.5 )
        icon = 0;
      else if( location.totalPowerUsage < -1.5 )
        icon = 1;
      else if( location.totalPowerUsage < -0.5 )
        icon = 2;
      else if( location.totalPowerUsage < 0.5)
        icon = 3;
      else if( location.totalPowerUsage > 1.5 )
        icon = 4;
      console.log( icon);
      const marker = L.marker([location.latitude, location.longitude], { icon: customIcons[ icon] })
        .bindPopup(`<strong>Address:</strong> ${location.address} ${location.addressNumber}<br><strong>Current power usage:</strong> ${location.totalPowerUsage.toFixed(3)}kWh`)
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
          this.selectedNeighborhood = "All";
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
    this.locationService.getLocationsForNeighborhood( this.selectedCity, this.selectedNeighborhood).subscribe(
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

  chartData = [
    {
      "name": "Centar",
      "series": [
        {
          "name": "Production",
          "value": 134
        },
        {
          "name": "Consumption",
          "value": 223
        }
      ]
    },{
      "name": "Stanovo",
      "series": [
        {
          "name": "Consumption",
          "value": 223
        },
        {
          "name": "Production",
          "value": 134
        }
      ]
    },{
      "name": "Palilule",
      "series": [
        {
          "name": "Consumption",
          "value": 223
        },
        {
          "name": "Production",
          "value": 134
        }
      ]
    },{
      "name": "Male pčelice",
      "series": [
        {
          "name": "Consumption",
          "value": 223
        },
        {
          "name": "Production",
          "value": 134
        }
      ]
    },
  ];
}
