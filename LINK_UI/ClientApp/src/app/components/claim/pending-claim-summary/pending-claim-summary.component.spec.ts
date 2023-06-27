import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PendingClaimSummaryComponent } from './pending-claim-summary.component';


describe('PendingClaimSummaryComponent', () => {
  let component: PendingClaimSummaryComponent;
  let fixture: ComponentFixture<PendingClaimSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PendingClaimSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingClaimSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
