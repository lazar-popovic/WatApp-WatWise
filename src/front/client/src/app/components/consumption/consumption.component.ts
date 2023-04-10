import { Component } from '@angular/core';
import {ChartData, ChartDataset, ChartOptions, registerables} from 'chart.js';
import { Chart } from 'chart.js';




@Component({
  selector: 'app-consumption',
  templateUrl: './consumption.component.html',
  styleUrls: ['./consumption.component.css']
})
export class ConsumptionComponent 
{

  color: any = 'rgba(206, 27, 14, 0.7)';

  ngOnInit()
  {
      this.createBarChart();
  }

  historyflag : boolean = true;
  predictionFlag : boolean = false;

  historyClick()
  {
    this.historyflag = true;
    var historyDiv = document.getElementById("history");
    if(historyDiv)  { historyDiv.style.color = "black"; }
    
    this.predictionFlag = false;
    var predictionDiv = document.getElementById("prediction");
    if(predictionDiv)  { predictionDiv.style.color = "gray";}
  }

  predictionClick(){
    this.historyflag = false;
    var historyDiv = document.getElementById("history");
    if(historyDiv)  { historyDiv.style.color = "gray"; }
    
    this.predictionFlag = true;
    var predictionDiv = document.getElementById("prediction");
    if(predictionDiv)  { predictionDiv.style.color = "black";}
  }

  chart: any;
  createBarChart()
  {
    const Canvas: any = document.getElementById("chart");
    const chart2d = Canvas.getContext("2d");
    const chart = new Chart(chart2d, {
      type: 'bar',
      data: {
        labels: Array.from({ length: 24 }, (_, i) => (0+i+"h").toString()),
        datasets: [{
          label: 'Hourly Consumption (kWh)',
          data: [1.250, 1.292, 1.333, 1.375, 1.417, 1.458, 1.500, 1.542, 1.583, 1.625, 1.667, 1.625, 1.583, 1.542, 1.500, 1.458, 1.417, 1.375, 1.333, 1.292, 1.250, 1.208, 1.167, 1.125, 1.083, 1.042, 1.083, 1.125, 1.167, 1.208, 1.250],
          backgroundColor: this.color,           
          // borderColor: 'rgba(28, 109, 163, 1)',
          // borderWidth: 1
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


//'rgba(75, 175, 242, 0.2)',  