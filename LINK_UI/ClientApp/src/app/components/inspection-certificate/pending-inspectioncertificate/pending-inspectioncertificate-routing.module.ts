import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PendingInspectioncertificateComponent } from './pending-inspectioncertificate.component';


const routes: Routes = [
  {
    path:'',
    component:PendingInspectioncertificateComponent
  },
  {
    path:'pending-ic',
    component:PendingInspectioncertificateComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PendingInspectioncertificateRoutingModule {
  
 }
