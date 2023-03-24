/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ToastrNotifService } from './toastr-notif.service';

describe('Service: ToastrNotif', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ToastrNotifService]
    });
  });

  it('should ...', inject([ToastrNotifService], (service: ToastrNotifService) => {
    expect(service).toBeTruthy();
  }));
});
