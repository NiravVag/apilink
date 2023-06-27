import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartingPortComponent } from './starting-port.component';

describe('StartingPortComponent', () => {
  let component: StartingPortComponent;
  let fixture: ComponentFixture<StartingPortComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StartingPortComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StartingPortComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
