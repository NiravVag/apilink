import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { EmailSubjectModule, PreDefinedFieldDataType, SubConfigColumnModel, SubConfigResponseResult, SubjectConfigMasterModel, SubjectConfigModel, TemplatePopup } from 'src/app/_Models/email-send/subject-config.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { DetailComponent } from '../../common/detail.component';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { EmailSubjectService } from 'src/app/_Services/email-send/email-subject.service';
import { Validator } from '../../common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';

@Component({
  selector: 'app-edit-email-subject',
  templateUrl: './edit-email-subject.component.html',
  styleUrls: ['./edit-email-subject.component.scss']
})
export class EditEmailSubjectComponent extends DetailComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  @ViewChild('myDiv') myDiv: ElementRef;
  @ViewChild('myDivFilter') myDivFilter: ElementRef;
  model: SubjectConfigModel;
  masterModel: SubjectConfigMasterModel;
  templatePopup: TemplatePopup;
  modelRef: NgbModalRef;
  copyId: number;
  _preDefinedFieldDataType = PreDefinedFieldDataType;
  _emailSubjectModule = EmailSubjectModule;

  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    private cusService: CustomerService, private emailSubService: EmailSubjectService,
    public validator: Validator, public modalService: NgbModal, public utility: UtilityService,
    public referenceService: ReferenceService, private activatedRoute: ActivatedRoute,) {

    super(router, route, translate, toastr, modalService);

    this.validator.setJSON("emailsend/subjectconfig.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.copyId = parseInt(this.activatedRoute.snapshot.paramMap.get("newid"));
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(id?: number) {

    this.model = new SubjectConfigModel();
    this.masterModel = new SubjectConfigMasterModel();
    this.templatePopup = new TemplatePopup();

    if (id && id > 0) {
      this.edit(id);
    }
    else {
      this.getCustomerListBySearch();
      this.getFieldColumnList();
      this.getDelimiterList();
      this.getEmailTypeList();
      this.getModuleList();
    }

    this.getDateFormats();
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {
    this.masterModel.customerModelRequest.searchText = this.masterModel.customerInput.getValue();
    this.masterModel.customerModelRequest.skip = this.masterModel.customerList.length;

    if (this.model.customerId && this.model.customerId > 0) {
      this.masterModel.customerModelRequest.id = this.model.customerId;
    }
    else {
      this.masterModel.customerModelRequest.id = null;
    }

    this.masterModel.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(data);
        }
        this.masterModel.customerModelRequest = new CommonDataSourceRequest();
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 customer on load
  getCustomerListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.masterModel.customerModelRequest.id = this.model.customerId;
    }
    else {
      this.masterModel.customerModelRequest.id = null;
    }

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest, term)
        : this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //get pre defined columns
  getFieldColumnList() {
    this.masterModel.FieldLoading = true;
    this.emailSubService.getFieldColumnList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.fieldColumnList = response.dataSourceList;

          if (this.model.templateColumnList && this.model.templateColumnList.length > 0) {
            this.getColumnListRemainField();
          }
          this.masterModel.FieldLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.FieldLoading = false;
        });
  }

  //get column list remaining fields
  getColumnListRemainField() {
    var tempcolumnList: SubConfigColumnModel[] = [];

    this.masterModel.fieldColumnList.forEach(element => {
      if (this.model.templateColumnList.findIndex(x => x.id === element.id) === -1) {
        tempcolumnList.push(element);
      }
    });
    this.masterModel.fieldColumnList = tempcolumnList;
  }

  //is form valid
  isformValid(): boolean {
    var isOk = this.validator.isValid('templateName') &&
      this.validator.isValid('emailTypeId') &&
      this.validator.isValid('delimiterId') &&
      this.validator.isValid('moduleId');

    // this.model.templateColumnList.forEach(element => {
    //   if (element.maxChar <= 0) {
    //     this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.LBL_MAX_CHAR_GREATER_THAN_ZERO");
    //     return !isOk;
    //   }
    // });

    if (isOk && (!this.model.templateColumnList || !(this.model.templateColumnList.length > 0))) {
      isOk = !isOk;
      this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.MSG_TEMP_COLUMN_LIST_REQ");
    }
    return isOk;
  }

  clearCustomer() {
    this.getCustomerListBySearch();
  }

  //assign selected delimiter on change event
  changeDelimiter(event) {
    if (event)
      this.model.delimiter = event.name;
  }

  //save the details
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isformValid()) {
      if (this.model.id == null || this.copyId == 0) {
        this.model.id = 0;
      }
      this.masterModel.saveLoading = true;
      this.emailSubService.save(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response) {
              if (response.result == SubConfigResponseResult.Success) {
                // if (this.fromSummary) {
                this.return('emailsub/summary');
                // }
                // else {
                //   this.edit(response.id);
                // }
                this.showSuccess('EMAIL_SUBJECT_CONFIG.LBL_TITLE', 'EMAIL_SUBJECT_CONFIG.MSG_SAVE_SUCCESS');
              }
              else if (response.result == SubConfigResponseResult.TemplateNameExists) {
                this.showWarning('EMAIL_SUBJECT_CONFIG.LBL_TITLE',
                  this.model.templateName + this.utility.textTranslate('EMAIL_SUBJECT_CONFIG.MSG_TEMP_NAME_EXISTS'));
              }
              else if (response.result == SubConfigResponseResult.TemplateFieldsExists) {
                this.showWarning('EMAIL_SUBJECT_CONFIG.LBL_TITLE', 'EMAIL_SUBJECT_CONFIG.MSG_TEMP_NAME_DETAILS_EXISTS');
              }
              else {
                this.showWarning('EMAIL_SUBJECT_CONFIG.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
            }
            this.masterModel.saveLoading = false;
          },
          error => {
            if (error && error.error && error.error.errors && error.error.statusCode == 400) {
              let validationErrors: [];
              validationErrors = error.error.errors;
              this.openValidationPopup(validationErrors);
            }
            else {
              this.showError("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterModel.saveLoading = false;
          });
    }
  }

  //edit the details
  edit(id: number) {
    this.emailSubService.edit(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.model = response.editDetails;

            //disable the control if already details used
            this.masterModel.isUseByEmailSend = this.copyId == 0 ? false : response.isUseByEmailSend;

            this.getFieldColumnList();
            this.getCustomerListBySearch();
            this.getDelimiterList();
            this.getEmailTypeList();
            this.getModuleList();
          }
        },
        error => {
          this.setError(error);
        });
  }

  /* Event when drag and drop item between predefined column list and template column list */
  drop(event: CdkDragDrop<string[]>, isTemplateColumn = false) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  /**Open popup edit template Name, width, sequence */
  editTemplateColumn(item, content) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });

    var templateData = this.model.templateColumnList.find(x => x.id == item.id);

    if (templateData && templateData.id > 0) {
      this.masterModel.templateId = item.id;
      this.templatePopup.maxChar = templateData.maxChar;
      this.templatePopup.isTitle = templateData.isTitle;
      this.templatePopup.titleCustomName = templateData.titleCustomName;
      this.templatePopup.maxItems = templateData.maxItems;
      this.templatePopup.dateFormat = templateData.dateFormat;
      this.templatePopup.isDateSeperator = !templateData.isDateSeperator;
      this.templatePopup.dataType = templateData.dataType;
      this.masterModel.popupTitle = templateData.name + ' ' + this.utility.textTranslate("EMAIL_SUBJECT_CONFIG.LBL_COLUMN_CONFIG");
    }
  }

  closeViewPopUp() {

    //this.masterModel.maxChar = null;
    this.templatePopup = new TemplatePopup();

    this.modelRef.close();
  }

  popupOk() {
    if (this.masterModel.templateId > 0) {

      var templateData = this.model.templateColumnList.find(x => x.id == this.masterModel.templateId);

      if (this.popupValid() && templateData) {
        templateData.maxChar = this.templatePopup.maxChar;
        templateData.isTitle = this.templatePopup.isTitle;
        templateData.titleCustomName = this.templatePopup.titleCustomName;
        if (templateData.dataType == this._preDefinedFieldDataType.List)
          templateData.maxItems = this.templatePopup.maxItems;
        if (templateData.dataType == this._preDefinedFieldDataType.Date) {
          templateData.dateFormat = this.templatePopup.dateFormat;
          templateData.isDateSeperator = !this.templatePopup.isDateSeperator;
        }
        this.modelRef.close();
        this.templatePopup = new TemplatePopup();
      }
    }

  }

  popupValid() {
    if (this.templatePopup.dataType != this._preDefinedFieldDataType.Date && (!this.templatePopup.maxChar || this.templatePopup.maxChar <= 0)) {
      this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.LBL_MAX_CHAR_GREATER_THAN_ZERO");
      return false;
    }
    else if (this.templatePopup.isTitle && !this.templatePopup.titleCustomName) {
      this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.MSG_TITLE_CUSTOM_NAME_REQ");
      return false;
    }
    else if (this.templatePopup.dataType == this._preDefinedFieldDataType.Date && !this.templatePopup.dateFormat) {
      this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.MSG_DATE_FORMAT_REQ");
      return false;
    }
    else if (this.templatePopup.dataType == this._preDefinedFieldDataType.List && !this.templatePopup.maxItems) {
      this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.MSG_MAX_ITEMS_REQ");
      return false;
    }
    return true;
  }


  getEditPath() {
    return "";
  }

  getViewPath() {
    return "";
  }

  //get email type list
  getEmailTypeList() {
    this.masterModel.emailTypeLoading = true;
    this.emailSubService.getEmailTypeList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.emailTypeList = response.dataSourceList;

          this.masterModel.emailTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.emailTypeLoading = false;
        });
  }

  //get module list
  getModuleList() {
    this.masterModel.moduleLoading = true;
    this.emailSubService.getModuleList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.moduleList = response.dataSourceList;

          this.masterModel.moduleLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.moduleLoading = false;
        });
  }

  //get delimiter values
  getDelimiterList() {
    this.masterModel.delimiterLoading = true;
    this.referenceService.getDelimiterList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.delimiterList = response.delimiterList;
          //assign delimiter list to filtered list  
          this.masterModel.filteredDelimiterList = this.masterModel.delimiterList;
          //if module id is filename then filter only file delimiter list
          if (this.model.moduleId == EmailSubjectModule.FileName)
            this.masterModel.filteredDelimiterList = this.masterModel.delimiterList.filter(x => x.isFile);

          this.masterModel.delimiterLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.delimiterLoading = false;
        });
  }

  reset() {
    this.model = new SubjectConfigModel();
    this.masterModel.isUseByEmailSend = false;
    this.masterModel.fieldColumnList = [];
    this.validator.isSubmitted = false;
    this.getFieldColumnList();
  }

  //get the date formats
  getDateFormats() {
    this.masterModel.dateFormatLoading = true;
    this.emailSubService.getDateFormats()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.dateFormats = response.dataSourceList;
          this.masterModel.dateFormatLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.dateFormatLoading = false;
        });
  }

  //assign the filtered demilter based on the module
  changeModule(event) {
    this.masterModel.filteredDelimiterList = this.masterModel.delimiterList;
    if (event && event.id == EmailSubjectModule.FileName)
      this.masterModel.filteredDelimiterList = this.masterModel.delimiterList.filter(x => x.isFile);

  }

  // getMaxCharData() {
  //   if(this.masterModel.maxChar || this.masterModel.maxChar <=0) {
  //     this.showWarning("EMAIL_SUBJECT_CONFIG.LBL_TITLE", "EMAIL_SUBJECT_CONFIG.LBL_MAX_CHAR_GREATER_THAN_ZERO");
  //   }
  // }

  // onDrop(event: DndDropEvent) {
  //   let index: number;

  //   for (var i = 0; i < this.masterModel.fieldColumnList.length; i++)
  //     if (this.masterModel.fieldColumnList[i].name == event.data.name) {
  //       index = i;
  //       break;
  //     }

  //   if (index >= 0) {
  //     this.masterModel.fieldColumnList.splice(index, 1);

  //     if (event.data) {
  //       let item: SubConfigColumnModel = {
  //         id: event.data.id,
  //         alaisName: event.data.alaisName,
  //         fieldId: event.data.fieldId,
  //         maxChar: event.data.maxChar,
  //         name: event.data.name,
  //         sort: event.data.sort
  //       };
  //       if (this.model.templateColumnList == null)
  //         this.model.templateColumnList = [];

  //       this.model.templateColumnList.push(item);
  //     }
  //   }
  // }

  // onDropLeft(event: DndDropEvent) {
  //   let index: number;
  //   for (var i = 0; i < this.model.templateColumnList.length; i++)
  //     if (this.model.templateColumnList[i].name == event.data.name) {
  //       index = i;
  //       break;
  //     }
  //   if (index >= 0) {
  //     this.model.templateColumnList.splice(index, 1);

  //     if (event.data) {
  //       let item: SubConfigColumnModel = {
  //         id: event.data.id,
  //         alaisName: event.data.alaisName,
  //         fieldId: event.data.fieldId,
  //         maxChar: event.data.maxChar,
  //         name: event.data.name,
  //         sort: event.data.sort
  //       };

  //       if (this.masterModel.fieldColumnList == null)
  //         this.masterModel.fieldColumnList = [];

  //       this.masterModel.fieldColumnList.push(item);
  //     }
  //   }
  // }

  //numeric validation for maxItems
  numericValidation(event) {
    var invalidChars = [
      "-",
      "+",
      "e",
      "E"
    ];
    if (invalidChars.includes(event.key) || event.target.value.length >= 5)
      event.preventDefault();
  }

}
