import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { AuthenticationNavigationComponent } from './authentication-navigation.component';
import { AuthenticationNavigationRoutingModule } from './authentication-navigation.routing';

@NgModule({
  declarations: [AuthenticationNavigationComponent],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    AuthenticationNavigationRoutingModule,
    FormsModule,     
    NgSelectModule,
    TranslateSharedModule.forRoot() 
  ]
})
export class AuthenticationNavigationModule {}
