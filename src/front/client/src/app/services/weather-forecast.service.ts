import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Weather } from '../Models/Weather';
import { Forecast } from '../Models/Forecast';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WeatherForecastService {

  constructor(private http: HttpClient) {}

  getWeather() :Promise<any>
  {
    return this.http.get(`${environment.apiUrl}weather/current`)
      .toPromise()
      .then((response: any) => {
        return new Weather(
          response.name,
          response.weather[0].description,
          `https://openweathermap.org/img/w/${response.weather[0].icon}.png`,
          response.main.temp,
          response.main.temp_min,
          response.main.temp_max,
          response.main.humidity,
          response.wind.speed
        );
      })
      .catch((error: any) => {
        console.log(error);
        return null;
      });
  }

  getForecast() :Promise<any>
  {
    return this.http.get(`${environment.apiUrl}weather/forecast`)
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
            item.wind.speed
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

