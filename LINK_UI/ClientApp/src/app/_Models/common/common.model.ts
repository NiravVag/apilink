import { APIService, ListSize, SupplierType } from "src/app/components/common/static-data-common";
import { commonDataSource } from "../booking/inspectionbooking.model";

export interface DataSource {
  id: number;
  name: string;
}

export interface ParentDataSource {
  id: number;
  name: string;
  parentId: number;
}
export enum ResponseResult {
  Success = 1,
  NoDataFound = 2
}

export class DataSourceResponse {
  dataSourceList: Array<DataSource>;
  result: ResponseResult;
}

export class CommonDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public supplierType: number;
  public id: number;
  public idList: Array<number>;
  locationId: number;
  supSearchTypeId: number;
  customerglCodes: Array<string>;
  serviceId: number;
  factoryId: number;
  locationIdList: Array<number>;
  constructor() {
    this.serviceId = APIService.Inspection;
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.idList = [];
    this.locationIdList = [];
    this.supSearchTypeId = 0;
  }
}

export class StaffDataSourceRequest extends CommonDataSourceRequest {
  employeeType: number;
  outSourceCompanyId: number;
}

export class CustomerCommonDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public supplierType: number;
  public id: number;
  public idList: Array<number>;
  locationId: number;
  public supSearchTypeId: number;
  public isStatisticsVisible: boolean;
  public isVirtualScroll: boolean;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.idList = [];
    this.supSearchTypeId = 0;
    this.isStatisticsVisible = false;
    this.isVirtualScroll = true;
  }
}

export class SupplierCommonDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public factoryId: number;
  public supplierType: number;
  public id: number;
  public idList: Array<number>;
  locationId: number;
  public supSearchTypeId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.idList = [];
    this.supSearchTypeId = 0;
  }
}

export class UserDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public supplierId: number;
  public supplierType: number;
  public id: number;
  public idList: Array<number>;
  locationId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.idList = [];
  }
}

export class CountryDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public countryIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.countryIds = [];
  }
}

export class CommonSupplierSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public countryIds: Array<number>;
  public supplierTypes: Array<number>;
  public id: number;
  public customerId: number;
  public provinceId: number;
  public cityIds: Array<number>;
  public isRegionalNameChecked: boolean;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.countryIds = [];
    this.supplierTypes = [];
    this.cityIds = [];
  }
}

export class CommonCustomerSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public customerId: number;
  public idList: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}
export class CommonZoneSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public zoneId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}
export class CommonOfficeZoneSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public zoneId: number;
  public officeIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.officeIds = [];
  }
}
export class CommonCountySourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public cityId: number;
  public countyId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export class QcDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public qcIds: Array<number>;
  public officeCountryIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.officeCountryIds = [];
    this.qcIds = [];
  }
}



export class CustomerDataSourceRequest {
  searchText: string;
  serviceId: number;
  customerglCodes: Array<string>;
  skip: number;
  take: number;

  constructor() {
    this.serviceId = APIService.Tcf;
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
  }
}

export class SupplierDataSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  customerglCodes: Array<string>;
  serviceId: number;
  supplierIds: Array<number>;
  supplierType: number;
  skip: number;
  take: number;
  supSearchTypeId: number;
  constructor() {
    this.serviceId = APIService.Inspection;
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
    this.supplierType = SupplierType.Supplier;
    this.customerIds = [];
    this.supplierIds = [];
  }
}

export class SupplierAllDataSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  customerglCodes: Array<string>;
  serviceId: number;
  supplierIds: Array<number>;
  supplierType: number;
  supSearchTypeId: number;
  constructor() {
    this.serviceId = APIService.Inspection;
    this.searchText = "";
    this.supplierType = SupplierType.Supplier;
    this.customerIds = [];
    this.supplierIds = [];
  }
}


export class ProductDataSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  productIds: Array<number>;
  id: number;
  skip: number;
  take: number;
  supplierIdList: Array<number>;
  factoryIdList: Array<number>;

  constructor() {
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
    this.customerIds = [];
    this.productIds = [];
    this.supplierIdList = [];
    this.factoryIdList = [];
  }
}

export class ProductDetailsDataSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  id: number;
  skip: number;
  take: number;

  constructor() {
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
    this.customerIds = [];
  }
}

export class BuyerDataSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  customerglCodes: Array<string>;
  serviceId: number;
  buyerIds: Array<number>;
  skip: number;
  take: number;

  constructor() {
    this.serviceId = APIService.Inspection;
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
  }
}

export class CustomerContactSourceRequest {
  searchText: string;
  customerIds: Array<number>;
  customerglCodes: Array<string>;
  contactTypeIds: Array<number>;
  serviceId: number;
  contactIds: Array<number>;
  skip: number;
  take: number;

  constructor() {
    this.serviceId = APIService.Inspection;
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
  }
}


export class ProductCategorySourceRequest {
  searchText: string;
  productCategoryIds: Array<number>;
  serviceId: number;
  skip: number;
  take: number;

  constructor() {
    this.serviceId = APIService.Inspection;
    this.productCategoryIds = [];
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
  }
}

export class ProductSubCategorySourceRequest {
  searchText: string;
  serviceId: number;
  productCategoryIds: Array<number>;
  productSubCategoryIds: Array<number>;
  skip: number;
  take: number;

  constructor() {
    this.serviceId = APIService.Inspection;
    this.productCategoryIds = [];
    this.productSubCategoryIds = [];
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
  }
}

export class ProductSubCategory2SourceRequest {
  searchText: string;
  productCategoryId: number;
  productSubCategoryIds: Array<number>;
  productSubCategory2Ids: Array<number>;
  skip: number;
  take: number;

  constructor() {
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
    this.productCategoryId = 0;
    this.productSubCategoryIds = [];
    this.productSubCategory2Ids = [];
  }
}


export class ProductSubCategory3SourceRequest {
  searchText: string;
  productCategoryId: number;
  productSubCategoryId: number;
  productSubCategory3Ids: Array<number>;
  productSubCategory2Ids: Array<number>;
  skip: number;
  take: number;

  constructor() {
    this.skip = 0;
    this.take = 10;
    this.searchText = "";
    this.productCategoryId = 0;
    this.productSubCategoryId = 0;
    this.productSubCategory3Ids = [];
    this.productSubCategory2Ids = [];
  }
}

export class ProvinceDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public countryIds: Array<number>;
  public provinceId: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.countryIds = [];
  }
}

export class CityDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public provinceId: number;
  public cityIds: Array<number>;
  public countryIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.cityIds = [];
  }
}

export class TownDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public countyId: number;
  public townIds: Array<number>;
  public countryIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export class UserServiceAccess {
  public id: number;
  public name: string;
  public userHasAccess: boolean;
  public serviceEnabledFileName: string;
  public serviceDisabledFileName: string;
  constructor() {
    this.userHasAccess = false;
    this.serviceEnabledFileName = "";
    this.serviceDisabledFileName = "";
  }
}

export class UserRightTypeAccess {
  public id: number;
  public name: string;
  public userHasAccess: boolean;
  public rightTypeFileName: string;
  constructor() {
    this.userHasAccess = false;
    this.rightTypeFileName = "";
  }
}

export class StartPortDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public startPortIds: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export class ProvinceListSearchRequest {
  countryIds: Array<number>;
}

export class CityListSearchRequest {
  provinceIds: Array<number>;
}

export class BookingIdDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public ids: Array<number>;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}


export class ProvinceListResponse {
  dataSourceList: Array<commonDataSource>;
  result: ProvinceResult;
}

export enum ProvinceResult {
  Success = 1,
  RequestShouldNotBeEmpty = 2,
  CountrySearchFilterEmpty = 3,
  DataNotFound = 4,
  Failed = 5
}

export class CityListResponse {
  dataSourceList: Array<commonDataSource>;
  result: ProvinceResult;
}

export enum CityResult {
  Success = 1,
  RequestShouldNotBeEmpty = 2,
  CountrySearchFilterEmpty = 3,
  DataNotFound = 4,
  Failed = 5
}


export class InvoiceReportTemplate {
  templateId: number;
}

export class InvoiceReportTemplateRequest {
  customerId: string;
  invoicePreviewTypes: string[];
  constructor() {
    this.invoicePreviewTypes = [];
  }
}

export class InvoicePreviewRequest {
  invoiceNo: string;
  previewTitle: string;
  invoicePreviewTypes: Array<any>;
  customerId: string;
  invoicePreviewFrom: InvoicePreviewFrom;
  invoiceData: Array<string> = [];
  bookingData: Array<number> = [];
  service: APIService;
  createdBy: number;
  isAccountingRole: boolean;
}

export enum InvoicePreviewFrom {
  InvoiceSummary = 1,
  InvoiceGenerate = 2,
  InvoiceStatusSummary = 3,
  InvoiceStatusGenerate = 4,
  CreditNoteSummary = 5,
  ExtarFees = 6,
  ManualInvoice = 7
}
