import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '../environments/environment';
import { User } from '../Models/User';
import { AuthService } from './auth-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http:HttpClient) { }

  createUser(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/register-user`, data, {observe: 'response'});
  }

  createEmployee(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/register-employee`, data, {observe: 'response'})
  }

  public getUser(id: string | null) : Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}user/${id}`);
  }

  public getProsumers(pageSize: number, pageNumber: number, name: string, address: string, order: string) : Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiUrl}user/get-prosumers-filtered?fullName=${name}&streetAddress=${address}&sortOrder=${order}&pageSize=${pageSize}&pageNumber=${pageNumber}`);
  }

  public getNumberOfProsumers(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}user/prosumers-number`, {observe: 'response'});
  }
  
  public getEmployees(name:string, sortOrder:string, pageSize: number, pageNumber: number) : Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiUrl}user/get-employees-filtered?fullName=${name}&pageSize=${pageSize}&pageNumber=${pageNumber}&sortOrder=${sortOrder}`);
  }

  public updatePassword(data: any, id: number) : Observable<any> {
    return this.http.patch<any>(`${environment.apiUrl}user/update-user-password?id=${id}`, data, {observe: 'response'});
  }

  public updateUser(data: any, id: number) : Observable<any> {
    return this.http.patch<any>(`${environment.apiUrl}user/update-user-name-email?id=${id}`, data, {observe: 'response'});
  }

  public uploadImage(data: any) : Observable<any>{ 
    return this.http.post<any>(`${environment.apiUrl}user/update-user-image`, data, { observe: 'response'})
  }

  public deleteUser(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrl}user/delete/${id}`, { observe: 'response'})
  }
}