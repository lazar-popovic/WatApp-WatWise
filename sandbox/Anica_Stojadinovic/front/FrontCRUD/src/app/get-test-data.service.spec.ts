import { TestBed } from '@angular/core/testing';

import { GetTestDataService } from './get-test-data.service';

describe('GetTestDataService', () => {
  let service: GetTestDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetTestDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
