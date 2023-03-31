import { Component } from '@angular/core';
import {ChartData, ChartDataset, ChartOptions, registerables} from 'chart.js';

import { Chart } from 'chart.js';
import {AuthService} from "../../services/auth-service.service";
import {UserService} from "../../services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {DeviceService} from "../../services/device.service";
import {DeviceDataService} from "../../services/device-data.service";
import {DatePipe} from "@angular/common";


@Component({
  selector: 'app-device-details',
  templateUrl: './device-details.component.html',
  styleUrls: ['./device-details.component.css'],

})
export class DeviceDetailsComponent
{
    categoryLabel: string = "";
    color: any = 'rgba(206, 27, 14, 0.7)';
    id : any = '' ;
    result: any[] = [];
    data: any[] = [];
    labels: any[] = [];
    device = {
      id: 0,
      userId: 0,
      name: "",
      activityStatus: false,
      deviceType: null,
      dataShare: false
    }
    constructor( private datePipe: DatePipe, private authService:AuthService, private deviceService: DeviceService, private route: ActivatedRoute, private router: Router, private deviceDataService: DeviceDataService) {
      this.deviceService.getDeviceById(this.route.snapshot.paramMap.get('id')).subscribe(
        result => {
          if( result.success) {
            this.device.id = result.data.id;
            this.device.userId = result.data.userId;
            this.device.name = result.data.name;
            this.device.activityStatus = result.data.activityStatus;
            this.device.deviceType = result.data.deviceType;
            switch ( result.data.deviceType.category)
            {
              case -1: this.categoryLabel = "Consumption in kW"; this.color = 'rgba(236, 27, 14, 0.7)'; break;
              case 0: this.categoryLabel = "In storage"; this.color = 'rgba(27, 254, 127, 0.9)'; break;
              case 1: this.categoryLabel = "Production in kW"; this.color = 'rgba(27, 27, 236, 0.7)'; break;
            }
            console.log( this.device);
          }
        }, error => {
          console.log( error);
        }
      )
      Chart.register(...registerables);
    }
    ngOnInit(): void
    {
    }

    todayFlag : boolean = true;
    monthFlag : boolean = false;
    yearFlag : boolean = false;

    todayClick()
    {

      this.todayFlag  = true; this.monthFlag  = false; this.yearFlag = false;
      var todayDiv = document.getElementById("today");
      if(todayDiv)
      {
        todayDiv.style.color = "#1676AC";
        todayDiv.style.backgroundColor =  "#ffff";
        todayDiv.style.padding = "5px";
        todayDiv.style.borderRadius = "5px";
      }

      var monthDiv = document.getElementById("month");
      if(monthDiv){ monthDiv.style.backgroundColor = "transparent"; monthDiv.style.color = "#ffff";}

      var yearDiv = document.getElementById("year");
      if(yearDiv){ yearDiv.style.backgroundColor = "transparent"; yearDiv.style.color = "#ffff";}

      this.deviceDataService.getDeviceDataForToday( this.device.id).subscribe(
        result => {
          if( result.success) {
            this.result = result.data;
            this.labels = this.result.map( e => this.datePipe.transform(e.timestamp,"shortTime"));
            this.data = this.result.map( e => e.value);
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
        monthDiv.style.color = "#1676AC";
        monthDiv.style.backgroundColor =  "#ffff";
        monthDiv.style.padding = "5px";
        monthDiv.style.borderRadius = "5px";
      }

      const todayDiv = document.getElementById("today");
      if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#ffff";}

      const yearDiv = document.getElementById("year");
      if(yearDiv){ yearDiv.style.backgroundColor = "transparent "; yearDiv.style.color="#ffff";}

      this.deviceDataService.getDeviceDataForMonth( this.device.id).subscribe(
        result => {
          if( result.success) {
            this.result = result.data;
            this.labels = this.result.map( e => e.timestamp);
            this.data = this.result.map( e => e.value);
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
        yearDiv.style.color = "#1676AC";
        yearDiv.style.backgroundColor =  "#ffff";
        yearDiv.style.padding = "5px";
        yearDiv.style.borderRadius = "5px";
      }

      const todayDiv = document.getElementById("today");
      if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="#ffff";}

      const monthDiv = document.getElementById("month");
      if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="#ffff";}

      this.deviceDataService.getDeviceDataForYear( this.device.id).subscribe(
        result => {
          if( result.success) {
            this.result = result.data;
            this.labels = this.result.map( e => e.timestamp);
            this.data = this.result.map( e => e.value);
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
        const canvas: any = document.getElementById("chart");
        const chart2d = canvas.getContext("2d");
        if( this.chart) {
          this.chart.destroy();
        }
        this.chart = new Chart(chart2d, {
          type: 'bar',
          data: {
            labels: this.labels,
            datasets: [{
              label: this.categoryLabel,
              data: this.data,
              backgroundColor: this.color
            }]
          },
          options: {  scales: { y: {  beginAtZero: true,
                ticks: {
                  callback: function(value, index, ticks) {
                    return value+'kW';
                  }
                } } } }
        });
    }

    createTable(): void
    {

      const table = document.createElement('table');
      table.classList.add('data-table');

      // create table header
      const thead = table.createTHead();
      const headerRow = thead.insertRow();
      const headers = ['', ''];
      headers.forEach(header => {
        const th = document.createElement('th');
        th.textContent = header;
        headerRow.appendChild(th);
      });

      // create table body
      const tbody = table.createTBody();
      this.labels.forEach((label, index) => {
        const row = tbody.insertRow();
        const dateCell = row.insertCell();
        const timeCell = row.insertCell();
        const consumptionCell = row.insertCell();
        timeCell.textContent = label;
        consumptionCell.textContent = this.data[index].toString();
      });

      const tableContainer = document.getElementById('tableShow');
      if (tableContainer) {
        tableContainer.innerHTML = '';
        tableContainer.appendChild(table);
      }
    }
}




