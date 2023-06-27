import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerDecisionComponent } from './customer-decision-summary/customer-decision.component';
import {EditCustomerDecisionComponent} from './edit-customer-decision/edit-customer-decision/edit-customer-decision.component';

const routes:  Routes = [
    {
        path: 'customer-decision',
        component : CustomerDecisionComponent
    },
    {
        path: 'customer-decision/:customerId/:customerdecision',
        component : CustomerDecisionComponent
    },
    {
        path: 'edit-customer-decision',
        component : EditCustomerDecisionComponent
    },
    {
        path: 'edit-customer-decision/:bookingId',
        component : EditCustomerDecisionComponent
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class CustomerDecisionRoutingModule { }
