import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CsAllocationComponent } from './cs-allocation.component';

describe('CsAllocationComponent', () => {
  let component: CsAllocationComponent;
  let fixture: ComponentFixture<CsAllocationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CsAllocationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CsAllocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
