import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerStyleComponent } from './editcustomerstyle.component';


const routes: Routes = [
  {
    path: 'edit-customer-style/:id',
    component: EditCustomerStyleComponent
  },
  {
    path: 'edit-customer-style/:id/:type',
    component: EditCustomerStyleComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusstyleeditRoutingModule { }
