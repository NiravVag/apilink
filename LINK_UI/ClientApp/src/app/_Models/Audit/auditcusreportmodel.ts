import { summaryModel } from "../summary.model";
import { CustomerItem } from "../customer/edit-customer.model";
import { SupplierItem } from "../supplier/edit-supplier.model";
import { Office } from "../office/edit-officemodel";
import { BehaviorSubject } from "rxjs";
import { CountryDataSourceRequest, DataSource } from "../common/common.model";
import { array } from "@amcharts/amcharts4/core";
export class auditcusreportrequest extends summaryModel {

    public searchtypeid: number;

    public searchtypetext: string = "";

    public customerid: number;

    public supplierid: number;

    public factoryidlst: any[] = [];

    public statusidlst: any[] = [];

    public datetypeid: number;

    public fromdate: string;

    public todate: string;

    public officeidlst: any[] = [];

    public serviceTypelst: any[] = [];

    factoryCountryIdList: any;
}
export class AuditCusReportItem {

    public auditId: number;

    public customer: string;

    public supplier: string;

    public factory: string;

    public serviceType: string;

    public serviceDate: string;

    public reportNo: string;

    public officeName: string;

    public statusId: number;

    public reportid?: number;

    public mimeType: string;

    public pathextension: string;

    public customerBookingNo: string;

    public reportUrl: string;

    public reportFileUniqueId: string;

    public reportFileName: string;
    public factoryCountry: string;
    public fbReportUrl: string;
    public fbreportid?:number;
}
export class Loadingstatus {
    cusloading: boolean = false;
    statusloading: boolean = false;
    officeloading: boolean = false;
    auditservicetypeloading: boolean = false;
    supplierloading: boolean = false;
    factloading: boolean = false;
    searchloading: boolean = false;
    downloadreport: boolean = false;
    countryLoading: boolean
}
export class DataList {
    customerlist: Array<CustomerItem> = [];
    supplierlist: Array<SupplierItem> = [];
    factorylist: Array<SupplierItem> = [];
    officelist: Array<Office> = [];
    statuslist: Array<_AuditStatus> = [];
    auditservicetypelist: Array<ServiceType> = [];
    countryList: Array<DataSource> = [];
    countryInput: BehaviorSubject<string>;
    countryRequest: CountryDataSourceRequest;

    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.countryRequest = new CountryDataSourceRequest();
    }
}
export class _AuditStatus {
    public id: number;
    public statusName: string;
}
export class ServiceType {
    public id: number;
    public name: string;
}
export enum AuditServiceTypeResponseResult {
    success = 1,
    error = 2
}
export enum AuditStatusResponseResult {
    success = 1,
    Error = 2
}
export enum AuditCusReportBookingDetailsResult {
    Success = 1,
    NotFound = 2,
    Error = 3,
    RequestError = 4
}
export class AuditStatusColor {
    public id: number
    public statusName: string
    public statusColor: string
    public totalCount: string
}
