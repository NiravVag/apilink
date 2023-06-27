import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, SupplierDataSourceRequest } from "../common/common.model";
import { EditOfficeModel } from "../office/edit-officemodel";
import { summaryModel } from "../summary.model";

export class ClaimSummaryModel extends summaryModel {
  public searchtypeid: number;

  public searchtypetext: string = "";

  public customerid: number;

  public supplierid: number;

  public statusidlst: any[] = [];

  public datetypeid: number;

  public fromdate: any;

  public todate: any;

  public claimDate: any = null;

  public statusId: number;

  public officeidlst: any[] = [];

  public advancedSearchtypeid: string;

  public advancedsearchtypetext: string = null;

  public countryId: number;

}

export interface ClaimStatuses {
  id: number;
  label: string;
}

export interface DataSource {
  id: number;
  name: string;
  isForwardToManager: boolean;
  countyId: number;
  invoiceType: number;
  statusName: string;
}
export interface ClaimSummaryResponse {
  customerList: Array<DataSource>;
  officeList: Array<EditOfficeModel>;
  statusList: Array<ClaimStatuses>;
  result: ClaimSummaryResult;
}

export enum ClaimSummaryResult {
  Success = 1,
  CustomerListNotFound = 2,
  CannotFindOfficeList = 3,
  CannotFindStatusList = 4
}

export class ClaimDataSourceResponse {
  public dataSource: Array<DataSource>;
  public result: ClaimDataSourceResult;
}

export enum ClaimDataSourceResult {
  Success = 1,
  CountryEmpty = 2,
  NotFound = 4
}
export class ClaimItem {

  public claimId: number;
  public bookingId: number;
  public claimNo: string;
  public claimDate: string;
  public customerId: number;
  public customerName: string;
  public supplierName: string;
  public inspectionDate: string;
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

export interface ClaimDataSummaryResponse {
  data: Array<ClaimItem>;
  result: ClaimDataSummaryResult;
  totalCount: number;
  index: number;
  pageSize: number;
  pageCount: number;
  statusList: any[];
}

export enum ClaimDataSummaryResult {
  Success = 1,
  NotFound = 2
}

export class ClaimToCancel {
  constructor() {
  }
  id: number;
  claimNo: string;
}

export class ClaimSummaryMasterData {

  requestCustomerModel: CommonDataSourceRequest;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  public customerList: any;

  supsearchRequest: SupplierDataSourceRequest;
  supInput: BehaviorSubject<string>;
  supLoading: boolean;
  public supplierList: any;
  
  public officeList: any;
  officeLoading: boolean
  officeInput: BehaviorSubject<string>;

  public statusList: any;
  statusLoading: boolean
  statusInput: BehaviorSubject<string>;

  countryList: any;
  countryLoading: boolean
  countryInput: BehaviorSubject<string>;

  productList: any;
  poList: any;
  containerProductList: any;
  containerPOList: any;
  containerItem: any;
  bookingItem: any

  bookingNumber: string;
  productId: string;
  productName: string;
  isproductOrPODetails: boolean;
  selectedAllProducts: boolean;
  pageLoader: boolean;
  setIconWidth: string;
  supplierName: string;
  customerName: string;
  officeNameList: Array<string>;
  factoryNameList: Array<string>;
  countryNameList: Array<string>;
  statusNameList: Array<string>;
  DateName: string;

  constructor() {
    this.pageLoader = false;

    this.customerInput = new BehaviorSubject<string>("");
    this.supInput = new BehaviorSubject<string>("");

    this.requestCustomerModel = new CommonDataSourceRequest();
    this.supsearchRequest = new SupplierDataSourceRequest();
  }
}