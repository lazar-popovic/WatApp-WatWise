import { Component, OnInit } from '@angular/core';
import { WeatherForecastService} from '../../services/weather-forecast.service';
import { Subscription } from 'rxjs';
import { Weather } from 'src/app/Models/Weather';
import { Forecast } from 'src/app/Models/Forecast';


@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.css']
})
export class WeatherForecastComponent implements OnInit {

  currentWeather: any;
  forecasts: any;
  isLoadedW: boolean = false;
  isLoadedF: boolean = false;

  busyWeather: Subscription | undefined;

  constructor(private weatherService: WeatherForecastService) {}

  ngOnInit() {
    this.weatherService.getWeather().subscribe((response: any) => {
      console.log(this.currentWeather);
        this.currentWeather = response;/*new Weather(
          response.name,
          response.weather[0].description,
          `https://openweathermap.org/img/w/${response.weather[0].icon}.png`,
          response.main.temp,
          response.main.temp_min,
          response.main.temp_max,
          response.main.humidity,
          response.wind.speed
        );*/
        console.log(this.currentWeather);
        this.isLoadedW = true;
      });

    this.weatherService.getForecast().subscribe((response: any) => {
      this.forecasts = response.list.filter((data: any) => {
        return data.dt_txt.includes('12:00:00');
      /*console.log( this.forecasts);
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
        ));*/
      });
      //this.forecasts = forecasts;
      this.isLoadedF = true;
      console.log( this.forecasts);
    });
  }
}
