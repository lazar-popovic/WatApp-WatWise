import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevicesInfoComponent } from './devices-info.component';

describe('DevicesInfoComponent', () => {
  let component: DevicesInfoComponent;
  let fixture: ComponentFixture<DevicesInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevicesInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DevicesInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
