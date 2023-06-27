import { TestBed, inject } from '@angular/core/testing';

import { ProductManagementService } from './productmanagement.service';

describe('ProductService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProductManagementService]
    });
  });

  it('should be created', inject([ProductManagementService], (service: ProductManagementService) => {
    expect(service).toBeTruthy();
  }));
});
