import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditCusReportComponent } from './audit-cus-report.component';

const routes: Routes = [
  {
    path:'',
    component:AuditCusReportComponent
  },
  {
    path:'audit-cus-report',
    component:AuditCusReportComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditCusReportRoutingModule { }
