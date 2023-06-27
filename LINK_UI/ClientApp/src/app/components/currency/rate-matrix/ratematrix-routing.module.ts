import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RateMatrixComponent } from './ratematrix.component';


const routes: Routes = [
  {
    path: '',
    component: RateMatrixComponent
  },
  {
    path: 'rate-matrix',
    component: RateMatrixComponent
  },
  {
    path: 'rate-matrix/:id/:from/:to/:type',
    component: RateMatrixComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RatematrixRoutingModule { }
