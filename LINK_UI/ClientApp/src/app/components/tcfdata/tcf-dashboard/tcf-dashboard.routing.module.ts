import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TcfDashboardComponent } from './tcf-dashboard.component';


const routes: Routes = [
  {
    path:'',
    component:TcfDashboardComponent
  },
  {
    path:'tcf-dashboard',
    component:TcfDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TcfDashboardRoutingModule { }
