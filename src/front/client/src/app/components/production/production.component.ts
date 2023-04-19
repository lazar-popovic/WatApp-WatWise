import { Component, OnInit } from '@angular/core';
import {ChartData, ChartDataset, ChartOptions, registerables} from 'chart.js';
import { Input } from '@angular/core';


import { Chart } from 'chart.js';
import {AuthService} from "../../services/auth-service.service";
import {UserService} from "../../services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {DeviceService} from "../../services/device.service";
import {DeviceDataService} from "../../services/device-data.service";
import {DatePipe} from "@angular/common";
import { ViewEncapsulation } from '@angular/core';
import { JWTService } from 'src/app/services/jwt.service';

@Component({
  selector: 'app-production',
  templateUrl: './production.component.html',
  styleUrls: ['./production.component.css']
})
export class ProductionComponent
{
  categoryLabel: string = "Production [kWh]";
  color: any = 'rgba(69, 94, 184, 1)';
  predColor: any = 'rgba(69, 94, 184, 0.4)';
  id : any = '' ;
  result: any[] = [];
  data: any[] = [];

  showEdit: boolean = false;
  showDelete: boolean = false;

  date: any;
  month: number = 4;
  yearForMonth: number = 2023;
  year: number = 2023;

  constructor( private jwtService: JWTService, private datePipe: DatePipe, private authService:AuthService, private route: ActivatedRoute, private router: Router, private deviceDataService: DeviceDataService) {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    let now = new Date();
    this.date = now.getFullYear() + "-" + (now.getMonth()+1) +"-" + now.getDate();
    this.historyClick();
  }

  historyflag : boolean = true;
  predictionFlag : boolean = false;

  todayFlag : boolean = true;
  monthFlag : boolean = false;
  yearFlag : boolean = false;

  tommorowFlag : boolean = false;
  threeDaysFlag : boolean = false;
  sevenDaysFlag : boolean = false;

  datasets: any[] = [];

  dataConsumption: any[] = [];
  dataProduction: any[] = [];

  historyClick(){
    this.historyflag = true;
    var historyDiv = document.getElementById("history-h3");
    if(historyDiv)  { historyDiv.style.color = "#3e3e3e"; }

    this.predictionFlag = false;
    var predictionDiv = document.getElementById("prediction-h3");
    if(predictionDiv)  { predictionDiv.style.color = "gray";}

    this.todayClick();
  }

  predictionClick(){
    this.historyflag = false;
    var historyDiv = document.getElementById("history-h3");
    if(historyDiv)  { historyDiv.style.color = "gray"; }

    this.predictionFlag = true;
    var predictionDiv = document.getElementById("prediction-h3");
    if(predictionDiv)  { predictionDiv.style.color = "#3e3e3e";}

    this.tommorowClick();
  }

  todayClick()
  {
    this.todayFlag  = true; this.monthFlag  = false; this.yearFlag = false;
    var todayDiv = document.getElementById("today");
    if(todayDiv)
    {
      todayDiv.style.color = "white";
      todayDiv.style.backgroundColor =  "#3E3E3E";
      todayDiv.style.padding = "5px";
      todayDiv.style.borderRadius = "10px";
    }

    var monthDiv = document.getElementById("month");
    if(monthDiv){ monthDiv.style.backgroundColor = "transparent"; monthDiv.style.color = "#3E3E3E";}

    var yearDiv = document.getElementById("year");
    if(yearDiv){ yearDiv.style.backgroundColor = "transparent"; yearDiv.style.color = "#3E3E3E";}

    let date = new Date( this.date);
    console.log( date);
    console.log( date.getDate(), date.getMonth()+1, date.getFullYear());
    this.deviceDataService.getUsersHistoryUsageByCategoryForDate( date.getDate(), date.getMonth()+1, date.getFullYear(), 1, this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.data = result.data;
          let now = new Date();
          if( date.toDateString() == now.toDateString()) {
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.color,
              borderWidth: 2
            },{
              data: result.data.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                                                               .map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: this.categoryLabel,
              backgroundColor: this.color,
              borderColor: this.color,
              borderWidth: 2
            }];

          } else if ( date > now) {
            console.log("pred");
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.color,
              borderWidth: 2
            }];
          } else {
            console.log("hist");
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.color,
              borderWidth: 2
            },{
              data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: this.categoryLabel,
              backgroundColor: this.color,
              borderColor: this.color,
              borderWidth: 2
            }];
          }
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  monthClick()
  {
    this.todayFlag = false; this.monthFlag  = true; this.yearFlag = false;
    const monthDiv = document.getElementById("month");
    if(monthDiv)
    {
      monthDiv.style.color = "white";
      monthDiv.style.backgroundColor =  "#3E3E3E";
      monthDiv.style.padding = "5px";
      monthDiv.style.borderRadius = "10px";
    }

    const todayDiv = document.getElementById("today");
    if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#3E3E3E";}

    const yearDiv = document.getElementById("year");
    if(yearDiv){ yearDiv.style.backgroundColor = "transparent "; yearDiv.style.color="#3E3E3E";}

    this.deviceDataService.getUsersHistoryUsageByCategoryForMonth( this.month, this.yearForMonth, 1, this.jwtService.userId).subscribe(
      (result:any) => {
        console.log( result)
        if( result.success) {
          this.data = result.data;
          let now = new Date();
          if( this.month == now.getMonth()+1) {
            console.log( result.data);
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.color,
              borderWidth: 2
            },{
              data: result.data.filter((ceu:any) => new Date(ceu.timestamp).getDate() <= new Date().getDate())
                                                               .map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: this.categoryLabel,
              backgroundColor: this.color,
              borderColor: this.color,
              borderWidth: 2
            }];
            console.log( this.datasets);

          } else if ( this.month > now.getMonth()+1) {
            console.log("pred");
            this.datasets = [{
              data: result.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.color,
              borderColor: this.color,
              borderWidth: 2
            }];
          } else {
            console.log("hist");
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.color,
              borderWidth: 2
            },{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: this.categoryLabel,
              backgroundColor: this.color,
              borderColor: this.color,
              borderWidth: 2
            }];
          }
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  yearClick()
  {
    this.todayFlag = false; this.monthFlag  = false; this.yearFlag = true;
    var yearDiv = document.getElementById("year");
    if(yearDiv)
    {
      yearDiv.style.color = "white";
      yearDiv.style.backgroundColor =  "#3E3E3E";
      yearDiv.style.padding = "5px";
      yearDiv.style.borderRadius = "10px";
    }

    const todayDiv = document.getElementById("today");
    if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#3E3E3E";}

    const monthDiv = document.getElementById("month");
    if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="#3E3E3E";}

    this.deviceDataService.getUsersHistoryUsageByCategoryForYear( this.year, 1, this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.data = result.data;
          this.datasets = [{
            data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
            label: this.categoryLabel,
            backgroundColor: this.predColor,
            borderColor: this.color,
            borderWidth: 2
          },{
            data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: this.categoryLabel,
            backgroundColor: this.color,
            borderColor: this.color,
            borderWidth: 2
          }]
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  tommorowClick()
  {
    this.tommorowFlag = true;
    this.threeDaysFlag = false;
    this.sevenDaysFlag = false;
    var yearDiv = document.getElementById("day1");
    if(yearDiv)
    {
      yearDiv.style.color = "white";
      yearDiv.style.backgroundColor =  "#3E3E3E";
      yearDiv.style.padding = "5px";
      yearDiv.style.borderRadius = "10px";
    }

    const todayDiv = document.getElementById("day2");
    if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#3E3E3E";}

    const monthDiv = document.getElementById("day3");
    if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="#3E3E3E";}

    this.deviceDataService.getUsersPredictionUsageByCategoryForNDays( 1, 1, this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.data = result.data;
          this.datasets = [{
            data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.value})),
            label: 'Predicted ' + this.categoryLabel,
            backgroundColor: this.predColor,
            borderColor: this.color,
            borderWidth: 2
          }];
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  threeDaysClick()
  {
    this.tommorowFlag = false;
    this.threeDaysFlag = true;
    this.sevenDaysFlag = false;
    var yearDiv = document.getElementById("day2");
    if(yearDiv)
    {
      yearDiv.style.color = "white";
      yearDiv.style.backgroundColor =  "#3E3E3E";
      yearDiv.style.padding = "5px";
      yearDiv.style.borderRadius = "10px";
    }

    const todayDiv = document.getElementById("day1");
    if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#3E3E3E";}

    const monthDiv = document.getElementById("day3");
    if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="#3E3E3E";}

    this.deviceDataService.getUsersPredictionUsageByCategoryForNDays( 3, 1, this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.data = result.data;
          this.datasets = [{
            data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted ' + this.categoryLabel,
            backgroundColor: this.predColor,
            borderColor: this.color,
            borderWidth: 2
          }];
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  sevenDaysClick()
  {
    this.tommorowFlag = false;
    this.threeDaysFlag = false;
    this.sevenDaysFlag = true;
    var yearDiv = document.getElementById("day3");
    if(yearDiv)
    {
      yearDiv.style.color = "white";
      yearDiv.style.backgroundColor =  "#3E3E3E";
      yearDiv.style.padding = "5px";
      yearDiv.style.borderRadius = "10px";
    }

    const todayDiv = document.getElementById("day1");
    if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#3E3E3E";}

    const monthDiv = document.getElementById("day2");
    if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="#3E3E3E";}

    this.deviceDataService.getUsersPredictionUsageByCategoryForNDays( 7, 1, this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.data = result.data;
          this.datasets = [{
            data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted ' + this.categoryLabel,
            backgroundColor: this.predColor,
            borderColor: this.color,
            borderWidth: 2
          }];
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  chart: any;
  createBarChart()
  {
    const canvas: any = document.getElementById("chart-canvas");
    const chart2d = canvas.getContext("2d");
    if( this.chart) {
      this.chart.destroy();
    }
    this.chart = new Chart(chart2d, {
      type: 'bar',
      data: {
        datasets: this.datasets
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              callback: function(value, index, ticks) {
                return value+'kW';
              }
            }
          }
        }
      }
    });
  }
}
