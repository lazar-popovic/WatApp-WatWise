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

  getWeather() :Observable<any>
  {
    return this.http.get(`${environment.apiUrl}weather/current`);
  }

  getForecast() :Observable<any>
  {
    return this.http.get(`${environment.apiUrl}weather/forecast`);
  }
}

export { Weather, Forecast };

