import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css']
})
export class CRUDComponent {
    students: any[] = [];
    test: any;
    constructor(private http: HttpClient) { }

    getAllItems(){
      this.http.get<any>('/api/Student').subscribe(res => {
        this.test = res;
    })
  }
}
