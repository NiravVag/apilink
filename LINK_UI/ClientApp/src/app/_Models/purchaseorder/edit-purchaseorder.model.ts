import { PurchaseOrderSummaryModel } from "./purchaseordersummary.model";
import { summaryModel } from "../../_Models/summary.model";
import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, CountryDataSourceRequest, ProductDataSourceRequest, SupplierDataSourceRequest } from "../common/common.model";

export class EditPurchaseOrderModel {
  public id: number;
  public pono: string;
  public officeId: number;
  public customerId: number;
  public supplierIds: Array<number>;
  public factoryIds: Array<number>;
  public departmentId: string;
  public brandId: string;
  public internalRemarks: string;
  public customerRemarks: string;
  public active: boolean;
  public createdBy: string;
  public createdTime: string;
  public accessType: number;
  public supplierId: number;
  public destinationCountryId: number;
  public etd: Date;
  public customerReferencePo: string;
  public purchaseOrderDetails: Array<purchaseOrderDetail>;
  public purchaseOrderAttachments: Array<AttachmentFile>;
  constructor() {
    this.id = 0;
    this.active = true;
    this.purchaseOrderDetails = [];
    this.purchaseOrderAttachments = [];
    this.supplierIds = [];
    this.factoryIds = [];
  }
}
export class purchaseOrderDetailModel extends summaryModel {
  public id: number;
}

export class purchaseOrderDetail {

  public id: number;
  public poId: number;
  public productId: number;
  public productName: string;
  public bookingStatus: number;
  public productDesc: string;
  public destinationCountryId: number;
  public etd: Date;
  public supplierId: number;
  public factoryId: number;
  public factoryReference: string;
  public quantity: number;
  public active: boolean;
  public factoryList: any;
  public isBooked: boolean;

  productList: any;
  productInput: BehaviorSubject<string>;
  productLoading: boolean;
  productRequest: ProductDataSourceRequest;

  countryList: any;
  countryInput: BehaviorSubject<string>;
  countryLoading: boolean;
  countryRequest: CountryDataSourceRequest;

  /* supplierList: any;
  supplierInput: BehaviorSubject<string>;
  supplierLoading: boolean;
  supplierRequest: SupplierDataSourceRequest; */

  constructor() {


    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();

    /* this.supplierInput = new BehaviorSubject<string>("");
    this.supplierRequest = new SupplierDataSourceRequest(); */

    this.productInput = new BehaviorSubject<string>("");
    this.productRequest = new ProductDataSourceRequest();

  }

}

export class purchaseOrderAttachment {
  public id: number;
  public poId: number;
  public documentDescription: string;
  public document: any;
  public active: boolean;
  public createdBy: number;
  public createdTime: Date;
  public deletedBy: number;
  public deletedTime: Date;
}

export class AttachmentFile {
  public id: number;
  public uniqueld: string;
  public fileName: string;
  public isNew: boolean;
  public mimeType: string;
  public file: File;
}
export class RemovePurchaseOrderDetail {
  public id: number;
  public accessType: number;
}

export class PurchaseOrderUploadData {
  public id: number;
  public pono: number;
  public product: string;
  public productBarcode: string;
  public productDescription: string
  public etd: string;
  public quantity: number;
  public destinationCountry: string
  public customer: string;
  public supplier: string
  public bookingDate: string
  public isProductNew: boolean
  public isSelected: boolean
}
export class AutoPoNumber {
  public customerId: number;
  public poname: string;
}

export class AutoPoNumberPage {
  public customerId: number;
  public poname: string;
  public page: number;
  public pageSize: number;
}

export class ProductList {
  id: number;
  name: string;
  description: string;
}


export class CommonCustomerProductDataSource {
  id: number;
  productId: string;
  productDescription: string;
}

export class CustomerProductDataSourceResponse {
  dataSourceList: any;
  result: DataSourceResult;
}

export class PurchaseOrderMaster {
  
  supplierList: any;
  supplierInput: BehaviorSubject<string>;
  supplierLoading: boolean;
  supplierRequest: CommonDataSourceRequest;

  factoryList: Array<number>;
  factoryInput: BehaviorSubject<string>;
  factoryLoading: boolean;
  factoryRequest: SupplierDataSourceRequest;
  editSupplierList: Array<number>;
  editFactoryList: Array<number>;

  

  constructor() {
    this.supplierList = [];
    this.supplierInput = new BehaviorSubject<string>("");
    this.supplierRequest = new CommonDataSourceRequest();

    this.factoryList = [];
    this.factoryInput = new BehaviorSubject<string>("");
    this.factoryRequest = new SupplierDataSourceRequest();
    this.editSupplierList = [];
    this.editFactoryList = [];
  }
}

export enum DataSourceResult {
  Success = 1,
  CannotGetList = 2,
  Failed = 3,
  RequestNotCorrectFormat = 4,
  ServiceIdRequired = 5

}