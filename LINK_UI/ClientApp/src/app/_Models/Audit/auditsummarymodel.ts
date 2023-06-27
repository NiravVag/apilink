import { array } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { summaryModel } from "../../_Models/summary.model";
import { CustomerCommonDataSourceRequest } from "../common/common.model";
export class Auditsummarymodel extends summaryModel {

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
    mandayDashboard: any;
    rejectChart: any[];
    mandayChartType: number;
}

export class AuditItem {

    public auditId: number;

    public customerName: string;

    public supplierName: string;

    public factoryName: string;

    public serviceType: string;

    public serviceDateFrom: string;

    public serviceDateTo: string;

    public poNumber: string;

    public reportNo: string;

    public office: string;

    public statusId: number;

    public bookingCreatedBy: number;

    public quotationStatus: QuotStatus;
    public customerBookingNo: string;

}
export class QuotStatus {
    id: number;
    label: string;
}


export class AuditMasterData {
    countryList: any;
    countryLoading: boolean
    countryInput: BehaviorSubject<string>;

    requestCustomerModel: CustomerCommonDataSourceRequest;
    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    }
}
