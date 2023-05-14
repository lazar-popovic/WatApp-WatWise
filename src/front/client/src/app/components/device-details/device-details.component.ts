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
//import { JWTService } from 'src/app/services/jwt.service';
import { JWTService } from '../../services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { Subscription } from 'rxjs';


interface DatepickerOptions {
  autoclose?: boolean;
}

@Component({
  selector: 'app-device-details',
  templateUrl: './device-details.component.html',
  styleUrls: ['./device-details.component.css','./device-details.component.2.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceDetailsComponent implements OnInit
{
    categoryLabel: string = "";
    color: any = 'rgba(206, 27, 14, 0.7)';
    predColor: any = 'rgba(206, 27, 14, 0.7)';
    id : any = '' ;
    result: any[] = [];
    data: any[] = [];
    device = {
      id: 0,
      userId: 0,
      name: "",
      activityStatus: false,
      deviceType: { type: null, category: null },
      deviceSubtype: { subtypeName: null },
      capacity: 1,
      dataShare: false,
      currentUsage: 0,
      dsoControl: false
    }
    capacity: number = 1;

    busy: Subscription | undefined;

    tableTitle: string = "Timestamp";
    roleId: number = 3;

    showEdit: boolean = false;
    showDelete: boolean = false;

    date: any;
    month: number = 4;
    yearForMonth: number = 2023;
    year: number = 2023;

    myDevice() {
      if( this.device.userId == this.jwtService.userId || this.roleId == 1 || this.roleId == 2)
        return true;
      else
        return false;
    }

    myDeviceBack() {
      if( this.device.userId == this.jwtService.userId)
        return true;
      else
        return false;
    }

    formForNewJob: boolean = false;
    showDeleteDeviceForm: boolean = false;
    showEditDeviceForm: boolean = false;

    constructor( private datePipe: DatePipe,
                 private authService:AuthService,
                 private deviceService: DeviceService,
                 private route: ActivatedRoute,
                 private router: Router,
                 private deviceDataService: DeviceDataService,
                 private jwtService: JWTService,
                 private toastrNotifService: ToastrNotifService) {
      this.roleId = this.jwtService.roleId;
      this.data=[1];
      this.busy = this.deviceService.getDeviceById(this.route.snapshot.paramMap.get('id')).subscribe(
        result => {
          if( result.success) {
            if( (result.data.userId == this.jwtService.userId) || (( this.jwtService.roleId == 1 || this.jwtService.roleId == 2 ) && result.data.dataShare == true)) {
              this.device.id = result.data.id;
              this.device.userId = result.data.userId;
              this.device.name = result.data.name;
              this.device.activityStatus = result.data.activityStatus;
              this.device.deviceType = result.data.deviceType;
              this.device.deviceSubtype = result.data.deviceSubtype;
              this.device.dataShare = result.data.dataShare;
              this.device.capacity = result.data.capacity;
              this.device.dsoControl = result.data.dsoControl;
              switch ( result.data.deviceType.category)
              {
                case -1:
                  this.categoryLabel = "Consumption [kWh]";
                  this.color = 'rgba(191, 65, 65, 1)';
                  this.predColor = 'rgba(191, 65, 65, 0.4)';
                  break;
                case 0:
                  this.categoryLabel = "In storage [kWh]";
                  this.color = 'rgba(27, 254, 127, 1)';
                  this.predColor = 'rgba(27, 254, 127, 0.4)';
                  break;
                case 1:
                  this.categoryLabel = "Production [kWh]";
                  this.color = 'rgba(69, 94, 184, 1)';
                  this.predColor = 'rgba(69, 94, 184, 0.4)';
                  break;
              }
              if( this.device.deviceType.category == 0) {
                this.capacity = this.device.capacity;
              }
              let now = new Date();
              this.date = now.getFullYear() + "-" + (now.getMonth()+1) +"-" + now.getDate();
              this.historyClick();
              this.deviceDataService.getDeviceCurrentUsage( this.device.id).subscribe(
                (result:any) => {
                  if( result.success) {
                    this.device.currentUsage = result.data.value;
                  }
                }
              )
              console.log( this.device);
            }
            else {
              this.router.navigate(['/profile',result.data.userId]);
            }
          }
        }, error => {
          console.log( error);
        }
      )
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

    showDropdown : boolean = false;

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
      this.data=[1];
      this.tableTitle = "Hour";
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
      this.deviceDataService.getDeviceDataForDate( date.getDate(), date.getMonth()+1, date.getFullYear(), this.device.id).subscribe(
        (result:any) => {
          if( result.success) {
            this.data = result.data.map((ceu: any) => {
              let ceuDate = new Date(ceu.timestamp);
              let currentDate = new Date();
              let timestamp = this.datePipe.transform(ceu.timestamp, "shortTime");
              let predictedValue = ceu.predictedValue.toFixed(3);
              let value = currentDate < ceuDate ? "/" : ceu.value.toFixed(3);
              if( this.device.deviceType.category == 0) {
                predictedValue = (predictedValue * this.device.capacity).toFixed(3);
                if( value != "/") {
                  value = ( value * this.device.capacity).toFixed(3);
                }
              }
              return { timestamp, predictedValue, value }
            });
            this.additionalStats();
            let now = new Date();
            if( date.toDateString() == now.toDateString()) {
              this.datasets = [{
                data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue * this.capacity})),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
                borderWidth: 2
              },{
                data: result.data.filter((ceu:any) => new Date(ceu.timestamp) <= new Date())
                                                                 .map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value * this.capacity})),
                label: this.categoryLabel,
                backgroundColor: this.color,
                borderColor: this.color,
                borderWidth: 2
              }];

            } else if ( date > now) {
              console.log("pred");
              this.datasets = [{
                data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue * this.capacity})),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
                borderWidth: 2
              }];
            } else {
              console.log("hist");
              this.datasets = [{
                data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.predictedValue * this.capacity})),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
                borderWidth: 2
              },{
                data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp,"shortTime"), y: ceu.value * this.capacity})),
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
      this.data=[1];
      this.tableTitle = "Day";
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

      this.deviceDataService.getDeviceDataForMonth(this.month, this.yearForMonth, this.device.id).subscribe(
        (result:any) => {
          console.log( result)
          if( result.success) {
            this.data = result.data.map((ceu: any) => ({ timestamp:ceu.timestamp, value:ceu.value.toFixed(3), predictedValue:ceu.predictedValue.toFixed(3) }));
            this.additionalStats();
            let now = new Date();
            if( this.month == now.getMonth()+1) {
              this.datasets = [{
                data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
                borderWidth: 2
              },{
                data: result.data.filter((ceu:any) => new Date(ceu.timestamp).getDate() <= new Date().getDate())
                                                                 .map( (ceu:any) => ({x: ceu.timestamp, y: ceu.value})),
                label: this.categoryLabel,
                backgroundColor: this.color,
                borderColor: this.color,
                borderWidth: 2
              }];

            } else if ( this.month > now.getMonth()+1) {
              console.log("pred");
              this.datasets = [{
                data: result.data.map((ceu: any) => ({ x: ceu.timestamp, y: ceu.predictedValue })),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
                borderWidth: 2
              }];
            } else {
              console.log("hist");
              this.datasets = [{
                data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
                label: 'Predicted ' + this.categoryLabel,
                backgroundColor: this.predColor,
                borderColor: this.predColor,
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
      this.data=[1];
      this.tableTitle = "Month";
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

      this.deviceDataService.getDeviceDataForYear(this.year, this.device.id).subscribe(
        (result:any) => {
          if( result.success) {
            this.data = result.data.map((ceu: any) => ({ timestamp:ceu.timestamp, value:ceu.value.toFixed(3), predictedValue:ceu.predictedValue.toFixed(3) }));
            this.additionalStats();
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: "Predicted " + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.predColor,
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
      this.data=[1];
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

      this.deviceDataService.getDeviceDataForNextNDays( this.device.id, 1).subscribe(
        (result:any) => {
          if( result.success) {
            this.data = result.data.map((ceu: any) => {
              const ceuDate = new Date(ceu.timestamp);
              const currentDate = new Date();
              const timestamp = this.datePipe.transform(ceu.timestamp, "shortTime");
              const predictedValue = ceu.predictedValue.toFixed(3);
              const value = currentDate < ceuDate ? "/" : ceu.value.toFixed(3);
              return { timestamp, predictedValue, value }
            });
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: this.datePipe.transform(ceu.timestamp, "shortTime"), y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.predColor,
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
      this.data=[1];
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

      this.deviceDataService.getDeviceDataForNextNDays(this.device.id, 3).subscribe(
        (result:any) => {
          if( result.success) {
            this.data = result.data.map((ceu: any) => {
              const ceuDate = new Date(ceu.timestamp);
              const currentDate = new Date();
              const timestamp = this.datePipe.transform(ceu.timestamp, "shortTime");
              const predictedValue = ceu.predictedValue.toFixed(3);
              const value = currentDate < ceuDate ? "/" : ceu.value.toFixed(3);
              return { timestamp, predictedValue, value }
            });
            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.predColor,
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
      this.data=[1];
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

      this.deviceDataService.getDeviceDataForNextNDays( this.device.id, 7).subscribe(
        (result:any) => {
          if( result.success) {
            this.data = result.data.map((ceu: any) => ({ timestamp:ceu.timestamp, value:ceu.value.toFixed(3), predictedValue:ceu.predictedValue.toFixed(3) }));

            this.datasets = [{
              data: result.data.map( (ceu:any) => ({x: ceu.timestamp, y: ceu.predictedValue})),
              label: 'Predicted ' + this.categoryLabel,
              backgroundColor: this.predColor,
              borderColor: this.predColor,
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
                  return value+'kWh';
                }
              }
            }
          }
        }
      });
    }

    additionalStatsData = {
      max: {
        timestamp: "",
        value: 0
      },
      min: {
        timestamp: "",
        value: 0
      },
      mean: 0,
      mae: 0,
      rmse: 0
    }

    additionalStats() {
      if( this.historyflag) {
        let values = this.data.filter( (ceu:any) => ceu.value != "/");
        if( values.length > 0) {
          this.additionalStatsData.max = values.reduce((prev, current) => (parseFloat(current.value) > parseFloat(prev.value) ? current : prev));
          this.additionalStatsData.min = values.reduce((prev, current) => (parseFloat(current.value) < parseFloat(prev.value) ? current : prev));
          this.additionalStatsData.mean = values.reduce((total, obj) => {
            return parseFloat(total) + parseFloat(obj.value);
          }, 0) / values.length;

          this.additionalStatsData.mae = values.reduce((total, obj) => {
            return total + Math.abs(parseFloat(obj.value) - parseFloat(obj.predictedValue));
          }, 0) / values.length;

          this.additionalStatsData.rmse = Math.sqrt(values.reduce((total, obj) => {
            return total + Math.pow(parseFloat(obj.value) - parseFloat(obj.predictedValue), 2);
          }, 0) / values.length);
        }
      }
    }

    showEditForm() {
      (document.querySelector('.device-details-overlay') as HTMLDivElement).style.display = 'block';
      (document.querySelector('.device-details-edit') as HTMLDivElement).style.display = 'block';
    }

    showDeleteForm() {
      (document.querySelector('.device-details-overlay') as HTMLDivElement).style.display = 'block';
      (document.querySelector('.device-details-delete') as HTMLDivElement).style.display = 'block';
    }

    hideForm(status: boolean) {
      (document.querySelector('.device-details-overlay') as HTMLDivElement).style.display = 'none';
      (document.querySelector('.device-details-edit') as HTMLDivElement).style.display = 'none';
      (document.querySelector('.device-details-delete') as HTMLDivElement).style.display = 'none';
    }

    displayTurnDialog: boolean = false;
    changeTo: boolean = false;
    turnDeviceForm( status: boolean) {
      console.log( "Status: " + status);
      if( status == true) {
        this.device.activityStatus = this.changeTo;
        this.deviceService.patchDeviceActivityStatus( this.device.id, this.changeTo).subscribe(
          (result: any) => {
            if( result.body.success) {
              this.toastrNotifService.showSuccess( result.body.data.message);
              this.device.activityStatus = this.changeTo;
              this.device.currentUsage = result.body.data.newUsage;
            }
          }
        );
      }
      else {
        this.device.activityStatus = !this.changeTo;
        this.toastrNotifService.showErrors(["Status of device was not changed!"]);
      }
      this.displayTurnDialog = false;
    }
  }
