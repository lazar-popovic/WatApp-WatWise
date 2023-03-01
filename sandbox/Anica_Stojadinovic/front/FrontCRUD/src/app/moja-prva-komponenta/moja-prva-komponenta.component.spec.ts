import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MojaPrvaKomponentaComponent } from './moja-prva-komponenta.component';

describe('MojaPrvaKomponentaComponent', () => {
  let component: MojaPrvaKomponentaComponent;
  let fixture: ComponentFixture<MojaPrvaKomponentaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MojaPrvaKomponentaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MojaPrvaKomponentaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
