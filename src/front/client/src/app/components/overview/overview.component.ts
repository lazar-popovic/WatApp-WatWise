import {Component, OnInit} from '@angular/core';
import {DeviceDataService} from "../../services/device-data.service";
import {JWTService} from "../../services/jwt.service";
import {Chart} from "chart.js";
import {DatePipe} from "@angular/common";
import {DeviceService} from "../../services/device.service";
import * as DataLabelsPlugin from 'chartjs-plugin-datalabels';

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


  chartActiveConsumers: any;
  chartActiveConsumersData = {
    labels: [] as any[],
    consumption: [] as any[]
  }
  chartActiveProducers: any;
  chartActiveProducersData = {
    labels: [] as any[],
    production: [] as any[]
  }
  constructor( private deviceDataService: DeviceDataService, private jwtService: JWTService, private datePipe: DatePipe, private deviceService: DeviceService) {
    Chart.register( DataLabelsPlugin);
  }

  ngOnInit(): void {
    const date = new Date();
    this.deviceDataService.getUserDayStats( date.getDate(), date.getMonth()+1, date.getFullYear(), this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          const currentHour = new Date().getHours();
          this.chart1Data.labels = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          if( this.chart1Data.labels.length == 0) {
            this.chart1Data.labels = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          }
          this.chart1Data.production = result.data.producingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour <= currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);
          this.chart1Data.consumption = result.data.consumingEnergyUsageByTimestamp.filter((entry:any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour <= currentHour;
          }).map( (d:any)=>d.totalEnergyUsage);

          const predictedProduction = result.data.producingEnergyUsageByTimestamp.filter((entry: any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour > currentHour;
          }).map((d: any) => d.totalEnergyUsage);

          const predictedConsumption = result.data.consumingEnergyUsageByTimestamp.filter((entry: any) => {
            const hour = new Date(entry.timestamp).getHours();
            return hour > currentHour;
          }).map((d: any) => d.totalEnergyUsage);

          const nullArray1 = Array(this.chart1Data.production.length).fill(null);
          const nullArray2 = Array(this.chart1Data.consumption.length).fill(null);

          this.chart1Data.predictedProduction = nullArray1.concat(predictedProduction);
          this.chart1Data.predictedConsumption = nullArray2.concat(predictedConsumption);

          this.drawChart1();
        }
      }, error => {
        console.log( error);
      }
    );

    const date2 = new Date();
    date2.setDate( date2.getDate() + 1);
    this.deviceDataService.getUserDayStats( date2.getDate(), date2.getMonth()+1, date2.getFullYear(), this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.chart2Data.labels = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          if( this.chart2Data.labels.length == 0) {
            this.chart2Data.labels = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.timestamp);
          }
          this.chart2Data.predictedProduction = result.data.producingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);
          this.chart2Data.predictedConsumption = result.data.consumingEnergyUsageByTimestamp.map( (d:any)=>d.totalEnergyUsage);

          this.drawChart2();
        }
      }, error => {
        console.log( error);
      }
    );

    this.deviceService.getTop3ByCategory( this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          let consumers = result.data.find( (c:any) => c.category == -1);
          if( consumers != null) {
            this.chartActiveConsumersData.labels = consumers.devices.map( (device:any) => device.name);
            this.chartActiveConsumersData.consumption = consumers.devices.map( (device:any) => device.value);
          } else {
            this.chartActiveConsumersData.consumption = [];
            this.chartActiveConsumersData.labels = [];
          }

          let producers = result.data.find( (c:any) => c.category == 1);
          if( producers != null) {
            this.chartActiveProducersData.labels = producers.devices.map( (device:any) => device.name);
            this.chartActiveProducersData.production = producers.devices.map( (device:any) => device.value);
          } else {
            this.chartActiveProducersData.production = [];
            this.chartActiveProducersData.labels = [];
          }

          console.log( this.chartActiveConsumersData);
          console.log( this.chartActiveProducersData);

          this.drawChartsTopActiveDevices();
        }
      }, error => {

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
          pointBackgroundColor: 'rgba(250, 65, 65,1)',
          backgroundColor: 'rgba(250, 65, 65,1)',
          borderColor: 'rgba(250, 65, 65, 0.1)',
          tension: 0.2,
          pointStyle: 'circle'
        },{
          label: "Predicted consumption (kW)",
          data: this.chart1Data.predictedConsumption,
          pointBackgroundColor: 'rgba(254, 0, 0, 0.5)',
          backgroundColor: 'rgba(254, 0, 0, 0.5)',
          borderColor: 'rgba(254, 0, 0, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'rectRounded'
        },{
          label: "Total production(kW)",
          data: this.chart1Data.production,
          pointBackgroundColor: 'rgba(0, 0, 255, 1)',
          backgroundColor: 'rgba(0, 0, 255, 1)',
          borderColor: 'rgba(0, 0, 255, 0.1)',
          tension: 0.2,
          pointStyle: 'circle'
        },{
          label: "Predicted production (kW)",
          data: this.chart1Data.predictedProduction,
          pointBackgroundColor: 'rgba( 0, 0,254, 0.5)',
          backgroundColor: 'rgba( 0, 0,254, 0.5)',
          borderColor: 'rgba( 0, 0,254, 0.1)',
          borderDash: [5,5],
          tension: 0.2,
          pointStyle: 'rectRounded'
        }]
      },
      options: {
        maintainAspectRatio: true,
        responsive: true,
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
          pointStyle: 'rectRounded'
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
          x: {

          },
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

  drawChartsTopActiveDevices() {
    const canvas: any = document.getElementById("chart-active-consumers");
    const chart2d = canvas.getContext("2d");
    if( this.chartActiveConsumers) {
      this.chartActiveConsumers.destroy();
    }
    this.chartActiveConsumers = new Chart(chart2d, {
      type: 'bar',
      data: {
        labels: this.chartActiveConsumersData.labels,
        datasets: [{
          data: this.chartActiveConsumersData.consumption,
          backgroundColor: 'rgba(254, 0, 0, 0.5)',
          borderColor: 'rgba(254, 0, 0, 0.1)'
        }]
      },
      options: {
        plugins: {
          legend: {
            display: false
          },
          datalabels: {
            anchor: 'end',
            align: 'right',
            formatter: (value) => {
              return value+'kW';
            }
          }
        },
        indexAxis: 'y',
        elements: {
          bar: {
            borderWidth: 2,
          }
        },
        responsive: true,
        scales: {
          y: {
            beginAtZero: true
          },
          x: {
            beginAtZero: true,
            display: false
          }
        }
      }
    });
    const canvas2: any = document.getElementById("chart-active-producers");
    const chart2d2 = canvas2.getContext("2d");
    if( this.chartActiveProducers) {
      this.chartActiveProducers.destroy();
    }
    this.chartActiveProducers = new Chart(chart2d2, {
      type: 'bar',
      data: {
        labels: this.chartActiveProducersData.labels,
        datasets: [{
          data: this.chartActiveProducersData.production,
          backgroundColor: 'rgba(0, 0, 254, 0.5)',
          borderColor: 'rgba(0, 0, 254, 0.1)'
        }]
      },
      options: {
        plugins: {
          legend: {
            display: false
          }
        },
        indexAxis: 'y',
        elements: {
          bar: {
            borderWidth: 2,
          }
        },
        responsive: true,
        scales: {
          y: {
            beginAtZero: true
          },
          x: {
            beginAtZero: true,
            display: false
          }
        }
      }
    });
  }
}
