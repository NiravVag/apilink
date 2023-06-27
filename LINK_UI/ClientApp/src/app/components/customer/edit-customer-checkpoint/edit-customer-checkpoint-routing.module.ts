import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerCheckpointComponent } from './edit-customer-checkpoint.component';


const routes: Routes = [
  {
    path: 'edit-customercheckpoint',
    component: EditCustomerCheckpointComponent
  },
  {
    path: 'edit-customercheckpoint/:id',
    component: EditCustomerCheckpointComponent
  },
  {
    path: '',
    component: EditCustomerCheckpointComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditCustomerCheckPointRoutingModule { }
