import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManualInvoiceSummaryComponent } from './manual-invoice-summary.component';

const routes: Routes = [
  {
    path:'',
    component: ManualInvoiceSummaryComponent
  },
  {
    path:'manual-invoice-summary',
    component: ManualInvoiceSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManualInvoiceSummaryRoutingModule { }
