import { CustomerSummaryItemModel } from "./cutomersummary.model";


export class EditCustomerModel {
  public id: number;
  public name: string;
  public startDate: any;
  public code: string;
  public email: string;
  public fax: string;
  public complexityLevel: number;
  public website: string;
  public customerAddresses: Array<CustomerAddress>;
  public others: string;
  public prospectStatus: number;
  public skillsRequired: number;
  public kam: any[];
  public kamItems: any[];
  public phone: string;
  public category: number;
  public margetSegment: number;
  public businessCountry: any[];
  public businessCountryItems: any[];
  public otherPhone: string;
  public language: number;
  public businessType: number;
  public quatationName: string;
  public icRequired: string;
  public glCode: string;
  public comments: string;
  public invoiceType: number;
  public group: number;
  public bookingDefaultComments: string;
  public brandPriority: any[];
  public brandPriorityItems: any[];
  public accountingLeader: number;
  public salesIncharge: any[];
  public salesInchargeItems: any[];
  public activitiesLevel: number;
  public relationshipStatus: number;
  public directCompetitor: string;
  public apiServiceIds: any;
  public customerEntityIds: number[];
  public mapCustomerContactEntityIds: number[] = [];
  public companyId: number;
  sisterCompanyIds: number[] = [];
  constructor() {
    this.id = 0;
    this.group = null;
    this.kam = [];
    this.kamItems = [];
    this.businessCountry = [];
    this.category = null;
    this.skillsRequired = null;
    this.prospectStatus = null;
    this.language = null;
    this.complexityLevel = null;
    this.businessType = null;
    this.invoiceType = null;
    this.margetSegment = null;
    this.customerAddresses = [];
    this.businessCountryItems = [];
    this.accountingLeader = null;
    this.activitiesLevel = null;
    this.relationshipStatus = null;
    this.salesIncharge = [];
    this.salesInchargeItems = [];
    this.brandPriority = [];
    this.brandPriorityItems = [];
    this.customerEntityIds = [];
    this.sisterCompanyIds = [];
  }
}

export class CustomerAddress {
  public id: number;
  public addressType: number;
  public address: string;
  public boxPost: string;
  public zipCode: string;
  public countryId: number;
  public cityId: number;
  //public regionList: Array<any>;
  public cityList: Array<any>;

}
export class CustomerItem {
  public id: number;
  public name: string
}
export enum CustomerSummaryResult {
  Success = 1,
  CannotGetCountryList = 2
}

