import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationNavigationComponent } from './authentication-navigation.component';

const routes: Routes = [
  {
    path:'',
    component:AuthenticationNavigationComponent
  },
  {
    path:'authentic',
    component:AuthenticationNavigationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationNavigationRoutingModule { }
