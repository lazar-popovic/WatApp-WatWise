import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';
import { enableDebugTools } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  loginProsumer(data : any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}prosumer/login`, data, {observe: 'response'});
  }

  loginDso(data : any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}dso/login`, data, {observe: 'response'});
  }

  verify(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/verify-account`, data, {observe : 'response'});
  }

  verifyToken(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/validate-token`, data, {observe: 'response'});
  }
}
