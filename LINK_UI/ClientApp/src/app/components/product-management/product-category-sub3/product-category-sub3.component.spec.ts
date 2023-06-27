import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCategorySub3Component } from './product-category-sub3.component';

describe('ProductCategorySub3Component', () => {
  let component: ProductCategorySub3Component;
  let fixture: ComponentFixture<ProductCategorySub3Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductCategorySub3Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCategorySub3Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
