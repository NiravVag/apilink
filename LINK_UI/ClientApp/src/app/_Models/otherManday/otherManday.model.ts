import { BehaviorSubject } from "rxjs";
import { PageSizeCommon } from "src/app/components/common/static-data-common";
import { BooleanLiteral } from "typescript";
import { CommonDataSourceRequest, CountryDataSourceRequest, QcDataSourceRequest } from "../common/common.model";
import { ScheduleFilterMaster } from "../Schedule/schedulemodel";
import { summaryModel } from "../summary.model";

export class OtherMandayModel extends summaryModel {
    public customerId: number;
    public serviceFromDate: any;
    public serviceToDate: any;
    public operationalCountryId: number;
    public officeCountryId: number;
    public qcId: number;
}

export class OtherMandayEditModel {
    public id?: number;
    public customerId: number;
    public serviceDate: any;
    public operationalCountryId: number;
    public officeCountryId: number;
    public purposeId: number;
    public qcId: number;
    public manday: number;
    public remarks: string;

    constructor(){
        this.id = 0;
    }
}

export class OtherMandaySummaryModel{
    customerList: any;
    customerLoading: boolean
    customerInput: BehaviorSubject<string>;
    customerRequest: CommonDataSourceRequest;

    operationalCountryList: any;
    operationalCountryLoading: boolean
    operationalCountryInput: BehaviorSubject<string>;
    operationalCountryRequest: CountryDataSourceRequest;

    officeCountryList: any;
    officeCountryLoading: boolean
    officeCountryInput: BehaviorSubject<string>;
    officeCountryRequest: CountryDataSourceRequest;

    qcList: any;
    qcLoading: boolean
    qcInput: BehaviorSubject<string>;
    qcRequest: QcDataSourceRequest;  

    searchloading: boolean;
    exportLoading: boolean;
    selectedPageSize: any;

    constructor(){
    this.customerRequest = new CommonDataSourceRequest();    
    this.customerInput = new BehaviorSubject<string>("");
    this.exportLoading = false;
    this.selectedPageSize = PageSizeCommon[0];
    
    this.operationalCountryRequest = new CountryDataSourceRequest();    
    this.operationalCountryInput = new BehaviorSubject<string>("");
    this.officeCountryRequest = new CountryDataSourceRequest();    
    this.officeCountryInput = new BehaviorSubject<string>("");
    this.qcRequest = new QcDataSourceRequest();
    this.qcInput = new BehaviorSubject<string>("");
    }
}

export class OtherMandayEditSummaryModel{
    customerList: any;
    customerLoading: boolean
    customerInput: BehaviorSubject<string>;
    customerRequest: CommonDataSourceRequest;

    operationalCountryList: any;
    operationalCountryLoading: boolean
    operationalCountryInput: BehaviorSubject<string>;
    operationalCountryRequest: CountryDataSourceRequest;

    officeCountryList: any;
    officeCountryLoading: boolean
    officeCountryInput: BehaviorSubject<string>;
    officeCountryRequest: CountryDataSourceRequest;

    qcList: any;
    qcLoading: boolean
    qcInput: BehaviorSubject<string>;
    qcRequest: QcDataSourceRequest;    
    
    purposeList: any;
    purposeLoading: boolean;

    saveloading: boolean;
    deleteLoading: boolean;

    constructor(){
    this.customerRequest = new CommonDataSourceRequest();    
    this.customerInput = new BehaviorSubject<string>("");
    
    this.operationalCountryRequest = new CountryDataSourceRequest();    
    this.operationalCountryInput = new BehaviorSubject<string>("");
    this.officeCountryRequest = new CountryDataSourceRequest();    
    this.officeCountryInput = new BehaviorSubject<string>("");
    this.qcRequest = new QcDataSourceRequest();
    this.qcInput = new BehaviorSubject<string>("");
    }
}
export enum OtherMandayResult{
    Success = 1,
    Fail = 2,
    NoDataFound = 3,
    AlreadyExists = 4,
    RequestNotCorrectFormat = 5
}