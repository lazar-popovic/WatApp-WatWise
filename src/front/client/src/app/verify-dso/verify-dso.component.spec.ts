import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyDsoComponent } from './verify-dso.component';

describe('VerifyDsoComponent', () => {
  let component: VerifyDsoComponent;
  let fixture: ComponentFixture<VerifyDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
