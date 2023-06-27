import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditEmailSendComponent } from './edit-email-send.component';

const routes: Routes = [
  {
    path: '',
    component: EditEmailSendComponent
  },
  {
    path: 'email-send',
    component: EditEmailSendComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditEmailSendRoutingModule { }
