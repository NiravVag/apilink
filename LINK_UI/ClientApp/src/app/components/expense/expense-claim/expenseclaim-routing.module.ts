import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExpenseClaimComponent } from './expense-claim.component';


const routes: Routes = [
  {
    path: '',
    component: ExpenseClaimComponent
  },
  {
    path: 'expense-claim',
    component: ExpenseClaimComponent
  },
  {
    path: 'expense-claim/:id',
    component: ExpenseClaimComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExpenseclaimRoutingModule { }
