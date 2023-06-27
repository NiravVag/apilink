import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SaleInvoiceSummaryComponent } from './sale-invoice-summary.component';

const routes: Routes = [
  {
    path: '',
    component: SaleInvoiceSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SaleInvoiceSummaryRoutingModule { }
