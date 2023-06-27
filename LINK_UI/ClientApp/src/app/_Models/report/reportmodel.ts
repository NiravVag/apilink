import { BehaviorSubject } from "rxjs";
import { EntityAccess } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CustomerCommonDataSourceRequest, SupplierDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class CustomerReportModel extends summaryModel {

    public searchtypeid: number;

    public searchtypetext: string = "";

    public customerid: number;

    public supplierid: number;

    public factoryidlst: any[] = [];

    public statusidlst: any[];

    public datetypeid: number;

    public fromdate: any;

    public todate: any;

    public officeidlst: any[] = [];
    public serviceTypelst: any[] = [];
    public customerBookingNo: string;
    callingFrom: number;
    reportDate: any;
    public advancedSearchtypeid: number;

    selectedCountryIdList: number[] = [];
    selectedProvinceIdList: number[] = [];
    selectedCityIdList: number[] = [];
    selectedBrandIdList: number[] = [];
    selectedDeptIdList: number[] = [];
    selectedBuyerIdList: number[] = [];
    barcode: string;
    isPicking: boolean;
    advancedsearchtypetext: string = null;
    supplierTypeId: number;
    isEAQF: boolean = false;
}

export class ReportItem {

    public bookingId: number;
    public customerBookingNo: string;
    public customerId: number;
    public customerName: string;
    public supplierName: string;
    public factoryName: string;
    public serviceType: string;
    public serviceTypeId: string;
    public serviceDateFrom: string;
    public productCategory: string;
    public serviceDateTo: string;
    public internalReferencePo: string;
    public office: string;
    public statusId: number;
    public bookingCreatedBy: number;
    public isPicking: false;
    public productList: any[];
    public factoryId: number;
    public isExpand: boolean = false;
    public reportSummaryLink: string;
    public reportDate: string;
    productCount: number;
    public bookingType: number;
    public isEAQF: boolean = false;
}

export class StatusRequest {
    bookingId: number;
    reportDate: any;
}

export enum CustomerReportSummaryResponseResult {
    Success = 1,
    NotFound = 2,
    Other = 3
}

export class ReportMaster {
    requestCustomerModel: CommonDataSourceRequest;
    requestCustomerCommonModel: CustomerCommonDataSourceRequest;
    customerInput: BehaviorSubject<string>;
    customerLoading: boolean;
    public customerList: any;

    supsearchRequest: CommonDataSourceRequest;
    supsearchDataRequest: SupplierDataSourceRequest;
    supInput: BehaviorSubject<string>;
    supLoading: boolean;
    public supplierList: any;

    facsearchRequest: SupplierDataSourceRequest;
    facInput: BehaviorSubject<string>;
    facLoading: boolean;
    public factoryList: any;

    bookingStatusList: any;
    bookingStatusLoading: boolean;

    officeLoading: boolean;
    officeList: any;
    DateName: string;
    supplierName: string;
    customerName: string;
    officeNameList: Array<string>;
    factoryNameList: Array<string>;
    countryNameList: Array<string>;
    serviceTypeNameList: Array<string>;
    brandNameList: Array<string>;
    buyerNameList: Array<string>;
    deptNameList: Array<string>;
    statusNameList: Array<string>;
    bookingType: number;
    inspectionBookingTypeVisible: boolean;;
    inspectionBookingTypes = [];

    selectedDate: string;
    selectedNumber: string;
    selectedNumberPlaceHolder: string;
    isShowSearchLens: boolean;

    serviceTypeList: any;
    serviceTypeLoading: any;

    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    countryList: any;
    countryLoading: boolean;
    countryInput: BehaviorSubject<string>;
    statusLoading: boolean;

    provinceLoading: boolean
    cityLoading: boolean;

    provinceList: any;
    cityList: any;
    pageLoader: boolean;

    productList: any;
    poList: any;
    containerProductList: any;
    containerPOList: any;
    containerItem: any;
    bookingItem: any
    isproductOrPODetails: boolean;
    bookingNumber: string;
    productName: string;
    productId: number;
    entityId:number;
    entityAccess=EntityAccess;

    constructor() {
        this.customerList = [];
        this.customerInput = new BehaviorSubject<string>("");
        this.requestCustomerModel = new CommonDataSourceRequest();
        this.requestCustomerCommonModel = new CustomerCommonDataSourceRequest();

        this.supsearchRequest = new CommonDataSourceRequest();
        this.supInput = new BehaviorSubject<string>("");
        this.supsearchDataRequest = new SupplierDataSourceRequest();

        this.facsearchRequest = new SupplierDataSourceRequest();
        this.facInput = new BehaviorSubject<string>("");

        this.customerLoading = false;
        this.supLoading = false;
        this.facLoading = false;
        this.bookingStatusLoading = false;
        this.officeLoading = false;
        this.countryInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
    }
}

export class UploadCustomReport {
    reportFileName: string;
    reportFileType: string;
    reportFileUrl: string;
    reportFileUniqueId: string;
    constructor() {
        this.reportFileName = "";
        this.reportFileType = "";
        this.reportFileUrl = "";
        this.reportFileUniqueId = "";
    }
}

export class UploadCustomReportRequest {
    apiReportId: number;
    fileUrl: string;
    uniqueId: string;
    fileName: string;
    fileType: string;
    constructor() {
        this.apiReportId = null;
        this.fileUrl = "";
        this.uniqueId = "";
        this.fileName = "";
        this.fileType = "";
    }
}

export class UploadCustomReportResponse {
    result: UploadCustomReportResult;
}

export enum UploadCustomReportResult {
    Success = 1,
    NotSaved = 2
}

export class ReportFileResponse {
    fileUrl: string;
    result: ReportFileResult;
}

export enum ReportFileResult {
    Success = 1,
    NotFound = 2
}

export class CustomReportMaster {
    reportFileName: string;
    reportFileType: string;
    reportFileUrl: string;
    reportFileUniqueId: string;
    apiReportId: number;
    constructor() {
        this.reportFileName = "";
        this.reportFileType = "";
        this.reportFileUrl = "";
        this.reportFileUniqueId = "";
        this.apiReportId = null;
    }
}