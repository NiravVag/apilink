import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
import { PendingExpenseComponent } from './pending-expense.component';
import { PendingExpenseRoutingModule } from './pending-expense-routing.module';

 

@NgModule({
  declarations: [PendingExpenseComponent],
  imports: [
    CommonModule,
    PendingExpenseRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class PendingExpenseModule { 

}
