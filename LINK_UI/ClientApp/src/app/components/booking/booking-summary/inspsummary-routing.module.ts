import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BookingSummaryComponent } from './booking-summary.component';


const routes: Routes = [
  {
    path:'',
    component:BookingSummaryComponent
  },
  {
    path:'booking-summary',
    component:BookingSummaryComponent
  },
  {
    path:'booking-summary/:id',
    component:BookingSummaryComponent
  },
  {
    path:'reinspection-booking/:type',
    component:BookingSummaryComponent
  }, 
  {
    path:'re-booking/:type',
    component:BookingSummaryComponent
  }, 
  {
    path:'quotation-pending/:type', 
    component:BookingSummaryComponent
  }, 
  {
    path:'quotation-pending/:type/:id', 
    component:BookingSummaryComponent
  },
  {
    path:'booking-pendingverification',
    component:BookingSummaryComponent
  },
  {
    path:'booking-pendingconfirmation',
    component:BookingSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspsummaryRoutingModule { }
