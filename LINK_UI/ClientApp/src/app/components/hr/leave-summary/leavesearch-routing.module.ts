import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LeaveSummaryComponent } from './leavesummary.component';


const routes: Routes = [
  {
    path: '',
    component: LeaveSummaryComponent
  },
  {
    path: 'leave-summary',
    component: LeaveSummaryComponent
  },
  {
    path: 'leave-approve',
    component: LeaveSummaryComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeavesearchRoutingModule { }
