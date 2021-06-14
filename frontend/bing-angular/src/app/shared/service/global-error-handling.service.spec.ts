/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GlobalErrorHandlingService } from './global-error-handling.service';

describe('Service: GlobalErrorHandling', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlobalErrorHandlingService]
    });
  });

  it('should ...', inject([GlobalErrorHandlingService], (service: GlobalErrorHandlingService) => {
    expect(service).toBeTruthy();
  }));
});
