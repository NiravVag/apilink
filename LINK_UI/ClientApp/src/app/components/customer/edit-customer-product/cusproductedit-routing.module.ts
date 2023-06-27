import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerProductComponent } from './editcustomerproduct.component';


const routes: Routes = [
  {
    path: 'edit-customer-product/:id',
    component: EditCustomerProductComponent
  },
  {
    path: 'edit-customer-product/:id/:type',
    component: EditCustomerProductComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusproducteditRoutingModule { }
