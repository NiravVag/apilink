import { BehaviorSubject } from "rxjs";
import { EntityAccess } from "src/app/components/common/static-data-common";
import { summaryModel } from "../../_Models/summary.model";
import { CommonDataSourceRequest, CustomerCommonDataSourceRequest, SupplierDataSourceRequest } from "../common/common.model";
import { DataSource, QuotationFromBooking } from '../quotation/quotation.model';
export class InspectionBookingsummarymodel extends summaryModel {

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
    public isBookingRequestRole: boolean;
    public isBookingConfirmRole: boolean;
    public isBookingVerifyRole: boolean;
    public isQuotationRequestRole: boolean;
    public isQuotationSearch: boolean;
    public Quotdetails: QuotationFromBooking;
    public serviceTypelst: any[] = [];
    public customerBookingNo: string;
    public advancedSearchtypeid: number;
    public advancedsearchtypetext: string = null;
    public quotationsStatusIdlst: any[] = [];
    public userIdList: any[] = [];
    public bookingType: number;
    callingFrom: number;
    reportDate: any;
    selectedCountryIdList: number[] = [];
    selectedProvinceIdList: number[] = [];
    selectedCityIdList: number[] = [];
    selectedBrandIdList: number[] = [];
    selectedDeptIdList: number[] = [];
    selectedCollectionIdList: number[] = [];
    selectedBuyerIdList: number[] = [];
    selectedPriceCategoryIdList: number[] = [];
    barcode: string;
    isEcoPack: boolean;
    isPicking: boolean;
    supplierTypeId: number;
    isEAQF: boolean;
    inspectionDFTransactions: any[] = [];
}

export class BookingItem {

    public bookingId: number;
    public customerId: number;
    public customerName: string;
    public poNumber: string;
    public supplierName: string;
    public factoryName: string;
    public serviceType: string;
    public serviceTypeId: string;
    public serviceDateFrom: string;
    public serviceDateTo: string;
    public firstServiceDateFrom: string;
    public firstServiceDateTo: string;
    public internalReferencePo: string;
    public office: string;
    public officeId: number;
    public statusId: number;
    public bookingCreatedBy: number;
    public isPicking: false;
    public isEAQF: false;
    public previousBookingNo: number;
    public quotationStatus: string;
    public applyDate: Date;
    public bookingType: number;
    factoryId: number;
    cancelBtnShow: boolean;
    rescheduleBtnShow: boolean;
    editBtnText: string;
    countryId: number;
    isQuotSelected: boolean;
    productCategory: string;
    supplierId: number;
    csName: string;
    isSplitBookingButtonVisible: boolean;
    isPickingButtonVisible: boolean;
    public productList: any[] = [];
    public statusList: any[] = [];
    public reportSummaryLink: string;
    customerBookingNo: string;
    deptNames: string;
    editImagePath: string;
    createdByName: string;
    isCombineVisible: boolean;
    productCount: number;
    isRowSelected: boolean;
    customerNameTrim: string;
    supplierNameTrim: string;
    factoryNameTrim: string;
    statusName: string;
    isCustomerTooltipShow: boolean;
    isSupplierTooltipShow: boolean;
    isFactoryTooltipShow: boolean;
    isAETooltipShow: boolean;
    isOfficeTooltipShow: boolean;
    isCreatedByShow: boolean;
    isServiceTypeShow: boolean;
    serviceTypeTrim: string;
    CreatedByTrim: string;
    OfficeTrim: string;
    AETrim: string;
    bookingCreatedFirstName: string;
    isAllProductSelected: boolean = false;
    productRefId: string;

}
export class HolidayRequest {
    serviceDateFrom: any;
    factoryCountryId: number;
    factoryId: number;
}

export class BookingReportItem {

    public bookingId: number;
    public customerId: number;
    public customerName: string;
    public poNumber: string;
    public supplierName: string;
    public factoryName: string;
    public serviceType: string;
    public serviceTypeId: number;
    public serviceDateFrom: string;
    public serviceDateTo: string;
    public internalReferencePo: string;
    public office: string;
    public statusId: number;
    public bookingCreatedBy: number;
    public isPicking: false;
    public previousBookingNo: number;
    public productList: any[];
    public factoryId: number;
    public cancelBtnShow: boolean;
    public rescheduleBtnShow: boolean;
    public editBtnText: string;
    public countryId: number;
    public isExpand: boolean = false;
    public isCsReport: boolean = false;
    public fbMissionId: number;
    public overAllStatus: string;
    public csList: any[];
    public customerBookingNo: string;
    public isEAQF: boolean = false;
}

export class BookingInfo {
    customerBookingNo: number;
}

export class BookingSummaryMasterData {

    requestCustomerModel: CustomerCommonDataSourceRequest;
    customerInput: BehaviorSubject<string>;
    customerLoading: boolean;
    public customerList: any;

    supsearchRequest: CommonDataSourceRequest;
    supInput: BehaviorSubject<string>;
    supLoading: boolean;
    public supplierList: any;

    facsearchRequest: SupplierDataSourceRequest;
    facInput: BehaviorSubject<string>;
    facLoading: boolean;
    public factoryList: any;
    public inspectionBookingTypes = [];
    inspectionBookingTypeVisible: boolean;


    countryList: any;
    countryLoading: boolean
    countryInput: BehaviorSubject<string>;

    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    collectionList: any;

    priceCategoryLoading: boolean;
    priceCategoryInput: BehaviorSubject<string>;
    priceCategoryList: any;

    officeLoading: boolean;
    officeList: any;

    statusLoading: boolean;
    bookingStatusList: Array<DataSource>;
    quotationStatusList: any;

    aeUserLoading: boolean;
    aeUserList: Array<DataSource>;

    serviceTypeList: any;
    serviceTypeLoading: any;

    selectedDate: string;
    selectedNumber: string;
    selectedNumberPlaceHolder: string;

    isShowColumn: boolean;
    isShowColumnImagePath: string;
    showColumnTooltip: string;

    isShowSearchLens: boolean;
    statusList: any;

    productList: any;
    poList: any;
    containerProductList: any;
    containerPOList: any;
    containerItem: any;
    bookingItem: any

    bookingNumber: string;
    productId: string;
    productName: string;
    isproductOrPODetails: boolean;
    selectedAllProducts: boolean;
    pageLoader: boolean;
    setIconWidth: string;
    supplierName: string;
    customerName: string;
    officeNameList: Array<string>;
    factoryNameList: Array<string>;
    countryNameList: Array<string>;
    serviceTypeNameList: Array<string>;
    brandNameList: Array<string>;
    buyerNameList: Array<string>;
    deptNameList: Array<string>;
    statusNameList: Array<string>;
    DateName: string;

    provinceLoading: boolean
    cityLoading: boolean;

    provinceList: any;
    cityList: any;

    entityId:number;
    entityAccess=EntityAccess;

    constructor() {
        this.pageLoader = false;
        this.isShowColumn = true;
        this.selectedAllProducts = false;
        this.countryInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");
        this.priceCategoryInput = new BehaviorSubject<string>("");

        this.customerInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.facInput = new BehaviorSubject<string>("");

        this.requestCustomerModel = new CustomerCommonDataSourceRequest();
        this.supsearchRequest = new CommonDataSourceRequest();

        this.facsearchRequest = new SupplierDataSourceRequest();

        this.bookingStatusList = new Array<DataSource>();
        this.quotationStatusList = [];
        this.aeUserList = new Array<DataSource>();

        this.provinceLoading = false;
        this.cityLoading = false;
        this.provinceList = [];
        this.cityList = [];
    }
}

export enum BookingSummaryStatusResponseResult {
    Success = 1,
    StatusListNotFound = 2,
    QuotationStatusListNotFound = 3
}
