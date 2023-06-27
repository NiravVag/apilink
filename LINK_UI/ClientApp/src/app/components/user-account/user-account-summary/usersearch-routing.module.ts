import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserAccountSummaryComponent } from './user-account-summary.component';


const routes: Routes = [
  {
    path: '',
    component: UserAccountSummaryComponent
  },
  {
    path: 'user-account-summary',
    component: UserAccountSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersearchRoutingModule { }
