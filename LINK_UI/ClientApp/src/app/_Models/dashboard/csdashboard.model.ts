import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest } from "../common/common.model";

export class CSDashboardRequest {
    customerId: number;
    supplierId: number;
    factoryIdList: Array<number>;
    factoryCountryIdList: Array<number>;
    officeIdList: Array<number>;    
    serviceDateFrom: any;
    serviceDateTo: any;
    countryIdList: Array<number>;
    brandIdList: Array<number>;
    deptIdList: Array<number>;
    buyerIdList: Array<number>;
    serviceTypeList: Array<number>;

    constructor() {
        this.factoryCountryIdList = [];
        this.customerId = null;
        this.supplierId = null;
        this.factoryIdList = [];
        this.officeIdList = [];
        this.countryIdList = [];
        this.serviceTypeList = [];
    }
}

export enum CSDashboardResult {
    Success = 1,
    CannotGetList = 2,
    Failed = 3,
    RequestNotCorrectFormat = 4
}

export class CSDashboardItem {
    name: string;
    count: number;
    color: string;
    trimmedName: string;
}

export class CSDashboardMasterModel {
   
    newCountDashboardLoading: boolean;
    newCountDashboardError: boolean;
    newCountDashboardDataFound: boolean;

    serviceTypeGraphLoading: boolean;
    serviceTypeGraphError: boolean;
    serviceTypeGraphFound: boolean;

    mandayByOfficeLoading: boolean;
    mandayByOfficeError: boolean;
    mandayByOfficeFound: boolean;

    reportCountLoading: boolean;
    reportCountError: boolean;
    reportCountFound: boolean;

    statusCountError: boolean;
    statusCountLoading: boolean;

    statusCountFound: boolean;
    // QuotationCountFound: boolean;
    // allocationCountFound: boolean;
    // fbReportCountFound: boolean;

    newCountDashboardItem: CSDashboardCountItem;
    serviceTypeGraphList: Array<CSDashboardItem>;
    mandayByOfficeList: Array<CSDashboardItem>;
    reportCountDayList : Array<ReportCountItem>;
    
    bookingStatusList: Array<StatusTaskCountItem>;
    quotationStatusList: Array<StatusTaskCountItem>;
    allocationStatusList: Array<StatusTaskCountItem>;
    reportStatusList: Array<StatusTaskCountItem>;
    
    quotationStatusCopyList: Array<StatusTaskCountItem>;

    bookingTaskList: Array<TaskCountItem>;
    quotationTaskList: Array<TaskCountItem>;
    allocationTaskList: Array<TaskCountItem>;
    reportTaskList:  Array<TaskCountItem>;

    bookingCardState: number;
    quotationCardState: number;
    allocationCardState: number;
    reportCardState: number;

    public supplierList: any;
    public countryList: any;
    public customerList: any;
    public factoryList: any;
    public officeList: any;
   
    filterCount: number;
    filterDataShown: boolean;
    
    countryInput: BehaviorSubject<string>;
    supInput: BehaviorSubject<string>;
    factInput: BehaviorSubject<string>;
    customerInput: BehaviorSubject<string>;

    countryLoading: boolean;
    supLoading: boolean;
    factLoading: boolean;
    customerLoading: boolean;
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


    constructor() {
        this.newCountDashboardItem = new CSDashboardCountItem();
        this.serviceTypeList = new Array<CSDashboardItem>();
        this.mandayByOfficeList = new Array<CSDashboardItem>();
        this.reportCountDayList =  Array<ReportCountItem>();
        
        this.bookingStatusList = new Array<StatusTaskCountItem>();
        this.allocationStatusList = new Array<StatusTaskCountItem>();
        this.reportStatusList = new Array<StatusTaskCountItem>();

        this.bookingTaskList = new Array<TaskCountItem>();
        this.allocationTaskList = new Array<StatusTaskCountItem>();
        this.reportTaskList = new Array<StatusTaskCountItem>();

        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.factInput = new BehaviorSubject<string>("");
        this.customerInput = new BehaviorSubject<string>("");
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
        this.brandSearchRequest =  new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();;
    }
}

export class CSDashboardCountItem
{
    customerCount : number;
    supplierCount : number;
    factoryCount : number;
    bookingCount : number;
    poCount : number;
    productCount : number;
}

export class ReportCountItem {
    count: number;
    date: string;
}

export class ReportCountXAxis {
    year: number;
    month: number;
    monthName: string;
}

// export class ReportCountItem {
//     year: number;
//     reportCount: number;
//     color: string;
//     monthlyData: Array<ReportCountMonthItem>;
// }

export class ReportCountMonthItem {
    month: number;
    monthManDay: number;
}

export class StatusTaskCountItem {
    statusCount: number;
    taskCount: number;
    statusName: string;
    taskLink: string;    
    trimName: string;
     taskName: string; 
}

export class TaskCountItem {
    taskCount: number;
    statusName: string;
    taskLink: string;   
    taskName: string; 
}

export class TaskStatusList {
    taskList: Array<TaskCountItem>;
    statusList: Array<StatusTaskCountItem>;
    cardState: number;
}

export enum TaskCardState {
    noTask = 1,
    OneTask = 2,
    TwoTask = 3
}
