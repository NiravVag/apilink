import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MandayDashboardComponent } from './manday-dashboard.component';


const routes: Routes = [
  {
    path: '',
    component: MandayDashboardComponent
  },
  {
    path: 'manday-dashboard',
    component: MandayDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MandayDashboardRoutingModule { }
