import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { DeviceDataService } from 'src/app/services/device-data.service';
import * as ChartAnnotation from 'chartjs-plugin-annotation';

@Component({
  selector: 'app-overview-dso',
  templateUrl: './overview-dso.component.html',
  styleUrls: ['./overview-dso.component.css']
})
export class OverviewDsoComponent implements OnInit {

  constructor( private deviceDataService: DeviceDataService, private datePipe: DatePipe) {
    Chart.register( ChartAnnotation);
  }

  liveChart: any = null;

  liveChartData = {
    production: [] as any[],
    consumption: [] as any[],
    predictedConsumption: [] as any[],
    predictedProduction: [] as any[]
  }

  ngOnInit(): void {
    this.deviceDataService.getDSOOverviewLiveData().subscribe(
      (result:any) => {
        if( result.success) {
          this.liveChartData.consumption = result.data.consumptionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() <= Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.liveChartData.production = result.data.productionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() <= Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.liveChartData.predictedConsumption = result.data.consumptionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() > Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.liveChartData.predictedProduction = result.data.productionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() > Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          console.log( this.liveChartData);
          this.drawLiveChart();
        }
      }, errors => {
        console.log( errors);
      }
    )
  }

  drawLiveChart() {
    const canvas: any = document.getElementById("live-chart");
    const chart2d = canvas.getContext("2d");
    if( this.liveChart) {
      this.liveChart.destroy();
    }
    this.liveChart = new Chart( chart2d, {
      type: 'bar',
      data: {
        datasets: [{
          data: this.liveChartData.consumption,
          label: 'Consumption [kWh]',
          backgroundColor: 'rgba(191, 65, 65, 1)',
          borderColor: 'rgba(191, 65, 65, 1)',
          borderWidth: 1
        },{
          data: this.liveChartData.predictedConsumption,
          label: 'Predicted consumption [kWh]',
          backgroundColor: 'rgba(191, 65, 65, 0.5)',
          borderColor: 'rgba(191, 65, 65, 0.5)',
          borderWidth: 1
        },{
          data: this.liveChartData.production,
          label: 'Production [kWh]',
          backgroundColor: 'rgba(69, 94, 184, 1)',
          borderColor: 'rgba(69, 94, 184, 1)',
          borderWidth: 1
        },{
          data: this.liveChartData.predictedProduction,
          label: 'Predicted production [kWh]',
          backgroundColor: 'rgba(69, 94, 184, 0.5)',
          borderColor: 'rgba(69, 94, 184, 0.5)',
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        resizeDelay: 10,
        plugins: {
          annotation: {
            annotations: {
              box1: {
                type: 'line',
                yMin: 0,
                yMax: 2,
                xMin: 6.5,
                xMax: 6.5,
              }
            }
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              callback: function(value, index, ticks) {
                return value+'kWh';
              }
            }
          }
        }
      }});

  }

}
