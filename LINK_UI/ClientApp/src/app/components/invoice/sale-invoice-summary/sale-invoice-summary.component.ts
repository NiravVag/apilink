import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { DataSourceResponse, ResponseResult } from 'src/app/_Models/common/common.model';
import { SaleInvoiceItem, SaleInvoiceSummaryModel, SaleInvoiceSummaryRequestModel } from 'src/app/_Models/invoice/sale-invoice-summary-model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { EditInvoiceService } from 'src/app/_Services/invoice/editinvoice.service';
import { SaleInvoiceSummaryService } from 'src/app/_Services/invoice/sale-invoice-summary.service';
import { QuotationService } from 'src/app/_Services/quotation/quotation.service';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from '../../common';
import { APIService, BookingSummaryNameTrim, FileContainerList, invoicedatetypelst, invoiceSearchtypelst, ListSize, PageSizeCommon, SearchType, SupplierType, UserType } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-sale-invoice-summary',
  templateUrl: './sale-invoice-summary.component.html',
  styleUrls: ['./sale-invoice-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '0',
        'padding-bottom': '16px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ]),
    trigger('expandCollapseMobileAd', [
      state('open', style({
        'height': '*',
        'opacity': 1
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class SaleInvoiceSummaryComponent extends SummaryComponent<SaleInvoiceSummaryRequestModel> {
  public model: SaleInvoiceSummaryRequestModel;
  filterDataShown: boolean;
  isFilterOpen: boolean;
  pagesizeitems = PageSizeCommon;
  invoiceSearchTypeList: any = invoiceSearchtypelst;
  invoiceDateTypeList: any = invoicedatetypelst;
  _customValidationForInvoiceNo: boolean = false;
  searchId: SearchType.BookingNo;
  masterModel: SaleInvoiceSummaryModel;
  componentDestroyed$: Subject<boolean> = new Subject();
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  _IsCustomerUser: boolean = false;
  toggleFormSection: boolean;
  selectedPageSize: number;
  _booksearttypeid = SearchType.BookingNo;

  constructor(private service: SaleInvoiceSummaryService, public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public pathroute: ActivatedRoute, public customerService: CustomerService, private supService: SupplierService, authserve: AuthenticationService,
    public editInvoiceService: EditInvoiceService, private quotService: QuotationService, private mandayDashboardService: MandayDashboardService,
    public fileStoreService: FileStoreService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
    this.toggleFormSection = false;
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
  }

  onInit(): void {
    //initialize the sales invoice source values
    this.initialize();
    this.validator.setJSON("invoice/sale-invoice-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    const invoiceNo = this.pathroute.snapshot.paramMap.get("invoiceno");
    if (invoiceNo) {
      this.model.searchTypeText = invoiceNo;
      this.GetSearchData();
    }

  }
  initialize() {
    //initialize the objects
    this.objectInitialization();
  }
  //initialize the objects
  objectInitialization() {
    this.filterDataShown = false;
    this.masterModel = new SaleInvoiceSummaryModel();
    this.model = new SaleInvoiceSummaryRequestModel();
    this.validator.isSubmitted = false;
    this.masterModel.DateName = invoicedatetypelst[0].name;
    this.model.searchTypeId = SearchType.InvoiceNo;
    this.model.dateTypeId = SearchType.InvoiceDate;
    this.model.serviceId = APIService.Inspection;
    this.masterModel.selectedNumber = this.invoiceSearchTypeList.find(x => x.id == SearchType.InvoiceNo) ? this.invoiceSearchTypeList.find(x => x.id == SearchType.InvoiceNo).shortName : "";
    this.masterModel.selectedNumberPlaceHolder = this.invoiceSearchTypeList.find(x => x.id == SearchType.InvoiceNo) ? "Enter " + this.invoiceSearchTypeList.find(x => x.id == SearchType.InvoiceNo).name : "";
    this.masterModel._paymentStatusList = [];
    this.isShownColumn();
  }
  isShownColumn() {
    this.masterModel.isShowColumn = this.masterModel.isShowColumn == true ?
      false : true;
    if (!this.masterModel.isShowColumn) {
      this.masterModel.isShowColumnImagePath = "assets/images/new-set/table-expand.svg";
      this.masterModel.showColumnTooltip = "Expand";
    }
    else {
      this.masterModel.isShowColumnImagePath = "assets/images/new-set/table-collapsea.svg";
      this.masterModel.showColumnTooltip = "Collapse";
    }
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    throw new Error('Method not implemented.');
  }
  ngAfterViewInit() {
    //get the customer details
    this.getCustomerListBySearch();
    if (this._IsCustomerUser && !this.model.customerId)
      this.model.customerId = this.currentUser.customerid;
    this.getInvoicePaymentStatus();
    this.getBillPaidByList();
    this.getServiceList();
    //get the customer based details
    this.getCustomerBasedDetails(this.model.customerId);
  }
  isfilterData() {
    if ((this.model.fromDate && this.model.fromDate != '') || (this.model.toDate && this.model.toDate != '') ||
      this.model.customerId > 0 || this.model.supplierId > 0 || this.model.invoiceTo || this.model.serviceId > 0 ||
      (this.model.factoryIdList && this.model.factoryIdList.length > 0) ||
      (this.model.paymentStatusIdList && this.model.paymentStatusIdList.length > 0))
      this.filterDataShown = true;
    else
      this.filterDataShown = false;
  }
  Reset() {
    this.initialize();
    this.ngAfterViewInit();
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
    if (window.innerWidth < 450) {

      if (this.isFilterOpen) {
        document.body.classList.add('disable-scroll');
      }
      else {
        document.body.classList.remove('disable-scroll');
      }
    }
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
    this.filterDataShown = false;
  }
  SetSearchTypemodel(item) {
    this.model.searchTypeId = item.id;
    this.masterModel.selectedNumber = item.shortName;

    if (item.id == SearchType.BookingNo) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.InvoiceNo) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
  }

  BookingNoValidation(bookingText) {
    this.masterModel.isShowSearchLens = this.model.searchTypeText != null && this.model.searchTypeText.trim() != "";
    this._customValidationForInvoiceNo = this.model.searchTypeId == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && ((isNaN(Number(bookingText))) || (bookingText.trim().length > 9));
  }
  SetSearchDatetype(searchdatetype) {
    this.model.dateTypeId = searchdatetype;
    this.masterModel.DateName = this.invoiceDateTypeList.filter(x => x.id == searchdatetype)?.name;
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "fromDate": {
        this.model.fromDate = null;
        break;
      }
      case "toDate": {
        this.model.toDate = null;
        break;
      }
    }
  }
  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchTypeText != null && this.model.searchTypeText.trim() == "" ? true : false;

    if (this.model.searchTypeText == null || this.model.searchTypeText == "") {

      if (!this.model.fromDate) {
        this.validator.isValid('fromDate');
      }

      else if (this.model.fromDate && !this.model.toDate) {
        this.validator.isValid('toDate');
      }
    }

    return isOk;
  }
  formValid(): boolean {
    let isOk = !this._customValidationForInvoiceNo && this.validator.isValidIf('fromDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('toDate', this.IsDateValidationRequired())

    return isOk;
  }
  export() {
    this.masterModel.exportDataLoading = true;
    this.model.isExport = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sale_invoice_summary");
      },
        error => {
          this.masterModel.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });
    let navigator: any = window.navigator;
    if (navigator && navigator.msSaveOrOpenBlob) {
      navigator.msSaveOrOpenBlob(blob, fileName + ".xlsx");
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
  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }
  GetSearchData() {
    this.masterModel.searchLoading = true;
    if (this.isFilterOpen)
      this.isFilterOpen = false;
    this.model.isExport = false;
    this.model.noFound = false;
    if (this.formValid()) {
      this.service.search(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success) {
              this.mapPageProperties(response);
              this.masterModel._paymentStatusList = response.paymentStatusCountList;
              this.model.items = response.data.map(x => {
                const item: SaleInvoiceItem = {
                  id: x.id,
                  invoiceDate: x.invoiceDate,
                  invoiceNo: x.invoiceNo,
                  invoicedName: x.invoicedName,
                  invoiceCurrency: x.invoiceCurrency,
                  totalFee: x.totalFee,
                  paymentStatusName: x.paymentStatusName,
                  paymentStatusId: x.paymentStatusId,
                  paymentDate: x.paymentDate,
                  uniqueId: x.uniqueId
                }
                return item;
              });
              const isAuditInvoice = this.model.items.find(x => x.serviceId == APIService.Audit);
              if (isAuditInvoice) {
                this.masterModel.isAuditInvoice = true;
              }
              else {
                this.masterModel.isAuditInvoice = false;
              }

              this.masterModel.searchLoading = false;
            }

            else if (response && response.result == ResponseResult.NoDataFound) {
              this.model.items = [];
              this.model.noFound = true;
              this.masterModel._paymentStatusList = [];
              this.masterModel.searchLoading = false;
            }
          },
          error => {
            this.setError(error);
            this.masterModel.searchLoading = false;
          });
    }
  }
  getSearchDetails() {
    if (this.model.searchTypeId && this.model.searchTypeText != '') {
      this.SearchDetails();
    }
  }
  GetStatusColor(statusid?) {
    if (this.masterModel._paymentStatusList != null && this.masterModel._paymentStatusList.length > 0 && statusid != null) {
      const result = this.masterModel._paymentStatusList.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  clickStatus(id) {
    if (id && id > 0) {

      //if it contains the value
      const isValueExists = this.model.paymentStatusIdList.includes(id);

      //open the filter
      if (!this.isFilterOpen)
        this.isFilterOpen = true;

      this.toggleFormSection = false;

      if (isValueExists) {
        this.model.paymentStatusIdList = this.model.paymentStatusIdList.filter(x => x != id);
        this.model.paymentStatusIdList = [...this.model.paymentStatusIdList];

      }
      else {
        this.model.paymentStatusIdList.push(id);
        this.model.paymentStatusIdList = [...this.model.paymentStatusIdList];
      }
    }
  }

  changeStatus(id) {
    return this.model.paymentStatusIdList.includes(id);
  }
  getFile(file: string) {
    this.fileStoreService.downloadBlobFile(file, FileContainerList.Invoice)
      .subscribe(res => {
        this.downloadInvoiceFile(res, res.type);
      },
        error => {
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });
  }
  downloadInvoiceFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }
  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerId) {
      this.masterModel.requestCustomerModel.idList.push(this.model.customerId);
    }
    else {
      this.masterModel.requestCustomerModel.idList = null;
    }

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerListByUserType(this.masterModel.requestCustomerModel, null, term)
        : this.customerService.getCustomerListByUserType(this.masterModel.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.masterModel.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.customerService.getCustomerListByUserType(this.masterModel.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.masterModel.requestCustomerModel.skip = 0;
          this.masterModel.requestCustomerModel.take = ListSize;
        }
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  ChangeCustomer(cusitem) {

    if (cusitem != null && cusitem.id != null) {
      this.assignSupplierDetails();
      //clear the list
      this.masterModel.supplierList = [];
      this.masterModel.factoryList = [];
      //clear the selected values
      this.model.supplierId = null;
      this.model.factoryIdList = [];

      this.getCustomerBasedDetails(cusitem.id);

    }

  }

  assignSupplierDetails() {
    this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
  }
  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
      if (this.model.supplierId)
        this.getFactoryListBySearch();

      const customerDetails = this.masterModel.customerList.find(x => x.id == customerId);
      if (customerDetails)
        this.masterModel.customerName =
          customerDetails.name.length > BookingSummaryNameTrim ?
            customerDetails.name.substring(0, BookingSummaryNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.masterModel.customerName = "";
    }
  }
  clearCustomer() {
    this.model.customerId = null;
    this.model.supplierId = null;
    this.masterModel.supplierList = [];
    this.model.factoryIdList = [];
    this.masterModel.factoryList = [];
    this.getCustomerListBySearch();
    this.assignSupplierDetails();
  }
  getSupplierListBySearch() {
    this.masterModel.supsearchRequest.supplierId = null;
    if (this.model.customerId)
      this.masterModel.supsearchRequest.customerId = this.model.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierId)
      this.masterModel.supsearchRequest.supplierId = this.model.supplierId;
    this.masterModel.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supsearchRequest.searchText = this.masterModel.supInput.getValue();
    this.masterModel.supsearchRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supsearchRequest.customerId = this.model.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
    this.masterModel.supLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(customerData);
        }
        this.masterModel.supsearchRequest.skip = 0;
        this.masterModel.supsearchRequest.take = ListSize;
        this.masterModel.supLoading = false;
      }),
      (error: any) => {
        this.masterModel.supLoading = false;
      };
  }
  ChangeSupplier(supitem) {
    this.model.factoryIdList = [];
    if (supitem != null && supitem.id != null) {
      this.getFactoryListBySearch();

      var supplierDetails = this.masterModel.supplierList.find(x => x.id == supitem.id);
      if (supplierDetails)
        this.masterModel.supplierName =
          supplierDetails.name.length > BookingSummaryNameTrim ?
            supplierDetails.name.substring(0, BookingSummaryNameTrim) + "..." : supplierDetails.name;
    }
    else {
      this.masterModel.supplierName = "";
    }
  }
  clearSupplier() {
    this.model.supplierId = null;
    this.data.factoryList = null;
    this.model.factoryIdList = [];
    this.getSupplierListBySearch();
  }
  applyFactoryRelatedFilters() {
    this.masterModel.facsearchRequest.customerIds = [];
    this.masterModel.facsearchRequest.supplierIds = [];
    if (this.model.customerId)
      this.masterModel.facsearchRequest.customerIds.push(this.model.customerId);
    this.masterModel.facsearchRequest.supplierType = SupplierType.Factory;
    if (this.model.supplierId)
      this.masterModel.facsearchRequest.supplierIds.push(this.model.supplierId);
    else
      this.masterModel.facsearchRequest.supplierIds = null;
  }
  getFactoryListBySearch() {

    this.applyFactoryRelatedFilters();
    this.masterModel.facInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.facLoading = true),
      switchMap(term => term
        ? this.supService.getSupplierDataSource(this.masterModel.facsearchRequest, null, term)
        : this.supService.getSupplierDataSource(this.masterModel.facsearchRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.facLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.factoryList = data;
        this.masterModel.facLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData() {

    this.applyFactoryRelatedFilters();
    this.masterModel.facsearchRequest.searchText = this.masterModel.facInput.getValue();
    this.masterModel.facsearchRequest.skip = this.masterModel.factoryList.length;
    this.masterModel.facLoading = true;
    this.supService.getSupplierDataSource(this.masterModel.facsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.factoryList = this.masterModel.factoryList.concat(customerData);
        }
        this.masterModel.facsearchRequest.skip = 0;
        this.masterModel.facsearchRequest.take = ListSize;
        this.masterModel.facLoading = false;
      }),
      (error: any) => {
        this.masterModel.facLoading = false;
      };
  }
  //get the invoice payment status list
  getInvoicePaymentStatus() {
    this.masterModel.invoicePaymentStatusLoading = true;
    this.editInvoiceService.getInvoicePaymentStatus()
      .pipe()
      .subscribe(
        response => {
          this.processInvoicePaymentStatusResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.invoicePaymentStatusLoading = false;
        });
  }

  //process the invoice payment status reponse
  processInvoicePaymentStatusResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {

        this.masterModel.invoicePaymentStatusList = response.dataSourceList;
      }
      else if (response.result == ResponseResult.NoDataFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_STATUS_NOT_FOUND');
      }
      this.masterModel.invoicePaymentStatusLoading = false;
    }
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
  clearInvoiceTo() {
    this.clearSupplierSelection();
    this.clearCustomerSelection();
    this.model.invoiceTo = null;
  }
  clearSupplierSelection() {
    this.model.supplierId = null;
    this.getSupplierData();
  }
  clearCustomerSelection() {
    this.model.customerId = null;
    this.getCustomerData(true);
  }
}
