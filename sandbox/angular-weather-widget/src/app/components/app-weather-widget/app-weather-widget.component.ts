import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-weather-widget',
  templateUrl: './app-weather-widget.component.html',
  styleUrls: ['./app-weather-widget.component.css']
})
export class AppWeatherWidgetComponent implements OnInit {

  currentWeather: any;
  forecast: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    const apiKey = 'e39e528663e833d20fe7c7aba6813e5e';
    const city = 'Kragujevac';
    const currentWeatherUrl = `https://api.openweathermap.org/data/2.5/weather?q=${city}&units=metric&appid=${apiKey}`;
    const forecastUrl = `https://api.openweathermap.org/data/2.5/forecast?q=${city}&units=metric&appid=${apiKey}`;

    this.http.get(currentWeatherUrl).subscribe((response: any) => {
      this.currentWeather = response;
    });

    this.http.get(forecastUrl).subscribe((response: any) => {
      this.forecast = response.list.filter((data: any) => {
        return data.dt_txt.includes('12:00:00');
      });
    });
  }

}
