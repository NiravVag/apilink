import { BehaviorSubject } from "rxjs";

export class AuditDashboardModel {
    public searchtypeid: number;

    public searchtypetext: string = "";

    public customerid: number;

    public supplierid: number;

    public factoryidlst: any[] = [];

    public statusidlst: any[] = [];

    public datetypeid: number;

    public fromdate: any;

    public todate: any;

    public officeidlst: any[] = [];

    factoryCountryIdList: Array<number>;

    auditorIdList: Array<number>;

    serviceTypeIdList: Array<number>;
    mandayChartType: number;
    rejectChart: any[];
}

export class AuditDashboardMasterData {
    public supplierList: any;
    public countryList: any;
    public customerList: any;
    public factoryList: any;
    public officeList: any;
    supplierName: string;
    customerName: string;
    countryNameList: Array<string>;
    filterCount: number;
    filterDataShown: boolean;
    officeNameList: Array<string>;
    factoryNameList: Array<string>;

    countryInput: BehaviorSubject<string>;
    supInput: BehaviorSubject<string>;
    factInput: BehaviorSubject<string>;
    customerInput: BehaviorSubject<string>;

    countryLoading: boolean;
    supLoading: boolean;
    factLoading: boolean;
    customerLoading: boolean;
    officeLoading: boolean;

    mandayChartTypeList: any;
    auditDashboardCount: AuditDashboardItem;
    serviceTypeDashboard: any;
    overviewChart: any;
    auditTypeDashboard: any;
    downloadreport: boolean;
    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.factInput = new BehaviorSubject<string>("");
        this.customerInput = new BehaviorSubject<string>("");
        this.countryLoading = false;
        this.supLoading = false;
        this.factLoading = false;
        this.customerLoading = false;
        this.mandayChartTypeList = MandayChartYear;
        this.officeLoading = false;
    }
}

export class AuditDashboardMapFilterRequest {
    searchTypeId: number;
    searchTypeText: string;
    customerId: number;
    supplierId: number;
    factoryIdlst: any[] = [];
    statusIdlst: any[] = [];
    dateTypeid: number;
    fromDate: any;
    toDate: any;
    officeidlst: any[] = [];
    factoryCountryIdList: Array<number>;;
    auditorIdList: Array<number>;;
    serviceTypeIdList: Array<number>;
    constructor() {
        this.customerId = null;
        this.supplierId = null;
        this.factoryCountryIdList = [];
        this.factoryIdlst = [];
        this.officeidlst = [];
    }
}

export class AuditDashboardResponse {
    public data: AuditDashboardItem;
    public result: ResponseResult;
}

export class AuditDashboardItem {
    totalAuditInProgressCount: number;
    totalAuditPlanningCount: number;
    totalAuditedCount: number;
    totalFactoryCount: number;
    constructor() {
        this.totalAuditInProgressCount = 0;
        this.totalAuditPlanningCount = 0;
        this.totalAuditedCount = 0;
        this.totalFactoryCount = 0;
    }
}
export class AuditDashboardDataFound {
    auditDashboardCountDataFound: boolean;
    serviceTypeDashboardDataFound: boolean;
    overviewDataFound: boolean;
    auditTypeDashboardDataFound: boolean;

}

export class AuditDashboardLoader {
    public dashboardLoading: boolean = true;
    public overviewChartLoading: boolean = true;
    public rejectChartLoading: boolean = true;
    public prodCategoryChartLoading: boolean = true;
    public resultDashboardLoading: boolean = true;
    public serviceTypeDashboardLoading: boolean = true;
    public manDayDashboardLoading: boolean = true;
    public bookingAverageTimeDashboardLoading: boolean = true;
    public quotationAverageTimeDashboardLoading: boolean = true;
    public productCategoryExportLoading: boolean = true;
    public serviceTypeExportLoading: boolean = true;
    public resultExportLoading: boolean = true;

    overviewChartError: boolean;
    rejectChartError: boolean;
    prodCategoryChartError: boolean;
    resultDashboardError: boolean;
    serviceTypeDashboardError: boolean;
    manDayDashboardError: boolean;
    bookingAverageTimeDashboardError: boolean;
    quotationAverageTimeDashboardError: boolean;
    auditTypeDashboardLoading: boolean;
    auditTypeDashboardExportLoading: boolean;
    auditTypeDashboardError: boolean;
    auditTypeExportLoading: boolean;
}

export const MandayChartYear = [
    { name: "MTD", id: 1 },
    { name: "YTD", id: 2 }
];

export enum ResponseResult {
    Success = 1,
    NotFound = 2,
    Fail = 3
}
