import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-weather-widget',
  templateUrl: './weather-widget.component.html',
  styleUrls: ['./weather-widget.component.css']
})
export class WeatherWidgetComponent implements OnInit {

    WeatherData: any;
    constructor() {}

    ngOnInit() {
        this.getWeatherData();
        console.log(this.WeatherData);
    }

    getWeatherData()
    {
      let data = JSON.parse("asfasf");
      this.setWeatherData(data);
    }

    setWeatherData(data: any)
    {
       this.WeatherData = data;

       let sunsetTime = new Date(this.WeatherData.sys.sunset * 1000);
       this.WeatherData.sunset_time = sunsetTime.toLocaleTimeString();
       let currentDate = new Date();
       this.WeatherData.isDay = (currentDate.getTime() < sunsetTime.getTime());
       this.WeatherData.temp = this.WeatherData.main.temp;
       this.WeatherData.temp_min = this.WeatherData.main.temp_min;
       this.WeatherData.temp_max = this.WeatherData.main.temp_max;
       this.WeatherData.temp_feels_like = this.WeatherData.main.temp_feels_like;
    }
}
