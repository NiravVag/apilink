import { BehaviorSubject } from "rxjs";
import { summaryModel } from "../summary.model";
import { CommonDataSourceRequest, CountryDataSourceRequest ,SupplierDataSourceRequest,ProductDataSourceRequest} from "../common/common.model";
import { PageSizeCommon } from "src/app/components/common/static-data-common";

export class PurchaseOrderSummaryModel extends summaryModel {

  destinationCountry: any[];
  customerValues: any
  purchaseFilterData:PurchaseOrderFilterModel;
  items: Array<PurchaseOrderSummaryItemModel> = [];

  constructor() {
    super();
    this.destinationCountry = [];    
    this.purchaseFilterData=new PurchaseOrderFilterModel();
  }
}

export class PurchaseOrderSummaryItemModel {

  constructor() {
  }
  id:number;
  pono: number;
  customerName: string;
  etd: string; 
  destinationCountry:string;
  isBooked:boolean;
  isDelete: boolean;
  bookingNumber: number;
  bookingCount: number;
  showBookingCount: number;
}

export class PurchaseOrderFilterModel {
  constructor() {
   this.customerId=null;
   this.fromEtd=null;
   this.toEtd=null;
   this.pono="";
   this.destinationCountry=null;
   this.factoryId=null;
   this.supplierId=null;
  }
  customerId: number;
  fromEtd: Date; 
  toEtd:Date;
  pono:string;
  destinationCountry:number;
  factoryId:number;
  supplierId:number;
}

export class purchaseOrderToRemove {

  constructor() {
  }
  id: number;
  name: string;
}
export class PurchaseOrderListModel{
  customerList: any;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  customerRequest: CommonDataSourceRequest;  

  countryList: any;
  countryInput: BehaviorSubject<string>;
  countryLoading: boolean;
  countryRequest: CountryDataSourceRequest;  

  supplierList: any;
  supplierInput: BehaviorSubject<string>;
  supplierLoading: boolean;
  supplierRequest: SupplierDataSourceRequest;  

  productList: any;
  productInput: BehaviorSubject<string>;
  productLoading: boolean;
  productRequest: ProductDataSourceRequest;  
  
  selectedPageSize: number;
  pagesizeitems : number[];
  pageLoader: boolean;
  poBookingDetails: Array<PoBookingDetail>;
  selectedPoNo: string;
  isCustomer: boolean;
  isInternalUser: boolean;
  constructor(){
    this.customerInput = new BehaviorSubject<string>("");
    this.customerRequest = new CommonDataSourceRequest();
    
    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();

    this.supplierInput = new BehaviorSubject<string>("");
    this.supplierRequest = new SupplierDataSourceRequest();

    this.productInput = new BehaviorSubject<string>("");
    this.productRequest = new ProductDataSourceRequest();
    this.pagesizeitems = PageSizeCommon;
    this.poBookingDetails = Array<PoBookingDetail>();
  } 
}

export class PoBookingDetail{
  statusName: string;
  supplierName: string;
  factoryName: string;
  bookingNumber: number;
  statusColor: string;
  serviceDateFrom: string;
  serviceDateTo: string;
}

export enum PoBookingResult {
  success = 1,
  notFound = 2
}

export class PoBookingDetailResponse {
  poBookingDetails: Array<PoBookingDetail>;
  result: PoBookingResult;
}
