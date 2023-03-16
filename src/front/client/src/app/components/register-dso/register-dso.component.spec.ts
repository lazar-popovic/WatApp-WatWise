import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterDSOComponent } from './register-dso.component';

describe('RegisterDSOComponent', () => {
  let component: RegisterDSOComponent;
  let fixture: ComponentFixture<RegisterDSOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterDSOComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterDSOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
