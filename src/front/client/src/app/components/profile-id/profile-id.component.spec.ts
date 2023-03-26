import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileIDComponent } from './profile-id.component';

describe('ProfileIDComponent', () => {
  let component: ProfileIDComponent;
  let fixture: ComponentFixture<ProfileIDComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileIDComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileIDComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
