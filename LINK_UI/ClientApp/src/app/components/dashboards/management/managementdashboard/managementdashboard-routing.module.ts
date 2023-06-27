import { debugOutputAstAsTypeScript } from '@angular/compiler';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{ManagementdashboardComponent} from './managementdashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: ManagementdashboardComponent
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManagementdashboardRoutingModule { }
