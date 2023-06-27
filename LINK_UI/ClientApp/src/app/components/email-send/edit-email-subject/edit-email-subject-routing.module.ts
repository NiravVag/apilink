import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditEmailSubjectComponent } from './edit-email-subject.component';

const routes: Routes = [
  {
    path: '',
    component: EditEmailSubjectComponent
  },
  {
    path: 'sub-config',
    component: EditEmailSubjectComponent
  },
  {
    path: 'sub-config/:id',
    component: EditEmailSubjectComponent
  },
  {
    path: 'sub-config/:id/:newid',
    component: EditEmailSubjectComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditEmailSubjectRoutingModule { }
