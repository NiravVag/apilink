import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReportSummaryComponent } from './report-summary.component';


const routes: Routes = [
  {
    path:'',
    component:ReportSummaryComponent
  },
  {
    path:'report-summary',
    component:ReportSummaryComponent
  },
  {
    path:'report-summary/:id',
    component:ReportSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsummaryRoutingModule { }
