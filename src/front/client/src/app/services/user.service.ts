import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http:HttpClient) { }

  createUser(data: any) : Observable<any>
  {
    return this.http.post(`${environment.apiUrl}auth/register-user`,data, {observe: 'response'});
  }

  public getUser(id: string | null) : Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}user/${id}`);
  }
}
