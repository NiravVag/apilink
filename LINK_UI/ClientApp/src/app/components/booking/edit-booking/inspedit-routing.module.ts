import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditBookingComponent } from './edit-booking.component';


const routes: Routes = [
  {
    path: 'edit-booking',
    component: EditBookingComponent
  }, 
  {
    path: 'edit-booking/:id',
    component: EditBookingComponent
  }, 
  {
    path: 'edit-booking/:id/:type',
    component: EditBookingComponent
  },
  {
    path:'editreinspection-booking/:id/:type',
    component:EditBookingComponent
  },
  {
    path:'editre-booking/:id/:type',
    component:EditBookingComponent
  },
  {
    path:'viewQuotebooking/:id/:type', 
    component:EditBookingComponent
  },  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspeditRoutingModule { }
