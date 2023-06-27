import { mode } from 'crypto-js';
import { LocationService } from './../../../_Services/location/location.service';
import { EntityFeature, InvoicePreviewType } from './../../common/static-data-common';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceItem, InvoiceStatusModel, InvoiceStatusRequestModel, invoiceSearchtypelst, invoicedatetypelst, InvoiceTypeClass, InvoiceCommunicationSaveRequest, InvoiceCommunicationSaveResponse, InvoiceCommunicationSaveResultResponse, InvoiceCommunicationTableResponse, InvoiceCommunicationTableResultResponse } from 'src/app/_Models/invoice/invoicestatus.model';
import { JsonHelper, Validator } from '../../common';
import { SummaryComponent } from '../../common/summary.component';
import { NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap, filter } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { SupplierType, SearchType, PageSizeCommon, DefaultDateType, APIService, RoleEnum, InvoiceStatusSummaryStatusList, InvoiceType, Url, DateObject, bookingSearchtypelst, MobileViewFilterCount, BookingSummaryNameTrim, BillPaidBy } from '../../common/static-data-common'
import { TranslateService } from '@ngx-translate/core';
import { CustomKpiService } from '../../../_Services/customKpi/customKpi.service';
import { of, Subject } from 'rxjs';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CommonDataSourceRequest, CountryDataSourceRequest, DataSource, InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult, DataSourceResponse } from 'src/app/_Models/common/common.model';
import { QuotationService } from 'src/app/_Services/quotation/quotation.service';
import { InvoiceStatusService } from 'src/app/_Services/invoice/invoicestatus.service';
import { InvoiceSummaryService } from 'src/app/_Services/invoice/invoicesummary.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UserModel } from '../../../_Models/user/user.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { InvoiceGenerateModel, InvoiceGenerateResponse, InvoiceGenerateResult, InvoiceSupplierInfo } from 'src/app/_Models/invoice/invoicegenerate.model';
import { InvoiceBillingTo, InvoiceRequestType } from 'src/app/_Models/customer/customer-price-card.model';
import { EditInvoiceService } from 'src/app/_Services/invoice/editinvoice.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { PaymentTerm } from 'src/app/_Models/quotation/quotation.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ExpenseService } from 'src/app/_Services/expense/expense.service';
import { EditEmailSendService } from 'src/app/_Services/email-send/edit-email-send.service';
import { BookingReportRequest, EmailPreviewData, EmailPreviewRequest, EmailPreviewResponseResult, EmailReportPreviewDetail, EmailSendingType, EmailSendResult } from 'src/app/_Models/email-send/edit-email-send.model';
import { EmailSendType } from 'src/app/_Models/email-send/email-config.model';

@Component({
  selector: 'app-invoice-status',
  templateUrl: './invoice-status.component.html',
  styleUrls: ['./invoice-status.component.scss'],
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
    ])
  ]
})
export class InvoiceStatusComponent extends SummaryComponent<InvoiceStatusRequestModel>{
  public model: InvoiceStatusRequestModel;
  public masterModel: InvoiceStatusModel;
  invoiceSearchTypeList: any = invoiceSearchtypelst;
  invoiceDateTypeList: any = invoicedatetypelst;
  emailPreviewModel: EmailPreviewRequest;
  emailSendingType=EmailSendingType;


  searchId = SearchType.BookingNo;
  _customValidationForInvoiceNo: boolean = false;
  selectedPageSize;
  invoiceGenerateModel: InvoiceGenerateModel;
  invoiceGenerateResponse: InvoiceGenerateResponse;
  invoiceModel: any;
  isBankIdMatched: boolean = false;
  invoicePreviewRequest= new InvoicePreviewRequest();

  serialNumberListToEmailPreview:any;
  emailPreviewTitle:string;

  pagesizeitems = PageSizeCommon;
  requestSupModel: CommonDataSourceRequest;
  requestFactoryModel: CommonDataSourceRequest;
  requestCustomerModel: CommonDataSourceRequest;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  paymentTerms = PaymentTerm;

  emailModelItem: Array<EmailPreviewData>;

  public user: UserModel;
  public hasAccountRole: boolean = false;
  _RoleEnum = RoleEnum;
  dialog: NgbModalRef | null;
  @ViewChild('invoicePreviewTemplate') invoicePreviewTemplateModal: TemplateRef<any>;
  @ViewChild('emailruleinfo') emailruleinfo;
  @ViewChild('emailPreviewContent') emailPreviewContent;
  componentDestroyed$: Subject<boolean> = new Subject();
  _booksearttypeid = SearchType.BookingNo;
  public _customvalidationforbookid: boolean = false;
  filterDataShown: boolean;
  filterCount: number;
  modelRef: NgbModalRef;
  communicationSaveModel: InvoiceCommunicationSaveRequest;
  communicationvalidator: any;
  private _translate: TranslateService;
  private _toastr: ToastrService;

  extraFeeInvoiceNo:string="Extra/Penalty InvoiceNo";
  extraFeeStatus:string="Extra/Penalty Status";
  extraFeeTotal:string="Extra/Penalty Total";
  extraFeeInvoicedate:string="Extra/penalty Invoice date";
  extraFeeBilledTo:string="Extra/penalty BilledTo";
  extraFeePaymentStatus:string="Extra/penalty Payment Status";
  extraFeePaymentDate:string="Extra/penalty Payment Date";

  invoicePreviewTypes = [InvoicePreviewType.Booking, InvoicePreviewType.Product, InvoicePreviewType.SimpleInvoice, InvoicePreviewType.PO, InvoicePreviewType.Audit];
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "editinvoice/edit-invoice";
  }

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, private mandayDashboardService: MandayDashboardService, public pathroute: ActivatedRoute,
    private service: InvoiceStatusService, private invoiceService: InvoiceSummaryService, private supService: SupplierService, private quotService: QuotationService, public modalService: NgbModal,
    private cusService: CustomerService, public utility: UtilityService,public emailSendService: EditEmailSendService,
      public exChangeservice: ExpenseService, private locationService: LocationService,
    public editInvoiceService: EditInvoiceService,
    public refService: ReferenceService,
    private invoiceSummaryService: InvoiceSummaryService,
    private kpiService: CustomKpiService, translate: TranslateService, toastr: ToastrService,
    public officeService: OfficeService, private jsonHelper: JsonHelper) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
    this._toastr = toastr;
    this._translate = translate;
  }

  onInit() {
    if (localStorage.getItem('currentUser')) {
      this.user = JSON.parse(localStorage.getItem('currentUser'));
      var index = this.user.roles.findIndex(x => x.id == this._RoleEnum.Accounting);
      if (index != -1) {
        this.hasAccountRole = true;
      }
    }
    this.model = new InvoiceStatusRequestModel();
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.validator.setJSON("invoice/invoice-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = this.validator.jsonHelper;
    this.validator.isSubmitted = false;
    this.masterModel = new InvoiceStatusModel();
    this.masterModel.additionalTax = null;
    this.masterModel.additionalTaxList = [{ "id": 10, "name": "10% (<2.5 M)" }, { "id": 12, "name": "12% (>=2.5 M)" }];
    this.masterModel.showAdditionalTax = false;
    this.masterModel._statuslist = [];
    this.requestSupModel = new CommonDataSourceRequest();
    this.requestFactoryModel = new CommonDataSourceRequest();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.communicationSaveModel = new InvoiceCommunicationSaveRequest();
    this.model.serviceId = APIService.Inspection;

    this.setInvoiceModel();
    this.getInvoiceTypeList();
    this.checkHideInvoiceFeatureExist();
    this.getServiceList();
    this.getSupListBySearch();
    this.getFactoryListBySearch();
    this.getCustomerListBySearch();
    this.getInvoiceStatusList();
    this.getIsMobile();
    this.getOfficeLocationList();
    this.getCountryListBySearch();
    this.getInvoicePaymentStatus();

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
    this.masterModel.selectedNumber = this.invoiceSearchTypeList.find(x => x.id == 1) ?
      this.invoiceSearchTypeList.find(x => x.id == 1).shortName : "";

    this.masterModel.selectedNumberPlaceHolder = this.invoiceSearchTypeList.find(x => x.id == 1) ?
      "Enter " + this.invoiceSearchTypeList.find(x => x.id == 1).name : "";

    this.isShownColumn();
    this.masterModel.DateName = invoicedatetypelst[0].name;
    this.masterModel.isInvoiceCreateVisible = false;

  }

  setInvoiceModel() {
    this.invoiceGenerateModel = new InvoiceGenerateModel();
    this.invoiceGenerateModel.invoiceType = InvoiceType.PreInvoice;
    this.invoiceGenerateModel.isInspection = true;
    this.invoiceGenerateModel.isTravelExpense = true;
    this.invoiceGenerateModel.invoicingRequest = InvoiceRequestType.NotApplicable;// not applicable


    // set bill to related information
  }

  setBilltoInformation(invoiceData) {

    if (invoiceData) {
      this.invoiceGenerateModel.invoiceTo = invoiceData.billTo;
    }

    if (invoiceData.billTo == InvoiceBillingTo.Customer) {

    }
    else if (invoiceData.billTo == InvoiceBillingTo.Supplier) {
      var supplierInfo = new InvoiceSupplierInfo();
      supplierInfo.supplierId = invoiceData.quotationSupplierId;
      supplierInfo.billedName = invoiceData.quotationSupplierName;
      supplierInfo.billingAddress = invoiceData.quotationSupplierAddress;
      supplierInfo.contactPersonIdList = invoiceData.quotationSupplierContacts;
      this.invoiceGenerateModel.supplierInfo = supplierInfo;
    }
    else if (invoiceData.billTo == InvoiceBillingTo.Factory) {
      var factoryinfo = new InvoiceSupplierInfo();
      factoryinfo.supplierId = invoiceData.quotationFactoryId;
      factoryinfo.billedName = invoiceData.quotationFactoryName;
      factoryinfo.billingAddress = invoiceData.quotationFactoryAddress;
      factoryinfo.contactPersonIdList = invoiceData.quotationFactoryContacts;
      this.invoiceGenerateModel.supplierInfo = factoryinfo;
    }

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
          this.getStatus(this.model.serviceId);
        },
        error => {
          this.setError(error);
          this.masterModel.serviceLoading = false;
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

  SetSearchTypemodel(item) {
    this.model.searchTypeId = item.id;
    this._customvalidationforbookid = this.model.searchTypeId == this._booksearttypeid
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
    this.masterModel.selectedNumber = item.shortName;

  }

  BookingNoValidation() {
    return this._customValidationForInvoiceNo = this.model.searchTypeId == this.searchId
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
  }

  changeInvoiceSelection(dataRow, event) {
    if (event && event.target.checked) {
      this.isFilterOpen = true;
      if (this.model.items.filter(x => x.selectedToGenerateInvoice).length > 1) {
        var firstItem = this.model.items.find(x => x.selectedToGenerateInvoice && x.bookingId != dataRow.bookingId);

        if (firstItem && dataRow.customerId == firstItem.customerId && dataRow.quotationCurrencyId == firstItem.quotationCurrencyId &&
          (dataRow.billTo == InvoiceBillingTo.Customer) ||
          (dataRow.billTo == InvoiceBillingTo.Supplier && dataRow.quotationSupplierId == firstItem.quotationSupplierId) ||
          (dataRow.billTo == InvoiceBillingTo.Factory && dataRow.quotationFactoryId == firstItem.quotationFactoryId)) {
          dataRow.selectedToGenerateInvoice = true;
          event.target.checked = true;
        }
        else {
          this.showError("Invoice status", "please select same billed Name,customer and currency bookings")
          dataRow.selectedToGenerateInvoice = false;
          event.target.checked = false;
        }
      }
    }

    this.masterModel.isInvoiceCreateVisible = this.model.items.filter(x => x.selectedToGenerateInvoice).length > 0 ? true : false;
  }



  formValid(): boolean {
    let isOk = !this.BookingNoValidation() && this.validator.isValidIf('invoiceToDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('invoiceFromDate', this.IsDateValidationRequired())

    return isOk;
  }

  GetSearchData() {
    this.masterModel.searchloading = true;
    this.model.noFound = false;
    if (this.isFilterOpen)
      this.isFilterOpen = false;
    this.filterDataShown = this.filterTextShown();
    if (this.formValid()) {
      this.service.search(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success) {
              this.masterModel.bangladesh_BankIdList = response.bangladesh_BankId.split(',');
              this.mapPageProperties(response);
              if (this.model.serviceId == APIService.Inspection) {
                this.masterModel._statuslist = response.invoiceStatuslst;
              } else {
                this.masterModel._statuslist = response.invoiceAuditStatusList;
              }
              this.model.items = response.data.map(x => {
                var item: InvoiceItem = {
                  id: x.id,
                  bookingId: x.bookingId,
                  auditId: x.auditId,
                  quotationId: x.quotationId,
                  serviceDate: x.serviceDate,
                  serviceId: x.serviceId,
                  invoiceNo: x.invoiceNo,
                  serviceName: x.serviceName,
                  customerId: x.customerId,
                  supplierId: x.supplierId,
                  factoryId: x.factoryId,
                  customerName: x.customerName,
                  supplierName: x.supplierName,
                  factoryName: x.factoryName,
                  factoryCountry: x.factoryCountry,
                  invoiceTo: x.invoiceTo,
                  invoiceTypeId: x.invoiceTypeId,
                  invoiceTypeName: x.invoiceTypeName,
                  bookingStatusName: x.bookingStatusName,
                  bookingStatusId: x.bookingStatusId,
                  invoiceStatusName: x.invoiceStatusName,
                  invoiceStatusColor: x.invoiceStatusColor,
                  invoiceDate: x.invoiceDate,
                  paymentStatusId: x.paymentStatusId,
                  paymentStatusName: x.paymentStatusName,
                  paymentDate: x.paymentDate,
                  service: x.service,
                  holdType: x.holdType,
                  holdReason: x.holdReason && x.holdReason.length > 35 ? x.holdReason : "",
                  holdReasonTrim: x.holdReason && x.holdReason > 35 ? x.holdReason.substring(0, 35) + "..." : x.holdReason, //35 length
                  isHoldReasonTooltipShow: x.holdReason && x.holdReason.length > 35,
                  quotationStatus: x.quotationStatus,
                  serviceStartDate: x.serviceStartDate,
                  serviceEndDate: x.serviceEndDate,
                  quotationStatusId: x.quotationStatusId,
                  billTo: x.billTo,
                  paymentTerms: x.paymentTerms,
                  quotationSupplierId: x.quotationSupplierId,
                  quotationSupplierName: x.quotationSupplierName,
                  quotationSupplierAddress: x.quotationSupplierAddress,
                  quotationSupplierContacts: x.quotationSupplierContacts,
                  quotationFactoryId: x.quotationFactoryId,
                  quotationFactoryName: x.quotationFactoryName,
                  quotationFactoryAddress: x.quotationFactoryAddress,
                  quotationFactoryContacts: x.quotationFactoryContacts,
                  invoiceTypeClassName: InvoiceTypeClass.find(y => y.id == x.invoiceTypeId) ? InvoiceTypeClass.find(y => y.id == x.invoiceTypeId).className : "",
                  quotationBilledName: x.quotationBilledName,
                  billingEntityId: x.billingEntityId,
                  bankId: x.bankId,
                  bankcurrencyId: x.bankcurrencyId,
                  bankcurrencyName: x.bankcurrencyName,
                  bankName: x.bankName,
                  billingEntityName: x.billingEntityName,
                  quotationBilledTo: x.invoiceTo,
                  taxList: x.taxList,
                  quotationTotalFees: x.quotationTotalFees,
                  exchangeRate: x.exchangeRate,
                  quotationCurrencyCode: x.quotationCurrencyCode,
                  quotationCurrencyName: x.quotationCurrencyName,
                  quotationCurrencyId: x.quotationCurrencyId,
                  customerLegalName: x.customerLegalName,
                  supplierLegalName: x.supplierLegalName,
                  factoryLegalName: x.factoryLegalName,
                  selectedToGenerateInvoice: false,
                  brandNames: x.brandNames,
                  currencyCode: x.currencyCode ? "(" + x.currencyCode + ")" : "",
                  invoiceAmount: x.invoiceAmount,
                  extraFeeInvoiceNo:x.extraFeeInvoiceNo,
                  extraFeesStatusName:x.extraFeesStatusName,
                  extraFeesAmount:x.extraFeesAmount,
                  extraFeesCurrencyCode:x.extraFeesCurrencyCode,
                  extraFeesInvoiceDate:x.extraFeesInvoiceDate,
                  extraFeesInvoiceTo:x.extraFeesInvoiceTo,
                  extraFeesPaymentStatusName:x.extraFeesPaymentStatusName,
                  extraFeesPaymentDate:x.extraFeesPaymentDate,
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

  Reset() {
    this.validator.isSubmitted = false;
    this.onInit();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  onChangeAdditionalTax(event) {
    this.calculateTotalFees();
  }

  export() {
    this.masterModel.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_Statsus_Summary");
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

  //get billing entity list
  getBillingEntityList() {
    this.masterModel.billingEntityLoading = true;
    this.refService.getBillingEntityList()
      .pipe()
      .subscribe(
        response => {
          this.masterModel.billingEntityLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.masterModel.billingEntityList = response.dataSourceList;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.billingEntityLoading = false;
        });
  }

  //get currency list
  getCurrencyList() {
    this.masterModel.currencyLoading = true;
    this.refService.getCurrencyListWithCode()
      .pipe()
      .subscribe(
        response => {
          this.masterModel.currencyLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.masterModel.currencyList = response.dataSourceList;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.currencyLoading = false;
        });
  }


  confirmInvoiceGenerate(content) {

    this.isBankIdMatched = false;

    var selectedBookingItems = [...this.model.items.filter(x => x.selectedToGenerateInvoice)];

    var nonSelectedBookingItems = [...this.model.items.filter(x => !x.selectedToGenerateInvoice
      && x.id == 0 && x.paymentTerms == PaymentTerm.PreInvoice && x.quotationStatusId == 7)];

    var missingBookingInSameQuotationList = [];

    nonSelectedBookingItems.forEach(element => {

      if (element && selectedBookingItems.filter(x => x.quotationId == element.quotationId).length > 0) {
        missingBookingInSameQuotationList.push(element.bookingId);
      }
    });

    if (missingBookingInSameQuotationList.length > 0) {
      this.showWarning("Invoice", "please select all bookings in quotation for " + missingBookingInSameQuotationList.join(','));
      return false;
    }
    this.masterModel.preInvoiceList = selectedBookingItems;
    // take first booking items from the invoice status
    var bookingItem = selectedBookingItems[0];

    if (bookingItem) {
      this.invoiceModel = bookingItem;
      this.masterModel.bankList = [];
      this.masterModel.billingEntityList = [];
      this.masterModel.currencyList = [];
      if (bookingItem.bankId) {
        this.masterModel.bankId = bookingItem.bankId;
        this.masterModel.bankcurrencyId = bookingItem.bankcurrencyId;
        this.masterModel.bankcurrencyName = bookingItem.bankcurrencyName;
      }

      this.getBillingEntityList();
      this.getCurrencyList();
      this.getInvoiceBankList(bookingItem.billingEntityId);
      this.masterModel.inspectionNo = bookingItem.bookingId;
      this.masterModel.serviceDate = this.utility.formatDate(bookingItem.serviceEndDate);
      this.masterModel.quotationBilledTo = bookingItem.quotationBilledTo;
      this.masterModel.quotationBilledName = bookingItem.quotationBilledName;
      this.masterModel.quotationTotalFees = bookingItem.quotationTotalFees;
      this.masterModel.billingEntityId = bookingItem.billingEntityId;
      this.masterModel.quotationCurrencyCode = bookingItem.quotationCurrencyCode;
      this.masterModel.quotationCurrencyName = bookingItem.quotationCurrencyName;
      this.masterModel.quotationCurrencyId = bookingItem.quotationCurrencyId;
      this.masterModel.taxList = bookingItem.taxList;

      if (bookingItem.billTo == BillPaidBy.Customer) {
        this.masterModel.quotationBilledTo = "Customer";
        this.masterModel.quotationBilledName = bookingItem.customerLegalName;
      }
      else if (bookingItem.billTo == BillPaidBy.Supplier) {
        this.masterModel.quotationBilledTo = "Supplier";
        this.masterModel.quotationBilledName = bookingItem.supplierLegalName;
      }
      else if (bookingItem.billTo == BillPaidBy.Factory) {
        this.masterModel.quotationBilledTo = "Factory";
        this.masterModel.quotationBilledName = bookingItem.factoryLegalName;
      }

      this.dialog = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
  }

  getAsDateFromString(dateString) {
    var dateParts = dateString.split("/");
    if (dateParts[0] < 10) {
      dateParts[0] = "0" + dateParts[0];
    }
    if (dateParts[1] < 10) {
      dateParts[1] = "0" + dateParts[1];
    }
    let newdateValue = dateParts[1] + ',' + dateParts[0] + ',' + dateParts[2];
    var dateObject = new Date(newdateValue);
    return dateObject
  }

  setExchangeRate() {
    if (this.masterModel.bankcurrencyId > 0 && this.masterModel.quotationCurrencyId > 0) {
      var actualdate = this.getAsDateFromString(this.masterModel.serviceDate);

      var inspectionDate = actualdate.getDate() + "-" + (actualdate.getMonth()+1) + "-" + actualdate.getFullYear();

      this.exChangeservice.getCurrecyRate(this.masterModel.bankcurrencyId, this.masterModel.quotationCurrencyId, inspectionDate)
        .pipe()
        .subscribe(
          res => {
            this.masterModel.exchangeRate = res;
            this.calculateTotalFees();
          },
          error => {
            this.masterModel.exchangeRate = 1;
            this.calculateTotalFees();
          });
    }

  }

  calculateTotalFees() {
    var totalTaxValue = this.masterModel.taxList.reduce((sum, current) => sum + current.taxValue, 0);


    var totalInvoiceFees = this.masterModel.preInvoiceList.reduce((sum, current) => sum + current.quotationTotalFees, 0);

    var totalFees = (totalInvoiceFees * this.masterModel.exchangeRate);

    // add aditional tax value
    if (this.masterModel.additionalTax > 0) {
      totalFees = totalFees * 100 / (100 - this.masterModel.additionalTax);
    }

    var actualTax = totalFees * totalTaxValue;

    this.masterModel.totalCalculatedAmount = (totalFees + actualTax).toFixed(2);
  }


  numericValidation(event) {
    this.utility.numericValidation(event, 7);
  }


  cancelConfirmInvoiceGenerate() {
    this.dialog.close();
  }

  onChangeBankList(event) {
    if (event) {
      this.isBankIdMatched = true;
      this.setAdditionalTaxValue();
      this.masterModel.bankcurrencyId = event.currencyId;
      this.masterModel.bankcurrencyName = this.masterModel.currencyList.filter(x => x.id == event.currencyId).map(x => x.currencyCode);
      this.masterModel.taxList = event.taxList == null ? [] : event.taxList;
      this.setExchangeRate();
    }
  }

  onChangeCompanyList(event) {
    if (event) {
      this.masterModel.bankId = null;
      this.isBankIdMatched = false;
      this.masterModel.bankcurrencyId = null;
      this.masterModel.exchangeRate = 1;
      this.masterModel.taxList = [];
      this.getInvoiceBankList(event.id);
    }
  }

  setAdditionalTaxValue() {
    this.masterModel.additionalTax = null;
    this.masterModel.showAdditionalTax = false;
    // if the bank is bangaladesh then show the additionall tax
    if (this.masterModel.bangladesh_BankIdList.filter(x => x == this.masterModel.bankId).length > 0) {

      var totalInvoiceFees = this.masterModel.preInvoiceList.reduce((sum, current) => sum + current.quotationTotalFees, 0);
      this.masterModel.showAdditionalTax = true;
      if (totalInvoiceFees < 250000) {
        this.masterModel.additionalTax = 10;
      }
      else if (totalInvoiceFees > 250000) {
        this.masterModel.additionalTax = 12;
      }
    }

  }

  onChangeCurrencyList(event) {
    if (event) {
      this.masterModel.bankcurrencyId = event.id;
      this.masterModel.bankcurrencyName = event.currencyCode;
      this.setExchangeRate();
    }
  }

  onClearCompanyList() {
    this.masterModel.bankId = null;
    this.masterModel.bankList = [];
    this.masterModel.exchangeRate = 1;
    this.masterModel.bankcurrencyId = null;
    this.masterModel.taxList = [];
    this.isBankIdMatched = false;
  }


  onClearBankList() {
    this.masterModel.exchangeRate = 1;
    this.masterModel.bankcurrencyId = null;
    this.masterModel.bankcurrencyName = '';
    this.masterModel.taxList = [];
    this.isBankIdMatched = false;
  }


  getInvoiceBankList(billingEntity) {
    this.masterModel.bankLoading = true;
    this.masterModel.bankList = [];
    this.refService.getBankList(billingEntity)
      .pipe()
      .subscribe(
        response => {
          if (response) {
            this.masterModel.bankList = response;
          }
          this.masterModel.bankLoading = false;

          // check bank id is matched or not
          if (this.masterModel.bankId && this.masterModel.bankList) {
            var bankDataMapped = this.masterModel.bankList.filter(x => x.id == this.masterModel.bankId);
            if (bankDataMapped.length >= 1) {
              this.isBankIdMatched = true;
              this.setAdditionalTaxValue();
              this.setExchangeRate();
            }
          }

        },
        error => {
          this.setError(error);
          this.masterModel.bankLoading = false;
        });
  }

  isInvoiceFormValid() {
    var isValid = false;
    if (this.masterModel.billingEntityId > 0 && this.masterModel.bankId > 0) {
      isValid = true;
    }
    else if (!(this.masterModel.billingEntityId > 0)) {
      this.showWarning("Invoice", "Billing Entity is mandatory");
    }
    else if (!(this.masterModel.bankId > 0)) {
      this.showWarning("Invoice", "Bank Id is mandatory");
    }
    return isValid;
  }


  async generateInvoice() {

    if (this.isInvoiceFormValid()) {

      var bookingItem = this.invoiceModel;


      if (!this.isBankIdMatched) {
        this.showError("Invoice", "Please Select Valid Bank");
        return false;
      }

      if (this.masterModel.bankcurrencyId > 0 && this.masterModel.bankId > 0) {
        if (!this.masterModel.exchangeRate || this.masterModel.exchangeRate == 0) {
          this.showError("Invoice", "Please enter ExchangeRate");
          return false;
        }
      }

      this.setInvoiceModel();
      this.setBilltoInformation(bookingItem);
      if (bookingItem && this.invoiceGenerateModel) {
        this.masterModel.invoiceGenerateLoading = true;
        this.invoiceGenerateModel.customerId = bookingItem.customerId;
        this.invoiceGenerateModel.service = bookingItem.serviceId;
        // tax calculation values
        this.invoiceGenerateModel.billingEntity = this.masterModel.billingEntityId;
        this.invoiceGenerateModel.bankAccount = this.masterModel.bankId;
        this.invoiceGenerateModel.currencyId = this.masterModel.bankcurrencyId;
        this.invoiceGenerateModel.exchangeRate = this.masterModel.exchangeRate;
        this.invoiceGenerateModel.additionalTax = this.masterModel.additionalTax;
        let bookingIds = this.model.items.filter(x => x.selectedToGenerateInvoice).map(a => a.bookingId);
        let serviceStartDateList = this.model.items.filter(x => x.selectedToGenerateInvoice).map(a => this.getAsDateFromString(this.utility.formatDate(a.serviceEndDate)));
        var uniqueServiceStartDate = serviceStartDateList.filter((v, i, a) => a.indexOf(v) === i);

        let startDate = new Date(Math.min.apply(null, uniqueServiceStartDate));
        let endDate = new Date(Math.max.apply(null, uniqueServiceStartDate));

        this.invoiceGenerateModel.realInspectionFromDate = new NgbDate(startDate.getFullYear(), startDate.getMonth() + 1, startDate.getDate());
        this.invoiceGenerateModel.realInspectionToDate = new NgbDate(endDate.getFullYear(), endDate.getMonth() + 1, endDate.getDate());

        let response: InvoiceGenerateResponse;
        this.invoiceGenerateModel.bookingNoList = [];
        if (bookingIds)
          this.invoiceGenerateModel.bookingNoList = bookingIds;
        try {

          response = await this.editInvoiceService.generateInvoice(this.invoiceGenerateModel);
          this.masterModel.invoiceGenerateLoading = false;
          this.masterModel.isInvoiceCreateVisible = false;
          this.dialog.close();
        }
        catch (e) {
          console.error(e);
          this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.invoiceGenerateLoading = false;
          this.dialog.close();
        }

        if (response) {
          this.dialog.close();
          this.processGenerteInvoiceResponse(response, bookingItem.customerId,bookingItem.bookingId);

        }

      }
    }
  }

  keyPressNumbersWithDecimal(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
      return false;
    }
    return true;
  }

  processGenerteInvoiceResponse(response: InvoiceGenerateResponse, customerId,bookingId) {
    this.invoiceGenerateResponse = response;
    switch (response.result) {
      case InvoiceGenerateResult.Success: {
        this.SearchDetails();
        this.processGenerateInvoiceSuccessResponse(this.invoiceGenerateResponse.invoiceData[0], customerId,InvoicePreviewFrom.InvoiceStatusGenerate,"Invoice Generated Successfully",bookingId);
        break;
      }
      case InvoiceGenerateResult.RequestIsNotValid:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_REQ_NOT_VALID');
        break;
      case InvoiceGenerateResult.NoPricecardRuleFound:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_PRICE_CARD_FOUND');
        break;
      case InvoiceGenerateResult.NoInspectionFound:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_INSPECTION_FOUND');
        break;
      case InvoiceGenerateResult.NoRuleMapped:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_RULE_MAPPED');
        break;
      case InvoiceGenerateResult.FutureDateNotAllowed:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_FUTURE_DATE_NOT_ALLOWED');
        break;
      case InvoiceGenerateResult.FromDateAfterToDate:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_TO_DATE_GREATER_FROM_DATE');
        break;
      case InvoiceGenerateResult.NoSupplierSelected:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_SUPPLIER_SELECTED');
        break;
      case InvoiceGenerateResult.SupplierIsRequired:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_SUPPLIER_REQ_INFO');
        break;
      case InvoiceGenerateResult.TravelOrInspectionRequired:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_TRAVEL_INSPECTION_FEES');
        break;

      case InvoiceGenerateResult.NoInvoiceConfigured:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_INVOICE_CONFIGURED');
        break;

      case InvoiceGenerateResult.BankIsRequired:
        this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_Bank_REQ');
        break;
    }
    this.masterModel.invoiceGenerateLoading = false;
  }

  processGenerateInvoiceSuccessResponse(invoiceNumber:string, customerId,callFrom :InvoicePreviewFrom,previewTitle:string,bookingId:number) {
    if (invoiceNumber) {
      this.invoicePreviewRequest= new InvoicePreviewRequest();    

      this.invoicePreviewRequest.customerId=customerId;
      this.invoicePreviewRequest.invoiceNo=invoiceNumber;
      this.invoicePreviewRequest.invoiceData=[invoiceNumber];
      this.invoicePreviewRequest.invoicePreviewTypes=this.invoicePreviewTypes;
      this.invoicePreviewRequest.invoicePreviewFrom=callFrom;
      this.invoicePreviewRequest.previewTitle=previewTitle;
      this.invoicePreviewRequest.service= APIService.Inspection;
      this.invoicePreviewRequest.bookingData=[bookingId];
    }
    this.dialog = this.modalService.open(this.invoicePreviewTemplateModal, { windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  showInvoicePreview(bookingItem)
  {
    this.invoiceGenerateResponse= new InvoiceGenerateResponse();
    this.invoiceGenerateResponse.invoiceData=[bookingItem.invoiceNo];
    this.processGenerateInvoiceSuccessResponse(bookingItem.invoiceNo,bookingItem.customerId,InvoicePreviewFrom.InvoiceStatusSummary,"Invoice Preview",bookingItem.bookingId);
  }

  closeInvoicePreview(){
    this.dialog.close()
  }


  redirectToEditInvoice(invoiceNo) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditInvoice + invoiceNo + "/" + this.invoiceGenerateModel.service;
    window.open(editPage);
  }

  navigateInvoiceSummary(invoiceNo) {
    if (this.dialog) {
      this.dialog.dismiss();
      this.dialog = null;
    }
    // this.returnwithsearchparam('invoicesummary/invoice-summary', invoiceNo);
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

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.requestSupModel.customerId = this.model.customerId;
    }
    else {
      this.requestSupModel.customerId = null;
    }
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
    if (this.model.customerId && this.model.customerId > 0) {
      this.requestSupModel.customerId = this.model.customerId;
    }
    else {
      this.requestSupModel.customerId = 0;
    }

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
  getFactoryListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.requestFactoryModel.customerId = this.model.customerId;
    }
    else {
      this.requestFactoryModel.customerId = null;
    }
    if (this.model.supplierId) {
      this.requestFactoryModel.supplierId = this.model.supplierId;
    }
    else {
      this.requestFactoryModel.supplierId = 0;
    }

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
    if (this.model.customerId) {
      this.requestFactoryModel.customerId = this.model.customerId;
    }
    else {
      this.requestFactoryModel.customerId = 0;
    }
    if (this.model.supplierId) {
      this.requestFactoryModel.supplierId = this.model.supplierId;
    }
    else {
      this.requestFactoryModel.supplierId = 0;
    }
    this.requestFactoryModel.supplierType = SupplierType.Factory;
    this.masterModel.factoryLoading = true;
    this.supService.getFactoryDataSourceList(this.requestFactoryModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.factoryList = this.masterModel.factoryList.concat(data);
        }
        if (isDefaultLoad)
          this.requestFactoryModel = new CommonDataSourceRequest();
        this.masterModel.factoryLoading = false;
      }),
      error => {
        this.masterModel.factoryLoading = false;
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

  clearFactorySelection() {
    this.model.factoryId = null;
    this.getFactoryListBySearch();
  }

  clearSupplierSelection() {
    this.model.supplierId = null;
    this.getFactoryListBySearch();
  }

  clearCustomerSelection() {
    this.model.customerId = null;
    this.model.supplierId = null;
    this.model.factoryId = null;
    this.model.brandIdList = [];
    this.getFactoryListBySearch();
    this.getSupListBySearch();
    this.masterModel.customerBrandList = [];
  }
  onChangeSupplier(supitem) {
    this.model.factoryId = null;
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
  onChangeCustomer(cusitem) {
    //clear the list
    // this.bookingMasterData.supplierList = [];
    // this.bookingMasterData.factoryList = [];
    //clear the selected values
    this.model.supplierId = null;
    this.model.factoryId = null;
    this.model.brandIdList = [];
    this.getFactoryListBySearch();
    this.getSupListBySearch();
    this.masterModel.customerBrandList = [];


    if (cusitem != null && cusitem.id > 0) {
      this.getCustomerBrands(cusitem.id);
      var customerDetails = this.masterModel.customerList.find(x => x.id == cusitem.id);
      if (customerDetails)
        this.masterModel.customerName =
          customerDetails.name.length > BookingSummaryNameTrim ?
            customerDetails.name.substring(0, BookingSummaryNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.masterModel.customerName = "";
    }
  }


  getInvoiceStatusList() {
    this.masterModel.statusLoading = true;
    this.masterModel.invoiceStatusList = InvoiceStatusSummaryStatusList;
    this.masterModel.statusLoading = false;
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

  SetSearchDatetype(item) {
    this.model.datetypeid = item.id;
    this.masterModel.DateName = item.name;
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
    console.log(this.isMobile);
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
  //navigate to Invoice page
  navigateToEditInvoice(invoiceNo, serviceId) {
    var invoicePage = 'api/editinvoice/edit-invoice/' + invoiceNo + '/' + serviceId;
    window.open(invoicePage);
  }
  //navigate to booking page
  navigateToBooking(bookingNo, serviceId) {

    var bookingPage = '';
    if (serviceId == APIService.Inspection) {
      bookingPage = 'api/inspedit/edit-booking/' + bookingNo;

    }
    else if (serviceId == APIService.Audit) {
      bookingPage = 'api/auditedit/edit-audit/' + bookingNo;
    }

    window.open(bookingPage);

  }
  //navigate to quotation page
  navigateToQuotation(quotationNo) {
    var quotationPage = 'api/quotations/edit-quotation/' + quotationNo;
    window.open(quotationPage);
  }

  clickStatus(id) {
    if (id && id > 0) {

      //if it contains the value
      var isValueExists = this.model.statusidlst.includes(id);

      //open the filter
      if (!this.isFilterOpen)
        this.isFilterOpen = true;

      //open the advance search

      this.toggleFormSection = false;

      if (isValueExists) {
        this.model.statusidlst = this.model.statusidlst.filter(x => x != id);
        this.model.statusidlst = [...this.model.statusidlst];

      }
      else {
        this.model.statusidlst.push(id);
        this.model.statusidlst = [...this.model.statusidlst];
      }
    }
  }


  changeStatus(id) {
    return this.model.statusidlst.includes(id);
  }

  statusChange(item) {
    if (item) {

      if (this.model.statusidlst && this.model.statusidlst.length > 0) {

        var statusLength = this.model.statusidlst.length;

        var statusDetails = [];
        var maxLength = statusLength > 1 ? 1 : statusLength;

        for (var i = 0; i < maxLength; i++) {
          statusDetails.push(this.masterModel.bookingStatusList.find(x => x.id == this.model.statusidlst[i]).name);
        }

        if (statusLength > 1) {
          statusDetails.push(" " + (statusLength - 1) + "+");
        }
        this.masterModel.statusNameList = statusDetails;
      }
      else {
        this.masterModel.statusNameList = [];
      }
    }
  }

  changeService(serviceId) {
    this.model.statusidlst = [];
    this.getStatus(serviceId);
  }

  getStatus(serviceId: number) {
    if (serviceId) {
      this.masterModel.bookingstatusLoading = true;
      this.service.getStatusbyService(serviceId)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.masterModel.bookingStatusList = result.dataSourceList;
            }
            else {
              this.masterModel.bookingStatusList = [];
            }
            this.masterModel.bookingstatusLoading = false;
          },
          error => {
            this.masterModel.bookingStatusList = [];
            this.masterModel.bookingstatusLoading = false;
          });
    }
    else {
      this.masterModel.bookingStatusList = [];
    }
  }


  //get the office list
  getOfficeLocationList() {
    this.masterModel.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processOfficeLocationResponse(response);
        },
        error => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        });
  }

  //process the office location response
  processOfficeLocationResponse(response) {
    if (response && response.result == ResponseResult.Success)
      this.masterModel.officeList = response.dataSourceList;
    else if (response && response.result == ResponseResult.NoDataFound)
      this.showError('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.TITLE.MSG_OFFICE_NOT_FOUND');
    this.masterModel.officeLoading = false;
  }
  getSearchDetails() {
    var searchLoading = true;
    if (this.model.searchTypeText && this.model.searchTypeText != '') {
      if (!this.isFilterOpen)
        this.isFilterOpen = this.model.searchTypeId != this._booksearttypeid && !(this.model.customerId > 0);
      this.masterModel.isInvoiceCreateVisible = false;
      this.SearchDetails();
    }
  }

  isfilterData() {
    if ((this.model.invoiceFromDate && this.model.invoiceFromDate != '') ||
      (this.model.invoiceToDate && this.model.invoiceToDate != '') ||
      this.model.searchTypeId > 0 ||
      this.model.customerId > 0 || this.model.supplierId > 0
      || (this.model.factoryId > 0) || this.model.serviceId ||
      (this.model.officeIdList && this.model.officeIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0) ||
      (this.model.paymentStatusIdList && this.model.paymentStatusIdList.length > 0) ||
      (this.model.brandIdList && this.model.brandIdList.length > 0)
    ) {
      this.filterDataShown = true;
    }
    else {
      this.filterDataShown = false;
    }
  }

  isShownColumn() {
    this.masterModel.isShowColumn = this.masterModel.isShowColumn ?
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

  officeChange(item) {
    if (item) {

      if (this.model.officeIdList && this.model.officeIdList.length > 0) {

        var officeLength = this.model.officeIdList.length;

        var officeDetails = [];
        var maxLength = officeLength > 1 ? 1 : officeLength;
        for (var i = 0; i < maxLength; i++) {

          officeDetails.push(this.masterModel.officeList.find(x => x.id == this.model.officeIdList[i]).name);
        }

        if (officeLength > 1) {
          officeDetails.push(" " + (officeLength - 1) + "+");
        }
        this.masterModel.officeNameList = officeDetails;
      }
      else {
        this.masterModel.officeNameList = [];
      }
    }
  }

  filterTextShown() {
    var isFilterDataSelected = false;

    if ((this.model.invoiceFromDate && this.model.invoiceFromDate != '') ||
      (this.model.invoiceToDate && this.model.invoiceToDate != '') || this.model.customerId > 0 ||
      this.model.supplierId > 0 || this.model.serviceId > 0 || this.model.searchTypeId > 0
      || (this.model.factoryId > 0) || this.model.invoiceType > 0 ||
      (this.model.invoiceStatusId && this.model.invoiceStatusId.length > 0) ||
      (this.model.officeIdList && this.model.officeIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0)
    ) {

      //desktop version
      if (!this.isMobile) {
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;
        //date add
        count = MobileViewFilterCount + count;

        if (this.model.customerId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.serviceId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.invoiceType > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.officeIdList && this.model.officeIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.invoiceStatusId && this.model.invoiceStatusId.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.statusidlst && this.model.statusidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.masterModel.supplierName = "";
        this.masterModel.customerName = "";
        this.masterModel.factoryName = "";
        this.masterModel.officeNameList = [];
        this.masterModel.statusNameList = [];
      }
    }

    return isFilterDataSelected;
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

    this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest)

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
          this.showError('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
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
        this.showError('INVOICE_STATUS.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_STATUS_NOT_FOUND');
      }
      this.masterModel.invoicePaymentStatusLoading = false;
    }
  }

  getCustomerBrands(id) {
    this.masterModel.customerBrandLoading = true;
    this.cusService.getCustomerBrands(id)
      .pipe()
      .subscribe(response => {
        if (response && response.result == ResponseResult.Success) {
          this.masterModel.customerBrandList = response.dataSourceList;
          this.masterModel.customerBrandLoading = false;
        }
        else {
          this.masterModel.customerBrandList = [];
          this.masterModel.customerBrandLoading = false;
        }
      },
        error => {
          this.masterModel.customerBrandLoading = false;
        });
  }

  isCommunicationPopupFormValid(): boolean {
    let isOk = this.communicationvalidator.isValid('comment');

    if (isOk && (!this.masterModel.invoiceNumber)) {
      this.showWarning('INVOICE_STATUS.LBL_TITLE', 'INVOICE_STATUS.LBL_INVOICE_NUMBER_REQ');
    }
    return isOk;
  }

  invoiceCommunicationMap() {
    this.communicationvalidator.isSubmitted = true;
    this.communicationvalidator.initTost();
    this.communicationSaveModel.invoiceNo = this.masterModel.invoiceNumber;
    if (this.isCommunicationPopupFormValid()) {
      this.communicationInvoiceSave(this.communicationSaveModel);
    }

  }

  openPopup(invoiceNumber: string, content) {
    this.masterModel.invoiceGenerateLoading = true;
    this.masterModel.invoiceNumber = invoiceNumber;
    this.communicationSaveModel.comment = "";
    this.communicationvalidator = Validator.getValidator(this.communicationSaveModel, "invoice/invoice-communication.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
    this.communicationvalidator.isSubmitted = false;
    //invoiceCommunicationPopup
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.communicationInvoiceSummaryData(invoiceNumber);
  }

  //Communication Save
  async communicationInvoiceSave(saveModel: InvoiceCommunicationSaveRequest) {
    var response: InvoiceCommunicationSaveResponse;
    this.masterModel.communicationSaveLoading = true;
    try {
      response = await this.service.communicationSave(saveModel);
      this.communicationInvoiceSaveResponse(response);
    }
    catch (e) {
      console.error(e);
      this.showError('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      this.masterModel.communicationSaveLoading = false;
    }
  }

  //process the Communication Save reponse
  communicationInvoiceSaveResponse(response: InvoiceCommunicationSaveResponse) {
    if (response) {
      switch (response.result) {
        case InvoiceCommunicationSaveResultResponse.Success:
          this.showSuccess('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');
          this.communicationSaveModel.comment = "";
          this.communicationInvoiceSummaryData(this.masterModel.invoiceNumber);
          break;
        case InvoiceCommunicationSaveResultResponse.Failed:
          this.showError('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
      this.masterModel.communicationSaveLoading = false;
    }
  }

  //assign validator
  assignValidator() {
    this.validator.setJSON("invoice/invoice-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
  }

  //Communication summary data
  async communicationInvoiceSummaryData(invoiceNumber: string) {
    var response: InvoiceCommunicationTableResponse;
    this.validator.isSubmitted = true;

    try {
      this.masterModel.communicationTableLoading = true;

      response = await this.service.communicationSummary(invoiceNumber);
      this.communicationInvoiceSummaryResponse(response);
    }
    catch (e) {
      console.error(e);
      this.showError('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      this.masterModel.communicationTableLoading = false;
    }
  }

  //process the Communication summary reponse
  communicationInvoiceSummaryResponse(response: InvoiceCommunicationTableResponse) {
    if (response) {
      switch (response.result) {
        case InvoiceCommunicationTableResultResponse.Success:
          this.masterModel.invoiceCommunicationTableList = response.invoiceCommunicationTableList;
          break;
        case InvoiceCommunicationTableResultResponse.Failed:
          this.masterModel.invoiceCommunicationTableList = [];
          this.showError('INVOICE_STATUS.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
        case InvoiceCommunicationTableResultResponse.NotFound:
          this.masterModel.invoiceCommunicationTableList = [];
          break;
      }
      this.masterModel.invoiceGenerateLoading = false;
      this.masterModel.communicationTableLoading = false;
    }
  }

  checkHideInvoiceFeatureExist() {
    this.refService.isEntityFeatureExist(EntityFeature.HideMonthlyInvoiceForOperationTeamInvoiceStatus)
      .subscribe((res: boolean) => {
        this.masterModel.hasHideMonthlyInvocieInvoiceStatus = res;
        if (res && !this.hasAccountRole) {
          this.model.invoiceType = InvoiceType.PreInvoice;
        }
      })
  }

  sendInvoiceEmail($event)
  {
    this.masterModel.emailRuleRequest=new BookingReportRequest();
    this.masterModel.emailRuleRequest.bookingIdList=$event.bookingData;
    this.masterModel.emailRuleRequest.serviceId=APIService.Inspection;
    this.masterModel.emailRuleRequest.emailSendingtype=this.emailSendingType.InvoiceStatus;
    this.getEmailRuleData();
  }

  
  popupOk(): boolean {
    return !(this.masterModel.emailRuleList.filter(x => x.isSelected).length > 0);
  }
  // get email rule list from email config
  getEmailRuleData() {
    this.masterModel.emailRuleLoading=true;
    this.emailSendService.getEmailRuleData(this.masterModel.emailRuleRequest)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.masterModel.emailRuleLoading=true;
          this.processSuccessemailRuleData(response);
        },
        error => {
          this.masterModel.emailRuleLoading=false;
          this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE'));
        });
  }

  processSuccessemailRuleData(response) {
    if (response) {

      if (response.result == EmailSendResult.OneRuleFound) {
        if (response.bookingIdsWithoutRule && response.bookingIdsWithoutRule.length > 0)//show warning if booking doesn't have a rule
        {
          this.masterModel.emailRuleRequest.bookingIdList = response.bookingIdsWithRule;
          this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.ruleId = response.ruleId;
        this.emailPreview();
      }
      else if (response.result == EmailSendResult.MoreThanOneRuleFound) {

        if (response.bookingIdsWithoutRule && response.bookingIdsWithoutRule.length > 0)//show warning if booking doesn't have a rule
        {
          this.masterModel.emailRuleRequest.bookingIdList = response.bookingIdsWithRule;
          this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.emailRuleList = response.emailRuleList;
        this.dialog.close();   
        this.dialog = this.modalService.open(this.emailruleinfo, { windowClass: "lgModelWidth", centered: true, keyboard: false, backdrop: 'static' });
      }
      else if (response.result == EmailSendResult.NoRuleFound) { //no rule found 
        this.masterModel.isNoRuleFound = true;
        this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE'));
      }
      else if (response.result == EmailSendResult.EachBookingHasDifferentRule) {
        this.masterModel.isEachBookingHasDifferentRule = true;
      }
      else {
        this.masterModel.isNoRuleFound = true;
        this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE'));
      }
    }
  }

  confirmEmailRule() 
  {
      this.masterModel.emailRuleLoading=true;
      this.emailPreview();
  }

  getRuleId(event: number) {
    if (event && event > 0)
      this.masterModel.ruleId = event;
  }

  closeEmailPreview($event) {
    this.dialog.close();
  }

  emailPreview() {
    this.emailPreviewModel = new EmailPreviewRequest();
    this.emailPreviewModel.emailReportAttachment = [];

    var emailPreviewData=new EmailReportPreviewDetail();
    emailPreviewData.bookingId=this.masterModel.emailRuleRequest.bookingIdList[0];     

    this.emailPreviewModel.emailReportPreviewData= [];
    this.emailPreviewModel.emailReportPreviewData.push(emailPreviewData);
    this.emailPreviewModel.emailRuleId = this.masterModel.ruleId;
    this.emailPreviewModel.esTypeId = EmailSendingType.InvoiceStatus;

    
   // this.masterModel.emailRuleRequest.bookingIdList = distinctBookingNoList;

   // this.masterModel.pageLoader = true;
    this.emailSendService.getEmailDetails(this.emailPreviewModel)
      .subscribe(res => {
        if (res.result == EmailPreviewResponseResult.success) {
          if (res.data)
            this.processEmailPreviewService(res.data);
            this.masterModel.emailRuleLoading=false;
        }
        else if (res.result == EmailPreviewResponseResult.failure) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.emailRuleLoading=false;
         // this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.emailrulenotvalid) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_EMAIL_RULE_NOT_VALID');
          this.masterModel.emailRuleLoading=false;
         // this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.inspectionsummarylinknotavailable) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_INSP_SUMMARY_LINK_NOT_AVAILABLE');
          this.masterModel.emailRuleLoading=false;
         // this.masterModel.pageLoader = false;
        }
      },
        error => {
         // this.masterModel.pageLoader = false;
         this.masterModel.emailRuleLoading=false;
        });
  }

  //process the email service response
  processEmailPreviewService(res) {
    if (res && res.length > 0) {

     this.serialNumberListToEmailPreview=res.map(x => x.invoiceNo);
     this.emailPreviewTitle="Invoice Send";

      this.emailModelItem = res.map(x => {
        var item: EmailPreviewData = {
          emailSubject: x.emailSubject,
          emailSubjectDisplay: x.emailSubject ? x.emailSubject.length >= 75 ? x.emailSubject.substring(0, 75) + '...' : x.emailSubject : "",
          emailBody: x.emailBody,
          emailCCList: x.emailCCList,
          emailBCCList: x.emailBCCList,
          emailValidOption: x.emailValidOption,
          ruleId: x.ruleId,
          emailToList: x.emailToList,
          attachmentList: x.attachmentList,
          reportBookingList: x.reportBookingList,
          emailId: x.emailId,
          isEmailSelected: false,
          isEmailValid: true,
          active: false,
          isenabled: true,
          ccMailText: "",
          toMailText: "",
          bccMailText: "",
          customerId:this.model.customerId
        }
        return item;
      });

      this.masterModel.emailRuleLoading=false;
      this.dialog = this.modalService.open(this.emailPreviewContent, { windowClass: "mailer-popup", centered: true, backdrop: 'static' });
     // this.masterModel.pageLoader = false;
    }
    else if (res.length == 0) {
      this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.emailRuleLoading=false;
     // this.masterModel.pageLoader = false;
    }
    else {
      this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.emailRuleLoading=false;
     // this.masterModel.pageLoader = false;
    }
  }

    //sending the mail
    sendEmail($event) {
      this.masterModel.emailSendLoading = true;
      var emailItems = $event.filter(x => x.isEmailSelected == true);
      this.emailSendService.sendEmail(emailItems)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response) {
              this.showSuccess('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_SEND_SHORTLY');
              this.masterModel.emailSendLoading = false;
              this.dialog.close();             
            }
            else {
              this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
            }
            this.masterModel.emailSendLoading = false;
          },
          error => {
            this.masterModel.emailSendLoading = false;
            this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
          });
    }

}
