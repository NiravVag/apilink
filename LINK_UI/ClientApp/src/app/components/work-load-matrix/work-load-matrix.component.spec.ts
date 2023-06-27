import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkLoadMatrixComponent } from './work-load-matrix.component';

describe('WorkLoadMatrixComponent', () => {
  let component: WorkLoadMatrixComponent;
  let fixture: ComponentFixture<WorkLoadMatrixComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkLoadMatrixComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkLoadMatrixComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
