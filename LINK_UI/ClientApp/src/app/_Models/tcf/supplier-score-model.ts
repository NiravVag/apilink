import { BehaviorSubject } from "rxjs";
import { BuyerDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, SupplierDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class SupplierScoreModel extends summaryModel {
  searchTypeId: number;
  searchTypeText: string = "";
  fromdate: any;
  todate: any;
  statusIds: Array<number>;
  customerGLCodes: Array<string>;
  supplierIds: Array<number>;
  buyerIds: Array<number>;
  productCategoryIds: Array<number>;
  productSubCategoryIds: Array<number>;
  customerContactIds: Array<number>;
  countryOriginIds: Array<number>;
  countryDestinationIds: Array<number>;
  pageIndex: number;
  globalDateTypeId: number;
}

export class SupplierScoreMaster {
  tokenLoading: boolean;
  userToken: string;

  supplierList: any;
  supplierLoading: boolean;
  supplierInput: BehaviorSubject<string>;
  requestSupplierModel: SupplierDataSourceRequest;
  
  customerList: any;
  customerLoading: boolean;
  customerInput: BehaviorSubject<string>;
  requestCustomerModel: CustomerDataSourceRequest;
  
  buyerList: any;
  buyerLoading: boolean;
  buyerInput: BehaviorSubject<string>;
  requestBuyerModel: BuyerDataSourceRequest;
  
  customerContactList: any;
  customerContactLoading: boolean;
  customerContactInput: BehaviorSubject<string>;
  requestCustomerContactModel: CustomerContactSourceRequest;
  
  productCategoryList: any;
  productCategoryLoading: boolean;
  productCategoryInput: BehaviorSubject<string>;
  requestProductCategoryModel: ProductCategorySourceRequest;

  productSubCategoryList: any;
  productSubCategoryLoading: boolean;
  productSubCategoryInput: BehaviorSubject<string>;
  requestProductSubCategoryModel: ProductSubCategorySourceRequest;

  countryOriginList: any;
  countryOriginLoading: boolean;
  countryOriginInput: BehaviorSubject<string>;
  requestCountryModel: CountryDataSourceRequest;

  countryDestinationList: any;
  countryDestinationLoading: boolean;
  countryDestinationInput: BehaviorSubject<string>;
  requestCountryDestinationModel: CountryDataSourceRequest;

  statusLoading: boolean;
  statusList: any;
  supplierScoreTermList: { id: number; name: string; }[];
  isDateShow: boolean;

  supplierScoreDocReceivedLoading: boolean;
  supplierScoreDocReceivedDataNotFound: boolean;
  supplierScoreDocReceivedlst: any;

  supplierScoreDocReviewedLoading: boolean;
  supplierScoreDocReviewedDataNotFound: boolean;
  supplierScoreDocReviewedlst: any;

  supplierScoreDocProvideLoading: boolean;
  supplierScoreDocProvideDataNotFound: boolean;
  supplierScoreDocProvidelst: any;

  supplierScoreListLoading: boolean;
  supplierScoreListDataNotFound: boolean;
  supplierScoreList: any;
  
  supplierScoreDocReceivedSupplierNamelst: Array<string>;
  supplierScoreDocStatelst: Array<string>;

  constructor() {
    this.tokenLoading = false;  
    this.statusLoading = false;
    this.isDateShow = false;
    this.supplierScoreDocReceivedLoading = false;
    this.supplierScoreDocReviewedLoading = false;
    this.supplierScoreDocProvideLoading = false;
    this.supplierScoreListLoading = false;
    this.supplierScoreDocReceivedSupplierNamelst = [];
    this.supplierScoreDocStatelst = [];
    this.supplierScoreDocReceivedDataNotFound = false;
    this.supplierScoreDocReviewedDataNotFound = false;
    this.supplierScoreDocProvideDataNotFound = false;
    this.supplierScoreListDataNotFound = false;

    this.supplierLoading = false;
    this.supplierInput = new BehaviorSubject<string>("");
    this.requestSupplierModel = new SupplierDataSourceRequest();

    this.customerLoading = false;
    this.customerInput = new BehaviorSubject<string>("");
    this.requestCustomerModel = new CustomerDataSourceRequest();

    this.buyerLoading = false;
    this.buyerInput = new BehaviorSubject<string>("");
    this.requestBuyerModel = new BuyerDataSourceRequest();

    this.customerContactLoading = false;
    this.customerContactInput = new BehaviorSubject<string>("");
    this.requestCustomerContactModel = new CustomerContactSourceRequest();

    this.productCategoryLoading = false;
    this.productCategoryInput = new BehaviorSubject<string>("");
    this.requestProductCategoryModel = new ProductCategorySourceRequest();

    this.productSubCategoryLoading = false;
    this.productSubCategoryInput = new BehaviorSubject<string>("");
    this.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();

    this.countryOriginLoading = false;
    this.countryOriginInput = new BehaviorSubject<string>("");
    this.requestCountryModel = new CountryDataSourceRequest();

    this.countryDestinationLoading = false;
    this.countryDestinationInput = new BehaviorSubject<string>("");
    this.requestCountryDestinationModel = new CountryDataSourceRequest();
  }
}
