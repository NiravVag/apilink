import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FullbridgeSummaryComponent } from './fullbridge-summary.component';

const routes: Routes = [
  {
    path:'',
    component:FullbridgeSummaryComponent
  },
  {
    path:'fullbridge-summary',
    component:FullbridgeSummaryComponent
  },
  {
    path:'fullbridge-summary/:id',
    component:FullbridgeSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FullbridgesummaryRoutingModule { }
