import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TravelTariffComponent } from './travel-tariff.component';


const routes: Routes = [
  {
    path: '',
    component: TravelTariffComponent
  },
  {
    path: 'travel-tariff',
    component: TravelTariffComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TravelTariffRoutingModule { }
