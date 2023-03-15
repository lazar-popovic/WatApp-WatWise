import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnergyUsageComponent } from './energy-usage.component';

describe('EnergyUsageComponent', () => {
  let component: EnergyUsageComponent;
  let fixture: ComponentFixture<EnergyUsageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnergyUsageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnergyUsageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
