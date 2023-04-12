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

  getTop3ByCategory( userId: number) : Observable<any> {
  return this.http.get(`${environment.apiUrl}device/get-top-3-devices-by-user-id?userId=${userId}`);
  }

  getDeviceById( id: any) : Observable<any> {
    return this.http.get(`${environment.apiUrl}device/${id}`);
  }

  updateDevice(id: number, data: any) : Observable<any> {
    return this.http.patch(`${environment.apiUrl}device/${id}`, data, {observe: 'response'});
  }
}
