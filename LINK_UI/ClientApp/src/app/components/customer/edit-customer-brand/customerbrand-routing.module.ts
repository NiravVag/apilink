import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerBrandComponent } from './edit-customer-brand.component';


const routes: Routes = [
  {
    path: '',
    component: EditCustomerBrandComponent
  },
  {
    path: 'edit-customer-brand',
    component: EditCustomerBrandComponent
  },
  {
    path: 'edit-customer-brand/:id',
    component: EditCustomerBrandComponent
  },
  {
    path: 'view-customer-brand/:id',
    component: EditCustomerBrandComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerbrandRoutingModule { }
