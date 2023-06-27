import { DateObject } from "src/app/components/common/static-data-common";

export class TCFDetail {
    barcode: string;
    tcfId: number;
    tcfName: string;
    completeStatus: string;
    statusId: number;
    statusName: string;
    customerName: string;
    customerRef: string;
    supplierName: string;
    supplierRef: string;
    departmentName: string;
    qaName: string;
    merchandiser: string;
    countryOrigin:string;
    destination: string;
    productDescription: string;
    etdDate: string;
    completeDate: string;
    requestDate: string;
    firstDocSubmission: number;
    lastDocReceived: string;
    docReceived: string;
    docRejected: string;
    docUnderReview: string;
    image1: string;
    image2: string;
    productUrl: string;
    productUrl2: string;
    tcfValidate: boolean;
    rawMaterial:string;
}

export class TCFScope {
    standardName: string;
    remark:string;
    trafficColor: number;
    version: string;
    regulationName: string;
    status: string;
    projectStandardId: number;
    scopeDetailList: Array<scopeDetail>;
    scopeDetailLoading: boolean;
    constructor() {
        this.scopeDetailList = [];
        this.scopeDetailLoading = false;
    }
}

export class scopeDetail {
    attachmentId: number;
    attachmentName: string;
    issuerName: string;
    receiveDate: string;
    issueDate: string;
    docExpDate: string;
    versionValidated: string;
    status: string;
    comment: string;
}

export class TCFDetailMaster {
    userToken: string;
    productInfoLoading: boolean;
    tcfScopeLoading: boolean;
    tcfScopeDetailLoading: boolean;
    standardListLoading: boolean;
    issuerListLoading: boolean;
    typeLoading: boolean;
    tcfStatus: string;
    isDetailLoader: boolean;
    completeStatus: number;
    completeStatusColor: number;
    standardList: any;
    typeList: any;
    issuerList: any;
    uploadTCFDocumentLoading: boolean;
    uploadTCFProductFileLoading: boolean;
    currentDate: DateObject;
    constructor() {
        this.productInfoLoading = false;
        this.tcfScopeLoading = false;
        this.tcfScopeDetailLoading = false;
        this.standardList = [];
        this.typeList = [];
        this.issuerList = [];
        this.uploadTCFDocumentLoading = false;
        this.uploadTCFProductFileLoading=false;
        var date = new Date();
        var currentDate = new DateObject();
        currentDate.day = date.getDate();
        currentDate.month = date.getMonth() + 1;
        currentDate.year = date.getFullYear();
        this.currentDate = currentDate;

    }
}

export class TCFDocumentUpload {
    documentName: string;
    standardIds: any;
    typeId: number;
    issuerId: number;
    tcfId: number;
    issueDate: any;
}


export enum CompleteStatusColor {
    Red = 1,
    Grey = 2,
    Orange = 3,
    Green = 4
}

export enum ValidateTCFResponse {
    Success = 1,
    Failure = 2
}

export enum TCFProductInfoResponse {
    Success = 1,
    NotFound = 2
}

export enum TCFScopeResponse {
    Success = 1,
    NotFound = 2
}

export enum TCFScopeDetailResponse {
    Success = 1,
    NotFound = 2
}

export enum TCFUploadDocumentResponse {
    Success = 1,
    Failure = 2
}

export class ProductFileInfo {
    public uploadLimit: number;
    fileSize: number;
    public uploadFileExtensions: string;
    constructor() {
        this.uploadLimit = 10;
        this.fileSize = 2048;
        this.uploadFileExtensions = "jpg,png,gif";
    }
}