import { DataList } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { BehaviorSubject } from "rxjs";
import { ListSize } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, DataSource } from "../common/common.model";


export class EditExtraFeesMaster {
  customerList: Array<DataSource>;
  supplierList: Array<DataSource>;
  factoryList: Array<DataSource>;
  billedToList: Array<DataSource>;
  currencyList: Array<DataSource>;
  serviceList: Array<DataSource>;
  billingEntityList: Array<DataSource>;
  bankList: Array<any>;
  bookingNumberList: Array<DataSource>;
  paymentStatusList: Array<DataSource>;
  extraFeeTypeList: Array<DataSource>;
  invoicePaymentTermList: Array<DataList>;
  masterExtraFeeTypeList: Array<DataSource>;
  invoiceNumberList: Array<DataSource>;
  officeList: Array<DataSource>;
  contactList: Array<DataSource>;
  invoiceBilledAddressLoading: boolean;
  invoicePaymentTermsLoading: boolean;
  customerLoading: boolean;
  supplierLoading: boolean;
  factoryLoading: boolean;
  billedToLoading: boolean;
  currencyLoading: boolean;
  serviceLoading: boolean;
  billingEntityLoading: boolean;
  bankLoading: boolean;
  bookingNumberLoading: boolean;
  paymentStatusLoading: boolean;
  extraFeeTypeLoading: boolean;
  invoiceNumberLoading: boolean;
  saveLoading: boolean;
  cancelLoading: boolean;
  cancelInvoiceLoading: boolean;
  invoiceLoading: boolean;
  isExtraInvoiceNoDisabled: boolean;
  customerInput: BehaviorSubject<string>;
  supplierInput: BehaviorSubject<string>;
  factoryInput: BehaviorSubject<string>;
  bookingInput: BehaviorSubject<string>;
  customerModelRequest: CommonDataSourceRequest;
  supplierModelRequest: CommonDataSourceRequest;
  factoryModelRequest: CommonDataSourceRequest;
  bookingModelRequest: BookingDataSourceRequest;
  manualInvoiceLoading: boolean;
  isAccountRole: boolean;
  isAERole: boolean;
  invoiceReportTemplateList: Array<DataSource>;
  invoiceReportTemplateListLoading: boolean;
  popUpLoading: boolean;
  templateBaseUrl: string;
  templateEntityId: string;
  previewLoading: boolean;
  selectedInvoiceNo: string;
  officeLoading: boolean;
  contactLoading: boolean;
  billingAddressList : Array<DataList>;
  serviceDate: string;
  customerName: string;
  supplierName: string;
  factoryName: string;

  constructor() {
    this.customerModelRequest = new CommonDataSourceRequest();
    this.supplierModelRequest = new CommonDataSourceRequest();
    this.factoryModelRequest = new CommonDataSourceRequest();
    this.bookingModelRequest = new BookingDataSourceRequest();
    this.customerInput = new BehaviorSubject<string>("");
    this.supplierInput = new BehaviorSubject<string>("");
    this.factoryInput = new BehaviorSubject<string>("");
    this.bookingInput = new BehaviorSubject<string>("");
    this.bookingNumberList = new Array<DataSource>();
    this.customerList = new Array<DataSource>();
    this.supplierList = new Array<DataSource>();
    this.factoryList = new Array<DataSource>();
    this.masterExtraFeeTypeList = new Array<DataSource>();
    this.extraFeeTypeList = new Array<DataSource>();
    this.invoiceReportTemplateList = new Array<DataSource>();
    this.officeList = new Array<DataSource>();
    this.contactList = new Array<DataSource>();
  }

}

export class BookingDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public factoryId: number;
  public bookingId: number;
  serviceId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export class EditExtraFee {
  customerId: number;
  supplierId: number;
  factoryId: number;
  billedToId: number;
  billedName: string;
  billedAddress: string;
  billedAddressId : number;
  currencyId: number;
  serviceId: number;
  billingEntityId: number;
  bankId: number;
  bookingNumberId: number;
  paymentStatusId: number;
  extraFeeTypeId: number;
  invoiceNumberId: number;
  taxValue: number;
  taxId: number;
  remarks: string;
  paymentDate: any;
  paymentTerms: string;
  paymentTermsId: number;
  paymentDuration: number;
  extraFeeInvoiceDate: any;
  extraFeeInvoiceNo: string;
  extraFeeTypeList: Array<EditExtraFeeType>;
  subTotal: string;
  totalFees: string;
  taxAmt: string;
  id: number;
  statusName: string;
  statusId: number;
  invoiceReportTemplateId: number;
  officeId: number;
  contactIdList: Array<number>;
  contactList: Array<DataSource>;
  invoiceCurrencyId: number;
  exchangeRate: number;
  constructor() {
    this.extraFeeTypeList = new Array<EditExtraFeeType>();
    this.taxValue = 0;
    this.taxAmt = "0";
    this.contactIdList = [];
  }
}

export class EditExtraFeeType {
  id: number;
  typeId: number;
  fees: number;
  remarks: string;
}

export class ExtraFeeDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public supplierType: number;
  id: number;
  locationId: number;
  idList: Array<number>;
  supSearchTypeId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export enum ExtraFeeResponseResult {
  Success = 1,
  NotFound = 2,
  Failure = 3,
  RequestNotCorrectFormat = 4,
  Exists = 5,
  InvoiceIdMapped = 6,
  DuplicateInvoice = 7
}

