import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditStaffComponent } from './edit-staff.component';


const routes: Routes = [
  {
    path: '',
    component: EditStaffComponent
  },
  {
    path: 'edit-staff/:id',
    component: EditStaffComponent,
  },
  {
    path: 'view-staff/:id',
    component: EditStaffComponent
  },
  {
    path: 'edit-staff',
    component: EditStaffComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffeditRoutingModule { }
