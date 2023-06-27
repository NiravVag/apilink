import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SupplierScoreComponent } from './supplier-score.component';

const routes: Routes = [
  {
    path: 'supplier-score',
    component: SupplierScoreComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupplierScoreRoutingModule { }
