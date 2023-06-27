import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditPurchaseorderComponent } from './edit-purchaseorder.component';


const routes: Routes = [
  {
    path: '',
    component: EditPurchaseorderComponent
  },
  {
    path: 'edit-purchaseorder/:id',
    component: EditPurchaseorderComponent
  },
  {
    path: 'edit-purchaseorder',
    component: EditPurchaseorderComponent
  },
  {
    path: 'view-purchaseorder/:id',
    component: EditPurchaseorderComponent
  }, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PoeditRoutingModule { }
