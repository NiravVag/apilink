import { summaryModel } from "../summary.model";
import { DataSource } from "../common/common.model";
import { BehaviorSubject, Observable, Subject } from "rxjs";

export class ExtraFeeSummaryRequestModel extends summaryModel {
  fromDate: any;
  toDate: any;
  customerId: number;
  supplierId: number;
  serviceId: number;
  billedTo: number;
  searchTypeId: number;
  datetypeid: number;
  searchTypeText: string = "";
  statuslst: Array<number>;
}
export class ExtraFeeSummaryModel {

  customerList: any;
  customerLoading: boolean;
  customerInput: BehaviorSubject<string>;

  supplierList: any;
  supplierLoading: boolean;
  supplierInput: BehaviorSubject<string>;

  serviceLoading: boolean;
  serviceList: any;

  billPaidByList: any;
  billPaidByListLoading: boolean;

  searchloading: boolean;
  viewLoading: boolean;
  exportDataLoading: boolean;

  extrafeeStatusList: any;
  statusLoading: boolean;

  _statuslist: Array<any> = [];
  constructor() {
    this.customerInput = new BehaviorSubject<string>("");
    this.supplierInput = new BehaviorSubject<string>("");
  }
}

export class ExtraFeeItem {
  extrafeeid: number;
  bookingId: number;
  customerBookingNo: string;
  billedTo: string;
  extraFeeType: string;
  customerName: string;
  supplierName: string;
  totalAmount: number;
  currency: string;
  service: string;
  invoiceno: string;
  extrafeeinvoiceno: string;
  remarks: string;
  statusId: number;
  invoiceCurrency: string;
  exchangeRate: number;
}
