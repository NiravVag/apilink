import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{QcdashboardComponent} from './qcdashboard.component'

const routes: Routes = [
  {
    path: '',
    component: QcdashboardComponent
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
export class QcdashboardRoutingModule { }
