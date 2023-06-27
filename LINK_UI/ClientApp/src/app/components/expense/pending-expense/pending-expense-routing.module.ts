import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PendingExpenseComponent } from './pending-expense.component';


const routes: Routes = [
  {
    path: '',
    component: PendingExpenseComponent
  },
  {
    path: 'pending-expense',
    component: PendingExpenseComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PendingExpenseRoutingModule { }
