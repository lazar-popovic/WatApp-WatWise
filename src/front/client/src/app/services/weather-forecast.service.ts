import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Weather } from '../Models/Weather';

@Injectable({
  providedIn: 'root',
})
export class WeatherForecastService {
  private API_KEY = 'e39e528663e833d20fe7c7aba6813e5e';
  private units = 'metric';
  private lang = 'en';

  constructor(private http: HttpClient) {}

  getWeather(city: string) {
    return this.http
      .get(
        `https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${this.API_KEY}&units=${this.units}&lang=${this.lang}&exclude=minutely`
      )
      .toPromise()
      .then((response: any) => {
        return new Weather(
          response.weather[0].description,
          `http://openweathermap.org/img/w/${response.weather[0].icon}.png`,
          response.main.temp,
          response.main.temp_min,
          response.main.temp_max,
          response.main.humidity,
          response.main.wind
        );
      })
      .catch((error: any) => {
        console.log(error);
        return null;
      });
  }
}
