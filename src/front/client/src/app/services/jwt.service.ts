import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JWTService {
  date: Date | undefined;
  token : string | null;
  constructor() { 
    this.token = localStorage.getItem("token");
  }

  get data()
  {
    return JSON.parse(atob(this.token!.split('.')[1]));
  }

  get roleId()
  {
    return this.data['RoleID'];
  }

  get userId()
  {
    return this.data['UserID'];
  }

  get expiredStatus()
  {
    this.date = new Date(0);
    this.date.setUTCSeconds(this.data['exp']);
    return this.date > (new Date());
  }
}
