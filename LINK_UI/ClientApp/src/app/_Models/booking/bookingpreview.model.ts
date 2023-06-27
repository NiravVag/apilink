import { commonDataSource } from "./inspectionbooking.model";

export class BookingPreviewData {
  public suppliercode: string = "";
  public factcode: string = "";
  public factaddress: string = "";
  public factRegaddress: string = "";
  public Officename: string = "";
  public cusname: string = "";
  public suppliername: string = "";
  public supplierRegionName: any = null;
  public factoryname: string = "";
  public factoryRegionName: any = null;
  public brand: string = "";
  public department: string = "";
  public buyer: string = "";
  public merchandiser: string = "";
  public season: string = "";
  public year: string = "";
  public servicetype: string = "";
  public servicefromdatepanel: string = "";
  public servicetodatepanel: string = "";
  public firstServicefromdatepanel: string = "";
  public firstServicetodatepanel: string = "";
  public factcreateddate: string = "";
  public phoneNumber: string = "";
  public previousBookingNo: string = "";
  public customerContact: Array<ContactDetailPreview>;
  public supplierContact: Array<ContactDetailPreview>;
  public factoryContact: Array<ContactDetailPreview>;
  public dfCustomerConfigPreviewBaseData: any;
  public dfCustomerConfigPreviewChildData: any;
  public dfCustomerConfigPreviewList: Array<ConfigPreviewData>;
  priceCategoryMessage: string = "";
  customerBookingNumber: string;
  public previousBookingNoList: Array<number>;
  public supplierPhoneNumber: string = "";

  constructor() {
    this.customerContact = new Array<ContactDetailPreview>();
    this.supplierContact = new Array<ContactDetailPreview>();
    this.factoryContact = new Array<ContactDetailPreview>();
    this.dfCustomerConfigPreviewBaseData = [];
  }
}

export class ContactDetailPreview {
  public name: string;
  public email: string;
}

export class ConfigPreviewData {
  public label: string;
  public value: string;
}

export class PriceCategoryRequest {
  customerId: number;
  priceCategoryId: number;
  productSubCategory2IdList: Array<number>;
}
export class PriceCategoryResponse {
  priceCategoryName: string;
  priceCategoryId: number;
  result: PriceCategoryResult;
}
export enum PriceCategoryResult {
  Success = 1,
  MultiplePriceCategory = 2,
  MismatchPriceCategory = 3,
  NodataFound = 4,
  RequestNotCorrectFormat = 5,
  SelectPriceCategory = 6
}
export class UserAccessRequest {
  CustomerId: number;
  DepartmentIds: Array<number>;
  BrandIds: Array<number>;
  OfficeIds: Array<number>;
  serviceId: number;
}

export enum UserAccessResult {
  Success = 1,
  NotFound = 2,
  RequestNotValid = 3
}

export class CSConfigListResponse {
  csList: Array<commonDataSource>;
  result: CSConfigListResult;
}

export enum CSConfigListResult {
  Success = 1,
  NotFound = 2,
  RequestNotValid = 3
}
