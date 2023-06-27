import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CSAllocationComponent } from './cs-allocation.component';
// import { CsAllocationComponent } from './cs-allocation.component';


const routes: Routes = [
  {
    path: '',
    component: CSAllocationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CsAllocationRoutingModule { }
