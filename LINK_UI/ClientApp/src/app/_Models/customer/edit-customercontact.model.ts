
export class EditCustomerContactModel {
  public id: number;
  public name: string;
  public lastName: string;
  public jobTitle: string;
  public email: string;
  public mobile: string;
  public phone: string;
  public fax: string;
  public others: string;
  public office: number;
  public comments: string;
  public contactTypes: any[];
  public contactTypeItems: any[];
  public promotionalEmail: number;
  public customerId: number;
  public active: boolean;
  public customerAddressList: any;
  public contactBrandList: any;
  public contactDepartmentList: any;
  public contactServiceList: any;
  public apiEntityIds: any;
  public entityServiceIds: any;
  public primaryEntity: number;
  public reportTo: number;
  contactSisterCompanyIds: number[] = [];
  constructor() {
    this.id = 0;
    this.primaryEntity = null;
    this.contactSisterCompanyIds = [];
  }
}

