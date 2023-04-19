import { Component, OnInit } from '@angular/core';
import { WeatherForecastService, Weather, Forecast } from '../../services/weather-forecast.service';


@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.css']
})
export class WeatherForecastComponent implements OnInit {

  currentWeather: Weather | null = null;
  forecasts: Forecast[] = [];

  constructor(private weatherService: WeatherForecastService) {
   // this.currentWeather = {};
  }

  ngOnInit() {
    this.weatherService.getWeather()
      .then((currentWeather: Weather | null) => {
        if(currentWeather)
          this.currentWeather = currentWeather;
        else
          console.log("PUKLO GET Weather");
      });
    this.weatherService.getForecast()
      .then((forecasts: Forecast[] | null) => {
        if(forecasts)
          this.forecasts = forecasts;
        else
          console.log("PUKLO GET Forecast");
      });
  }
}
