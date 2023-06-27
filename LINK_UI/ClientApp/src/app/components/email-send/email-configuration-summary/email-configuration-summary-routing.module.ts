import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailConfigurationSummaryComponent } from './email-configuration-summary.component';

const routes: Routes = [
  {
    path: '',
    component: EmailConfigurationSummaryComponent
  },
  {
    path: 'summary',
    component: EmailConfigurationSummaryComponent
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailConfigurationSummaryRoutingModule { }
