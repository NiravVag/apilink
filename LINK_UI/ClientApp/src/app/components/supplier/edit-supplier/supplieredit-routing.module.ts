import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditSupplierComponent } from './edit-supplier.component';


const routes: Routes = [
  {
    path: '',
    component: EditSupplierComponent
  },
  {
    path: 'edit-supplier/:id',
    component: EditSupplierComponent
  },
  {
    path: 'view-supplier/:id',
    component: EditSupplierComponent
  },
  {
    path: 'new-supplier',
    component: EditSupplierComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SuppliereditRoutingModule { }
