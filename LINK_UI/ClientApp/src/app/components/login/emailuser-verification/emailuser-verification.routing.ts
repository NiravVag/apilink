import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailUserVerificationComponent } from './emailuser-verification.component';

const routes: Routes = [
  {
    path:'',
    component:EmailUserVerificationComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailUserVerificationRoutingModule { }
