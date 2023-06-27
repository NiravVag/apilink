import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomKpiComponent } from './custom-kpi.component';

const routes: Routes = [
  {
    path:'',
    component:CustomKpiComponent
  },
  {
    path:'custom-kpi',
    component:CustomKpiComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomKpiRoutingModule { }
