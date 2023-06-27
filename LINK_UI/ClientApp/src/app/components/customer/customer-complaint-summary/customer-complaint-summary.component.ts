import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComplaintSummaryModel, ComplaintSummaryRequestModel } from 'src/app/_Models/customer/customer-complaint.model';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common'
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerComplaintService } from 'src/app/_Services/customer/customercomplaint.service';
import { AdvanceSearchType, APIService, complaintAdvanceSearchtypelst, complaintsummaryDatetypelst, generalComplaintsummaryDatetypelst, DefaultDateType, extrafeeSearchtypelst, ListSize, PageSizeCommon, SearchType, Url, GeneralDefaultDateType } from '../../common/static-data-common';
import { TranslateService } from '@ngx-translate/core';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customer-complaint-summary',
  templateUrl: './customer-complaint-summary.component.html',
  styleUrls: ['./customer-complaint-summary.component.scss'],

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
export class CustomerComplaintSummaryComponent extends SummaryComponent<ComplaintSummaryRequestModel> {

  componentDestroyed$: Subject<boolean> = new Subject;
  model: ComplaintSummaryRequestModel;
  summaryModel: ComplaintSummaryModel;
  searchtypelst: any = extrafeeSearchtypelst;
  datetypelst: any = complaintsummaryDatetypelst;
  defaultDatetypelst: any = generalComplaintsummaryDatetypelst;
  bookingAdvanceSearchtypelst: any = complaintAdvanceSearchtypelst;
  _customvalidationforbookid: boolean = false;
  _searchtypeid = SearchType.BookingNo;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  modelRef: NgbModalRef;
  showProductPoAdvSrch: boolean = false;
  showBookingDateType: boolean = false;
  public exportDataLoading = false;

  constructor(router: Router, route: ActivatedRoute, public validator: Validator, private refService: ReferenceService,
    private service: CustomerComplaintService, translate: TranslateService, private customerService: CustomerService, public utility: UtilityService,
    private modalService: NgbModal) {
    super(router, validator, route, translate);
  }

  getData() {
    this.getSearchData();
  }
  getPathDetails(): string {
    return "cuscomplaintedit/edit-customercomplaint/";
  }

  onInit(): void {
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.objectInitialize();
    this.getService();
    this.getComplaintTypeData();
    this.getCustomerListBySearch();
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  objectInitialize() {
    this.selectedPageSize = PageSizeCommon[0];
    this.model = new ComplaintSummaryRequestModel();
    this.summaryModel = new ComplaintSummaryModel();
    this.validator.setJSON("customer/customer-complaint-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = this.showBookingDateType ? DefaultDateType.ServiceDate : GeneralDefaultDateType.ApplyDate;
    this.model.advancedSearchtypeid = AdvanceSearchType.ProductName;
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchtypeid = searchtype;
  }

  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }

  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }

  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this._searchtypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && ((isNaN(Number(this.model.searchtypetext))) || (this.model.searchtypetext.trim().length > 9));
  }

  IsDateValidationRequired(): boolean {
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }

  serviceTypeValidationRequired(): boolean {
    // let isOk;
    // if (this.model.searchtypetext != null) {
    //   isOk = this.validator.isValid('serviceId');
    // }

    // if (this.model.datetypeid == DefaultDateType.ServiceDate && (this.model.fromdate != null && this.model.todate != null)) {
    //   isOk = this.validator.isValid('serviceId');
    // }

    return true;
  }

  getService() {
    this.summaryModel.serviceLoading = true;
    this.refService.getServiceList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.summaryModel.serviceList = response.dataSourceList;
        }
        else {
          this.summaryModel.serviceList = [];
        }
        this.summaryModel.serviceLoading = false;
      },
        error => {
          this.summaryModel.serviceList = [];
          this.summaryModel.serviceLoading = false;
        });
  }

  getComplaintTypeData() {
    this.summaryModel.complaintTypeLoading = true;
    this.service.getComplaintType()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.complaintTypeList = response.complaintDataList;
          else
            this.error = response.result;
          this.summaryModel.complaintTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.summaryModel.complaintTypeLoading = false;
        });
  }

  clearDateInput(controlName: any) {
    this.validator.isSubmitted = false;
    switch (controlName) {
      case "fromdate": {
        this.model.fromdate = null;
        break;
      }
      case "todate": {
        this.model.todate = null;
        break;
      }
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  getSearchData() {

    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.summaryModel.searchLoading = true;

      this.service.getComplaintSummary(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          if (res.result == ResponseResult.Success) {
            this.mapPageProperties(res);
            this.model.items = res.data;
            this.model.noFound = false;
          }
          else if (res.result == 2) {
            this.model.noFound = true;
            this.model.items = [];
          }
          this.summaryModel.searchLoading = false;
        },
          error => {
            this.summaryModel.searchLoading = false;
            this.model.noFound = true;
            this.model.items = [];
          });
    }
  }

  SearchDetails() {
    this.validator.initTost();
    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.getSearchData();
  }

  reset() {
    this.showProductPoAdvSrch = false;
    this.showBookingDateType = false
    this.objectInitialize();
    this.getService();
    this.getComplaintTypeData();
    this.getCustomerListBySearch();
  }

  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "Complaint_Summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Complaint_Summary.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  formValid(): boolean {
    var res = this.validator.isValid('complaintTypeId');
    let isOk = this.validator.isValid('complaintTypeId') && !this._customvalidationforbookid && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired());

    if (isOk && this.model.searchtypetext != null && this.model.searchtypetext != "") {
      isOk = this.validator.isValid('serviceId');
    }

    if (isOk && this.model.datetypeid == DefaultDateType.ServiceDate) {
      isOk = this.validator.isValid('serviceId');
    }

    if (isOk && this.model.advancedsearchtypetext && !this.model.customerId) {
      isOk = this.validator.isValid('customerid');
    }
    return isOk;
  }

  getCustomerListBySearch() {

    this.summaryModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryModel.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.summaryModel.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.customerList = data;
        this.summaryModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryModel.requestCustomerModel.searchText = this.summaryModel.customerInput.getValue();
      this.summaryModel.requestCustomerModel.skip = this.summaryModel.customerList.length;
    }

    this.summaryModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.summaryModel.requestCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.customerList = this.summaryModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.summaryModel.requestCustomerModel.skip = 0;
          this.summaryModel.requestCustomerModel.take = ListSize;
        }
        this.summaryModel.customerLoading = false;
      }),
      error => {
        this.summaryModel.customerLoading = false;
        this.setError(error);
      };
  }

  //open pop up
  openConfirm(id, content) {

    this.summaryModel.deleteId = id;

    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  delete() {
    this.summaryModel.deleteLoading = true;
    this.service.delete(this.summaryModel.deleteId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.summaryModel.deleteLoading = false;
            this.modelRef.close();
            this.getSearchData();
            this.showSuccess('COMPLAINT_SUMMARY.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
          }
          else {
            this.showError('COMPLAINT_SUMMARY.LBL_TITLE', 'COMPLAINT_SUMMARY.MSG_DELETE_SUCCESS');
            this.summaryModel.deleteLoading = false;
            this.modelRef.close();
          }
        },
        error => {
          this.setError(error);
          this.showError('COMPLAINT_SUMMARY.LBL_TITLE', 'COMMON.MSG_CANNOT_DELETE');
          this.summaryModel.deleteLoading = false;
        });
  }

  redirectRegisterPage(id) {
    this.getDetails(id);
  }

  redirectToBookingEditPage(bookingId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
  }

  changeComplaintType() {
    if (this.model.complaintTypeId == 2) {
      this.showProductPoAdvSrch = false;
      this.model.advancedsearchtypetext = null;
      this.showBookingDateType = false;
      this.model.datetypeid = GeneralDefaultDateType.ApplyDate;
    }
    else if (this.model.complaintTypeId == 1 && this.model.serviceId == APIService.Inspection) {
      this.showProductPoAdvSrch = true;
    }
    if (this.model.complaintTypeId == 1) {
      this.showBookingDateType = true;
      this.model.datetypeid =  DefaultDateType.ServiceDate;
    }
  }

  changeService() {
    if (this.model.serviceId != APIService.Inspection) {
      this.showProductPoAdvSrch = false;
      this.model.advancedsearchtypetext = null;
    }
    else {
      this.showProductPoAdvSrch = true;
    }
  }

  clearComplaintType() {
    this.showProductPoAdvSrch = true;
    this.showBookingDateType = false;
    this.model.advancedsearchtypetext = null;
  }

  clearService() {
    this.showProductPoAdvSrch = false;
    this.model.advancedsearchtypetext = null;
  }
}
