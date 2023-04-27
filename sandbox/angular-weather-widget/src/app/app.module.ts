import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
//import { WeatherWidgetComponent } from './components/weather-widget/weather-widget.component';
import { AppWeatherWidgetComponent } from './components/app-weather-widget/app-weather-widget.component';

@NgModule({
  declarations: [
    AppComponent,
    //WeatherWidgetComponent,
    AppWeatherWidgetComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
