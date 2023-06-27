import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditDashboardComponent } from './audit-dashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: AuditDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditDashboardRoutingModule { }
