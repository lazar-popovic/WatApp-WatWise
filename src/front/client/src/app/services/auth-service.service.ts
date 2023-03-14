import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';

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
}
