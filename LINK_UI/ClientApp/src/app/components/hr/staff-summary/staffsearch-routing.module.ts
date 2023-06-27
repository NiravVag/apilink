import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StaffSummaryComponent } from './staffsummary.component';


const routes: Routes = [
  {
    path: '',
    component: StaffSummaryComponent,
  },
  {
    path: 'staff-summary',
    component: StaffSummaryComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffsearchRoutingModule { }
