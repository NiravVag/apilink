import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditEmailConfigurationComponent } from './edit-email-configuration.component';

const routes: Routes = [
  {
    path: 'config',
    component: EditEmailConfigurationComponent
  },
  {
    path: 'config/:id',
    component: EditEmailConfigurationComponent
  },
  {
    path: 'config/:id/:newid',
    component: EditEmailConfigurationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditEmailConfigurationRoutingModule { }
