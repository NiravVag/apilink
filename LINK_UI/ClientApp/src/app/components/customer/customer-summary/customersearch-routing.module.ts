import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerSummaryComponent } from './customersummary.component';


const routes: Routes = [
  {
    path:'',
    component:CustomerSummaryComponent
  },
  {
    path:'customer-summary',
    component:CustomerSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersearchRoutingModule { }
