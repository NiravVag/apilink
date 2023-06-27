import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { DragDropComponent } from './drag-drop/drag-drop.component';
import { SideFilterComponent } from './side-filter/side-filter.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { CustomerSideFilterComponent } from './customer-side-filter/customer-side-filter.component';
import { FileUploadComponent } from '../file-upload/file-upload.component';
import { ValidationPopupComponent } from '../validation-popup/validation-popup.component';
import { InvoicePreviewComponent } from './invoice-preview/invoice-preview.component';
import { TranslateModule } from '@ngx-translate/core';
import { EmailPreviewComponent } from './email-preview/email-preview.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { BlankPageComponent } from './blank-page/blank-page.component';
import { DynamicFormComponent } from '../dynamicfields/dynamic-form/dynamicform.component';
import { DynamicFormControlComponent } from '../dynamicfields/dynamic-controls/dynamiccontrols.component';

@NgModule({
  declarations: [BreadcrumbComponent,
    FileUploadComponent,
    DragDropComponent,
    SideFilterComponent,
    CustomerSideFilterComponent,
    InvoicePreviewComponent,
    ValidationPopupComponent,
    EmailPreviewComponent,
    BlankPageComponent,
    DynamicFormComponent,
    DynamicFormControlComponent
  ],
  exports: [BreadcrumbComponent,
    FileUploadComponent,
    DragDropComponent,
    SideFilterComponent,
    InvoicePreviewComponent,  
    CustomerSideFilterComponent,
    ValidationPopupComponent,
    EmailPreviewComponent,
    DynamicFormComponent,
    DynamicFormControlComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    ReactiveFormsModule,
    AngularEditorModule,
    TranslateModule.forRoot()
  ],
  providers: [NgbActiveModal]
})
export class SharedModule { }
