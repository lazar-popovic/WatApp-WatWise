import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoHolderComponent } from './logo-holder.component';

describe('LogoHolderComponent', () => {
  let component: LogoHolderComponent;
  let fixture: ComponentFixture<LogoHolderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogoHolderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LogoHolderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
