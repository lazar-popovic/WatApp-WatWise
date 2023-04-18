import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Weather } from '../Models/Weather';
import { Forecast } from '../Models/Forecast';

@Injectable({
  providedIn: 'root',
})
export class WeatherForecastService {
  private API_KEY = 'e39e528663e833d20fe7c7aba6813e5e';
  private units = 'metric';
  private lang = 'en';

  constructor(private http: HttpClient) {}

  getWeather(city: string): Promise<Weather | null> {
    return this.http
      .get(
        `https://cors-anywhere.herokuapp.com/http://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${this.API_KEY}&units=${this.units}&lang=${this.lang}&exclude=minutely`
      )
      .toPromise()
      .then((response: any) => {
        return new Weather(
          response.weather[0].description,
          `https://openweathermap.org/img/w/${response.weather[0].icon}.png`,
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

  getForecast(city: string): Promise<Forecast[] | null> {
    return this.http.get(`https://cors-anywhere.herokuapp.com/http://api.openweathermap.org/data/2.5/forecast?q=${city}&appid=${this.API_KEY}&units=${this.units}&lang=${this.lang}`)
      .toPromise()
      .then((response: any) => {
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
            item.main.wind
          ));
        }
        return forecast;
      })
      .catch((error: any) => {
        console.log(error);
        return null;
      });
  }
}

export { Weather, Forecast };

