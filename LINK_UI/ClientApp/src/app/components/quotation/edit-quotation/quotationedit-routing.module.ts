import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditQuotationComponent } from './edit-quotation.component';
import { QuotationSummaryComponent } from '../quotation-summary/quotationsummary.component';
const routes: Routes = [
  {
    path: 'new-quotation',
    component: EditQuotationComponent
  },
  {
    path: 'edit-quotation/:id',
    component: EditQuotationComponent
  },
  {
    path: 'quotation-summary',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-approve',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-confirm',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-clientpending',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-rejected',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-summary/:type/:customertasktype',
    component: QuotationSummaryComponent
  },
  {
    path: 'quotation-summary/:id',
    component: QuotationSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuotationeditRoutingModule { }
