import { summaryModel } from "../summary.model";
import { DataSource, Service, QuotationStatus } from "./quotation.model";
import { EditOfficeModel } from "../office/edit-officemodel";
import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest } from "../common/common.model";
import { commonDataSource } from "../booking/inspectionbooking.model";
import { EntityAccess, SearchType } from "src/app/components/common/static-data-common";
export class Quotationsummarymodel extends summaryModel {

  public searchtypeid: number;

  public searchtypetext: string = "";

  public customerid: number;

  public supplierid: number;

  public factoryidlst: any[] = [];

  public statusidlst: any[] = [];

  public datetypeid: string;

  public fromdate: any;

  public todate: any;

  public statusId: number;

  public officeidlst: any[] = [];

  public serviceId: number;

  public advancedSearchtypeid: string;

  public advancedsearchtypetext: string = null;

  public serviceTypelst: any[] = [];

  public invoiceNo: string = "";

  public invoiceDate: any = null;

  brandIdList: number[] = [];
  deptIdList: number[] = [];
  buyerIdList: number[] = [];
  supplierTypeId: number;
  public isEAQF: boolean = false;
}

export class QuotationItem {

  public quotationId: number;
  public bookingNoCusBookingNo: string;
  public quotationDate: string;
  public serviceDateList: string;
  public customerId: number;
  public customerName: string;
  public supplierName: string;
  public factoryName: string;
  public serviceType: string;
  public office: string;
  public statusId: number;
  public statusName: string;
  supplierNameTrim: string;
  factoryNameTrim: string;
  serviceDateListTrim: string;
  billPaidByName: string;
  customerRemark: string;
  isPreview: boolean;
  validatedBy: number;
  validatedMsg: string
  validatedMsgTbl: string
  validatedUserName: string;
  validatedOn: string;
  bookingIdStatusColorList: any[];
  brandName: string;
  departmentName: string;
}

export interface QuotationSummaryResponse {
  customerList: Array<DataSource>;
  officeList: Array<EditOfficeModel>;
  statusList: Array<QuotStatus>;
  serviceList: Array<Service>;
  result: QuotationSummaryResult;
}

export interface QuotStatus {
  id: number;
  label: string;
}

export enum QuotationSummaryResult {
  Success = 1,
  CustomerListNotFound = 2,
  CannotFindOfficeList = 3,
  CannotFindStatusList = 4
}

export interface QuotationDataSummaryResponse {
  data: Array<QuotationItem>;
  result: QuotationDataSummaryResult;
  totalCount: number;
  index: number;
  pageSize: number;
  pageCount: number;
  statusList: any[];
}

export enum QuotationDataSummaryResult {
  Success = 1,
  NotFound = 2
}
export class StatusChange {
  quotationStatus: QuotationStatus;
  item: QuotationItem;
}

export enum QuotationCustomerTaskType {
  PendingQuotations = 1,
  CompletedQuotations = 2
}

export enum QuotationPageType {
  CustomerTaskDashboard = 1,
}


export class InvoiceModel {
  public quotationId: number;
  public invoiceNo: string;
  public invoiceDate: any;
  public invoiceRemarks: string;
  public Service: number;
}


export class QuotationMasterModel {
  brandLoading: boolean;
  brandInput: BehaviorSubject<string>;
  brandList: any;

  deptLoading: boolean;
  deptInput: BehaviorSubject<string>;
  deptList: any;

  buyerLoading: boolean;
  buyerInput: BehaviorSubject<string>;
  buyerList: any;

  isAuditService: boolean;
  servicetypeLoading: boolean;

  supsearchRequest: CommonDataSourceRequest;
  supplierList: any;
  supInput: BehaviorSubject<string>;
  supplierLoading: boolean;
  searchQuotationNumber: string;
  isCustomerLogin: boolean;
  quotationSearchTypeList: Array<commonDataSource>;
  bookingSearchTypeId = SearchType.BookingNo;
  entityId:number;
  entityAccess=EntityAccess;
  constructor() {
    this.brandInput = new BehaviorSubject<string>("");
    this.buyerInput = new BehaviorSubject<string>("");
    this.deptInput = new BehaviorSubject<string>("");
    this.supsearchRequest = new CommonDataSourceRequest();
    this.supInput = new BehaviorSubject<string>("");
    this.quotationSearchTypeList = new Array<commonDataSource>();
  }
}