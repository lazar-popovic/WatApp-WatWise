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
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-today?deviceId=${deviceId}`);
  }
  getDeviceDataForMonth( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-month?deviceId=${deviceId}`);
  }
  getDeviceDataForYear( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-year?deviceId=${deviceId}`);
  }
  getUserDayStats( day: number, month: number, year: number, userId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-day-total-for-user?day=${day}&month=${month}&year=${year}&userId=${userId}`);
  }
  getDSOOverviewLiveData() : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-energy-usage-total-7-hours-for-all-devices`);
  }
  getDSOSharedDataForDate() : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-today`);
  }
  getDSOSharedDataForMonth() : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-month`);
  }
  getDSOSharedDataForYear() : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-year`);
  }
}
