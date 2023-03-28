import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MapHubService {
  private connection: signalR.HubConnection;
  private locationsSubject = new BehaviorSubject<any[]>([]);
  public locations$ = this.locationsSubject.asObservable();

  constructor(toastr: ToastrService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5226/hubs/mapHub')
      .withAutomaticReconnect()
      .build();


      this.connection.start().catch(error => console.log(error));

    this.connection.on('AddUsersLocation', (userId, latitude, longitude, address, addressNumber, city, locationId) => {


      const locations = this.locationsSubject.getValue();
      locations.push({ userId, latitude, longitude, address, addressNumber, city, locationId });
      this.locationsSubject.next(locations);
      toastr.info("Loaded up initial locations");
    });


    this.connection.on('UpdateUserLocation', (userId, latitude, longitude, address, addressNumber, city, locationId) => {
      const locations = this.locationsSubject.getValue();
      const index = locations.findIndex(l => l.userId === userId);
      if (index !== -1) {
        locations[index] = { userId, latitude, longitude, address, addressNumber, city, locationId };
        this.locationsSubject.next(locations);
      }
      toastr.info(userId + "has changed adress");
    });


    this.connection.on('GetInitialLocationsError', (error) => {

      console.log('Error getting initial locations:', error);
    });

    this.connection.on('UpdateLocationError', (error) => {
      console.log('Error updating location:', error);
    });

    //this.connection.start().catch(error => console.log('SignalR connection error:', error));
  }

  public updateLocation(userId: number, latitude: number, longitude: number, address: string, addressNumber: number, city: string, locationId: number) {
    this.connection.invoke('UpdateLocation', userId, latitude, longitude, address, addressNumber, city, locationId)
      .catch(error => console.log('Error updating location:', error));
  }

  public getInitialLocations() {
    this.connection.invoke('GetInitialLocations')
      .catch(error => console.log('Error getting initial locations:', error));
  }

  stopHubConnection()
  {
    this.connection.stop().catch(error => console.log(error));
  }

}
