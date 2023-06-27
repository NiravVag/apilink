import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { KpiRoutingModule } from "../kpi/kpi-routing.module";
import { ClaimSummaryComponent } from "./claim-summary/claim-summary.component";
import { CreditNoteSummaryComponent } from "./credit-note-summary/credit-note-summary.component";
import { EditClaimComponent } from "./edit-claim/edit-claim.component";
import { EditCreditNoteComponent } from "./edit-credit-note/edit-credit-note.component";
import { PendingClaimSummaryComponent } from "./pending-claim-summary/pending-claim-summary.component";

const routes: Routes = [
    
    {
        path: '',
        component: EditClaimComponent
    },
    {
        path: 'edit-claim',
        component: EditClaimComponent
    },
    {
        path: 'edit-claim/:id',
        component: EditClaimComponent
    },
    {
        path: 'claim-summary',
        component: ClaimSummaryComponent
      },
      {
        path: 'pending-claim-summary',
        component: PendingClaimSummaryComponent
      },
      {
        path: 'credit-note-summary',
        component: CreditNoteSummaryComponent
      },
      {
        path: 'edit-credit-note',
        component: EditCreditNoteComponent
      },
      {
        path: 'edit-credit-note/:id',
        component: EditCreditNoteComponent
      }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports : [RouterModule]  
})
export class ClaimRoutingModule {}

