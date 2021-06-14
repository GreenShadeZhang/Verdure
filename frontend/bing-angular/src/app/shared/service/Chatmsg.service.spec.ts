/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ChatmsgService } from './Chatmsg.service';

describe('Service: Chatmsg', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ChatmsgService]
    });
  });

  it('should ...', inject([ChatmsgService], (service: ChatmsgService) => {
    expect(service).toBeTruthy();
  }));
});
