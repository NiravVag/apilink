import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RejectionRateComponent } from './rejection-rate.component';

const routes: Routes = [
  {
    path: 'rejection-rate',
    component: RejectionRateComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RejectionRateRoutingModule { }
