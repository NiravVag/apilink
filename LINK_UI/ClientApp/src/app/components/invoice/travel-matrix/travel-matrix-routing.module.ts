import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TravelMatrixComponent } from './travel-matrix.component';


const routes: Routes = [
  {
    path: '',
    component: TravelMatrixComponent
  },
  {
    path: 'travel-matrix',
    component: TravelMatrixComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TravelMatrixRoutingModule { }
