import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SupplierSummaryComponent } from './suppliersummary.component';


const routes: Routes = [
  {
    path: 'supplier-summary',
    component: SupplierSummaryComponent
  },
  {
    path: '',
    component: SupplierSummaryComponent
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SuppliersearchRoutingModule { }
