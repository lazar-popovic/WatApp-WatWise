import {Component, OnInit} from '@angular/core';
import {DeviceDataService} from "../../services/device-data.service";
import {JWTService} from "../../services/jwt.service";
import {Chart} from "chart.js";
import {DatePipe} from "@angular/common";
@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.css']
})
export class OverviewComponent implements OnInit
{
  chart1: any;
  chart1Data = {
    labels: [] as any[],
    production: [] as any[],
    consumption: [] as any[],
    predictedConsumption: [] as any[],
    predictedProduction: [] as any[]
  }
  chart2: any;
  chart2Data = {
    labels: [] as any[],
    predictedConsumption: [] as any[],
    predictedProduction: [] as any[]
  }
  constructor( private deviceDataService: DeviceDataService, private jwtService: JWTService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    const date = new Date();
    console.log( date);
    this.deviceDataService.getUserDayStats(date.getDay(), date.getMonth(), date.getFullYear(), this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          const currentHour = new Date().getHours();
          this.chart1Data.labels = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          if( this.chart1Data.labels.length == 0) {
            this.chart1Data.labels = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          }
          //this.chart1Data.production = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          //this.chart1Data.consumption = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.production = result.data.producingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour <= currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.consumption = result.data.consumingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour <= currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.predictedProduction = result.data.producingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour > currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.predictedConsumption = result.data.consumingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour > currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);

          console.log( this.chart1Data);
          this.drawChart1();
        }
      }, error => {
        console.log( error);
      }
    );
    const date2 = new Date();
    date2.setDate( date2.getDate() + 1);
    console.log( date2);
    this.deviceDataService.getUserDayStats(date2.getDay(), date2.getMonth(), date2.getFullYear(), this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.chart2Data.labels = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          if( this.chart2Data.labels.length == 0) {
            this.chart2Data.labels = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          }
          this.chart2Data.predictedProduction = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          this.chart2Data.predictedConsumption = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);

          console.log( this.chart2Data);
          this.drawChart2();
        }
      }, error => {
        console.log( error);
      }
    );
  }

  drawChart1(): void {
    const currentHour = new Date().getHours();
    const canvas: any = document.getElementById("chart1");
    const chart2d = canvas.getContext("2d");
    if( this.chart1) {
      this.chart1.destroy();
    }
    this.chart1 = new Chart(chart2d, {
      type: 'line',
      data: {
        labels: this.chart1Data.labels.map(d=>this.datePipe.transform(d,"shortTime")),
        datasets: [{
          label: "Total consumption(kW)",
          data: this.chart1Data.consumption,
          pointBackgroundColor: 'rgba(255, 0, 0,1)',
          backgroundColor: 'rgba(255, 0, 0,1)',
          borderColor: 'rgba(255, 0, 0, 0.1)',
          tension: 0.2,
          pointStyle: 'circle'
        },{
          label: "Predicted consumption (kW)",
          data: this.chart1Data.predictedConsumption.splice(0, this.chart1Data.consumption.length),
          pointBackgroundColor: 'rgba(254, 0, 0, 0.5)',
          backgroundColor: 'rgba(254, 0, 0, 0.5)',
          borderColor: 'rgba(254, 0, 0, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'circle'
        },{
          label: "Total production(kW)",
          data: this.chart1Data.production,
          pointBackgroundColor: 'rgba(0, 0, 255, 1)',
          backgroundColor: 'rgba(0, 0, 255, 1)',
          borderColor: 'rgba(0, 0, 255, 0.1)',
          tension: 0.2,
          pointStyle: 'rectRounded'
        },{
          label: "Predicted production (kW)",
          data: this.chart1Data.predictedProduction.splice(0, this.chart1Data.production.length),
          pointBackgroundColor: 'rgba( 0, 0,254, 0.5)',
          backgroundColor: 'rgba( 0, 0,254, 0.5)',
          borderColor: 'rgba( 0, 0,254, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'rectRounded'
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

  drawChart2(): void {
    const currentHour = new Date().getHours();
    const canvas: any = document.getElementById("chart2");
    const chart2d = canvas.getContext("2d");
    if( this.chart2) {
      this.chart2.destroy();
    }
    this.chart2 = new Chart(chart2d, {
      type: 'line',
      data: {
        labels: this.chart2Data.labels.map(d=>this.datePipe.transform(d,"shortTime")),
        datasets: [{
          label: "Predicted consumption (kW)",
          data: this.chart2Data.predictedConsumption,
          pointBackgroundColor: 'rgba(254, 0, 0, 0.5)',
          backgroundColor: 'rgba(254, 0, 0, 0.5)',
          borderColor: 'rgba(254, 0, 0, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'circle'
        },{
          label: "Predicted production (kW)",
          data: this.chart2Data.predictedProduction,
          pointBackgroundColor: 'rgba( 0, 0,254, 0.5)',
          backgroundColor: 'rgba( 0, 0,254, 0.5)',
          borderColor: 'rgba( 0, 0,254, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'rectRounded'
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
