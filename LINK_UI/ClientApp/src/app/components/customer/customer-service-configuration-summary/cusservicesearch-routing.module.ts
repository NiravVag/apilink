import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerServiceConfigurationComponent } from './customer-serviceconfiguration.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerServiceConfigurationComponent
  },
  {
    path: 'customer-serviceconfig',
    component: CustomerServiceConfigurationComponent
  },
  {
    path: 'customer-serviceconfig/:id',
    component: CustomerServiceConfigurationComponent
  }
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusservicesearchRoutingModule { }
