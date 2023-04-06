import {Component, OnInit} from '@angular/core';
import {DeviceDataService} from "../../services/device-data.service";
import {DatePipe} from "@angular/common";
import {Chart} from "chart.js";

@Component({
  selector: 'app-energy-usage',
  templateUrl: './energy-usage.component.html',
  styleUrls: ['./energy-usage.component.css']
})
export class EnergyUsageComponent implements OnInit {

  constructor( private deviceDataService: DeviceDataService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    this.historyClick();
  }

  historyflag : boolean = true;
  predictionFlag : boolean = false;
  todayFlag : boolean = true;
  monthFlag : boolean = false;
  yearFlag : boolean = false;
  result: any[] = [];
  data: any[] = [];

  dataConsumption: any[] = [];
  dataProduction: any[] = [];

  historyClick(){
    this.historyflag = true;
    var historyDiv = document.getElementById("history-h3");
    if(historyDiv)  { historyDiv.style.color = "black"; }

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
    if(predictionDiv)  { predictionDiv.style.color = "black";}
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

    this.deviceDataService.getDSOSharedDataForDate().subscribe(
      (result:any) => {
        if( result.success) {
          this.dataProduction = [];
          this.dataConsumption = [];
          if( this.chart)
            this.chart.destroy();
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
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

    this.deviceDataService.getDSOSharedDataForMonth().subscribe(
      (result:any) => {
        if( result.success) {
          this.dataProduction = [];
          this.dataConsumption = [];
          if( this.chart)
            this.chart.destroy();
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
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

    this.deviceDataService.getDSOSharedDataForYear().subscribe(
      (result:any) => {
        if( result.success) {
          this.dataConsumption = [];
          this.dataProduction = [];
          if( this.chart)
            this.chart.destroy();
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
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
    console.log( this.dataConsumption, this.dataProduction);
    const canvas: any = document.getElementById("chart-canvas");
    const chart2d = canvas.getContext("2d");
    if( this.chart) {
      console.log("unistavam");
      this.chart.destroy();
    }
    this.chart = new Chart(chart2d, {
      type: 'bar',
      data: {
        datasets: [{
          data: this.dataConsumption,
          label: 'Consumption [kWh]',
          backgroundColor: 'rgba(191, 65, 65, 1)',
          borderColor: 'rgba(191, 65, 65, 1)',
          borderWidth: 1
        },{
          data: this.dataProduction,
          label: 'Production [kWh]',
          backgroundColor: 'rgba(69, 94, 184, 1)',
          borderColor: 'rgba(69, 94, 184, 1)',
          borderWidth: 1
        }]
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
