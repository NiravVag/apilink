import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerStyleSummaryComponent } from './customerstylesummary.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerStyleSummaryComponent
  },
  {
    path: 'customer-stylesummary',
    component: CustomerStyleSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusstylesearchRoutingModule { }
