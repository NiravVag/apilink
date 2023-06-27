import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerDepartmentComponent } from './customerdepartment.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerDepartmentComponent
  },
  {
    path: 'customer-department',
    component: CustomerDepartmentComponent
  },
  {
    path: 'customer-department/:id',
    component: CustomerDepartmentComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerdepRoutingModule { }
