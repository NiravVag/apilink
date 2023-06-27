import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ScheduleSummaryComponent } from './schedule-summary/schedule-summary.component';
import {ScheduleAllocationComponent} from './schedule-allocation/schedule-allocation.component';
import { QcAvailabilityComponent } from './qc-availability/qc-availability.component';
import { EditQcBlockComponent } from './edit-qc-block/edit-qc-block.component';
import { QcBlockSummaryComponent } from './qc-block-summary/qc-block-summary.component';

const routes: Routes = [
  {
    path: '',
    component: ScheduleSummaryComponent
  },
  {
    path: 'schedule-summary',
    component: ScheduleSummaryComponent
  },
  {
    path: 'schedule-summary/:id',
    component: ScheduleSummaryComponent
  },
  {
    path: 'schedule-allocation',
    component: ScheduleAllocationComponent
  },
  {
    path: 'schedule-allocation/:id',
    component: ScheduleAllocationComponent
  },
  {
    path: 'schedule-pending',
    component: ScheduleSummaryComponent
  },
  {
    path: 'qc-availability',
    component: QcAvailabilityComponent
  },
  {
    path: 'edit-qc-block',
    component: EditQcBlockComponent
  },
  {
    path: 'edit-qc-block/:id',
    component: EditQcBlockComponent
  },
  {
    path: 'qc-block-summary',
    component: QcBlockSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ScheduleRoutingModule { }
