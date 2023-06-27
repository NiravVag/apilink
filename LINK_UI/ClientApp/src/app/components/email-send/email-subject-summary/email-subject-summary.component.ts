import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { SubConfigItem, SubConfigSummaryMasterModel, SubConfigSummaryModel } from 'src/app/_Models/email-send/subject-config-summary.model';
import { SubConfigResponseResult } from 'src/app/_Models/email-send/subject-config.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { EmailSubjectService } from 'src/app/_Services/email-send/email-subject.service';
import { Validator } from '../../common';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-email-subject-summary',
  templateUrl: './email-subject-summary.component.html',
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '24px',
        'padding-bottom': '24px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class EmailSubjectSummaryComponent extends SummaryComponent<SubConfigSummaryModel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  pagesizeitems = PageSizeCommon;
  isFilterOpen: boolean;
  selectedPageSize;
  masterModel: SubConfigSummaryMasterModel;
  private modelRef: NgbModalRef;
  toggleFormSection: boolean;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, private cusService: CustomerService, private emailSubService: EmailSubjectService,
    private modalService: NgbModal,public utility: UtilityService) {
    super(router, validator, route, translate, toastr);
    
    this.isFilterOpen = true;

    this.validator.setJSON("emailsend/subjectconfigsummary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(): void {
    this.model = new SubConfigSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.masterModel = new SubConfigSummaryMasterModel();

    this.getEmailTypeList();
    this.getModuleList();
  }
  
  ngAfterViewInit() {
    this.getCustomerListBySearch();

  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  reset() {
    this.onInit();
    this.getCustomerListBySearch();
  }

  getData(): void {
    this.getSearchData();
  }

  getPathDetails(): string {
    return "email/sub-config";
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.masterModel.customerModelRequest.searchText = this.masterModel.customerInput.getValue();
    this.masterModel.customerModelRequest.skip = this.masterModel.customerList.length;
   
    // if (this.masterModel.customerModelRequest.searchText == null && this.model.customerIds && this.model.customerIds.length > 0) {
    //   this.masterModel.customerModelRequest.idList = this.model.customerIds;
    // }
    // else {
    // }

      this.masterModel.customerModelRequest.idList = [];
    this.masterModel.customerModelRequest.customerId = 0;
    
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

    if( this.model.customerIds &&  this.model.customerIds.length >0) {
      this.masterModel.customerModelRequest.idList = this.model.customerIds;
    }
    
    this.masterModel.customerModelRequest.customerId = 0;
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

  //search api call
  getSearchData() {
    this.masterModel.searchLoading = true;
    this.model.noFound = false;
    this.emailSubService.search(this.model)
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == SubConfigResponseResult.Success) {
            this.mapPageProperties(response);
            this.model.items = response.data.map(x => {
              var item: SubConfigItem = {
                subConfigId: x.subConfigId,
                customerName: x.customerName,
                templateDisplayName: x.templateDisplayName,
                templateName: x.templateName,
                isDelete: x.isDelete,
                emailType:x.emailType,
                moduleName:x.moduleName
              }
              return item;
            });
            this.masterModel.searchLoading = false;
          }
          else if (response && response.result == SubConfigResponseResult.NotFound) {
            this.model.items = [];
            this.model.noFound = true;
            this.masterModel.searchLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;
        });
  }

  //search details
  SearchDetails() {
    this.validator.isSubmitted = true;
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  //open pop up
  openConfirm(id, content) {

    this.masterModel.deleteId = id;

    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  //remove the id assign
  getId() {
    this.masterModel.deleteId = null;
  }

  //delete the email config subject
  delete() {
    this.masterModel.deleteLoading = true;
    if (this.masterModel.deleteId > 0) {
      this.emailSubService.delete(this.masterModel.deleteId)
      .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response) {
              if (response.result == SubConfigResponseResult.Success) {
                this.showSuccess('EMAIL_SUBJECT_CONFIG_SUMMARY.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
                this.reset();
              }
              else if(response.result == SubConfigResponseResult.MappedEmailRule) {
                this.showWarning('EMAIL_SUBJECT_CONFIG_SUMMARY.LBL_TITLE', 'EMAIL_SUBJECT_CONFIG_SUMMARY.MSG_MAPPED_EMAIL_RULE');
              }
              else {
                this.showError('EMAIL_SUBJECT_CONFIG_SUMMARY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
              this.masterModel.deleteLoading = false;
              this.modelRef.close();
              this.getSearchData();
            }           
          },
          error => {
            this.setError(error);
            this.masterModel.deleteLoading = false;
          });
    }
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

   //redirect to form page
   redirectRegisterPage(id: number, newId: number) {
      this.getDetails(id, newId);
  }

  clearCustomer() {  
    this.getCustomerListBySearch();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }
}
