import { debugOutputAstAsTypeScript } from '@angular/compiler';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{QuantitativedashboardComponent} from './quantitativedashboard.component';
const routes: Routes = [
  {
    path: 'dashboard',
    component: QuantitativedashboardComponent
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuantitativedashboardRoutingModule { }
