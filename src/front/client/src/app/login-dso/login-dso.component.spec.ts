import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginDSOComponent } from './login-dso.component';

describe('LoginDSOComponent', () => {
  let component: LoginDSOComponent;
  let fixture: ComponentFixture<LoginDSOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginDSOComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginDSOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
