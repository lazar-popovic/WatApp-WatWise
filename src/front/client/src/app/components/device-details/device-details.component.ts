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
    
    ngOnInit(): void {
      this.createBarChart();
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
    
    createBarChart() {
      const canvas: any = document.getElementById("barChart");
      const ctx = canvas.getContext("2d");
  
      const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: Array.from({ length: 31 }, (_, i) => (i + 1).toString()),
          datasets: [{
            label: 'Consumption (kWh)',
            data: [30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 26, 27, 28, 29, 30],
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
}


 