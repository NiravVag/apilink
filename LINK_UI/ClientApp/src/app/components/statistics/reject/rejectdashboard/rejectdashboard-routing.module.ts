import { debugOutputAstAsTypeScript } from '@angular/compiler';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{RejectdashboardComponent} from './rejectdashboard.component';
const routes: Routes = [
  {
    path: 'dashboard',
    component: RejectdashboardComponent
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RejectdashboardRoutingModule { }
