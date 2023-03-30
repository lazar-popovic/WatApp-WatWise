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
  constructor( private deviceDataService: DeviceDataService, private jwtService: JWTService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    this.deviceDataService.getUserTodayStats( this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          const currentHour = new Date().getHours();
          this.chart1Data.labels = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          this.chart1Data.production = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.consumption = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          /*this.chart1Data.production = result.data.producingEnergyUsageByTimestamp.filter((entry:any) => {
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
          }).map( (d:any)=>d.totalEnergyUsage);*/
          this.drawChart1();
        }
      }, error => {
        console.log( error);
      }
    )
  }

  drawChart1(): void {
    const currentHour = new Date().getHours();
    const canvas: any = document.getElementById("chart");
    const chart2d = canvas.getContext("2d");
    if( this.chart1) {
      this.chart1.destroy();
    }
    this.chart1 = new Chart(chart2d, {
      type: 'line',
      data: {
        labels: this.chart1Data.labels.map(d=>this.datePipe.transform(d,"shortTime")),
        datasets: [{
          label: "Total consumption (kW)",
          data: this.chart1Data.consumption,
          backgroundColor: "red",
          fill: false
        },{
          label: "Total production (kW)",
          data: this.chart1Data.production,
          backgroundColor: "blue",
          fill: false
        }/*,{
          label: "Predicted production (kW)",
          data: this.chart1Data.predictedProduction,
          backgroundColor: "blue",
          fill: false
        },{
          label: "Predicted consumption (kW)",
          data: this.chart1Data.predictedConsumption,
          backgroundColor: "blue",
          fill: false
        }*/]
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
