import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DeviceSchedulerService {
  constructor( private http: HttpClient) { }

  insertDeviceJob( data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}scheduler/device-job`, data, {observe: 'response'});
  }
  getActiveJobForDeviceId( deviceId: number) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}scheduler/get-active-job-for-device-id?deviceId=${deviceId}`);
  }
  getJobsForUserId( userId: number, active: boolean) : Observable<any>
  {
    return this.http.get(`${environment.apiUrl}scheduler/get-all-jobs-for-user-id?userId=${userId}&active=${active}`);
  }
  removeJob( jobId: number) : Observable<any> {
    return this.http.post(`${environment.apiUrl}scheduler/cancel-job?jobId=1`, {}, {observe: 'response'});
  }
}
