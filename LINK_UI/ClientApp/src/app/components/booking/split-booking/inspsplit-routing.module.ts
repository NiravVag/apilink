import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SplitBookingComponent } from './split-booking.component';


const routes: Routes = [
  {
    path: 'split-booking/:id',
    component: SplitBookingComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspsplitRoutingModule { }
