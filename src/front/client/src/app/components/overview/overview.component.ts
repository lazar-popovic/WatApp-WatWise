import {Component, OnInit} from '@angular/core';
import {DeviceDataService} from "../../services/device-data.service";
import {JWTService} from "../../services/jwt.service";
import {Chart} from "chart.js";
import {DatePipe} from "@angular/common";
import {DeviceService} from "../../services/device.service";
import * as DataLabelsPlugin from 'chartjs-plugin-datalabels';
import { Color, LegendPosition, ScaleType } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.css']
})
export class OverviewComponent implements OnInit
{
  yAxisTickFormatting(value: number) {
    return value;
  }
  xAxisTickFormatting( value: string) {
    return  value.split(":")[0];
  }

  currentConsumption: number = 0;
  currentProduction: number = 0;

  cards = {
    totalConsumption: 0 as number,
    totalProduction: 0 as number,
    currentConsumption: 0 as number,
    currentProduction: 0 as number
  }

  chart1 = {
    data: [] as any[],
    colors: {
      name: 'mycolors',
      selectable: true,
      group: ScaleType.Ordinal,
      domain: [],
    } as Color,
    legendPosition: "below" as LegendPosition
  }

  chart2 = {
    data: [] as any[],
    colors: {
      name: 'mycolors',
      selectable: true,
      group: ScaleType.Ordinal,
      domain: ['rgba(191, 65, 65, 0.4)', 'rgba(69, 94, 184, 0.4)'],
    } as Color,
    legendPosition: "below" as LegendPosition
  }

  chart3 = {
    data: [] as any[],
    colors: {
      name: 'mycolors',
      selectable: true,
      group: ScaleType.Ordinal,
      domain: ['rgba(191, 65, 65, 1)'],
    } as Color
  }

  chart4 = {
    data: [] as any[],
    colors: {
      name: 'mycolors',
      selectable: true,
      group: ScaleType.Ordinal,
      domain: ['rgba(69, 94, 184, 1)'],
    } as Color
  }

  constructor( private deviceDataService: DeviceDataService, private jwtService: JWTService, private datePipe: DatePipe, private deviceService: DeviceService) {
    Chart.register( DataLabelsPlugin);
  }

  ngOnInit(): void {
    this.getChart1Data();

    this.getChart2Data();

    this.getActiveDevices();
  }

  getChart1Data() {
    const date = new Date();
    this.deviceDataService.getUserDayStats( date.getDate(), date.getMonth()+1, date.getFullYear(), this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.chart1.data = [];
          if( result.data.consumingEnergyUsageByTimestamp.length > 0) {
            console.log("c da");
            this.chart1.data.push({
              name:"Consumption [kWh]",
              series: result.data.consumingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                              .map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.totalEnergyUsage}))
            });
            this.chart1.colors.domain.push('rgba(191, 65, 65, 1)');
          }
          if( result.data.producingEnergyUsageByTimestamp.length > 0) {
            console.log("p da");
            this.chart1.data.push({
              name:"Production [kWh]",
              series: result.data.producingEnergyUsageByTimestamp.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                              .map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.totalEnergyUsage}))
            });
            this.chart1.colors.domain.push('rgba(69, 94, 184, 1)');
          }
          if( result.data.producingEnergyUsageByTimestamp.length > 0) {
            console.log("pp da");
            this.chart1.data.push({
              name:"Predicted production [kWh]",
              series: result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.predictedValue}))
            });
            this.chart1.colors.domain.push('rgba(69, 94, 184, 0.4)');
          }
          if( result.data.consumingEnergyUsageByTimestamp.length > 0) {
            console.log("pc da");
            this.chart1.data.push({
              name:"Predicted consumption [kWh]",
              series: result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.predictedValue}))
            });
            this.chart1.colors.domain.push('rgba(191, 65, 65, 0.4)');
          }
          console.log( this.chart1.data);
          this.cards.totalConsumption = result.data.consumingEnergyUsageByTimestamp.reduce((total:any, current:any) => {
            return total + current.predictedValue;
          }, 0);

          this.cards.totalProduction = result.data.producingEnergyUsageByTimestamp.reduce((total:any, current:any) => {
            return total + current.predictedValue;
          }, 0);

          this.cards.currentConsumption = this.chart1.data.find(d => d.name === "Consumption [kWh]")?.series.slice(-1)[0]?.value;
          if( this.cards.currentConsumption == undefined) {
            this.cards.currentConsumption = 0;
          }
          this.cards.currentProduction = this.chart1.data.find(d => d.name === "Production [kWh]")?.series.slice(-1)[0]?.value;
          if( this.cards.currentProduction == undefined) {
            this.cards.currentProduction = 0;
          }

          console.log("cards");
          console.log(this.cards);
        }
      }, error => {
        console.log( error);
      }
    );//this.datePipe.transform(ceu.timestamp,"shortTime")
  }

  getChart2Data() {
    const date2 = new Date();
      date2.setDate( date2.getDate() + 1);
      this.deviceDataService.getUserDayStats( date2.getDate(), date2.getMonth()+1, date2.getFullYear(), this.jwtService.userId).subscribe(
        (result:any) => {
          if( result.success) {
            this.chart2.data = [];
            if( result.data.consumingEnergyUsageByTimestamp.length > 0) {
              this.chart2.data.push({
                "name":"Predicted consumption [kWh]",//this.datePipe.transform(ceu.timestamp,"shortTime")
                "series": result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.predictedValue}))
              });
            }
            if( result.data.producingEnergyUsageByTimestamp.length > 0) {
              this.chart2.data.push({
                "name":"Predicted production [kWh]",//this.datePipe.transform(ceu.timestamp,"shortTime")
                "series": result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5), value: ceu.predictedValue}))
              });
            }
            /*this.chart2.data = [{
                "name":"Predicted consumption [kWh]",//this.datePipe.transform(ceu.timestamp,"shortTime")
                "series": result.data.consumingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5).toTimeString(), value: ceu.predictedValue}))
              },{
                "name":"Predicted production [kWh]",
                "series": result.data.producingEnergyUsageByTimestamp.map( (ceu:any) => ({name: ceu.timestamp.split("T")[1].substring(0, 5).toTimeString(), value: ceu.predictedValue}))
              }
            ];*/
          }
        }, error => {
          console.log( error);
        }
      );
  }

  getActiveDevices() {
    this.deviceService.getTop3ByCategory( this.jwtService.userId).subscribe(
      (result:any) => {
        if( result.success) {
          this.chart3.data = [];
          let consumers = result.data.find( (c:any) => c.category == -1);
          if( consumers != null) {
            this.chart3.data = consumers.devices;
          }
          this.chart4.data = [];
          let producers = result.data.find( (c:any) => c.category == 1);
          if( producers != null) {
            this.chart4.data = producers.devices;
          }
        }
      }, error => {

      }
    );
  }
}
