import { InoiceDataAccessService } from 'src/app/_Services/invoice/invoicedataaccess.service';
import { mode } from 'crypto-js';
import { BillingMethod } from 'src/app/components/common/static-data-common';
import { Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { error } from 'protractor';
import { of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, DataSourceResponse, InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { BookingDataSourceRequest, EditExtraFee, EditExtraFeesMaster, EditExtraFeeType, ExtraFeeResponseResult } from 'src/app/_Models/invoice/editextrafeesinvoice.model';
import { InvoiceBankGetResult } from 'src/app/_Models/invoice/invoicebank';
import { UserModel } from 'src/app/_Models/user/user.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { CustomerContactService } from 'src/app/_Services/customer/customercontact.service';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { ExpenseService } from 'src/app/_Services/expense/expense.service';
import { EditExtraFeeService } from 'src/app/_Services/invoice/editextrafee.service';
import { EditInvoiceService } from 'src/app/_Services/invoice/editinvoice.service';
import { InvoiceBankService } from 'src/app/_Services/invoice/invoicebank.service';
import { InvoiceSummaryService } from 'src/app/_Services/invoice/invoicesummary.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper } from '../../common/jsonHelper';
import { APIService, BillPaidBy, CustomerContact, ExtraFeeStatus, FactoryContact, InvoicePaymentStatus, InvoicePreviewType, RoleEnum, SupplierContact, SupplierType } from '../../common/static-data-common';
import { Validator } from '../../common/validator';
import { InvoicePreviewComponent } from '../../shared/invoice-preview/invoice-preview.component';
import { InvoiceBilledAddressResponse, InvoiceBilledAddressResult } from 'src/app/_Models/invoice/editinvoice.model';
import { InvoiceBillingTo } from 'src/app/_Models/customer/customer-price-card.model';

@Component({
  selector: 'app-edit-extra-fees',
  templateUrl: './edit-extra-fees.component.html'
})
export class EditExtraFeesComponent extends DetailComponent {
  masterData: EditExtraFeesMaster;
  model: EditExtraFee;
  extraFeeTypeModel: EditExtraFeeType;
  currentUser: UserModel;
  public _roleEnum = RoleEnum;
  extraFeesValidators: Array<any> = [];
  jsonHelper: JsonHelper;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  extraFeeStatus = ExtraFeeStatus;
  public modelRef: NgbModalRef;
    public contatLabel: string = "Contact";
    invoicePreviewRequest = new InvoicePreviewRequest();
    @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;
  DefaultExchangeRate: number = 1;
  isManualInvoice = false;
  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService, private invoiceBankService: InvoiceBankService,
    private cusService: CustomerService, public validator: Validator, public exChangeservice: ExpenseService,
    private supService: SupplierService, public customerPriceCardService: CustomerPriceCardService,
    private bookingService: BookingService, private customerCheckPointService: CustomerCheckPointService,
    public referenceService: ReferenceService, public editInvoiceService: EditInvoiceService,
    public extraFeeService: EditExtraFeeService, private invoiceSummaryService: InvoiceSummaryService,
    authservice: AuthenticationService, public utility: UtilityService, public modalService: NgbModal,
    private cusContService: CustomerContactService) {

    super(router, route, translate, toastr, modalService);
    this.currentUser = authservice.getCurrentUser();

    this.validator.setJSON("invoice/edit-extra-fee.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;

    this._translate = translate;
    this._toastr = toastr;
  }

  onInit(id?: number) {
    this.masterData = new EditExtraFeesMaster();
    this.model = new EditExtraFee();
    this.masterData.isAccountRole = this.currentUser.roles.filter(x => x.id == this._roleEnum.Accounting).length > 0;
    this.masterData.isAERole = this.currentUser.roles.filter(x => x.id == this._roleEnum.InspectionVerified).length > 0;

    this.masterData.isExtraInvoiceNoDisabled = true;

    this.getBillingToList();
    this.getCurrencyList();
    this.getServiceList();
    this.getBillingEntityList();
    this.getInvoicePaymentStatus();
    this.getInvoiceExtraTypeList();
    this.getOfficeList();
    this.getInvoicePaymentTermList();

    if (id && id > 0) {
      this.edit(id);
    }
    else {
      this.model.exchangeRate = this.DefaultExchangeRate;
      // this.getCustomerListBySearch();
      this.AddExtraTypeFeeRow();
    }
  }

  //add a extra type fee in the table
  AddExtraTypeFeeRow() {

    var extraFeeType: EditExtraFeeType = {
      id: 0,
      typeId: null,
      fees: null,
      remarks: ''
    };

    this.model.extraFeeTypeList.push(extraFeeType);

    this.extraFeesValidators.push({
      extraFeeType: extraFeeType,
      validator: Validator.getValidator(extraFeeType, "invoice/edit-extra-fee-type.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
    });
  }

  //fetch the first 10 customer on load
  getCustomerListBySearch() {
    if (this.model.customerId) {
      this.masterData.customerModelRequest.id = this.model.customerId;
    }
    this.masterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest, term)
        : this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterData.customerList = data;
        this.masterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.customerModelRequest.searchText = this.masterData.customerInput.getValue();
      this.masterData.customerModelRequest.skip = this.masterData.customerList.length;
    }
    this.masterData.supplierModelRequest.serviceId = this.model.serviceId;
    if (this.model.customerId) {
      this.masterData.customerModelRequest.customerId = this.model.customerId;
    }
    this.masterData.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.customerList = this.masterData.customerList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.customerModelRequest = new CommonDataSourceRequest();
        this.masterData.customerLoading = false;
      }),
      error => {
        this.masterData.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomer(event) {
    // this.masterData.bookingNumberList = [];
    // this.model.bookingNumberId = null;

    // this.masterData.invoiceNumberList = [];
    // this.model.invoiceNumberId = null;

    this.model.factoryId = null;
    this.masterData.factoryList = [];

    if (event) {
      this.model.supplierId = null;
      this.getSupListBySearch();
      if (this.model.serviceId > 0) {
        // this.getBookingListBySearch();
      }
    }
    else {
      this.model.supplierId = null;
      this.masterData.supplierList = [];
    }
  }

  clearCustomerSelection() {

    this.getCustomerData(true);

    this.model.factoryId = null;
    this.masterData.factoryList = [];

  }

  //fetch the supplier data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.supplierModelRequest.searchText = this.masterData.supplierInput.getValue();
      this.masterData.supplierModelRequest.skip = this.masterData.supplierList.length;
    }
    this.masterData.supplierModelRequest.serviceId = this.model.serviceId;
    this.masterData.supplierModelRequest.customerId = this.model.customerId;
    this.masterData.supplierModelRequest.supplierType = SupplierType.Supplier;
    this.masterData.supplierLoading = true;
    this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.supplierList = this.masterData.supplierList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.supplierModelRequest = new CommonDataSourceRequest();
        this.masterData.supplierLoading = false;
      }),
      error => {
        this.masterData.supplierLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.masterData.supplierModelRequest.id = this.model.supplierId;
    this.masterData.supplierList = null;
    this.masterData.supplierModelRequest.supplierType = SupplierType.Supplier;
    this.masterData.supplierModelRequest.serviceId = this.model.serviceId;
    this.masterData.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterData.supplierList = data;
        this.masterData.supplierLoading = false;
      });
  }

  clearSupplierDetails() {
    this.getSupplierData(true);
  }

  changeSupplier(event) {

    if (event) {
      this.model.factoryId = null;
      this.getFactListBySearch();
    }
    else {
      this.model.factoryId = null;
      this.masterData.factoryList = null;
    }

  }

  // get bank details by bank id
  getBankDetails(id) {
    this.invoiceBankService.getBankDetails(id).then((res) => {
      if (res && res.result === InvoiceBankGetResult.success) {
        this.model.invoiceCurrencyId = parseInt(res.bankDetails.accountCurrency);
        this.setExchangeRate();
      }
    });
  }

  //getBillingToList
  getBillingToList() {
    this.masterData.billedToLoading = true;
    this.getInvoiceBilledAddress();
    this.customerPriceCardService.getBillingToList()
      .pipe()
      .subscribe(
        response => {
          this.processBillToResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterData.billedToLoading = false;
        });
  }

  //process the bill to list response
  processBillToResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {
        this.masterData.billedToList = response.dataSourceList;
      }
      if (response.result == ResponseResult.NoDataFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_BILL_TO_NOT_FOUND');
      }
      this.masterData.billedToLoading = false;
    }
  }

  //get currency list
  getCurrencyList() {
    this.masterData.currencyLoading = true;
    this.bookingService.GetCurrency()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.currencyList = response.currencyList;
          this.masterData.currencyLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.currencyLoading = false;
        });
  }

  //get the service list
  getServiceList() {
    this.masterData.serviceLoading = true;
    this.customerCheckPointService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.masterData.serviceList = response.serviceList;
            this.model.serviceId = APIService.Inspection;
            this.getBookingListBySearch();
          }
          this.masterData.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.serviceLoading = false;
        });
  }

  //get billing entity list
  getBillingEntityList() {
    this.masterData.billingEntityLoading = true;
    this.referenceService.getBillingEntityList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.billingEntityList = response.dataSourceList;
          this.masterData.billingEntityLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.billingEntityLoading = false;
        });
  }

  //get inovoice bank list
  getInvoiceBankList(billingEntityId: number) {
    this.masterData.bankLoading = true;
    this.referenceService.getBankList(billingEntityId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.length > 0)
            this.masterData.bankList = response;
          this.masterData.bankLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.bankLoading = false;
        });
  }

  onChangeBillingEntity(event) {
    if (event) {
      this.onClearBillingEntity();
      if (event.id && event.id > 0)
        this.getInvoiceBankList(event.id);
    }
    this.clearTaxValue();
    this.calcFeeAmount();
  }

  onClearBillingEntity() {
    this.model.bankId = null;
    this.masterData.bankList = [];

    this.clearTaxValue();
    this.calcFeeAmount();
  }

  //get the invoice payment status list
  getInvoicePaymentStatus() {
    this.masterData.paymentStatusLoading = true;
    this.editInvoiceService.getInvoicePaymentStatus()
      .pipe()
      .subscribe(
        response => {
          this.processInvoicePaymentStatusResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterData.paymentStatusLoading = false;
        });
  }

  //process the invoice payment status reponse
  processInvoicePaymentStatusResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {
        this.masterData.paymentStatusList = response.dataSourceList;
        this.model.paymentStatusId = InvoicePaymentStatus.NotPaid;
      }
      else if (response.result == ResponseResult.NoDataFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_STATUS_NOT_FOUND');
      }
      this.masterData.paymentStatusLoading = false;
    }
  }

  //get invoice extra type list
  getInvoiceExtraTypeList() {
    this.masterData.extraFeeTypeLoading = true;
    this.referenceService.getInvoiceExtraTypeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.masterData.extraFeeTypeList = response.dataSourceList;
            this.masterData.masterExtraFeeTypeList = response.dataSourceList;
          }
          this.masterData.extraFeeTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.extraFeeTypeLoading = false;
        });
  }

  //fetch the first 10 booking for the cus,fact,sup on load
  getBookingListBySearch() {

    // this.masterData.bookingModelRequest.customerId = this.model.customerId;
    // this.masterData.bookingModelRequest.supplierId = this.model.supplierId;
    // this.masterData.bookingModelRequest.factoryId = this.model.factoryId;
    this.masterData.bookingModelRequest.serviceId = this.model.serviceId;

    this.masterData.bookingInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.bookingNumberLoading = true),
      switchMap(term => term
        ? this.extraFeeService.getBookingDataSourceList(this.masterData.bookingModelRequest, term)
        : this.extraFeeService.getBookingDataSourceList(this.masterData.bookingModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.bookingNumberLoading = false))
      ))
      .subscribe(data => {
        this.masterData.bookingNumberList = data;
        this.masterData.bookingNumberLoading = false;
      });
  }

  //fetch the booking data with virtual scroll
  getBookingNoData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.bookingModelRequest.searchText = this.masterData.bookingInput.getValue();
      this.masterData.bookingModelRequest.skip = this.masterData.bookingNumberList.length;
    }

    // this.masterData.bookingModelRequest.customerId = this.model.customerId;
    // this.masterData.bookingModelRequest.supplierId = this.model.supplierId;
    // this.masterData.bookingModelRequest.factoryId = this.model.factoryId;
    this.masterData.bookingModelRequest.serviceId = this.model.serviceId;

    this.masterData.bookingNumberLoading = true;
    this.extraFeeService.getBookingDataSourceList(this.masterData.bookingModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.bookingNumberList = this.masterData.bookingNumberList.concat(data);
        }
        if (isDefaultLoad) {
          this.masterData.bookingModelRequest = new BookingDataSourceRequest();

          // this.masterData.bookingModelRequest.customerId = this.model.customerId;
          // this.masterData.bookingModelRequest.supplierId = this.model.supplierId;
          // this.masterData.bookingModelRequest.factoryId = this.model.factoryId;
          this.masterData.bookingModelRequest.serviceId = this.model.serviceId;
        }


        this.masterData.bookingNumberLoading = false;
      }),
      error => {
        this.masterData.bookingNumberLoading = false;
        this.setError(error);
      };
  }

  onChangeBillTo() {
    if (this.model.billedToId && this.model.bookingNumberId) {
      if (this.model.serviceId) {
        this.masterData.invoiceNumberList = [];
        this.model.invoiceNumberId = null;
        this.getInvoiceNumberList(this.model.bookingNumberId, this.model.billedToId, this.model.serviceId);
      }
      this.model.contactIdList = [];
      this.getInvoiceBilledAddress();
      this.getContactList(this.model.bookingNumberId, this.model.billedToId);
      this.masterData.billingAddressList = null;
      this.model.billedAddressId = null;
      this.model.billedAddress = null;
    }
    else {
      this.onClearBillTo();
    }

    this.contatLabel = this.model.billedToId == BillPaidBy.Customer ? CustomerContact : (this.model.billedToId == BillPaidBy.Factory ? FactoryContact : SupplierContact);
    if (this.model.billedToId && this.model.bookingNumberId) {
      switch (this.model.billedToId) {
        case BillPaidBy.Customer:
          this.model.billedName = this.masterData.customerName;
          break;
        case BillPaidBy.Supplier:
          this.model.billedName = this.masterData.supplierName;
          break;
        case BillPaidBy.Factory:
          this.model.billedName = this.masterData.factoryName;
          break;
      }
    }
  }

  onClearBillTo() {
    this.model.billedName = null;
    this.masterData.billingAddressList = null;
    this.model.billedAddressId = null;
    this.model.billedAddress = null;
    this.model.contactIdList = [];
    this.model.contactList = [];
  }

  changeBookingSelection(event) {
    if (event && event.id) {
      this.model.customerId = event.customerId;
      this.model.supplierId = event.supplierId;
      this.model.factoryId = event.factoryId;
      this.masterData.customerName = event.customerName;
      this.masterData.supplierName = event.supplierName;
      this.masterData.factoryName = event.factoryName;
      if (this.model.billedToId > 0) {
        this.getInvoiceNumberList(event.id, this.model.billedToId, this.model.serviceId);
        this.model.contactIdList = [];
        this.getContactList(event.id, this.model.billedToId);
      }
      if (this.model.bankId > 0) {
        this.getTaxValue(this.model.bankId, event.id);
      }
      this.getInvoiceBilledAddress();
      this.getCustomerListBySearch();
      this.getSupListBySearch();
      this.getFactListBySearch();

      this.masterData.serviceDate = event.serviceDate;
      this.model.billedAddress = null;
      this.model.billedAddressId = null;
      this.model.billedName = null;
      this.model.contactIdList = [];
      this.model.contactList = [];

      this.setExchangeRate();
      this.onChangeBillTo();
    }
    else {
      this.calcFeeAmount();
      this.clearTaxValue();
    }
  }

  clearBookingSelection() {
    this.getBookingNoData(true);
    this.clearTaxValue();
    this.calcFeeAmount();
    this.model.invoiceNumberId = null;
    this.masterData.invoiceNumberList = [];
    this.clearBookingFields();
  }

  clearBookingFields() {
    this.model.customerId = null;
    this.model.supplierId = null;
    this.model.factoryId = null;
    this.masterData.customerName = null;
    this.masterData.supplierName = null;
    this.masterData.factoryName = null;
    this.model.billedToId = null;
    this.model.billedAddress = null;
    this.model.billedName = null;
    this.model.billedAddressId = null;
    this.model.contactIdList = [];
    this.model.contactList = [];
  }
  //fetch the facotry data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.factoryModelRequest.searchText = this.masterData.factoryInput.getValue();
      this.masterData.factoryModelRequest.skip = this.masterData.factoryList.length;
    }
    this.masterData.factoryModelRequest.serviceId = this.model.serviceId;
    this.masterData.factoryModelRequest.id = this.model.factoryId;
    // this.masterData.factoryModelRequest.customerId = this.model.customerId;
    this.masterData.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterData.factoryLoading = true;
    this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.factoryList = this.masterData.factoryList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.factoryModelRequest = new CommonDataSourceRequest();
        this.masterData.factoryLoading = false;
      }),
      error => {
        this.masterData.factoryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    this.masterData.factoryModelRequest.id = this.model.factoryId;
    this.masterData.factoryList = null;
    this.masterData.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterData.factoryModelRequest.serviceId = this.model.serviceId;
    this.masterData.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.factoryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.factoryList = data;
        this.masterData.factoryLoading = false;
      });
  }

  clearFactorySelection() {
    this.getFactoryData(true);
  }

  changeFactorySelection() {
    this.masterData.bookingNumberList = [];
    this.model.bookingNumberId = null;

    this.masterData.invoiceNumberList = [];
    this.model.invoiceNumberId = null;
    this.getBookingNoData(true);
  }

  changeBank(event) {
    if (event && event.id > 0) {
      if (this.model.bookingNumberId > 0)
        this.getTaxValue(event.id, this.model.bookingNumberId);
      if (this.masterData.isAccountRole)
        this.model.invoiceCurrencyId = event.currencyId;
      this.setExchangeRate();
    }
    else {
      this.calcFeeAmount();
      this.clearTaxValue();
    }
  }

  clearBank() {
    this.clearTaxValue();
    this.calcFeeAmount();
  }

  clearTaxValue() {
    this.model.taxValue = 0;
    this.model.taxId = null;
  }

  //get tax value by bank id and booking id
  getTaxValue(bankId: number, bookingId: number) {
    this.extraFeeService.getTaxValue(bankId, bookingId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success && response.taxDetail && response.taxDetail.length > 0) {
            //calculate multiple taxes
            this.model.taxValue = response.taxDetail.reduce(
              (x, current) => x + parseFloat(current.taxValue),
              0
            );
            this.calcFeeAmount();
          }
          else {
            this.clearTaxValue();
            this.calcFeeAmount();
          }
        },
        error => {
          this.setError(error);
        });
  }

  //save the extra fee
  save(ismanualinvoice: boolean) {
    this.isManualInvoice = ismanualinvoice;
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.extraFeesValidators)
      item.validator.isSubmitted = true;

    if (this.isFormValid(ismanualinvoice)) {
      this.masterData.saveLoading = true;

      this.extraFeeService.save(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ExtraFeeResponseResult.Success) {
              if (this.fromSummary) {
                this.return('extrafeessummary/summary');
              }
              else {
                this.edit(response.id);
              }
              if (!ismanualinvoice) {
                this.showSuccess('EXTRA_FEE.LBL_EXTRA_FEE', 'COMMON.MSG_SAVED_SUCCESS');
              }
              else {
                this.masterData.saveLoading = false;
                this.generateManualInvoice();
              }

            }
            else if (response && response.result == ExtraFeeResponseResult.Exists) {

              var billName = this.masterData.billedToList.find(x => x.id == this.model.billedToId) ?
                this.masterData.billedToList.find(x => x.id == this.model.billedToId).name : "";

              this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', this.utility.textTranslate('EXTRA_FEE.MSG_BOOKING_EXISTS') +
                this.model.bookingNumberId + ' ' +
                this.utility.textTranslate('EXTRA_FEE.MSG_BILLING_EXISTS') + billName +
                this.utility.textTranslate('EXTRA_FEE.MSG_BOOKING_EXIST'));
            }

            this.masterData.saveLoading = false;
          },
          error => {
            if (error && error.error && error.error.errors && error.error.statusCode == 400) {
              let validationErrors: [];
              validationErrors = error.error.errors;
              this.openValidationPopup(validationErrors);
            }
            else {
              this.showError("EXTRA_FEE.LBL_EXTRA_FEE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterData.saveLoading = false;
          });
    }
  }

  //add extra fee type row
  addExtraType() {
    this.AddExtraTypeFeeRow();
  }

  //validation
  isFormValid(isManualInvoice: boolean): boolean {
    var isOk = this.validator.isValid('customerId') &&
      this.validator.isValid('billedToId') &&
      this.validator.isValid('serviceId') &&
      this.validator.isValid('bookingNumberId') &&
      this.validator.isValid('paymentStatusId') &&
      this.validator.isValid('currencyId') &&
      this.validator.isValid('billedAddress') &&
      this.validator.isValid('paymentTerms') &&
      this.validator.isValid('officeId') &&
      this.validator.isValid('contactIdList') &&
      this.extraFeesValidators.every((x) => x.validator.isValid('typeId') && x.validator.isValid('fees'))
      && this.extraFeesValidators.length > 0;

    if (isOk && (this.masterData.isAccountRole && (this.model.id === undefined || this.model.id === null || this.model.id === 0 || this.model.statusId == this.extraFeeStatus.Pending))) {
      isOk = this.validator.isValid('invoiceCurrencyId') &&
        this.validator.isValid('exchangeRate')
    }

    if (isManualInvoice && isOk) {
      isOk = this.validator.isValid('extraFeeInvoiceDate');
    }

    return isOk;
  }

  //calculate the total tax amount and tax calc
  calcFeeAmount() {
    var subTot: number = 0;

    this.extraFeesValidators.forEach(item => {
      subTot += item.extraFeeType.fees;
    });

    this.model.subTotal = subTot.toFixed(2);

    if (!this.model.taxValue)
      this.model.taxValue = 0;

    if (this.model.taxValue || subTot)
      this.model.taxAmt = ((subTot * this.model.taxValue)).toFixed(2);

    this.model.totalFees = ((subTot + parseFloat(this.model.taxAmt)) * this.model.exchangeRate).toFixed(2);

  }

  //delete the record in table extra fee type
  deleteExtraFeeType(index) {

    this.model.extraFeeTypeList.splice(index, 1);
    this.extraFeesValidators.splice(index, 1);

    this.calcFeeAmount();

  }

  // close the dropdown
  closeExtraFeeType() {
    this.openExtraFeeType();
  }
  openExtraFeeType() {
    //the below code is to remove the customer if it is available with the above record.
    this.masterData.extraFeeTypeList = Object.assign([],
      this.masterData.masterExtraFeeTypeList);

    this.extraFeesValidators.forEach(element => {

      if (element.extraFeeType.typeId != null) {
        this.masterData.extraFeeTypeList = this.masterData.extraFeeTypeList
          .filter(x => x.id != element.extraFeeType.typeId);
      }
    });
  }

  //edit the extra fee data
  edit(id: number) {
    this.extraFeeService.edit(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.model = response.data;

            this.masterData.bookingModelRequest.bookingId = this.model.bookingNumberId;
            this.masterData.customerModelRequest.id = this.model.customerId;
            this.masterData.supplierModelRequest.id = this.model.supplierId;
            this.masterData.factoryModelRequest.id = this.model.factoryId;
            this.masterData.contactList = this.model.contactList;

            this.extraFeeTypeValidatorMap();

            if (this.model.billingEntityId > 0) {
              this.getInvoiceBankList(this.model.billingEntityId);
            }
            this.getCustomerListBySearch();
            this.getSupListBySearch();
            this.getFactListBySearch();
            this.getBookingNoData(true);
            this.getInvoiceNumberList(this.model.bookingNumberId, this.model.billedToId, this.model.serviceId);
            this.getInvoiceExtraTypeList();
            this.getOfficeList();
            this.getContactList(this.model.bookingNumberId, this.model.billedToId);

            if (this.model.exchangeRate == null || this.model.exchangeRate == undefined)
              this.model.exchangeRate = this.DefaultExchangeRate;

            this.model.taxAmt = (parseFloat(this.model.taxAmt).toFixed(2)).toString();
            this.model.subTotal = (parseFloat(this.model.subTotal).toFixed(2)).toString();
            this.model.totalFees = (parseFloat(this.model.totalFees).toFixed(2)).toString();
          }
        },
        error => {
          this.setError(error);
        });
  }

  //extra type list added to validator list
  extraFeeTypeValidatorMap() {
    this.extraFeesValidators = [];

    this.model.extraFeeTypeList.forEach(item => {
      this.extraFeesValidators.push({
        extraFeeType: item,
        validator: Validator.getValidator(item, "invoice/edit-extra-fee-type.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
      });
    });
  }

  //get invoice number list
  getInvoiceNumberList(bookingId: number, billedToId: number, serviceId: number) {
    this.masterData.invoiceNumberLoading = true;
    this.extraFeeService.getInvoiceNoList(bookingId, billedToId, serviceId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.masterData.invoiceNumberList = response.dataSourceList;
          }
          this.masterData.invoiceNumberLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.invoiceNumberLoading = false;
        });
  }

  //cancel the extra fee type data
  cancel() {
    this.masterData.cancelLoading = true;
    this.extraFeeService.cancel(this.model.id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            if (this.fromSummary) {
              this.return('extrafeessummary/summary');
            }
            else {
              this.edit(this.model.id);
            }
            this.showSuccess('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_CANCEL_SUCCESS');
          }
          this.masterData.cancelLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.cancelLoading = false;
        });
  }

  closeViewPopUp() {
    this.model.invoiceReportTemplateId = null;
    this.modelRef.close();
  }

  //open the popup
  openTemplatePopUp() {
    this.masterData.popUpLoading = false;
    this.masterData.previewLoading = true;
    var invoiceNo = this.masterData.invoiceNumberList &&
      this.masterData.invoiceNumberList.find(x => x.id == this.model.invoiceNumberId) &&
      this.masterData.invoiceNumberList.find(x => x.id == this.model.invoiceNumberId).name ?
      this.masterData.invoiceNumberList.find(x => x.id == this.model.invoiceNumberId).name : "";
      this.invoicePreviewRequest= new InvoicePreviewRequest();

    if (invoiceNo && invoiceNo != "") {
      this.invoicePreviewRequest.invoiceNo=invoiceNo;
      this.invoicePreviewRequest.invoiceData=[invoiceNo];
    }
    else if (this.model.extraFeeInvoiceNo && this.model.extraFeeInvoiceNo != '') {
      this.invoicePreviewRequest.invoiceNo=this.model.extraFeeInvoiceNo;
      this.invoicePreviewRequest.invoiceData=[this.model.extraFeeInvoiceNo];
    }

    this.invoicePreviewRequest.customerId=this.model.customerId.toString();
    this.invoicePreviewRequest.invoicePreviewTypes=[InvoicePreviewType.InvoiceExtraFee];
    this.invoicePreviewRequest.invoicePreviewFrom=InvoicePreviewFrom.ExtarFees;
    this.invoicePreviewRequest.previewTitle="Invoice Preview";
    this.invoicePreviewRequest.service= APIService.Inspection;

    this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
      {
          windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
          centered: true, backdrop: 'static'
      });

    this.masterData.previewLoading = false;
  }

  //generate manual invoice
  generateManualInvoice() {
    if (this.model.id > 0) {
      this.masterData.manualInvoiceLoading = true;
      this.extraFeeService.generateManualInvoice(this.model.id)
        .pipe()
        .subscribe(
          response => {
            if (response) {
              if (response.result == ExtraFeeResponseResult.Success) {
                if (this.fromSummary) {
                  this.return('extrafeessummary/summary');
                }
                else {
                  this.edit(response.id);
                }
                this.showSuccess('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_INVOICE_GENERATED');
              }
              else if (response.result == ExtraFeeResponseResult.InvoiceIdMapped) {
                this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_INVOICE_ID_ALREADY_MAPPED');
              }
              else if (response.result == ExtraFeeResponseResult.DuplicateInvoice) {
                this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_INVOICE_NUMBER_DUPLICATE');
              }
            }
            this.masterData.manualInvoiceLoading = false;
            this.modelRef.close();
          },
          error => {
            this.setError(error);
            this.masterData.manualInvoiceLoading = false;
            this.modelRef.close();
          });
    }
  }

  //open confirm popup to generate manual invoice
  openConfirmPopup(generateInvoicePopup) {
    this.isManualInvoice = true;
    this.validator.isSubmitted = true;
    if (this.model.billingEntityId > 0 && this.model.bankId > 0 && this.model.extraFeeInvoiceDate)
      this.modelRef = this.modalService.open(generateInvoicePopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });

    else {
      if (this.model.billingEntityId == null || this.model.billingEntityId == 0)
        this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', 'QUOTATION.MSG_BILLINGENTITY_REQ');

      else if (this.model.bankId == null || this.model.bankId == 0)
        this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', 'EDIT_INV_BANK.MSG_BANK_NAME_REQUIRED');
      else if (!this.model.extraFeeInvoiceDate)
        this.showWarning('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_EXTRA_FEE_INVOICE_DATE_REQUIRED');
    }
  }

  getEditPath() {
    return "";
  }

  getViewPath() {
    return "";
  }

  getOfficeList() {
    this.masterData.officeLoading = true;
    this.referenceService.getInvoiceOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.officeList = response.dataSourceList;
          this.masterData.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.officeLoading = false;
        });
  }

  getContactList(bookingId, billedToId) {
    this.masterData.contactLoading = true;
    if (billedToId == BillPaidBy.Customer) {
      this.cusContService.getCustomerContactByBookingAndService(bookingId, this.model.serviceId)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              this.masterData.contactList = response.dataSourceList;
            this.masterData.contactLoading = false;
          },
          error => {
            this.setError(error);
            this.masterData.contactLoading = false;
          });
    }

    else {

      var supType = billedToId == BillPaidBy.Factory ? SupplierType.Factory : SupplierType.Supplier;
      this.masterData.supplierModelRequest.serviceId = this.model.serviceId;
      this.supService.getSupplierContactByBooking(bookingId, supType, this.model.serviceId)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              this.masterData.contactList = response.dataSourceList;
            this.masterData.contactLoading = false;
          },
          error => {
            this.setError(error);
            this.masterData.contactLoading = false;
          });
    }
  }

  //clear service selected values
  clearServiceSelection() {
    this.clearBookingInvoiceValues();

    this.getBookingListBySearch();
  }

  //change service
  changeServiceSelection(event) {
    this.clearBookingInvoiceValues();

    this.getBookingListBySearch();

    this.clearBookingFields();
  }

  //reset the details
  clearBookingInvoiceValues() {
    this.masterData.bookingNumberList = [];
    this.model.bookingNumberId = null;

    this.masterData.invoiceNumberList = [];
    this.model.invoiceNumberId = null;
  }

  //cancel extra invoice
  cancelExtraFeeInvoice() {
    this.masterData.cancelInvoiceLoading = true;
    this.extraFeeService.cancelExtraFeeInvoice(this.model.id).subscribe(response => {
      if (response && response.result == ResponseResult.Success) {
        this.return('extrafeessummary/summary');
        this.showSuccess('EXTRA_FEE.LBL_EXTRA_FEE', 'EXTRA_FEE.MSG_CANCEL_SUCCESS');
      }
      this.masterData.cancelInvoiceLoading = false;
    },
      error => {
        this.setError(error);
        this.masterData.cancelInvoiceLoading = false;
      });
  }

  // Set Exchange Rate
  setExchangeRate() {
    if (this.model.invoiceCurrencyId > 0 && this.model.currencyId > 0 && this.masterData.serviceDate) {
      var actualdate = this.getAsDateFromString(this.masterData.serviceDate);

      var inspectionDate = actualdate.getDate() + "-" + actualdate.getMonth() + "-" + actualdate.getFullYear();
      this.exChangeservice.getCurrecyRate(this.model.invoiceCurrencyId, this.model.currencyId, inspectionDate)
        .pipe()
        .subscribe(
          res => {
            this.model.exchangeRate = res;
            this.calcFeeAmount();
          },
          error => {
            this.model.exchangeRate = this.DefaultExchangeRate;
            this.calcFeeAmount();
          });
    }
  }

  getAsDateFromString(dateString) {
    var dateParts = dateString.split("/");
    var dateObject = new Date(+dateParts[2], dateParts[1], +dateParts[0]);
    return dateObject
  }

  //on change currency dropdwn
  onchangeCurrency() {
    if (this.model.invoiceCurrencyId === null || this.model.invoiceCurrencyId === undefined)
      this.model.invoiceCurrencyId = this.model.currencyId;
    //set the exchange rate value
    this.setExchangeRate();
  }

  //on change invoice currency dropdown
  onchangeInvoiceCurrency() {
    //set the exchange rate value
    this.setExchangeRate();
  }
  //on change exchange rate calculate the fee amount
  onChangeExchangeRate() {
    this.calcFeeAmount();
  }

  numericValidation(event) {
    this.utility.numericValidation(event, 11);
  }
  getAddressData(billToId, searchId) {
    this.masterData.invoiceBilledAddressLoading = true;
    this.editInvoiceService.getInvoiceBillingAddress(billToId, searchId)
      .pipe()
      .subscribe(
        response => {
          this.processAddressData(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterData.invoiceBilledAddressLoading = false;
        });
  }
  //process the billed address data
  processAddressData(response: InvoiceBilledAddressResponse) {
    if (response) {
      if (response.result == InvoiceBilledAddressResult.Success) {
        this.masterData.billingAddressList = response.billedAddress;
      }
      else if (response.result == InvoiceBilledAddressResult.AddressNotFound) {
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_BILL_TO_ADDRESS_NOT_FOUND');
      }
      this.masterData.invoiceBilledAddressLoading = false;
    }
  }

  //assign the billing address value
  changeBillingAddress(event) {
    // this.model.billedAddress=null;
    if (event)
      this.model.billedAddress = event.name;
  }
  getInvoiceBilledAddress() {
    switch (this.model.billedToId) {
      case InvoiceBillingTo.Supplier:
        this.getAddressData(this.model.billedToId, this.model.supplierId);
        break;
      case InvoiceBillingTo.Customer:
        this.getAddressData(this.model.billedToId, this.model.customerId);
        break;
      case InvoiceBillingTo.Factory:
        this.getAddressData(this.model.billedToId, this.model.factoryId);
        break;
    }
  }
  processInvoicePaymentTermsResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success)
        this.masterData.invoicePaymentTermList = response.dataSourceList;
      else if (response.result = ResponseResult.NoDataFound)
        this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_TERMS_NOT_FOUND');
    }
    this.masterData.invoicePaymentTermsLoading = false;
  }
  getInvoicePaymentTermList() {
    this.masterData.invoicePaymentTermsLoading = true;
    this.referenceService.getInvoicePaymentTypeList()
      .pipe()
      .subscribe(
        response => {
          this.processInvoicePaymentTermsResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterData.invoicePaymentTermsLoading = false;
        });
  }
  //assign the payment terms value
  changePaymentTerms(event) {
    if (event) {
      this.model.paymentTerms = event.name;
      this.model.paymentDuration = event.duration;
    }
  }

  closeInvoicePreview() {
    this.modelRef.close();
  }
}
