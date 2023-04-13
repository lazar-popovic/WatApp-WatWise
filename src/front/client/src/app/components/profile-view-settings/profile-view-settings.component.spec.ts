import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileViewSettingsComponent } from './profile-view-settings.component';

describe('ProfileViewSettingsComponent', () => {
  let component: ProfileViewSettingsComponent;
  let fixture: ComponentFixture<ProfileViewSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileViewSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileViewSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
