import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerComponent } from './edit-customer.component';


const routes: Routes = [
  {
    path: '',
    component: EditCustomerComponent
  },
  {
    path: 'edit-customer',
    component: EditCustomerComponent
  },
  {
    path: 'edit-customer/:id',
    component: EditCustomerComponent
  },
  {
    path: 'view-customer/:id',
    component: EditCustomerComponent
  },
  {
    path: 'new-customer',
    component: EditCustomerComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomereditRoutingModule { }
