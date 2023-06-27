import { Validator } from "src/app/components/common";
import { summaryModel } from "../summary.model";

export class TCFDocumentRequest extends summaryModel {
   searchTypeId: number;
   searchTypeText: string = "";
   statusIds: Array<number>;
   customerIds: Array<number>;
   customerGLCodes: Array<string>;
   searchtypeId: number;
   searchtypetext: any;
   datetypeid: any;
   fromdate: any;
   todate: any;
   fromdateValue: any;
   todateValue: any;
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
   dateTypeId: any;

}


export class TCFDocumentResponse {
   tcfDocumentList: Array<TCFDocumentItem>;
   constructor() {
      this.tcfDocumentList = [];
   }
}


export class TCFDocumentItem {
   documentName: string;
   isSelected: boolean;
   tcfId: number;
   tcfDocumentDetails: Array<TCFDocumentDetail>;
   constructor() {
      this.tcfDocumentDetails = new Array<TCFDocumentDetail>();
   }
}

export class TCFDocumentDetail {
   trafficColor: number;
   attachmentName: string;
   statusOfStandard: string;
   docName: string;
   docType: string;
   dateReceived: string;
   docStatus: string;
   comment: string;
}

export enum Vindication {
   Orange = 1,
   Red = 2,
   Green = 3
}

export class TCFDocumentUpload {
   tcfName: string;
   documentName: string;
   standardIds: any;
   typeId: number;
   issuerId: number;
   tcfId: number;
   issueDate: any;
}

export class TCFDocumentUploadValidator {
   tcfDocumentUpload: TCFDocumentUpload;
   documentValidator: Validator;
   constructor() {
      this.tcfDocumentUpload = new TCFDocumentUpload();
   }
}

export class TCFDocumentMaster {
   standardList: any;
   typeList: any;
   issuerList: any;
   standardListLoading: boolean;
   typeLoading: boolean;
   isDetailLoader: boolean;
   issuerListLoading: boolean;
   uploadTCFDocumentLoading: boolean;
   tcfDocumentValidators: Array<any>;
   constructor() {
      this.standardList = [];
      this.typeList = [];
      this.issuerList = [];
      this.standardListLoading = false;
      this.typeLoading = false;
      this.isDetailLoader = false;
      this.issuerListLoading = false;
      this.uploadTCFDocumentLoading = false;
   }
}

export enum TCFDocumentDetailResponse{
   Success=1,
   NotFound=2
}