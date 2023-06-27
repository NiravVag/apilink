import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerContactComponent } from './edit-customercontact.component';


const routes: Routes = [
  {
    path: '',
    component: EditCustomerContactComponent
  },
  {
    path: 'edit-customer-contact',
    component: EditCustomerContactComponent
  },
  {
    path: 'edit-customer-contact/:id',
    component: EditCustomerContactComponent
  },
  {
    path: 'view-customer-contact/:id',
    component: EditCustomerContactComponent
  },
  {
    path: 'add-customer-contact/:id/:type',
    component: EditCustomerContactComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CuscontacteditRoutingModule { }
