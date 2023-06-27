import { BehaviorSubject } from "rxjs";
import { EntityAccess } from "src/app/components/common/static-data-common";
import { summaryModel } from "../../_Models/summary.model";
export class ScheduleModel extends summaryModel {

    public searchtypeid: number;
    public searchtypetext: string = "";
    public customerid: number;
    public supplierid: number;
    public factoryidlst: any[] = [];
    public statusidlst: any[];
    public countryId: number;
    public provinceId: number;
    public cityId: number;
    public datetypeid: number;
    public fromdate: any;
    public todate: any;
    public officeidlst: any[] = [];
    public quotationsStatusIdlst: any[] = [];
    public qcIdlst: any[] = [];
    public zoneIdlst: any[] = [];
    public exportType: number;
}

export class BookingItemSchedule {

    public bookingId: number;
    public customerId: number;
    public customerName: string;
    public poNumber: string;
    public supplierName: string;
    public factoryName: string;
    public serviceType: string;
    public serviceDateFrom: string;
    public serviceDateTo: string;
    public internalReferencePo: string;
    public office: string;
    public statusName: string;
    public statusId: number;
    public bookingCreatedBy: number;
    public isPicking: false;
    public previousBookingNo: number;
    public productCategory: string;
    factoryId: number;
    countryId: number;
    manDay: number;
    serviceDateQCNames: Array<ServiceDateQCNames>;
    serviceDateCSNames: Array<ServiceDateCSNames>;
    serviceDateAdditionalQCNames: Array<ServiceDateQCNames>;
    public factoryLocation: string;
    public actualManDay: string;
    public productCount: number;
    public reportCount: number;
    public serviceDate: string;
    public isBookingSelected: boolean;
    public showCheckBox: boolean;
    public isMandayButtonVisible: boolean;
    public factoryProvinceName: string;
    public factoryCityName: string;
    public factoryZoneName: string;
    public sampleSize: number;
    public firstServiceDate: string;
    public quotationStatus: string;
    public showAddButton: boolean;
    public factoryCountyName: string;
    public factoryTownName: string;
    public plannedManday: number;
    public hasQcVisible: boolean;
    public qcVisibleToEmail: boolean;
    public isQcVisibleBookingSelected: boolean;
    public calculatedWorkingHours: number;
    public productSubCategory: string;
    public productSubCategory2: string[];
    productId: string;
    isMSChartProduct: boolean;
    public isEAQF: boolean = false;
    customerProductId: number;
}

export class StaffSchedule {
    staffID: number;
    StaffName: string;
    EmailAddress: string;
    EmergencyCall: string;
}

export class QCInfo {
    qcName: number;
    actualManDay: string;
}
export class CSInfo {
    csName: number;
    actualManDay: string;
}
export class ServiceDateQCNames {
    qcInfo: Array<QCInfo>;
    serviceDate: string;
}
export class ServiceDateCSNames {
    qcInfo: Array<CSInfo>;
    serviceDate: string;
}

export class MandayModel {
    bookingNo: number;
    travelManday: number;
    totalManday: number;
    suggestedManday: number;
    mandayList: Array<QuotationManday>;
}


export class QuotationManday {
    bookingId: number;
    serviceDate: string;
    manDay: number
    remarks: string
}

export enum QuotationMandayResult {
    success = 1,
    notFound = 2,
    other = 3
}

export class ScheduleFilterMaster {
    public qcList: any;
    qcListName: string;
    filterCount: number;
    filterDataShown: boolean;

    qcInput: BehaviorSubject<string>;

    qcLoading: boolean;

    constructor() {
        this.qcInput = new BehaviorSubject<string>("");
        this.qcLoading = false;
    }
}
export class ScheduleZoneFilterMaster {
    public zoneList: any;
    zoneListName: string;
    filterCount: number;
    filterDataShown: boolean;

    zoneInput: BehaviorSubject<string>;

    zoneLoading: boolean;

    constructor() {
        this.zoneInput = new BehaviorSubject<string>("");
        this.zoneLoading = false;
        this.zoneList = [];
    }
}
export class QcVisibilityBookingModel {
    bookingIdlst: Array<number>;
}
export class bookingDataQcVisibleRequest {
    bookingDataQcVisible: Array<BookingDataQcVisible>;

}
export class BookingDataQcVisible {
    bookingId: number;
    serviceDate: string;
    isQcVisibility: boolean;
}

export class ScheduleProductModel {
    productId: string;
    orderQty: number;
    poNumber: string;
    msChart: string;
}

export class ScheduleMasterData{
    entityId:number;
    entityAccess=EntityAccess;
}
