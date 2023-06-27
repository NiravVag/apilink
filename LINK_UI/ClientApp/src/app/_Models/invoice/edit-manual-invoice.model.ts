import { EntityAccess } from './../../components/common/static-data-common';
import { array } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { BillPaidBy, ListSize } from "src/app/components/common/static-data-common";
import {
  CommonDataSourceRequest,
  CommonSupplierSourceRequest,
  DataSource,
} from "../common/common.model";

export class EditManualInvoiceMasterModel {
  header: string;
  customerInput: BehaviorSubject<string>;
  customerList: any;
  customerLoading: boolean;
  requestCustomerModel: CommonDataSourceRequest;

  billingToLoading: boolean;
  billingToList: Array<any>;
  billPaidBy = BillPaidBy;

  supplierfactoryList: any;
  supplierfactoryLoading: boolean;
  supplierId: number;
  supplierfactoryInput: BehaviorSubject<string>;
  requestSupModel: CommonDataSourceRequest;

  supplierList: any;
  supplierLoading: boolean;
  supplierInput: BehaviorSubject<string>;
  requestSupFactoryModel: CommonDataSourceRequest;

  supplierHeader: string;
  billedName: string;
  customerBilledAddress: string;
  supplierFactoryBilledAddress: string;
  customerName: string;
  supplierName: string;
  factoryName: string;
  supplierFactoryName: string;

  isInvoiceDataLoading: boolean;
  invoiceDataLoadingMsg: string;

  serviceList: Array<DataSource>;
  serviceLoading: boolean;

  serviceTypeList: Array<any>;
  serviceTypeLoading: boolean;

  officeList: Array<any>;
  officeLoading: boolean;

  countryList: Array<any>;
  countryLoading: boolean;

  invoicePaymentTypeList: Array<any>;
  invoicePaymentTypeLoading: boolean;

  billingEntityLoading: boolean;
  billingEntityList: Array<any>;

  invoiceBankList: Array<any>;
  invoiceBankLoading: boolean;

  currencyList: Array<any>;
  currencyLoading: boolean;

  saveloading: boolean;
  bankTaxes: Array<any>;

  serviceFeeTotal: string;
  expChargeBackTotal: string;
  otherCostTotal: string;
  taxServiceFeeTotal: string;
  taxExpChargeBackTotal: string;
  taxOtherCostTotal: string;
  invoiceTotal: string;

  totalTaxes: string;

  itemFeeType = ItemFeeType;

  billedAddressLoading: boolean;
  oldInvoiceNo: string;

  previewLoading: boolean;
  popUpLoading: boolean;

  invoiceReportTemplateList: Array<DataSource>;
  invoiceReportTemplateListLoading: boolean;

  templateBaseUrl: string;
  templateEntityId: string;

  bookingNumberList: Array<DataSource>;
  bookingNumberLoading: boolean;
  bookingInput: BehaviorSubject<string>;
  bookingModelRequest: BookingDataSourceRequest;
  entityId: number;
  entityAccess = EntityAccess;
  constructor() {
    this.customerInput = new BehaviorSubject<string>("");
    this.customerList = [];
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.billingToList = [];
    this.supplierfactoryList = [];
    this.supplierfactoryInput = new BehaviorSubject<string>("");
    this.requestSupFactoryModel = new CommonDataSourceRequest();
    this.requestSupModel = new CommonDataSourceRequest();

    this.serviceList = [];
    this.serviceTypeList = [];
    this.countryList = [];
    this.invoicePaymentTypeList = [];
    this.billingEntityList = [];
    this.invoiceBankList = [];

    this.bankTaxes = [];

    this.invoiceReportTemplateList = [];
    this.bookingModelRequest = new BookingDataSourceRequest();
    this.bookingInput = new BehaviorSubject<string>("");
    this.bookingNumberList = new Array<DataSource>();
  }
}

export class EditManualInvoiceModel {
  id: number;
  customerId: number;
  invoiceTo: number;
  supplierId: number;
  invoiceNo: string;
  invoiceDate: any;
  attn: string;
  billedName: string;
  billedAddress: string;
  email: string;
  serviceId: number;
  serviceType: number;
  currencyId: number;
  paymentTerms: string;
  bankId: number;
  officeId: number;
  paymentDuration: number;
  countryId: number;
  fromDate: any;
  toDate: any;
  billingEntity: number;
  bookingNo: number;
  invoiceItems: EditManualInvoiceItem[];
  invoiceReportTemplateId: any;
  contactList: any[];
  constructor() {
    this.invoiceItems = [];
  }
}

export class EditManualInvoiceItem {
  id: number;
  description: string;
  serviceFee: number;
  expChargeBack: number;
  otherCost: number;
  remarks: string;
  manday: number;
  unitPrice: number;
  discount: number;
}

export enum ManualInvoiceResult {
  Success = 1,
  NotFound = 2,
  InvoiceNoAlreadyExist = 3,
  InvoiceItemNotFound = 4,
}

export enum ItemFeeType {
  serviceFee = 1,
  expChargeBack = 2,
  otherCost = 3,
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
