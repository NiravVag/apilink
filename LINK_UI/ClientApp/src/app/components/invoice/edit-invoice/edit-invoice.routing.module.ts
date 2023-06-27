import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditInvoiceComponent } from './edit-invoice.component';

const routes: Routes = [
  {
    path:'',
    component:EditInvoiceComponent
  },
  {
    path: 'edit-invoice/:id',
    component: EditInvoiceComponent
  }, 
  {
    path: 'edit-invoice/:id/:service',
    component: EditInvoiceComponent
  },
  {
    path:'edit-invoice',
    component:EditInvoiceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditInvoiceRoutingModule { }
