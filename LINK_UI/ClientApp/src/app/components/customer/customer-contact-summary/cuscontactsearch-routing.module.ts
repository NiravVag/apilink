import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerContactSummaryComponent } from './customercontactsummary.component';


const routes: Routes = [
  {
    path: '',
    component: CustomerContactSummaryComponent
  },
  {
    path: 'customer-contact-summary',
    component: CustomerContactSummaryComponent
  },
  {
    path: 'customer-contact-summary/:id',
    component: CustomerContactSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CuscontactsearchRoutingModule { }
