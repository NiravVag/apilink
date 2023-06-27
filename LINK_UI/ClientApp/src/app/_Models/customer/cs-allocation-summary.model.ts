import { UserAccountSummaryModel } from 'src/app/_Models/useraccount/useraccount.model';
import { BehaviorSubject } from "rxjs";
import { CountryDataSourceRequest, DataSource, UserDataSourceRequest } from "../common/common.model";
import { Service } from "../quotation/quotation.model";
import { summaryModel } from "../summary.model";

export class CsAllocationSearchModel extends summaryModel {
  customerId: number;
  officeId: number;
  serviceId: number;
  productCategoryIds: number[];
  departmentIds: number[];
  brandIds: number[];
  userIds: number[];
  userType: number;
  factoryCountryIds: number[];
  constructor() {
    super();
    this.productCategoryIds = [];
    this.departmentIds = [];
    this.brandIds = [];
    this.userIds = [];
    this.factoryCountryIds = [];
  }
}

export class CsAllocationMasterModel {
  customerList: Array<DataSource>;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;

  officeLoading: boolean;
  officeList: any;

  serviceList: Array<Service>;
  serviceLoading: boolean;

  productCategoryListLoading: boolean;
  productCategoryList: any;

  deptList: any;
  deptLoading: boolean;


  brandList: any;
  brandLoading: boolean;

  searchloading: boolean;
  exportLoading: boolean;

  staffList: Array<any>;
  staffLoading: boolean;
  staffInput: BehaviorSubject<string>;
  requestStaffDataSource: UserDataSourceRequest;

  countryList: any;
  countryLoading: boolean
  countryInput: BehaviorSubject<string>;
  countryRequest: CountryDataSourceRequest;

  constructor() {
    this.customerList = [];
    this.customerInput = new BehaviorSubject<string>("");

    this.officeList = [];

    this.serviceList = [];

    this.productCategoryList = [];

    this.deptList = [];
    this.brandList = [];
    this.staffList = [];
    this.requestStaffDataSource = new UserDataSourceRequest();
    this.staffInput = new BehaviorSubject<string>("");

    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();
    this.countryList = [];
  }
}

export class CSAllocationDeleteItem {
  daUserCustomerIds: number[];
}
