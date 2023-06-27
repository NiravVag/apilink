import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadPurchaseOrderLiteComponent } from './upload-purchaseorder-lite.component';


const routes: Routes = [
  {
    path: '',
    component: UploadPurchaseOrderLiteComponent
  },
  {
    path: 'upload-lite-purchaseorder',
    component: UploadPurchaseOrderLiteComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseOrderliteRoutingModule { }
