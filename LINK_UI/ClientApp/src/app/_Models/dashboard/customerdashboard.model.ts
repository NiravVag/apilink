export class CustomerDashboard {
    public businessOVDashboard: CustomerBusinessOVDashboard;
    public apiraDashboard: Array<CustomerAPIRADashboard>;
    public productCategoryDashboard: Array<ProductCategoryDashboard>;
    public customerResultDashboard: Array<CustomerResultDashboard>;
    public inspectionRejectDashboard: Array<InspectionRejectDashboard>;
    public supplierPerformanceDashboard: SupplierPerformanceDashboard;
    public inspectionManDayOverview: InspectionManDayOverview;
    public quotationTaskData: QuotationTaskData;
    public customerCsContact: CustomerCsContact;
}

export class CustomerBusinessOVDashboard {
    public bookingCount: number;
    public productsCount: number;
    public factoryCount: number;
    public manDays: number;
    public productPercentage: number;
    public factoryPercentage: number;
    public manDayPercentage: number;
}

export class CustomerAPIRADashboard {
    public statusName: string;
    public statusColor: string;
    public totalCount: number;
    public name: string;
}

export class ProductCategoryDashboard {
    public statusName: string;
    public statusColor: string;
    public totalCount: number;
    public imagePath: string;
    productName: string;
}

export class CustomerResultDashboard {
    public statusName: string;
    public statusColor: string;
    public totalCount: number;
    public name: string;
}

export class InspectionRejectDashboard {
    public statusName: string;
    public statusColor: string;
    public totalCount: number;
}

export class SupplierPerformanceDashboard {
    public bookingLeadTime: number;
    public etdLeadTime: number;
    public bookingRevisons: number;
}

export class CustomerInfo {
    public id: number;
    public name: string;
}

export class InspectionManDayOverview {
    public totalInspections: number;
    public totalManDays: number;
    public totalReports: number;
    public lastDaysPercentage: number;
    public averagePercentage: number;
    public totalManDaysPercentage: number;
    public averagePercentagePositive: number;
    public totalManDaysPercentagePositive: number;
}

export class CustomerCsContact {
    public id: number;
    public staffId: number;
    public fullName: string;
    public emailAddress: string;
    public mobileNo: number;
}



export class CustomerDashBoardGraphData {
    public graphWidth: number;
    public isCollapsed: boolean;
    public barChartWidth: number;
    public overlayHeight: number;
    public mapCardHeight: number;
    public barChartHeight: number;
    public donutChartSize: number;
    public donutChartthickness: number;
    public togglePhoneContainer: boolean;
    public GuarageChartSize: number;
}

export class CustomerFilterPopupData {
    public filterState: boolean;
}

export class QuotationTaskData {
    public pendingQuotations: number;
    public completedQuotations: number;
}

export class CustomerDashBoardLoader
{
    public taskLoading:boolean;
    public manDaysLoading:boolean;
    public businessOverviewLoading:boolean;
    public inspectedBookingLoading:boolean;
    public supplierPerformanceLoading:boolean;
    public productOverviewLoading:boolean;
    public apiresultLoading:boolean;
    public customerResultLoading:boolean;
    public inspectionRejectLoading:boolean;

    supplierPerformanceError: boolean;
    inspectedBookingError: boolean;
    productOverviewError: boolean;
    inspectionRejectError:boolean;
    manDaysError:boolean;
    customerDecisionLoading: boolean;
}

export class CustomerDashBoardDataFound
{
    public taskDataFound:boolean;
    public manDaysFound:boolean;
    public businessOverviewFound:boolean;
    public inspectedBookingFound:boolean;
    public supplierPerformanceFound:boolean;
    public productOverviewFound:boolean;
    public apiresultFound:boolean;
    public customerResultFound:boolean;
    public inspectionRejectFound:boolean;
}

export enum ManDayResult {
    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

export enum CustomerTaskEnum {
    PendingQuotationTask = 1,
    CompletedQuotationTask = 2
}
export enum MapGeoLocationResult {
    Success = 1,
    Failure = 2
}
