import { Observable, Subject, BehaviorSubject } from "rxjs";
import { DataSource } from "../common/common.model";
import { List } from "@amcharts/amcharts4/core";

export class ManDayUtilizationDashboard {
    serviceList: Array<DataSource>;
    officeList: Array<DataSource>;
    mandayEmployeeTypeSubYearList: Array<DataSource>;
    countryList: Array<DataSource>;

    serviceLoading: boolean;
    officeLoading: boolean;
    countryLoading: boolean;

    mandayYearLoading: boolean;
    mandayUtilisationLoading: boolean;
    mandayEmployeeTypeLoading: boolean;

    mandayEmployeeTypeChartLoading: boolean;

    mandayByYearExportLoading: boolean;
    mandayByUtilisationExportLoading: boolean;
    mandayByEmployeeTypeExportLoading: boolean;

    searchLoading: boolean;

    countryInput: BehaviorSubject<string>;

    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.countryList = [];
    }
}


export class MandayUtilizationRequest {
    serviceDateFrom: any;
    serviceDateTo: any;
    countryIdList: Array<number>;
    serviceId: number;
    officeIdList: Array<number>;

    customerIdList: Array<number>;

    mandayYearSubCountryId: number;
    mandayCustomerSubCountryId: number;

    mandayCountrySubCountryId: number;
    mandayCountrySubProvinceId: number;

    mandayEmployeeTypeSubYear: number;

    mandayYearSubCustomerId: number;
    mandayEmployeeTypeSubCustomerId: number;
    supplierId: number;
    statusIdList: Array<number>;
    comparedServiceDateFrom: any;
    comparedServiceDateTo: any;

    mandayType: number;
    isCompareData:boolean;
}

export class UtilizationDashboard {
    office: string
    hourMandDays: number;
    workDays: number;
    leaves: number;
    maxPotential: number;
    outsourceMandays: number;
    outsousrceMandaysPercentage: number;
    utilizationRateLastYear: number;
    utilizationRateCurrentYear: number;
    utilizationPercentage: number;
}

export class UtilizationGraphData {
    totalUtilization: number;
    gradingData: Array<GradingData>;
}

export class GradingData {
    title: string;
    color: string;
    lowScore: number;
    highScore: number;
}
