import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefectParetoComponent } from './defect-pareto.component';

const routes: Routes = [
  {
    path: 'defectpareto',
    component: DefectParetoComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DefectParetoRoutingModule { }
