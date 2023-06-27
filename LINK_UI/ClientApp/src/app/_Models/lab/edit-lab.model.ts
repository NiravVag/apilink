
export class EditLabModel {
  public id: number;
  public name: string;
  public locLanguageName: string;
  public email: string;
  public phone: string;
  public fax: string;
  public mobile: string;
  public webSite: string;
  public typeId: number;
  public legalName: string;
  public glCode: string;
  public contactPersonName: string;
  public comment: string;
  public addressList: Array<LabAddress>;
  public customerContactList: Array<CustomerContact>;
  public customerList: Array<LabCustomer>;
  constructor() {
    this.addressList = [];
    this.customerList = [];
    this.customerContactList = [];
  }
}

export class LabAddress {
  public id: number;
  public countryId: number;
  public regionId: number;
  public cityId: number;
  public zipCode: string;
  public way: string;
  public addressTypeId: number;
  public localLanguage: string;
  public regionList: Array<any>;
  public cityList: Array<any>;

}

export class CustomerContact {
  public customerList: Array<LabCustomer>;
  public contactId: number;
  public contactName: string;
  public contactEmail: string;
  public jobTitle: string;
  public phone: string;
  public fax: string;
  public mobile: string;
  public comment: string;
}

export class LabCustomer {
  public id: number;
  public name: string;
  public code: string;
}

