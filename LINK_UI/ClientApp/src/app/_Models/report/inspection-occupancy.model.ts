import { BehaviorSubject } from "rxjs";
import { RightsEnum } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CountryDataSourceRequest, StaffDataSourceRequest, UserDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class InspectionOccupancyModel extends summaryModel {
    officeCountryId: number;
    officeId: number;
    qA: number;
    employeeType: number;
    outSourceCompany: number;
    fromDate: any;
    toDate: any;
    utilizationRate: boolean;
    inspectionOccupancyCategories: number[];
    constructor() {
        super();
        this.inspectionOccupancyCategories = [];
    }
}

export class InspectionOccupancyMasterModel {
    officeList: any[];
    officeLoading: boolean;

    officeCountryList: any[];
    officeCountryLoading: boolean;
    officeCountryRequest: CountryDataSourceRequest;

    employeeTypeList: any[];
    employeeTypeLoading: boolean;

    outSourceCompanyList: any[];
    outSourceCompanyLoading: boolean;


    staffList: Array<any>;
    staffLoading: boolean;
    staffInput: BehaviorSubject<string>;
    requestStaffDataSource: StaffDataSourceRequest;

    exportLoading: boolean;

    statusList: any[];
    utilizationRate: boolean;
    constructor() {
        this.officeList = [];
        this.officeCountryList = [];
        this.officeCountryRequest = new CountryDataSourceRequest();
        this.employeeTypeList = [];
        this.outSourceCompanyList = [];

        this.staffList = [];
        this.requestStaffDataSource = new StaffDataSourceRequest();
        this.staffInput = new BehaviorSubject<string>("");

        this.statusList = [];
    }
}