import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrimengEmployeeTableComponent } from './primeng-employee-table.component';

describe('PrimengEmployeeTableComponent', () => {
  let component: PrimengEmployeeTableComponent;
  let fixture: ComponentFixture<PrimengEmployeeTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrimengEmployeeTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrimengEmployeeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
