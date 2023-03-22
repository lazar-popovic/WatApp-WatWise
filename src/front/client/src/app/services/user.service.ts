import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient) { }

  createUser(data: any) : Observable<any>
  {
    return this.http.post(`${environment.apiUrl}auth/register-user`,data, {observe: 'response'});
  }
}
