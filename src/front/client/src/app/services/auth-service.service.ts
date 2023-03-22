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

  login(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/login`, data, {observe: 'response'});
  }

  verify(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/verify-account`, data, {observe : 'response'});
  }

  verifyToken(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/verify-token`, data, {observe: 'response'});
  }

  forgotPassword(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/forgot-password`, data, { observe: 'response'});
  }

  resetPassword(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/reset-password`, data, {observe : 'response'});
  }
}
