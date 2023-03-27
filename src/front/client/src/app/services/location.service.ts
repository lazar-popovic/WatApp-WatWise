import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {environment} from "../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor( private http: HttpClient) { }

  getLocations() : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}location/all-locations`);
  }

  getUsersForLocationId( locationId: number)
  {
    return this.http.get(`${environment.apiUrl}user/users-with-locationId?id=${locationId}`);
  }
}
