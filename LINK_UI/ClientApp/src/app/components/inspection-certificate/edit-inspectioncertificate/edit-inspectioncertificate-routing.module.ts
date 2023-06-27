import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditInspectioncertificateComponent } from './edit-inspectioncertificate.component';

const routes: Routes = [
  {
    path: '',
    component: EditInspectioncertificateComponent
  },
  {
    path: 'edit-ic/:id',
    component: EditInspectioncertificateComponent
  },
  {
    path: 'new-ic',
    component: EditInspectioncertificateComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditInspectioncertificateRoutingModule { }
