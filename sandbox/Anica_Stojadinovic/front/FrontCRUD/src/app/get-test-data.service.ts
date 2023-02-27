import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GetTestDataService {

  test: any;
  constructor(private http:HttpClient) { }

  getTest(){
      return this.http.get<any>('api/Student');
  }

}
