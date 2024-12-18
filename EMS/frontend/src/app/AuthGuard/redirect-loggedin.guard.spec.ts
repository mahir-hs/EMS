import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { redirectLoggedinGuard } from './redirect-loggedin.guard';

describe('redirectLoggedinGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => redirectLoggedinGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
