import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EmailSendSummaryComponent } from "./email-send-summary.component";

const routes: Routes = [
    {
        path: 'summary',
        component: EmailSendSummaryComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmailSendSummaryRoutingModule {}