// import { InvoiceItem } from 'src/app/_Models/invoice/invoicesummary.model';
// import { InvoiceItem } from './../../../_Models/invoice/invoicestatus.model';
import { InvoiceNumberData } from './../../../_Models/email-send/edit-email-send.model';
import { EditManualInvoiceItem } from './../../../_Models/invoice/edit-manual-invoice.model';
import { mode } from 'crypto-js';
import { Component, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { BehaviorSubject, of } from "rxjs";
import {
  catchError,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  tap,
} from "rxjs/operators";
import { JsonHelper, Validator } from "src/app/components/common";
import { DetailComponent } from "src/app/components/common/detail.component";
import {
  APIService,
  BillPaidBy,
  CustomerContact,
  FactoryContact,
  InvoicePreviewType,
  ListSize,
  EntityAccess,
  SupplierContact,
  SupplierType,
  UserType,
} from "src/app/components/common/static-data-common";
import {
  CommonDataSourceRequest,
  DataSourceResponse,
  InvoicePreviewFrom,
  InvoicePreviewRequest,
  ResponseResult,
} from "src/app/_Models/common/common.model";
import {
  EditManualInvoiceMasterModel,
  EditManualInvoiceModel,
  ManualInvoiceResult,
} from "src/app/_Models/invoice/edit-manual-invoice.model";
import { InvoiceMoExistsResult } from "src/app/_Models/invoice/editinvoice.model";
import { InvoiceBankTaxRequest } from "src/app/_Models/invoice/invoicebank";
import { UtilityService } from "src/app/_Services/common/utility.service";
import { CustomerService } from "src/app/_Services/customer/customer.service";
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { CustomerPriceCardService } from "src/app/_Services/customer/customerpricecard.service";
import { EditInvoiceService } from "src/app/_Services/invoice/editinvoice.service";
import { InvoiceBankService } from "src/app/_Services/invoice/invoicebank.service";
import { InvoiceSummaryService } from "src/app/_Services/invoice/invoicesummary.service";
import { ManualInvoiceService } from "src/app/_Services/invoice/manual-invoice.service";
import { LocationService } from "src/app/_Services/location/location.service";
import { ReferenceService } from "src/app/_Services/reference/reference.service";
import { SupplierService } from "src/app/_Services/supplier/supplier.service";
import { EditExtraFeeService } from 'src/app/_Services/invoice/editextrafee.service';
import { InvoicePreviewComponent } from "../../shared/invoice-preview/invoice-preview.component";
import { BookingDataSourceRequest } from 'src/app/_Models/invoice/editextrafeesinvoice.model';

@Component({
  selector: "app-edit-manual-invoice",
  templateUrl: "./edit-manual-invoice.component.html",
  styleUrls: ["./edit-manual-invoice.component.scss"],
})
export class EditManualInvoiceComponent extends DetailComponent {
  masterModel: EditManualInvoiceMasterModel;
  model: EditManualInvoiceModel;
  manualInvoiceItemValidator: Array<any>;
  EditManualInvoiceItem : Array<any>;
  public jsonHelper: JsonHelper;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public modelRef: NgbModalRef;
  invoicePreviewRequest = new InvoicePreviewRequest();
  @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;

  constructor(
    private manualInvoiceService: ManualInvoiceService,
    public locationService: LocationService,
    public refService: ReferenceService,
    public editInvoiceService: EditInvoiceService,
    private invoiceBankService: InvoiceBankService,
    private customerCheckPointService: CustomerCheckPointService,
    private invoiceSummaryService: InvoiceSummaryService,
    private customerPriceCardService: CustomerPriceCardService,
    public extraFeeService: EditExtraFeeService,
    public cusService: CustomerService,
    public supService: SupplierService,
    public modalService: NgbModal,
    public validator: Validator,
    router: Router,
    route: ActivatedRoute,
    translate: TranslateService,
    toastr: ToastrService,
    modal: NgbModal,
    utility: UtilityService
  ) {
    super(router, route, translate, toastr, modal, utility);
    this._toastr = toastr;
    this._translate = translate;
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.masterModel = new EditManualInvoiceMasterModel();
    this.masterModel.supplierHeader = "Supplier";
    this.model = new EditManualInvoiceModel();
    this.validator.setJSON("invoice/edit-manual-invoice.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.jsonHelper = this.validator.jsonHelper;
    this.manualInvoiceItemValidator = [];
    this.getServiceList();
    this.init(id);
    this.masterModel.entityId = Number(this.utility.getEntityId());
  }

  initialize() {
    this.getBookingListBySearch();
    this.getCustomerListBySearch();
    this.getBillingToList();
    this.getOfficeList();
    this.getCountryList();
    this.getInvoicePaymentType();
    this.getBillingEntityList();
    this.getCurrencyList();
  }
  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }
  numericValidation(event) {
    this.utility.numericValidation(event, 10);
  }
  getEntityID() {
    this.utility.getEntityId();
  }
  getCustomerListBySearch() {
    if (this.model.customerId)
      this.masterModel.requestCustomerModel.id = this.model.customerId;
    this.masterModel.customerInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.masterModel.customerLoading = true)),
        switchMap((term) =>
          this.cusService
            .getCustomerDataSourceList(
              this.masterModel.requestCustomerModel,
              term
            )
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.masterModel.customerLoading = false))
            )
        )
      )
      .subscribe((data) => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.masterModel.requestCustomerModel.searchText =
        this.masterModel.customerInput.getValue();
      this.masterModel.requestCustomerModel.skip =
        this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.cusService
      .getCustomerDataSourceList(this.masterModel.requestCustomerModel)
      .subscribe((customerData) => {
        if (IsVirtual) {
          this.masterModel.requestCustomerModel.skip = 0;
          this.masterModel.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.masterModel.customerList =
              this.masterModel.customerList.concat(customerData);
          }
        }
        this.masterModel.customerLoading = false;
      }),
      (error) => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomerData(event) {
    this.masterModel.supplierfactoryList = null;
    this.model.supplierId = null;
    this.masterModel.customerName = event.name;
    if (this.model.customerId && this.model.invoiceTo) {
      this.getSupplierFactoryListBySearch();
    }
    this.getCustomerAddressList();
    this.setBiiledNameAndAddress();
  }

  //getBillingToList
  getBillingToList() {
    this.masterModel.billingToLoading = true;
    this.customerPriceCardService
      .getBillingToList()
      .pipe()
      .subscribe(
        (response) => {
          this.processBillToResponse(response);
        },
        (error) => {
          this.setError(error);
          this.showError(
            "MANUAL_INVOICE_REGISTER.LBL_TITLE",
            "COMMON.MSG_UNKNONW_ERROR"
          );
          this.masterModel.billingToLoading = false;
        }
      );
  }
  //process the bill to list response
  processBillToResponse(response: DataSourceResponse) {
    if (response) {
      if (response.result == ResponseResult.Success) {
        this.masterModel.billingToList = response.dataSourceList;
      }
      if (response.result == ResponseResult.NoDataFound) {
        this.showError(
          "MANUAL_INVOICE_REGISTER.LBL_TITLE",
          "MANUAL_INVOICE_REGISTER.MSG_BILL_TO_NOT_FOUND"
        );
      }
      this.masterModel.billingToLoading = false;
    }
  }

  changeBillingTo() {
    if (
      this.model.customerId &&
      this.model.invoiceTo !== this.masterModel.billPaidBy.Customer
    ) {
      this.masterModel.supplierfactoryList = null;
      this.resetSupplierInfo();
      this.getSupplierFactoryListBySearch();
    }

    this.setBiiledNameAndAddress();

  }

  getSupplierFactoryListBySearch() {
    this.masterModel.requestSupFactoryModel.customerId = this.model.customerId;
    this.masterModel.requestSupFactoryModel.searchText = "";
    this.masterModel.requestSupFactoryModel.skip = 0;
    // if (this.model.supplierId)
    //   this.masterModel.requestSupFactoryModel.id = this.model.supplierId;
    if (
      this.model.invoiceTo === undefined ||
      this.model.invoiceTo == this.masterModel.billPaidBy.Supplier
    ) {
      this.masterModel.requestSupFactoryModel.supplierType =
        SupplierType.Supplier;
      this.masterModel.supplierHeader = "Supplier";
    } else if (this.model.invoiceTo == this.masterModel.billPaidBy.Factory) {
      this.masterModel.requestSupFactoryModel.supplierType =
        SupplierType.Factory;
      this.masterModel.supplierHeader = "Factory";
    }

    this.masterModel.supplierfactoryInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.masterModel.supplierfactoryLoading = true)),
        switchMap((term) =>
          term
            ? this.supService.getFactoryDataSourceList(
              this.masterModel.requestSupFactoryModel,
              term
            )
            : this.supService
              .getFactoryDataSourceList(
                this.masterModel.requestSupFactoryModel
              )
              .pipe(
                catchError(() => of([])), // empty list on error
                tap(() => (this.masterModel.supplierfactoryLoading = false))
              )
        )
      )
      .subscribe((data) => {
        this.masterModel.supplierfactoryList = data;
        this.masterModel.supplierfactoryLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.requestSupFactoryModel.searchText =
        this.masterModel.supplierfactoryInput.getValue();
      this.masterModel.requestSupFactoryModel.skip =
        this.masterModel.supplierfactoryList.length;
    }
    this.masterModel.requestSupFactoryModel.customerId = this.model.customerId;
    if (this.model.invoiceTo == this.masterModel.billPaidBy.Supplier)
      this.masterModel.requestSupModel.supplierType = SupplierType.Supplier;
    else if (this.model.invoiceTo == this.masterModel.billPaidBy.Factory)
      this.masterModel.requestSupModel.supplierType = SupplierType.Factory;
    this.masterModel.supplierfactoryLoading = true;
    this.supService
      .getFactoryDataSourceList(this.masterModel.requestSupFactoryModel)
      .subscribe((data) => {
        if (data && data.length > 0) {
          this.masterModel.supplierfactoryList =
            this.masterModel.supplierfactoryList.concat(data);
        }
        if (isDefaultLoad)
          this.masterModel.requestSupFactoryModel =
            new CommonDataSourceRequest();
        this.masterModel.supplierfactoryLoading = false;
      }),
      (error) => {
        this.masterModel.supplierfactoryLoading = false;
        this.setError(error);
      };
  }

  resetSupplierInfo() {
    this.masterModel.supplierfactoryInput = new BehaviorSubject("");
    this.model.supplierId = null;

    // if (this.model.customerId) {
    //   this.getSupplierFactoryListBySearch();
    // }

    this.setBiiledNameAndAddress();
  }

  changeSupplier(event) {
    if (event) {
      this.masterModel.supplierFactoryName = event.name;
      this.getSupplierFactoryAddressList();
      this.setBiiledNameAndAddress();
    }
  }

  async checkInvoiceExists() {
    let result = false;
    this.model.invoiceNo = this.model.invoiceNo.trimStart();
    this.model.invoiceNo = this.model.invoiceNo.trimEnd();
    if (this.model.invoiceNo && this.model.invoiceNo !== '') {
      result = await this.getInvoiceNumberExistsStatus();
      if (result) {
        this.showWarning(
          "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
          "MANUAL_INVOICE_REGISTER.MSG_INVOICE_ALREDY_EXIST"
        );
      } else {
        this.showSuccess(
          "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
          "MANUAL_INVOICE_REGISTER.MSG_INVOICE_NUMBER_NOT_EXISTS"
        );
      }
    } else {
      result = false;
      this.showWarning(
        "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
        "MANUAL_INVOICE_REGISTER.LBL_INVOICE_NO_CANNOT_BE_EMPTY"
      );
    }
    return result;
  }

  //get the invoice number exists status
  async getInvoiceNumberExistsStatus(): Promise<boolean> {
    this.masterModel.isInvoiceDataLoading = true;
    this.masterModel.invoiceDataLoadingMsg = "Checking Invoice Exists Or Not";
    let response: boolean;
    try {
      response = await this.manualInvoiceService.checkInvoiceNumberExist(
        this.model.invoiceNo
      );
    } catch (e) {
      console.error(e);
      this.masterModel.isInvoiceDataLoading = false;
      this.showError(
        "MANUAL_INVOICE_REGISTER.LBL_TITLE",
        "COMMON.MSG_UNKNONW_ERROR"
      );
    }
    this.masterModel.isInvoiceDataLoading = false;
    return response;
  }

  setBiiledNameAndAddress() {
    if (
      this.model.invoiceTo &&
      this.model.customerId &&
      this.model.invoiceTo === this.masterModel.billPaidBy.Customer
    ) {
      this.model.billedName = this.masterModel.customerName;
      this.model.billedAddress = this.masterModel.customerBilledAddress;
    } else if (
      this.model.invoiceTo &&
      this.model.supplierId &&
      this.model.invoiceTo !== this.masterModel.billPaidBy.Customer
    ) {
      this.model.billedName = this.masterModel.supplierFactoryName;
      this.model.billedAddress = this.masterModel.supplierFactoryBilledAddress;
    } else {
      this.model.billedName = "";
      this.model.billedAddress = "";
    }
  }

  //get the service list
  getServiceList() {
    this.masterModel.serviceLoading = true;
    this.customerCheckPointService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.masterModel.serviceList = response.serviceList;
            this.model.serviceId = APIService.Inspection;
            this.getBookingListBySearch();
          }
          this.masterModel.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.serviceLoading = false;
        });
  }

  // changeService(event) {
  //   this.model.serviceType = null;
  //   this.masterModel.serviceTypeList = [];
  //   if (event && event.id > 0) {
  //     this.getServiceTypeList();
  //   }
  // }

  changeServiceSelection(event) {
    this.clearBookingInvoiceValues();

    this.getBookingListBySearch();

    if (event && event.id && this.model.bookingNo && this.model.invoiceTo) {
      this.getInvoiceNumberList(this.model.bookingNo, this.model.invoiceTo, event.id);
    }

    this.clearBookingFields();
  }

  //reset the details
  clearBookingInvoiceValues() {
    this.masterModel.bookingNumberList = [];
    this.model.bookingNo = null;
    this.model.invoiceNo = null;
  }

  getServiceTypeList() {
    if (!this.model.serviceId)
      return;
    this.masterModel.serviceTypeLoading = true;
    this.refService
      .getServiceTypeListByServiceIds([this.model.serviceId])
      .subscribe((res) => {
        if (res && res.result === 1) {
          this.masterModel.serviceTypeList = res.dataSourceList;
        }
        this.masterModel.serviceTypeLoading = false;
      });
  }

  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.refService
      .getInvoiceOfficeList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.officeList = response.dataSourceList;
          this.masterModel.officeLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        }
      );
  }

  clearDateInput(controlName: any) {
    this.model[controlName] = null;
  }

  getCountryList() {
    this.masterModel.countryLoading = true;
    this.locationService.getCountrySummary().subscribe(
      (res) => {
        if (res && res.result === 1) {
          this.masterModel.countryList = res.countryList;
        }
        this.masterModel.countryLoading = false;
      },
      (error) => {
        this.masterModel.countryList = [];
        this.masterModel.countryLoading = false;
      }
    );
  }

  getInvoicePaymentType() {
    this.masterModel.invoicePaymentTypeLoading = true;
    this.refService
      .getInvoicePaymentTypeList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.invoicePaymentTypeList = response.dataSourceList;
          this.masterModel.invoicePaymentTypeLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.invoicePaymentTypeLoading = false;
        }
      );
  }

  onChangePaymentType(event) {
    if (event) {
      this.model.paymentTerms = event.name;
      this.model.paymentDuration = event.duration;
    }
  }

  onChangeBillingEntity(event) {
    if (event) {
      this.onClearBillingEntity();
      this.getInvoiceBankList(event.id);
    }
  }

  onClearBillingEntity() {
    this.model.bankId = null;
    this.masterModel.invoiceBankList = [];
  }

  getInvoiceBankList(billingEntity) {
    this.masterModel.invoiceBankLoading = true;
    this.refService
      .getInvoiceBankList(billingEntity)
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.invoiceBankList = response.dataSourceList;
          this.masterModel.invoiceBankLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.invoiceBankLoading = false;
        }
      );
  }

  getBillingEntityList() {
    this.masterModel.billingEntityLoading = true;
    this.refService
      .getBillingEntityList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.billingEntityList = response.dataSourceList;
          this.masterModel.billingEntityLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.billingEntityLoading = false;
        }
      );
  }

  getCurrencyList() {
    this.masterModel.currencyLoading = true;

    this.refService.getCurrencyList().subscribe(
      (res) => {
        if (res.result == ResponseResult.Success) {
          this.masterModel.currencyList = res.dataSourceList;
        } else {
          this.masterModel.currencyList = [];
        }
        this.masterModel.currencyLoading = false;
      },
      (error) => {
        this.masterModel.currencyList = [];
        this.masterModel.currencyLoading = false;
      }
    );
  }

  addItem() {
    let invoiceItem: EditManualInvoiceItem = {
      id: 0,
      description: "",
      expChargeBack: null,
      otherCost: null,
      remarks: null,
      serviceFee: null,
      manday: 0,
      unitPrice: 0,
      discount: 0
    };
    this.manualInvoiceItemValidator.push({
      invoiceItem: invoiceItem,
      validator: Validator.getValidator(
        invoiceItem,
        "invoice/edit-manual-invoice-item.valid.json",
        this.jsonHelper,
        this.validator.isSubmitted,
        this._toastr,
        this._translate
      ),
    });
  }
  removeInoviceItem(index) {
    this.manualInvoiceItemValidator.splice(index, 1);
  }

  formValid(): boolean {
    let isOk =
      this.validator.isValid("customerId") &&
      this.validator.isValidIf("supplierId", this.isSupplierIsRequird()) &&
      this.validator.isValid("invoiceTo") &&
      this.validator.isValid("invoiceNo") &&
      this.validator.isValid("invoiceDate") &&
      this.validator.isValid("billedName") &&
      this.validator.isValid("billedAddress") &&
      this.validator.isValid("bankId") &&
      this.validator.isValid("officeId") &&
      this.validator.isValid("fromDate") &&
      this.validator.isValid("toDate") &&
      this.manualInvoiceItemValidator.every(
        (x) =>
          x.validator.isValid("description") &&
          x.validator.isValid("serviceFee")
      );

    return isOk;
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.manualInvoiceItemValidator)
      item.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.invoiceItems = this.manualInvoiceItemValidator.map(x => x.invoiceItem);
      this.masterModel.saveloading = true;
      this.manualInvoiceService.saveManualInvoice(this.model).subscribe(
        (res) => {
          if (res.result == ResponseResult.Success) {
            this.showSuccess(
              "MANUAL_INVOICE_REGISTER.MSG_SAVE_RESULT",
              "MANUAL_INVOICE_REGISTER.MSG_SAVE_OK"
            );
            this.masterModel.saveloading = false;
            this.validator.initTost();
            this.validator.isSubmitted = false;
            this.return("manualinvoicesearch/manual-invoice-summary");
          } else if (
            res.result == ManualInvoiceResult.InvoiceNoAlreadyExist
          ) {
            this.showError(
              "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
              "MANUAL_INVOICE_REGISTER.MSG_ALREADY_EXISTS"
            );
          } else {
            this.showError(
              "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
              "MANUAL_INVOICE_REGISTER.LBL_SAVE_FAILED"
            );
          }
          this.masterModel.saveloading = false;

        },
        (error) => {
          this.showError(
            "MANUAL_INVOICE_REGISTER.MSG_SAVE_RESULT",
            "MANUAL_INVOICE_REGISTER.MSG_UNKNONW_ERROR"
          );
          this.masterModel.saveloading = false;
          //this.waitingService.close();
        }
      );
    }
  }
  returnToSummary() {
    this.return('manualinvoicesearch/manual-invoice-summary');
  }
  init(id?: number) {
    if (id && id > 0) {
      this.masterModel.header = "MANUAL_INVOICE_REGISTER.LBL_EDIT_INVOICE";
      this.manualInvoiceService.getManualInvoice(id).then((res) => {
        if (res && res.result === 1) {
          if (res.result == ResponseResult.Success) {
            this.model = res.manualInvoice;
            this.mapInvoiceItems(this.model);
            let invoiceNo = this.model.invoiceNo;
            this.masterModel.oldInvoiceNo = invoiceNo;
            this.getBankDetails(this.model.bankId);
            this.initialize();
            this.getBookingNoData(true);
            this.getSupplierFactoryListBySearch();
            this.getServiceTypeList();
          } else if (res.result == ManualInvoiceResult.NotFound) {
            this.showError(
              "MANUAL_INVOICE_REGISTER.LBL_MANUAL_INVOICE",
              "MANUAL_INVOICE_REGISTER.MSG_NOT_FOUND"
            );
          }
        }
      });
    } else {
      this.masterModel.header = "MANUAL_INVOICE_REGISTER.LBL_ADD_INVOICE";
      this.initialize();
      this.addItem();
    }
  }

  mapInvoiceItems(model: EditManualInvoiceModel) {
    model.invoiceItems.forEach((x) => {
      this.manualInvoiceItemValidator.push({
        invoiceItem: x,
        validator: Validator.getValidator(
          x,
          "invoice/edit-manual-invoice-item.valid.json",
          this.jsonHelper,
          this.validator.isSubmitted,
          this._toastr,
          this._translate
        ),
      });
    });
  }

  //calculate the sub total fees
  calculateTotalFees() {
    this.masterModel.serviceFeeTotal = this.manualInvoiceItemValidator.reduce(
      (x, current) => x + (current.invoiceItem.serviceFee - current.invoiceItem.discount ? parseFloat(current.invoiceItem.discount) : 0),
      0
    ).toFixed(2);

    this.masterModel.expChargeBackTotal =
      this.manualInvoiceItemValidator.reduce(
        (x, current) => x + (current.invoiceItem.expChargeBack ? parseFloat(current.invoiceItem.expChargeBack) : 0),
        0
      ).toFixed(2);

    this.masterModel.otherCostTotal = this.manualInvoiceItemValidator.reduce(
      (x, current) => x + (current.invoiceItem.otherCost ? parseFloat(current.invoiceItem.otherCost) : 0),
      0
    ).toFixed(2);

    this.masterModel.bankTaxes.forEach((x) => {
      x.taxExpChargeBackTotal =
        (parseFloat(x.taxValue) * parseFloat(this.masterModel.expChargeBackTotal)).toFixed(2);
    })
    this.masterModel.bankTaxes.forEach((x) => {
      x.taxServiceFeeTotal =
        (parseFloat(x.taxValue) * parseFloat(this.masterModel.serviceFeeTotal)).toFixed(2);
    });
    this.masterModel.bankTaxes.forEach((x) => {
      x.taxOtherCostTotal =
        (parseFloat(x.taxValue) * parseFloat(this.masterModel.otherCostTotal)).toFixed(2);
    });

    this.masterModel.invoiceTotal =
      (parseFloat(this.masterModel.serviceFeeTotal) +
        parseFloat(this.masterModel.expChargeBackTotal) +
        parseFloat(this.masterModel.otherCostTotal) +
        this.masterModel.bankTaxes.reduce(
          (x, current) => x + parseFloat(current.taxServiceFeeTotal),
          0
        ) +
        this.masterModel.bankTaxes.reduce(
          (x, current) => x + parseFloat(current.taxOtherCostTotal),
          0
        ) +
        this.masterModel.bankTaxes.reduce(
          (x, current) => x + parseFloat(current.taxExpChargeBackTotal),
          0
        )).toFixed(2);
  }

  getBankDetails(id: number) {
    if (!this.model.toDate) {
      return;
    }
    let invoiceBankTaxRequest = new InvoiceBankTaxRequest();
    invoiceBankTaxRequest.toDate = this.model.toDate;
    this.invoiceBankService.getBankTaxesByDate(id, invoiceBankTaxRequest).then((res) => {
      if (res && res.result === 1) {
        if (this.model.billingEntity === undefined) {
          this.model.billingEntity = res.bankDetails.billingEntity;
          this.getInvoiceBankList(this.model.billingEntity);
        }

        this.masterModel.bankTaxes = res.bankTaxDetails;
        this.masterModel.totalTaxes = res.bankTaxDetails.reduce(
          (x, current) => x + parseFloat(current.taxValue),
          0
        ).toFixed(2);
        this.calculateTotalFees();
      }
    });
  }
  changeBank() {
    if (this.model.bankId > 0) {
      this.getBankDetails(this.model.bankId);
    }
  }

  getDecimalValuewithTwoDigits(numArr) {
    if (numArr.length > 1) {
      var afterDot = numArr[1];
      if (afterDot.length > 2) {
        return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
      }
    }
  }
  validateNegativeValue(value) {
    if (parseInt(value) < 0) return true;
    return false;
  }

  validateDecimal(item, itemFeeType): void {
    let numArr: Array<string>;
    if (item[itemFeeType]) {
      if (!this.validateNegativeValue(item[itemFeeType])) {
        numArr = item[itemFeeType].toString().split(".");
        var value = this.getDecimalValuewithTwoDigits(numArr);
        setTimeout(() => {
          if (value) item[itemFeeType] = value;
        }, 10);
      } else {
        setTimeout(() => {
          item[itemFeeType] = null;
        }, 10);
      }
    }
  }
  isSupplierIsRequird(): boolean {
    return this.model.invoiceTo !== this.masterModel.billPaidBy.Customer;
  }

  getCustomerAddressList() {
    this.masterModel.billedAddressLoading = true;
    this.cusService.getCustomerAddressList(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          // if (response && response.result == ResponseResult.Success)
          //   const acc= response.dataSourceList.find(x=>x.AddressType==1);
          // this.masterModel.billedAddressLoading = false;
          this.mapBilledAddress(response.dataSourceList, UserType.Customer);
        },
        error => {
          this.setError(error);
          this.masterModel.billedAddressLoading = false;
        });
  }
  mapBilledAddress(dataSource: Array<any>, userType: UserType) {
    const accountingAddress = dataSource.find(x => x.addressType === 1);
    if (accountingAddress) {
      if (userType == UserType.Customer)
        this.masterModel.customerBilledAddress = accountingAddress.name;
      else
        this.masterModel.supplierFactoryBilledAddress = accountingAddress.name;
    }
    const headOfficeAddress = dataSource.find(x => x.addressType === 2);
    if (headOfficeAddress) {
      if (userType == UserType.Customer)
        this.masterModel.customerBilledAddress = headOfficeAddress.name;
      else
        this.masterModel.supplierFactoryBilledAddress = headOfficeAddress.name;
    }
    this.setBiiledNameAndAddress();
  }
  getSupplierFactoryAddressList() {
    this.masterModel.billedAddressLoading = true;
    this.supService.getSupplierFactoryAddress(this.model.supplierId)
      .pipe()
      .subscribe(
        response => {
          this.mapBilledAddress(response.dataSourceList, UserType.Supplier);
        },
        error => {
          this.setError(error);
          this.masterModel.billedAddressLoading = false;
        });
  }

  clearCustomer() {
    this.model.customerId = null;
    this.masterModel.requestCustomerModel.id = null;
    this.getCustomerListBySearch();
  }

  closeViewPopUp() {
    this.model.invoiceReportTemplateId = null;
    this.modelRef.close();
  }

  //open the popup
  openTemplatePopUp() {

    var invoiceNo = this.model.invoiceNo;
    if (invoiceNo && invoiceNo != "") {

      this.invoicePreviewRequest = new InvoicePreviewRequest();
      this.invoicePreviewRequest.customerId = this.model.customerId.toString();
      this.invoicePreviewRequest.invoiceNo = invoiceNo;
      this.invoicePreviewRequest.invoiceData = [invoiceNo];
      this.invoicePreviewRequest.invoicePreviewTypes = [InvoicePreviewType.ManualInvoice];
      this.invoicePreviewRequest.invoicePreviewFrom = InvoicePreviewFrom.ManualInvoice;
      this.invoicePreviewRequest.previewTitle = "Invoice Preview";
      this.invoicePreviewRequest.service = APIService.Inspection;


      this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
        {
          windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
          centered: true, backdrop: 'static'
        });

    }
    this.masterModel.previewLoading = false;
  }

  onChangeServiceToDate() {
    this.changeBank();
  }

  closeInvoicePreview() {
    this.modelRef.close();
  }

  changeBookingSelection(event) {
    if (event && event.id) {
      this.model.customerId = event.customerId;
      if (this.model.invoiceTo > 0) {
        this.getInvoiceNumberList(event.id, this.model.invoiceTo, this.model.serviceId);
      }
      this.getCustomerListBySearch();
      this.getBillingToList();
      this.getServiceList();
      this.getOfficeList();
      this.getCountryList();
      this.getInvoicePaymentType();
      this.getBillingEntityList();
      this.getCurrencyList();
    }
  }

  clearBookingSelection() {
    this.getBookingNoData(true);
    this.model.bookingNo = null;
    this.masterModel.bookingNumberList = [];
    this.clearBookingFields();
  }

  getEntityDetail() {
    var currentEntityId = Number(this.utility.getEntityId());
    if (currentEntityId == EntityAccess.SGT)
      this.getBookingNoData(true);
  }

  //fetch the booking data with virtual scroll
  getBookingNoData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.bookingModelRequest.searchText = this.masterModel.bookingInput.getValue();
      this.masterModel.bookingModelRequest.skip = this.masterModel.bookingNumberList.length;
    }

    this.masterModel.bookingModelRequest.serviceId = this.model.serviceId;
    this.masterModel.bookingNumberLoading = true;
    this.extraFeeService.getBookingDataSourceList(this.masterModel.bookingModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.bookingNumberList = this.masterModel.bookingNumberList.concat(data);
        }
        if (isDefaultLoad) {
          this.masterModel.bookingModelRequest = new BookingDataSourceRequest();
          this.masterModel.bookingModelRequest.serviceId = this.model.serviceId;
        }
        this.masterModel.bookingNumberLoading = false;
      }),
      error => {
        this.masterModel.bookingNumberLoading = false;
        this.setError(error);
      };
  }

  clearBookingFields() {
    this.model.customerId = null;
    this.masterModel.customerName = null;
    this.model.invoiceTo = null;
    this.model.invoiceNo = null;
    this.model.invoiceDate = null;
    this.model.billedAddress = null;
    this.model.billedName = null;
    this.model.currencyId = null;
    this.model.bankId = null;
    this.model.officeId = null;
    this.model.fromDate = null;
    this.model.toDate = null;
  }

  //fetch the first 10 booking for the cus,fact,sup on load
  getBookingListBySearch() {
    this.masterModel.bookingModelRequest.serviceId = this.model.serviceId;
    this.masterModel.bookingInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.bookingNumberLoading = true),
      switchMap(term => term
        ? this.extraFeeService.getBookingDataSourceList(this.masterModel.bookingModelRequest, term)
        : this.extraFeeService.getBookingDataSourceList(this.masterModel.bookingModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.bookingNumberLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.bookingNumberList = data;
        this.masterModel.bookingNumberLoading = false;
      });
  }

  //get invoice number list
  getInvoiceNumberList(bookingId: number, billedToId: number, serviceId: number) {
    this.masterModel.billingToLoading = true;
    this.extraFeeService.getInvoiceNoList(bookingId, billedToId, serviceId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.masterModel.billingToList = response.dataSourceList;
          }
          else {
            this.onClearBillTo();
          }
          this.masterModel.billingToLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.billingToLoading = false;
        });
  }

  onClearBillTo() {
    this.model.invoiceTo = null;
    this.masterModel.billingToList = [];
  }

  calculateServiceFee(invoiceItem){
    invoiceItem.serviceFee = invoiceItem.manday * invoiceItem.unitPrice;
  }
}

