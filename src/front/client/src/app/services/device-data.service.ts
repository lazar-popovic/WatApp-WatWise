import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {environment} from "../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class DeviceDataService {
  constructor( private http: HttpClient) { }
  getDeviceDataForDate( day: number, month: number, year: number, deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-date?day=${day}&month=${month}&year=${year}&deviceId=${deviceId}`);
  }
  getDeviceDataForMonth( month: number, year: number, deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-month?month=${month}&year=${year}&deviceId=${deviceId}`);
  }
  getDeviceDataForYear( year: number, deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-year?year=${year}&deviceId=${deviceId}`);
  }
  getDeviceDataForNextNDays( deviceId: number, days: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}device-data/get-device-data-for-next-n-days?id=${deviceId}&numberOfDays=${days}`);
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
    return this.http.get(`${environment.apiUrl}device-data/get-allowed-share-devices-data-for-date?day=${day}&month=${month}&year=${year}`);
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
    return this.http.get(`${environment.apiUrl}device-data/get-devices-data-prediction-allowed-share-for-nextNdays?numberOfDays=${days}`);
  }
}
