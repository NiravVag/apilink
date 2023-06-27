import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditSummaryComponent } from './audit-summary.component';


const routes: Routes = [
  {
    path:'',
    component:AuditSummaryComponent
  },
  {
    path:'audit-summary',
    component:AuditSummaryComponent
  },
  {
    path:'audit-summary/:id',
    component:AuditSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditsummaryRoutingModule { }
