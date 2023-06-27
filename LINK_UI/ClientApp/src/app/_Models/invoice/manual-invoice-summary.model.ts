import { BehaviorSubject } from "rxjs";
import { EntityAccess } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class ManualInvoiceSummaryMasterModel {
  invoiceToList: Array<any>;
  invoiceToLoading: boolean;

  customerInput: BehaviorSubject<string>;
  customerList: any;
  customerLoading: boolean;
  requestCustomerModel: CommonDataSourceRequest;

  searchloading: boolean;
  statuslist: Array<any>;

  exportLoading: boolean;
  entityId: number;

  entityAccess=EntityAccess;
  constructor() {
    this.invoiceToList = [];
    this.customerList = [];

    this.customerInput = new BehaviorSubject<string>("");
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.statuslist = [];
  }
}

export class ManualInvoiceSummaryModel extends summaryModel {
  fromDate: any;
  toDate: any;
  customerId: number;
  invoiceTo: number;
  invoiceNo: string;
  invoiceStatusId: Array<any>;
  isEAQF: boolean;
  constructor() {
    super();
    this.invoiceStatusId = [];
    this.isEAQF = false;
  }
}
