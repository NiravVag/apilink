import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TcfDetailComponent } from './tcf-detail.component';


const routes: Routes = [
  {
    path:'',
    component:TcfDetailComponent
  },
  {
    path:'tcf-detail',
    component:TcfDetailComponent
  },
  {
    path:'tcf-detail/:tcfId',
    component:TcfDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TcfDetailRoutingModule { }
