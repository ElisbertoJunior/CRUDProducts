import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth-service.service';

describe('AuthServiceService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });
});
