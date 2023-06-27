import { NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ProductDataSourceRequest } from "../common/common.model";
import * as am4core from "@amcharts/amcharts4/core";

export class QuantitativeDashboardModel {
    public quantitativeDashboardCount: QuantitativeDashboardCountResponse;
    public mandayDashboard: MandayDashboard;
    public mandayByCountryDashboard: Array<QuantitativeDashboardCommonItem>;
    public turnOverDashboard: TurnOverData;
    public serviceTypeDashboard: Array<QuantitativeDashboardCommonItem>;
    public inspectionServiceTypeDashboard: Array<ProductCategoryDashboard>;
    public bookingQuantityDashboard: Array<OrderQuantityYearChart>;

    searchLoading: boolean;
    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    collectionSearchRequest: CommonCustomerSourceRequest;
    buyerSearchRequest: CommonCustomerSourceRequest;
    supsearchRequest: CommonDataSourceRequest;
    customerInput: BehaviorSubject<string>;
     supplierList: any;
     countryList: any;
     customerList: any;
     customerLoading: boolean;
    
     supplierName: string;
    customerName: string;
    countryListName: string;
    filterCount: number;
    filterDataShown: boolean;

    countryInput: BehaviorSubject<string>;
    supInput: BehaviorSubject<string>;

    countryLoading: boolean;
    supLoading: boolean;

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
	
    countryRequest: CountryDataSourceRequest;
    
    orderQuantityXAxis: Array<OrderQuantityXAxisModel>;
    prodCategoryChart: Array<ProductCategoryDashboard>;

    productCategoryList: any;
    productCategoryListLoading: boolean;


     //For Searchable and scrollable product dropdown
     productList: any;
     productInput: BehaviorSubject<string>;
     productLoading: boolean;
     productRequest: ProductDataSourceRequest;
     supplierTypeId: number;
     
    constructor() {
        this.quantitativeDashboardCount = new QuantitativeDashboardCountResponse();
        this.mandayDashboard = new MandayDashboard();
        this.mandayByCountryDashboard = new Array<QuantitativeDashboardCommonItem>();
        this.turnOverDashboard = new TurnOverData();
        this.serviceTypeDashboard = new Array<QuantitativeDashboardCommonItem>();
        this.inspectionServiceTypeDashboard = new Array<ProductCategoryDashboard>();
        this.bookingQuantityDashboard = new Array<OrderQuantityYearChart>();

        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");
        this.customerInput=new BehaviorSubject<string>("");

        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.supsearchRequest = new CommonDataSourceRequest();
        this.countryRequest = new CountryDataSourceRequest();

        this.orderQuantityXAxis = new  Array<OrderQuantityXAxisModel>();
        this.prodCategoryChart = new Array<ProductCategoryDashboard>();

        this.productInput = new BehaviorSubject<string>("");
        this.productRequest = new ProductDataSourceRequest();
    }
}

export class QuantitativeDashboardCountResponse {
    public totalManday: number;
    public totalFactoryCount: number;
    public totalInspCount: number;
    public totalReportCount: number;
    public totalQcCount: number;
    public totalVendorCount: number;
}
export class ProductCategoryDashboard {
    public id: number;
    public statusName: string;
    public totalCount: number;
    public statusColor: am4core.Color;
    public name: string;
}
export class QuantitativeDashBoardLoader
{
    public dashboardLoading:boolean = true;
    public manDayDashboardLoading:boolean = true;
    public mandayExportLoading = true;
    public mandayCountryDashboardLoading = true;
    public manDayCountryChartExportLoading = true;
    public turnOverDashboardLoading = true;
    public serviceTypeTurnOverDashboardLoading = true;
    public serviceTypeTurnOverDashboardExportLoading;
    public serviceTypeInspectionDashboardLoading = true;
    public serviceTypeInspectionDashboardExportLoading = true;

    manDayCountryChartError: boolean;
    manDayDashboardError: boolean;
    turnOverDashboardError: boolean;
    serviceTypeTurnOverDashboardError: boolean;
    serviceTypeInspectionDashboardError: boolean;
    bookingQuantityDashboardError: boolean;

    orderQuantityExportLoading: boolean;
    orderQuantityLoading: boolean;
    orderQuantityError: boolean;

    productCategoryExportLoading: boolean;
    productCategoryLoading: boolean;
    productCategoryError: boolean;
}

export class QuantitativeDashBoardDataFound
{
    public quantitativeDashboardCountDataFound:boolean;
    public manDayDashboardDataFound:boolean;
    public manDayCountryDashboardDataFound:boolean;
    public turnOverDashboardDataFound:boolean;
    public serviceTypeTurnOverDashboardDataFound:boolean;
    public serviceTypeInspectionDashboardDataFound:boolean;
    public bookingQuantityDashboardDataFound:boolean;
    orderQuantityFound: boolean;
    productCategoryFound: boolean;
}

export class QuantitativeDashboardRequest {
    customerId: number;
    supplierId: number;
    selectedCountryIdList: any;
    serviceDateFrom: any;
    serviceDateTo: any;
    selectedBrandIdList: Array<number>;
    selectedBuyerIdList: Array<number>;
    selectedCollectionIdList: Array<number>;
    selectedDeptIdList: Array<number>;
    productCategoryId: number;
    selectedProdCategoryIdList: Array<number>;
    selectedProductIdList: Array<number>;
    constructor() {
        this.selectedCountryIdList =[];
        this.selectedCollectionIdList = [];
        this.selectedBuyerIdList = [];
        this.selectedBrandIdList = [];
        this.selectedDeptIdList = [];
        this.selectedProdCategoryIdList=[];
        this.selectedProductIdList = [];
    }
}

export class QuantitativeDashboardSummaryResponse {
    public data: QuantitativeDashboardCountResponse;
    public result: ResponseResult;
    public inspectionIdList: Array<number>;
}

export enum ResponseResult {
    Success = 1,
    NotFound = 2,
    Fail = 3
}

export class MandayDashboard {
    mandayData: Array<MandayYearChart>;
    result: ResponseResult;
}

export class MandayYearChart {
    year: number;
    mandayCount: number;
    color: string;
    percentage: number;
    percentagePositive: number;
    monthlyData: Array<MandayYearChartItem>;
}

export class MandayYear {
    year: number;
    month: number;
    monthName: string;
}

export class MandayYearChartItem {
    month: number;
    monthManDay: number;
}

export class QuantitativeDashboardCommonItem
{
    name: string;
    trimmedName: string;
    count: number;
    color: am4core.Color;
    percentage: number;
    percentagePositive: number;
}

export class TurnOverData {
    totalTurnOver: number;
    customerTurnOver: number;
    supplierTurnOver: number;
    totalTurnOverPercentage: number;
    customerTurnOverPercentage: number;
    supplierTurnOverPercentage: number;
    totalTurnOverPercentagePositive: number;
    customerTurnOverPercentagePositive: number;
    supplierTurnOverPercentagePositive: number;
}

export class OrderQuantityYearChart
{
    year: number;
    orderCount: number;
    color: string;
    monthlyData: Array<OrderQtyChartItem>;
}
export class OrderQtyChartItem{
    month: number;
    monthOrderQuantity: number;
}

export class OrderQuantityXAxisModel {
    year: number;
    month: number;
    monthName: string;
}

export class ProductCategoryRequest {
    searchRequest: QuantitativeDashboardRequest;
    productCategoryId: number;
}

export class DashboardServiceDateList {
    dateName: string;
    date: NgbDate;
}