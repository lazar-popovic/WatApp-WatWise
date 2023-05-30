import {Component, OnInit} from '@angular/core';
import {DeviceDataService} from "../../services/device-data.service";
import {DatePipe} from "@angular/common";
import {Chart} from "chart.js";
import { DateService } from 'src/app/services/date.service';

@Component({
  selector: 'app-energy-usage',
  templateUrl: './energy-usage.component.html',
  styleUrls: ['./energy-usage.component.css']
})
export class EnergyUsageComponent implements OnInit {

  constructor( private deviceDataService: DeviceDataService, private datePipe: DatePipe, private dateService: DateService) {
  }

  ngOnInit(): void {
    this.date  =  this.dateService.toDateString( new Date());
    let futureDate = (new Date()).setDate((new Date()).getDate() + 7)
    this.maxDate = this.dateService.toDateString( new Date(futureDate));
    this.historyClick();
  }
  tableTitle: string = "Timestamp";
  columns: any[] = [];
  columnLabels: any[] = [];
  historyflag : boolean = true;
  predictionFlag : boolean = false;

  todayFlag : boolean = true;
  monthFlag : boolean = false;
  yearFlag : boolean = false;

  tommorowFlag : boolean = false;
  threeDaysFlag : boolean = false;
  sevenDaysFlag : boolean = false;

  result: any[] = [];
  data = {
    consumingEnergyUsageByTimestamp:[] as any[],
    producingEnergyUsageByTimestamp:[] as any[]
  }
  datasets: any[] = [];

  dataConsumption: any[] = [];
  dataProduction: any[] = [];

  maxDate: any;
  date: any;
  month: number = 4;
  yearForMonth: number = 2023;
  year: number = 2023;

  historyClick(){
    this.columns = ['timestamp','predictedConsumption','consumption','predictedProduction','production'];
    this.columnLabels = ['Hour','Predicted Consumption [kWh]','Consumption [kWh]','Predicted Production [kWh]','Production [kWh]'];
    this.historyflag = true;
    var historyDiv = document.getElementById("history-h3");
    if(historyDiv)  { historyDiv.style.color = "#3e3e3e"; }

    this.predictionFlag = false;
    var predictionDiv = document.getElementById("prediction-h3");
    if(predictionDiv)  { predictionDiv.style.color = "gray";}

    this.todayClick();
  }

  predictionClick(){
    this.columns = ['timestamp','predictedConsumption','predictedProduction'];
    this.columnLabels = ['Hour','Predicted Consumption [kWh]','Predicted Production [kWh]'];
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
    this.tableTitle = "Hour";
    this.columnLabels[0] = "Hour";
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
    this.deviceDataService.getDSOSharedDataForDate( date.getDate(), date.getMonth()+1, date.getFullYear()).subscribe(
      (result:any) => {
        if( result.success) {
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.value}));
          let now = new Date();
          console.log(date.toDateString());
          console.log(now.toDateString());
          if( date.toDateString() == now.toDateString()) {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.consumingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                                                               .map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: 'Consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 1)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                                                               .map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: 'Production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 1)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            }];

          } else if ( date > now) {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            }];
          } else {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: 'Consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 1)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
              label: 'Production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 1)',
              borderColor: 'rgba(69, 94, 184, 1)',
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
    this.tableTitle = "Day";
    this.columnLabels[0] = "Day";
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

    this.deviceDataService.getDSOSharedDataForMonth( this.month, this.yearForMonth).subscribe(
      (result:any) => {
        if( result.success) {
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          let now = new Date();
          if( this.month == now.getMonth()+1) {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.consumingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp).getDate() <= new Date().getDate())
                                                               .map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: 'Consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 1)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp).getDate()  <= new Date().getDate() )
                                                               .map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: 'Production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 1)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            }];

          } else if (this.month > now.getMonth()+1) {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            }];
          } else {
            this.datasets = [{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 0.4)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: 'Consumption [kWh]',
              backgroundColor: 'rgba(191, 65, 65, 1)',
              borderColor: 'rgba(191, 65, 65, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 0.4)',
              borderColor: 'rgba(69, 94, 184, 1)',
              borderWidth: 2
            },{
              data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
              label: 'Production [kWh]',
              backgroundColor: 'rgba(69, 94, 184, 1)',
              borderColor: 'rgba(69, 94, 184, 1)',
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
    this.tableTitle = "Month";
    this.columnLabels[0] = "Month";
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

    this.deviceDataService.getDSOSharedDataForYear( this.year).subscribe(
      (result:any) => {
        if( result.success) {
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.datasets = [{
            data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
            label: 'Predicted consumption [kWh]',
            backgroundColor: 'rgba(191, 65, 65, 0.4)',
            borderColor: 'rgba(191, 65, 65, 1)',
            borderWidth: 2
          },{
            data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Consumption [kWh]',
            backgroundColor: 'rgba(191, 65, 65, 1)',
            borderColor: 'rgba(191, 65, 65, 1)',
            borderWidth: 1
          },{
            data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
            label: 'Predicted production [kWh]',
            backgroundColor: 'rgba(69, 94, 184, 0.4)',
            borderColor: 'rgba(69, 94, 184, 1)',
            borderWidth: 2
          },{
            data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Production [kWh]',
            backgroundColor: 'rgba(69, 94, 184, 1)',
            borderColor: 'rgba(69, 94, 184, 1)',
            borderWidth: 1
          }];
          this.createBarChart();
        }
      }, error => {
        console.log( error)
      }
    );
  }

  tommorowClick()
  {
    this.tableTitle = "Hour";
    this.columnLabels[0] = "Hour";
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

    this.deviceDataService.getDSOPredictionForDays(1).subscribe(
      (result:any) => {
        if( result.success) {
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.value}));
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.datasets = [{
            data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
            label: 'Predicted consumption [kWh]',
            backgroundColor: 'rgba(191, 65, 65, 0.6)',
            borderColor: 'rgba(191, 65, 65, 1)',
            borderWidth: 2
          },{
            data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value})),
            label: 'Predicted production [kWh]',
            backgroundColor: 'rgba(69, 94, 184, 0.6)',
            borderColor: 'rgba(69, 94, 184, 1)',
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
    this.tableTitle = "Hour";
    this.columnLabels[0] = "Hour";
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

    this.deviceDataService.getDSOPredictionForDays(3).subscribe(
      (result:any) => {
        if( result.success) {
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.datasets = [{
            data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted consumption [kWh]',
            backgroundColor: 'rgba(191, 65, 65, 0.6)',
            borderColor: 'rgba(191, 65, 65, 1)',
            borderWidth: 2
          },{
            data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted production [kWh]',
            backgroundColor: 'rgba(69, 94, 184, 0.6)',
            borderColor: 'rgba(69, 94, 184, 1)',
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
    this.tableTitle = "Day";
    this.columnLabels[0] = "Day";
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

    this.deviceDataService.getDSOPredictionForDays(7).subscribe(
      (result:any) => {
        if( result.success) {
          this.dataConsumption = result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.dataProduction = result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value}));
          this.result = this.transformEnergyUsage( result.data.consumingEnergyUsageByTimestamp, result.data.producingEnergyUsageByTimestamp);
          this.datasets = [{
            data: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted consumption [kWh]',
            backgroundColor: 'rgba(191, 65, 65, 0.6)',
            borderColor: 'rgba(191, 65, 65, 1)',
            borderWidth: 2
          },{
            data: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
            label: 'Predicted production [kWh]',
            backgroundColor: 'rgba(69, 94, 184, 0.6)',
            borderColor: 'rgba(69, 94, 184, 1)',
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
            beginAtZero: false,
            title: {
              display: true,
              text: 'Electrical energy [kWh]',
              font: {
                size: 16,
                weight: 'bold',
              },
            },
          },
        }
      }
    });
  }

  transformEnergyUsage( consumingEnergyUsage: any[], producingEnergyUsage: any[]): any[] {
    console.log( consumingEnergyUsage);
    console.log( producingEnergyUsage);
    const transformedData: any[] = [];
    const consumingMap: Map<string, any> = new Map();
    for (let i = 0; i < consumingEnergyUsage.length; i++) {
      consumingMap.set(consumingEnergyUsage[i].timestamp, consumingEnergyUsage[i]);
    }
    const producingMap: Map<string, any> = new Map();
    for (let i = 0; i < producingEnergyUsage.length; i++) {
      producingMap.set(producingEnergyUsage[i].timestamp, producingEnergyUsage[i]);
    }
    const uniqueTimestamps: Set<string> = new Set([...consumingMap.keys(), ...producingMap.keys()]);
    for (const timestamp of uniqueTimestamps) {
      const transformedEntry: any = {
        timestamp,
        consumption: 0,
        predictedConsumption: 0,
        production: 0,
        predictedProduction: 0,
      };
      if (consumingMap.has(timestamp)) {
        const consumingData: any = consumingMap.get(timestamp);
        transformedEntry.consumption = consumingData.value;
        transformedEntry.predictedConsumption = consumingData.predictedValue;
      }
      if (producingMap.has(timestamp)) {
        const producingData: any = producingMap.get(timestamp);
        transformedEntry.production = producingData.value;
        transformedEntry.predictedProduction = producingData.predictedValue;
      }
      transformedData.push(transformedEntry);
    }
    return transformedData;
  }
}
