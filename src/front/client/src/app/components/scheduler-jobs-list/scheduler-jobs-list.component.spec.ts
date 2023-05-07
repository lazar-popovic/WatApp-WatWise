/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SchedulerJobsListComponent } from './scheduler-jobs-list.component';

describe('SchedulerJobsListComponent', () => {
  let component: SchedulerJobsListComponent;
  let fixture: ComponentFixture<SchedulerJobsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulerJobsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulerJobsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
