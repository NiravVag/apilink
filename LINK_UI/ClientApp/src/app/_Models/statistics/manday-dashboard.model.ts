import { Observable, Subject } from "rxjs";
import { DataSource } from "../common/common.model";
import { Data } from "@angular/router";
import { array } from "@amcharts/amcharts4/core";
import { MandayCompanyByList, MandayDropDown } from "src/app/components/common/static-data-common";

export class ManDayDashboard {
    mandayTermId: number;
    compareBy: number;
    factoryCountryList: Observable<DataSource[]>;
    serviceList: Array<DataSource>;
    officeList: Array<DataSource>;
    mandayTermList: Array<DataSource>;
    loading: boolean;
    serviceLoading: boolean;
    officeLoading: boolean;
    mandayTermLoading: boolean;
    countryLoading: boolean;
    countryInput: Subject<string>;
    isDateShow: boolean;
    isAuditShow: boolean;
    isMandayTermShow: boolean;
    countLoading: boolean;
    mandayYearLoading: boolean;
    mandayCustomerLoading: boolean;
    mandayCountryLoading: boolean;
    mandayEmployeeTypeLoading: boolean;

    mandayYearChartLoading: boolean;
    mandayCustomerChartLoading: boolean;
    mandayCountryChartLoading: boolean;
    mandayEmployeeTypeChartLoading: boolean;


    mandayByYearExportLoading: boolean;
    mandayByCustomerExportLoading: boolean;
    mandayByCountryExportLoading: boolean;
    mandayByEmployeeTypeExportLoading: boolean;

    searchLoading: boolean;

    mandayYearSubCountryList: Observable<DataSource[]>;
    mandayYearSubCountryLoading: boolean;
    mandayYearSubCountryInput: Subject<string>;

    mandayYearSubCustomerList: Observable<DataSource[]>;
    mandayYearSubCustomerLoading: boolean;
    mandayYearSubCustomerInput: Subject<string>;

    mandayCustomerSubCountryList: Observable<DataSource[]>;
    mandayCustomerSubCountryLoading: boolean;
    mandayCustomerSubCountryInput: Subject<string>;

    mandayCountrySubCountryList: Observable<DataSource[]>;
    mandayCountrySubCountryLoading: boolean;
    mandayCountrySubCountryInput: Subject<string>;

    mandayCountrySubProvinceList: DataSource[];
    mandayCountrySubProvinceLoading: boolean;
    isMandayCountrySubProvinceShow: boolean;

    mandayEmployeeTypeSubCustomerList: Observable<DataSource[]>;
    mandayEmployeeTypeSubCustomerLoading: boolean;
    mandayEmployeeTypeSubCustomerInput: Subject<string>;

    mandayEmployeeTypeSubYearList: Array<DataSource>;

    customerList: Observable<DataSource[]>;
    customerLoading: boolean;
    customerInput: Subject<string>;
    toggleFormSection: boolean;

    mandayCompanyByList = MandayCompanyByList;
    mandayTypeList = MandayDropDown;

    constructor() {
        this.countryInput = new Subject<string>();
        this.customerInput = new Subject<string>();
        this.mandayYearSubCountryInput = new Subject<string>();
        this.mandayCustomerSubCountryInput = new Subject<string>();
        this.mandayCountrySubCountryInput = new Subject<string>();
        this.mandayYearSubCustomerInput = new Subject<string>();
        this.mandayEmployeeTypeSubCustomerInput = new Subject<string>();
    }
}


export class MandayDashboardRequest {
    serviceDateFrom: any;
    serviceDateTo: any;
    countryIdList: Array<number>;
    serviceId: number;
    officeIdList: Array<number>;
    customerIdList: Array<number>;
    supplierId: number;

    mandayYearSubCountryId: number;
    mandayCustomerSubCountryId: number;

    mandayCountrySubCountryId: number;
    mandayCountrySubProvinceId: number;

    mandayEmployeeTypeSubYear: number;

    mandayYearSubCustomerId: number;
    mandayEmployeeTypeSubCustomerId: number;
    statusIdList: Array<number>;

    comparedServiceDateFrom: any;
    comparedServiceDateTo: any;

    mandayType: number;

    isCompareData: boolean;
}

export class MandayDashboardResponse {
    data: MandayDashboardItem;
    result: MandayDashboardResult;
}

export class MandayDashboardItem {
    totalManday: number;
    totalCount: number;
    totalReportCount: number;
}

export enum MandayDashboardResult {
    success = 1,
    fail = 2,
    notFound = 3
}

export enum MandayTerm {
    Week = 1,
    Month = 2,
    Year = 3,
    Custom = 4
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
    month_Year: string;
}

export class MandayYearChartItem {
    month: number;
    monthManDay: number;
    monthActualManDay: number;
}

export class MandayYearChartResponse {
    monthYearXAxis: Array<MandayYear>;
    data: Array<MandayYearChart>;
    result: MandayDashboardResult;
}

export class MandayCountryChart {
    name: string;
    mandayCount: number;
    comparedPercentage: number;
}

export class MandayCountryChartResponse {
    data: Array<MandayCountryChart>;
    result: MandayDashboardResult;
    totalCount: number;
    constructor() {
        this.data = new Array<MandayCountryChart>();
    }
}

export class MandayCustomerChart {
    customerName: string;
    mandayCount: number;
    percentage: number;
    color: string;
    comparedPercentage: number;
}

export class MandayCustomerChartResponse {
    data: Array<MandayCustomerChart>;
    result: MandayDashboardResult;
}

export class MandayEmployeeTypeChart {
    employeeType: string;
    color: string;
    mandayCount: number;
    percentage: number;
    monthlyData: Array<EmployeeTypes>;
}

export class EmployeeTypes {
    employeeType: string;
    mandayCount: number;
    month: number;
    mandayPercentage: number;
}

export class MandayEmployeeTypeChartResponse {
    data: Array<MandayEmployeeTypeChart>;
    result: MandayDashboardResult;
    monthYearXAxis: Array<MandayYear>;
}
export class FilterModel {
    fromDate: string;
    toDate: string;
    comparedFrom: string;
    comparedTo: string;
    OtherFilter: Array<string>;
}
