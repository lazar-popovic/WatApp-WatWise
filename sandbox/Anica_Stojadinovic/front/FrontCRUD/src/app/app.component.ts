import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'MOJ FRONT MOTHERFUCKER!!!';
  dal_sam_normalan = false;
  test:any = String("NISTA");
  
  constructor(private http:HttpClient){}

  public pokreni(): void {
    const url: string = '/api/Student';

    this.http.get(url).subscribe((res)=>{
      debugger;
      this.test = res;
      console.log(res);
    });
    this.title = "SDASD";
  }
}
