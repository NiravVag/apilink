import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from "../../common/translate.share.module";

import { CustomerComplaintSummaryComponent } from "./customer-complaint-summary.component"
import { CustomerComplaintSummaryRoutingModule } from "./customer-complaint-summary-routing.module";

@NgModule({
    declarations: [CustomerComplaintSummaryComponent],
    imports: [CommonModule,
        CustomerComplaintSummaryRoutingModule,
        SharedModule,
        FormsModule,
        NgbPaginationModule,
        NgSelectModule,
        NgbDatepickerModule,
        TranslateSharedModule.forRoot()
    ]
})

export class CustomerComplaintSummaryModule{
    
}