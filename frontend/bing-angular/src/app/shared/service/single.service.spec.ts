/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SingleService } from './single.service';

describe('Service: Single', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SingleService]
    });
  });

  it('should ...', inject([SingleService], (service: SingleService) => {
    expect(service).toBeTruthy();
  }));
});
