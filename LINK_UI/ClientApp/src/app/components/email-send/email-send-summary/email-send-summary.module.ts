import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgbDatepickerModule, NgbPaginationModule } from "@ng-bootstrap/ng-bootstrap";
import { NgSelectModule } from "@ng-select/ng-select";
import { TranslateModule } from "@ngx-translate/core";
import { TranslateSharedModule } from "../../common/translate.share.module";
import { SharedModule } from "../../shared/shared.module";
import { ValidationPopupComponent } from "../../validation-popup/validation-popup.component";
import { EmailSendSummaryRoutingModule } from "./email-send-summary-routing.module";
import { EmailSendSummaryComponent } from "./email-send-summary.component";

@NgModule({
    declarations: [EmailSendSummaryComponent],
    entryComponents:[ValidationPopupComponent],
    imports: [
      CommonModule,
      EmailSendSummaryRoutingModule,
      SharedModule,
      FormsModule,
      NgSelectModule,
      NgbPaginationModule,   
      NgbDatepickerModule, 
      TranslateSharedModule.forRoot()
    ]
  })

export class EmailSendSummaryModule {}