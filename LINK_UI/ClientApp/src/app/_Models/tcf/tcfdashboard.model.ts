import { BehaviorSubject } from "rxjs";
import { BuyerDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, SupplierDataSourceRequest } from "../common/common.model";

const config = require("src/assets/appconfig/appconfig.json");

export class TCFDocumentRequestData {
    requestBase: string;
    requestUrl: string;
    token: string;
    constructor() {
        this.requestBase = config.TCF.baseUrl;
        this.token = config.TCF.masterToken;
    }
}

export class TCFDashBoardMaster {
    tokenLoading: boolean;
    userToken: string;
    searchLoading: boolean;
    tcfStatusLoading: boolean;
    tcfInProgressLoading: boolean;
    tcfSupplierBoardLoading: boolean;
    tcfRejectedAnalysisLoading: boolean;
    tcfScopeDashboardLoading: boolean;
    tcfStatusNotFound: boolean;
    tcfInProgressNotFound: boolean;
    tcfSupplierBoardNotFound: boolean;
    tcfRejectedAnalysisNotFound: boolean;
    tcfScopeDashboardNotFound: boolean;
    isGenericToken: boolean = false;
    emptyToken: string = "";


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

    statusLoading: boolean = false;
    statusList: any;
    tcfTermList: { id: number; name: string; }[];
    compareBy: number;
    isDateShow: boolean;
    isTCFTermShow: boolean;
    constructor() {
        this.tcfRejectedAnalysisNotFound = false;
        this.tcfInProgressNotFound = false;
        this.tcfScopeDashboardNotFound=false;
        this.isGenericToken = false;
        this.emptyToken = "";
  
        this.supplierInput = new BehaviorSubject<string>("");
        this.requestSupplierModel = new SupplierDataSourceRequest();

        this.customerInput = new BehaviorSubject<string>("");
        this.requestCustomerModel = new CustomerDataSourceRequest();
  
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
    }
}

export class TCFFilterRequest {
    fromDate: any;
    toDate: any;
    supplierIds: Array<number>;
    globalDateTypeId: number;
    customerIds: Array<string>;
    searchTypeId: number;
    searchTypeText: string;
    statusIds: Array<number>;
    countryOriginIds: Array<number>;
    countryDestinationIds: Array<number>;
    customerContactIds: Array<number>;
    buyerIds: Array<number>;
    buyerDepartmentIds: Array<number>;
    product: string;
    productCategoryIds: Array<number>;
    productSubCategoryIds: Array<number>;
    pictureUploaded: boolean;
    pageSize: number;
    index: number;
    constructor() {
        this.supplierIds = [];
        this.customerIds = [];
    }
}

export class TCFDashboardResponse {
    statusDashBoardData: StatusDashBoardData;
    rejectedAnalysisData: RejectedAnalysisData;
    rejectedRate: RejectedRate;
    supplierScoreBoard: SupplierScoreBoard;
    scopeDashboardData: ScopeDashboardData;
    constructor() {
        this.statusDashBoardData = new StatusDashBoardData();
        this.rejectedAnalysisData = new RejectedAnalysisData();
        this.rejectedRate = new RejectedRate();
        this.supplierScoreBoard = new SupplierScoreBoard();
        this.scopeDashboardData = new ScopeDashboardData();
    }
}

export class SupplierScoreBoard {
    averageCompletionTime: number;
    averageFirstDocSubmission: number;
    constructor() {
        this.averageCompletionTime = 0;
        this.averageFirstDocSubmission = 0;
    }
}

export class StatusDashBoardData {
    inProgressCount: number;
    completedCount: number;
    finlizedCount: number;
    constructor() {
        this.inProgressCount = 0;
        this.completedCount = 0;
        this.finlizedCount = 0;
    }
}


export class RejectedAnalysisData {
    docReceived: number;
    docUnderReview: number;
    docPending: number;
    docValidated: number;
    docRejected: number;
    docUnderReviewedPerecentage: number;
    docPendingPercentage: number;
    docValidatedPercentage: number;
    docRejectedPercentage: number;
    constructor() {
        this.docReceived = 0;
        this.docUnderReview = 0;
        this.docPending = 0;
        this.docValidated = 0;
        this.docRejected = 0;
        this.docUnderReviewedPerecentage = 0;
        this.docPendingPercentage = 0;
        this.docValidatedPercentage = 0;
        this.docRejectedPercentage = 0;
    }
}

export class ScopeDashboardData {
    scopeUnderReview: number;
    scopeWaiting: number;
    scopeConfirm: number;
    scopeExpired: number;
    scopeNotSet: number;
    scopeUnderReviewPerecentage: number;
    scopeWaitingPercentage: number;
    scopeConfirmPercentage: number;
    scopeExpiredPercentage: number;
    scopeNotSetPercentage: number;
    constructor() {
        this.scopeUnderReview = 0;
        this.scopeWaiting = 0;
        this.scopeConfirm = 0;
        this.scopeExpired = 0;
        this.scopeNotSet = 0;
        this.scopeUnderReviewPerecentage = 0;
        this.scopeWaitingPercentage = 0;
        this.scopeConfirmPercentage = 0;
        this.scopeExpiredPercentage = 0;
        this.scopeNotSetPercentage = 0;
    }
}

export class RejectedRate {
    docReceived: number;
    docWaiting: number;
    docRejected: number;
    docRejectedPercentage: number;
    constructor() {
        this.docReceived = 0;
        this.docWaiting = 0;
        this.docRejected = 0;
        this.docRejectedPercentage = 0;
    }
}

export enum StatusDashboardResponse {
    Success = 1,
    NotFound = 2
}

export enum RejectedAnalysisResponse {
    Success = 1,
    NotFound = 2
}

export enum TCFInProgressResponse {
    Success = 1,
    NotFound = 2
}


export enum TCFScopeDashboardResponse {
    Success = 1,
    NotFound = 2
}

export enum TCFTerm {
    Week = 1,
    Month = 2,
    Year = 3,
    Custom = 4
}

export enum TCFTermEnum {
    LastYear = 1,
    LastSixMonth = 2,
    LastThreeMonth = 3,
    Custom = 5
}

export enum SupplierScoreTermEnum {
    LastTenDay = 1,
    LastThreeMonth = 2,
    LastSixMonth = 3,
    Custom = 6
}