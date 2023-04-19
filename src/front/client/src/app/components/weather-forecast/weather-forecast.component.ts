import { Component, OnInit } from '@angular/core';
import { WeatherForecastService, Weather, Forecast } from '../../services/weather-forecast.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.css']
})
export class WeatherForecastComponent implements OnInit {

  currentWeather: Weather | undefined;
  forecasts: Forecast[] = [];

  busyWeather: Subscription | undefined;

  constructor(private weatherService: WeatherForecastService) {
  }

  ngOnInit() {
    this.weatherService.getWeather().subscribe((response: any) => {
        console.log( this.currentWeather);
        this.currentWeather = new Weather(
          response.name,
          response.weather[0].description,
          `https://openweathermap.org/img/w/${response.weather[0].icon}.png`,
          response.main.temp,
          response.main.temp_min,
          response.main.temp_max,
          response.main.humidity,
          response.wind.speed
        );
        console.log( this.currentWeather);
      });

    this.weatherService.getForecast().subscribe((response: any) => {
      console.log( this.forecasts);
      const forecast = [];
      for (let i = 0; i < response.list.length; i += 8) {
        const item = response.list[i];
        forecast.push(new Forecast(
          new Date(item.dt_txt),
          item.weather[0].description,
          `https://openweathermap.org/img/w/${item.weather[0].icon}.png`,
          item.main.temp,
          item.main.temp_min,
          item.main.temp_max,
          item.main.humidity,
          item.wind.speed
        ));
      }
      this.forecasts = forecast;
      console.log( this.forecasts);
    });
  }
}
