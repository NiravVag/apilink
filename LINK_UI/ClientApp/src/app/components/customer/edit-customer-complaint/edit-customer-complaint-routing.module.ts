import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerComplaintComponent } from './edit-customer-complaint.component';


const routes: Routes = [
  {
    path: 'edit-customercomplaint',
    component: EditCustomerComplaintComponent
  },
  {
    path: 'edit-customercomplaint/:id',
    component: EditCustomerComplaintComponent
  },
  {
    path: '',
    component: EditCustomerComplaintComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditCustomerComplaintRoutingModule { }
