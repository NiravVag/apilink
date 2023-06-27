import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DFCustomerConfigSummaryComponent } from './dfcustomerconfigsummary.component';


const routes: Routes = [
  {
    path:'',
    component:DFCustomerConfigSummaryComponent
  },
  {
    path:'dfcustomerconfig-summary',
    component:DFCustomerConfigSummaryComponent
  },
  {
    path:'dfcustomerconfig-summary/:id',
    component:DFCustomerConfigSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DFCustomerConfigSummaryRoutingModule { }