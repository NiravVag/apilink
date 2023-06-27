import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingRoutingModule } from './landing-routing-module';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../common/translate.share.module';
import { LandingComponent } from './landing/landing.component';

@NgModule({
  declarations: [ LandingComponent],
  imports: [
    CommonModule,
    LandingRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class LandingModule {

 }
