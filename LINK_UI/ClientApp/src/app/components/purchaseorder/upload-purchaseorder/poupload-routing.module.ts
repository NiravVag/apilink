import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadPurchaseorderComponent } from './upload-purchaseorder.component';


const routes: Routes = [
  {
    path: 'uplaod-purchaseorder',
    component: UploadPurchaseorderComponent
  },
  {
    path: '',
    component: UploadPurchaseorderComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PouploadRoutingModule { }
