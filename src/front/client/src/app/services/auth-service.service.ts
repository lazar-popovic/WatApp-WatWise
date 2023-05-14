import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';
import { enableDebugTools } from '@angular/platform-browser';
import { JWTService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient, private jwtService: JWTService) { }

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

  get isLogged() {
    if(this.jwtService.checkToken == false){
      return false;
    }
    if(this.jwtService.expiredStatus)
      return false;
    return true;
  }

  get roleId() {
    return this.jwtService.roleId;
  }

  get userId() {
    return this.jwtService.userId;
  }

  logOut() {
    localStorage.removeItem('token');
    localStorage.clear()
    this.jwtService.token = null;
  }
}
