import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SupFactDashboardComponent } from './sup-fact-dashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: SupFactDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupFactDashboardRoutingModule { }
