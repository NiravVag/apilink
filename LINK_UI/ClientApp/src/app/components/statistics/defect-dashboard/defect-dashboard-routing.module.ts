import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DefectDashboardComponent } from './defect-dashboard.component';


const routes: Routes = [
  {
    path: 'dashboard',
    component: DefectDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DefectDashboardRoutingModule { }
