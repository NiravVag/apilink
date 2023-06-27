import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest } from "../common/common.model";
import { MandayDashboard } from "../dashboard/managementdashboard.model";

export class FinanceDashboardModel {
    billedMandayData: MandayData;
    mandayRateData: MandayData;
    billedMandayTableData: TableModel;
    mandayRateTableData: TableModel;
    countryChartData: Array<PieChartModel>;
    prodCategoryChartData: Array<PieChartModel>;
    servicetypeChartData: Array<PieChartModel>;
    chargeBackChartData: any;
    quotationData: any;
    RatioAnalysisTableData: RatioTableModel;

    constructor(){
        this.billedMandayData = new MandayData();
        this.mandayRateData = new MandayData();
        this.billedMandayTableData = new TableModel();
        this.mandayRateTableData = new TableModel();
        this.countryChartData = new Array<PieChartModel>();
        this.prodCategoryChartData = new Array<PieChartModel>();
        this.servicetypeChartData = new Array<PieChartModel>();
        this.RatioAnalysisTableData = new RatioTableModel();
    }
}

export class FinanceDashboardLoader{
    billedMandayChartLoading: boolean;
    billedMandayDataFound: boolean;
    billedMandayChartError: boolean;

    mandayRateChartLoading: boolean;
    mandayRateDataFound: boolean;
    mandayRateChartError: boolean;

    countryChartLoading: boolean;
    countryDataFound: boolean;
    countryChartError: boolean;
    countryChartExportLoading: boolean;

    prodCategoryChartLoading: boolean;
    prodCategoryDataFound: boolean;
    prodCategoryChartError: boolean;
    prodCategoryChartExportLoading: boolean;

    servicetypeChartLoading: boolean;
    servicetypeDataFound: boolean;
    servicetypeChartError: boolean;
    servicetypeChartExportLoading: boolean;

    chargeBackChartLoading: boolean;
    chargeBackDataFound: boolean;
    chargeBackChartError: boolean;

    quotationchartLoading: boolean;
    quotationDataFound: boolean;
    quotationChartError: boolean;

    tableTabActive: boolean;
    mandayrateTabActive:boolean;
    billedmandayTabActive:boolean;
    ratioTabActive:boolean;
    tableExportLoading: boolean;

    constructor(){
        this.billedMandayChartLoading = true;
        this.mandayRateChartLoading = true;
        this.countryChartLoading = true;
        this.prodCategoryChartLoading = true;
        this.servicetypeChartLoading = true;
        this.chargeBackChartLoading = true;
        this.tableTabActive = true;
        this.mandayrateTabActive= true;
        this.billedmandayTabActive=false;
        this.ratioTabActive=false;
        this.quotationchartLoading = true;
        this.tableExportLoading = true;

        this.billedMandayDataFound = false;
        this.billedMandayChartError = false;
        this.mandayRateChartError = false;
        this.mandayRateDataFound = false;
        this.countryChartError = false;
        this.countryDataFound = false;
        this.prodCategoryChartError = false;
        this.prodCategoryDataFound = false;
        this.servicetypeDataFound = false;
        this.servicetypeChartError = false;
        this.chargeBackDataFound = false;
        this.chargeBackChartError = false;
        this.quotationDataFound = false;
        this.quotationChartError = false;
    }
}

export class FinanceDashboardRequestModel{
    serviceDateFrom: any;
    serviceDateTo: any;
    customerId: number;
    supplierId: number;
    countryIdList: Array<number>;
    factoryIdList: Array<number>;
    brandIdList: Array<number>;
    deptIdList: Array<number>;
    buyerIdList: Array<number>;
    serviceTypeList: Array<number>;
    officeIdList: Array<number>
    isBilledMandayExport: boolean;
    ratioCustomerIdList:  Array<number>;
    ratioEmployeeTypeIdList: Array<number>;

    constructor () {
        this.customerId = null;
        this.supplierId = null;
        this.countryIdList = [];
        this.officeIdList = [];
        this.serviceTypeList = [];
        this.ratioCustomerIdList = [];
    }
}

export class FinanceDashboardMandayData{
    month: number;
    monthName: string;
    year: number;
    manday: number;
    inspFees: number;
    rate: number;
}

export class MandayData{
    data: Array<MandayYearChart>;;
    budget: MandayYearChart;
}

export class MandayYearChart {
    year: number;
    mandayCount: number;
    color: string;
    monthlyData: Array<MandayYearChartItem>;
}

export class MandayYearChartItem {
    month: number;
    monthManDay: number;
}

export class TableModel{
    actual: Array<number>;//Array<MandayYearChartItem>;
    lastYear: Array<number>;//Array<MandayYearChartItem>;
    budget: Array<number>;
    isActualLessThanBudget: boolean;

    constructor(){
        this.actual = new Array<number>();
        this.lastYear = new Array<number>();
        this.budget = new Array<number>();
    }
}
export class RatioTableModel{
        customerId:number;
        customer:string;
        billedManday:number;
        productionManday :number;
        ratio :number;
        revenue:number;
        billedAvgManday :number;
        productionAvgManday :number;
        isTotal:boolean;
        chargeBack: number;
        totalExpense: number;
        netIncome: number;
        netMDRate: number;
}

export class PieChartModel{
    Name : string;
    Count : number;
    Color: string;
}

export class FinanceDashboardFilterMaster {
    public supplierList: any;
    public countryList: any;
    public customerList: any;
    public ratioCustomerList: any;
    public factoryList: any;
    public officeList: any;
   
    filterCount: number;
    filterDataShown: boolean;
    
    countryInput: BehaviorSubject<string>;
    supInput: BehaviorSubject<string>;
    factInput: BehaviorSubject<string>;
    customerInput: BehaviorSubject<string>;
    ratioCustomerInput: BehaviorSubject<string>;


    countryLoading: boolean;
    supLoading: boolean;
    factLoading: boolean;
    customerLoading: boolean;
    ratioCustomerLoading: boolean;
    officeLoading: boolean;
    
    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    buyerSearchRequest: CommonCustomerSourceRequest;
    countryRequest: CountryDataSourceRequest;
    supsearchRequest: CommonDataSourceRequest;
    requestFactModel:CommonDataSourceRequest;
    requestCustomerModel:CommonDataSourceRequest;
    requestInnerRatioCustomerModel:CommonDataSourceRequest;


    serviceTypeList: any;
    serviceTypeLoading: boolean;

    searchLoading: boolean;

    supplierName: string;
    customerName: string;
    officeNameList: Array<string>;
    factoryNameList: Array<string>;
    countryNameList: Array<string>;
    serviceTypeNameList: Array<string>;
    brandNameList: Array<string>;
    buyerNameList: Array<string>;
    deptNameList: Array<string>;

    isRatioCustomerVisible: boolean = true;
    employeeTypeLoading: boolean = true;
    employeeTypeList: any;
    tableLoading: boolean;
    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.factInput = new BehaviorSubject<string>("");
        this.customerInput = new BehaviorSubject<string>("");
        this.ratioCustomerInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.countryLoading = false;
        this.supLoading = false;
        this.factLoading = false;
        this.customerLoading = false;
        this.officeLoading = false;

        this.supsearchRequest = new CommonDataSourceRequest();
        this.requestFactModel = new CommonDataSourceRequest();
        this.countryRequest = new CountryDataSourceRequest();
        this.requestCustomerModel = new CommonDataSourceRequest();
        this.requestInnerRatioCustomerModel = new CommonDataSourceRequest();
        this.brandSearchRequest =  new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();;
    }
}
