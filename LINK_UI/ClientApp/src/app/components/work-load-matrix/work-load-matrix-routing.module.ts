import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkLoadMatrixComponent } from './work-load-matrix.component';


const routes: Routes = [
  {
    path: 'summary',
    component: WorkLoadMatrixComponent
  }
  // ,{
  //   path: 'work-load-matrix-summary',
  //   component: WorkLoadMatrixComponent
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkLoadMatrixRoutingModule { }
