import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditSupplierLiteComponent } from './edit-supplier-lite.component';


const routes: Routes = [
  {
    path: '',
    component: EditSupplierLiteComponent
  },
  {
    path: 'new-lite-supplier',
    component: EditSupplierLiteComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupplierliteRoutingModule { }
