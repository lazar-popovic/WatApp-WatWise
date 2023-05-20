import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateService {

constructor() { }
  toDateString( date: Date) : string {
    let newDate
    let month: any = (date.getMonth()+1);
    let day: any =  date.getDate();
    if (month<10) month = "0" + month;
    if (day<10) day = "0" + day;
    newDate  =  date.getFullYear() + "-" + month + "-" + day;
    return newDate;
  }
}
