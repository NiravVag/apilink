import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{CusdashboardComponent} from './cusdashboard.component'
import {QuotationSummaryComponent} from '../../../quotation/quotation-summary/quotationsummary.component'

const routes: Routes = [
  {
    path: '',
    component: CusdashboardComponent
}
/* ,
{
  path: 'quotation/dashboardquotation-summary/type',
  component: QuotationSummaryComponent
} */
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CusdashboardRoutingModule { }
