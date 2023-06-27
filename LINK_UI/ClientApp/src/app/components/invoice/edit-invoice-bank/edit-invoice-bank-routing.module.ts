import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditInvoiceBankComponent } from './edit-invoice-bank.component';


const routes: Routes = [
  {
    path: '',
    component: EditInvoiceBankComponent
  },
  {
    path: 'edit-invoice-bank',
    component: EditInvoiceBankComponent
  },
  {
    path: 'edit-invoice-bank/:id',
    component: EditInvoiceBankComponent
  }  
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditInvoiceBankRoutingModule { }
