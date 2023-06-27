import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CancelAuditComponent } from './cancel-audit.component';


const routes: Routes = [
  {
    path: 'cancel-audit/:id',
    component: CancelAuditComponent
  },
  {
    path: 'cancel-audit/:id/:type',
    component: CancelAuditComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditcancelRoutingModule { }
