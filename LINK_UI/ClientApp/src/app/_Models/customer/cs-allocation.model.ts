import { BehaviorSubject } from "rxjs";
import { HRProfileEnum, NotificationList, Service, staffUserTypes } from "src/app/components/common/static-data-common";
import { CountryDataSourceRequest, DataSource, UserDataSourceRequest } from "../common/common.model";
import { UserAccountSummaryModel } from "../useraccount/useraccount.model";

export class EditCSAllocationModel {
  isEdit: boolean;
  customerId: number;
  officeIds: number[];
  serviceIds: number[];
  productCategoryIds: number[];
  departmentIds: number[];
  brandIds: number[];
  userIds: number[];
  staffs: Array<EditSelectedStaffs>;
  factoryCountryIds: number[];

  constructor() {
    this.productCategoryIds = [];
    this.departmentIds = [];
    this.brandIds = [];
    this.serviceIds = [];
    this.officeIds = [];
    this.staffs = [];
    this.userIds = [];
    this.factoryCountryIds = [];
  }
}

export class EditSelectedStaffs {
  id: number;
  profile: number;
  name: string;
  primaryCS: boolean;
  primaryReportChecker: boolean;
  notification: number[];
  isprimaryCSHide: boolean;
  isprimaryReportCheckerHide: boolean;

  notificationList: Array<DataSource>;

  constructor() {

    this.notificationList = NotificationList;
  }
}


export class EditCSAllocationMasterModel {
  saveLoading: boolean;
  customerList: Array<DataSource>;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;

  officeLoading: boolean;
  officeList: any;

  serviceList: Array<any>;
  serviceLoading: boolean;

  productCategoryListLoading: boolean;
  productCategoryList: any;

  deptList: any;
  deptLoading: boolean;


  brandList: any;
  brandLoading: boolean;

  searchloading: boolean;

  staffList: Array<any>;
  staffLoading: boolean;
  staffInput: BehaviorSubject<string>;
  requestStaffDataSource: UserDataSourceRequest;
  selectedStaffs: Array<any>;

  hRProfileEnum = HRProfileEnum;
  profileList: Array<DataSource>;

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
    this.staffInput = new BehaviorSubject<string>("");
    this.requestStaffDataSource = new UserDataSourceRequest();
    this.selectedStaffs = [];

    this.profileList = staffUserTypes;;

    this.countryList = [];
    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();
  }
}

