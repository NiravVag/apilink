import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from "@ng-bootstrap/ng-bootstrap";
import { NgSelectModule } from "@ng-select/ng-select";
import { TranslateSharedModule } from "../common/translate.share.module";
import { SharedModule } from "../shared/shared.module";
import { ValidationPopupComponent } from "../validation-popup/validation-popup.component";
import { ClaimRoutingModule } from "./claim-routing.module";
import { EditClaimComponent } from "./edit-claim/edit-claim.component";
import { ClaimSummaryComponent } from './claim-summary/claim-summary.component';
import { EditCreditNoteComponent } from './edit-credit-note/edit-credit-note.component';
import { PendingClaimSummaryComponent } from "./pending-claim-summary/pending-claim-summary.component";
import { CreditNoteSummaryComponent } from './credit-note-summary/credit-note-summary.component';


@NgModule({
  declarations: [EditClaimComponent, ClaimSummaryComponent, PendingClaimSummaryComponent, EditCreditNoteComponent, CreditNoteSummaryComponent],
  entryComponents: [ValidationPopupComponent],
  imports: [
    NgbModule,
    CommonModule,
    ClaimRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ClaimModule {

}