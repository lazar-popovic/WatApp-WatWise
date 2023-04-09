import { Component } from '@angular/core';
import {ChartData, ChartDataset, ChartOptions, registerables} from 'chart.js';

@Component({
  selector: 'app-consumption',
  templateUrl: './consumption.component.html',
  styleUrls: ['./consumption.component.css']
})
export class ConsumptionComponent 
{

  ngOnInit()
  {

  }

  chart: any;
  createBarChart()
  {
      // const canvas: any = document.getElementById("chart");
      // const chart2d = canvas.getContext("2d");
      // if( this.chart) {
      //   this.chart.destroy();
      // }
      // this.chart = new Chart(chart2d, {
      //   type: 'bar',
      //   data: {
      //     labels: this.labels,
      //     datasets: [{
      //       label: this.categoryLabel,
      //       data: this.data,
      //       backgroundColor: this.color
      //     }]
      //   },
      //   options: {  scales: { y: {  beginAtZero: true,
      //         ticks: {
      //           callback: function(value, index, ticks) {
      //             return value+'kW';
      //           }
      //         } } } }
      // });
  }

}
