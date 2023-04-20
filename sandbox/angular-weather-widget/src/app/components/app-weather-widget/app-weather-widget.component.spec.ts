import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppWeatherWidgetComponent } from './app-weather-widget.component';

describe('AppWeatherWidgetComponent', () => {
  let component: AppWeatherWidgetComponent;
  let fixture: ComponentFixture<AppWeatherWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppWeatherWidgetComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppWeatherWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
