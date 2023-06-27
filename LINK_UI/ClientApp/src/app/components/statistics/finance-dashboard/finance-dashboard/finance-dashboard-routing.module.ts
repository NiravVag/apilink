import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FinanceDashboardComponent } from './finance-dashboard.component';


const routes: Routes = [
  {
    path: 'dashboard',
    component: FinanceDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinanceDashboardRoutingModule { }
