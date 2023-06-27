import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExchangeRateComponent } from './exchangerate.component';


const routes: Routes = [
  {
    path: '',
    component: ExchangeRateComponent
  },
  {
    path: 'edit-exchange',
    component: ExchangeRateComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExchangerateRoutingModule { }
