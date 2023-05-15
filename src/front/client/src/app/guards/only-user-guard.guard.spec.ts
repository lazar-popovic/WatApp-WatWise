import { TestBed } from '@angular/core/testing';

import { OnlyUserGuardGuard } from './only-user-guard.guard';

describe('OnlyUserGuardGuard', () => {
  let guard: OnlyUserGuardGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(OnlyUserGuardGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
