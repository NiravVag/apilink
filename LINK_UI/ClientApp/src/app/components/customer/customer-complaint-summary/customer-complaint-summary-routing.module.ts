import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerComplaintSummaryComponent } from './customer-complaint-summary.component';

const routes: Routes = [
    {
        path: '',
        component: CustomerComplaintSummaryComponent
    },
    {
        path: 'customer-complaint-summary',
        component: CustomerComplaintSummaryComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CustomerComplaintSummaryRoutingModule{

}