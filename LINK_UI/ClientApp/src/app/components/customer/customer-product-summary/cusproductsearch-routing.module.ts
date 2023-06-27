import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerProductSummaryComponent } from './customerproductsummary.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerProductSummaryComponent
  },
  {
    path: 'customer-productsummary',
    component: CustomerProductSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusproductsearchRoutingModule { }
