import { summaryModel } from "../summary.model";

export class CustomerSerConfigSummaryModel extends summaryModel {

  customerValue: number;
  serviceValue: number;
  customerList: any[];
  items: Array<CustomerServiceConfigItemModel> = [];

  constructor() {
    super();
    //this.customerValues = [];
    this.noFound = false;
  }
}

export class CustomerServiceConfigItemModel {
  constructor() {
  }

  id: number;
  service: string;
  productCategory: string;
  serviceType: string;
  customerName: string;
  samplingMethod: string;
  list: Array<CustomerServiceConfigItemModel>;
  isExpand: boolean = false
}



//export class CustomerContactSummaryItemModel {
//  constructor() {
//  }
//  id: number;
//  name: string;
//  job: string;
//  email: string;
//  phone: string;
//  list: Array<CustomerContactSummaryItemModel>;
//  isExpand: boolean = false;
//}


export class customerServiceConfigToRemove {
  constructor() {
  }
  id: number;
  name: string;
}
