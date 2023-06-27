import { SupplierSummaryItemModel } from "./suppliersummary.model";

export class EditSuplierModel {
  public id: number;
  public name: string;
  public locLanguageName: string;
  public email: string;
  public phone: string;
  public fax: string;
  public mobile: string;
  public webSite: string
  public typeId: number;
  public levelId: number;
  public dailyProduction: string;
  public ownerId: number;
  public totalStaff: string;
  public legalName: string;
  public glCode: string;
  public contactPersonName: string;
  public comment: string;
  public addressList: Array<Address>;
  public supplierContactList: Array<CustomerContact>;
  public customerList: Array<Customer>;
  public supplierParentList: Array<SupplierSummaryItemModel>;
  public isNewSupplier: boolean;
  public status: number;
  public vatNo: string;
  public apiServiceIds: any;
  public apiContactServiceList: any;
  public companyId: number;
  isFromBookingPage: boolean;
  supplierEntityIds: number[];
  mapAllSupplierContacts: boolean;
  public gradeList: Array<Grade>;
  constructor() {
    this.ownerId = 1;
    this.addressList = [];
    this.customerList = [];
    this.supplierContactList = [];
    this.supplierParentList = [];
    this.isNewSupplier = false;
    this.status = 1;
    this.apiServiceIds = [];
    this.supplierEntityIds = [];
    this.gradeList = [];
  }
}

export class Address {
  public id: number;
  public countryId: number;
  public regionId: number;
  public cityId: number;
  public zipCode: string;
  public way: string;
  public addressTypeId: number;
  public longitude: number;
  public latitude: number;
  public localLanguage: string;
  public countyId: number;
  public townId: number;

  public regionList: Array<any>;
  public cityList: Array<any>;
  public countyList: Array<any>;
  public townList: Array<any>;

}

export class CustomerContact {
  public customerList: Array<Customer>;
  public contactId: number;
  public contactName: string;
  public contactEmail: string;
  public jobTitle: string;
  public phone: string;
  public fax: string;
  public mobile: string;
  public comment: string;
  public contactAPIServiceIds: any;
  public apiEntityIds: any;
  public entityServiceList: any;
  public entityServiceIds: any;
  public primaryEntityList: any;
  public contactEntityList: any;
  public primaryEntity: number;
  showPopupLoading: boolean;
}

export class Customer {
  public id: number;
  public name: string;
  public code: string;
  public creditTerm: number;
  public isStatisticsVisibility: boolean;
}
export class Grade {
  public id: number;
  public customerId: number;
  public customerName: string;
  public levelId: number;
  public level: string;
  public periodFrom: any;
  public periodTo: any;
  public isDisable: boolean = false;
}
export class SupplierItem {
  public id: number;
  public name: string;
}
export enum SupplierListResult {
  Success = 1,
  NodataFound = 2
}

export enum SaveSupplierResult {
  Success = 1,
  SupplierIsNotSaved = 2,
  SupplierIsNotFound = 3,
  SupplierExists = 4,
  FactoryCountyTownNotFound = 5,
  SupplierEntityNotRemoved = 6,
  SupplierDetailsExists = 7,
  SupplierCodeExists = 8,
}
