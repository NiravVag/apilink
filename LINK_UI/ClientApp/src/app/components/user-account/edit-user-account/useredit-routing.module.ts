import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditUserAccountComponent } from './edit-user-account.component';


const routes: Routes = [
  {
    path: 'edit-user-account/:id/:type',
    component: EditUserAccountComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsereditRoutingModule { }
