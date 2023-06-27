import { ListSize } from './../../components/common/static-data-common';
import { StaffSchedule } from "./schedulemodel";
import { StaffeditRoutingModule } from "src/app/components/hr/edit-staff/staffedit-routing.module";
import { NumberFormatStyle } from "@angular/common";
import { BehaviorSubject } from "rxjs";
import { CommonOfficeZoneSourceRequest, CommonZoneSourceRequest } from "../common/common.model";
import { EmployeeType } from "src/app/components/common/static-data-common";

export class scheduleAllocationModel {
    public bookingNo: number;
    public serviceDateFrom: Date;
    public serviceDateTo: Date;
    public customerName: string;
    public supplierName: string;
    public quotationManDay: number;
    public bookingStatus: number;
    public factoryName: StaffeditRoutingModule;
    public bookingComments: string;
    public comment: string;
    public serviceType: string;
    public countryName: string;
    public countryId: number;
    public provinceName: string;
    public cityName: string;
    public countyName: string;
    public townName: string;
    public townId: number;
    public factoryAddress: string;
    public regionalAddress: string;
    public totalProduct: number;
    public totalReports: number;
    public totalContainers: number;
    public totalSampleSize: number;
    public totalCombineCount: number;
    public staffAllocation: Array<allocationStaff>;
    public customerId: number;
    public travelManday: number;
    public previousBookingNo: Array<number>;
    public actualManday: number;
    productCategory: string;
    productSubCategory: string;
    productSubCategory2: string;
    isEntityLevelAutoQcExpenseEnabled: boolean;
    isServiceTypeLevelAutoQcExpenseEnabled: boolean;
    isBookingInvoiced:boolean;
    suggestedManday: number;
}

export class QcAutoExpense {
    public inspectionId: number;
    public countryId: number;
    public countryName: string;
    public qcId: number;
    public qcName: string;
    public startPortId: number;
    public startPortName: string;
    public factoryTownId: number;
    public factoryTownName: string;
    public tripTypeId: number;
    public tripTypeName: string;
    public comments: string;
    public expenseStatus: number;
}

export class QcAutoExpenseMaster {
    public starPortData: any;
    public townData: any;
    public tripTypeData: any;
}

export class allocationStaff {
    public serviceDate: Date;
    public QCList: Array<StaffSchedule>;
    public additionalQCList: Array<StaffSchedule>;
    public CSList: Array<StaffSchedule>;
    public actualManDay: number;
    public QC: Array<number>;
    public additionalQC: Array<number>;
    public CS: Array<number>;
    public availableManDay: number;
    public manDay: number;
    public remarks: string;
    public isLeader: boolean;
    public leaderId: number;
    public isQcVisibility: boolean;
    public qcAutoExpenses: Array<QcAutoExpense>
}

export class SaveScheduleModel {
    public bookingId: number;
    public comment: string;
    public allocationCSQCStaff: Array<saveAllocationStaff>;
}

export class saveAllocationStaff {
    public serviceDate: Date;
    public actualManDay: number;
    public comment: string;
    public QC: Array<number>;
    public additionalQC: Array<number>;
    public CS: Array<number>;
    public isLeader: number = 0;
    public isQcVisibility: boolean;
    public qcAutoExpenses: Array<QcAutoExpense>
}

export enum SaveScheduleResponseResult {
    Success = 1,
    SaveUnsuccessful = 2,
    SaveFBDataFailure = 3,
    BookingProcessAlready = 4,
    ReportProcessedAlready = 5
}

export class ScheduleAllocationMasterModel {

    entityList: Array<any>;

    employeeTypeList: Array<any>;
    employeeTypeLoading: boolean;


    staffSearchDataSource: StaffSearchDataSource;
    staffInput: BehaviorSubject<string>;
    staffLoading: boolean;
    zoneInput: BehaviorSubject<string>;
    zoneLoading: boolean;
    zoneList: Array<any>;
    zoneRequest: CommonOfficeZoneSourceRequest;

    outSourceCompanyLoading: boolean;
    outSourceCompanyList: Array<any>;
    _EmployeeType = EmployeeType;

    startPortLoading: boolean;
    startPortList: Array<any>;

    marketSegmentLoading: boolean;
    marketSegmentList: Array<any>;

    productCategoryLoading: boolean;
    productCategoryList: Array<any>;

    expertiseLoading: boolean;
    expertiseList: Array<any>;

    officeLoading: boolean;
    officeList: Array<any>;

    employeeListLength: number;

    isShowQCDropdown = false;
    isShowAddQCDropdown = false;
    isShowReportCheckerDropdown = false;

    qcSearchLoading: boolean;

    staffAllocationType = StaffAllocationType;
    constructor() {
        this.entityList = [];
        this.employeeTypeList = [];
        this.staffSearchDataSource = new StaffSearchDataSource();
        this.staffInput = new BehaviorSubject<string>("");
        this.zoneList = [];
        this.zoneInput = new BehaviorSubject<string>("");
        this.zoneRequest = new CommonOfficeZoneSourceRequest();

        this.outSourceCompanyList = [];
        this.startPortList = [];

        this.marketSegmentList = [];
        this.productCategoryList = [];
        this.expertiseList = [];

        this.officeList = [];

    }
}


export class StaffSearchDataSource {
    serviceDate: string;
    bookingId: number;
    type: string;
    officeId: number;
    entityId: number;
    employeeType: number;
    zoneId: number;
    outSourceCompany: number;
    startPortId: number;
    marketSegmentId: number;
    productCategoryId: number;
    expertiseId: number;
    searchText: string;
    skip: number;
    take = ListSize

}

export enum StaffAllocationType {
    QC = 1,
    AdditionalQC = 2,
    CS = 3
}