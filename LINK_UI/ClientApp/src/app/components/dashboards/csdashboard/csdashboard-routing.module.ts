import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CsdashboardComponent } from './csdashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: CsdashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CsdashboardRoutingModule { }
