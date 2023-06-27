import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditManualInvoiceComponent } from './edit-manual-invoice.component';

const routes: Routes = [{
  path: '',
  component: EditManualInvoiceComponent
},
{
  path: 'manual-invoice-edit',
  component: EditManualInvoiceComponent
},
{
  path: 'manual-invoice-edit/:id',
  component: EditManualInvoiceComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditManualInvoiceRoutingModule { }
