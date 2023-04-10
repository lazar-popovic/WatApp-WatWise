import { Component } from '@angular/core';
import {ChartData, ChartDataset, ChartOptions, registerables} from 'chart.js';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-production',
  templateUrl: './production.component.html',
  styleUrls: ['./production.component.css']
})
export class ProductionComponent 
{
  color: any = 'rgba(62, 234, 50, 0.8)';

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

  predictionClick()
  {
    this.historyflag = false;
    var historyDiv = document.getElementById("history");
    if(historyDiv)  { historyDiv.style.color = "gray"; }
    
    this.predictionFlag = true;
    var predictionDiv = document.getElementById("prediction");
    if(predictionDiv)  { predictionDiv.style.color = "black";}
  }

  day1Flag : boolean = true;
  day2Flag : boolean = false;
  day3Flag : boolean = false;

  day1Click()
  {
      this.day1Flag  = true; this.day2Flag  = false; this.day3Flag = false;
      
      var day1 = document.getElementById("day1");
      if(day1)
      {
        day1.style.color = "#1676AC";
        day1.style.backgroundColor =  "#ffff";
        day1.style.padding = "5px";
        day1.style.borderRadius = "10px";
      }

      var day2 = document.getElementById("day2");
      if(day2){ day2.style.backgroundColor = "transparent"; day2.style.color = "#ffff";}

      var day3 = document.getElementById("day3");
      if(day3){ day3.style.backgroundColor = "transparent"; day3.style.color = "#ffff";}

      /*Create chart*/
  }

  day2Click()
  {
      this.day1Flag  = false; this.day2Flag  = true; this.day3Flag = false;
      
      var day1 = document.getElementById("day1");
      if(day1){ day1.style.backgroundColor = "transparent"; day1.style.color = "#ffff";}

      var day2 = document.getElementById("day2");
      if(day2)
      {
        day2.style.color = "#1676AC";
        day2.style.backgroundColor =  "#ffff";
        day2.style.padding = "5px";
        day2.style.borderRadius = "10px";
      }

      var day3 = document.getElementById("day3");
      if(day3){ day3.style.backgroundColor = "transparent"; day3.style.color = "#ffff";}

      /*Create chart*/
  }

  day3Click()
  {
      this.day1Flag  = false; this.day2Flag  = false; this.day3Flag = true;
      
      var day1 = document.getElementById("day1");
      if(day1){ day1.style.backgroundColor = "transparent"; day1.style.color = "#ffff";}

      var day2 = document.getElementById("day2");
      if(day2){ day2.style.backgroundColor = "transparent"; day2.style.color = "#ffff";}
      
      var day3 = document.getElementById("day3");
      if(day3)
      {
        day3.style.color = "#1676AC";
        day3.style.backgroundColor =  "#ffff";
        day3.style.padding = "5px";
        day3.style.borderRadius = "10px";
      }

      /*Create chart*/
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
