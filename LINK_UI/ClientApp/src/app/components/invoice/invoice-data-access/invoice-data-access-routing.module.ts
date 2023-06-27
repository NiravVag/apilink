import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvoiceDataAccessComponent } from './invoice-data-access.component';


const routes: Routes = [
  {
    path: '',
    component: InvoiceDataAccessComponent
  },
  {
    path: 'invoice-data-access',
    component: InvoiceDataAccessComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceDataAccessRoutingModule { }
