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
  color: any = 'rgba(26, 55, 173, 0.7)';

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

  todayFlag : boolean = true;
  monthFlag : boolean = false;
  yearFlag : boolean = false;

  todayClick()
  {
      this.todayFlag  = true; this.monthFlag  = false; this.yearFlag = false;
      
      var today = document.getElementById("today");
      if(today)
      {
        today.style.color = "#1676AC";
        today.style.backgroundColor =  "#ffff";
        today.style.padding = "5px";
        today.style.borderRadius = "10px";
      }

      var month = document.getElementById("month");
      if(month){ month.style.backgroundColor = "transparent"; month.style.color = "#ffff";}

      var year = document.getElementById("year");
      if(year){ year.style.backgroundColor = "transparent"; year.style.color = "#ffff";}

      /*Create chart*/
      this.createBarChart();
  }

  monthClick()
  {
      this.todayFlag  = false; this.monthFlag  = true; this.yearFlag = false;
      
      var today = document.getElementById("today");
      if(today){ today.style.backgroundColor = "transparent"; today.style.color = "#ffff";}

      var month = document.getElementById("month");
      if(month)
      {
        month.style.color = "#1676AC";
        month.style.backgroundColor =  "#ffff";
        month.style.padding = "5px";
        month.style.borderRadius = "10px";
      }

      var year = document.getElementById("year");
      if(year){ year.style.backgroundColor = "transparent"; year.style.color = "#ffff";}

      /*Create chart*/
      this.createBarChart();
  }

  yearClick()
  {
      this.todayFlag  = false; this.monthFlag  = false; this.yearFlag = true;
      
      var today = document.getElementById("today");
      if(today){ today.style.backgroundColor = "transparent"; today.style.color = "#ffff";}

      var month = document.getElementById("month");
      if(month){ month.style.backgroundColor = "transparent"; month.style.color = "#ffff";}
      
      var year = document.getElementById("year");
      if(year)
      {
        year.style.color = "#1676AC";
        year.style.backgroundColor =  "#ffff";
        year.style.padding = "5px";
        year.style.borderRadius = "10px";
      }

      /*Create chart*/
      this.createBarChart();
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
