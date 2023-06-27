import { EntityAccess } from "src/app/components/common/static-data-common";
import { summaryModel } from "../summary.model";

export class CustomerSummaryModel extends summaryModel {

  groupValues: any[];
  customerValues: any[];
  customerData: CustomerSearchModel;
  items: Array<CustomerSummaryItemModel> = [];
  isEAQF: boolean;

  constructor() {
    super();
    this.groupValues = [];
    this.customerValues = [];
    this.customerData = new CustomerSearchModel();
    this.isEAQF = false;
  }
}

export class CustomerMasterData{
  entityId:number;
  entityAccess=EntityAccess;
}

export class CustomerSummaryItemModel {

  constructor() {
  }
  id: number;
  name: string;
  group: string;
  list: Array<CustomerSummaryItemModel>;
  isExpand: boolean = false;
}

export class CustomerSearchModel {
  constructor() {
    this.customerId = 0;
    this.groupId = 0;
    this.isEAQF = false;
  }
  customerId: number;
  groupId: number;
  isEAQF: boolean;
}

export class customerToRemove {

  constructor() {
  }
  id: number;
  name: string;
}
