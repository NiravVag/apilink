import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectioncertificateSummaryComponent } from './inspectioncertificate-summary.component';


const routes: Routes = [
  {
    path:'',
    component:InspectioncertificateSummaryComponent
  },
  {
    path:'ic-summary',
    component:InspectioncertificateSummaryComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspectioncertificateSummaryRoutingModule {
  
 }
