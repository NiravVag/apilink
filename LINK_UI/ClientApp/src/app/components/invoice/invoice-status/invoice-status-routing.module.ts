import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvoiceStatusComponent } from './invoice-status.component';

const routes: Routes = [
  {
    path:'',
    component:InvoiceStatusComponent
  },
  {
    path:'invoice-summary',
    component:InvoiceStatusComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceStatusRoutingModule { }
