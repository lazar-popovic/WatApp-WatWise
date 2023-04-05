import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordViewSettingsComponent } from './password-view-settings.component';

describe('PasswordViewSettingsComponent', () => {
  let component: PasswordViewSettingsComponent;
  let fixture: ComponentFixture<PasswordViewSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PasswordViewSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordViewSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
