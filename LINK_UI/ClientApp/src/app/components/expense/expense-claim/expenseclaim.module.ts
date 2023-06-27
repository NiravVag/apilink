import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExpenseclaimRoutingModule } from './expenseclaim-routing.module';
import { ExpenseClaimComponent } from './expense-claim.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

@NgModule({
  declarations: [ExpenseClaimComponent],
  imports: [
    CommonModule,
    ExpenseclaimRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ExpenseclaimModule { 

}
