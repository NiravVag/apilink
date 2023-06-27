import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditDfCustomerConfigurationComponent } from './editdfcustomerconfiguration.component';



const routes: Routes = [
  {
    path: 'editdfcustomerconfig',
    component: EditDfCustomerConfigurationComponent
  },
  {
    path: 'editdfcustomerconfig/:id',
    component: EditDfCustomerConfigurationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditDfCustomerConfigurationRoutingModule { 

 

}
