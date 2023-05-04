import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayViewSettingsComponent } from './display-view-settings.component';

describe('DisplayViewSettingsComponent', () => {
  let component: DisplayViewSettingsComponent;
  let fixture: ComponentFixture<DisplayViewSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DisplayViewSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DisplayViewSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
