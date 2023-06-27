import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditExtraFeesComponent } from './edit-extra-fees.component';


const routes: Routes = [
  {
    path: '',
    component: EditExtraFeesComponent
  },
  {
    path: 'edit-extra-fees',
    component: EditExtraFeesComponent
  },
  {
    path: 'edit-extra-fees/:id',
    component: EditExtraFeesComponent
  }  
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditExtraFeesRoutingModule { }
