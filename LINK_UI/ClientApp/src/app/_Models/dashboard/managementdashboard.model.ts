import { BehaviorSubject } from "rxjs";
import { APIService } from "src/app/components/common/static-data-common";


export class ManagementDashboardModel {
    public managementDashboardCount: ManagementDashboardCountResponse;
    public overviewChart: OverviewChartResponse;
    public rejectChart: Array<InspectionRejectDashboard>;
    public prodCategoryChart: Array<ProductCategoryDashboard>;
    public resultDashboard: Array<ProductCategoryDashboard>;
    public serviceTypeDashboard: Array<ProductCategoryDashboard>;
    public mandayDashboard: MandayDashboard;
    public bookingAverageTimeDashboard: AverageBookingTimeItem;
    public quotationAverageTimeDashboard: AverageQuotationTimeItem;


    constructor() {
        this.managementDashboardCount = new ManagementDashboardCountResponse();
        this.overviewChart = new OverviewChartResponse();
        this.rejectChart = new Array<InspectionRejectDashboard>();
        this.prodCategoryChart = new Array<ProductCategoryDashboard>();
        this.resultDashboard = new Array<ProductCategoryDashboard>();
        this.serviceTypeDashboard = new Array<ProductCategoryDashboard>();
        this.mandayDashboard = new MandayDashboard();
        this.bookingAverageTimeDashboard = new AverageBookingTimeItem();
        this.quotationAverageTimeDashboard = new AverageQuotationTimeItem();
    }
}

export class ManagementDashBoardLoader {
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
}

export class ManagementDashBoardDataFound {
    public managementDashboardCountDataFound: boolean;
    public overviewDataFound: boolean;
    public rejectDataFound: boolean;
    public prodCategoryDataFound: boolean;
    public supplierPerformanceDataFound: boolean;
    public productOverviewDataFound: boolean;
    public resultDashboardDataFound: boolean;
    public serviceTypeDashboardDataFound: boolean;
    public manDayDashboardDataFound: boolean
    public averageBookingTimeDashboardDataFound: boolean
    public averageQuotationTimeDashboardDataFound: boolean
}

export class ManagementDashboardRequest {
    public customerId: number;
    public supplierId: number;
    public serviceDateFrom: any;
    public serviceDateTo: any;
    public factoryIdList: Array<number>;
    public statusIdList: Array<number>;
    public countryIdList: Array<number>;
    public selectedSupplierIdList: Array<number>;
    public mandayChartType: number;
    public officeIdList: Array<number>;

    constructor() {
        this.mandayChartType = 1;
        this.countryIdList = [];
        this.customerId = null;
        this.supplierId = null;
        this.factoryIdList = [];
        this.officeIdList = [];
    }
}

export enum ResponseResult {
    Success = 1,
    NotFound = 2,
    Fail = 3
}

export class ManagementDashboardCountResponse {
    public totalManday: number;
    public totalFactoryCount: number;
    public totalInspCount: number;
    public totalProductCount: number;
    public totalMandayLastYear: number;
    public totalInspCountLastYear: number;
    public totalProductCountLastYear: number;
    public totalFactoryCountLastYear: number;
    public factoryDifferencePercentage: number;
    public inspectionDifferencePercentage: number;
    public productDifferencePercentage: number;
    public mandayDifferencePercentage: number;
    public totalCustomerCount: number;
    public totalCustomerCountLastYear: number;
    public customerDifferencePercentage: number;
    public totalReportCount: number;
    public totalReportCountLastYear: number;
    public reportDifferencePercentage: number;

}

export class OverviewChartResponse {
    public totalReports: number;
    public totalBookingCount: number;
    public totalCustomerCount: number;
    public claimRate: any;
    public quotationRejectedByCustomerCount: number;
}

export class ManagementDashboardSummaryResponse {
    public data: ManagementDashboardCountResponse;
    public result: ResponseResult;
    public inspectionIdList: Array<number>;
    public officeIds: Array<number>;
}

export class InspectionRejectDashboard {
    public statusName: string;
    public statusColor: string;
    public totalCount: number;
}

export class ProductCategoryDashboard {
    public id: number;
    public statusName: string;
    public totalCount: number;
    public statusColor: string;
    public name: string;
}

export class MandayDashboard {
    public mandayData: Array<MandayYearChart>;
    //public budget: BudgetMandayData;
    public budget: MandayYearChart;
}

export class BudgetMandayData {
    public mandayCount: number;
    public color: string;
}

export class MandayYearChart {
    year: number;
    mandayCount: number;
    color: string;
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

export class ManagementDashboardFilterMaster {
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

export class CustomerDataSourceRequest {
    searchText: string;
    serviceId: number;
    skip: number;
    take: number;

    constructor() {
        this.serviceId = APIService.Inspection;
        this.skip = 0;
        this.take = 10;
        this.searchText = "";
    }
}

export class AverageBookingTimeItem {
    public requestToVerified: number;
    public verifiedToConfirmed: number;
    public confirmedToScheduled: number;
    public dateRevisions: number;
}

export class AverageQuotationTimeItem {
    public requestToVerified: number;
    public verifiedToSentToClient: number;
    public sentToClientToValidated: number;
    public totalDays: number;
}

export const MandayChartYear = [
    { name: "MTD", id: 1 },
    { name: "YTD", id: 2 }
];

