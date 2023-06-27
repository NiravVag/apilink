import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExtraFeesSummaryComponent } from './extra-fees-summary.component';


const routes: Routes = [
  {
    path: '',
    component: ExtraFeesSummaryComponent
  },
  {
    path: 'summary',
    component: ExtraFeesSummaryComponent
  },
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExtraFeesSummaryRoutingModule { }
