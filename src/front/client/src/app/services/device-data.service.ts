import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {environment} from "../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class DeviceDataService {
  constructor( private http: HttpClient) { }
  getDeviceDataForToday( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-data-for-today?deviceId=${deviceId}`);
  }
  getDeviceDataForMonth( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-data-for-month?deviceId=${deviceId}`);
  }
  getDeviceDataForYear( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-data-for-year?deviceId=${deviceId}`);
  }
  getUserTodayStats( userId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-today-total-for-user?userId=${userId}`);
  }
}