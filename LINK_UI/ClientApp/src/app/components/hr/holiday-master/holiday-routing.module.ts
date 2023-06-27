import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HolidaymasterComponent } from './holidaymaster.component';


const routes: Routes = [
  {
    path: '',
    component: HolidaymasterComponent
  },
  {
    path: 'holiday-master',
    component: HolidaymasterComponent
  },
  {
    path: 'show-holiday',
    component: HolidaymasterComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HolidayRoutingModule { }
