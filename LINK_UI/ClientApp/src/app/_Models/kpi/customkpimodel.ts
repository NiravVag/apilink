import { BehaviorSubject } from "rxjs";
import { CountryDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class CustomkpiModel extends summaryModel {
  public fromdate: any;
  public todate: any;
  public customerId: number;
  public templateId: number;
  public serviceTypeIdLst: Array<number>;
  public officeIdLst: Array<number>;
  public invoiceNo: string;
  public brandIdList: Array<number>;
  public departmentIdList: Array<number>;
  public bookingNo: number;
  public customerMandayGroupByFields: Array<number>;
  public countryIds: any;
  public invoiceTypeIdList: Array<number>;
  public paymentStatusIdList: Array<number>;
}

export class CustomKpiLists {
  public brandList: any;
  public departmentList: any;
  public serviceTypeList: any;

  public brandLoading: boolean;
  public departmentLoading: boolean;
  public serviceTypeLoading: boolean;

  countryList: any;
  countryInput: BehaviorSubject<string>;
  countryLoading: boolean;
  countryRequest: CountryDataSourceRequest;

  constructor() {
    this.brandList = [];
    this.departmentList = [];
    this.serviceTypeList = [];

    this.countryList = [];
    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();
  }
}
