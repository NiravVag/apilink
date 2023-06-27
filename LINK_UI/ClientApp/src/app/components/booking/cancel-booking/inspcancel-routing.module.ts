import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CancelBookingComponent } from './cancel-booking.component';


const routes: Routes = [
  {
    path: 'cancel-booking/:id',
    component: CancelBookingComponent
  },
  {
    path: 'cancel-booking/:id/:type',
    component: CancelBookingComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspcancelRoutingModule { }
