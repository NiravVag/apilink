import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataManagementSummary } from './data-management-summary.component';


const routes: Routes = [
  {
    path: '',
    component: DataManagementSummary
  },
  {
    path: 'dmsummary',
    component: DataManagementSummary
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DataManagementSummaryRoutingModule { }
