import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MandayUtilizationDashboardComponent } from './manday-utilization-dashboard.component';


const routes: Routes = [
  {
    path: '',
    component: MandayUtilizationDashboardComponent
  },
  {
    path: 'manday-utilization-dashboard',
    component: MandayUtilizationDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MandayUtilizationDashboardRoutingModule { }
