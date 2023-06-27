import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InspectionOccupancyComponent } from './inspection-occupancy.component';

const routes: Routes = [{
  path: '',
  component: InspectionOccupancyComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspectionOccupancyRoutingModule { }
