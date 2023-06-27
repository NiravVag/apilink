import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleRightConfigurationComponent } from './role-right-configuration.component';


const routes: Routes = [
  {
    path: 'role-right-configuration',
    component: RoleRightConfigurationComponent
  },  
  {
    path: '',
    component: RoleRightConfigurationComponent
  },  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolerightRoutingModule { }
