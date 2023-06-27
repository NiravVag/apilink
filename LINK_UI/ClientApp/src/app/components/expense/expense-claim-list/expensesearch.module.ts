import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExpensesearchRoutingModule } from './expensesearch-routing.module';
import { ExpenseClaimListComponent } from './expense-claim-list.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [ExpenseClaimListComponent],
  imports: [
    CommonModule,
    ExpensesearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class ExpensesearchModule { 

}
