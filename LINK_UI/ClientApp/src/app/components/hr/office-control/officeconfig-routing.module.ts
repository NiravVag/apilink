import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OfficeControlComponent } from './officecontrol.component';


const routes: Routes = [
  {
    path: '',
    component: OfficeControlComponent
  },
  {
    path: 'office-control',
    component: OfficeControlComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfficeconfigRoutingModule { }
