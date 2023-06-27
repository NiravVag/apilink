import { NgxGalleryImage, NgxGalleryOptions } from "ngx-gallery-9";
import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource, ProductDataSourceRequest, SupplierCommonDataSourceRequest } from "../common/common.model";
import { ResponseResult } from "../common/common.model";
import { MandayYear } from "../dashboard/managementdashboard.model";
import { summaryModel } from "../summary.model";
import { FactoryDataSourceRequest } from "./defect-dashboard.model";

export class RejectionDashboardModel {
    public apiResultDashboard: ResultDashboardResponse;
    public customerResultDashboard: ResultDashboardResponse;
    public productCategoryDashboard: ProductCategoryChart;
    public vendorDashboard: VendorChart;
    public countryDashboard: VendorChart;
    public rejectDashboard: RejectChartResponse;
    public rejectDashboardPopUpData: RejectionPopUpData;

    supplierList: any;
    customerList: any;
    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;

    supplierName: string;
    customerName: string;
    filterCount: number;
    filterDataShown: boolean;

    innerCountryList: any;
    innerCountryInput: BehaviorSubject<string>;
    innerCountryLoading: boolean;
    innerCountryRequest: CountryDataSourceRequest;

    countryList: any;
    countryListName: string;
    countryInput: BehaviorSubject<string>;
    countryLoading: boolean;
    countryRequest: CountryDataSourceRequest;

    supInput: BehaviorSubject<string>;
    supLoading: boolean;

    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    productCategoryList: any;
    productCategoryListLoading: boolean;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    collectionList: any;

    searchLoading: boolean;
    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    collectionSearchRequest: CommonCustomerSourceRequest;
    buyerSearchRequest: CommonCustomerSourceRequest;
    supsearchRequest: CommonDataSourceRequest;

    fbReportResultId:number;
    selectedReasonList:any;
    fbReportResultReasonList:any;
    selectedSubCatogoryList:any;
    fbReportResultSubCatogoryList:any;

    lblfbReportHead:string;
    lblfbReportReason:string;
    lblfbReportCount:string;

    supplierTypeId: number;
    //For Searchable and scrollable product dropdown
    productList: any;
    productInput: BehaviorSubject<string>;
    productLoading: boolean;
    productRequest: ProductDataSourceRequest;
    factoryId:number;
    factoryIds: Array<number>;
    factoryList: any;
    factoryRequest: FactoryDataSourceRequest;
    factoryInput: BehaviorSubject<string>;
    factoryLoading: boolean;
    serviceTypeLoading: boolean;
    serviceTypeList: any;
    noFound: boolean;
    groupByFilter: { name: string; id: number; }[];
    exportDataLoading: boolean;
    selectedNumberPlaceHolder: string;
    selectedNumber: string;
    isShowSearchLens: boolean;
    isShowColumn: boolean;
    isShowColumnImagePath: string;
    showColumnTooltip: string;

    constructor() {
        this.isShowColumn = true;
        this.apiResultDashboard = new ResultDashboardResponse();
        this.customerResultDashboard = new ResultDashboardResponse();
        this.productCategoryDashboard = new ProductCategoryChart();
        this.vendorDashboard = new VendorChart();
        this.countryDashboard = new VendorChart();
        this.rejectDashboard = new RejectChartResponse();

        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");
        this.innerCountryInput = new BehaviorSubject<string>("");

        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.supsearchRequest = new CommonDataSourceRequest();
        this.countryRequest = new CountryDataSourceRequest();
        this.innerCountryRequest = new CountryDataSourceRequest();
        this.customerInput = new BehaviorSubject<string>("");


        this.productInput = new BehaviorSubject<string>("");
        this.productRequest = new ProductDataSourceRequest();
        this.factoryInput = new BehaviorSubject<string>("");
        this.factoryRequest = new FactoryDataSourceRequest();
    }
}

export class ResultDashboardResponse {
    public dashboardData: Array<ResultDashboard>;
    public totalReports: number;
}

export class ResultDashboard {
    public id: number;
    public statusName: string;
    public totalCount: number;
    public statusColor: string;
    public name: string;
}


export class RejectionDashBoardLoader {
    public apiResultDashboardLoading: boolean = true;
    public apiResultDashboardError: boolean = false;
    public apiResultDashboardExportLoading: boolean = false;

    public customerResultDashboardLoading: boolean = true;
    public customerResultDashboardError: boolean = false;
    public customerResultDashboardExportLoading: boolean = false;

    public productCategoryDashboardLoading: boolean = true;
    public productCategoryDashboardError: boolean = false;
    public productCategoryDashboardExportLoading: boolean = false;

    public vendorDashboardLoading: boolean = true;
    public vendorDashboardError: boolean = false;
    public vendorDashboardExportLoading: boolean = false;

    public countryDashboardLoading: boolean = true;
    public countryDashboardError: boolean = false;
    public countryDashboardExportLoading: boolean = false;

    public rejectDashboardLoading: boolean = true;
    public rejectDashboardError: boolean = false;
    public rejectDashboardExportLoading: boolean = false;

    public rejectDashboardPopUpLoading: boolean = true;
    public rejectDashboardPopUpError: boolean = false;

    public rejectDashboardTableExportLoading: boolean = false;
}

export class RejectionDashBoardDataFound {
    public apiResultDashboardDataFound: boolean = false;
    public customerResultDashboardDataFound: boolean = false;
    public productCategoryDashboardDataFound: boolean = false;
    public vendorDashboardDataFound: boolean = false;
    public countryDashboardDataFound: boolean = false;
    public rejectDashboardDataFound: boolean = false;
    public rejectDashboardPopUpDataFound: boolean = false;
}

export class RejectionDashboardRequest extends summaryModel{
    public customerId: number;
    public supplierId: number;
    public selectedCountryIdList: any;
    public serviceDateFrom: any;
    public serviceDateTo: any;
    public selectedBrandIdList: Array<number>;
    public selectedBuyerIdList: Array<number>;
    public selectedCollectionIdList: Array<number>;
    public selectedDeptIdList: Array<number>;
    public countryId: number;

    public month: number;
    public year: number;
    public rejectReason: string;

    public popUpSupplierId: number;
    public popUpFactoryId: number;

    public popUpSelectedPhotoSupplierId:number;
    public popUpSelectedPhotoFactoryId:number;

    public searchBy: string;
    public FbResultId: number;
    public SummaryNames: Array<String>;
    public SubcatogoryList: Array<String>;

    public selectedProdCategoryIdList: Array<number>;
    public selectedProductIdList: Array<number>;
    public selectedFactoryIdList: Array<number>;
    public selectedSupplierIdList: Array<number>;
    public selectedServiceTypeIdList: Array<number>;
    groupByFilter: Array<number>;
    isExport: boolean;
    searchtypetext: string = "";
    searchtypeid: number;
    factoryId: number;
    supplierTypeId: number;
    constructor() {
        super();
        this.selectedCountryIdList = [];
        this.selectedCollectionIdList = [];
        this.selectedBuyerIdList = [];
        this.selectedBrandIdList = [];
        this.selectedDeptIdList = [];
        this.SummaryNames = [];
        this.SubcatogoryList = [];
        this.selectedProdCategoryIdList=[];
        this.selectedProductIdList=[];
        this.selectedFactoryIdList = [];
        this.selectedSupplierIdList = [];
        this.selectedServiceTypeIdList = [];
        this.groupByFilter = [];
    }
}

export class RejectionDashboardSummaryResponse {
    public data: Array<ResultDashboard>;
    public totalReports: number;
    public result: ResponseResult;
    public bookingIdList: Array<number>;
}

export class ProductCategoryChart {
    public data: Array<ChartData>;
    public xAxis: Array<DataSource>;
    public maxYAxisValue: number;
    public totalReports: number;
    public legendList: Array<DataSource>;
}

export class ChartData {
    public resultName: string;
    public color: string;
    public count: number;
    public data: Array<ChartItem>;
}

export class ChartItem {
    public resultId: number;
    public resultName: string;
    public totalCount: number;
    public id: number;
    public name: string;
    public percentage: number;
}

export class VendorChart {
    public data: Array<ChartData>;
    public yAxis: Array<DataSource>;
    public maxXAxisValue: number;
    public totalReports: number;
}

export class CountryChartRequest {
    public searchRequest: RejectionDashboardRequest;
    public countryId: number;
    public clearSelection: boolean;
}

export class RejectChartMonthItem {
    public name: string;
    public monthName: string;
    public year: number;
    public monthCount: number;
    public totalCount: number;
}

export class RejectChartYearData {
    public reasonName: string;
    public subcatogory: string;
    public name: string;
    public count: number;
    public percentage: number;
    public monthlyData: Array<RejectChartMonthItem>;
}
export class RejectChartRequest {
    public searchRequest: RejectionDashboardRequest;
    public fbResultId: number;
}
export class RejectChartSubcatogoryRequest {
    public searchRequest: RejectionDashboardRequest;
    public fbResultId: number;
    public ResultNames: Array<string>;
    constructor() {
        this.ResultNames = [];
    }
}
export class RejectChartSubcatogory2Request {
    public searchRequest: RejectionDashboardRequest;
    public fbResultId: number;
    public ResultNames: Array<string>;
    public SubCatogory: Array<string>;

    constructor() {
        this.ResultNames = [];
        this.SubCatogory = [];
    }
}

export class RejectChartResponse {
    public data: Array<RejectChartYearData>;
    public rejectReasonList: Array<string>;
    public monthNameList: Array<MandayYear>;
}

export class RejectionPopUpData {
    public reportCount: number;
    public inspectionCount: number;
    public factoryCount: number;
    public supplierData: Array<RejectionFactoryData>;
    public reportInfo: Array<RejectionReportData>;
    constructor(){
        this.supplierData = [];
    }
}

export class RejectionReportData{
    public reportNo:string;
    public reportLink:string;
}

export class RejectionFactoryData {
    public supplierId: number;
    public supplierName: string;
    public factoryId: number;
    public factoryName: string;
    public rejectionCount: number
    public reportCount: number;
    public photoCount: number;
}

export class PopUpListModel {

    supPopUpInput: BehaviorSubject<string>;
    supPopUpLoading: boolean;
    supplierPopUpList: any;
    supPopUpsearchRequest: CommonDataSourceRequest;

    factPopUpInput: BehaviorSubject<string>;
    factPopUpLoading: boolean;
    factoryPopUpList: any;
    factPopUpsearchRequest: CommonDataSourceRequest;

    imageLoading: boolean;
    imageList: any;

    rejectionImages: NgxGalleryImage[];
    rejectOptions: NgxGalleryOptions[];

    constructor() {
        this.supPopUpInput = new BehaviorSubject<string>("");
        this.supPopUpsearchRequest = new CommonDataSourceRequest();

        this.factPopUpInput = new BehaviorSubject<string>("");
        this.factPopUpsearchRequest = new CommonDataSourceRequest();
    }
}

export class RejectionRateResponse {
    public resultDataList: RejectionRateList;
    public rejectionRateGroupList: RejectionRateGroupList[];
    public reportResultNameList: ReportResultNameList[];
    public reportDecisionNameList: ReportDecisionNameList[];
}

export class RejectionRateList {
    public rejectionRateReportResultLists: RejectionRateReportResultList[];
    public rejectionRateDecisionLists: RejectionRateDecisionList[];
}

export class RejectionRateReportResultList {
    factoryCountryId: number;
    factoryCountryName: string;
    supplierId: number;
    supplierName: string;
    factoryId: number;
    factoryName: string;
    brandId: number;
    brandName: string;
    inspectionCount: number;
    presentedQty: number;
    inspectedQty: number;
    orderQty: number;
    resultId: number;
    resultName: string;
    totalCount: number;
}

export class RejectionRateDecisionList {
    factoryCountryId: number;
    factoryCountryName: string;
    supplierId: number;
    supplierName: string;
    factoryId: number;
    factoryName: string;
    brandId: number;
    brandName: string;
    totalDecisionCount: number;
    customerResultId: number;
    customDecisionName: string;
}

export class RejectionRateGroupList {
    factoryCountryId: number;
    factoryCountryName: string;
    supplierId: number;
    supplierName: string;
    factoryId: number;
    factoryName: string;
    brandId: number;
    brandName: string;
    inspectionCount: number;
    presentedQty: number;
    inspectedQty: number;
    orderQty: number;
    totalCount: number;
}

export class ReportResultNameList {
    id: string;
    name: string;
}

export class ReportDecisionNameList {
    id: number;
    name: string;
    customName: string;
    customerId: number;
    isDefault: true;
    cusDecId: number;
}

export class RejectionRateSearchRequest {
    rejectionDashboardSearchRequest: RejectionDashboardRequest;
    groupByFilter: GroupByFilterEnum[];
}

export enum GroupByFilterEnum {
    FactoryCountry = 1,
    Supplier = 2,
    Factory = 3,
    Brand = 4
}
