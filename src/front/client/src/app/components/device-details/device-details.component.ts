import { Component } from '@angular/core';
import { ChartData, ChartDataset, ChartOptions } from 'chart.js';

import { Chart } from 'chart.js';


@Component({
  selector: 'app-device-details',
  templateUrl: './device-details.component.html',
  styleUrls: ['./device-details.component.css']
})
export class DeviceDetailsComponent 
{
   
    ngOnInit(): void 
    {
      this.chartSelect();
    }

    todayFlag : boolean = true;
    monthFlag : boolean = false;
    yearFlag : boolean = false;
    
    todayClick()  
    {
      
      this.todayFlag  = true; this.monthFlag  = false; this.yearFlag = false;
      this.chartSelect();
      var todayDiv = document.getElementById("today");
      if(todayDiv)
      {
        todayDiv.style.color = "#ffff";
        todayDiv.style.backgroundColor = "#1676AC";
        todayDiv.style.padding = "5px";
        todayDiv.style.borderRadius = "5px";
      }

      var monthDiv = document.getElementById("month");
      if(monthDiv){ monthDiv.style.backgroundColor = "transparent"; monthDiv.style.color = "black";}

      var yearDiv = document.getElementById("year");
      if(yearDiv){ yearDiv.style.backgroundColor = "transparent"; yearDiv.style.color = "black";}

      this.chartSelect();
    }

    monthClick()  
    {
      this.todayFlag = false; this.monthFlag  = true; this.yearFlag = false;
      this.chartSelect();
      const monthDiv = document.getElementById("month");
      if(monthDiv)
      {
        monthDiv.style.color = "#ffff";
        monthDiv.style.backgroundColor = "#1676AC";
        monthDiv.style.padding = "5px";
        monthDiv.style.borderRadius = "5px";
      }

      const todayDiv = document.getElementById("today");
      if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="black";}

      const yearDiv = document.getElementById("year");
      if(yearDiv){ yearDiv.style.backgroundColor = "transparent "; yearDiv.style.color="black";}

      this.chartSelect();     
    }

    yearClick()  
    {
      this.todayFlag = false; this.monthFlag  = false; this.yearFlag = true;
      this.chartSelect();
      var yearDiv = document.getElementById("year");
      if(yearDiv)
      {
        yearDiv.style.color = "#ffff";
        yearDiv.style.backgroundColor = "#1676AC";
        yearDiv.style.padding = "5px";
        yearDiv.style.borderRadius = "5px";
      }

      const todayDiv = document.getElementById("today");
      if(todayDiv){ todayDiv.style.backgroundColor = "transparent "; todayDiv.style.color="black";}

      const monthDiv = document.getElementById("month");
      if(monthDiv){ monthDiv.style.backgroundColor = "transparent "; monthDiv.style.color="black";}

      this.chartSelect();     
    }

    chartSelect()
    {
      if(this.todayFlag==true){
        this.createBarChart_today();
        this.createTable_today();
      }
      
      if(this.monthFlag==true){
        this.createBarChart_month();
      }

      if(this.yearFlag==true){
        this,this.createBarChart_year();
      }
      
    }
    
    createBarChart_today() 
    {
        const todayCanvas: any = document.getElementById("barChartDay");
        const today = todayCanvas.getContext("2d");
        const todayChart = new Chart(today, {
          type: 'bar',
          data: {
            labels: Array.from({ length: 24 }, (_, i) => (0+i+"h").toString()),
            datasets: [{
              label: 'Hourly Consumption (kWh)',
              data: [1.250, 1.292, 1.333, 1.375, 1.417, 1.458, 1.500, 1.542, 1.583, 1.625, 1.667, 1.625, 1.583, 1.542, 1.500, 1.458, 1.417, 1.375, 1.333, 1.292, 1.250, 1.208, 1.167, 1.125, 1.083, 1.042, 1.083, 1.125, 1.167, 1.208, 1.250],
              backgroundColor: 'rgba(75, 175, 242, 0.2)',             
              borderColor: 'rgba(28, 109, 163, 1)',
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
    }
    
    createBarChart_month() 
    {
        const monthCanvas: any = document.getElementById("barChartMonth");
        const month = monthCanvas.getContext("2d");
        const monthChart = new Chart(month, {
          type: 'bar',
          data: {
            labels: Array.from({ length: 31 }, (_, i) => (i + 1+".").toString()),
            datasets: [{
              label: 'Daily Consumption (kWh)',
              data: [30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 26, 27, 28, 29, 30 ],
              backgroundColor: 'rgba(206, 237, 55, 0.5)',
              borderColor: 'rgba(144, 173, 10, 1)',
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
    }

    createBarChart_year() 
    {
        const yearCanvas: any = document.getElementById("barChartYear");
        const month = yearCanvas.getContext("2d");
        const monthChart = new Chart(month, {
          type: 'bar',
          data: {
            labels:['JAN','FEB','MAR','APR','MAY','JUN','JUL','AVG','SEP','OKT','NOV','DEC'],
            
            datasets: [{
              label: 'Monthly Consumption (kWh)',
              data: [984, 941, 890, 914, 948, 948, 923, 950, 890, 875, 879, 890],
              backgroundColor: 'rgba(255, 99, 132, 0.2)',
              borderColor: 'rgba(255, 99, 132, 1)',
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
    }

    createTable_today(): void 
    {
      const tableData = {
        labels: Array.from({ length: 24 }, (_, i) => (0+i+"h").toString()),
        data: [1.250, 1.292, 1.333, 1.375, 1.417, 1.458, 1.500, 1.542, 1.583, 1.625, 1.667, 1.625, 1.583, 1.542, 1.500, 1.458, 1.417, 1.375, 1.333, 1.292, 1.250, 1.208, 1.167, 1.125, 1.083, 1.042, 1.083, 1.125, 1.167, 1.208, 1.250]
      };
      
      const table = document.createElement('table');
      table.classList.add('data-table');
      
      // create table header
      const thead = table.createTHead();
      const headerRow = thead.insertRow();
      const headers = ['Date', 'Time', 'Consumption'];
      headers.forEach(header => {
        const th = document.createElement('th');
        th.textContent = header;
        headerRow.appendChild(th);
      });
      
      // create table body
      const tbody = table.createTBody();
      const today = new Date();
      const day = today.getDate();
      const month = today.getMonth() + 1;
      const year = today.getFullYear();
      tableData.labels.forEach((label, index) => {
        const row = tbody.insertRow();
        const dateCell = row.insertCell();
        const timeCell = row.insertCell();
        const consumptionCell = row.insertCell();
        dateCell.textContent = `${day}/${month}/${year}`;
        timeCell.textContent = label;
        consumptionCell.textContent = tableData.data[index].toString();
      });
      
      const tableContainer = document.getElementById('tableContainer_today');
      if (tableContainer) {
        tableContainer.innerHTML = '';
        tableContainer.appendChild(table);
      }
    }
    
    
    
}



  // createBarChart() {
    //   var canvas: any = document.getElementById("barChart");
    //   var ctx = canvas.getContext("2d");
  
    //   var myChart = new Chart(ctx, {
    //     type: 'bar',
    //     data: {
    //       labels: ["January", "February", "March", "April", "May", "June", "July"],
    //       datasets: [{
    //         label: 'Temperature',
    //         data: [25, 24, 23, 22, 21, 20, 19],
    //         backgroundColor: [
    //           'rgba(255, 99, 132, 0.2)',
    //           'rgba(54, 162, 235, 0.2)',
    //           'rgba(255, 206, 86, 0.2)',
    //           'rgba(75, 192, 192, 0.2)',
    //           'rgba(153, 102, 255, 0.2)',
    //           'rgba(255, 159, 64, 0.2)',
    //           'rgba(255, 99, 132, 0.2)'
    //         ],
    //         borderColor: [
    //           'rgba(255, 99, 132, 1)',
    //           'rgba(54, 162, 235, 1)',
    //           'rgba(255, 206, 86, 1)',
    //           'rgba(75, 192, 192, 1)',
    //           'rgba(153, 102, 255, 1)',
    //           'rgba(255, 159, 64, 1)',
    //           'rgba(255, 99, 132, 1)'
    //         ],
    //         borderWidth: 1
    //       }]
    //     }
    //   });
    // }


 