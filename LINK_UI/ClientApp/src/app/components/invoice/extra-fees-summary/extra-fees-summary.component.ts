import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Validator } from '../../common';
import { SummaryComponent } from '../../common/summary.component';
import { ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ExtraFeeItem, ExtraFeeSummaryModel, ExtraFeeSummaryRequestModel } from '../../../_Models/invoice/extrafeesummary.model';
import { TranslateService } from '@ngx-translate/core';
import { APIService, DefaultDateType, extrafeedatetypelst, extrafeeSearchtypelst, PageSizeCommon, SearchType, SupplierType } from '../../common/static-data-common';
import { CommonDataSourceRequest, ResponseResult } from '../../../_Models/common/common.model';
import { MandayDashboardService } from '../../../_Services/statistics/manday-dashboard.service';
import { debounceTime, distinctUntilChanged, first, switchMap, tap, catchError } from 'rxjs/operators';
import { QuotationService } from '../../../_Services/quotation/quotation.service';
import { of } from 'rxjs';
import { SupplierService } from '../../../_Services/supplier/supplier.service';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { ExtrafeesummaryService } from '../../../_Services/invoice/extrafeesummary.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-extra-fees-summary',
  templateUrl: './extra-fees-summary.component.html',
  styleUrls: ['./extra-fees-summary.component.scss'],
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
export class ExtraFeesSummaryComponent extends SummaryComponent<ExtraFeeSummaryRequestModel> {
  pagesizeitems = PageSizeCommon;
  requestSupModel: CommonDataSourceRequest;
  requestCustomerModel: CommonDataSourceRequest;
  public masterModel: ExtraFeeSummaryModel;
  selectedPageSize;
  _customValidationForInvoiceNo: boolean = false;
  searchId = SearchType.BookingNo;
  isFilterOpen: boolean;
  extrafeeSearchTypeList: any = extrafeeSearchtypelst;
  extrafeeDateTypeList: any = extrafeedatetypelst;
  toggleFormSection: boolean;
  onInit(): void {
    this.model = new ExtraFeeSummaryRequestModel();
    this.validator.setJSON("invoice/extrafee-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.masterModel = new ExtraFeeSummaryModel();
    this.requestSupModel = new CommonDataSourceRequest();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.selectedPageSize = PageSizeCommon[0];
    this.getServiceList();
    this.getBillPaidByList();
    this.getSupListBySearch();
    this.getCustomerListBySearch();
    this.getIsMobile();
    this.getExtraFeeStatusList();
    this.model.searchTypeId = SearchType.BookingNo;
    this.model.serviceId = APIService.Inspection;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    // var invoiceNo = this.pathroute.snapshot.paramMap.get("invoiceno");
    // if (invoiceNo) {
    //   this.model.searchTypeText = invoiceNo;
    //   this.model.searchTypeId = this.invoiceSearchTypeList[1].id;
    //   this.GetSearchData();
    // }
    // else {
    //   this.model.searchTypeId = SearchType.BookingNo;
    // }
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "extrafeesedit/edit-extra-fees";
  }

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, private mandayDashboardService: MandayDashboardService, private quotService: QuotationService,
    private supService: SupplierService, private cusService: CustomerService, public pathroute: ActivatedRoute,
    public utility: UtilityService,
    public service: ExtrafeesummaryService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
  }
  redirectToEditextrafees(id) {
    this.getDetails(id);
  }

  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchTypeText != null && this.model.searchTypeText.trim() == "" ? true : false;

    if (this.model.searchTypeText == null || this.model.searchTypeText == "") {

      if (!this.model.fromDate) {
        this.validator.isValid('fromdate');
      }

      else if (this.model.fromDate && !this.model.toDate) {
        this.validator.isValid('todate');
      }
    }

    return isOk;
  }

  //get service type list
  getServiceList() {

    this.masterModel.serviceLoading = true;
    this.mandayDashboardService.getServiceList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.serviceList = response.dataSourceList;
          this.masterModel.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.serviceLoading = false;
        });
  }

  getBillPaidByList() {
    this.masterModel.billPaidByListLoading = true;
    this.quotService.getBillPaidByist()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.billPaidByList = response.dataSourceList;
          this.masterModel.billPaidByListLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.billPaidByListLoading = false;
        });
  }
  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  BookingNoValidation() {
    return this._customValidationForInvoiceNo = this.model.searchTypeId == this.searchId
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
  }

  formValid(): boolean {
    let isOk = !this.BookingNoValidation() && this.validator.isValidIf('toDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromDate', this.IsDateValidationRequired())

    return isOk;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }
  Reset() {
    this.onInit();
  }
  export() {
    this.masterModel.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExtraFee_summary");
      },
        error => {
          this.masterModel.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName + ".xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName + ".xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportDataLoading = false;
  }
  GetStatusColor(statusid?) {
    if (this.masterModel._statuslist != null && this.masterModel._statuslist.length > 0 && statusid != null) {
      var result = this.masterModel._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  SearchByStatus(id) {
    this.model.statuslst = [];
    this.model.statuslst.push(id);
    this.SearchDetails();
  }
  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.requestSupModel.customerId = 0;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.masterModel.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.requestSupModel, term)
        : this.supService.getFactoryDataSourceList(this.requestSupModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supplierLoading = false;
      });
  }
  getSupplierData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestSupModel.searchText = this.masterModel.supplierInput.getValue();
      this.requestSupModel.skip = this.masterModel.supplierList.length;
    }
    this.requestSupModel.customerId = this.model.customerId;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.masterModel.supplierLoading = true;
    this.supService.getFactoryDataSourceList(this.requestSupModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(data);
        }
        if (isDefaultLoad)
          this.requestSupModel = new CommonDataSourceRequest();
        this.masterModel.supplierLoading = false;
      }),
      error => {
        this.masterModel.supplierLoading = false;
        this.setError(error);
      };
  }
  //fetch the first 10 suppliers for the customer on load
  getCustomerListBySearch() {
    this.requestCustomerModel.customerId = 0;
    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.requestCustomerModel, term)
        : this.cusService.getCustomerDataSourceList(this.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.requestCustomerModel.skip = this.masterModel.customerList.length;
    }
    this.masterModel.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.requestCustomerModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(data);
        }
        if (isDefaultLoad)
          this.requestCustomerModel = new CommonDataSourceRequest();
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }
  clearSupplierSelection() {
    this.model.supplierId = null;
    this.getSupplierData(true);
  }
  clearCustomerSelection() {
    this.model.customerId = null;
    this.getCustomerData(true);
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  clearBillTo() {
    this.clearSupplierSelection();
    this.clearCustomerSelection();
    this.model.billedTo = null;
  }
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "Fromdate": {
        this.model.fromDate = null;
        break;
      }
      case "Todate": {
        this.model.toDate = null;
        break;
      }
    }
  }
  getExtraFeeStatusList() {
    this.masterModel.statusLoading = true;

    this.service.getExtraFeeStatus()
      .pipe()
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.masterModel.extrafeeStatusList = response.dataSourceList;
          this.masterModel.statusLoading = false;
        }
        else {
          this.masterModel.statusLoading = false;
        }
      },
        error => {
          this.masterModel.statusLoading = false;
          this.setError(error);
        });
  }
  GetSearchData() {
    this.masterModel.searchloading = true;
    this.model.noFound = false;
    if (this.formValid()) {
      this.service.search(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success) {
              this.mapPageProperties(response);
              this.masterModel._statuslist = response.statusCountList;
              this.model.items = response.data.map(x => {
                var item: ExtraFeeItem = {
                  extrafeeid: x.extrafeeid,
                  bookingId: x.bookingId,
                  billedTo: x.billedTo,
                  currency: x.currency,
                  customerBookingNo: x.customerBookingNo,
                  customerName: x.customerName,
                  extraFeeType: x.exFeeType,
                  extrafeeinvoiceno: x.extraFeeInvoiceNumber,
                  invoiceno: x.invoiceNumber,
                  remarks: x.remarks,
                  service: x.service,
                  supplierName: x.supplierName,
                  totalAmount: x.totalAmt,
                  statusId: x.statusId,
                  invoiceCurrency: x.invoiceCurrency,
                  exchangeRate: x.exchangeRate
                }
                return item;
              });
              this.masterModel.searchloading = false;
            }

            else if (response && response.result == ResponseResult.NoDataFound) {
              this.model.items = [];
              this.model.noFound = true;
              this.masterModel.searchloading = false;
            }
          },
          error => {
            this.setError(error);
            this.masterModel.searchloading = false;
          });
    }
  }
}
