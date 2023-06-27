import { BehaviorSubject } from "rxjs";
import { ListSize } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CountryDataSourceRequest, UserDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";



export class CustomerComplaintModel {

  public complaintTypeList: Array<any>;
  public complaintTypeLoading: boolean;
  public complaintCategoryList: Array<any>;
  public complaintCategoryLoading: boolean;
  public complaintRecipientTypeList: Array<any>;
  public complaintRecipientTypeLoading: boolean;
  public complaintDepartmentList: Array<any>;
  public complaintDepartmentLoading: boolean;
  public officeList: Array<any>;
  public officeLoading: boolean;
  public serviceList: Array<any>;
  public serviceLoading: boolean;
  public showBookingControls: boolean;
  public showBookingDetails: boolean;
  public showBookingProductDetails: boolean;
  public bookingDetailModel: ComplaintBookingData;
  public removeDetailId: number;
  public removeDetailIdIndex: number;


  public bookingNoList: any;
  public bookingNoInput: BehaviorSubject<string>;
  public bookingNoLoading: boolean;
  public bookingNoRequest: BookingNoDataSourceRequest;

  countryList: any;
  countryInput: BehaviorSubject<string>;
  countryLoading: boolean;
  countryRequest: CountryDataSourceRequest;

  customerList: any;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  customerRequest: CommonDataSourceRequest;

  userList: any;
  userInput: BehaviorSubject<string>;
  userLoading: boolean;
  userRequest: UserDataSourceRequest;

  public savedataloading: boolean;

  constructor() {
    this.showBookingProductDetails = false;

    this.bookingNoInput = new BehaviorSubject<string>("");
    this.bookingNoRequest = new BookingNoDataSourceRequest();

    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();

    this.customerInput = new BehaviorSubject<string>("");
    this.customerRequest = new CommonDataSourceRequest();

    this.userInput = new BehaviorSubject<string>("");
    this.userRequest = new UserDataSourceRequest();

    this.bookingDetailModel = new ComplaintBookingData();
    this.complaintTypeList = [];

  }
}

export class EditCustomerComplaintModel {
  id: number;
  complaintTypeId: number;
  serviceId: number;
  bookingNo: number;
  complaintDate: Date;
  recipientTypeId: number;
  departmentId: number;
  customerId: number;
  countryId: number;
  officeId: number;
  userIds: Array<number>;
  complaintDetails: Array<complaintDetail>;
  remarks:string
  constructor() {
    this.id = 0;
    this.complaintDetails = [];
    this.complaintDate = null;
    this.userIds = [];
  }
}

export enum ComplaintType {
  Booking = 1,
  General = 2
}

export class BookingNoDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public serviceId: number;
  public id: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}
export class ComplaintBookingData {
  bookingNo: number;
  serviceDateFrom: string;
  serviceDateTo: string;
  customerName: string;
  customerId: number;
  supplierName: string;
  factoryName: string;
  bookingStatus: number;
  serviceTypeId: number;
  serviceType: string;
  countryName: string;
  provinceName: string;
  firstProductName: string;
  officeName: string;
  bookingQty: number;
}
export class ComplaintBookingProductData {
  id: number;
  bookingId: number;
  productId: number;
  productName: string;
  productDescription: string;
  poNumbers: string;
  bookingQuantity: number;
}
export enum ResponseResult {
  Success = 1,
  CannotGetList = 2,
  Failed = 3,
  RequestNotCorrectFormat = 4,
  ServiceIdRequired = 5
}
export class complaintDetail {
  public id: number;
  public productId: number;
  public productDesc: string;
  public productList: any;
  public categoryId: number;
  public title: string;
  public categoryList: any;
  public description: string;
  public correctiveAction: string;
  public answerDate: Date;
  public remarks: string;

}


export class ComplaintSummaryRequestModel extends summaryModel {
  public searchtypeid: number;
  public searchtypetext: string = "";
  public datetypeid: number;
  public fromdate: any;
  public todate: any;
  public advancedSearchtypeid: number;
  public advancedsearchtypetext: string = null
  public complaintTypeId: number;
  public serviceId: number;
  public customerId: number;
}

export class ComplaintSummaryModel {

  public serviceList: any;
  public complaintTypeList: any;

  public serviceLoading: boolean;
  public complaintTypeLoading: boolean;

  public searchLoading: boolean;

  public requestCustomerModel: CommonDataSourceRequest;
  public customerLoading: boolean;
  public customerList: any;
  public customerInput: BehaviorSubject<string>;

  public deleteId: number;
  public deleteLoading: boolean;

  constructor() {
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.customerInput = new BehaviorSubject<string>("");
  }
}


