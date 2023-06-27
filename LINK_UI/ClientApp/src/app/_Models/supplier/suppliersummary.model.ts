import { BehaviorSubject } from "rxjs";
import { EntityAccess } from "src/app/components/common/static-data-common";
import { CityDataSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ProvinceDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class SupplierSummaryModel extends summaryModel {

  typeValues: Array<number>;
  countryValues: Array<number>;
  suppValues: number;
  items: Array<SupplierSummaryItemModel> = [];
  isRegionalNameChecked: boolean = false;
  customerId: number;
  provinceId: number;
  cityValues: Array<number>;
  email: string;
  phoneNumber: string;
  isEAQF: boolean;
  constructor() {
    super();
    this.typeValues = [];
    this.countryValues = [];
    this.cityValues = [];
    this.isEAQF = false;
  }
}

export class SupplierSummaryItemModel {

  constructor() {
  }

  id: number;
  name: string;
  countryName: string;
  regionName: string;
  cityName: string;
  typeId: number;
  typeName: string;
  list: Array<SupplierSummaryItemModel>;
  isExpand: boolean = false;
  canBeDeleted: number;
}


export class supplierToRemove {

  constructor() {
  }

  id: number;
  name: string;
}

export class SupplierSummaryListModel {
  customerList: any;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  customerRequest: CommonDataSourceRequest;

  provinceList: any;
  provinceInput: BehaviorSubject<string>;
  provinceLoading: boolean;
  provinceRequest: ProvinceDataSourceRequest;

  cityList: any;
  cityInput: BehaviorSubject<string>;
  cityLoading: boolean;
  cityRequest: CityDataSourceRequest;

  constructor() {
    this.customerInput = new BehaviorSubject<string>("");
    this.customerRequest = new CommonDataSourceRequest();

    this.provinceInput = new BehaviorSubject<string>("");
    this.provinceRequest = new ProvinceDataSourceRequest();

    this.cityInput = new BehaviorSubject<string>("");
    this.cityRequest = new CityDataSourceRequest();
  }
}
export class SupplierMasterData{
  entityId:number;
  entityAccess=EntityAccess;
}