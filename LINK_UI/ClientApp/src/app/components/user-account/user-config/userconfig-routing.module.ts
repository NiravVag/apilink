import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserConfigComponent } from './user-config.component';


const routes: Routes = [
  {
    path: 'user-config/:id/:userId/:type',
    component: UserConfigComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserConfigRoutingModule { }
