import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerServiceConfigComponent } from './edit-customerserviceconfig.component';


const routes: Routes = [
  {
    path: 'edit-customerserviceconfig/:id',
    component: EditCustomerServiceConfigComponent
  },
  {
    path: 'edit-customerserviceconfig/:id/:customerid',
    component: EditCustomerServiceConfigComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusserviceeditRoutingModule { }
