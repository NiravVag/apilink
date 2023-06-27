import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FoodAllowanceComponent } from './food-allowance.component';

describe('FoodAllowanceComponent', () => {
  let component: FoodAllowanceComponent;
  let fixture: ComponentFixture<FoodAllowanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FoodAllowanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FoodAllowanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
