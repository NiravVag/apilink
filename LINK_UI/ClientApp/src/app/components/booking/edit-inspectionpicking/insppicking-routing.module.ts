import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditInspectionPickingComponent } from './edit-inspectionpicking.component';


const routes: Routes = [
  {
    path: 'edit-inspectionpicking/:id/:customerid/:statusId',
    component: EditInspectionPickingComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InsppickingRoutingModule { }
