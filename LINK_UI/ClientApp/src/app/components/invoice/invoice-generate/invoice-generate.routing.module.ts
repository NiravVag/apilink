import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvoiceGenerateComponent } from './invoice-generate.component';

const routes: Routes = [
  {
    path:'',
    component:InvoiceGenerateComponent
  },
  {
    path:'invoice-generate',
    component:InvoiceGenerateComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceGenerateRoutingModule { }
