import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerReportComponent } from './customer-reports/customer-report.component';

const routes: Routes = [
  {
    path: '',
    component: CustomerReportComponent
  },
  {
    path: 'customer-report',
    component: CustomerReportComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
