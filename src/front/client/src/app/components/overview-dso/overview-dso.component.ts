import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { DeviceDataService } from 'src/app/services/device-data.service';
import * as ChartAnnotation from 'chartjs-plugin-annotation';
import { LocationService } from 'src/app/services/location.service';
import { Color } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-overview-dso',
  templateUrl: './overview-dso.component.html',
  styleUrls: ['./overview-dso.component.css']
})
export class OverviewDsoComponent implements OnInit {

  constructor( private deviceDataService: DeviceDataService,
               private datePipe: DatePipe,
               private locationService: LocationService) {
    Chart.register( ChartAnnotation);
  }

  cards = {
    currentConsumption: 0 as number,
    currentProduction: 0 as number,
    nextHourConsumption: 0 as number,
    nextHourProduction: 0 as number
  }

  liveChart: any = null;

  liveChartData = {
    production: [] as any[],
    consumption: [] as any[],
    predictedConsumption: [] as any[],
    predictedProduction: [] as any[]
  }

  ngOnInit(): void {
    this.getCities();
    this.deviceDataService.getDSOOverviewLiveData().subscribe(
      (result:any) => {
        if( result.success) {
          this.liveChartData.consumption = result.data.consumptionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() <= Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.liveChartData.production = result.data.productionEnergyUsage.filter( (ceu:any) => new Date(ceu.timestamp).getTime() <= Date.now()).map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value}));
          this.liveChartData.predictedConsumption = result.data.consumptionEnergyUsage.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue}));
          this.liveChartData.predictedProduction = result.data.productionEnergyUsage.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue}));

          this.cards.currentConsumption = this.liveChartData.consumption[ this.liveChartData.consumption.length - 1]?.y;
          this.cards.currentProduction = this.liveChartData.production[ this.liveChartData.production.length - 1]?.y;

          this.cards.nextHourConsumption = this.liveChartData.predictedConsumption[ this.liveChartData.consumption.length]?.y;
          this.cards.nextHourProduction = this.liveChartData.predictedProduction[ this.liveChartData.production.length]?.y;

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

  selectedCityConsumption: string = "";
  selectedCityProduction: string = "";

  colorSchemeConsumption = {
    domain: ['rgba(191, 65, 65, 1)', 'rgba(191, 65, 65, 0.4)']
  } as Color;
  colorSchemeProduction = {
    domain: ['rgba(69, 94, 184, 1)', 'rgba(69, 94, 184, 0.4)']
  } as Color;

  chartDataConsumption: any[] = [];
  chartDataProduction: any[] = [];

  cities: any[] = [];
  getCities() : void {
    this.locationService.getCities( ).subscribe(
      ( result: any) => {
        if( result.success) {
          this.cities = result.data;
          this.selectedCityConsumption = this.cities[0];
          this.selectedCityProduction = this.cities[0];
          this.getTop5ConsumingNeighborhoods();
          this.getTop5ProducingNeighborhoods();
          console.log( result.data);
        }
        else {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }

  getTop5ConsumingNeighborhoods() : void {
    this.locationService.getTop5Neighborhoods( this.selectedCityConsumption, -1).subscribe((result:any) => {
      if( result.success) {
        this.chartDataConsumption = result.data.map((item:any) => ({
          name: item.neighborhood,
          series: [
            { name: "Consumption [kWh]", value: item.powerUsage },
            { name: "Predicted Consumption [kWh]", value: item.predictedPowerUsage }
          ]
        }));
        console.log( this.chartDataConsumption);
      }
    }, (errors:any) => {
      console.log( errors);
    })
  }

  getTop5ProducingNeighborhoods() : void {
    this.locationService.getTop5Neighborhoods( this.selectedCityProduction, 1).subscribe((result:any) => {
      if( result.success) {
        this.chartDataProduction = result.data.map((item:any) => ({
          name: item.neighborhood,
          series: [
            { name: "Production [kWh]", value: item.powerUsage },
            { name: "Predicted Production [kWh]", value: item.predictedPowerUsage }
          ]
        }));
      }
    }, (errors:any) => {
      console.log( errors);
    })
  }
}
