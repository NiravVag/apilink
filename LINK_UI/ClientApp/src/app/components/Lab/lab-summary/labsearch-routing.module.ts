import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LabSummaryComponent } from './lab-summary.component';


const routes: Routes = [
  {
    path:'',
    component:LabSummaryComponent
  },
  {
    path:'lab-summary',
    component:LabSummaryComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabsearchRoutingModule { }
