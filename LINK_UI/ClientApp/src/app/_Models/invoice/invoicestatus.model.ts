 
import { string } from "@amcharts/amcharts4/core";
import { BehaviorSubject} from "rxjs";
import { BookingReportRequest, EmailRuleData } from "../email-send/edit-email-send.model";
import { CountryDataSourceRequest, DataSource } from "../common/common.model";
 
import { summaryModel } from "../summary.model";
import { DataList } from "../useraccount/userconfig.model";

export const invoicedatetypelst = [
  { name: "Service Date", id: 4 }
];

export const invoiceSearchtypelst = [
  { name: "Inspection No", id: 1, shortName: "IN#" },
];

export class InvoiceStatusRequestModel extends summaryModel {
  invoiceFromDate: any;
  invoiceToDate: any;
  customerId: number;
  supplierId: number;
  factoryId: number;
  factoryCountryIds: Array<number>;
  serviceId: number;
  invoiceTo: number;
  invoiceType: number;
  searchTypeId: number;
  datetypeid: number;
  searchTypeText: string = "";
  invoiceStatusId: Array<number>;
  statusidlst: any[] = [];
    officeIdList: any[] = [];
    paymentStatusIdList: Array<number>;
    brandIdList: Array<number>;
}

export class InvoiceStatusModel {

  customerList: any;
  customerLoading: boolean;
  customerInput: BehaviorSubject<string>;

  invoiceTypeList: any;
  invoiceTypeLoading: boolean;

  factoryList: any;
  factoryLoading: boolean;
  factoryInput: BehaviorSubject<string>;

  supplierList: any;
  supplierLoading: boolean;
  supplierInput: BehaviorSubject<string>;

  serviceLoading: boolean;
  serviceList: any;

  // invoiceReportTemplateList: any;
  // invoiceReportTemplateListLoading: boolean;

  searchloading: boolean;
  viewLoading: boolean;
  exportDataLoading: boolean;
  saveInvoiceLoading: boolean;

  templateBaseUrl: string;
  templateEntityId: string;
  _statuslist: Array<any> = [];

  invoiceStatusList: any;
  statusLoading: boolean;

  bookingStatusList: any;
  bookingstatusLoading: boolean;
  // kpiTemplateList: any;
  // kpiTemplateListLoading: boolean;
  // kpiExportLoading: boolean;

  // cancelInvoiceLoading: boolean;

  invoiceGenerateLoading: boolean;

  invoiceReportTemplateList: any;
  invoiceReportTemplateListLoading: boolean;
  invoiceReportTemplateId: number;
  selectedInvoiceNo: string;
  selectedCustomerId: number;
  templateVisible: boolean;
  officeLoading: boolean;
  officeList: any;
  selectedNumber: string;
  selectedNumberPlaceHolder: string;
  isShowColumn: boolean;
  isShowColumnImagePath: string;
  showColumnTooltip: string;
  supplierName: string;
  customerName: string;
  officeNameList: Array<string>;
  factoryName: string;
  countryNameList: Array<string>;
  serviceTypeNameList: Array<string>;
  brandNameList: Array<string>;
  buyerNameList: Array<string>;
  deptNameList: Array<string>;
  statusNameList: Array<string>;
  DateName: string;

  // popup for invoice tax mapping information
  inspectionNo: string;
  quotationBilledTo: string;
  quotationBilledName: string;
  quotationTotalFees: number;
  quotationCurrencyName: string;
  quotationCurrencyCode: string;
  quotationCurrencyId: number;
  billingEntityName: string;
  billingEntityId: number;
  bankName: string;
  bankId: number;
  bankcurrencyId: number;
  bankcurrencyName: string;
  exchangeRate: number;
  totalCalculatedAmount: string;
  taxList: any;
  serviceDate: string;


  billingEntityList: any;
  billingEntityLoading: boolean;

  bankList: any;
  bankLoading: boolean;

  currencyList: any;
  currencyLoading: boolean;

    emailRuleRequest:BookingReportRequest;
    emailRuleList: Array<EmailRuleData>;
    ruleId: number;
    isNoRuleFound: boolean;
    isEachBookingHasDifferentRule: boolean;
    emailSendLoading:boolean;
    emailRuleLoading:boolean;
    preInvoiceList:any;
    isInvoiceCreateVisible:boolean;

  bangladesh_BankIdList: any;
  additionalTaxList: any;
  additionalTax: number;
  additionalTaxLoading: boolean;
  showAdditionalTax: boolean;

  factoryCountryList: Array<any>;
  factoryCountryLoading: boolean;
  factoryCountryInput: BehaviorSubject<string>;
  factoryCountryRequest: CountryDataSourceRequest;
  customerBrandLoading: boolean;
  customerBrandList: Array<any>;
  invoicePaymentStatusLoading: boolean;
  invoicePaymentStatusList: Array<DataSource>;
  communicationSaveLoading: boolean;
  communicationTableLoading: boolean;
  invoiceCommunicationTableList: Array<InvoicecommunicationTable>;
  invoiceNumber: string;
  comment: string;
  hasHideMonthlyInvocieInvoiceStatus: boolean = false;
  constructor() {
    this.isShowColumn = true;
    this.factoryInput = new BehaviorSubject<string>("");
    this.customerInput = new BehaviorSubject<string>("");
    this.supplierInput = new BehaviorSubject<string>("");    
    this.bankList = [];
    this.factoryCountryInput = new BehaviorSubject<string>("");
    this.factoryCountryRequest = new CountryDataSourceRequest();
    this.invoiceCommunicationTableList = new Array<InvoicecommunicationTable>();
  }
}

export class InvoiceItem {
  id: number;
  bookingId: number;
  auditId: number;
  quotationId: number;
  serviceDate: string;
  serviceId: number;
  invoiceNo: string;
  serviceName: string;
  customerId: number;
  supplierId: number;
  factoryId: number;
  customerName: string;
  supplierName: string;
  factoryName: string;  
  invoiceTo: string;
  invoiceTypeId: number;
  invoiceTypeName: string;
  bookingStatusId: number;
  invoiceStatusName: string;
  bookingStatusName: string;
  invoiceStatusColor: string;
  invoiceDate: string;
  paymentStatusId: number;
  paymentStatusName: string;
  paymentDate: string;
  service: string;
  holdType: string;
  holdReason: string;
  holdReasonTrim: string;
  isHoldReasonTooltipShow: boolean;  
  quotationStatus: string;
  serviceStartDate: Date;
  serviceEndDate: Date;
  quotationStatusId: number;
  billTo: number;
  paymentTerms: number;

  quotationSupplierId: number;
  quotationSupplierName: string
  quotationSupplierAddress: string;
  quotationSupplierContacts: Array<number>;

  quotationFactoryId: number;
  quotationFactoryName: string
  quotationFactoryAddress: string
  quotationFactoryContacts: Array<number>;
  invoiceTypeClassName: string;
  quotationBilledTo: string;
  quotationBilledName: string;
  quotationTotalFees: number;
  quotationCurrencyName: string;
  quotationCurrencyCode: string;
  quotationCurrencyId: number;
  billingEntityName: string;
  billingEntityId: number;
  bankName: string;
  bankId: number;
  bankcurrencyId: number;
  bankcurrencyName: string;
  exchangeRate: number;
  taxList: any;
  customerLegalName: string;
  factoryLegalName: string;
  supplierLegalName: string;
  selectedToGenerateInvoice: boolean;
  factoryCountry: string;
  brandNames: string;
  invoiceAmount: number;
  currencyCode: string;

  extraFeeInvoiceNo:string;
  extraFeesStatusName:string;
  extraFeesAmount:number;
  extraFeesCurrencyCode:string;
  extraFeesInvoiceDate:string;
  extraFeesInvoiceTo:string;
  extraFeesPaymentStatusName:string;
  extraFeesPaymentDate:string;

}


export const InvoiceTypeClass = [
  { name: "Monthly Invoice", id: 1, className: "highlight-badge in-progress" },
  { name: "Pre Invoice", id: 2, className: "highlight-badge" }
];

export class InvoiceCommunicationSaveRequest {
  comment: string;
  invoiceNo: string;
}

export class InvoiceCommunicationSaveResponse {
  result: InvoiceCommunicationSaveResultResponse;
}

export enum InvoiceCommunicationSaveResultResponse {
  Success = 1,
  NotFound = 2,
  Failed = 3
}
export enum InvoiceCommunicationTableResultResponse {
  Success = 1,
  NotFound = 2,
  Failed = 3
}

export class InvoicecommunicationTable {
  comment: string;
  createdBy: string;
  createdOn: string;
}

export class InvoiceCommunicationTableResponse {
  result: InvoiceCommunicationTableResultResponse;
  invoiceCommunicationTableList: Array<InvoicecommunicationTable>;
}