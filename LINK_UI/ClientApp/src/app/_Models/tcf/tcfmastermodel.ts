import { array } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { BuyerDataSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, SupplierDataSourceRequest } from "../common/common.model";

export class TCFMasterModel {

   statusList: any;
   buyerDepartmentList: any;

   customerList: any;
   customerLoading: boolean;
   customerInput: BehaviorSubject<string>;
   requestCustomerModel: CustomerDataSourceRequest;

   supplierList: any;
   supplierLoading: boolean;
   supplierInput: BehaviorSubject<string>;
   requestSupplierModel: SupplierDataSourceRequest;

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
   requestCountryDestinationModel: CountryDataSourceRequest;

   countryDestinationList: any;
   countryDestinationLoading: boolean;
   countryDestinationInput: BehaviorSubject<string>;

   statusLoading: boolean = false;
   buyerdepartmentLoading: boolean = false;

   parentCustomerIds: any;

   searchloading: boolean = false;

   isProcessLoader: boolean;

   constructor() {
      this.customerInput = new BehaviorSubject<string>("");
      this.requestCustomerModel = new CustomerDataSourceRequest();

      this.supplierInput = new BehaviorSubject<string>("");
      this.requestSupplierModel = new SupplierDataSourceRequest();

      this.buyerInput = new BehaviorSubject<string>("");
      this.requestBuyerModel = new BuyerDataSourceRequest();

      this.customerContactInput = new BehaviorSubject<string>("");
      this.requestCustomerContactModel = new CustomerContactSourceRequest();

      this.productCategoryInput = new BehaviorSubject<string>("");
      this.requestProductCategoryModel = new ProductCategorySourceRequest();

      this.productSubCategoryInput = new BehaviorSubject<string>("");
      this.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();

      this.countryOriginInput = new BehaviorSubject<string>("");
      this.countryDestinationInput = new BehaviorSubject<string>("");
      this.requestCountryModel = new CountryDataSourceRequest();
      this.requestCountryDestinationModel = new CountryDataSourceRequest();

      this.isProcessLoader = false;
   }
}
