import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationLoginRoutingModule } from './authentication-login.routing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login.component';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    AuthenticationLoginRoutingModule,
    FormsModule,     
    NgSelectModule,
    TranslateSharedModule.forRoot() 
  ]
})
export class AuthenticationLoginModule {}
