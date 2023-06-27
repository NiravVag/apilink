import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditProvinceComponent } from './edit-province.component';

describe('EditProvinceComponent', () => {
  let component: EditProvinceComponent;
  let fixture: ComponentFixture<EditProvinceComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditProvinceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProvinceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
