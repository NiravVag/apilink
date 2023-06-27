import { NgxGalleryImage, NgxGalleryOptions } from "ngx-gallery-9";
import { BehaviorSubject } from "rxjs";
import { APIService, DefectCriticalId, DefectDashboardDataLengthTrim, DefectMajorId, DefectMinorId, DefectNameList, ListSize, SupplierType } from "src/app/components/common/static-data-common";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource, ProductDataSourceRequest, ResponseResult, SupplierCommonDataSourceRequest } from "../common/common.model";
import { CustomerInfo } from "../dashboard/customerdashboard.model";
import { summaryModel } from "../summary.model";

export class DefectDashboardModel extends summaryModel {
    fromDate: any;
    toDate: any;
    customerId: number;
    supplierId: number;
    factoryIds: Array<number>;
    factoryCountryIds: Array<number>;
    innerDefectYearId: number;
    selectedBrandIdList: Array<number>;
    selectedBuyerIdList: Array<number>;
    selectedCollectionIdList: Array<number>;
    selectedDeptIdList: Array<number>;
    selectedProdCategoryIdList: Array<number>;
    selectedProductIdList: Array<number>;
    supplierTypeId: number;
    serviceTypelst: Array<number>;
    groupByFilter: Array<number>;
    searchtypetext: string = "";
    searchtypeid: number;
    constructor() {
        super();
        this.factoryIds = [];
        this.factoryCountryIds = [];
        this.selectedCollectionIdList = [];
        this.selectedBuyerIdList = [];
        this.selectedBrandIdList = [];
        this.selectedDeptIdList = [];
        this.selectedProdCategoryIdList = [];
        this.selectedProductIdList = [];
        this.factoryIds=[];
        this.groupByFilter = [];
    }
}

export class DefectMasterModel {

    defectTabActive: boolean;
    supplierOrFactoryListCount: number;

    defectwordFrame: string;
    defectTitle: string;
    checkboxSelected: boolean;
    defectPerformanceAnalysis: DefectPerformanceAnalysis;
    hideDefectCritical: boolean;
    hideDefectMajor: boolean;
    hideDefectMinor: boolean;
    reportIdList: Array<number>;
    countryListName: string;
    supplierName: string;
    factoryName: string;
    customerName: string;
    filterCount: number;

    yearList: Array<DataSource>;
    searchLoading: boolean;


    customerList: Array<DataSource>;
    supplierList: Array<DataSource>;
    factoryList: Array<DataSource>;
    countryList: Array<DataSource>;
    defectList: Array<DataSource>;

    productCategoryList: any;
    productCategoryListLoading: boolean;

    customerInput: BehaviorSubject<string>;
    supplierInput: BehaviorSubject<string>;
    factoryInput: BehaviorSubject<string>;
    countryInput: BehaviorSubject<string>;
    defectInput: BehaviorSubject<string>;

    defectRequest: CommonDataSourceRequest;
    countryRequest: CountryDataSourceRequest;
    supplierRequest: CommonDataSourceRequest;
    factoryRequest: FactoryDataSourceRequest;

    filterDataShown: boolean;
    customerLoading: boolean;
    supplierLoading: boolean;
    factoryLoading: boolean;
    countryLoading: boolean;
    defectLoading: boolean;
    customerInfo: CustomerInfo;

    defectCategoryList: Array<DefectCategoryModel>;
    defectCategoryLoading: boolean;
    defectCategoryFound: boolean;
    defectCategoryError: boolean;
    defectCategoryDataLengthTrim: number;
    defectExportLoading: boolean;

    lineGraphDefectList: Array<DefectYearCountDataModel>;
    lineGraphChartDefectList: Array<DefectYearCountDataModel>;
    lineGraphDefectLoading: boolean;
    lineGraphDefectFound: boolean;
    lineGraphDefectError: boolean;
    lineGraphDefectExportLoading: boolean;
    lineGraphLoadingChart: boolean;
    defectYear: Array<DefectYear>;

    paretoExportLoading: boolean;
    paretoList: Array<ParetoModel>;
    paretoLoading: boolean;
    paretoFound: boolean;
    paretoError: boolean;

    lowPerformanceExportLoading: boolean;
    lowPerformanceList: Array<SupFactDefectAnalysis>;
    lowPerformanceLoading: boolean;
    lowPerformanceFound: boolean;
    lowPerformanceError: boolean;

    countryDefectExportLoading: boolean;
    countryDefectList: Array<CountryDefectModel>;
    countryDefectLoading: boolean;
    countryDefectFound: boolean;
    countryDefectError: boolean;
    countryListXAxis: Array<CountryModel>;

    defectNameList: any;
    defectCriticalId: number;
    defectMajorId: number;
    defectMinorId: number;

    defectCountList: Array<DefectCountList>;
    defectCountLoading: boolean;

    defectPhotoList: Array<DefectPhotoList>;
    defectPhotoLoading: boolean;

    defectOptions: NgxGalleryOptions[];
    defectImages: NgxGalleryImage[];

    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    collectionList: any;

    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    collectionSearchRequest: CommonCustomerSourceRequest;
    buyerSearchRequest: CommonCustomerSourceRequest;


    //For Searchable and scrollable product dropdown
    productList: any;
    productInput: BehaviorSubject<string>;
    productLoading: boolean;
    productRequest: ProductDataSourceRequest;
    serviceTypeLoading: boolean;
    serviceTypeList: any;
    groupByFilter: { name: string; id: number; }[];
    noFound: boolean;
    reportDefectGroupList: ReportDefectGroupData[];
    reportDefectList: ReportDefectData[];
    factoryCountryGroup: boolean;
    supplierGroup: boolean;
    factoryGroup: boolean;
    brandGroup: boolean;
    exportDataLoading: boolean;
    selectedNumberPlaceHolder: string;
    selectedNumber: string;
    isShowSearchLens: boolean;
    isShowColumn: boolean;
    isShowColumnImagePath: string;
    showColumnTooltip: string;
    constructor() {
        this.isShowColumn = true;
        this.customerInfo = new CustomerInfo();
        this.countryRequest = new CountryDataSourceRequest();
        this.supplierRequest = new CommonDataSourceRequest();
        this.factoryRequest = new FactoryDataSourceRequest();
        this.defectRequest = new CommonDataSourceRequest();
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();

        this.customerInput = new BehaviorSubject<string>("");
        this.supplierInput = new BehaviorSubject<string>("");
        this.factoryInput = new BehaviorSubject<string>("");
        this.countryInput = new BehaviorSubject<string>("");
        this.defectInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");

        this.customerList = new Array<DataSource>();
        this.supplierList = new Array<DataSource>();
        this.factoryList = new Array<DataSource>();
        this.countryList = new Array<DataSource>();

        this.defectCategoryList = new Array<DefectCategoryModel>();

        this.defectPerformanceAnalysis = new DefectPerformanceAnalysis();

        this.defectImages = [];

        this.defectNameList = DefectNameList;
        this.defectCriticalId = DefectCriticalId;
        this.defectMajorId = DefectMajorId;
        this.defectMinorId = DefectMinorId;

        this.defectCategoryDataLengthTrim = DefectDashboardDataLengthTrim;
        this.defectPerformanceAnalysis.innerPerformanceFilter.typeId = SupplierType.Factory;


        this.productInput = new BehaviorSubject<string>("");
        this.productRequest = new ProductDataSourceRequest();
    }
}

export class FactoryDataSourceRequest {
    searchText: string;
    skip: number;
    take: number;
    customerId: number;
    supplierId: number;
    supplierType: number;
    factoryId: number;
    id: number;
    locationId: number;
    idList: Array<number>;
    supSearchTypeId: number;
    customerglCodes: Array<string>;
    serviceId: number;
    constructor() {
        this.serviceId = APIService.Inspection;
        this.searchText = "";
        this.skip = 0;
        this.take = ListSize;
    }
}

export enum DefectDashboardResult {
    Success = 1,
    NotFound = 2,
    Error = 3,
    RequestNotCorrectFormat = 4
}

export class BookingReportModel {
    bookingId: number;
    reportId: number;
    factoryId: number;
}
export class BookingReportResponse {
    bookingReportModel: Array<BookingReportModel>;
    monthXAxis: Array<DefectYear>;
    result: DefectDashboardResult;
    constructor() {
        this.bookingReportModel = new Array<BookingReportModel>();
    }
}

export class DefectCategoryModel {
    name: string;
    categoryName: string;
    color: string;
    defectCountByCategory: number;
}

export class DefectCategoryResponse {
    defectCategoryList: Array<DefectCategoryModel>;
    result: DefectDashboardResult;
    constructor() {
        this.defectCategoryList = new Array<DefectCategoryModel>();
    }
}

//line graph defect model starts
export class DefectYearCountDataModel {
    defectName: string;
    defectYearCount: number;
    color: string;
    defectMonthList: Array<DefectMonth>;
}

export class DefectMonth {
    month: number;
    year: number;
    defectMonthCount: number;
    monthName: string;
    defectName: string;
}

export class DefectYearCountResponse {
    defectCountList: Array<DefectYearCountDataModel>;
    result: DefectDashboardResult;
}

export class DefectYearInnerCountResponse {
    defectCountList: Array<DefectYearCountDataModel>;
    result: DefectDashboardResult;
    monthXAxis: Array<DefectYear>;
}

export class DefectYear {
    year: number;
    month: number;
}

//line graph defect model ends
export class ParetoModel {
    color: string;
    defectName: string;
    defectCount: number;
    percentage: number;
    name: string;
}

export class ParetoDefectResponse {
    paretoList: Array<ParetoModel>;
    result: DefectDashboardResult;
}


export class DefectPerformanceFilter {
    defectName: string;
    // defectId: number;
    defectSelected: Array<number>;
    typeId: number;

    supOrFactId: number;
    defectSelect: number;

}

export class DefectPerformanceAnalysis {
    topPerformanceFilter: DefectDashboardModel;
    innerPerformanceFilter: DefectPerformanceFilter;
    constructor() {
        this.topPerformanceFilter = new DefectDashboardModel();
        this.innerPerformanceFilter = new DefectPerformanceFilter();
    }
}

export class DefectPerformanceResponse {
    result: DefectDashboardResult;
    performanceDefectList: Array<SupFactDefectAnalysis>;
}

export class SupFactDefectAnalysis {
    supOrFactName: string;
    supOrFactId: number;
    critical: number;
    major: number;
    minor: number;
    totalDefect: number;
    totalReports: number;
    defectReportInfo: Array<ReportDefectInfo>;
    defectCriticalIds: Array<number>;
    defectMajorIds: Array<number>;
    defectMinorIds: Array<number>;
    defectPhotoIds: Array<number>;
    constructor() {
        this.defectReportInfo = new Array<ReportDefectInfo>();
    }
}

export class DefectCountList {
    count: number;
    defectName: string;
}

export class ReportDefectInfo {
    ReportNo: string;
    ReportLink: string;
}

export class DefectPhotoList {
    defectPhotoPath: string;
    description: string;
}

export class CountryDefectModel {
    defectName: string;
    name: string;
    count: number;
    color: string;
    countryDefectData: Array<DefectCountModel>;
}

export class DefectCountModel {
    defectName: string;
    count: number;
    countryId: number;
}

export class CountryModel {
    countryId: number;
    countryName: string;
}

export class CountryDefectChartResponse {
    data: Array<CountryDefectModel>;
    countryList: Array<CountryModel>;
    result: DefectDashboardResult;
}

export class ReportDefectResponse {
    reportDefectList: ReportDefectData[];
    result: ResponseResult;
}

export class ReportDefectData {
    factoryCountryId: number;
    factoryCountryName: string;
    supplierId: number;
    supplierName: string;
    factoryId: number;
    factoryName: string;
    brandId: number;
    brandName: string;
    inspectionCount: number;
    reportCount: number;
    totalDefectCount: number;
    defects: ReportDefectGroupData[];
}

export class ReportDefectGroupData {
    defectName: string;
    critical: number;
    major: number;
    minor: number;
    defectCount: number;
}
