import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvoiceBankComponent } from './invoice-bank.component';

const routes: Routes = [
  {
    path:'',
    component:InvoiceBankComponent
  },
  {
    path:'invoice-bank-summary',
    component:InvoiceBankComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceBankRoutingModule { }
 