import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DeviceService {

constructor( private http: HttpClient) { }
  getDeviceTypesByCategory( category: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device/get-types-by-category?category=${category}`);
  }
  insertDevice( data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}device/insert-device`, data, {observe: 'response'});
  }
  getDevicesByUserId(userId: number) : Observable<any> {
    return this.http.get(`${environment.apiUrl}device/get-devices-by-user-id?userId=${userId}`);
  }
}
