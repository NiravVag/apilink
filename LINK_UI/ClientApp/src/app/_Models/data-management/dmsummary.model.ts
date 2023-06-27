import { summaryModel } from "../summary.model";
import { ModuleListResult } from "./dm.module.model";

export class DMSummaryModel extends summaryModel {

  idModule: number;
  moduleName: string;
  fileName: string;
  isCustomerRequired: boolean;
  idCustomer: number | null;
  description: string;
  items: Array<DMSummaryItemModel> = [];

  constructor() {
    super();
  }
}

export class DMSummaryItemModel {
  id: number;
  customer: string;
  module: string;
  documentName: string;
  documentType: string;
  documentSize: number;
  documentUrl: string;
  description: string;
  createdOn: string;
  editRight: boolean;
  deleteRight: boolean;
  downloadRight: boolean;
  dmDetailId: number;
  documentId: string;
  brands: string;
  departments: string;
}


export interface DmModule {
  id: number;
  parentId: number;
  moduleName: string;
  ranking: number;
  needCustomer: boolean;
  children: Array<DmModule>;
  selected: number | null;
}

export interface DMModuleListResponse {
  list: Array<DmModule>;
  result: ModuleListResult;
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
  }
  customerId: number;
  groupId: number;
}

export class dmFileToRemove {
  constructor() {
  }
  id: number;
  name: string;
}
