import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditInvoiceSendComponent } from './edit-invoice-send.component';

const routes: Routes = [
  {
    path: '',
    component: EditInvoiceSendComponent
  },
  {
    path: 'invoice-send',
    component: EditInvoiceSendComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditInvoiceSendRoutingModule { }
