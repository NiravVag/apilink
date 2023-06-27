import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerPriceCardSummaryComponent } from './customer-price-card-summary.component';


const routes: Routes = [
  {
    path: 'price-card-summary',
    component: CustomerPriceCardSummaryComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerPriceCardSummaryRoutingModule { }
