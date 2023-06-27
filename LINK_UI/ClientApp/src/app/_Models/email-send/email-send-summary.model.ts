import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, CountryDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class EmailSendSummaryModel{   

    customerList: any;
    customerInput: BehaviorSubject<string>;
    customerLoading: boolean;
    customerRequest: CommonDataSourceRequest;    

    countryList: any;
    countryInput: BehaviorSubject<string>;
    countryLoading: boolean;
    countryRequest: CountryDataSourceRequest;

    supInput: BehaviorSubject<string>;
    supLoading: boolean;
    supplierList: any;
    supRequest: CommonDataSourceRequest;

    factoryInput: BehaviorSubject<string>;
    factoryLoading: boolean;
    factoryList: any;
    factoryRequest: CommonDataSourceRequest;

    officeList: any;
    officeLoading:boolean;

    statusList: any;
    statusLoading: boolean;

    serviceTypeList: any;
    serviceTypeLoading: boolean;

    brandList: any;
    brandLoading: boolean;

    departmentList: any;
    departmentLoading: boolean;

    collectionList: any;
    collectionLoading: boolean;

    buyerList: any;
    buyerLoading: boolean;   
    
    aeList: any;
    aeLoading: boolean;

    searchLoading: boolean;
    sendEmailLoading: boolean;

    serviceList: any;
    serviceLoading: boolean;

    exportLoading: boolean;

    apiResultList: any;
    apiResultLoading: boolean;

    constructor(){
        this.customerInput = new BehaviorSubject<string>("");
        this.customerRequest = new CommonDataSourceRequest();

        this.countryInput = new BehaviorSubject<string>("");
        this.countryRequest = new CountryDataSourceRequest();

        this.supInput = new BehaviorSubject<string>("");
        this.supRequest = new CommonDataSourceRequest();

        this.factoryInput = new BehaviorSubject<string>("");
        this.factoryRequest = new CommonDataSourceRequest();

        this.apiResultLoading=false;
    }
}

export class EmailSendRequestModel  extends summaryModel{
    public searchTypeId: number;

    public searchtypetext: string = "";

    public customerId: number;

    public supplierId: number;

    public factoryidlst: any[] = [];

    public statusidlst: any[] = [];

    public datetypeid: number;

    public fromdate: any;

    public todate: any;

    public officeidlst: any[] = [];
    public serviceTypelst: any[] = [];
    public userIdList: any[] = [];
    selectedCountryIdList: number[] = [];
    selectedBrandIdList: number[] = [];
    selectedDeptIdList: number[] = [];
    selectedCollectionIdList: number[] = [];
    selectedBuyerIdList: number[] = [];

    public serviceId: number;
    bookingIdList: Array<number> = [];

    selectedAPIResultIdList: number[] = [];
}

export class EmailSendSummaryItem {
    bookingId: number;
    bookingCustomerNumer: string = "";
    customerName: string = "";
    supplierName: string = "";
    factoryName: string = "";
    serviceTypeName: string = "";
    officeName: string = "";
    serviceDate: string = "";
    showCheckBox: boolean;
    reportCount: number;
    pendingCount:number;
    successReportCount:number;
    isBookingSelected: boolean;
    customerId: number;
    statusId: number;
}

export class Parameter{
    bookingIdList: Array<number>;
    serviceId: number;

    constructor(){
        this.bookingIdList = new Array<number>();
    }
}
