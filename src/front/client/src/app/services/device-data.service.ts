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
  getDSOSharedDataForDate( day: number, month: number, year: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-today?day=${day}&month=${month}&year=${year}`);
  }
  getDSOSharedDataForMonth( month: number, year: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-month?month=${month}&year=${year}`);
  }
  getDSOSharedDataForYear( year: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-year?year=${year}`);
  }
  getDSOPredictionForDays( days: number) : Observable<any>
  {
    switch( days) {
      case 1: return this.http.get(`${environment.apiUrl}device-data/get-prediction-allowed-share-devices-data-for-tomorrow`);
      case 3: return this.http.get(`${environment.apiUrl}device-data/get-prediction-allowed-share-devices-data-for-next3days`);
      default: return this.http.get(`${environment.apiUrl}device-data/get-prediction-allowed-share-devices-data-for-next7days`);
    }
  }
}
