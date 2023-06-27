import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerBuyerComponent } from './customer-buyer.component';


const routes: Routes = [
  {
    path:'',
    component:CustomerBuyerComponent
  },
  {
    path:'customer-buyer',
    component:CustomerBuyerComponent
  },
  {
    path: 'customer-buyer/:id',
    component: CustomerBuyerComponent
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerbuyerRoutingModule { }
