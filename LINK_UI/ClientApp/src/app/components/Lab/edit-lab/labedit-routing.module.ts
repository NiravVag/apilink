import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditLabComponent } from './edit-lab.component';


const routes: Routes = [
  {
    path: '',
    component: EditLabComponent
  },
  {
    path: 'edit-lab/:id',
    component: EditLabComponent
  },
  {
    path: 'new-lab',
    component: EditLabComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabeditRoutingModule { }
