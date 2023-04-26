import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {

  constructor(private http: HttpClient) { }

  getAddress(address: string) {
    return this.http.get(`${environment.apiUrl}location/address-autocomplete?streetAddress=${encodeURIComponent(address)}`, {observe: 'response'});
  }
}
