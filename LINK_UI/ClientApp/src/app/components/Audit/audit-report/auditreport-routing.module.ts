import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditReportComponent } from './audit-report.component';


const routes: Routes = [
  {
    path: 'audit-report/:id',
    component: AuditReportComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditreportRoutingModule { }
