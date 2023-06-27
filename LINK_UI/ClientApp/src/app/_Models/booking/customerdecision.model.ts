import { NgxGalleryImage, NgxGalleryOptions } from "ngx-gallery-9";
import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource, SupplierDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class CustomerDecisionRequestModel extends summaryModel {

    public searchtypeid: number;
    public searchtypetext: string = "";
    public customerid: number;
    public supplierid: number;
    public factoryidlst: number[] = [];
    public datetypeid: number;
    public fromdate: any;
    public todate: any;
    public officeidlst: any[] = [];
    public serviceTypelst: any[] = [];
    public customerBookingNo: string;
    public advancedSearchtypeid: number;
    public advancedsearchtypetext: string = null
    public selectedCountryIdList: number[] = [];
    public selectedBrandIdList: number[] = [];
    public selectedDeptIdList: number[] = [];
    public selectedCollectionIdList: number[] = [];
    public selectedBuyerIdList: number[] = [];
    public statusidlst: any[] = [];
    public cusDecisionGiven: number;
    public fbReportResultList: number[] = [];
    public showFbResult: boolean;
    public bookingIds: number[] = [];
}

export class CustomerDecisionMasterData {

    requestCustomerModel: CommonDataSourceRequest;
    customerInput: BehaviorSubject<string>;
    customerLoading: boolean;
    public customerList: any;

    supsearchRequest: CommonDataSourceRequest;
    supInput: BehaviorSubject<string>;
    supLoading: boolean;
    public supplierList: any;

    facsearchRequest: CommonDataSourceRequest;
    facInput: BehaviorSubject<string>;
    facLoading: boolean;
    public factoryList: any;

    countryRequest: CountryDataSourceRequest;
    countryList: any;
    countryLoading: boolean
    countryInput: BehaviorSubject<string>;

    brandSearchRequest: CommonCustomerSourceRequest;
    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptSearchRequest: CommonCustomerSourceRequest;
    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerSearchRequest: CommonCustomerSourceRequest;
    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    collectionSearchRequest: CommonCustomerSourceRequest;
    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    collectionList: any;

    officeLoading: boolean;
    officeList: any;

    serviceTypeList: any;
    serviceTypeLoading: any;

    fbReportResultList: any;
    fbReportResultLoading: boolean;
    searchloading: boolean = false;

    exportLoading: boolean;

    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");

        this.customerInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.facInput = new BehaviorSubject<string>("");

        this.requestCustomerModel = new CommonDataSourceRequest();
        this.supsearchRequest = new CommonDataSourceRequest();

        this.facsearchRequest = new CommonDataSourceRequest();

        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();

        this.countryRequest = new CountryDataSourceRequest();
    }

}

export class EditCustomerDecisionModel {
    public bookingData: any = {};
    public bookingProductList: Array<BookingProductList>;
    public reportData: ReportData;
    public reportProducts: Array<ReportProducts>;
    public reportContainers: any[] = [];
    public inspectionCustomerDecisionList: any[] = [];
    public inspectionReportSummaryList: any[];
    public productGalleryOptions: NgxGalleryOptions[];
    public productGalleryImages: NgxGalleryImage[] = [];
    public inspectionDefectList: any[] = [];
    public additionalproductGalleryImages: NgxGalleryImage[] = [];
    public additionalproductGalleryOptions: NgxGalleryOptions[];
    public inspectionProductDefectList: any[] = [];

    public problematicSummaryList: any[];

    public isCriticalDefectavailable: boolean = false;
    public isMajorDefectavailable: boolean = false;
    public isMinorDefectavailable: boolean = false;

    public totalCriticalCount: number;
    public totalMajorCount: number;
    public totalMinorCount: number;

    public isCriticalProductDefectavailable: boolean = false;
    public isMajorProductDefectavailable: boolean = false;
    public isMinorProductDefectavailable: boolean = false;

    public saveLoading: boolean = false;
    public loading: boolean = true;
    public dataLoading: boolean = true;

    constructor() {
        this.reportData = new ReportData();
        this.reportProducts = new Array<ReportProducts>();
        this.bookingProductList = new Array<BookingProductList>();
    }
}

export class ReportData {
    reportTitle: string;
    reportPath: string;
    reportResult: string;
    reportPhoto: string;
    reportStatus: string;
    customerDecisionStatus: string
    customerDecisionComments: string;
    startDate: string;
    toDate: string;
    inspectionDate: string;
    reportId: string;
    reportResultId: number;
    customerResultId: number;
    additionalPhotos: Array<string>;
}

export class ReportProducts {

    ProductId: number;
    InspectionPoId: number;
    productName: string;
    productDescription: string;
    bookingQuantity: string;
    inspectedQuantity: string;
    presentedQuantity: string;
    minor: number;
    major: number;
    critical: number;
    destinationCountry: number;
    combineProductId: string;
    combineProductCount: number;
    combineAql: number;
    reportId: number;
    prodDescTrim: string;
    isDescTooltipShow: boolean;
    poNumber: Array<string>;
}

export class BookingProductList {
    bookingId: number;
    reportId: number;
    resportResultId: number;
    customerDecisionResultId: number;
    productId: string;
    reportResultName: string;
    customerDecisionName: string;
    isCheckboxSelected: boolean;
    productIdList: Array<string>;
    customerDecisionComment: string;
    customerDecisionResultCusDecId: number;
    reportTitle: string;
    productPhoto: string;
}

export class CustomerDecisionListSaveRequest {
    public reportIdList: Array<number>;
    public customerResultId: any;
    public comments: string;
    public sendEmailToFactoryContacts: boolean;
    public bookingId: number;
    public isAutoCustomerDecision: boolean;
}

export class ResultType {
    id: any;
    name: string;
}
