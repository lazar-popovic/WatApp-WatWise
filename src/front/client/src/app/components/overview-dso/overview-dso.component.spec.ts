import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverviewDsoComponent } from './overview-dso.component';

describe('OverviewDsoComponent', () => {
  let component: OverviewDsoComponent;
  let fixture: ComponentFixture<OverviewDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OverviewDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OverviewDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
