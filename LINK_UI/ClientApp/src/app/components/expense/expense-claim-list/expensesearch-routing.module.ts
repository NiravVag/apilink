import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExpenseClaimListComponent } from './expense-claim-list.component';


const routes: Routes = [
  {
    path: '',
    component: ExpenseClaimListComponent
  },
  {
    path: 'expenseclaim-list',
    component: ExpenseClaimListComponent
  },
  {
    path: 'expenseclaim-approve',
    component: ExpenseClaimListComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExpensesearchRoutingModule { }
