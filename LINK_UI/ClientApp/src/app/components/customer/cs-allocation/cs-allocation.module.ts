import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { SharedModule } from '../../shared/shared.module';
import { CsAllocationRoutingModule } from './cs-allocation-routing.module';
import { CSAllocationComponent } from './cs-allocation.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    CSAllocationComponent
  ],
  imports: [
    CommonModule,
    CsAllocationRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CsAllocationModule { 

}
