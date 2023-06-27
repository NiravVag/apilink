import { InvoiceDiscountRegisterComponent } from './invoice-discount-register.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: InvoiceDiscountRegisterComponent
  },
  {
    path: 'invoice-discount-edit',
    component: InvoiceDiscountRegisterComponent
  },
  {
    path: 'invoice-discount-edit/:id',
    component: InvoiceDiscountRegisterComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceDiscountRegisterRoutingModule { }
