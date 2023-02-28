import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDevicesComponent } from './form-devices.component';

describe('FormDevicesComponent', () => {
  let component: FormDevicesComponent;
  let fixture: ComponentFixture<FormDevicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FormDevicesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
