import { TestBed, inject } from '@angular/core/testing';

import { DataServiceServiceService } from './data-service-service.service';

describe('DataServiceServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DataServiceServiceService]
    });
  });

  it('should be created', inject([DataServiceServiceService], (service: DataServiceServiceService) => {
    expect(service).toBeTruthy();
  }));
});
