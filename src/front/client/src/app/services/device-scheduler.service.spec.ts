/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { DeviceSchedulerService } from './device-scheduler.service';

describe('Service: DeviceScheduler', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DeviceSchedulerService]
    });
  });

  it('should ...', inject([DeviceSchedulerService], (service: DeviceSchedulerService) => {
    expect(service).toBeTruthy();
  }));
});
