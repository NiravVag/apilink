import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DefectParetoComponent } from './defect-pareto.component';

describe('DefectParetoComponent', () => {
  let component: DefectParetoComponent;
  let fixture: ComponentFixture<DefectParetoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DefectParetoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DefectParetoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
