import { summaryModel } from "../summary.model";
const config = require("src/assets/appconfig/appconfig.json");

export class TCFLandingRequest extends summaryModel {
   statusIds: Array<number>;
   customerIds: Array<number>;
   customerGLCodes: Array<string>;
   searchTypeId: number;
   searchTypeText: string = "";
   dateTypeId: any;
   fromdate: any;
   todate: any;
   supplierIds: Array<number>;
   buyerIds: Array<number>;
   buyerDepartmentIds: Array<number>;
   customerCategoryIds: Array<number>;
   productCategoryIds: Array<number>;
   productSubCategoryIds: Array<number>;
   customerContactIds: Array<number>;
   countryOriginIds: Array<number>;
   countryDestinationIds: Array<number>;
   pictureUploaded: boolean;
   pageSize: number;
   pageIndex: number;
   globalDateTypeId: number;
   supplierTypeId: number;

}

export class TCFResponse {
   tcfList: Array<TCFItem>;
   tcfDetail: TCFDetail;
   constructor() {
      this.tcfList = [];
      this.tcfDetail = new TCFDetail();
   }
}

export class TCFDetailResponse {
   tcfDetail: TCFDetail;
   constructor() {
      this.tcfDetail = new TCFDetail();
   }
}

export class TCFItem {
   tcfId: number;
   tcfNo: string;
   trafficLight: string;
   trafficColor: string;
   status: string;
   statusId: number;
   completeStatus: string;
   customrRefShortName:string;
   customerRefName: string;
   customerRef: string;
   productCategory: string;
   supplier: string;
   customer: string;
   firstETD: string;
   requestDate: string;
}

export class TCFDetail {
   productImage: string;
   productUrl: string;
   customerContact: string;
   merchandisingContact: string;
   buyer: string;
   buyerDepartment: string;
   firstDocumentSubmission: string;
   lastDocumentReceived: string;
   documentsReceived: number;
   documentsRejected: number;
   documentsUnderReview: number;

   constructor() {
      this.productImage = "";
      this.customerContact = "";
      this.merchandisingContact = "";
      this.buyer = "";
      this.buyerDepartment = "";
      this.firstDocumentSubmission = "";
      this.lastDocumentReceived = "";
      this.documentsReceived = 0;
      this.documentsRejected = 0;
      this.documentsUnderReview = 0;
   }
}

export enum TrafficLightColor {
   Red = 1,
   Orange = 2,
   Green = 3
}

export class TCFListRequest {
   searchTypeId: number;
   searchTypeText: any;
   statusIds: Array<number>;
   customerIds: Array<string>;
   customerGLCodes: Array<string>;
   supplierIds: Array<number>;
   buyerIds: Array<number>;
   customerContactIds: Array<number>;
   buyerDepartmentIds: Array<number>;
   productCategoryIds: Array<number>;
   productSubCategoryIds: Array<number>;
   countryOriginIds: Array<number>;
   countryDestinationIds: Array<number>;

   dateTypeId: any;
   fromDate: any;
   toDate: any;

   pictureUploaded: boolean;
   pageSize: number;
   index: number;

   constructor(){
      this.customerIds=[];
      this.customerGLCodes=[];
   }

}

export const datetypelst = [
   { name: "Service Date", id: 4 },
   { name: "Apply date", id: 5 },
   { name: "First Service Date", id: 9 }
];

interface KeyValuePair {
   key: string;
   value: string;
}

export class userTokenRequest {
   userId: number;
   userType: string;
}

export class TCFRequestData {
   requestBase: string;
   requestUrl: string;
   token: string;
   isGenericToken:boolean;
   client:number;
   constructor() {
      this.requestBase = config.TCF.baseUrl;
      this.token = config.TCF.masterToken;
   }
}

export enum ValidateTCFResponse {
   Success = 1,
   Failure = 2
}

export enum TCFListResponse {
   Success = 1,
   NotFound = 2
}

export enum TCFDetailsResponse {
   Success = 1,
   NotFound = 2
}

export enum TCFValidDocumentResponse {
   Success = 1,
   NotFound = 2
}

export enum TCFReportResponse {
   Success = 1,
   NotFound = 2
}
