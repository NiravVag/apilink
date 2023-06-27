import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LeaveRequestComponent } from './leave-request.component';


const routes: Routes = [
  {
    path: '',
    component: LeaveRequestComponent
  },
  {
    path: 'leave-request/:id',
    component: LeaveRequestComponent
  },
  {
    path: 'leave-request',
    component: LeaveRequestComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaverequestRoutingModule { }
