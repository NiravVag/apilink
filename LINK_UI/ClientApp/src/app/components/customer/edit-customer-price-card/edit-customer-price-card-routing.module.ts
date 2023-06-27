import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCustomerPriceCardComponent } from './edit-customer-price-card.component';


const routes: Routes = [
  {
    path: 'price-card',
    component: EditCustomerPriceCardComponent
  },
  {
    path: 'price-card/:id',
    component: EditCustomerPriceCardComponent
  },
  {
    path: 'price-card/:id/:newid',
    component: EditCustomerPriceCardComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditCustomerPriceCardRoutingModule { }
