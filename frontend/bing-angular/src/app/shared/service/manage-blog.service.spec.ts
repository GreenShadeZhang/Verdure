/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ManageBlogService } from './manage-blog.service';

describe('Service: ManageBlog', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ManageBlogService]
    });
  });

  it('should ...', inject([ManageBlogService], (service: ManageBlogService) => {
    expect(service).toBeTruthy();
  }));
});
