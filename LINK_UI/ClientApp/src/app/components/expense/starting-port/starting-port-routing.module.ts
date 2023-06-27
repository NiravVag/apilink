import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StartingPortComponent } from './starting-port.component';


const routes: Routes = [
  {
    path: '',
    component: StartingPortComponent
  },
  {
    path: 'starting-port',
    component: StartingPortComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StartingPortRoutingModule { }
