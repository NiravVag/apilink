import { summaryModel } from "../summary.model";

export class CustomerContactSummaryModel extends summaryModel {

  customerValue: number;
  customerList: any[];
  contactBrandList: any[];
  contactDepartmentList: any[];
  contactServiceList: any[];
  contactName: string;
  items: Array<CustomerContactSummaryItemModel> = [];

  constructor() {
    super();
    //this.customerValues = [];
    this.contactName = "";
    this.noFound = false;
    this.contactBrandList = [];
    this.contactDepartmentList = [];
    this.contactServiceList = [];
  }

}

export class CustomerContactSummaryItemModel {
  constructor() {
  }

  id: number;
  name: string;
  lastName: string;
  job: string;
  email: string;
  phone: string;
  list: Array<CustomerContactSummaryItemModel>;
  isExpand: boolean = false;
  brand: string;
  department: string;
  service: string;
  reportToName:string;
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


export class customerContactToRemove {
  constructor() {
  }
  id: number;
  name: string;
}

export class UpdateInvoiceDetailRequest {
  invoiceBaseDetail: UpdateInvoiceBaseDetail;
  invoiceDetails: Array<UpdateInvoiceDetail>;
}

export class UpdateInvoiceBaseDetail {
  invoiceNo: string;
  invoiceDate: string;
  postDate: string;
  subject: string;
  invoiceTo: number;
  invoicedName: string;
  invoicedAddress: string;
  contactIds: Array<number>
  paymentTerms: string;
  paymentDuration: string;
  Office: number;
  invoiceMethod: number;
  currency: number;
  invoicePaymentStatus: number;
  invoicePaymentDate: string;
  totalInvoiceFees: number;
  totalTaxAmount: number;
}

export class UpdateInvoiceDetail {
  invoiceNo: number;
  subject: string
  inspectionId: number;
  manDays: number;
  unitPrice: number;
  inspectionFees: number;
  travelAirFees: number;
  travelLandFees: number;
  hotelFees: number;
  otherFees: number;
  discount: number;
  Remarks: string;

}

export class UpdateInvoiceDetailsResponse {
  public result: UpdateInvoiceDetailResult
}

export enum UpdateInvoiceDetailResult {
  Success = 1,
  Failure = 2
}

export enum UserDetailResult {
  Success = 1,
  CannotSaveUserAccount = 2,
  CurrentUserAccountNotFound = 3,
  CannotMapRequestToEntites = 4,
  DuplicateName = 5,
  RequestNotCorrectFormat = 6,
  Failure = 7
}
