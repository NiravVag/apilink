import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvoiceDiscountSummaryComponent } from './invoice-discount-summary.component';

const routes: Routes = [
  {
    path: '',
    component: InvoiceDiscountSummaryComponent
  },
  {
    path: 'invoice-discount-summary',
    component: InvoiceDiscountSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceDiscountSummaryRoutingModule { }
