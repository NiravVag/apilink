import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailSubjectSummaryComponent } from './email-subject-summary.component';

const routes: Routes = [
  {
    path: '',
    component: EmailSubjectSummaryComponent
  },
  {
    path: 'summary',
    component: EmailSubjectSummaryComponent
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailSubjectSummaryRoutingModule { }
