import { Customer } from './../../../_Models/supplier/edit-supplier.model';
import { LocationService } from './../../../_Services/location/location.service';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceItem, InvoicePdfAvailableRequest, InvoicePdfCreatedResponseResult, InvoiceSummaryModel, InvoiceSummaryRequestModel } from 'src/app/_Models/invoice/invoicesummary.model';
import { Validator } from '../../common';
import { SummaryComponent } from '../../common/summary.component';
import { NgbDate, NgbModal, NgbModalRef, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap, timestamp } from 'rxjs/operators';
import { concat, zip } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Country, invoiceSearchtypelst, SupplierType, BillPaidBy, SearchType, PageSizeCommon, Url, DefaultDateType, datetypelst, InvoiceStatus, invoicedatetypelst, DateObject, APIService, InvoicePreviewType, RoleEnum, ListSize, APIServiceEnum } from '../../../components/common/static-data-common'
import { TranslateService } from '@ngx-translate/core';
import { CustomKpiService } from '../../../_Services/customKpi/customKpi.service';
import { CustomkpiModel } from '../../../_Models/kpi/customkpimodel'
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { of } from 'rxjs';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CommonDataSourceRequest, DataSource, InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult, CountryDataSourceRequest, DataSourceResponse } from 'src/app/_Models/common/common.model';
import { QuotationService } from 'src/app/_Services/quotation/quotation.service';
import { InvoiceSummaryService } from 'src/app/_Services/invoice/invoicesummary.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { InvoicePreviewComponent } from '../../shared/invoice-preview/invoice-preview.component';
import { InvoiceKpiTemplateRequest } from 'src/app/_Models/invoice/invoice-kpi-template-request';
import { PaymentTerm } from 'src/app/_Models/quotation/quotation.model';
import { InvoiceBillingTo } from 'src/app/_Models/customer/customer-price-card.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { EditInvoiceService } from 'src/app/_Services/invoice/editinvoice.service';

@Component({
  selector: 'app-invoice-summary',
  templateUrl: './invoice-summary.component.html',
  styleUrls: ['./invoice-summary.component.scss'],
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
export class InvoiceSummaryComponent extends SummaryComponent<InvoiceSummaryRequestModel>{
  public model: InvoiceSummaryRequestModel;
  public masterModel: InvoiceSummaryModel;
  public pdfAvailableRequest: InvoicePdfAvailableRequest;
  invoiceSearchTypeList: any = invoiceSearchtypelst;
  invoiceDateTypeList: any = invoicedatetypelst;
  _defaultDateType: any = datetypelst[0];
  _billPaidBy: any = BillPaidBy;
  _service = APIService;
  searchId = SearchType.BookingNo;
  _customValidationForInvoiceNo: boolean = false;
  selectedPageSize;
  public modelRef: NgbModalRef;
  public popUpLoading: boolean = false;
  public selectedInvoiceNo: string;
  pagesizeitems = PageSizeCommon;
  requestSupModel: CommonDataSourceRequest;
  requestFactoryModel: CommonDataSourceRequest;
  requestCustomerModel: CommonDataSourceRequest;
  completeKpiTemplateList: any;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  isAuditInvoice: boolean;
  invoicePreviewRequest = new InvoicePreviewRequest();
  @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;
  customerIds: number[] = [];
  paymentTerms = PaymentTerm;
  currentUser: UserModel;
  private currentRoute: Router;

  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {

    return "editinvoice/edit-invoice";
  }

  getInvoiceSendDetails(): string {

    return "invoicesend/invoice-send";
  }

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, private mandayDashboardService: MandayDashboardService, public pathroute: ActivatedRoute,
    private service: InvoiceSummaryService, private supService: SupplierService, private quotService: QuotationService, public modalService: NgbModal,
    private cusService: CustomerService, public utility: UtilityService,
    private kpiService: CustomKpiService, translate: TranslateService, toastr: ToastrService, private authService: AuthenticationService,
    private locationService: LocationService, public customerPriceCardService: CustomerPriceCardService,
    public editInvoiceService: EditInvoiceService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
    this.currentUser = authService.getCurrentUser();
    this.currentRoute = router;
  }

  onInit() {
    this.model = new InvoiceSummaryRequestModel();
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.validator.setJSON("invoice/invoice-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.masterModel = new InvoiceSummaryModel();
    this.requestSupModel = new CommonDataSourceRequest();
    this.requestFactoryModel = new CommonDataSourceRequest();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.model.serviceId = APIService.Inspection;
    this.isAuditInvoice = false;
    this.getServiceList();
    this.getInvoiceTypeList();
    this.getBillPaidByList();
    this.getSupListBySearch();
    this.getFactoryListBySearch();
    this.getInvoiceStatusList();
    this.getKpiTemplateList();
    this.getIsMobile();
    this.getCountryListBySearch();
    this.getInvoiceOffice();
    this.getInvoicePaymentStatus();
    this.getBillingMethodList();
    this.isShownColumn();

    this.selectedPageSize = PageSizeCommon[0];
    var invoiceNo = this.pathroute.snapshot.paramMap.get("invoiceno");
    if (invoiceNo) {
      this.model.searchTypeText = invoiceNo;
      this.model.searchTypeId = this.invoiceSearchTypeList[1].id;
      this.GetSearchData();
    }
    else {
      this.model.searchTypeId = SearchType.BookingNo;
    }
    this.masterModel.isInvoiceEmailSendButtonVisible = false;
  }



  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchTypeText != null && this.model.searchTypeText.trim() == "" ? true : false;

    if (this.model.searchTypeText == null || this.model.searchTypeText == "") {

      if (!this.model.invoiceFromDate) {
        this.validator.isValid('invoiceFromDate');
      }

      else if (this.model.invoiceFromDate && !this.model.invoiceToDate) {
        this.validator.isValid('invoiceToDate');
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
    let isOk = !this.BookingNoValidation() && this.validator.isValidIf('invoiceToDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('invoiceFromDate', this.IsDateValidationRequired())

    return isOk;
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
              this.masterModel._statuslist = response.invoiceStatuslst;
              this.model.items = response.data.map(x => {
                var item: InvoiceItem = {
                  id: x.id,
                  invoiceDate: x.invoiceDate,
                  invoiceNo: x.invoiceNo,
                  invoiceCurrency: x.invoiceCurrency,
                  invoiceTo: x.invoiceTo,
                  invoiceToName: x.invoiceToName,
                  inspFees: x.inspFees,
                  isInspection: x.isInspection,
                  isTravelExpense: x.isTravelExpense,
                  service: x.service,
                  serviceType: x.serviceType,
                  totalFee: x.totalFee,
                  travelFee: x.travelFee,
                  otherExpense: x.otherExpense,
                  discount: x.discount,
                  hotelFee: x.hotelFee,
                  invoiceBookingDataList: [],
                  invoiceStatusId: x.invoiceStatusId,
                  customerIdList: x.customerIdList,
                  extraFees: x.extraFees,
                  invoiceTypeName: x.invoiceTypeName,
                  invoiceTypeId: x.invoiceTypeId,
                  serviceId: x.serviceId,
                  customerName: x.customerName,
                  factoryCountry: x.factoryCountry,
                  billingMethodName: x.billingMethodName,
                  invoiceOfficeName: x.invoiceOfficeName,
                  paymentStatusName: x.paymentStatusName,
                  selectedToSendEmail:false,
                  billTo:x.billTo,
                  bankId:x.bankId
                }
                return item;
              });
              var isAuditInvoice = this.model.items.find(x => x.serviceId == APIService.Audit);
              if (isAuditInvoice) {
                this.isAuditInvoice = true;
              }
              else {
                this.isAuditInvoice = false;
              }

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

  changeInvoiceSelection(dataRow, event) {

    if (event && event.target.checked) {
      this.isFilterOpen = true;
      if (this.model.items.filter(x => x.selectedToSendEmail).length > 1) {

        var firstItem = this.model.items.find(x => x.selectedToSendEmail && x.id!=dataRow.id);

        if(firstItem && dataRow.billTo == firstItem.billTo &&
          dataRow.invoiceToName==firstItem.invoiceToName &&
          dataRow.invoiceCurrency==firstItem.invoiceCurrency &&
          dataRow.bankId == firstItem.bankId &&
          dataRow.invoiceTypeId == firstItem.invoiceTypeId)
        {
          dataRow.selectedToSendEmail = true;
          event.target.checked = true;
        }
        else {
          this.showError("Invoice Summary", "please select same Billed to, Billed name, Invoice Type,bank and Currency")
          dataRow.selectedToSendEmail = false;
          event.target.checked = false;
        }
      }
    }

    this.masterModel.isInvoiceEmailSendButtonVisible = this.model.items.filter(x => x.selectedToSendEmail).length > 0 ? true : false;
  }

  openTemplatePopUp(selectedInvoiceNo, customerIdList) {
    this.popUpLoading = false;
    this.invoicePreviewRequest = new InvoicePreviewRequest();

    this.invoicePreviewRequest.customerId = customerIdList[0];
    this.invoicePreviewRequest.invoiceNo = selectedInvoiceNo;
    this.invoicePreviewRequest.invoiceData = [selectedInvoiceNo];
    this.invoicePreviewRequest.invoicePreviewTypes = [InvoicePreviewType.Booking, InvoicePreviewType.Product, InvoicePreviewType.SimpleInvoice, InvoicePreviewType.PO, InvoicePreviewType.Audit];
    this.invoicePreviewRequest.invoicePreviewFrom = InvoicePreviewFrom.InvoiceSummary;
    this.invoicePreviewRequest.previewTitle = "Invoice Preview";
    this.invoicePreviewRequest.service = APIService.Inspection;

    this.invoicePreviewRequest.isAccountingRole = this.currentUser.roles.some(x => x.id == RoleEnum.Accounting)
    this.invoicePreviewRequest.createdBy = this.currentUser.id;
    this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
      {
        windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
        centered: true, backdrop: 'static'
      });
  }

  closeInvoicePreview() {
    this.modelRef.close();
  }

  getInvoiceTypeList() {
    this.masterModel.invoiceTypeLoading = true;
    this.cusService.getInvoiceType()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.masterModel.invoiceTypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.masterModel.invoiceTypeLoading = false;

        },
        error => {
          this.masterModel.invoiceTypeLoading = false;
          this.setError(error);
        });
  }

  openCancelPopUp(invoiceNo, content) {
    this.selectedInvoiceNo = invoiceNo;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });

  }

  cancelInvoice() {
    this.masterModel.cancelInvoiceLoading = true;
    this.service.cancelInvoice(this.selectedInvoiceNo)
      .pipe()
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.masterModel.cancelInvoiceLoading = false;
            this.modelRef.close();
            this.showSuccess('INVOICE_SUMMARY.LBL_TITLE', 'INVOICE_SUMMARY.MSG_INVOICE_CANCELLED_SUCCESS');
            this.GetSearchData();
          }
          else {
            this.masterModel.cancelInvoiceLoading = false;
            this.showError('INVOICE_SUMMARY.LBL_TITLE', 'INVOICE_SUMMARY.MSG_INVOICE_CANCEL_FAIL');
          }
        },
        error => {
          this.masterModel.cancelInvoiceLoading = false;
          this.showError('INVOICE_SUMMARY.LBL_TITLE', 'INSPECTION_CANCEL.MSG_UNKNOWN_ERROR');
        });
  }

  Reset() {
    this.onInit();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  toggleExpandRow(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.invoiceBookingDataList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getinvoiceBookingList(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  getinvoiceBookingList(rowItem) {
    this.service.getInvoiceBooking(rowItem.invoiceNo, rowItem.serviceId)
      .pipe()
      .subscribe(
        res => {
          if (res.result == ResponseResult.Success) {
            rowItem.invoiceBookingDataList = res.data;
          }
        },
        error => {

        });
  }

  closeViewPopUp() {
    this.model.invoiceReportTemplateId = null;
    this.modelRef.close();
  }

  export() {
    this.masterModel.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_summary");
      },
        error => {
          this.masterModel.exportDataLoading = false;
        });
  }


  checkInvoicePdfAvailableList() {
    this.masterModel.checkPdfInvoiceLoading = true;
    this.pdfAvailableRequest = new InvoicePdfAvailableRequest();
    this.pdfAvailableRequest.invoiceNumbers = this.model.items.filter(x => x.selectedToSendEmail).map(x => x.invoiceNo);
    this.service.checkPdfInvoice(this.pdfAvailableRequest)
      .subscribe(res => {


        if (res) {
          // positive case and redirect to email preview page
          if (res.result == InvoicePdfCreatedResponseResult.PdfCreatedToAllInvoice) {
            this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getInvoiceSendDetails()}`], { queryParams: { paramParent: encodeURI(JSON.stringify(this.model)) } });
          }
          // negative case and show the validation
          else if (res.result == InvoicePdfCreatedResponseResult.PdfCreatedToFewInvoice) {
            this.showError("Invoice Summary", "please upload the invoice pdf for " + res.invoiceNumbers.join(","));
          }
          // negative case and show the validation
          else if (res.result == InvoicePdfCreatedResponseResult.PdfNotCreatedToAnyInvoice) {
            this.showError("Invoice Summary", "please upload the invoice pdf for " + res.invoiceNumbers.join(","));
          }
          // Request is wrong so please validate the input
          else if (res.result == InvoicePdfCreatedResponseResult.RequestIsNotValid) {
            this.showError("Invoice Summary", "Request is not valid, Please check");
          }
        }
        this.masterModel.checkPdfInvoiceLoading = false;
      },
        error => {
          this.masterModel.checkPdfInvoiceLoading = false;
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

  redirectToEditInvoice(invoiceNo, serviceId) {
    this.model.serviceId = serviceId;
    this.getDetails(invoiceNo);
  }

  mapPageProperties(response: any) {

    this.model.index = response.index;
    this.model.pageSize = response.pageSize;
    this.model.totalCount = response.totalCount;
    this.model.pageCount = response.pageCount;

  }

  GetStatusColor(statusid?) {
    if (this.masterModel._statuslist != null && this.masterModel._statuslist.length > 0 && statusid != null) {
      var result = this.masterModel._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }

  SearchByStatus(id) {
    this.model.invoiceStatusId = [];
    this.model.invoiceStatusId.push(id);
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

  //fetch the supplier data with virtual scroll
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
        this.requestSupModel.skip = 0;
        this.requestSupModel.take = ListSize;
        this.masterModel.supplierLoading = false;
      }),
      error => {
        this.masterModel.supplierLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getFactoryListBySearch() {
    this.requestFactoryModel.customerId = 0;
    this.requestFactoryModel.supplierType = SupplierType.Factory;
    this.masterModel.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.requestFactoryModel, term)
        : this.supService.getFactoryDataSourceList(this.requestFactoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factoryLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.factoryList = data;
        this.masterModel.factoryLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestFactoryModel.searchText = this.masterModel.factoryInput.getValue();
      this.requestFactoryModel.skip = this.masterModel.factoryList.length;
    }
    this.requestFactoryModel.customerId = this.model.customerId;
    this.requestFactoryModel.supplierType = SupplierType.Factory;
    this.masterModel.factoryLoading = true;
    this.supService.getFactoryDataSourceList(this.requestFactoryModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.factoryList = this.masterModel.factoryList.concat(data);
        }
        this.requestFactoryModel.skip = 0;
        this.requestFactoryModel.take = ListSize;
        this.masterModel.factoryLoading = false;
      }),
      error => {
        this.masterModel.factoryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getCustomerListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.requestCustomerModel.id = this.model.customerId;
    }

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

  clearFactorySelection() {
    this.model.factoryId = null;
    this.getFactoryData(true);
  }

  clearSupplierSelection() {
    this.model.supplierId = null;
    this.getSupplierData(true);
  }

  clearCustomerSelection() {
    this.model.customerId = null;
    this.getCustomerData(true);
  }
  clearTemplate() {
    this.model.invoiceReportTemplateId = null;
  }

  clearInvoiceTo() {
    this.clearSupplierSelection();
    this.clearCustomerSelection();
    this.model.invoiceTo = null;
  }

  getInvoiceStatusList() {
    this.masterModel.statusLoading = true;

    this.service.getInvoiceStatus()
      .pipe()
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.masterModel.invoiceStatusList = response.dataSourceList.filter(x => x.id != 4 && x.id != 5);
          this.masterModel.statusLoading = false;
        }
        else {
          this.showError('INVOICE_SUMMARY.TITLE', 'INVOICE_SUMMARY.MSG_INVOICE_STATUS_NOT_FOUND');
          this.masterModel.statusLoading = false;
        }
      },
        error => {
          this.masterModel.statusLoading = false;
          this.setError(error);
        });
  }

  getKpiTemplateList() {
    this.masterModel.kpiTemplateListLoading = true;
    const request = new InvoiceKpiTemplateRequest();
    request.customerIds = this.customerIds;

    this.service.getKpiTemplateList(request)
      .pipe()
      .subscribe(response => {
        if (response.result = ResponseResult.Success) {
          this.masterModel.kpiTemplateList = response.templateList;
          this.completeKpiTemplateList = response.templateList;
          this.masterModel.kpiTemplateListLoading = false;
        }
        else {
          this.masterModel.kpiTemplateListLoading = false;
        }
      },
        error => {
          this.setError(error);
          this.masterModel.kpiTemplateListLoading = false;
        });
  }

  filterKpiTemplateList(customerId: Array<number>) {
    if (customerId != null && customerId.length > 0) {
      this.masterModel.kpiTemplateList = this.completeKpiTemplateList;
      this.masterModel.kpiTemplateList = this.masterModel.kpiTemplateList.filter(x => x.customerId == customerId || x.customerId == null);
    }

    else if (!customerId) {
      this.masterModel.kpiTemplateList = this.completeKpiTemplateList;
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
    console.log(this.isMobile);
  }

  exportKpiTemplate(kpTtemplateId) {
    this.masterModel.kpiExportLoading = true;

    var kpiRequestModel: CustomkpiModel = {
      fromdate: null,
      todate: null,
      templateId: kpTtemplateId,
      customerId: this.model.customerId,
      serviceTypeIdLst: null,
      officeIdLst: null,
      invoiceNo: this.selectedInvoiceNo,
      pageSize: 10,
      index: 1,
      pageCount: 1,
      totalCount: 0,
      noFound: true,
      items: [],
      brandIdList: null,
      departmentIdList: null,
      bookingNo: null,
      customerMandayGroupByFields: [],
      countryIds: [],
      invoiceTypeIdList: [],
      paymentStatusIdList: []
    };

    this.kpiService.exportSummary(kpiRequestModel)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_statement");
        this.masterModel.kpiExportLoading = false;
        this.modelRef.close();
        this.model.invoicekpiTemplateId = null;
      },
        error => {
          this.masterModel.kpiExportLoading = false;
        });
  }

  closekpiTemplateViewPopUp() {
    this.modelRef.close();
    this.model.invoicekpiTemplateId = null
  }

  openKpiTemplatePopUp(content, selectedInvoiceNo, customerId) {
    this.popUpLoading = false;
    this.selectedInvoiceNo = selectedInvoiceNo;
    this.customerIds = customerId;
    this.getKpiTemplateList();
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "Fromdate": {
        this.model.invoiceFromDate = null;
        break;
      }
      case "Todate": {
        this.model.invoiceToDate = null;
        break;
      }
    }
  }

  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.factoryCountryRequest.searchText = this.masterModel.factoryCountryInput.getValue();
      this.masterModel.factoryCountryRequest.skip = this.masterModel.factoryCountryList.length;
    }

    this.masterModel.factoryCountryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.factoryCountryList = this.masterModel.factoryCountryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.masterModel.factoryCountryRequest = new CountryDataSourceRequest();
        this.masterModel.factoryCountryLoading = false;
      }),
      error => {
        this.masterModel.factoryCountryLoading = false;
        this.setError(error);
      };
  }

  getCountryListBySearch() {

    if (this.model.factoryCountryIds != null && this.model.factoryCountryIds.length > 0) {
      this.masterModel.factoryCountryRequest.countryIds = this.model.factoryCountryIds;
    }
    this.masterModel.factoryCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factoryCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest, term)
        : this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factoryCountryLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.factoryCountryList = data;
        this.masterModel.factoryCountryLoading = false;
      });
  }

  //getBillingMethodList
  getBillingMethodList() {
    this.masterModel.billingMethodLoading = true;

    this.customerPriceCardService.getBillingMethodList()
      .pipe()
      .subscribe(
        response => {
          this.processBillingMethodListResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');

          this.masterModel.billingMethodLoading = false;
        });

  }

  //process the billing method list response
  processBillingMethodListResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {
        this.masterModel.billingMethodList = response.dataSourceList;
      }
      else if (response.result == ResponseResult.NoDataFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_BILL_METHOD_NOT_FOUND');
      }
      this.masterModel.billingMethodLoading = false;
    }
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
  //get the invoice office list
  getInvoiceOffice() {
    this.masterModel.invoiceOfficeLoading = true;
    this.editInvoiceService.getInvoiceOffice()
      .pipe()
      .subscribe(
        response => {
          this.processInvoiceOfficeResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.invoiceOfficeLoading = false;
        });
  }

  //process the invoice office response
  processInvoiceOfficeResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {
        this.masterModel.invoiceOfficeList = response.dataSourceList;
      }
      else if (response.result == ResponseResult.NoDataFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_OFFICE_NOT_FOUND');
      }
      this.masterModel.invoiceOfficeLoading = false;
    }
  }

  ngAfterViewInit() {
    this.getCustomerListBySearch();
    this.getCountryListBySearch();
  }

  clearFactoryCountry() {
    this.masterModel.factoryCountryRequest.countryIds = [];
    this.getCountryListBySearch();
  }

    sendInvoiceEmail() {
        this.checkInvoicePdfAvailableList();
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
}
