import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BookingDetailComponent } from './booking-detail.component';


const routes: Routes = [
  {
    path:'',
    component:BookingDetailComponent
  },
  {
    path:'booking-detail',
    component:BookingDetailComponent
  },
  
  {
    path:'booking-detail/:bookingId/:reportId/:containerId', 
    component:BookingDetailComponent
  }  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookingDetailRoutingModule { }
