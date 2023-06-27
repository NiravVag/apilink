import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TcfSummaryComponent } from './tcf-summary.component';


const routes: Routes = [
  {
    path: '',
    component: TcfSummaryComponent
  },
  {
    path: 'tcf-summary',
    component: TcfSummaryComponent
  },
  {
    path: 'tcf-summary/:pagetype/:tasktype',
    component: TcfSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TcfSummaryRoutingModule { }
