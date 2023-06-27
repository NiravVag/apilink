import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerCollectionComponent } from './customer-collection.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerCollectionComponent
  },
  {
    path: 'edit-customer-collection',
    component: CustomerCollectionComponent
  },
  {
    path: 'edit-customer-collection/:id',
    component: CustomerCollectionComponent
  },
  {
    path: 'view-customer-collection/:id',
    component: CustomerCollectionComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerCollectionRoutingModule { }
