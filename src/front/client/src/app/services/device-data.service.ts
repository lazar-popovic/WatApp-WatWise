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
  getUserTodayStats( day: number, month: number, year: number, userId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-day-total-for-user?day=${day}&month=${month}&year=${year}&userId=${userId}&`);
  }
}
